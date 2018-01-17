using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoadersAndLogic;
using SharedCode;

namespace WebApplication1.Controllers
{
    using System.Collections.ObjectModel;

    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {    
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(LoginInfoVM postedObj)
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";

            if (ModelState.IsValid &&  Authentication.AuthenticateUser(postedObj.OnePerson.Email, postedObj.Password) )
            {
                Session["hash"] = Authentication.HashPassword(postedObj.OnePerson.Email, postedObj.Password);

                LoginInfoVM info = Loader.queryLoginfoPersonBySession((string)Session["hash"]);

                Loader.updateLoginTime(info.LoginId, DateTime.Now);

                if( info.Role >= (int)SharedCode.Security.RoleEnum.Admin )
                {
                    return RedirectToAction(WebConstants.HOME_ADMIN_PAGE);
                }
                else
                {
                    return RedirectToAction(WebConstants.HOME_USER_PAGE);
                }
            }

            return View(postedObj);
        }



        /// <summary>
        /// Reigester a user-ONLY role! 
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Register(LoginInfoVM postedObj)
        {
            if (ModelState.IsValid && Authentication.RegisterUser(postedObj.OnePerson.Email, postedObj.Password))
            {
                Loader.createLoginPerson(postedObj);  // this only create a Use-Role
                return RedirectToAction(WebConstants.HOME_PAGE);
            }
            return View();         
        }


        // Requires User role
        public ActionResult UserPage()
        {
            LoginInfoVM info = Loader.queryLoginfoPersonBySession((string)Session["hash"]);
            return View(info);
        }

        // Requires Admin role
        public ActionResult AdminPage()
        {
            var lps = Loader.queryAllLoginPersons();
            return View(lps);
        }
        
      

        // Requires Admin role, Admin can assign ANY ROLE to new user!!!
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUser(LoginInfoVM postedObj)
        {

            //ViewBag.Command = "view_detail";
            if (ModelState.IsValid && Authentication.RegisterUser(postedObj.OnePerson.Email, postedObj.Password))
            {
                Loader.createLoginPerson(postedObj);
                return RedirectToAction(WebConstants.HOME_ADMIN_PAGE);
            }
            return View(postedObj);
        }

        // Requires role >= user 
        public ActionResult EditUser(int PersonId)
        {
            // don't allow regular user to forgery "PersonId", that he can only view his own infomation
            //LoginInfoVM loginUser = Loader.queryLoginfoPersonBySession((string)Session["hash"]);
            //if( !UnForgeryLink.CheckLinkForPersonId(role, PersonId, loginUser))
            //{
            //    ViewBag.UserRoleLimit = "Forgery link is not allowed!";
            //    return View();
            //}

            LoginInfoVM savedInfo = Loader.queryLoginPersonByPersonId(PersonId);

            ViewBag.myRole = this.role;

            if( this.role >= (int)SharedCode.Security.RoleEnum.Admin)
            {
                return View(savedInfo);
            }
            else if (savedInfo.IsLive == false && this.role < (int)SharedCode.Security.RoleEnum.Admin)
            {
                ViewBag.UserRoleLimit = "As a deactive user, you need require admin to active you!";
                return View();
            }

            return View(savedInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditUser(LoginInfoVM postedObj)
        { 
            if (ModelState.IsValid)
            {
                Loader.updateLoginPersion(postedObj);
                  
                if ( this.role >= (int)SharedCode.Security.RoleEnum.Admin )
                {
                    return RedirectToAction(WebConstants.HOME_ADMIN_PAGE);
                }
                else
                {
                    return RedirectToAction(WebConstants.HOME_USER_PAGE);
                }
            }
            return View(postedObj);
        }

        


        /// <summary>
        ///  Admin shall create a new application
        /// </summary>
        /// <returns></returns>
        public ActionResult AddApp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddApp(ApplicationVM postedObj)
        {
            if (ModelState.IsValid )
            {
                var apps = Loader.queryAllApps();

                if( !apps.Any(x=>x.AppName==postedObj.AppName) )
                {
                    Loader.createApp(postedObj);
                }
                return RedirectToAction(WebConstants.HOME_ADMIN_PAGE);
            }
            return View(postedObj);
        }

        // user role
        public ActionResult ViewApps(int PersonId)
        { 
            // user role not access deactive app in the query.
            ICollection<ApplicationVM> queryAppsByPid = Loader.queryAllAppsByPersonId(PersonId);
            return View(queryAppsByPid);
        }


        // admin role
        public ActionResult ViewAllApps( )
        {
            var apps = Loader.queryAllAppsUsers();
  
            return View(apps);
        }


        //user role
        public ActionResult ViewErrorLog( LogOrderSearchVM errsWithOp )
        {
            //// user role cannot access deactive app in database query

            var errCollection = Loader.queryErrorsByAppId(errsWithOp.AppId);

            var AppName = Loader.queryAppByAppId(errsWithOp.AppId).AppName;
            ViewBag.AppName = AppName;
            ViewBag.AppId = errsWithOp.AppId;

            ViewBag.IdSortParam = errsWithOp.sortOrder == null ? "id_desc" : "";
            ViewBag.MsgSortParam = errsWithOp.sortOrder == "msg" ? "msg_desc" : "msg";
            ViewBag.TimeSortParam = errsWithOp.sortOrder == "time" ? "time_desc" : "time";
            ViewBag.LevelSortParam = errsWithOp.sortOrder == "level" ? "level_desc" : "level";
            ViewBag.ExSortParam = errsWithOp.sortOrder == "ex" ? "ex_desc" : "ex";

            switch (errsWithOp.sortOrder)
            {
                case "id_desc":
                    errCollection = errCollection.OrderByDescending(e => e.ErrorId).ToList();
                    break;
                case "msg":
                    errCollection = errCollection.OrderBy(e => e.ErrorMessage).ToList();
                    break;
                case "msg_desc":
                    errCollection = errCollection.OrderByDescending(e => e.ErrorMessage).ToList();
                    break;
                case "time":
                    errCollection = errCollection.OrderBy(e => e.Time).ToList();
                    break;
                case "time_desc":
                    errCollection = errCollection.OrderByDescending(e => e.Time).ToList();
                    break;
                case "level":
                    errCollection = errCollection.OrderBy(e => e.LogLevel).ToList();
                    break;
                case "level_desc":
                    errCollection = errCollection.OrderByDescending(e => e.LogLevel).ToList();
                    break;
                case "ex":
                    errCollection = errCollection.OrderBy(e => e.ExMessage).ToList();
                    break;
                case "ex_desc":
                    errCollection = errCollection.OrderByDescending(e => e.ExMessage).ToList();
                    break;
                default:
                    errCollection = errCollection.OrderBy(e => e.ErrorId).ToList();
                    break;
            }


            // list? chart?
            if (errsWithOp.chartId != null)
            {
                ViewBag.ChartId = errsWithOp.chartId;
            }

            return View(errCollection);
        }


        // user role

        public ActionResult ClearLogs( int AppId, int? ErrId )
        {
            if (ErrId == null )
            {
                Loader.deleteAllLogsBelongToApp(AppId);
            }
            else
            {
                Loader.deleteOneLog((int)ErrId);
            }
            return RedirectToAction(WebConstants.VIEW_ERRORLOG_PAGE,
                WebConstants.HOME_CONTROLLER,
                new { AppId = AppId });
        }

        // logoff  ,  user-role
        public ActionResult Logoff()
        {
            Session.Clear();
            return RedirectToAction(WebConstants.HOME_PAGE, WebConstants.HOME_CONTROLLER);
        }






        // admin role
        public ActionResult EditApp(int appId )
        {
            var appVM = Loader.queryAppByAppId(appId);
            var appPidFVM = new ApplicationPersonIdFlatVM()
            {
                ApplicationId = appVM.ApplicationId,
                AppName = appVM.AppName,
                IsActive = appVM.IsActive,
                PersonId = 0
            };
            return View(appPidFVM);
        }
        // update app
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditApp(ApplicationPersonIdFlatVM instance)
        {
            if( ModelState.IsValid )
            {
                Loader.updateAppForActiveAndOwnership(instance);
                return RedirectToAction(WebConstants.VIEW_ALLAPPS_PAGE, WebConstants.HOME_CONTROLLER);
            }
            return View(instance);
        }





        /// <summary>
        /// Error Handling
        /// </summary>
        /// <returns></returns>
        public ActionResult Error( string err )
        {
            ViewBag.err = err;
            return View();
        }
    }
}