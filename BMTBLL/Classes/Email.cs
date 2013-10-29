#region Modification History

//  ******************************************************************************
//  Module        : Email
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/09/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    12-20-2011      Email Body with Importance
//  Mirza Fahad Ali Baig    31-12-2012      Create SendEmail function to send an email (Now it can be use multiple times from different places)
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class Email
    {
        #region VARIABLE
        private Security _security = new Security();
        #endregion


        #region CONSTRUCTOR
        public Email()
        { }

        #endregion

        #region FUNCTIONS
        public void SendUserCredentialDetails(string emailAddress, string userName, string password, string projectName, string companyName, string body)
        {

            try
            {
                // TODO: Get complete domain name with virtual directory (if exists).
                string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
                if (virtualDirectory.Length > 1)
                {
                    virtualDirectory += "/";
                }

                Uri uri = HttpContext.Current.Request.Url;
                String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + virtualDirectory;


                string subject = projectName + " Account Information";


                body = body.Replace("[emailAddress]", emailAddress);
                body = body.Replace("[username]", userName);
                body = body.Replace("[password]", password);
                body = body.Replace("[projectName]", projectName);
                body = body.Replace("[companyName]", companyName);
                body = body.Replace("[url]", host);

                SendEmail(emailAddress, subject, body);

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public void SendUserCredentialDetails(string emailAddress, string userName, string password, string projectName, string companyName, string body, int userId)
        {

            try
            {
                // TODO: Get complete domain name with virtual directory (if exists).
                string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
                if (virtualDirectory.Length > 1)
                {
                    virtualDirectory += "/";
                }

                Uri uri = HttpContext.Current.Request.Url;
                String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + virtualDirectory;


                string subject = projectName + " Account Information";


                body = body.Replace("[emailAddress]", emailAddress);
                body = body.Replace("[username]", userName);
                body = body.Replace("[password]", password);
                body = body.Replace("[projectName]", projectName);
                body = body.Replace("[companyName]", companyName);
                body = body.Replace("[url]", host);
                body = body.Replace("[userId]", userId.ToString());

                SendEmail(emailAddress, subject, body);

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public void SendEmail(string toAddress, string subject, string body)
        {

            try
            {
                // Get smtp settings from web.config
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                // Create email
                string emailFromTitle = System.Configuration.ConfigurationManager.AppSettings["EmailFromTitle"].ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(settings.Smtp.From, emailFromTitle, System.Text.Encoding.UTF8);

                message.To.Add(toAddress);
                message.Subject = subject;

                message.Body = body;

                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(settings.Smtp.Network.Host);
                client.Port = settings.Smtp.Network.Port;

                //client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);

                // Send Email 
                client.Send(message);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SendEmail(string toAddress, string subject, string body, string applicationPath)
        {

            try
            {
                // Get smtp settings from web.config
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(applicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                // Create email
                string emailFromTitle = System.Configuration.ConfigurationManager.AppSettings["EmailFromTitle"].ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(settings.Smtp.From, emailFromTitle, System.Text.Encoding.UTF8);

                message.To.Add(toAddress);
                message.Subject = subject;

                message.Body = body;

                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(settings.Smtp.Network.Host);
                client.Port = settings.Smtp.Network.Port;

                //client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);

                // Send Email 
                client.Send(message);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SendNCQARequest(int _projectId, string _licenseNumber, string _userName, string _password, int enterpriseId)
        {

            try
            {
                SystemInfoBO systemInfoBO = new SystemInfoBO();
                string emailAddress = systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.mailTo);
                string subject = "NCQA Submission Request";
                string body = systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.NCQASubmissionMail);

                ProjectBO projectBO = new ProjectBO();
                string siteName = projectBO.GetSiteNameByProjectID(_projectId);

                body = body.Replace("[SiteName]", siteName);
                body = body.Replace("[ISSLicenseNumber]", _licenseNumber);
                body = body.Replace("[UserName]", _userName);
                body = body.Replace("[Password]", _password);
                body = body.Replace("[RequestedOn]", DateTime.Now.ToString("MM-dd-yyyy"));

                SendEmail(emailAddress, subject, body);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public void SendNCQASuccessful(int _projectId, string _licenseNumber, int enterpriseId, string applicationPath)
        {
            try
            {
                SystemInfoBO systemInfoBO = new SystemInfoBO();
                string emailAddress = systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.mailTo);
                string subject = "NCQA Submission Successful";
                string body = systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.NCQASubmissionSuccessfulMail);

                ProjectBO projectBO = new ProjectBO();
                string siteName = projectBO.GetSiteNameByProjectID(_projectId);

                body = body.Replace("[SiteName]", siteName);
                body = body.Replace("[ISSLicenseNumber]", _licenseNumber);                
                body = body.Replace("[CompletedOn]", DateTime.Now.ToString("MM-dd-yyyy"));

                SendEmail(emailAddress, subject, body, applicationPath);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion




    }
}
