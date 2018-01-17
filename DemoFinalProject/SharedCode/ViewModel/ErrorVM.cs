using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedCode
{
    public class ErrorVM
    {
        public int ErrorId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Time { get; set; }
        public int LogLevel { get; set; }
        public string  ExMessage { get; set; }
    }
}
