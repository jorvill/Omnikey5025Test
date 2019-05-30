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

namespace Ok5025Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Omnikey 5025 CL Test";
            ReaderDetector readerDetector = new ReaderDetector();

            Console.WriteLine("Reader List:");
            readerDetector.ListReaders();
            int rselec = 0;
            Console.Write("Select Reader: ");
            bool ok = Int32.TryParse(Console.ReadLine(), out rselec);
            if (ok)
            {
                
                readerDetector.SelectReader(rselec);
                Ok5025Commands ok5025 = new Ok5025Commands();
                ok5025.SetReader(readerDetector.ReaderName);

                string resp = "Continue";
                int status = Ok5025Commands.SUCCESS;
                int length = 0;
                byte[] pacdata = new byte[16];
                bool cardIsPresent = false;
                do
                {
                    Console.WriteLine("Present a card to the reader and press ENTER");
                    Console.ReadLine();

                    //Check if card is Present

                    cardIsPresent = ok5025.CheckIfCardIsPresent();

                    if (cardIsPresent)
                    {
                        status = ok5025.ConnectCard();
                        if (status == Ok5025Commands.SUCCESS)
                        {
                            status = ok5025.GetCardID(ref pacdata, ref length);
                            Console.WriteLine("{0} [{1} bytes]", Utilities.ConvertByteArrayToStringHex(pacdata, length), length);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Present a card and try again");
                    }

                    Console.WriteLine("Press ENTER to read other card or write EXIT to leave...");
                    Console.Write(">");
                    resp = Console.ReadLine().ToUpper();

                } while (resp != "EXIT");

            }
            else
            {
                Console.WriteLine("Wrong!");
            }
            

           

        }
    }
}
