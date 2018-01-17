
namespace DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        [MaxLength(50)]
        public string AppName { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Person> Persons { get; set; }  // many to many

        public virtual ICollection<Error> Errors { get; set; }  // 1 to many
    }
}
