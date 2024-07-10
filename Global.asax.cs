using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using Hotel.ViewModel;
using Hotel.Authentication;
using System.Globalization;

namespace Hotel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CultureInfo culture = new CultureInfo("en-US");
                //(CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            culture.DateTimeFormat.LongTimePattern = "";


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);

                principal.ID = serializeModel.ID;
                principal.FullName = serializeModel.FullName;
                principal.Login = serializeModel.Login;
                principal.Roles = serializeModel.Role.ToArray<string>();

                HttpContext.Current.User = principal;
            }

        }
    }
}
