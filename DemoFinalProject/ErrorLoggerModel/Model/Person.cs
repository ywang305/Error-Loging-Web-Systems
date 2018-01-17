namespace DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(80)]
        public string Email { get; set; }

        public virtual ICollection<Application> Applications { get; set; }


    }
}
