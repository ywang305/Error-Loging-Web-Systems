
namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Error
    {
        [Key]
        public int ErrorId { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime Time { get; set; }

        public int LogLevel { get; set; }

        public string ExMessage { get; set; }
    }
}
