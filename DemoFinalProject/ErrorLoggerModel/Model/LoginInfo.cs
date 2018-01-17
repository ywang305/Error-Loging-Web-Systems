namespace DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LoginInfo
    {
        [Key]
        public int LoginId { get; set; }

        public System.DateTime LoginTime { get; set; } = System.DateTime.Now;

        public string Password { get; set; }

        public bool IsLive { get; set; }

        public int Role { get; set; } = 0;
        /// <summary>
        /// !!!!! If you do not make this virtual, navigational properties will NOT work
        /// </summary>
        public virtual Person OnePerson { get; set; }
    }
}
