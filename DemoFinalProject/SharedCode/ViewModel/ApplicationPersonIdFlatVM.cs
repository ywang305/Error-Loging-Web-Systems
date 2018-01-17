using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedCode
{
    public class ApplicationPersonIdFlatVM
    {
        public int ApplicationId { get; set; }

        [Required]
        public string AppName { get; set; }
        [Required]
        public bool IsActive { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int PersonId { get; set; }
    }
}
