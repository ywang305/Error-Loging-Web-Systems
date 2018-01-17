namespace DatabaseModel
{
    using System;
    using System.Data.Entity;

    public partial class ErrorModelDbContext  : DbContext
    {
        public ErrorModelDbContext()
            : base("name=ErrorModel")
        {
            // Set the custom initializer
            Database.SetInitializer<ErrorModelDbContext>(new ErrorModelDbInitializer());
        }

        #region Properties used to build the DB

        public DbSet<Person> PersonSet { get; set; }
        public DbSet<LoginInfo> LoginSet { get; set; }
        public DbSet<Application> ApplicationSet { get; set; }
        public DbSet<Error> ErrorSet { get; set; }

        #endregion

        /// <summary>
        /// If you want to do anything custom, you can put it in here.. Otherwise, the method is a waste
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
