#region Modification History

//  ******************************************************************************
//  Module        : Email
//  Created By    : NA
//  When Created  : NA
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01-31-2012      Remove Email duplication code and use emailSend function from email class
//  Mirza Fahad Ali Baig    01-31-2012      Add regions for functions, variables etc...
//  Mirza Fahad Ali Baig    01-31-2012      Change public security Modifier from Functions with protected Modifier
//  Mirza Fahad Ali Baig    01-31-2012      Add logging
//  Mirza Fahad Ali Baig    03-07-2012      Optimize the sending email code and name handling for consultant user
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Configuration;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class InviteFriend : System.Web.UI.Page
    {
        #region VARIABLES
        private Email email = new Email();
        private UserBO useraccount = new UserBO();

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblname.Text += Session["UserName"].ToString();
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
                Invitefriend();
                MsgInviteFriend.Success("Invitation sent successfully.");
                ClearFields();
            }
            catch (Exception exception) { string ExceptionMessage = exception.StackTrace; }
        }

        #endregion

        #region FUNCTIONS
        protected void Invitefriend()
        {
            try
            {
                int userId = Session["UserApplicationId"] != null ? Convert.ToInt32(Session["UserApplicationId"]) : 0;
                string friendName = txtName.Text.Trim();
                string friendEmail = txtemail.Text.Trim();
                string message = txtDesc.Text.Trim();

                string senderName = string.Empty;
                string senderEmail = string.Empty;
                senderEmail = Session["UserEmail"] != null ? Session["UserEmail"].ToString() : string.Empty;

                // get login user details
                useraccount.GetUserByUserId(userId);

                // if Log-in user has no personal details (e.g: consulant user)
                senderName = useraccount.FistName != null ? useraccount.FistName + " " + useraccount.LastName : senderEmail;

                // get credential of current user e.g: MD etc..
                string credentialname = useraccount.GetUserCredentialByUserId(userId);

                string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;

                // get complete domain address
                String host = Util.GetHostPath();

                // seperator
                string comma = ", ";

                if (credentialname == null || credentialname == string.Empty)
                    comma = " ";

                if (message.Trim() != string.Empty)
                    message = "Here is a brief message from " + senderName + ":<br/> " + message + "<br/><br/>";
                else
                    message = string.Empty;


                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                string projectName = Util.GetSystemProjectName(enterpriseId);
                string inviteFriendMail = Util.GetSystemInviteFriendMail(enterpriseId);
                string companyName = Util.GetSystemCompany(enterpriseId);


                inviteFriendMail = inviteFriendMail.Replace("[friendName]", friendName);
                inviteFriendMail = inviteFriendMail.Replace("[senderName]", senderName);
                inviteFriendMail = inviteFriendMail.Replace("[comma]", comma);
                inviteFriendMail = inviteFriendMail.Replace("[projectName]", projectName);
                inviteFriendMail = inviteFriendMail.Replace("[companyName]", companyName);
                inviteFriendMail = inviteFriendMail.Replace("[message]", message);
                inviteFriendMail = inviteFriendMail.Replace("[credentialname]", credentialname);
                inviteFriendMail = inviteFriendMail.Replace("[url]", host);

                string subject = "Invitation from " + senderName;

                string desclaimers = "<br /><br /><br /><p style='font-size:12.5px; color:Gray;'>This transmission contains confidential information belonging to the sender that is legally privileged and proprietary and may be subject to protection under the law, including the Health Insurance Portability and Accountability Act (HIPAA). If you are not the intended recipient of this e-mail, you are prohibited from sharing, copying, or otherwise using or disclosing its contents. If you have received this e-mail in error, please notify the sender immediately by reply e-mail and permanently delete this e-mail and any attachments without reading, forwarding or saving them. Thank you.</p>";
                inviteFriendMail = inviteFriendMail + desclaimers;

                email.SendEmail(friendEmail, subject, inviteFriendMail);

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void ClearFields()
        {
            try
            {
                txtName.Text = txtemail.Text = txtDesc.Text = string.Empty;
            }
            catch (Exception exception)
            {
                string ExceptionMessage = exception.StackTrace;
            }
        }

        #endregion
    }
}