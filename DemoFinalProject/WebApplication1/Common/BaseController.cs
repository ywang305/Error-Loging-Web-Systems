using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharedCode;
using SharedCode.Security;

namespace WebApplication1.Controllers 
{
    /// <summary>
    /// Base controller, should be inherited from for every controller
    /// </summary>
    public class BaseController : Controller
    {
        

        #region Properties & Controller

        #region String constants

        protected static string DETAILED_UNAUTHORIZED_ERROR = "User {0} attempted to access {1}/{2}.";
        protected static string USER_UNAUTHORIZED_ERROR = "You are not authorized for the specified page.";
        protected static string DETAILED_FORGERY_LINK_ERROR = "User {0} attempted to forgery link parameter {1} with {2} in Action {3}.";
        #endregion

        /// <summary>
        /// Authentication class
        /// </summary>
        private PageAuthentication authentication { get; set; }
        
        public int role = 0;


        public BaseController()
        {
            this.authentication = new PageAuthentication();
        }
        #endregion

        #region Overrides of events
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ToDo: setup a stop watch..

            // gather data
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;
            string sessionID = (string)HttpContext.Session["hash"];

            var loginPerson = LoadersAndLogic.Loader.queryLoginfoPersonBySession(sessionID);
            role = loginPerson.Role;


            ///  --- <  Forgery link  is not allowed > ---
            ///
            if( role < (int)RoleEnum.Admin )
            {

                // --- < EditUser/ViewApp parameter ( PersonId ) > ---

                if (action == WebConstants.EDIT_USER_PAGE || action == WebConstants.VIEW_APPS_PAGE)
                {
                    //  uer IsLive? if not, user cannot access app page

                    if (loginPerson.IsLive == false)
                    {
                        filterContext.Result = RedirectToAction(WebConstants.HOME_USER_PAGE);
                        return;
                    }

                    // user forgery link cause exception

                    var paramPersonId = filterContext.ActionParameters.First().Value;
                    if ((int)paramPersonId != loginPerson.OnePerson.PersonId)
                    {
                        //TempDate can be used for redirection c->s
                        string errorDetails = string.Format(DETAILED_FORGERY_LINK_ERROR,
                            loginPerson.OnePerson.FirstName, loginPerson.OnePerson.PersonId, paramPersonId, action);
                        TempData["errorDetails"] = errorDetails;
                        throw new UnauthorizedAccessException(errorDetails);
                    }
                }

                // --- < ViewLog parameter ( AppId ) > ---

                if ( action==WebConstants.VIEW_ERRORLOG_PAGE )
                {
                    var paramObj = filterContext.ActionParameters.First().Value as SharedCode.LogOrderSearchVM;
                    var queryApps = LoadersAndLogic.Loader.queryAllAppsByPersonId(loginPerson.OnePerson.PersonId);

                    if (  false == queryApps.Any( app=> app.ApplicationId == (int)paramObj.AppId))
                    {
                        //TempDate can be used for redirection c->s
                        string errorDetails = string.Format(DETAILED_FORGERY_LINK_ERROR,
                            loginPerson.OnePerson.FirstName, loginPerson.OnePerson.PersonId, paramObj.AppId, action);
                        TempData["errorDetails"] = errorDetails;
                        throw new UnauthorizedAccessException(errorDetails);
                    }
                }
            }  
            





            //authenticate user
            if (!this.authentication.IsUserAuthorized(controller, action, (RoleEnum)role))
            {
                //TempDate can be used for redirection c->s
                TempData["errorMessage"] = USER_UNAUTHORIZED_ERROR;
                string errorDetails = string.Format(DETAILED_UNAUTHORIZED_ERROR, loginPerson.OnePerson.FirstName, controller, action);
                TempData["errorDetails"] = errorDetails;
                throw new UnauthorizedAccessException(errorDetails);
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //ToDo: log the time it took to load for performance


            //Optional -> do other checks
            base.OnActionExecuted(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Blow out the session
            Session.Clear();

            // Log the error, default the .net web appid = 1
            string errorMessage = filterContext.Exception.Message;
            int logLevel = new Random().Next(1, 5);
            LoadersAndLogic.Loader.insertOneLog(new ApplicationVM() { ApplicationId = 1,
                Errors = { new ErrorVM() { ErrorMessage=errorMessage, Time=DateTime.Now, LogLevel= logLevel, ExMessage=filterContext.Exception.ToString() } } });

            //Redirect user to the error page    
            filterContext.ExceptionHandled = true;


            bool debug = false;
            if (debug == true || role>=(int)RoleEnum.Admin )
            {
                filterContext.Result = RedirectToAction(SharedCode.WebConstants.ERROR_PAGE, SharedCode.WebConstants.HOME_CONTROLLER, new { err = errorMessage });
            }
            else
            {
                filterContext.Result = RedirectToAction(SharedCode.WebConstants.ERROR_PAGE, SharedCode.WebConstants.HOME_CONTROLLER);
            }
            base.OnException(filterContext);
        }

        #endregion
    }
}