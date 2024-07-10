using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Hotel.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties

        public int ID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        #endregion
        public IIdentity Identity {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
                return true;
            return false;
        }
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}