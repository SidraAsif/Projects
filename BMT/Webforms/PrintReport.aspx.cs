using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;

namespace BMT.Webforms
{
    public partial class PrintReportPage : System.Web.UI.Page
    {
        #region VARIABLES

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void hdnButton_Click(object sender, EventArgs e)
        {
            try
            {
                int attemptCount = 0;
                

                while (attemptCount < 600)// 10 Minutes
                {
                    if (Session["FilePath"] != null && Session["FilePath"].ToString() != String.Empty)
                    {
                        string path = Session["FilePath"].ToString();

                        Random random = new Random();
                        int randomNumber = random.Next(0, 100000);

                        path = path + "?" + randomNumber.ToString();
                        Logger.PrintInfo("Receiving report path : {0}", path);

                        Session["FilePath"] = String.Empty;
                        Response.Redirect(path, false);

                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        attemptCount++;
                    }
                }

                Logger.PrintInfo("TimeOut: Report could not be generated.");
                lblMessage.Visible = true;
                divProgress.Style.Add("display", "none");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                //Response.Redirect("~/Webforms/PrintReport.aspx", false);
            }

        }

        #endregion
    }
}