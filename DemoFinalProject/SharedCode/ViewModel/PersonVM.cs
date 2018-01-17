using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedCode
{
    public class PersonVM
    {
        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<ApplicationVM> Applications { get; set; } = new List<ApplicationVM>();
    }
}
