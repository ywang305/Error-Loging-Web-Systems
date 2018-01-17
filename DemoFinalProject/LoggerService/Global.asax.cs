using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SharedCode;


namespace LoggerService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Controllers.BlockingQWrapper<ApplicationVM>.Initialize();

            var logThread = new System.Threading.Thread(
                () =>
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(5000);    // every 5s to check whether insert logs to database.
                        List<ApplicationVM> appErrList = new List<ApplicationVM>();
                        var appVM = Controllers.BlockingQWrapper<ApplicationVM>.rcvQ.deQ();   // could be blocked here
                        appErrList.Add(appVM);

                        int size = Controllers.BlockingQWrapper < ApplicationVM >.rcvQ.size();
                        for (int i = 0; i < size; ++i)
                        {
                            appErrList.Add(Controllers.BlockingQWrapper < ApplicationVM >.rcvQ.deQ());
                        }
                        LoadersAndLogic.Loader.insertLogs(appErrList);
                        
                    }
                }
            );
            logThread.Start();
        }


    }
}
