using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SharedCode;
using LoadersAndLogic;
using System.Threading;

namespace LoggerService.Controllers
{
    public class ErrorLoggerController : ApiController
    {
        /*
         * [HttpPost] Post = Create   
         * Get = query
         * Delete = delete
         * PUT = update
        */

        // POST  api/ErrorLogger
        public void PostWriteLogger(ApplicationVM appErr)
        {
            BlockingQWrapper<ApplicationVM>.rcvQ.enQ(appErr);
        }
    }
}

