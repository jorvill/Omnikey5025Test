using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JvlSCard;

namespace Ok5025Test
{
    class ReaderDetector
    {
        private bool _IsReaderConnected = false;
        private string _ReaderName = "OMNIKEY CARDMAN 5x21 CL 0";

        private SCardRoutines scardr;

        private ArrayList _ReaderList;

        public ReaderDetector()
        {
            scardr = new SCardRoutines();
        }


        #region ListReaders
        public void ListReaders()
        {
             
                  
            _ReaderList = scardr.ListReaders();
            if (_ReaderList.Count > 0)
            {
                _IsReaderConnected = true;
                for (int i = 0; i < _ReaderList.Count;i++)          
                    Console.WriteLine(" ({0}) {1} ",i, _ReaderList[i]);
            }
            else
            {
                _IsReaderConnected = false;
            }
            
            
        }
        #endregion

        #region SelectReader
        public void SelectReader(int number)
        {
            try
            {
                _ReaderName = _ReaderList[number].ToString();
                Console.WriteLine("Selected Reader: {0}", _ReaderName);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Invalid Reader Number!");
            }
        }
        #endregion


        public bool IsReaderConnected
        {
            get { return _IsReaderConnected; }
        }

        public string ReaderName
        {
            get { return _ReaderName; }
        }
    }
    
}
