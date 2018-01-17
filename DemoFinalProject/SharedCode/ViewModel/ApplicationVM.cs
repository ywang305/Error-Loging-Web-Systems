using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedCode
{
    public class ApplicationVM
    {
        public int ApplicationId { get; set; }

        [Required]
        public string AppName { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<PersonVM> Persons { get; set; } = new List<PersonVM>();

        public ICollection<ErrorVM> Errors { get; set; } = new List<ErrorVM>();

    }
}
