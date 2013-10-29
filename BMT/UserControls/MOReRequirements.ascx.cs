using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.UI.HtmlControls;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;
using System.Reflection;

public partial class MOReRequirements : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const string DEFAULT_QUESTIONNAIRE_TYPE = "NCQA";
    private const int DEFAULT_SELECTED_TAB_ID = 0; /*To Summary Tab*/
    private const int DEFAULT_HEADER_CHILD_START = 4;
    private const int DEFAULT_HEADER_CHILD_END = 8;
    private const string DEFAULT_NODE_TEXT = "NCQA Submission";

    #endregion

    #region PROPERTIES
    public int SectionId { get; set; }
    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; }
    public int TemplateId { get; set; }

    #endregion

    #region VARIABLES
    private QuestionBO _questionBO;
    private MOReBO _MORe;
    UserControl _summary = null;
    UserControl _more = null;
    #endregion

    #region EVENTS

    protected override bool OnBubbleEvent(object source, EventArgs args)
    {
        CommandEventArgs e = args as CommandEventArgs;
        if (e != null && e.CommandName == "Reload")
        {
            // load summary tab again
            MOReSummary summary = _summary as MOReSummary;
            if (summary != null)
                summary.Reload();
        }
        if (e != null && e.CommandName == "ReloadMORe")
        {
            // load more tab again
            MORe more = _more as MORe;
            if (more != null)
                more.Reload();
        }
        return base.OnBubbleEvent(source, args);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Visible)
            {

                //TemplateId = 1; // For testing
                if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.User.ToString())
                {
                    string disclaimerRequired = Util.GetSystemDisclaimerRequired(Convert.ToInt32(Session["MedicalGroupId"]));
                    if (disclaimerRequired == "YES")
                    {
                        UserBO userBO = new UserBO();
                        bool isDisclaimerPassed = userBO.IsDisclaimerPassed(Convert.ToInt32(Session["UserApplicationId"]));

                        LoadControls();
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

            string requiredDocumentsEnabled = Util.GetSystemRequiredDocumentsEnabled(Convert.ToInt32(Session["MedicalGroupId"]));
            if (requiredDocumentsEnabled == "YES")
                hdnRequiredDocsEnabled.Value = "YES";

            string markAsCompleteEnabled = Util.GetSystemMarkAsCompleteEnabled(Convert.ToInt32(Session["MedicalGroupId"]));
            if (markAsCompleteEnabled == "YES")
                hdnMarkAsCompleteEnabled.Value = "YES";

            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == "Template")
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);

                LoadTabs();
                // Get Questionnaire By Type
                _questionBO = new QuestionBO();
                _questionBO.QuestionnaireId = (int)enQuestionnaireType.DetailedQuestionnaire;
                _questionBO.ProjectUsageId = ProjectUsageId;

                // get questionnaire
                //string recievedQuestionnaire = _questionBO.GetQuestionnaireByType(Convert.ToInt32(Session["MedicalGroupId"]));

                //Session[enSessionKey.NCQAQuestionnaire.ToString()] = recievedQuestionnaire;

                Session["TemplateId"] = hiddenTemplateId.Value = TemplateId.ToString();
                Session["SiteId"] = SiteId.ToString();
                Session["NCQAQuestionnaireId"] = (int)enQuestionnaireType.DetailedQuestionnaire;

                int activeTab = Convert.ToInt32(hiddenClickTab.Value);
                //if (activeTab == 0)
                   // LoadUserControls(DEFAULT_SELECTED_TAB_ID);//TabRendering(DEFAULT_SELECTED_TAB_ID);
                //else
                    LoadUserControls(activeTab); //TabRendering(activeTab);


                //ControlSettings();
                upnlMOReRequirements.Update();
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadUserControls(int selectedTab)
    {

        try
        {
            /************************    NCQA Summary     *****************************/
            UserControl summary = (UserControl)LoadControl(@"~/UserControls/MOReSummary.ascx");
            _summary = summary;
            PropertyInfo[] summaryInfo = summary.GetType().GetProperties();

            foreach (PropertyInfo item in summaryInfo)
            {
                if (item.CanWrite)
                {
                    switch (item.Name)
                    {

                        case "ProjectUsageId":
                            item.SetValue(summary, ProjectUsageId, null);
                            break;
                        case "SiteId":
                            item.SetValue(summary, SiteId, null);
                            break;
                        case "SiteName":
                            item.SetValue(summary, SiteName, null);
                            break;
                        case "TemplateId":
                            item.SetValue(summary, TemplateId, null);
                            break;
                        case "SectionId":
                            item.SetValue(summary, SectionId, null);
                            break;
                        default: break;
                    }
                }
            }

            PlaceHolderControls.Controls.Add(summary);



            /***************************   General    **********************************/

            UserControl general = (UserControl)LoadControl(@"~/UserControls/MOReGeneral.ascx");
            PropertyInfo[] generalInfo = general.GetType().GetProperties();

            foreach (PropertyInfo item in generalInfo)
            {
                if (item.CanWrite)
                {
                    switch (item.Name)
                    {

                        case "ProjectUsageId":
                            item.SetValue(general, ProjectUsageId, null);
                            break;
                        case "PracticeId":
                            item.SetValue(general, PracticeId, null);
                            break;
                        case "PracticeName":
                            item.SetValue(general, PracticeName, null);
                            break;
                        case "SiteId":
                            item.SetValue(general, SiteId, null);
                            break;
                        case "SiteName":
                            item.SetValue(general, SiteName, null);
                            break;
                        case "TemplateId":
                            item.SetValue(general, TemplateId, null);
                            break;
                        default: break;
                    }
                }
            }
            PlaceHolderControls.Controls.Add(general);

            if ((selectedTab != 0 && selectedTab != 1) || (Convert.ToInt32(hiddenOldClickTab.Value) >= 2))
            {
                /***************************  PCMH  *********************************/

                List<KnowledgeBase> Kb = _MORe.GetKnowledgeBaseHeadersByTemplateId(TemplateId);

                hdnTotalControls.Value = Kb.Count.ToString();

                for (int index = 0; index < Kb.Count; index++)
                {
                    if (((selectedTab - 1) == (index + 1)) || ((index + 2) == Convert.ToInt32(hiddenOldClickTab.Value)))
                    {
                        UserControl MORe = new UserControl();
                        MORe = (UserControl)LoadControl(@"~/UserControls/MORe.ascx");
                        _more = MORe;
                        PropertyInfo[] info = MORe.GetType().GetProperties();
                        MORe.ID = "MORe" + (index + 1);
                        //MORe.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        MORe.Visible = false;

                        foreach (PropertyInfo item in info)
                        {
                            if (item.CanWrite)
                            {
                                switch (item.Name)
                                {
                                    case "PracticeId":
                                        item.SetValue(MORe, PracticeId, null);
                                        break;
                                    case "PracticeName":
                                        item.SetValue(MORe, PracticeName, null);
                                        break;
                                    case "ProjectUsageId":
                                        item.SetValue(MORe, ProjectUsageId, null);
                                        break;
                                    case "Node":
                                        item.SetValue(MORe, DEFAULT_NODE_TEXT, null);
                                        break;
                                    case "SiteName":
                                        item.SetValue(MORe, SiteName, null);
                                        break;
                                    case "SiteId":
                                        item.SetValue(MORe, SiteId, null);
                                        break;
                                    case "MOReType":
                                        item.SetValue(MORe, (index + 1)/*(selectedTab - 1)*/, null);
                                        break;
                                    case "TemplateId":
                                        item.SetValue(MORe, TemplateId, null);
                                        break;
                                    case "HeaderId":
                                        item.SetValue(MORe, Kb[index].KnowledgeBaseId/*Kb[(selectedTab - 1)].KnowledgeBaseId*/, null);
                                        break;

                                    default: break;
                                }
                            }
                        }

                        PlaceHolderControls.Controls.Add(MORe);


                        string id = "MORe" + (selectedTab - 1);
                        MORe.Visible = MORe.ID == id;
                    }
                }


            }

            summary.Visible = selectedTab == 0;
            general.Visible = selectedTab == 1;

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void LoadTabs()
    {

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Attributes.Add("class", "divTabs");

        HtmlGenericControl ul = new HtmlGenericControl("ul");
        ul.Attributes.Add("class", "tabs");

        HtmlGenericControl liSummary = new HtmlGenericControl("li");
        //liSummary.Attributes.Add("class", "activeTab");
        liSummary.ID = "tabList0";

        ul.Controls.Add(liSummary);

        LinkButton SummaryTab = new LinkButton();

        SummaryTab.Text = "Summary";
        SummaryTab.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        SummaryTab.OnClientClick = "javascript:updateClickTab(0);";

        liSummary.Controls.Add(SummaryTab);

        HtmlGenericControl liGeneral = new HtmlGenericControl("li");
        liGeneral.ID = "tabList1";

        LinkButton GeneralTab = new LinkButton();

        GeneralTab.Text = "General";
        GeneralTab.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        GeneralTab.OnClientClick = "javascript:updateClickTab(1);";
        //liGeneral.Attributes.Add("class", "activeTab");
        liGeneral.Controls.Add(GeneralTab);

        ul.Controls.Add(liGeneral);

        if (hiddenClickTab.Value != null)
        {
            if (hiddenClickTab.Value == "0")
            {
                liGeneral.Attributes.Remove("class");
                liSummary.Attributes.Add("class", "activeTab");
            }

            else if (hiddenClickTab.Value == "1")
            {
                liSummary.Attributes.Remove("class");
                liGeneral.Attributes.Add("class", "activeTab");
                //ScriptManager.RegisterStartupScript(this,this.GetType(),"TabSwitch","window.open(document.URL,\"_self\");",true);
            }

        }

        _MORe = new MOReBO();

        List<KnowledgeBase> Kb = _MORe.GetKnowledgeBaseHeadersByTemplateId(TemplateId);

        for (int index = 0; index < Kb.Count; index++)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.ID = "tabList" + (index + 2);

            LinkButton Tab = new LinkButton();

            Tab.Text = Kb[index].TabName;
            Tab.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            Tab.OnClientClick = "javascript:updateClickTab(" + (index + 2) + ");";

            if (hiddenClickTab.Value != null)
            {
                if (Convert.ToInt32(hiddenClickTab.Value) == (index + 2))
                {
                    li.Attributes.Remove("class");
                    li.Attributes.Add("class", "activeTab");
                    Session["TemplateHeaderId"] = Kb[index].KnowledgeBaseId;
                    Session["HeaderSequence"] = (index + 2);
                }

            }

            li.Controls.Add(Tab);
            ul.Controls.Add(li);
        }


        div.Controls.Add(ul);
        pnlTabs.Controls.Add(div);

    }

   

    #endregion

}