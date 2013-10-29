using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;

public partial class SRAFindings : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public int SiteId { get; set; }
    public int SectionId { get; set; }

    #endregion

    #region Variables
    private XDocument questionnaire;

    private XDocument storedQuestionnaire;
    private SRAFindingsDetails _sraFindingsDetails;

    private int userApplicationId;
    private string userType;
    private int practiceId;

    private int highRiskCount;
    private int mediumRiskCount;
    private int lowRiskCount;

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 7;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Risk Found <br />(High & Medium only)";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Risk Rating";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Existing Control Measures Applied";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Recommended Control Measures";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Owner";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Remediation Steps";
    private const string DEFAULT_HEADER_PARENT_POS7 = "Target Date";

    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "tableElement";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "element-table";

    private const string DEFAULT_THREAT_LABEL_ID_PREFIX = "lblThreatVulnerability";
    private const string DEFAULT_EXISTING_CONTROL_LABEL_ID_PREFIX = "lblExistingControl";
    private const string DEFAULT_RECOMMENDED_LABEL_ID_PREFIX = "lblRecommendedControlMeasure";

    private const string DEFAULT_OWNER_LABEL_ID_PREFIX = "lblOwner";
    private const string DEFAULT_OWNER_TEXTBOX_ID_PREFIX = "txtboxOwner";

    private const string DEFAULT_REMEDIATION_STEPS_LABEL_ID_PREFIX = "lblRemediation";
    private const string DEFAULT_REMEDIATION_STEPS_TEXTBOX_ID_PREFIX = "txtboxRemediation";

    private const string DEFAULT_DATE_PICKER_TEXTBOX_ID_PREFIX = "txtPostdate";

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "Findings and Recommendations";


    #endregion

    #region CONTROLS
    private Table findingTable;
    private TableRow _tableRow;
    private TableCell _tableCell;
    private Table elementTable;
    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[enSessionKey.UserApplicationId.ToString()] != null)
        {
            userApplicationId = Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]);
            userType = Session[enSessionKey.UserType.ToString()].ToString();
            practiceId = Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]);
        }
        else
        {
            SessionHandling sessionHandling = new SessionHandling();
            sessionHandling.ClearSession();
            Response.Redirect("~/Account/Login.aspx");
        }

    }

    protected void btnPrintFindings_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateReport(false);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion

    #region FUNCTIONS

    public void GenerateLayout()
    {
        try
        {
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                if (Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0))
                {
                    chbFinalize.Checked = Convert.ToBoolean(questionnaire.Root.Element("Findings").Attribute("Finalize").Value);

                    if (questionnaire.Root.Element("Findings").Attributes().Count() == 2)
                        lblFindUserInfo.Text = questionnaire.Root.Element("Findings").Attribute("Completedby").Value;
                    else
                        lblFindUserInfo.Text = "";
                }
                else
                {
                    chbFinalize.Checked = false;
                    lblFindUserInfo.Text = "";
                }

                bool isLock = Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0 ? questionnaire.Root.Element("Findings").Attribute("Finalize").Value : "false")
                    || Convert.ToBoolean(questionnaire.Root.Elements("Followup").Attributes().Count() > 0 ? questionnaire.Root.Element("Followup").Attribute("Finalize").Value : "false");
                pnlFindings.Enabled = !isLock;

                pnlFindings.Controls.Clear();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                lblSiteName.Text = siteBO.Name;

                findingTable = new Table();
                findingTable.ID = "findingTable";
                findingTable.ClientIDMode = ClientIDMode.Static;
                pnlFindings.Controls.Add(findingTable);

                LoadHeader();
                GenerateQuestionaire();

                message.Clear("");
            }
            else
                message.Info("Questionnaire against the selected site doesn't exist.");
        }
        catch (Exception exception)
        {
            message.Error("An error occured while generating the layout. Please try again!");
            throw exception;
        }
    }

    protected void LoadHeader()
    {
        try
        {
            _tableRow = new TableRow();
            _tableRow.ClientIDMode = ClientIDMode.Static;

            for (int ColumnIndex = 1; ColumnIndex <= DEFAULT_TOTAL_COLUMNS; ColumnIndex++)
            {
                _tableCell = new TableCell();
                _tableCell.ID = "headerCell" + Convert.ToString(ColumnIndex);
                _tableCell.CssClass = "header";
                _tableCell.ClientIDMode = ClientIDMode.Static;

                switch (ColumnIndex)
                {
                    case 1:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS1;
                        _tableCell.Width = 126;
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Width = 35;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Width = 100;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Width = 150;
                        break;

                    case 5:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS5;
                        _tableCell.Width = 80;
                        break;

                    case 6:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS6;
                        _tableCell.Width = 140;
                        break;

                    case 7:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS7;
                        _tableCell.Width = 70;
                        break;

                    default:
                        break;
                }

                _tableRow.Controls.Add(_tableCell);

            }
            findingTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void GenerateQuestionaire()
    {
        try
        {
            highRiskCount = 0;
            mediumRiskCount = 0;
            lowRiskCount = 0;

            for (int riskIndex = 1; riskIndex <= 2; riskIndex++)
            {
                AddRiskHeader(riskIndex);

                for (int index = 1; index <= 2; index++)
                {
                    AddElementsGroups(riskIndex.ToString(), index.ToString());
                    AddElementsValues(riskIndex.ToString(), index.ToString());
                }
            }

            lblHighRisks.Text = highRiskCount + " High Risks Found";
            lblMediumRisks.Text = mediumRiskCount + " Medium Risks Found";
            lblLowRisks.Text = lowRiskCount + " Low Risks Found";
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void AddElementsGroups(string riskIndex, string index)
    {
        try
        {
            string title = index == "1" ? "People & Process" : "Technology";
            string id = riskIndex + index;
            Label _label;
            Label _whiteSpaceLabel;
            Image _image;

            _tableRow = new TableRow();
            _tableCell = new TableCell();

            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:SRAtoggle('" + id + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgElement" + id;
            _image.ImageUrl = "../Themes/Images/Plus-1.png";
            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp; &nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = title;
            _label.Width = 700;
            _label.CssClass = "element-label";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            findingTable.Controls.Add(_tableRow);

            title = title == "People & Process" ? "Process" : title;
            AddElementsTable(title, riskIndex, index);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddRiskHeader(int riskIndex)
    {
        try
        {
            string title = riskIndex == 1 ? "High & Medium Risks" : "Low Risks";
            Label _label;
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            _label = new Label();
            _label.Text = title;
            _label.Width = 725;
            _label.CssClass = "risk-header";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            findingTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsTable(string title, string riskIndex, string index)
    {
        try
        {
            string elementSequence = riskIndex + index;

            elementTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + elementSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;
            elementTable.CssClass = DEFAULT_ELEMENT_TABLE_CSS_CLASS;
            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;

            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            findingTable.Controls.Add(_tableRow);

            AddStandards(title, riskIndex, index);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandards(string title, string riskIndex, string index)
    {
        try
        {
            foreach (XElement element in questionnaire.Root.Elements(title).Elements("Element"))
            {
                foreach (XElement standard in element.Elements("Standard"))
                {
                    AddStandardRow(element.Attribute("sequence").Value, standard, riskIndex, index, title);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(string elementId, XElement standard, string riskIndex, string index, string title)
    {
        try
        {
            string likelihood = standard.Attribute("Likelihood").Value;
            string impact = standard.Attribute("Impact").Value;
            string riskRating = Util.CalculateRiskRating(likelihood, impact);
            Label _label;
            Image _image;
            TextBox _textbox;

            if ((riskIndex == "1" && (riskRating == "highRisk" || riskRating == "mediumRisk")) || (riskIndex == "2" && (riskRating == "lowRisk")))
            {

                highRiskCount = riskRating == "highRisk" ? ++highRiskCount : highRiskCount;
                mediumRiskCount = riskRating == "mediumRisk" ? ++mediumRiskCount : mediumRiskCount;
                lowRiskCount = riskRating == "lowRisk" ? ++lowRiskCount : lowRiskCount;

                string standardId = standard.Attribute("sequence").Value;
                string threatVulnerability = standard.Attribute("ThreatVulnerability").Value;
                string recommendedControlMeasure = standard.Attribute("RecommendedControlMeasure").Value;

                string editablestuff = standard.Attribute("Editablestuff").Value;
                string sourceId = standard.Attribute("SourceId").Value;
                string existingControl = string.Empty;

                IEnumerable<XElement> peopleProcess = from el in questionnaire.Root.Elements("Screening").Elements("Element").Elements("Standard")
                                                      where (string)el.Attribute("Id") == sourceId
                                                      select el;

                foreach (XElement screeningElement in peopleProcess)
                {
                    title = title == "Process" ? "PeopleOrProcess" : title;
                    existingControl = screeningElement.Attribute(title).Value;
                }

                _tableRow = new TableRow();
                string elementSequence = riskIndex + index;
                string id = elementSequence + elementId + standardId;

                //Threat Vulnerability

                _label = new Label();
                _label.ID = DEFAULT_THREAT_LABEL_ID_PREFIX + id;
                _label.Text = threatVulnerability;
                _label.ClientIDMode = ClientIDMode.Static;
                _label.CssClass = "findings-riskfound";
                _label.Width = 126;

                _tableCell = new TableCell();
                _tableCell.Width = 126;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                //Risk Rating

                _image = new Image();
                _image.ClientIDMode = ClientIDMode.Static;
                _image.ID = "imgRiskRating" + id;
                _image.ImageUrl = "../Themes/Images/" + riskRating + ".png";
                _image.CssClass = "findings-riskrating";

                _tableCell = new TableCell();
                _tableCell.Width = 35;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_image);
                _tableRow.Controls.Add(_tableCell);

                //Existing Control

                _label = new Label();
                _label.ID = DEFAULT_EXISTING_CONTROL_LABEL_ID_PREFIX + id;
                _label.Text = existingControl;
                _label.ClientIDMode = ClientIDMode.Static;
                _label.CssClass = "findings-existing";
                _label.Width = 102;

                _tableCell = new TableCell();
                _tableCell.Width = 102;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                //Recommended Control Measure

                _label = new Label();
                _label.ID = DEFAULT_RECOMMENDED_LABEL_ID_PREFIX + id;
                _label.Text = recommendedControlMeasure + "<br /><br />" + editablestuff;
                _label.ClientIDMode = ClientIDMode.Static;
                _label.CssClass = "process-recommended";
                _label.Width = 150;

                _tableCell = new TableCell();
                _tableCell.Width = 150;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                //Owner

                _label = new Label();
                _label.ID = DEFAULT_OWNER_LABEL_ID_PREFIX + id;
                _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_OWNER_TEXTBOX_ID_PREFIX + id + "');");
                _label.ClientIDMode = ClientIDMode.Static;
                _label.CssClass = "findings-owner";
                _label.Width = 80;
                _label.ToolTip = "Click here to add Owner";
                _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);

                _textbox = new TextBox();
                _textbox.CssClass = "findings-owner";
                _textbox.ID = DEFAULT_OWNER_TEXTBOX_ID_PREFIX + id;
                _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
                _textbox.Style.Add("display", "none");
                _textbox.ClientIDMode = ClientIDMode.Static;
                _textbox.Width = 75;
                _textbox.Height = 14;

                _tableCell.Controls.Add(_textbox);
                _tableRow.Controls.Add(_tableCell);


                //Remediation Steps

                _label = new Label();
                _label.ID = DEFAULT_REMEDIATION_STEPS_LABEL_ID_PREFIX + id;
                _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_REMEDIATION_STEPS_TEXTBOX_ID_PREFIX + id + "');");
                _label.ClientIDMode = ClientIDMode.Static;
                _label.CssClass = "findings-remediation";
                _label.Width = 130;
                _label.ToolTip = "Click here to add Remediation Steps";
                _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);

                _textbox = new TextBox();
                _textbox.CssClass = "findings-remediation";
                _textbox.ID = DEFAULT_REMEDIATION_STEPS_TEXTBOX_ID_PREFIX + id;
                _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
                _textbox.Style.Add("display", "none");
                _textbox.ClientIDMode = ClientIDMode.Static;
                _textbox.Width = 125;
                _textbox.Height = 14;

                _tableCell.Controls.Add(_textbox);
                _tableRow.Controls.Add(_tableCell);

                //Target Date                
                _textbox = new TextBox();
                _textbox.ID = DEFAULT_DATE_PICKER_TEXTBOX_ID_PREFIX + id;
                _textbox.CssClass = "datepicker";
                _textbox.ClientIDMode = ClientIDMode.Static;
                _textbox.Attributes.Add("onclick", "javascript:DatePicker('" + _textbox.ID + "');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_textbox);
                _tableRow.Controls.Add(_tableCell);

                elementTable.Controls.Add(_tableRow);

            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsValues(string riskIndex, string index)
    {
        try
        {
            string title = index == "1" ? "Process" : "Technology";
            string elementSequence = riskIndex.ToString() + index.ToString();
            Label _label;
            TextBox _textbox;

            foreach (XElement Element in questionnaire.Root.Elements(title).Elements("Element"))
            {
                foreach (XElement standard in Element.Elements("Standard"))
                {
                    string likelihood = standard.Attribute("Likelihood").Value;
                    string impact = standard.Attribute("Impact").Value;
                    string riskRating = Util.CalculateRiskRating(likelihood, impact);

                    if ((riskIndex == "1" && (riskRating == "highRisk" || riskRating == "mediumRisk")) || (riskIndex == "2" && (riskRating == "lowRisk")))
                    {
                        string elementId = Element.Attribute("sequence").Value;
                        string standardId = standard.Attribute("sequence").Value;

                        string id = elementSequence + elementId + standardId;
                        string sourceId = standard.Attribute("SourceId").Value;

                        string ownerValue = string.Empty;
                        string remediationStepsValue = string.Empty;
                        string targetDateValue = string.Empty;

                        IEnumerable<XElement> findings = from el in questionnaire.Root.Elements("Findings").Elements("Standard")
                                                         where (string)el.Attribute("SourceId") == elementSequence + sourceId
                                                         select el;

                        foreach (XElement findingsElement in findings)
                        {
                            ownerValue = findingsElement.Attribute("Owner").Value;
                            remediationStepsValue = findingsElement.Attribute("RemediationSteps").Value;
                            targetDateValue = findingsElement.Attribute("TargetDate").Value;
                        }


                        // Owner

                        _label = (Label)pnlFindings.FindControl(DEFAULT_OWNER_LABEL_ID_PREFIX + id);
                        _label.Text = ownerValue;

                        _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_OWNER_TEXTBOX_ID_PREFIX + id);
                        _textbox.Text = ownerValue;

                        // Remediation Steps

                        _label = (Label)pnlFindings.FindControl(DEFAULT_REMEDIATION_STEPS_LABEL_ID_PREFIX + id);
                        _label.Text = remediationStepsValue;

                        _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_REMEDIATION_STEPS_TEXTBOX_ID_PREFIX + id);
                        _textbox.Text = remediationStepsValue;

                        // Target Date

                        _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_DATE_PICKER_TEXTBOX_ID_PREFIX + id);
                        _textbox.Text = targetDateValue;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void SaveFinding()
    {
        try
        {
            if (Page.IsPostBack)
            {
                if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
                {
                    string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                    questionnaire = storedQuestionnaire = XDocument.Parse(RecievedQuestionnaire);

                    GenerateControlsIds();

                    SaveSRAFinding();

                    QuestionBO _questionBO = new QuestionBO();
                    _questionBO.SaveFilledQuestionnaire((int)enQuestionnaireType.SRAQuestionnaire, ProjectUsageId,SiteId, storedQuestionnaire.Root, Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void GenerateControlsIds()
    {
        try
        {
            findingTable = new Table();
            findingTable.ID = "findingTable";
            findingTable.ClientIDMode = ClientIDMode.Static;
            pnlFindings.Controls.Add(findingTable);

            for (int riskIndex = 1; riskIndex <= 2; riskIndex++)
            {
                for (int index = 1; index <= 2; index++)
                {
                    string title = index == 1 ? "Process" : "Technology";
                    AddElementsTable(title, riskIndex.ToString(), index.ToString());
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void SaveSRAFinding()
    {
        try
        {
            if (storedQuestionnaire.Root.Elements("Findings").Elements().Count() > 0)
                storedQuestionnaire.Root.Elements("Findings").Elements().Remove();


            if (storedQuestionnaire.Root.Elements("Findings").Count() == 0)
            {
                XElement _findingElement = new XElement("Findings");
                storedQuestionnaire.Root.Add(_findingElement);
            }

            SaveStandards();

            if (chbFinalize.Checked)
            {
                if (!(storedQuestionnaire.Root.Element("Findings").Attributes().Count() == 2))
                {
                    storedQuestionnaire.Root.Element("Findings").Attributes().Remove();

                    XAttribute finalize = new XAttribute("Finalize", chbFinalize.Checked);
                    storedQuestionnaire.Root.Element("Findings").Add(finalize);

                    UserBO _userBO = new UserBO();
                    string userDetails = _userBO.GetUserDetailsByUserId(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));

                    XAttribute completedBy = new XAttribute("Completedby", userDetails);
                    storedQuestionnaire.Root.Element("Findings").Add(completedBy);
                }
            }
            else
            {
                if (storedQuestionnaire.Root.Element("Findings").HasAttributes)
                {
                    storedQuestionnaire.Root.Element("Findings").Attributes().Remove();
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void SaveStandards()
    {
        try
        {
            for (int riskIndex = 1; riskIndex <= 2; riskIndex++)
            {
                for (int index = 1; index <= 2; index++)
                {
                    string title = index == 1 ? "Process" : "Technology";

                    foreach (XElement element in storedQuestionnaire.Root.Elements(title).Elements("Element"))
                    {
                        foreach (XElement standard in element.Elements("Standard"))
                        {
                            SaveStandardValues(element.Attribute("sequence").Value, standard, riskIndex.ToString(), index.ToString(), title);
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void SaveStandardValues(string elementId, XElement standard, string riskIndex, string index, string title)
    {
        try
        {
            string likelihood = standard.Attribute("Likelihood").Value;
            string impact = standard.Attribute("Impact").Value;
            string riskRating = Util.CalculateRiskRating(likelihood, impact);
            TextBox _textbox;

            if ((riskIndex == "1" && (riskRating == "highRisk" || riskRating == "mediumRisk")) || (riskIndex == "2" && (riskRating == "lowRisk")))
            {
                string standardId = standard.Attribute("sequence").Value;
                string elementSequence = riskIndex + index;

                string id = elementSequence + elementId + standardId;
                string sourceId = standard.Attribute("SourceId").Value;

                XElement _standard = new XElement("Standard");
                storedQuestionnaire.Root.Element("Findings").Add(_standard);

                XAttribute source = new XAttribute("SourceId", elementSequence + sourceId);

                //Owner

                _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_OWNER_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                XAttribute owner = new XAttribute("Owner", _textbox.Text);

                //Remediation Steps

                _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_REMEDIATION_STEPS_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                XAttribute remediationSteps = new XAttribute("RemediationSteps", _textbox.Text);

                //Target Date

                _textbox = (TextBox)pnlFindings.FindControl(DEFAULT_DATE_PICKER_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                XAttribute targetDate = new XAttribute("TargetDate", _textbox.Text);

                storedQuestionnaire.Root.Element("Findings").Elements("Standard").Last().Add(source, owner, remediationSteps, targetDate);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private void SetFormValue(Control control)
    {
        if (!Request.Form.HasKeys())
            return;

        string value = Request.Form[control.UniqueID];
        if (value != null)
        {
            if (control is TextBox)
                (control as TextBox).Text = value;
        }
    }

    private List<SRAFindingsDetails> GenerateFindingsList()
    {
        try
        {
            List<SRAFindingsDetails> lstSRAFindingsDetails = new List<SRAFindingsDetails>();
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                highRiskCount = 0;
                mediumRiskCount = 0;
                lowRiskCount = 0;

                for (int riskIndex = 1; riskIndex <= 2; riskIndex++)
                {
                    for (int index = 1; index <= 2; index++)
                    {
                        string title = index == 1 ? "Process" : "Technology";

                        foreach (XElement element in questionnaire.Root.Elements(title).Elements("Element"))
                        {
                            foreach (XElement standard in element.Elements("Standard"))
                            {
                                string likelihood = standard.Attribute("Likelihood").Value;
                                string impact = standard.Attribute("Impact").Value;
                                string riskRating = Util.CalculateRiskRating(likelihood, impact);


                                if ((riskIndex == 1 && (riskRating == "highRisk" || riskRating == "mediumRisk")) || (riskIndex == 2 && (riskRating == "lowRisk")))
                                {
                                    highRiskCount = riskRating == "highRisk" ? ++highRiskCount : highRiskCount;
                                    mediumRiskCount = riskRating == "mediumRisk" ? ++mediumRiskCount : mediumRiskCount;
                                    lowRiskCount = riskRating == "lowRisk" ? ++lowRiskCount : lowRiskCount;

                                    string elementSequence = riskIndex.ToString() + index.ToString();
                                    string sourceId = standard.Attribute("SourceId").Value;
                                    string existingControl = string.Empty;

                                    string ownerValue = string.Empty;
                                    string remediationStepsValue = string.Empty;
                                    string targetDateValue = string.Empty;

                                    IEnumerable<XElement> peopleProcess = from el in questionnaire.Root.Elements("Screening").Elements("Element").Elements("Standard")
                                                                          where (string)el.Attribute("Id") == sourceId
                                                                          select el;

                                    foreach (XElement screeningElement in peopleProcess)
                                    {
                                        title = title == "Process" ? "PeopleOrProcess" : title;
                                        existingControl = screeningElement.Attribute(title).Value;
                                    }


                                    IEnumerable<XElement> findings = from el in questionnaire.Root.Elements("Findings").Elements("Standard")
                                                                     where (string)el.Attribute("SourceId") == elementSequence + sourceId
                                                                     select el;

                                    foreach (XElement findingsElement in findings)
                                    {
                                        ownerValue = findingsElement.Attribute("Owner").Value;
                                        remediationStepsValue = findingsElement.Attribute("RemediationSteps").Value;
                                        targetDateValue = findingsElement.Attribute("TargetDate").Value;
                                    }


                                    _sraFindingsDetails = new SRAFindingsDetails();
                                    _sraFindingsDetails.RiskType = riskIndex == 1 ? "High & Medium Risks" : riskIndex == 2 ? "Low Risks" : "";
                                    _sraFindingsDetails.RiskTitle = title == "PeopleOrProcess" ? "People & Process" : title;

                                    _sraFindingsDetails.ThreatVulnerability = standard.Attribute("ThreatVulnerability").Value;
                                    _sraFindingsDetails.RiskRating = riskRating;
                                    _sraFindingsDetails.ExistingControlMeasuresApplied = existingControl;
                                    _sraFindingsDetails.RecommendedControlMeasure = standard.Attribute("RecommendedControlMeasure").Value + "\n" + standard.Attribute("Editablestuff").Value;

                                    _sraFindingsDetails.Owner = ownerValue;
                                    _sraFindingsDetails.RemediationSteps = remediationStepsValue;
                                    _sraFindingsDetails.Date = targetDateValue;

                                    lstSRAFindingsDetails.Add(_sraFindingsDetails);

                                }
                            }
                        }
                    }
                }
            }
            return lstSRAFindingsDetails;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void GenerateReport(bool isSRACopy)
    {
        try
        {
            List<SRAFindingsDetails> lstSRAFindingsDetails = GenerateFindingsList();

            Logger.PrintInfo("SRA Findings Report generation: Process start.");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            Logger.PrintDebug("Declaring parameters to report.");

            SiteBO siteBO = new SiteBO();
            siteBO.GetSiteBySiteId(SiteId);
            string siteName = siteBO.Name;

            UserAccountBO userAccountBO = new UserAccountBO();
            List<string> userDetails = userAccountBO.GetUserInformation(Convert.ToInt32(Session["UserApplicationId"]));

            if (Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0))            
                if (questionnaire.Root.Element("Findings").Attributes().Count() == 2)
                    lblFindUserInfo.Text = questionnaire.Root.Element("Findings").Attribute("Completedby").Value;
                else
                    lblFindUserInfo.Text = "";           

            ReportParameter paramSiteName = new ReportParameter("paramSiteName", siteName);
            //ReportParameter paramDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
            ReportParameter paramDate = new ReportParameter("paramDate", System.DateTime.Now.ToString());
            ReportParameter paramTitle = new ReportParameter("paramTitle", DEFAULT_REPORT_TITLE);
            ReportParameter paramDateTitle = new ReportParameter("paramDateTitle", "Target Date");

            ReportParameter paramLogo = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter paramHighRisk = new ReportParameter("paramHighRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\highRisk.png");
            ReportParameter paramMediumRisk = new ReportParameter("paramMediumRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\mediumRisk.png");
            ReportParameter paramLowRisk = new ReportParameter("paramLowRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\lowRisk.png");

            ReportParameter paramCompletedBy = new ReportParameter("paramCompletedBy", lblFindUserInfo.Text);
            ReportParameter paramHighRiskFound = new ReportParameter("paramHighRiskFound", highRiskCount + " High Risks Found");
            ReportParameter paramMediumRiskFound = new ReportParameter("paramMediumRiskFound", mediumRiskCount + " Medium Risks Found");
            ReportParameter paramLowRiskFound = new ReportParameter("paramLowRiskFound", lowRiskCount + " Low Risks Found");
            ReportParameter paramUserDetails = new ReportParameter("paramUserDetails", userDetails[2] + " " + userDetails[3]);

            ReportParameter[] param = { paramSiteName, paramDate, paramTitle, paramDateTitle, paramLogo, paramHighRisk, paramMediumRisk, paramLowRisk,
                                      paramCompletedBy, paramHighRiskFound, paramMediumRiskFound, paramLowRiskFound,paramUserDetails};

            // Create Datasource to report
            Logger.PrintDebug("Set data source to report.");

            ReportDataSource rptDataSource = new ReportDataSource("SRAFindingsDS", lstSRAFindingsDetails);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rptDataSource);

            viewer.LocalReport.DisplayName = DEFAULT_REPORT_TITLE;
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptSRAFindings.rdlc";
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();

            // Assign parameter to report after assigned the data source
            Logger.PrintDebug("Set report parameters.");
            viewer.LocalReport.SetParameters(param);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Generating PDF Report
            Logger.PrintDebug("Generating PDF report.");
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            // Getting file path to db
            siteName = siteName.Replace(",", "").Replace("//", "/").Replace("/", "");
            string siteNameMark = " " + "(" + siteName + ")";
            string path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, false);

            if (isSRACopy)
                path = Util.GetTempPdfPath(PracticeId, "5");

            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /*0= Destination path to store the pdf on server*/
            string savingPath = pathList[1]; /*1= saving path to store the file location in database */

            // Save file on server*/
            Logger.PrintInfo("Report saving on " + destinationPath + ".");
            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");

            if (!isSRACopy)
            {
                PrintBO print = new PrintBO();
                print.SaveSRAData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, userApplicationId, practiceId, DEFAULT_DOC_CONTENT_TYPE,SectionId,ProjectUsageId);
                Session["FilePath"] = savingPath;
            }

            Logger.PrintInfo("SRA " + DEFAULT_REPORT_TITLE + " Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion


}


