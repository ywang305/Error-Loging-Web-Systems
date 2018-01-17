using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Library
{
    using SharedCode;

    public class Logger
    {
        private int ApplicationId;
        private int SERVICE_PORT = 54531;
        //private string SERVICE_URL = "http://localhost:{0}/";  //54531
        private string SERVICE_URL = "http://localhost/LoggerService:{0}/";  //54531
        private string LOG_ACTION = "Api/ErrorLogger";
        private HttpClient client = new HttpClient();

        private BlockingQueue<ErrorVM> sendQ = new BlockingQueue<ErrorVM>();


        /// <summary>
        /// Constructor. Put whatever initialization code in here that you need
        /// </summary>
        public Logger( int AppId, string serverUrl= "http://localhost:{0}/", int serverPort = 54531)
        {
            ApplicationId = AppId;
            SERVICE_URL = serverUrl;
            SERVICE_PORT = serverPort;
            client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));

            Thread sendThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(200); // sleep and let sendQ be filled up

                    /// send out a list of errors rather than one by one. 
                    var errVM = sendQ.deQ();        // here could be block by empty Q!
                    ApplicationVM appErr = new ApplicationVM()
                    {
                        ApplicationId = this.ApplicationId,
                    };
                    appErr.Errors.Add(errVM);

                    int qsize = sendQ.size();
                    for (int i = 0; i < qsize; ++i)
                    {
                        errVM = sendQ.deQ();
                        appErr.Errors.Add(errVM);
                    }

                    callLoggerWebApi(appErr);
                }
            }
            );
            sendThread.Start();
        }

        /// <summary>
        /// This method is called by the test harness. So inside of it you should call your logger..
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <param name="logLevel">Error Log Level</param>
        /// <param name="ex">Optional Exception</param>
        //public void Log(string errorMessage, int logLevel, Exception ex = null)
        //{
        //    //   logger() here .  this is a stub to allow us to do mean things....
        //    //   Configure Err View Model

        //    ErrorVM errVM = new ErrorVM()
        //    {
        //        ErrorMessage = errorMessage,
        //        Time = DateTime.Now,
        //        LogLevel = logLevel,
        //        ExMessage = (ex == null) ? "NULL" : ex.ToString()
        //    };

        //    sendQ.enQ(errVM);

        //    Console.WriteLine(errVM.ErrorMessage);
        //}

        public void Log(string errorMessage, int logLevel, Exception ex = null)
        {
            ErrorVM errVM = new ErrorVM()
            {
                ErrorMessage = errorMessage,
                Time = DateTime.Now,
                LogLevel = logLevel,
                ExMessage = (ex == null) ? "NULL" : ex.ToString()
            };

            sendQ.enQ(errVM);
            //await Task.Run( ()=> sendQ.enQ(errVM) );

            Console.WriteLine(errVM.ErrorMessage);
        }

        private async void callLoggerWebApi(ApplicationVM appErr)
        {
            while (true)
            {
                try
                {
                    await Task.Run(()=>client.PostAsJsonAsync(LOG_ACTION, appErr).Wait());

                    Console.WriteLine("Logger WebService completed.");
                    Console.ReadLine();
                    break;
                }
                catch (Exception e)
                {
                    string except = e.Message;
                    Console.WriteLine(except);
                    System.Threading.Thread.Sleep(400);

                    continue;
                }
            }
        }
    }
}

