using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace XYZShoppingMVCApp_DuyTang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            // decrypt the received authentication cookie, extract roles from it
            // attach the roles to the user principle
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (authCookie == null)
                return; // no authentication cookie found
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                return; // we can log exception later
            }
            if (authTicket == null)
                return; // cookie failed to decrypt
                        // valid cookie ---, extract the pipe delimited set of roles
            string roles = authTicket.UserData;
            string[] rolesArray = roles.Split(new char[] { '|' });
            
            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(User.Identity, rolesArray);
        }
    }
}
