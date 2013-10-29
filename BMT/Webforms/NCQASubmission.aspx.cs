using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Threading;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;
using BMTBLL.Helper;

using log4net;
using log4net.Config;

namespace BMT.Webforms
{
    public partial class NCQASubmission : System.Web.UI.Page
    {
        #region VARIABLES

        private int userApplicationId;
        private string userType;
        private int enterpriseId;
        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserApplicationId"] != null || Session["UserType"] != null)
                {
                    userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                    userType = Session["UserType"].ToString();

                    if (!Page.IsPostBack)
                    {
                        if (userType == enUserRole.SuperAdmin.ToString())
                        {
                            LoadingProcess();
                            pnlNCQASubmission.Visible = false;
                        }
                        else
                        {
                            BindNCQASubmissionRequests();
                        }
                    }
                }
                else
                {
                    SessionHandling _sessionHandling = new SessionHandling();
                    _sessionHandling.ClearSession();
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void ddlEnterprise_OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                    {
                        pnlNCQASubmission.Visible = true;
                        BindNCQASubmissionRequests();
                    }
                    else
                    {
                        pnlNCQASubmission.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdNCQASubmission_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdNCQASubmission.EditIndex = e.NewEditIndex;
                BindNCQASubmissionRequests();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdNCQASubmission_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdNCQASubmission.EditIndex = -1;
                BindNCQASubmissionRequests();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdNCQASubmission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && grdNCQASubmission.EditIndex == e.Row.RowIndex)
                {
                    NCQASubmissionBO ncqaSubmission = new NCQASubmissionBO();

                    DropDownList ddlNCQAStatus = (DropDownList)e.Row.FindControl("ddlNCQAStatus");
                    ddlNCQAStatus.DataValueField = "SubmissionStatusId";
                    ddlNCQAStatus.DataTextField = "Name";
                    ddlNCQAStatus.DataSource = ncqaSubmission.GetAllSubmissionStatus();
                    ddlNCQAStatus.DataBind();

                    string selectedValue = ((Label)e.Row.FindControl("lblStatusText")).Text;
                    ddlNCQAStatus.Items.FindByText(selectedValue).Selected = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSubmissionType = (Label)e.Row.FindControl("lblSubmissionType");

                    HyperLink hypPassword = (HyperLink)e.Row.FindControl("hypPassword");
                    hypPassword.Text = "********";

                    string projectUsageId = grdNCQASubmission.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string practiceSiteId = grdNCQASubmission.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    DateTime requestedOn = Convert.ToDateTime(grdNCQASubmission.DataKeys[e.Row.RowIndex].Values[2].ToString());

                    hypPassword.NavigateUrl = "javascript:showPassword(" + projectUsageId + "," + practiceSiteId + ",'" + requestedOn + "'," + lblSubmissionType.Text + ");";
                    hypPassword.CssClass = "hide-Underline";

                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                    if (lblStatus != null)
                    {
                        if (lblStatus.Text != "Pending")
                        {
                            ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                            btnEdit.Enabled = false;
                            btnEdit.ImageUrl = "~/Themes/Images/edit-Disabled-16.png";

                            ImageButton btnSubmit = (ImageButton)e.Row.FindControl("btnSubmit");
                            btnSubmit.Enabled = false;
                            btnSubmit.ImageUrl = "~/Themes/Images/Submit-Disabled-16.png";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdNCQASubmission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNCQASubmission.EditIndex = -1;
            grdNCQASubmission.PageIndex = e.NewPageIndex;
            BindNCQASubmissionRequests();
        }

        protected void grdNCQASubmission_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int projectUsageId = Convert.ToInt32(grdNCQASubmission.DataKeys[e.RowIndex].Values[0].ToString());
                int practiceSiteId = Convert.ToInt32(grdNCQASubmission.DataKeys[e.RowIndex].Values[1].ToString());
                DateTime requestedOn = Convert.ToDateTime(grdNCQASubmission.DataKeys[e.RowIndex].Values[2].ToString());

                Label lblSubmissionType = (Label)grdNCQASubmission.Rows[e.RowIndex].FindControl("lblHdnSubmissionType");
                DropDownList ddlNCQAStatus = (DropDownList)grdNCQASubmission.Rows[e.RowIndex].FindControl("ddlNCQAStatus");

                if (Convert.ToInt32(lblSubmissionType.Text) == (int)enSubmissionType.Old)
                {
                    NCQASubmissionBO ncqaSubmission = new NCQASubmissionBO();
                    ncqaSubmission.UpdateNCQAStatus(projectUsageId, practiceSiteId, Convert.ToInt32(ddlNCQAStatus.SelectedValue), Convert.ToInt32(Session["UserApplicationId"]), requestedOn);
                }
                else if (Convert.ToInt32(lblSubmissionType.Text) == (int)enSubmissionType.New)
                {
                    NCQASubmissionMethod ncqaSubmissionMethod = new NCQASubmissionMethod();
                    ncqaSubmissionMethod.UpdateSubmissionStatus(projectUsageId, practiceSiteId, Convert.ToInt32(ddlNCQAStatus.SelectedValue), Convert.ToInt32(Session["UserApplicationId"]), requestedOn);
                }

                grdNCQASubmission.EditIndex = -1;
                BindNCQASubmissionRequests();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void grdNCQASubmission_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {
                    //Get MapPath, Uri, applicationPath, virtual Path
                    string mapPath = Server.MapPath("~/");
                    Uri uri = HttpContext.Current.Request.Url;

                    string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
                    if (virtualDirectory.Length > 1)
                        virtualDirectory += "/";


                    //Configure Log4Net
                    log4net.Config.XmlConfigurator.Configure();
                    log4net.ILog log = log4net.LogManager.GetLogger("SubmissionAppender");


                    //Get Submission Request Attributes
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    string password = string.Empty;
                    int projectUsageId = Convert.ToInt32(grdNCQASubmission.DataKeys[rowIndex].Values[0].ToString());
                    int practiceSiteId = Convert.ToInt32(grdNCQASubmission.DataKeys[rowIndex].Values[1].ToString());
                    DateTime requestedOn = Convert.ToDateTime(grdNCQASubmission.DataKeys[rowIndex].Values[2].ToString());

                    Label lblISSLicenseNumber = (Label)grdNCQASubmission.Rows[rowIndex].FindControl("lblISSLicenseNumber");
                    Label lblUserName = (Label)grdNCQASubmission.Rows[rowIndex].FindControl("lblUserName");
                    Label lblSubmissionType = (Label)grdNCQASubmission.Rows[rowIndex].FindControl("lblSubmissionType");


                    //Get SiteId, PracticeId   
                    //ProjectBO projectBO = new ProjectBO();
                    SiteBO siteBO = new SiteBO();

                    //int siteId = projectBO.GetSiteIDByProjectID(Convert.ToInt32(projectId));
                    siteBO.GetSiteBySiteId(practiceSiteId);
                    int practiceId = siteBO.PracticeId;


                    if (Convert.ToInt32(lblSubmissionType.Text) == (int)enSubmissionType.Old)
                    {
                        ExistingNCQASubmission(projectUsageId, practiceSiteId, requestedOn, lblISSLicenseNumber.Text, lblUserName.Text, mapPath, uri, applicationPath, log, virtualDirectory);
                    }
                    else if (Convert.ToInt32(lblSubmissionType.Text) == (int)enSubmissionType.New)
                    {
                        Label lblTemplateId = (Label)grdNCQASubmission.Rows[rowIndex].FindControl("lblTemplateId");
                        NCQASubmissionToAPI(projectUsageId, practiceSiteId, requestedOn, lblISSLicenseNumber.Text, lblUserName.Text, mapPath, uri, applicationPath, log, virtualDirectory,
                            Convert.ToInt32(lblTemplateId.Text));
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }
        #endregion

        #region FUNCTIONS
        protected void LoadingProcess()
        {
            try
            {
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    PracticeBO practiceBO = new PracticeBO();
                    IQueryable EnterpriseList;
                    EnterpriseList = practiceBO.GetEnterprises();
                    ddlEnterprise.DataTextField = "Name";
                    ddlEnterprise.DataValueField = "ID";
                    ddlEnterprise.DataSource = EnterpriseList;
                    ddlEnterprise.DataBind();

                    //Add Default item in comboBox
                    ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
                    lblEnterprise.Visible = ddlEnterprise.Visible = true;
                }
                else
                {
                    lblEnterprise.Visible = ddlEnterprise.Visible = false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void BindNCQASubmissionRequests()
        {
            try
            {
                GetEnterpriseId();

                NCQASubmissionBO ncqaSubmission = new NCQASubmissionBO();
                grdNCQASubmission.DataSource = ncqaSubmission.GetAllSubmissionRequest(enterpriseId);
                grdNCQASubmission.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void GetEnterpriseId()
        {
            try
            {
                userType = Session[enSessionKey.UserType.ToString()].ToString();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    if (!string.IsNullOrEmpty(ddlEnterprise.SelectedValue))
                    {
                        enterpriseId = Convert.ToInt32(ddlEnterprise.SelectedValue);
                    }
                }
                else
                    enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ExistingNCQASubmission(int projectUsageId, int practiceSiteId, DateTime requestedOn, string issLicenseNumber, string userName, string mapPath, Uri uri, string applicationPath,
            log4net.ILog log, string virtualDirectory)
        {
            try
            {
                string password = string.Empty;

                //Get PracticeId   
                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(practiceSiteId);
                int practiceId = siteBO.PracticeId;

                NCQASubmissionBO ncqaSubmission = new NCQASubmissionBO();
                password = ncqaSubmission.GetPasswordByProjectId(projectUsageId, practiceSiteId, requestedOn);
                ncqaSubmission.UpdateNCQAStatus(projectUsageId, practiceSiteId, (int)enSubmissionStatus.InProgress, Convert.ToInt32(Session["UserApplicationId"]), requestedOn);

                //Bind Submission Request
                BindNCQASubmissionRequests();


                //Get Questionnaire
                QuestionBO questionBO = new QuestionBO();
                //Fix//questionBO.ProjectId = Convert.ToInt32(projectId);
                questionBO.QuestionnaireId = (int)enQuestionnaireType.DetailedQuestionnaire;

                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);
                string recievedQuestionnaire = questionBO.GetQuestionnaireByType(medicalGroupId);

                //Parallel Task 
                Task asyncTask = Task.Factory.StartNew(() =>
                {
                    //NCQADocumentServices documentServices = new NCQADocumentServices();
                    //documentServices.SubmitNCQADocuments(issLicenseNumber, userName, password, Convert.ToInt32(projectId), enterpriseId, mapPath, uri, applicationPath, log, siteId, practiceId, recievedQuestionnaire);

                    ////Print Report
                    ////NCQASummary ncqaSummary = (NCQASummary)Page.LoadControl("~/UserControls/NCQASummary.ascx");
                    ////ncqaSummary.SiteName = projectBO.GetSiteNameByProjectID(Convert.ToInt32(projectId));
                    ////ncqaSummary.ProjectId = Convert.ToInt32(projectId);
                    ////ncqaSummary.practiceId = practiceId;
                    ////string ncqaNoteslocation = ncqaSummary.GenerateNotesReport(mapPath, uri, virtualDirectory, recievedQuestionnaire);

                    //documentServices.AddNotesDocument(ncqaNoteslocation, issLicenseNumber, userName, password, log);

                    //Update Status                        
                    ncqaSubmission.UpdateNCQAStatus(projectUsageId, practiceSiteId, (int)enSubmissionStatus.Fulfilled, Convert.ToInt32(Session["UserApplicationId"]), requestedOn);
                    BindNCQASubmissionRequests();
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void NCQASubmissionToAPI(int projectUsageId, int practiceSiteId, DateTime requestedOn, string issLicenseNumber, string userName, string mapPath, Uri uri, string applicationPath,
            log4net.ILog log, string virtualDirectory, int templateId)
        {
            try
            {
                string password = string.Empty;

                //Get SiteId, PracticeId   
                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(practiceSiteId);
                int practiceId = siteBO.PracticeId;

                NCQASubmissionMethod ncqaSubmissionMethod = new NCQASubmissionMethod();
                password = ncqaSubmissionMethod.GetPasswordByProjectId(practiceSiteId, requestedOn);
                ncqaSubmissionMethod.UpdateSubmissionStatus(projectUsageId, practiceSiteId, (int)enSubmissionStatus.InProgress, Convert.ToInt32(Session["UserApplicationId"]), requestedOn);

                //Bind Submission Request
                BindNCQASubmissionRequests();

                //Parallel Task 
                Task asyncTask = Task.Factory.StartNew(() =>
                {
                    ncqaSubmissionMethod.SubmitToNCQA(issLicenseNumber, userName, password, projectUsageId, enterpriseId, mapPath, uri, applicationPath, log, practiceSiteId, practiceId, templateId);

                    ////Print Report
                    //MOReSummary moreSummary = (MOReSummary)Page.LoadControl("~/UserControls/MOReSummary.ascx");
                    //moreSummary.SiteName = projectBO.GetSiteNameByProjectID(Convert.ToInt32(projectId));
                    //moreSummary.ProjectId = projectId;
                    //moreSummary.TemplateId = templateId;
                    //string notesReportlocation = moreSummary.GenerateNotesReport(mapPath, uri, virtualDirectory, practiceId);

                    //ncqaSubmissionMethod.AddNotesDocument(notesReportlocation, issLicenseNumber, userName, password, log);

                    ////Update NCQA Status                        
                    ncqaSubmissionMethod.UpdateSubmissionStatus(projectUsageId, practiceSiteId, (int)enSubmissionStatus.Fulfilled, Convert.ToInt32(Session["UserApplicationId"]), requestedOn);
                    BindNCQASubmissionRequests();
                });


            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}