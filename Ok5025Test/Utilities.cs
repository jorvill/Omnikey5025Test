using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ok5025Test
{
    class Utilities
    {
        public const int SUCCESS = 0x9000;
        public const int DATOS_INCOMPLETOS = 0x4001;
        public const int FORMATO_INCORRECTO = 0x4002;

        #region ConvertByteArrayToStringHex
        /// <summary>
        /// Convierte un arreglo de bytes a una cadena de texto con formato hexadecimal
        /// </summary>
        /// <param name="bytearray">Arreglo de bytes a convertir.</param>
        /// <param name="length">Longitud del Arreglo de Bytes</param>
        /// <returns>Regresa cadena de Texto</returns>
        public static string ConvertByteArrayToStringHex(byte[] bytearray, int length)
        {
            StringBuilder strB = new StringBuilder();
            for (int i = 0; i < length; i++)
            {

                strB.Append(bytearray[i].ToString("X2"));
            }
            return strB.ToString();
        }

        #endregion

        #region ConvertStringHexToByteArray
        /// <summary>
        /// Convierte una cadena de texto representando un número hexadecimal a un arreglo de bytes.
        /// </summary>
        /// <param name="str"> Cadena de texto que representa el número en hexadecimal</param>
        /// <param name="byteArray">Arreglo de bytes</param>
        /// <param name="byteArrayLength">Longitud del arreglo de bytes</param>
        /// <returns>Regresa iClassStatusCodes. SUCCESS en caso de Exito.</returns>
        static public int ConvertStringHexToByteArray(string str, ref byte[] byteArray, int byteArrayLength)
        {
            int resp = SUCCESS;
            int i, k = 0;

            String strBuffData = str;

            //Borra los espacios entre bytes
            for (i = 0; i < strBuffData.Length; i++)
            {
                if (strBuffData.Substring(i, 1).CompareTo(" ") == 0)
                    strBuffData = strBuffData.Remove(i, 1);

            }

            if (strBuffData.Length == 2 * byteArrayLength)
            {
                //Copia la cadena a un arreglo de enteros
                try
                {
                    k = 0;
                    for (i = 0; i < byteArrayLength; i++)
                    {
                        byteArray[i] = Convert.ToByte(strBuffData.Substring(k, 2), 16);
                        k = k + 2;
                    }

                    resp = SUCCESS;

                }
                catch (IndexOutOfRangeException siobe)
                {
                    System.Console.WriteLine("Exception..." + siobe.Message);
                    //MessageBox.Show("Datos Incompletos. Deben ser " + Convert.ToString(byteArrayLength * 2) + " caracteres en formato hexadecimal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resp = DATOS_INCOMPLETOS;
                }
                catch (ArgumentOutOfRangeException are)
                {
                    System.Console.WriteLine("Exception..." + are.Message);
                    //MessageBox.Show("Datos Incompletos. Deben ser " + Convert.ToString(byteArrayLength * 2) + " caracteres en formato hexadecimal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resp = DATOS_INCOMPLETOS;
                }
                catch (FormatException fe)
                {
                    System.Console.WriteLine("Exception..." + fe.Message);
                    //MessageBox.Show("Formato  Incorrecto. Deben ser " + Convert.ToString(byteArrayLength * 2) + " caracteres en formato hexadecimal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resp = FORMATO_INCORRECTO;
                }

            }
            else
            {
                //MessageBox.Show("Datos Incompletos. Deben ser " + Convert.ToString(byteArrayLength * 2) + " caracteres en formato hexadecimal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resp = DATOS_INCOMPLETOS;
            }

            return resp;
        }

        #endregion

    }
}
