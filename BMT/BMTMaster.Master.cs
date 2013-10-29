using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT
{
    public partial class BMTMaster : System.Web.UI.MasterPage
    {
        #region VARIABLES
        private SessionHandling _sessionHandling;
        private PageBo _pageBO;

        private string userType;
        private string userEmail
        {
            get
            {
                if (Session["UserEmail"] == null)
                    return string.Empty;
                return (string)Session["UserEmail"];
            }
            set
            {
                Session["UserEmail"] = value;
            }
        }

        private List<BMTBLL.Page> _listOfPages;

        #endregion

        #region EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);

                if (!IsPostBack)
                {
                    lblWelcomeUser.Text += Session["UserName"].ToString();
                    Page.Title = Util.GetPageTitle(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
                    SetBannerLink();
                }

                _pageBO = new PageBo();
                userType = Session["userType"] != null ? Session["userType"].ToString() : string.Empty;
               
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    // Enable all Menu when SuperUser logged in
                    pnlAdminMenu.Visible = pnlUserMenu.Visible = lblWelcomeSuperUser.Visible = lbChangePassword.Visible =
                        pnlNestedMenu.Visible = true;
                    
                    pnlWelcomeUser.Visible = false;

                    // SuperUser welcome message
                    lblWelcomeSuperUser.Text = "Welcome, " + Session["UserName"].ToString();

                    // Get the list of allowed pages
                    _listOfPages = _pageBO.GetListOfPages(enUserRole.SuperUser, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    // Enable only consultant Menu                        
                    pnlAdminMenu.Visible = lblWelcomeSuperUser.Visible = false;

                    hideReport.Visible = false;
                    // Enable Default Welcome user label
                    pnlWelcomeUser.Visible = pnlUserMenu.Visible = lbChangePassword.Visible = pnlNestedMenu.Visible = true;

                    // Get the list of allowed pages
                    _listOfPages = _pageBO.GetListOfPages(enUserRole.Consultant, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
                }
                else
                {
                    // Hide Admin Menu 
                    pnlAdminMenu.Visible = lblWelcomeSuperUser.Visible = lbChangePassword.Visible = pnlNestedMenu.Visible = false;

                    // Enable Default Welcome user label
                    pnlUserMenu.Visible = pnlWelcomeUser.Visible = true;

                   // Get the list of allowed pages
                    _listOfPages = _pageBO.GetListOfPages(enUserRole.User, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
                }

                #region SECURITY

                string pageName = Request.Url.AbsolutePath;
                pageName = pageName.Substring(pageName.LastIndexOf('/') + 1);

                var _currentPageList = (from pageRecord in _listOfPages
                                        where pageRecord.Name == pageName
                                        select pageRecord);
                if (_currentPageList.Count() == 0)
                {
                    Response.Redirect("~/Webforms/Home.aspx", false);
                }

                #endregion
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);
            }

        }


        public void lbChangepassword_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Account/ChangePassword.aspx?email=" + userEmail + "&TempU=" + Session[enSessionKey.UserApplicationId.ToString()].ToString(), false);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }


        public void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["userType"] != null)
                {
                    _sessionHandling = new SessionHandling();
                    _sessionHandling.ClearSession();
                }
                Response.Redirect("~/Account/Login.aspx", false);

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }


        protected void btnResetSession_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        #endregion

        #region FUNCTIONS

        public void SetBannerLink()
        {
            SystemInfoBO systemInfo = new SystemInfoBO();
            hdnAdRotatorList.Value = systemInfo.GetSystemAdRotatorList(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            string topAdRotator = hdnAdRotatorList.Value.ToString().Split(';')[0];
            footerAd.NavigateUrl = topAdRotator.Split('|')[1];
            footerImg.ImageUrl = topAdRotator.Split('|')[0];
            hdnCurrentAdIndex.Value = "0";
        }

        #endregion

    }
}