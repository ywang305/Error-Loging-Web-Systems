using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Security
{
    public class PageAuthentication
    {
        public bool IsUserAuthorized(string controller, string action, RoleEnum userRole)
        {
            //To Do: get use's role based on userName, from DB,  (done in BasecController)

            // get required role from the Matrix (this will fail if we haven't registered the requested controller/action combination
            RoleEnum requiredRole = SecurityMatrix.Matrix.First(x => x.Controller == controller && x.Action == action).MinimumRoleNeeded;
            return userRole >= requiredRole;
        }
    }
}
