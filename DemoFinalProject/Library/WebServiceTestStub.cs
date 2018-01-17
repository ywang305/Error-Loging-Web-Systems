using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Formatting;

//namespace Library
//{
//    public class WebServiceTestStub
//    {
//        private static int SERVICE_PORT = 54531;
//        private static string SERVICE_URL = "http://localhost:{0}/";  //54531
//        private static string LOG_ACTION = "Api/ErrorLogger";

//        private static void WriteErrorLog( ErrorLog errlog )
//        {
//            int tries = 100;
//            while( tries >0 )
//            {
//                try
//                {
//                    HttpClient client = new HttpClient();
//                    client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));

//                    client.PostAsJsonAsync(LOG_ACTION, errlog).Wait();
//                    Console.WriteLine("Call WebService completed.");
//                    Console.ReadLine();
//                    break;
//                }
//                catch (Exception e)
//                {
//                    string except = e.Message;
//                    System.Threading.Thread.Sleep(400);
//                    tries -= 1;
//                    continue;
//                }
//            }   
//        }


//        // Test Stub
//        static void Main(string[] args)
//        {
//            ErrorLog aLog = new ErrorLog()
//            {
//                AppId = 2,
//                Message = "Overflow error",
//                Time = DateTime.Now
//            };
//            WriteErrorLog(aLog);
//        }
//    }
//}
