using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedCode;

namespace LoggerService.Controllers
{
    public class BlockingQWrapper<T>
    {
        public static BlockingQueue<T> rcvQ;

        public static void Initialize()
        {
            rcvQ = new BlockingQueue<T>();
        }        
    }
}