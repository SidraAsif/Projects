using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BMTBLL;
using BMTBLL.Enumeration;
using Microsoft;
using System.Text;
using System.Collections;

using BMT.WEB;

namespace BMT.Webforms
{
    public partial class Home : System.Web.UI.Page
    {

        #region VARIABLES
        PageContentBO PageContent = new PageContentBO();
        private string userType;        

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                Message.Clear("");

                // If new user signup
                IsNew();

                // Get userType
                userType = Session["userType"] != null ? Session["userType"].ToString() : string.Empty;
                Content1.InnerHtml = GetHTML(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        #endregion

        #region FUNCTIONS
        protected void IsNew()
        {
            try
            {
                if (Request.QueryString["registered"] != null)
                {
                    Message.Success("You have been registered successfully. Your login details has been sent to your email address.");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        private string GetHTML(int enterpriseId)
        {
            try
            {
                PageBo Page = new PageBo();

                int pageId = Page.GetPageId("Home.aspx", enUserRole.SuperUser, enterpriseId);

                object content = PageContent.GetPageContent(pageId);
                if (content != null)
                    return Convert.ToString(content);
                else
                    return string.Empty;


            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        #endregion
    }


}

