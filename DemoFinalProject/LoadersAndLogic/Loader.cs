using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SharedCode;
using DatabaseModel;
using SharedCode.Security;
using System.Threading;

namespace LoadersAndLogic
{
    public class Loader
    {
        //public static BlockingQueue<ApplicationVM> recvErrQ = new BlockingQueue<ApplicationVM>();

        //static Loader()
        //{
        //    var logThread = new Thread(
        //        () =>
        //        {
        //            while(true)
        //            {
        //                List<ApplicationVM> appErrList = new List<ApplicationVM>();
        //                var appVM = recvErrQ.deQ();   // could be blocked here
        //                appErrList.Add(appVM); 

        //                int size = recvErrQ.size();
        //                for( int i=0; i<size; ++i)
        //                {
        //                    appErrList.Add(recvErrQ.deQ());
        //                }
        //                insertLogs(appErrList);
        //                Thread.Sleep(5000);    // every 5s to check whether insert logs to database.
        //            }
        //        }
        //        );
        //    logThread.Start();
        //    //logThread.Join();
        //}



        #region ----< QUERY >-----


        public static LoginInfoVM queryLoginfoPersonBySession(string hash)
        {
            LoginInfoVM info = new LoginInfoVM();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                if (db.LoginSet.Any(x => x.Password == hash))
                {
                    var dbinfo = db.LoginSet.Where(x => x.Password == hash).FirstOrDefault();
                    info.LoginId = dbinfo.LoginId;
                    info.LoginTime = dbinfo.LoginTime;
                    info.Password = dbinfo.Password;
                    info.IsLive = dbinfo.IsLive;
                    info.Role = dbinfo.Role;
                    info.OnePerson = new PersonVM()
                    {
                        PersonId = dbinfo.OnePerson.PersonId,
                        FirstName = dbinfo.OnePerson.FirstName,
                        LastName = dbinfo.OnePerson.LastName,
                        Email = dbinfo.OnePerson.Email
                    };
                }
            }
            return info;
        }

        public static LoginInfoVM queryLoginPersonByPersonId(int personId)
        {
            LoginInfoVM info = null;
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var query = (from n in db.LoginSet
                             where n.OnePerson.PersonId == personId
                             select n).First();

                PersonVM person = new PersonVM()
                {
                    PersonId = query.OnePerson.PersonId,
                    FirstName = query.OnePerson.FirstName,
                    LastName = query.OnePerson.LastName,
                    Email = query.OnePerson.Email
                };

                info = new LoginInfoVM()
                {
                    LoginId = query.LoginId,
                    LoginTime = query.LoginTime,
                    Password = query.Password,
                    Role = query.Role,
                    IsLive = query.IsLive,
                    OnePerson = person
                };
            }
            return info;
        }

        public static ICollection<LoginInfoVM> queryAllLoginPersons()
        {
            ICollection<LoginInfoVM> lgs = new List<LoginInfoVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryLogins = (from n in db.LoginSet select n).ToList();
                foreach (var login in queryLogins)
                {
                    lgs.Add(new LoginInfoVM()
                    {
                        OnePerson = new PersonVM()
                        {
                            PersonId = login.LoginId,
                            FirstName = login.OnePerson.FirstName,
                            LastName = login.OnePerson.LastName,
                            Email = login.OnePerson.Email
                        },
                        LoginId = login.LoginId,
                        LoginTime = login.LoginTime,
                        Password = login.Password,
                        IsLive = login.IsLive,
                        Role = login.Role
                    });

                }
            }

            return lgs;
        }

        // note: user role cannot access deactive app
        public static ICollection<ApplicationVM> queryAllAppErrsByPsersonId(int id)
        {
            ICollection<ApplicationVM> appVMs = new List<ApplicationVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryUserRole = (from n in db.LoginSet where n.OnePerson.PersonId == id select n).First().Role;

                List<Application> queryApps;
                // check whether user-role, if yes, filter deactive apps
                if( queryUserRole < (int)RoleEnum.Admin )
                {
                    queryApps = (from n in db.ApplicationSet
                                 where n.Persons.Any(x => x.PersonId == id) && n.IsActive==true
                                 select n).ToList();
                }
                else
                {
                    queryApps = (from n in db.ApplicationSet
                                 where n.Persons.Any(x => x.PersonId == id)
                                 select n).ToList();
                }

                foreach (var app in queryApps)
                {
                    var appVM = new ApplicationVM()
                    {
                        ApplicationId = app.ApplicationId,
                        AppName = app.AppName,
                        IsActive = app.IsActive                        
                    };

                    foreach (var err in app.Errors)
                    {
                        var errVm = new ErrorVM()
                        {
                            ErrorId = err.ErrorId,
                            ErrorMessage = err.ErrorMessage,
                            Time = err.Time,
                            LogLevel = err.LogLevel,
                            ExMessage = err.ExMessage                            
                        };
                        appVM.Errors.Add(errVm);
                    }

                    appVMs.Add(appVM);
                }
            }

            return appVMs;
        }

        public static ICollection<ErrorVM> queryErrorsByAppId(int AppId)
        {
            ICollection<ErrorVM> errVMs = new List<ErrorVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryApp = (from n in db.ApplicationSet where n.ApplicationId == AppId select n).First();

                queryApp.Errors.ToList().ForEach(
                    err => errVMs.Add( new ErrorVM() {
                        ErrorId =err.ErrorId,
                        ErrorMessage = err.ErrorMessage,
                        Time = err.Time,
                        LogLevel = err.LogLevel,
                        ExMessage = err.ExMessage
                    } )
                    );
            }
            return errVMs;
        }

        // just all apps from ApplicationSet
        public static ICollection<ApplicationVM> queryAllApps( )
        {
            ICollection<ApplicationVM> appsVM = new List<ApplicationVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryApps = (from n in db.ApplicationSet select n).ToList();
                queryApps.ForEach(app => appsVM.Add(new ApplicationVM()
                {
                    ApplicationId = app.ApplicationId,
                    AppName = app.AppName,
                    IsActive = app.IsActive
                }
                ) );
            }
            return appsVM;
        }

        // qurey all apps may/ maynot include person id
        public static ICollection<ApplicationVM> queryAllAppsUsers()
        {
            ICollection<ApplicationVM> appsVM = new List<ApplicationVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryApps = (from n in db.ApplicationSet select n).ToList();
                queryApps.ForEach(
                    app =>
                    {
                        appsVM.Add(new ApplicationVM()
                        {
                            ApplicationId = app.ApplicationId,
                            AppName = app.AppName,
                            IsActive = app.IsActive,
                            Persons = new List<PersonVM>()
                        });

                        app.Persons.ToList().ForEach( 
                            person =>
                            appsVM.Last().Persons.Add( new PersonVM() { PersonId = person.PersonId, FirstName = person.FirstName, LastName=person.LastName, Email=person.Email } )
                        );
                    }
                 );
            }
            return appsVM;
        }

        // query all person's apps
        public static ICollection<PersonVM> queryAllUserApps()
        {
            ICollection<PersonVM> usersVM = new List<PersonVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var query = (from u in db.PersonSet
                             select u).ToList();

                foreach ( var user in query)
                {
                    var myApps = new List<ApplicationVM>();
                    (from app in user.Applications
                     select app).ToList().ForEach( a=> myApps.Add(
                         new ApplicationVM()
                         {
                             ApplicationId = a.ApplicationId,
                             AppName = a.AppName,
                             IsActive = a.IsActive
                         }
                         ));
                    usersVM.Add(
                    new PersonVM()
                    {
                        PersonId = user.PersonId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Applications = myApps
                    });
                }       
            }
            return usersVM;
        }


        //  Note: user-role is not allowed to acess deactive app
        public static ICollection<ApplicationVM> queryAllAppsByPersonId( int pid )
        {
            ICollection<ApplicationVM> appsVM = new List<ApplicationVM>();
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var userRole = (from n in db.LoginSet where pid == n.OnePerson.PersonId select n).First().Role;

                List<Application> queryApps;

                // user-role not access deactive app
                if( userRole < (int)SharedCode.Security.RoleEnum.Admin )
                {
                    queryApps = (from n in db.ApplicationSet
                                 where n.Persons.Any(p => p.PersonId == pid) && n.IsActive == true
                                 select n).ToList();
                }else
                {
                    queryApps = (from n in db.ApplicationSet
                                 where n.Persons.Any(p => p.PersonId == pid)
                                 select n).ToList();
                }

                queryApps.ForEach(app => appsVM.Add(new ApplicationVM()
                {
                    ApplicationId = app.ApplicationId,
                    AppName = app.AppName,
                    IsActive = app.IsActive
                }
                ));
            }
            return appsVM;
        }

        // only query app
        public static ApplicationVM queryAppByAppId(int appId)
        {
            ApplicationVM appVM;
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var queryDbApp = (from a in db.ApplicationSet where a.ApplicationId == appId select a).First();

                appVM = new ApplicationVM()
                {
                    ApplicationId = queryDbApp.ApplicationId,
                    AppName = queryDbApp.AppName,
                    IsActive = queryDbApp.IsActive
                };

                //db.SaveChanges();
            }
            return appVM;
        }

        #endregion

        #region  ---- < Create > -----

        // toDo, use SALT, prevent duplicate registering
        public static void createLoginPerson(LoginInfoVM lo)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                Person person = new Person()
                {
                    FirstName = lo.OnePerson.FirstName,
                    LastName = lo.OnePerson.LastName,
                    Email = lo.OnePerson.Email
                };

                string hashedPwd = SharedCode.Authentication.Authentication.HashPassword(person.Email, lo.Password);
                db.LoginSet.Add(new LoginInfo() { Password = hashedPwd, IsLive=true, Role = (int)RoleEnum.User, OnePerson = person });
                db.SaveChanges();
            }
        }

        public static void createApp(ApplicationVM app)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                db.ApplicationSet.Add(
                        new Application()
                        {
                            AppName = app.AppName,
                            IsActive = app.IsActive
                        }
                    );
                db.SaveChanges();
            }
        }


        /// <summary>
        ///  run on a dedicated thread in static constructor using BlockingQ. 
        ///  List paramter help improve DB performance, opening DB once and insert chunk logs
        /// </summary>
        public static void insertLogs( ICollection<ApplicationVM> appErrList )
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                db.Database.Initialize(false);

                foreach( var appErrVM in appErrList )
                {
                    var queryApp = (from app in db.ApplicationSet
                                    where app.ApplicationId == appErrVM.ApplicationId
                                    select app).First();
                    if (queryApp == null)
                        continue;
                    appErrVM.Errors.ToList().ForEach(errVM => queryApp.Errors.Add(
                      new Error()
                      {
                          ErrorMessage = errVM.ErrorMessage,
                          Time = errVM.Time,
                          LogLevel = errVM.LogLevel,
                          ExMessage = errVM.ExMessage
                      }
                      ));
                }

                db.SaveChanges();
            }
        }

        // -- < For .Net WebApplication Log > --

        public static void insertOneLog(ApplicationVM apperrVM)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                db.Database.Initialize(false);

                var queryApp = (from app in db.ApplicationSet
                                where app.ApplicationId == apperrVM.ApplicationId
                                select app).First();
                if (queryApp != null)
                {
                    foreach( var errVM in apperrVM.Errors)
                    {
                        queryApp.Errors.Add(new Error() {
                            ErrorMessage =errVM.ErrorMessage,
                            Time =errVM.Time,
                            LogLevel =errVM.LogLevel,
                            ExMessage =errVM.ExMessage});
                    }   
                }

                db.SaveChanges();
            }
        }
        #endregion

        #region ---- < UPDATE > -----

        public static void updateLoginTime(int loginId, DateTime now)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                db.Database.Initialize(false);

                var dbQuery = (from n in db.LoginSet
                               where n.LoginId == loginId
                               select n).First();
                dbQuery.LoginTime = now;

                db.SaveChanges();
            }
        }


        public static void updateLoginPersion(LoginInfoVM lg)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                db.Database.Initialize(false);

                var dbinfo = (from n in db.LoginSet
                            where n.LoginId==lg.LoginId
                            select n).First();

                dbinfo.Password = SharedCode.Authentication.Authentication.HashPassword(lg.OnePerson.Email, lg.Password);
                dbinfo.Role = lg.Role;
                dbinfo.IsLive = lg.IsLive;
                dbinfo.OnePerson.FirstName = lg.OnePerson.FirstName;
                dbinfo.OnePerson.LastName = lg.OnePerson.LastName;
                dbinfo.OnePerson.Email = lg.OnePerson.Email;
                dbinfo.LoginTime = dbinfo.LoginTime < lg.LoginTime ? lg.LoginTime : dbinfo.LoginTime;

                db.SaveChanges();
            }
        }

        // assign an application to some user / note in appFVM, personid could = 0!
        public static void updateAppForActiveAndOwnership( ApplicationPersonIdFlatVM appFVM)
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                db.Database.Initialize(false);

                var queryDbApp = (from a in db.ApplicationSet where a.ApplicationId == appFVM.ApplicationId select a).First();
                queryDbApp.AppName = appFVM.AppName;
                queryDbApp.IsActive = appFVM.IsActive;

                // the updated PersonId could be 0, means not related to any user

                if( appFVM.PersonId > 0)
                {
                    var queryDbPerson = (from p in db.PersonSet where p.PersonId == appFVM.PersonId select p).FirstOrDefault();

                    if(queryDbPerson!=null)
                    {
                        // only update user's app if he hasn't own it

                        if (false == queryDbApp.Persons.Any(p => p.PersonId == queryDbPerson.PersonId))
                        {
                            queryDbApp.Persons.Add(queryDbPerson);
                        }
                    }  
                }
                db.SaveChanges();
            }
        }


        #endregion

        #region ---- < Delete > ----

        /// <summary>
        ///  delete all logs under an application
        /// </summary>
        /// <param name="ApplicationId"></param>

        public static void deleteAllLogsBelongToApp( int ApplicationId )
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                var dbQueryApp = (from n in db.ApplicationSet
                                  where n.ApplicationId == ApplicationId
                                  select n).First();
                
                //dbQueryApp.Errors.Clear();  it doesn't delete rows in ErrorTable

                dbQueryApp.Errors.ToList().ForEach(
                    err => db.ErrorSet.Remove(err)
                    );

                db.SaveChanges();
            }
        }

        /// <summary>
        ///  Delete one item of errLog via ErrId
        /// </summary>
        /// <param name="ApplicationId"></param>
        public static void deleteOneLog( int ErrId )
        {
            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);
         
                var dbQueryErr = (from n in db.ErrorSet where n.ErrorId == ErrId select n).First();
                db.ErrorSet.Remove(dbQueryErr);

                db.SaveChanges();
            }
        }

        #endregion


        #region ----< Create & Delete DB >----
        /// <summary>
        /// Creates the DB
        /// </summary>
        public static void CreateDB()
        {
            Console.WriteLine("~~~~ Creating the DB ~~~~\n");

            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                // Seeding runs the first time you try to use the DB, so we make it seed here..
                // It only runs IF the initializer condition is met, regardless of the True/False above
                db.PersonSet.Count();
            }
        }

        /// <summary>
        /// Deletes the DB
        /// </summary>
        public static void DeleteDB()
        {
            Console.WriteLine("~~~~ Deleting the DB ~~~~");
            Console.WriteLine();

            using (DatabaseModel.ErrorModelDbContext db = new DatabaseModel.ErrorModelDbContext())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
            }
        }
        #endregion

    }

}
