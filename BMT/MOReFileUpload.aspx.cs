using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Globalization;

using BMT.WEB;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class MOReFileUpload : System.Web.UI.Page
    {

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["elementId"] != null && Request.QueryString["factorId"] != null
                    && Request.QueryString["PCMHId"] != null && Request.QueryString["ProjectUsageId"] != null
                    && Request.QueryString["PracticeName"] != null && Request.QueryString["SiteName"] != null
                    && Request.QueryString["Node"] != null && Request.QueryString["PracticeId"] != null
                    && Request.QueryString["SiteId"] != null && Request.QueryString["templateId"] != null)
                {
                    hiddenPracticeId.Value = Request.QueryString["PracticeId"];
                    hiddenPracticeName.Value = System.Web.HttpUtility.JavaScriptStringEncode(Request.QueryString["PracticeName"]);
                    hiddenSiteName.Value = Request.QueryString["SiteName"].Trim() == string.Empty ? "BMT" : System.Web.HttpUtility.JavaScriptStringEncode(Request.QueryString["SiteName"]);
                    hiddenSiteId.Value = Request.QueryString["SiteId"];
                    hiddenNode.Value = Request.QueryString["Node"].Trim() == string.Empty ? "NCQA Submission" : Request.QueryString["Node"];
                    hiddenProjectUsageId.Value = Request.QueryString["ProjectUsageId"];
                    hiddenElementId.Value = Request.QueryString["elementId"];
                    hiddenFactorId.Value = Request.QueryString["factorId"];
                    hiddenPCMHId.Value = Request.QueryString["PCMHId"];
                    hiddenTemplateId.Value = Request.QueryString["templateId"];

                    if (!Page.IsPostBack)
                        rbDocSelecttion.SelectedIndex = 0;

                    pnpNCQALink.Style.Add("display", "block");
                }
                else if (Request.QueryString["projectUsageId"] != null &&
                    Request.QueryString["practiceName"] != null &&
                    Request.QueryString["siteName"] != null
                    && Request.QueryString["siteId"] != null
                    && Request.QueryString["PracticeId"] != null
                    && Request.QueryString["templateId"] != null)
                {

                    hiddenProjectUsageId.Value = Request.QueryString["projectUsageId"];
                    hiddenPracticeId.Value = Request.QueryString["PracticeId"];
                    hiddenPracticeName.Value = System.Web.HttpUtility.JavaScriptStringEncode(Request.QueryString["practiceName"]);
                    hiddenSiteName.Value = Request.QueryString["siteName"].Trim() == string.Empty ? "BMT" : System.Web.HttpUtility.JavaScriptStringEncode(Request.QueryString["SiteName"]);
                    hiddenSiteId.Value = Request.QueryString["siteId"];
                    hiddenTemplateId.Value = Request.QueryString["templateId"];
                    hiddenElementId.Value = "0";
                    hiddenFactorId.Value = "0";
                    hiddenPCMHId.Value = "0";

                    ddldocType.Items.Clear();
                    ddldocType.Items.Add(new ListItem("Site Document", "6"));

                    pnpNCQALink.Style.Add("display", "none");
                    pnlExistingDoc.Style.Add("display", "none");
                }

                if (Request.QueryString["DocName"] != null || Request.QueryString["ReferencePage"] != null || Request.QueryString["RelevancyLevel"] != null ||
                    Request.QueryString["File"] != null || Request.QueryString["DocLinkedTo"] != null || Request.QueryString["DocType"] != null)
                {
                    Session["pcmhId"] = Request.QueryString["PCMHId"];
                    Session["elementId"] = Request.QueryString["elementId"];

                    pnlUploadNewFile.Style.Add("display", "none");
                    rbDocSelecttion.Style.Add("display", "none");
                    imgUploadDoc.Attributes.Add("onclick", "javascript:upload_Click('" + Request.QueryString["DocName"] + "','" + Request.QueryString["ReferencePage"] + "','" +
                        Request.QueryString["RelevancyLevel"] + "','" + Request.QueryString["File"] + "','" + Request.QueryString["DocLinkedTo"] + "','" + Request.QueryString["DocType"] + "','" + Request.QueryString["templateId"] + "');");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        #endregion
    }
}