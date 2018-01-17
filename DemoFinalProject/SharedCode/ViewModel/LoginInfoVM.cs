using System;
using System.ComponentModel.DataAnnotations;

namespace SharedCode
{
    public class LoginInfoVM
    {
        public int LoginId { get; set; }
        public DateTime LoginTime { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string Password { get; set; }

        public bool IsLive { get; set; }

        public int Role { get; set; } = 0;
        public PersonVM OnePerson { get; set; }
    }
}
