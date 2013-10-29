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

using BMTBLL;
using BMT.WEB;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class Submit_Feedback : System.Web.UI.Page
    {

        private string _UserName;
        private int PracticeId;
        private int userApplicationId
        {
            get
            {
                if (Session["UserApplicationId"] == null)
                    return 0;
                return (int)Session["UserApplicationId"];
            }
            set
            {
                Session["UserApplicationId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserApplicationId"] == null || Session["UserType"] == null || Session["PracticeId"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut");
                }


                _UserName = Session["UserName"].ToString();
                PracticeId = Convert.ToInt32(Session["PracticeId"]);
                lblname.Text = _UserName;

                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                string projectName = Util.GetSystemProjectName(enterpriseId);
                lblDescription.Text = "Thank you for contacting the " + projectName + " team.";


            }
            catch (Exception exception)
            {
                string ExceptionMessage = exception.StackTrace;
            }
        }
        public void SendFeedback(string UserName)
        {
            try
            {
                string Subject = txtSubject.Text;
                string Description = txtDesc.Text;

                PracticeBO PracBO = new PracticeBO();
                UserBO _UserBo = new UserBO();
                UserAccountBO UserAccount = new UserAccountBO();
                List<string> _UserInfo = UserAccount.GetUserInformation(userApplicationId);

                string UserId = string.Empty;
                string UserEmai = string.Empty;
                string FirstName = string.Empty;
                string LastName = string.Empty;
                string userPracticeName = string.Empty;

                if (_UserInfo.Count > 0)
                {
                    UserId = _UserInfo[0];
                    UserEmai = _UserInfo[1];
                    FirstName = _UserInfo[2];
                    LastName = _UserInfo[3];
                    userPracticeName = _UserInfo[4];
                }

                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                string EmailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailToAddress"].ToString();
                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(settings.Smtp.From);
                message.To.Add(EmailAddress);
                message.Subject = Util.GetSystemProjectName(enterpriseId) + "-Feedback";

                message.Body = "User Name:  " + FirstName + "" + LastName + "<br />" + "Practice Name:  " + userPracticeName + "<br/>" + "User Email: " + UserEmai + "<br/>" + "Subject:  " + Subject + "<br />" + "Description:  " + Description + "<br />" + "UserID:  " + lblname.Text;

                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(settings.Smtp.Network.Host);
                client.Port = settings.Smtp.Network.Port;
                client.Credentials = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
                client.Send(message);

                FeedbackMesssage.Success("Thank you. Your Feedback has been submitted.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }


        }
        public void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                SendFeedback(_UserName);
                ClearFields();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }
        public void ClearFields()
        {

            txtSubject.Text = txtDesc.Text = string.Empty;
        }

    }
}