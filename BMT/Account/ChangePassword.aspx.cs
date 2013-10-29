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
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region VARIABLES

        private UserBO _userBO = new UserBO();
        private Security _security = new Security();

        private string oldPassword;
        private int userId;

        #endregion

        #region EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    int enterpriseId = Util.GetEnterpriseId();
                    if (enterpriseId != 0)
                    {
                        Session[enSessionKey.EnterpriseId.ToString()] = enterpriseId;
                        
                        //Display Enterprise Info
                        Page.Title = Util.GetPageTitle(enterpriseId) + " | Change Password";
                        lblVer.Text = Util.GetSystemVersion(enterpriseId);
                        lblsysteminfo.Text = Util.GetSystemCopyright(enterpriseId);
                        lbLogin.Style.Add("display", "none");

                        string email = Util.GetSystemMailTo(enterpriseId);
                        mailToSupport.Text = email;
                        mailToSupport.NavigateUrl = "mail" + "to:" + email;
                    }
                    else
                    {
                        Message.Error("Enterprise Name is incorrect.");
                    }
                }

                if (Request.QueryString["email"] != null)
                {
                    string emailAddress = Request.QueryString["email"];

                    if (Request.QueryString["TempU"] != null)
                    {
                        userId = Convert.ToInt32(Request.QueryString["TempU"].ToString());

                        // Fetching and Display Account details
                        List<string> _userDetails = _userBO.GetAccountDetailByUserId(userId);

                        lblName.Text = _userDetails[0];
                        lblUsername.Text = _userDetails[1];

                        // Decrypt Orignal password
                        oldPassword = _security.Decrypt(_userDetails[2]);
                    }
                    else
                    {
                        Message.Error("User Id is incorrect.");
                    }
                }
                else
                {
                    Message.Error("Email Address is incorrect.");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Password cannot be change at this time.");
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                // Check of old password is correct?
                if (txtCurrentPassword.Text != oldPassword)
                {
                    Message.Error("Current password is incorrect.");
                    return;
                }
                else
                {
                    string password = txtNewPassword.Text;
                    password = _security.Encrypt(password);
                    _userBO.changePassword(userId, password);

                    lbLogin.Style.Add("display", "visible");
                    Message.Success("Password changed successfully.");
                    oldPassword = txtNewPassword.Text;
                    return;
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Password couldn't be change.");
            }
        }

        #endregion
    }
}