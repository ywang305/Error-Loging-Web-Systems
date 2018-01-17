
namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// A custom initializer that will seed the data in.
    /// 
    /// There are 3 types of Initializers..
    ///     DropCreateDatabaseIfModelChanges
    ///     CreateDatabaseIfNotExists
    ///     DropCreateDatabaseAlways
    /// </summary>
    class ErrorModelDbInitializer : DropCreateDatabaseIfModelChanges<ErrorModelDbContext>
    {
        /// <summary>
        /// Seeds data into the DB
        /// </summary>
        protected override void Seed(ErrorModelDbContext context)
        {
            if ( true )
            {
                #region seeding Db with Table's item
                Console.WriteLine("  ###  Seeding  ####");

                Person person1 = new Person()
                {
                    //PersonId = 0001,
                    FirstName = "Bruce",
                    LastName = "Willies",
                    Email = "BruceWillies@yahoo.com",
                    Applications = new List<Application>()
                };
                Application app1 = new Application()
                {
                    //ApplicationId = 1,
                    AppName = "My iOS Mobile App",
                    IsActive = true,
                    Persons = new List<Person>() { person1 },
                    Errors = new List<Error>()
                };
                Application app2 = new Application()
                {
                    //ApplicationId = 2,
                    AppName = "My Android Mobile App",
                    IsActive = true,
                    Persons = new List<Person>() { person1 },
                    Errors = new List<Error>()                    
                };
                person1.Applications.Add(app1);
                person1.Applications.Add(app2);
                LoginInfo admin1 = new LoginInfo()
                {
                    //LoginId = 0001,
                    LoginTime = DateTime.Now,
                    Password = SharedCode.Authentication.Authentication.HashPassword(person1.Email, "pwd"),
                    IsLive = true,
                    Role = 8,
                    OnePerson = person1
                };
                Error err1 = new Error()
                {
                    //ErrorId=1,
                    ErrorMessage = "Null Objection Reference!",
                    Time = DateTime.Now,
                    LogLevel = 2,
                    ExMessage = new Exception().ToString()
                };
                Error err2 = new Error()
                {
                    //ErrorId=2,
                    ErrorMessage = "Runtime error",
                    Time = DateTime.Now,
                    LogLevel = 4,
                    ExMessage = new Exception().ToString()
                };
                Error err3 = new Error()
                {
                    //ErrorId = 2,
                    ErrorMessage = "Runtime error",
                    Time = DateTime.Now,
                    LogLevel = 4,
                    ExMessage = new Exception().ToString()
                };
                app1.Errors.Add(err1);
                app1.Errors.Add(err2);
                app2.Errors.Add(err3);



                // The order is important, since we are setting up references
                context.PersonSet.Add(person1);
                context.LoginSet.Add(admin1);
                context.ErrorSet.Add(err1);
                context.ErrorSet.Add(err2);
                context.ApplicationSet.Add(app1);
                context.ApplicationSet.Add(app2);
                
                
                ///<example> below exception, not use below!</example>
                /*context.LoginSet.Add(admin1);
                context.ApplicationSet.Add(app1);
                context.ApplicationSet.Add(app2); */

                
                #endregion
            }


            // letting the base method do anything it needs to get done
            base.Seed(context);

            // Save the changes you made, when adding the data above
            context.SaveChanges();
        }
    }
}
