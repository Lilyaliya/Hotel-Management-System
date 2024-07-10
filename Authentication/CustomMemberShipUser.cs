using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Hotel.Models;

namespace Hotel.Authentication
{
    public class CustomMemberShipUser: MembershipUser
    {
        #region User Properties
        public int ID { get; set; }
        public string FullName { get; set; }
        public override string Email { get; set; }
        public string Login { get; set; }
        public int? VerifiedBy { get; set; }
        public string Role { get; set; }

        #endregion

        public CustomMemberShipUser(Actor user) : base("CustomMembership", user.Login, user.ID, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            ID = user.ID;
            FullName = user.FullName;
            Email = user.Email;
            Login = user.Login;
            Role = user.Role;
            VerifiedBy = user.VerifiedBy;
        }
    }
}