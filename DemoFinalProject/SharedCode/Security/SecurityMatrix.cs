/*
    Dusan Palider
    CSE 686
    Spring 2016

    Please consult global.asax for information about this code
*/

using System.Collections.Generic;

namespace SharedCode.Security
{
    public class SecurityMatrix
    {
        /// <summary>
        /// Security Matrix, containing all of the controller/actions and the needed roles
        /// </summary>
        public static ICollection<SecurityAccess> Matrix { get; set; }

        /// <summary>
        /// Initializes the security matrix
        /// </summary>
        public static void Initialize()
        {
            Matrix = new List<SecurityAccess>()
            {
                #region Home Controller

                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.HOME_PAGE,
                    MinimumRoleNeeded = RoleEnum.None
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.LOG_OFF,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.ERROR_PAGE,
                    MinimumRoleNeeded = RoleEnum.None
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.HOME_USER_PAGE,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.HOME_ADMIN_PAGE,
                    MinimumRoleNeeded = RoleEnum.Admin
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.REGISTER_PAGE,
                    MinimumRoleNeeded = RoleEnum.None
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.ADD_USER_PAGE,
                    MinimumRoleNeeded = RoleEnum.Admin
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.EDIT_USER_PAGE,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.VIEW_ERRORLOG_PAGE,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.ADD_APP_PAGE,
                    MinimumRoleNeeded = RoleEnum.Admin
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.VIEW_APPS_PAGE,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.VIEW_ALLAPPS_PAGE,
                    MinimumRoleNeeded = RoleEnum.Admin
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.CLEAR_LOGS,
                    MinimumRoleNeeded = RoleEnum.User
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.EDIT_APP_PAGE,
                    MinimumRoleNeeded = RoleEnum.Admin
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.SEARCH_LOGS,
                    MinimumRoleNeeded = RoleEnum.User
                }

                #endregion
            };
        }
    }
}
