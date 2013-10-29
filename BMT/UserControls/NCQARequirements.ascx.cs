using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class NCQARequirements : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const string DEFAULT_QUESTIONNAIRE_TYPE = "NCQA";
    private const int DEFAULT_SELECTED_TAB_ID = 8; /*To Summary Tab*/
    private const int DEFAULT_HEADER_CHILD_START = 4;
    private const int DEFAULT_HEADER_CHILD_END = 8;
    private const string DEFAULT_NODE_TEXT = "NCQA Submission";

    #endregion

    #region PROPERTIES
    public int ProjectId { get; set; }
    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; }

    #endregion

    #region VARIABLES
    private QuestionBO _questionBO;
    private int enterpriseId;
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;

            enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
            if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.User.ToString())
            {
                string disclaimerRequired = Util.GetSystemDisclaimerRequired(enterpriseId);
                if (disclaimerRequired == "YES")
                {
                    UserBO userBO = new UserBO();
                    bool isDisclaimerPassed = userBO.IsDisclaimerPassed(Convert.ToInt32(Session["UserApplicationId"]));

                    if (!isDisclaimerPassed)
                    {
                        pnlTabs.Visible = pcmh1.Visible = pcmh2.Visible = pcmh3.Visible = pcmh4.Visible = pcmh5.Visible =
                        pcmh6.Visible = general.Visible = summary.Visible = false;

                        pnlDisclaimer.Visible = true;
                        string disclaimerString = Util.GetSystemDisclaimerText(enterpriseId);
                        lblDisclaimer.Text = disclaimerString;                       
                    }
                    else
                    {
                        LoadControls();
                    }
                }
                else
                {
                    LoadControls();
                }
            }
            else
            {
                LoadControls();
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    #endregion

    #region FUNCTIONS

    protected void LoadControls()
    {
        try
        {
            if (Session[enSessionKey.UserType.ToString()] != null)
            {
                if (Session[enSessionKey.UserType.ToString()].ToString() != enUserRole.User.ToString())
                    hdnIsConsultant.Value = "true";
                else
                    hdnIsConsultant.Value = "false";
            }

            string requiredDocumentsEnabled = Util.GetSystemRequiredDocumentsEnabled(enterpriseId);
            if (requiredDocumentsEnabled == "YES")
                hdnRequiredDocsEnabled.Value = "YES";

            string markAsCompleteEnabled = Util.GetSystemMarkAsCompleteEnabled(enterpriseId);
            if (markAsCompleteEnabled == "YES")
                hdnMarkAsCompleteEnabled.Value = "YES";

            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == "NCQARequirements")
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);


                // Get Questionnaire By Type
                _questionBO = new QuestionBO();
                _questionBO.QuestionnaireId = (int)enQuestionnaireType.DetailedQuestionnaire;
                _questionBO.ProjectId = ProjectId;

                // get questionnaire
                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(PracticeId);
                string recievedQuestionnaire = _questionBO.GetQuestionnaireByType(medicalGroupId);

                Session[enSessionKey.NCQAQuestionnaire.ToString()] = recievedQuestionnaire;


                Session["NCQAQuestionnaireId"] = (int)enQuestionnaireType.DetailedQuestionnaire;

                int activeTab = Convert.ToInt32(hiddenClickTab.Value);
                if (activeTab == 0)
                    TabRendering(DEFAULT_SELECTED_TAB_ID);
                else
                    TabRendering(activeTab);

                ControlSettings();
                upnlNCQARequirements.Update();
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void ControlSettings()
    {
        try
        {
            // ProjectId, Practiceid, SiteId, SiteName, NodeName (Tool)
            pcmh1.ProjectId = pcmh2.ProjectId = pcmh3.ProjectId = pcmh4.ProjectId =
                pcmh5.ProjectId = pcmh6.ProjectId = general.ProjectId = summary.ProjectId = ProjectId;

            pcmh1.PracticeId = pcmh2.PracticeId = pcmh3.PracticeId = pcmh4.PracticeId =
               pcmh5.PracticeId = pcmh6.PracticeId = general.PracticeId = PracticeId;

            pcmh1.PracticeName = pcmh2.PracticeName = pcmh3.PracticeName = pcmh4.PracticeName =
                pcmh5.PracticeName = pcmh6.PracticeName = general.PracticeName = PracticeName;

            general.SiteId = pcmh1.SiteId = pcmh2.SiteId = pcmh3.SiteId = pcmh4.SiteId = pcmh5.SiteId =
                pcmh6.SiteId = SiteId;

            pcmh1.SiteName = pcmh2.SiteName = pcmh3.SiteName = pcmh4.SiteName =
           pcmh5.SiteName = pcmh6.SiteName = general.SiteName = summary.SiteName = SiteName;

            pcmh1.Node = pcmh2.Node = pcmh3.Node = pcmh4.Node = pcmh5.Node = pcmh6.Node = DEFAULT_NODE_TEXT;

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void TabRendering(int selectedTab)
    {
        try
        {
            pcmh1.Visible = pcmh2.Visible = pcmh3.Visible = pcmh4.Visible = pcmh5.Visible =
                pcmh6.Visible = general.Visible = summary.Visible = false;
            if (selectedTab == 1)
                pcmh1.Visible = true;
            else if (selectedTab == 2)
                pcmh2.Visible = true;
            else if (selectedTab == 3)
                pcmh3.Visible = true;
            else if (selectedTab == 4)
                pcmh4.Visible = true;
            else if (selectedTab == 5)
                pcmh5.Visible = true;
            else if (selectedTab == 6)
                pcmh6.Visible = true;
            else if (selectedTab == 7)
                general.Visible = true;
            else if (selectedTab == 8)
                summary.Visible = true;
            else
                pcmh1.Visible = pcmh2.Visible = pcmh3.Visible = pcmh4.Visible = pcmh5.Visible =
           pcmh6.Visible = general.Visible = summary.Visible = false;


        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlDisclaimer.Visible = false;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        UserBO userBO = new UserBO();
        userBO.UpdateDisclaimerPassed(Convert.ToInt32(Session["UserApplicationId"]));

        pnlTabs.Visible = pcmh1.Visible = pcmh2.Visible = pcmh3.Visible = pcmh4.Visible = pcmh5.Visible =
        pcmh6.Visible = general.Visible = summary.Visible = true;
        pnlDisclaimer.Visible = false;

        Uri uri = HttpContext.Current.Request.Url;
        Response.Redirect(uri.ToString(), false);
    }
}