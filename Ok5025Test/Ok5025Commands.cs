/**
 * HID GLOBAL
 * LAM Tech Support
 * DEMO CODE 
 * Author: Jorge Villicana
 * Date: 2015-04-03
 * 
 * // THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
 * **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JvlSCard;

namespace Ok5025Test
{
    class Ok5025Commands
    {


        public const int SUCCESS = 0x9000;
        public const int CARD_NOT_PRESENT = 0x2002;
        public const int INVALID_CARD = 0x2001;

        private string _strReaderName = "OMNIKEY 5025 CL 0";
        private int _cardType = 0;

        private SCardRoutines scardr;
        private uint _hCard = 0;

        #region CONSTRUCTORS
        //Default Constructor
        public Ok5025Commands()
        {
            scardr = new SCardRoutines();
           
        }
        //Default Constructor
        public Ok5025Commands(UInt32 hCard)
        {
            this._hCard = hCard;
        }

        #endregion


        #region CARD CONNECTION COMMANDS

        #region ConnectCard
        /// <summary>
        /// Establsih connection with the reader
        /// </summary>
        /// <returns>SUCCESS or iClass Status Code</returns>
        public int ConnectCard()
        {
            int status = scardr.StartCardConnection(ref _cardType, _strReaderName);
            if (status == 0)
            {
                status = SUCCESS;
                _hCard = scardr.hCard;
            }
            else
                status = CARD_NOT_PRESENT;
            return status;
        }
        #endregion

        #region DisconnectCard
        /// <summary>
        /// Close connection with the card
        /// </summary>
        /// <returns>SUCCESS or iCLASS Status code</returns>
        public int DisconnectCard()
        {
            int status = scardr.DisconnectCard();
            if (status == 0)
                status = SUCCESS;
            else
                status = CARD_NOT_PRESENT;
            return status;
        }
        #endregion

       
        #endregion


        #region CheckIfCardIsPresent
        /// <summary>
        /// Verify if a card is present
        /// </summary>
        /// <returns></returns>
        public bool CheckIfCardIsPresent()
        {
            bool b = false;

            int cardstate = scardr.GetReaderState(_strReaderName);

            if (cardstate == SCardRoutines.CARD_PRESENT)
                b = true;
            if (cardstate == SCardRoutines.CARD_NOT_PRESENT)
                b = false;

            return b;
        }

        #endregion

        #region SetReader
        public void SetReader(string strReaderName)
        {
            _strReaderName = strReaderName;

        }
        #endregion

        #region GetCardID
        /// <summary>
        /// Obtiene el ID de la tarjeta Prox 125 KHz en formato Hexadecimal.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idlength"></param>
        /// <returns>regresa SUCCESS</returns>
        public int GetCardID(ref byte[] id, ref int idlength)
        {
            int resp = SUCCESS;
           
            uint pcbAttrLen = 64;

                id = scardr.GetAttrib(ref pcbAttrLen);
                idlength = (int)pcbAttrLen;
                if (pcbAttrLen == 0)
                    resp = INVALID_CARD;
                return resp;
         
        }
        #endregion

        #region GetUID
        /// <summary>
        /// Get Unique Identifier
        /// </summary>
        /// <param name="csn">8 byte array.</param>
        /// <param name="numbytes">CSN length</param>
        /// <returns>SUCCESS. iClass status code otherwise.</returns>
        public int GetUID(ref byte[] uid, ref int numbytes)
        {
            int status = 0;
            byte[] ucByteSend = new byte[16];
            byte[] pucReceiveData = new byte[16];
            ucByteSend[0] = 0xFF;//CLA 
            ucByteSend[1] = 0xCA;//INS 
            ucByteSend[2] = 0x00;//P1 
            ucByteSend[3] = 0x00;//P2 
            ucByteSend[4] = 0x00; //Le
            int ulnByteSend = 5;
            int pullReceiveDataBufLen = 36;

            int rc = scardr.Transmit(ucByteSend, ulnByteSend, ref pucReceiveData, ref pullReceiveDataBufLen);

            if (rc == 0)
            {
                status = pucReceiveData[pullReceiveDataBufLen - 2] << 8;
                status = status | pucReceiveData[pullReceiveDataBufLen - 1];

                if (status == SUCCESS)
                {
                    for (int i = 0; i < pullReceiveDataBufLen - 2; i++)
                        uid[i] = pucReceiveData[i];
                    numbytes = pullReceiveDataBufLen - 2;
                }

            }


            return status;
        }
        #endregion
    }
}
