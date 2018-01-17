using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Security
{
    public class SecurityAccess
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public RoleEnum MinimumRoleNeeded { get; set; }
    }
}
