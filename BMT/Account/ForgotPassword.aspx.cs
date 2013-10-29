using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.Account
{
    public partial class ForgotPassword : System.Web.UI.Page
    {

        #region VARIABLES
        private UserAccountBO _userAccountBO = new UserAccountBO();
        private Email _email = new Email();
        private Security _security = new Security();

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Message.Clear("");
                    int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);

                    if (enterpriseId != 0)
                    {
                        //Display Enterprise Info
                        Page.Title = Util.GetPageTitle(enterpriseId) + " | Forgot Password";
                        lblVer.Text = Util.GetSystemVersion(enterpriseId);
                        lblsysteminfo.Text = Util.GetSystemCopyright(enterpriseId);
                    }
                    else
                    {
                        Message.Error("Enterprise Name is incorrect.");
                    }                    
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                string emailAddress = txtEmail.Text.Trim().ToLower().ToString();

                // Get login details
                GetUserLoginInfo(emailAddress, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("The Server has encounterd an error, Please try again later.");
            }
        }
        #endregion

        #region FUNCTIONS
        protected void GetUserLoginInfo(string emailAddress, int enterpriseId)
        {
            try
            {
                _userAccountBO = new UserAccountBO();
                _security = new Security();
                List<string> _credentialDetails = _userAccountBO.GetUserLoginInfo(emailAddress, enterpriseId);

                string userName = string.Empty;
                string password = string.Empty;
                string projectName = string.Empty;
                string companyName = string.Empty;
                string body = string.Empty;
                string userId = string.Empty;

                // If an Account found against the entered email address
                if (_credentialDetails.Count > 0)
                {
                    userName = _credentialDetails[0];
                    password = _credentialDetails[1];
                    userId = _credentialDetails[2];

                    projectName = Util.GetSystemProjectName(enterpriseId);
                    companyName = Util.GetSystemCompany(enterpriseId);
                    body = Util.GetSystemBody(enterpriseId);

                    // Decrypt the password
                    password = _security.Decrypt(password);
                    _email.SendUserCredentialDetails(emailAddress, userName, password, projectName, companyName, body, Convert.ToInt32(userId));

                    Message.Success("Account details has been sent.");
                }
                else
                    Message.Error("Email couldn't be generated. Contact your site Administrator.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

    }
}