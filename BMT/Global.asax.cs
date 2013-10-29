using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using BMT.WEB;
namespace BMT
{
    public class Global : System.Web.HttpApplication
    {
        #region VARIABLES

        #endregion

        #region EVENTS
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
           
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // TODO: Always use Secure connection {Enabled:SSL}            
            //if (!Request.IsSecureConnection)
            //{
            //    string path = string.Format("https{0}", Request.Url.AbsoluteUri.Substring(4));

            //    Response.Redirect(path);
            //}

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

            // get last error object
            Exception exception = HttpContext.Current.Server.GetLastError().GetBaseException();

            // log the error.        
            Logger.PrintError(exception);

            if (exception.Message.Contains("Timeout expired."))
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string location = url.Split('/')[3];
                if (location == "Webforms")
                    Response.Redirect("~/Account/Login.aspx?LastRequest=" + location);
                else
                    Response.Redirect("/" + location + "/Account/Login.aspx?LastRequest=" + url);
            }
            // commit this line to include the customer Error
            //HttpContext.Current.Server.ClearError(); // clear the last error from server. 

        }

        protected void Session_End(object sender, EventArgs e)
        {
           
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        #endregion
    }
}