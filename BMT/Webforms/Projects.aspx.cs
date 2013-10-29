#region Modification History

//  ******************************************************************************
//  Module        : Projects
//  Created By    : NA
//  When Created  : NA
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date                                Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01-01-2012 - 01-31-2012             Reorganized Code, Add super User functionality etc...
//  Mirza Fahad Ali Baig    01-31-2012                          Add logging
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Web.Services;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class Projects : System.Web.UI.Page
    {
        #region CONSTANTS
        //public string DEFAULT_PRICECONTROL_CONTENT_TYPE = "PriceCalculator";
        public string SECTION_TYPE_PROJECT_FOLDER = "Project Folder";
        //public string DEFAULT_EHR_UPLOAD_DOCUMENT_CONTENT_TYPE = "EHRUploadedDocuments";
        //public string DEFAULT_EXPRESSASSESSMENT_CONTENT_TYPE = "ExpressAssessment";
        //public string DEFAULT_NCQA_REQUIREMENT_CONTENT_TYPE = "NCQARequirements";
		public string SECTION_TYPE_TEMPLATE = "Template";
        public string TEMPLATE_STANDARD_FOLDER = "StandardFolder";
        public string TEMPLATE_DOCUMENTATION_PACKAGE = "DocumentStore";
       // public string DEFAULT_PRICE_CALCULATOR_REPORT_NAME = "PriceCalculator";
        public string SECTION_TYPE_FOLDER = "Folder";
        public string SECTION_TYPE_IT_CONSULTANT = "IT Consultant";
        public string SECTION_TYPE_FORM = "Form";
        #endregion

        #region VARIABLES
        private ProjectBO _project = new ProjectBO();
        private PracticeBO _practice;
        int UserId;
        int ProjectUsageId;
        int SectionId;
        string UserType;
        int PracticeId;
        int EnterpriseId;
        string PracticeName;
        int SiteId;
		int templateId;
        string SiteName;

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] != null)
                {
                    LoadingProcess();
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void ddlPractices_OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    if (UserType == enUserRole.SuperAdmin.ToString())
                    {
                        Session["EnterpriseId"] = ddlEnterprise.SelectedValue;
                        EnterpriseId = Convert.ToInt32(Session["EnterpriseId"]);
                    }

                    Session["PracticeId"] = ddlPractices.SelectedValue;

                    // clear existing questionnaire on practice change 
                    if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                        Session[enSessionKey.NCQAQuestionnaire.ToString()] = null;

                    PracticeId = Convert.ToInt32(Session["PracticeId"]);

                    // Reset control before to switch on next practice
                    PriceCalculator.OnResetFlag();

                    Response.Redirect("~/Webforms/Projects.aspx", false);
                    //Response.Redirect(Request.RawUrl); 
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
                        ddlPractices.Enabled = true;
                        GetPracticeByEnterpriseId(Convert.ToInt32(ddlEnterprise.SelectedValue));

                        Session["PracticeId"] = ddlPractices.SelectedValue;
                        PracticeId = Convert.ToInt32(Session["PracticeId"]);

                        Session["EnterpriseName"] = ddlEnterprise.SelectedItem.Text.ToString();

                        Session["EnterpriseId"] = ddlEnterprise.SelectedValue;
                        EnterpriseId = Convert.ToInt32(Session["EnterpriseId"]);
                    }
                    else
                    {
                        Session["PracticeId"] = PracticeId = 0;
                    }                    

                    // clear existing questionnaire on practice change 
                    if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                        Session[enSessionKey.NCQAQuestionnaire.ToString()] = null;

                    // Reset control before to switch on next practice
                    PriceCalculator.OnResetFlag();

                    Response.Redirect("~/Webforms/Projects.aspx", false);

                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        #endregion

        #region FUNCTION

        private void LoadingProcess()
        {
            Security _security = new Security();
            TreeBO _tree = new TreeBO();
            #region HandleDashboardRequest
            if (Request.QueryString["PTemp"] != null)
            {
                string tempPracticeId = Request.QueryString["PTemp"].ToString();
                tempPracticeId = _security.Decrypt(tempPracticeId);
                Session["PracticeId"] = tempPracticeId;
            }

            if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.SuperAdmin.ToString())
            {
                if (Request.QueryString["ETemp"] != null)
                {
                    if (Request.QueryString["ETemp"].ToString() != string.Empty)
                    {
                        string tempEnterpriseId = Request.QueryString["ETemp"].ToString();
                        tempEnterpriseId = _security.Decrypt(tempEnterpriseId);

                        Session["EnterpriseId"] = tempEnterpriseId;
                        EnterpriseId = Convert.ToInt32(Session["EnterpriseId"]);
                    }
                }
            }

            #endregion

            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                UserId = Session[enSessionKey.UserApplicationId.ToString()] != null ? Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]) : 0;
                UserType = Session[enSessionKey.UserType.ToString()] != null ? Session[enSessionKey.UserType.ToString()].ToString() : string.Empty;
                PracticeId = Session[enSessionKey.PracticeId.ToString()] != null ? Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]) : 0;
                EnterpriseId = Session[enSessionKey.EnterpriseId.ToString()] != null ? Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]) : 0;
				templateId = Request.QueryString["TemplateID"] != null ? Convert.ToInt32(Request.QueryString["TemplateID"]) : 0;
            }
            else
            {
                SessionHandling _sessionHandling = new SessionHandling();
                _sessionHandling.ClearSession();
                Response.Redirect("~/Account/Login.aspx");
            }


            if (!Page.IsPostBack)
            {
                if (UserType == enUserRole.SuperUser.ToString() || UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.SuperAdmin.ToString())
                {
                    GetPractices();
                }
                else if (UserType == enUserRole.User.ToString())
                {
                    string IsNCQADashboardVisible = Util.IsNCQADashboardVisible(EnterpriseId);
                    if (IsNCQADashboardVisible == "Yes")
                    {
                        if (Request.QueryString["NodeProjectUsageID"] == null)
                        {
                            //string siteName = _tree.GetDefaultSiteName(PracticeId);
                            //int SID = _tree.GetSiteID(PracticeId, siteName);
                            //int PID = _tree.GetProjectID(SID);

                            //string sectionType = "NCQARequirements";
                            //int SectionId = PID;
                            //string Path = "PCMH Recognition/" + siteName + "/3";

                            //Response.Redirect("~/Webforms/Projects.aspx?NodeContentType=" + sectionType + "&NodeProjectUsageID=" + SectionId.ToString() + "&Active=ctl00_bodyContainer_TreeControl_treeViewt7&Path=" + Path, false);
                        }
                    }
                }

            }

            if (Request.QueryString["NodeProjectUsageID"] == null)
            {
                TreeControl.IsFromTree = false;
            }
            else
            {
                TreeControl.IsFromTree = true;
            }


            #region ON_EACH_POSTBACK

            GetRequiredValues();
            BindProperties();

            #region RETURN_TO_ASSESSMENT_CASE

            //if (Session["ReturnContentType"] != null)
            //{
            //    if (Session["ReturnContentType"].ToString() == "Form" && ProjectUsageId != 0)
            //    {
            //        string returnCall = Session["IfReturn"].ToString();
            //        if (returnCall == "Default")
            //        {
            //            ExpressAssessment.Visible = NCQARequirements.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible =
            //                 btnUploadDocuments.Visible = ProjectDocument.Visible = MOReRequirments.Visible = false;
            //            ExpressAssessment.UserId = UserId;
            //            ExpressAssessment.ProjectUsageId = ProjectUsageId;
            //            ExpressAssessment.SiteName = SiteName;
            //            ExpressAssessment.Visible = true;
            //            Session[enSessionKey.currentControl.ToString()] = "ExpressAssessment";
            //            Session["IfReturn"] = null;
            //        }
            //    }

            //}
            //if (Request.QueryString["NodeContentType"] != null)
            //{
            //    if (Request.QueryString["NodeContentType"] == "PriceCalculator" && Session["IfReturn"] != null)
            //    {
            //        pnlDynamicControl.Controls.Clear();
            //        Session[enSessionKey.currentControl.ToString()] = DEFAULT_PRICECONTROL_CONTENT_TYPE;
            //        PriceCalculator.IsReturn = true;
            //        PriceCalculator.Visible = true;
            //        btnAddEHR.Visible = true;
            //        Session["IfReturn"] = null;
            //    }
            //}

            #endregion

            #region Reports
            // load  Report

            // clear all dynamic control if exist
            pnlDynamicControl.Controls.Clear();

            if (Request.QueryString["report"] != null && Request.QueryString["puid"] != null)
            {
                if (Request.QueryString["report"] == "expressAssessment")
                {
                    // Disable All control on each post back
                    ProjectUsageId = Convert.ToInt32(Request.QueryString["puid"]);
                    SiteId = Convert.ToInt32(Request.QueryString["SiteId"]);
                    SectionId = Convert.ToInt32(Request.QueryString["NodeSectionID"]);
                    NCQARequirements.Visible = ExpressAssessment.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible = MOReRequirments.Visible = false;

                    ExpressAssessmentResult expressAssessmentResult = Page.LoadControl("~/UserControls/ExpressAssessmentResult.ascx") as ExpressAssessmentResult;
                    expressAssessmentResult.ProjectUsageId = ProjectUsageId;
                    expressAssessmentResult.SectionId = SectionId;
                    expressAssessmentResult.SiteName = _project.GetSiteNameBySiteID(SiteId);
                    expressAssessmentResult._questionnaireType = enQuestionnaireType.SimpleQuestionnaire;
                    expressAssessmentResult.SiteId = SiteId;

                    Session["IfReturn"] = "Default";
                    pnlDynamicControl.Controls.Add(expressAssessmentResult);
                    Session[enSessionKey.currentControl.ToString()] = "ExpressAssessmentResult";
                }
                else if (Request.QueryString["report"] == "PriceCalculator")
                {

                    NCQARequirements.Visible = ExpressAssessment.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible =
                        PriceCalculator.Visible = btnAddEHR.Visible = MOReRequirments.Visible = false;

                    PriceComparison PriceComparison = Page.LoadControl("~/UserControls/PriceComparison.ascx") as PriceComparison;
                    PriceComparison.ProjectUsageId = Convert.ToInt32(Request.QueryString["puid"]);
                    PriceComparison.PracticeName = _project.GetPracticeNameByPracticeID(PracticeId);
                    PriceComparison.SystemSequence = Request.QueryString["system"];
                    PriceComparison.SectionId = Convert.ToInt32(Request.QueryString["NodeSectionID"]);

                    pnlDynamicControl.Controls.Add(PriceComparison);
                    Session[enSessionKey.currentControl.ToString()] = "PriceCalculator";

                }
            }

            #endregion

            #region CONTROL_HANDLING

            if (hdnTreeNodeID.Value != "")
            {
                Session["selectedNodeId"] = hdnTreeNodeID.Value;
            }
            else
            {
                if (Session["selectedNodeId"] != null)
                {
                    hdnTreeNodeID.Value = Session["selectedNodeId"].ToString();
                }
            }

            #endregion

            // Load selected control            
            LoadControl();

            // generate Tree
            // Pass userid to tree control 
            string path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;
            
            TreeControl.IsSRA = path.Contains("SRA") ? true : false;
            TreeControl.PracticeId = PracticeId;            
            TreeControl.SiteName = SiteName;

            #endregion
        }

        private void GetPractices()
        {
            try
            {
                IQueryable _practiceList;
                IQueryable _enterpriseList;
                // TODO: Get the list of practices When SuperUser/Consultant User logged-in
                if (UserType == enUserRole.SuperUser.ToString() || UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.SuperAdmin.ToString())
                {
                    if (UserType == enUserRole.SuperAdmin.ToString())
                    {
                        lblEnterprise.Visible = ddlEnterprise.Visible = lblPractices.Visible = ddlPractices.Visible = true;

                        _practice = new PracticeBO();
                        _enterpriseList = _practice.GetEnterprises();
                        ddlEnterprise.DataTextField = "Name";
                        ddlEnterprise.DataValueField = "ID";
                        ddlEnterprise.DataSource = _enterpriseList;
                        ddlEnterprise.DataBind();

                        //Add Default item in comboBox
                        ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Enabled = false;

                        if (EnterpriseId > 0)
                        {
                            ddlEnterprise.SelectedIndex = ddlEnterprise.Items.IndexOf(ddlEnterprise.Items.FindByValue(EnterpriseId.ToString()));
                            ddlPractices.Enabled = true;
                            GetPracticeByEnterpriseId(EnterpriseId);
                        }
                    }
                    else
                    {
                        lblPractices.Visible = ddlPractices.Visible = true;

                        _practice = new PracticeBO();
                        int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                        _practiceList = _practice.GetPractices(UserId, UserType, enterpriseId);

                        ddlPractices.DataTextField = "Name";
                        ddlPractices.DataValueField = "ID";
                        ddlPractices.DataSource = _practiceList;
                        ddlPractices.DataBind();

                        //Add Default item in comboBox
                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue(PracticeId.ToString()));
                    }

                }
                else
                {
                    ddlPractices.Items.Clear();
                    lblPractices.Visible = ddlPractices.Visible = false;
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void GetRequiredValues()
        {
            try
            {
                // Fetching All Required values
                if (!Page.IsPostBack)
                {
                    Session["CurrentProjectUsageId"] = Request.QueryString["NodeProjectUsageID"];
                }

                ProjectUsageId = Session["CurrentProjectUsageId"] != null && Session["CurrentProjectUsageId"].ToString() != string.Empty ? Convert.ToInt32(Session["CurrentProjectUsageId"]) : 0;

                if (ProjectUsageId != 0)
                {
                    PracticeName = _project.GetPracticeNameByPracticeID(PracticeId);
                    SiteId = Request.QueryString["SiteId"] != null && Request.QueryString["SiteId"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["SiteId"]) : 0;
                    SiteName = _project.GetSiteNameBySiteID(SiteId);
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        protected void BindProperties()
        {
            try
            {
                #region BIND_PROPERTIES

                //  Set proprties of all controls
                if (ProjectUsageId != 0)
                {
                    // Bind properties on each page load

                    // NCQARequirement control
                    NCQARequirements.ProjectUsageId = ProjectUsageId;
                    NCQARequirements.PracticeId = PracticeId;
                    NCQARequirements.PracticeName = PracticeName;
                    NCQARequirements.SiteId = SiteId;
                    NCQARequirements.SiteName = SiteName;

                    // MOReRequirments control
                    MOReRequirments.ProjectUsageId = ProjectUsageId;
                    MOReRequirments.PracticeId = PracticeId;
                    MOReRequirments.PracticeName = PracticeName;
                    MOReRequirments.SiteId = SiteId;
                    MOReRequirments.SiteName = SiteName;
                    //MOReRequirments.TemplateId = templateId;

                    // ExpressAssessment control
                    ExpressAssessment.UserId = UserId;
                    ExpressAssessment.ProjectUsageId = ProjectUsageId;
                    ExpressAssessment.SiteId = SiteId;
                    ExpressAssessment.SiteName = SiteName;

                    // NCQASubmission control
                    NCQASubmission.ProjectUsageId = ProjectUsageId;
                    NCQASubmission.SiteId = SiteId;
                    NCQASubmission.SiteName = SiteName;
                    NCQASubmission.PracticeId = PracticeId;
                    NCQASubmission.PracticeName = PracticeName;

                    // NCQARequirement control
                    SecurityRiskAssessment.ProjectUsageId = ProjectUsageId;
                    SecurityRiskAssessment.PracticeId = PracticeId;
                    SecurityRiskAssessment.SiteId = SiteId;
      

                    ProjectDocument.ToolSectionId = Convert.ToInt32(Session["CurrentProjectUsageId"] != null && Session["CurrentProjectUsageId"].ToString() != string.Empty ? Convert.ToInt32(Session["CurrentProjectUsageId"]) : 0);
                    ProjectDocument.PracticeId = PracticeId;

                    btnAddEHR.Visible = false;

                    // IT Consultant Control
                    ITConsultant.PracticeId = PracticeId;

                }

                #endregion
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        protected void LoadControl()
        {

            try
            {
                #region __DOPOSTBACK_HANDLING

                // Fetching values form Query string to check the informaiton against the selected Node
                string sectionType = Request.QueryString["NodeContentType"] != null ? Request.QueryString["NodeContentType"] : string.Empty;
                int ProjectUsageId = Request.QueryString["NodeProjectUsageID"] != null && Request.QueryString["NodeProjectUsageID"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["NodeProjectUsageID"]) : 0;
                int SectionId = Request.QueryString["NodeSectionID"] != null && Request.QueryString["NodeSectionID"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["NodeSectionID"]) : 0;
                string Path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;
				templateId = Request.QueryString["TemplateID"] != null ? Convert.ToInt32(Request.QueryString["TemplateID"]) : 0;
                SiteId = Request.QueryString["SiteId"] != null && Request.QueryString["SiteId"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["SiteId"]) : 0;

                if (sectionType != string.Empty && SectionId != 0)
                {
                    Session["ReturnContentType"] = Path;
                }

                if (sectionType != string.Empty && SectionId != 0)
                {
                    // By Default Clear All controls
                    pnlDynamicControl.Controls.Clear();
                    Session[enSessionKey.currentControl.ToString()] = sectionType;

                    if (sectionType == SECTION_TYPE_PROJECT_FOLDER)
                    {
                        EnableUploadControl(SectionId, PracticeId, ProjectUsageId);
                    }
                    else if (sectionType == TEMPLATE_STANDARD_FOLDER)
                    {
                        EnableStandardDocumentControl(SectionId);
                        if (!(UserType == enUserRole.SuperUser.ToString() || UserType == enUserRole.SuperAdmin.ToString()))
                            btnUploadDocuments.Visible = false;
                    }
                    else if (sectionType == SECTION_TYPE_FORM && ProjectUsageId != 0)
                    {
                        ProjectBO project = new ProjectBO();
                        int fkFormId = project.GetFormIdBySectionId(SectionId);
                        NCQARequirements.Visible = ExpressAssessment.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible =
                           btnUploadDocuments.Visible = ProjectDocument.Visible = lblContentTypeName.Visible = MOReRequirments.Visible =  false;

                       Session[enSessionKey.currentControl.ToString()] = SECTION_TYPE_FORM;
                        if (fkFormId == (int)enFormType.ExpressAssessment)
                        {
                            ExpressAssessment.SectionId = SectionId;
                            ExpressAssessment.Visible = true;
                        }
                        if (fkFormId == (int)enFormType.SecurityRiskAssessment)
                        {
                            SecurityRiskAssessment.SectionId = SectionId;
                            SecurityRiskAssessment.Visible = true;
                        }
                        if (fkFormId == (int)enFormType.EHRSelection)
                        {
                            PriceCalculator.ProjectUsageId = ProjectUsageId;
                            PriceCalculator.SectionId = SectionId;
                            PriceCalculator.Visible = true;
                            btnAddEHR.Visible = true;
                        }
                           
                    }
                    else if (sectionType == SECTION_TYPE_TEMPLATE && ProjectUsageId != 0)
                    {
                        ProjectBO project = new ProjectBO();
                        int fkTempId = project.GetTemplateIdBySectionId(SectionId);
                        MOReRequirments.Visible = ExpressAssessment.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible =
                            btnUploadDocuments.Visible = ProjectDocument.Visible = lblContentTypeName.Visible = NCQARequirements.Visible = false;

                        Session[enSessionKey.currentControl.ToString()] = SECTION_TYPE_TEMPLATE;
                        MOReRequirments.SectionId = SectionId;
                        MOReRequirments.TemplateId = fkTempId;
                        MOReRequirments.Visible = true;
                    }

                    else if (sectionType == TEMPLATE_DOCUMENTATION_PACKAGE && ProjectUsageId != 0)
                    {
                        NCQARequirements.Visible = ExpressAssessment.Visible = NCQASubmission.Visible = btnUploadDocuments.Visible =
						btnUploadDocuments.Visible = ProjectDocument.Visible = lblContentTypeName.Visible = MOReRequirments.Visible = false;

                        ProjectBO project = new ProjectBO();
                        int fkTempId = project.GetTemplateIdByProjectUsageId(ProjectUsageId);
                        NCQASubmission.TemplateId = fkTempId;
                        Session[enSessionKey.currentControl.ToString()] = TEMPLATE_DOCUMENTATION_PACKAGE;

                        NCQASubmission.Visible = true;
                    }
                    else if (sectionType == SECTION_TYPE_IT_CONSULTANT)
                    {
                        Session[enSessionKey.currentControl.ToString()] = SECTION_TYPE_IT_CONSULTANT;
                        ITConsultant.Visible = true;
                    }
                    else if (sectionType == SECTION_TYPE_FOLDER)
                    {
                        EnableUploadControl(SectionId, 0, 0);
                        if (!(UserType == enUserRole.SuperUser.ToString() || UserType == enUserRole.SuperAdmin.ToString()))
                            btnUploadDocuments.Visible = false;
                    }
                }
                #endregion
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void EnableUploadControl(int SectionId, int practiceId,int projectUsageId)
        {
            Session["TableName"] = enDbTables.ProjectDocument.ToString();
            Session["SectionId"] = SectionId.ToString();
            Session["ProjectUsageId"] = projectUsageId.ToString();
            Session["PracticeId"] = PracticeId.ToString();
            Session[enSessionKey.currentControl.ToString()] = ContentType;

            ProjectDocument.ToolSectionId = SectionId;
            ProjectDocument.PracticeId = practiceId;
            ProjectDocument.ProjectUsageId = projectUsageId;
            btnUploadDocuments.Visible = ProjectDocument.Visible = true;
            lblContentTypeName.Text = _project.GetNodeNameBySectionId(Convert.ToInt32(Session["SectionId"].ToString()));
            lblContentTypeName.Visible = true;

        }

        private void EnableStandardDocumentControl(int SectionId)
        {
            Session["TableName"] = enDbTables.ProjectDocument.ToString();
            Session["SectionId"] = SectionId.ToString();
            Session["ProjectUsageId"] = 0;
            Session[enSessionKey.currentControl.ToString()] = ContentType;

            ProjectDocument.ToolSectionId = SectionId;
            ProjectDocument.PracticeId = 0;
            ProjectDocument.Visible = true;
            btnUploadDocuments.Visible = false;
            int tempId = _project.GetTemplateIdByParentSectionId(SectionId);
            ProjectDocument.TemplateId = tempId;
            lblContentTypeName.Text = _project.GetStandardFolderName(tempId);
            lblContentTypeName.Visible = true;

        }

        private void GetPracticeByEnterpriseId(int enterpriseId)
        {
            _practice = new PracticeBO();
            IQueryable _practiceList;
            _practiceList = _practice.GetPracticesByEnterpriseId(enterpriseId);

            ddlPractices.DataTextField = "Name";
            ddlPractices.DataValueField = "ID";
            ddlPractices.DataSource = _practiceList;
            ddlPractices.DataBind();

            //Add Default item in comboBox
            ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue(PracticeId.ToString()));
        }
        #endregion

    }
}
