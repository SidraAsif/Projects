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

public partial class SRAProcess : System.Web.UI.UserControl
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
    private SRARiskDetails _sraRiskDetails;

    private int userApplicationId;
    private string userType;
    private int practiceId;

    private bool isLock;
    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 8;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Threat Vulnerability";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Recommended Control Measure";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Existing Control";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Existing Control Effectiveness";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Exposure Potential";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Likelihood";
    private const string DEFAULT_HEADER_PARENT_POS7 = "Impact";
    private const string DEFAULT_HEADER_PARENT_POS8 = "Risk Rating";

    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "tableElement";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "element-table";

    private const string DEFAULT_THREAT_LABEL_ID_PREFIX = "lblThreatVulnerability";
    private const string DEFAULT_RECOMMENDED_LABEL_ID_PREFIX = "lblRecommendedControlMeasure";
    private const string DEFAULT_EDITABLE_LABEL_ID_PREFIX = "lblEditable";
    private const string DEFAULT_EXISTING_CONTROL_LABEL_ID_PREFIX = "lblExistingControl";

    private const string DEFAULT_EDITABLE_TEXTBOX_ID_PREFIX = "txtboxEditable";
    private const string DEFAULT_EXISTING_CONTROL_DDL_ID_PREFIX = "ddlExistingControlEffectiveness";
    private const string DEFAULT_LIKELIHOOD_DDL_ID_PREFIX = "ddlLikelihood";
    private const string DEFAULT_IMPACT_DDL_ID_PREFIX = "ddlImpact";
    private const string DEFAULT_COMMENTS_HIDDENFIELD_ID_PREFIX = "hdnComments";

    private readonly string[] DEFAULT_EFFECTIVE_OPTIONS = { "Effective", "Partially Effective", "Not Effective" };
    private readonly string[] DEFAULT_LIKELIHOOD_OPTIONS = { "Very Likely", "Likely", "Not Likely" };
    private readonly string[] DEFAULT_IMPACT_OPTIONS = { "High", "Medium", "Low" };

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "People And Processes Risks";

    #endregion

    #region CONTROLS
    private Table processTable;
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
            hdnUsrName.Value = Session["UserName"].ToString();
        }
        else
        {
            SessionHandling sessionHandling = new SessionHandling();
            sessionHandling.ClearSession();
            Response.Redirect("~/Account/Login.aspx");
        }
    }

    protected void btnPrintProcess_Click(object sender, EventArgs e)
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

                isLock = Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0 ? questionnaire.Root.Element("Findings").Attribute("Finalize").Value : "false")
                    || Convert.ToBoolean(questionnaire.Root.Elements("Followup").Attributes().Count() > 0 ? questionnaire.Root.Element("Followup").Attribute("Finalize").Value : "false");
                pnlProcess.Enabled = !isLock;

                pnlProcess.Controls.Clear();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                lblSiteName.Text = siteBO.Name;

                processTable = new Table();
                processTable.ID = "processTable";
                processTable.ClientIDMode = ClientIDMode.Static;
                pnlProcess.Controls.Add(processTable);

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
                        _tableCell.Width = 115;
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Width = 170;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Width = 100;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Width = 90;
                        break;

                    case 5:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS5;
                        _tableCell.Width = 50;
                        break;

                    case 6:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS6;
                        _tableCell.Width = 70;
                        break;

                    case 7:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS7;
                        _tableCell.Width = 70;
                        break;

                    case 8:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS8;
                        _tableCell.Width = 35;
                        break;

                    default:
                        break;
                }

                _tableRow.Controls.Add(_tableCell);

            }

            processTable.Controls.Add(_tableRow);
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
            foreach (XElement element in questionnaire.Root.Elements("Process").Elements("Element"))
            {
                AddElementsGroup(element);
                AddElementsTable(element);
                AddElementsValues(element);

                _tableRow = new TableRow();
                _tableCell = new TableCell();

                _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
                _tableCell.Height = 10;

                _tableRow.Controls.Add(_tableCell);
                processTable.Controls.Add(_tableRow);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void AddElementsGroup(XElement Element)
    {
        try
        {
            _tableRow = new TableRow();
            _tableCell = new TableCell();
            Label _whiteSpaceLabel;
            Label _label;
            string elementTitle = Element.Attribute("title").Value;
            string elementSequence = Element.Attribute("sequence").Value;

            Image _image = new Image();
            _image.Attributes.Add("onclick", "javascript:SRAtoggle('" + elementSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgElement" + elementSequence;
            _image.ImageUrl = "../Themes/Images/Plus-1.png";
            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp; &nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = elementSequence + ". " + elementTitle;
            _label.Width = 700;
            _label.CssClass = "element-label";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            processTable.Controls.Add(_tableRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsTable(XElement Element)
    {
        try
        {
            string elementSequence = Element.Attribute("sequence").Value;

            elementTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + elementSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;
            elementTable.CssClass = DEFAULT_ELEMENT_TABLE_CSS_CLASS;
            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;

            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            processTable.Controls.Add(_tableRow);

            AddStandards(Element);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandards(XElement Element)
    {
        try
        {
            foreach (XElement standard in Element.Elements("Standard"))
            {
                AddStandardRow(Element.Attribute("sequence").Value, standard);
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(string elementId, XElement standard)
    {
        try
        {
            string standardId = standard.Attribute("sequence").Value;
            string threatVulnerability = standard.Attribute("ThreatVulnerability").Value;
            string recommendedControlMeasure = standard.Attribute("RecommendedControlMeasure").Value;
            string editablestuff = standard.Attribute("Editablestuff").Value;
            string sourceId = standard.Attribute("SourceId").Value;
            string existingControlEffectiveness = standard.Attribute("ExistingControlEffectiveness").Value;            
            string commentsText = GetCommentsPerStandard(standard.Elements("PrivateNote"));
            Label _label;
            ConsultingUserBO _consultingUserBO = new ConsultingUserBO();
            bool practiceWithConsultant = _consultingUserBO.isPracticeWithConsultant(PracticeId);

            string existingControl = string.Empty;
            string exposurePotential = string.Empty;            

            IEnumerable<XElement> peopleProcess = from el in questionnaire.Root.Elements("Screening").Elements("Element").Elements("Standard")
                                                  where (string)el.Attribute("Id") == sourceId
                                                  select el;


            foreach (XElement screeningElement in peopleProcess)
            {
                existingControl = screeningElement.Attribute("PeopleOrProcess").Value;
                exposurePotential = screeningElement.Attribute("Response").Value == "Addressed" ? "lowRisk" : (screeningElement.Attribute("Response").Value == "Partially Addressed" ? "mediumRisk" : screeningElement.Attribute("Response").Value == "Not Addressed" ? "highRisk" : "null");
            }

            string likelihood = standard.Attribute("Likelihood").Value;
            string impact = standard.Attribute("Impact").Value;
            string riskRating = Util.CalculateRiskRating(likelihood, impact);

            _tableRow = new TableRow();
            string id = elementId + standardId;

            //Threat Vulnerability

            _label = new Label();
            _label.ID = DEFAULT_THREAT_LABEL_ID_PREFIX + id;
            _label.Text = threatVulnerability;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "process-threat";
            _label.Width = 115;

            _tableCell = new TableCell();
            _tableCell.Width = 115;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);


            //Recommended Control Measure
            Table recommended = new Table();
            recommended.CellSpacing = 0;
            recommended.CellPadding = 0;
            recommended.Style.Add("padding", "0px");
            recommended.Width = 165;

            _label = new Label();
            _label.ID = DEFAULT_RECOMMENDED_LABEL_ID_PREFIX + id;
            _label.Text = recommendedControlMeasure;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "process-recommended";
            _label.Width = 165;

            _tableCell = new TableCell();
            _tableCell.Width = 165;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            TableRow tableRow = new TableRow();
            tableRow.Controls.Add(_tableCell);
            recommended.Controls.Add(tableRow);

            // Editable Stuff
            _label = new Label();
            _label.ID = DEFAULT_EDITABLE_LABEL_ID_PREFIX + id;
            _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_EDITABLE_TEXTBOX_ID_PREFIX + id + "');");
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "process-editable";
            _label.Width = 165;
            _label.ToolTip = "Click here to add comments";
            _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

            _tableCell = new TableCell();
            _tableCell.Width = 165;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            TextBox _textbox = new TextBox();
            _textbox.CssClass = "process-editable";
            _textbox.ID = DEFAULT_EDITABLE_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 160;
            _textbox.Height = 14;

            _tableCell.Controls.Add(_textbox);

            tableRow = new TableRow();
            tableRow.Controls.Add(_tableCell);
            recommended.Controls.Add(tableRow);

            _tableCell = new TableCell();
            _tableCell.Width = 165;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(recommended);
            _tableRow.Controls.Add(_tableCell);

            //Existing Control

            _label = new Label();
            _label.ID = DEFAULT_EXISTING_CONTROL_LABEL_ID_PREFIX + id;
            _label.Text = existingControl;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "process-existing";
            _label.Width = 100;

            _tableCell = new TableCell();
            _tableCell.Width = 100;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            //Existing Control Effectiveness

            DropDownList _dropDownList = new DropDownList();
            _dropDownList.ID = DEFAULT_EXISTING_CONTROL_DDL_ID_PREFIX + id;
            _dropDownList.ClientIDMode = ClientIDMode.Static;
            _dropDownList.CssClass = "process-existingeffective";
            _dropDownList.Width = 95;

            _dropDownList.Items.Add(new ListItem("Select", "0"));

            int position = 1;
            foreach (string item in DEFAULT_EFFECTIVE_OPTIONS)
            {
                _dropDownList.Items.Add(new ListItem(item, position++.ToString()));
            }

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_dropDownList);

            tableRow = new TableRow();
            tableRow.Controls.Add(_tableCell);

            Table openNotes = new Table();
            openNotes.CellSpacing = 0;
            openNotes.CellPadding = 0;
            openNotes.Style.Add("padding", "0px");

            openNotes.Controls.Add(tableRow);

            if (practiceWithConsultant)
            {
                Literal _literal = new Literal();

                //Open Review Notes
                _literal.Text = "<div id='divOpenReviewNotes" + id + "' class='div-Open-Review-Notes' >";
                _literal.Text += "<a href='javascript:onClickOpenReview(" + id + ");'>Open Review Notes</a>&nbsp;&nbsp;&nbsp;";
                _literal.Text += "<img id='imgOpenReview" + id + "' src='";
                _literal.Text += commentsText == string.Empty ? "../Themes/Images/element-note-empty.png" : "../Themes/Images/element-note.png";
                _literal.Text += "' alt='Review Notes' title ='Review Notes'>";
                _literal.Text += "</div>";

                //Close Review Notes
                _literal.Text += "<div id='divCloseReviewNotes" + id + "' class='div-Close-Review-Notes'>";
                _literal.Text += "<a href='javascript:onClickCloseReview(" + id + ");'>Close Review Notes</a>&nbsp;&nbsp;&nbsp;";
                _literal.Text += "<img id='imgCloseReview" + id + "' src='";
                _literal.Text += commentsText == string.Empty ? "../Themes/Images/element-note-empty.png" : "../Themes/Images/element-note.png";
                _literal.Text += "' alt='Review Notes' title ='Review Notes'>";
                _literal.Text += "<br /><input id='txtComments" + id + "' type='text' class='div-txtbox-comments' ";
                _literal.Text += isLock ? "disabled='disabled'" : string.Empty;                
                _literal.Text += "/>&nbsp;<img onclick=onAddClick(" + id + "); src='../Themes/Images/addcomment.png' alt='Add Comments' title ='Add Comments' ";
                _literal.Text += isLock ? "disabled='disabled'" : string.Empty;
                _literal.Text += "/><br /><div id='divcomments" + id + "' class='div-comments'> ";
                _literal.Text += commentsText;
                _literal.Text += "</div></div>";

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_literal);

                tableRow = new TableRow();
                tableRow.Controls.Add(_tableCell);
                openNotes.Controls.Add(tableRow);
            }

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(openNotes);
            _tableRow.Controls.Add(_tableCell);

            //Exposure Potential

            Image _image = new Image();
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgExposurePotential" + id;
            _image.ImageUrl = "../Themes/Images/" + exposurePotential + ".png";
            _image.CssClass = "process-exposurepotential";

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Width = 50;
            _tableCell.Controls.Add(_image);
            _tableRow.Controls.Add(_tableCell);

            //Likelihood

            _dropDownList = new DropDownList();
            _dropDownList.ID = DEFAULT_LIKELIHOOD_DDL_ID_PREFIX + id;
            _dropDownList.ClientIDMode = ClientIDMode.Static;
            _dropDownList.CssClass = "process-likelihood";
            _dropDownList.Width = 70;
            _dropDownList.Attributes.Add("onchange", "javascript:OnChangeScore('" + id + "');");

            _dropDownList.Items.Add(new ListItem("Select", "0"));

            position = 3;
            foreach (string item in DEFAULT_LIKELIHOOD_OPTIONS)
            {
                _dropDownList.Items.Add(new ListItem(item, position--.ToString()));
            }

            _tableCell = new TableCell();
             _tableCell.Width = 70;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_dropDownList);
            _tableRow.Controls.Add(_tableCell);

            //Impact

            _dropDownList = new DropDownList();            
            _dropDownList.ID = DEFAULT_IMPACT_DDL_ID_PREFIX + id;
            _dropDownList.ClientIDMode = ClientIDMode.Static;
            _dropDownList.CssClass = "process-impact";
            _dropDownList.Width = 70;
            _dropDownList.Attributes.Add("onchange", "javascript:OnChangeScore('" + id + "');");

            _dropDownList.Items.Add(new ListItem("Select", "0"));

            position = 3;
            foreach (string item in DEFAULT_IMPACT_OPTIONS)
            {
                _dropDownList.Items.Add(new ListItem(item, position--.ToString()));
            }

            _tableCell = new TableCell();
            _tableCell.Width = 70;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_dropDownList);
            _tableRow.Controls.Add(_tableCell);

            //Risk Rating

            _image = new Image();
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgRiskRating" + id;
            _image.ImageUrl = "../Themes/Images/" + riskRating + ".png";
            _image.CssClass = "process-riskrating";

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Width = 35;
            _tableCell.Controls.Add(_image);
            _tableRow.Controls.Add(_tableCell);

            //Hidden Comments

            HiddenField _hdnField = new HiddenField();
            _hdnField.Value = string.Empty;
            _hdnField.ID = DEFAULT_COMMENTS_HIDDENFIELD_ID_PREFIX + id;
            _hdnField.ClientIDMode = ClientIDMode.Static;

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_hdnField);
            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsValues(XElement Element)
    {
        try
        {
            foreach (XElement standard in Element.Elements("Standard"))
            {

                string elementId = Element.Attribute("sequence").Value;
                string standardId = standard.Attribute("sequence").Value;
                TextBox _textbox;
                Label _label;
                DropDownList _dropDownList;
                string id = elementId + standardId;

                // Editable Stuff

                _label = (Label)pnlProcess.FindControl(DEFAULT_EDITABLE_LABEL_ID_PREFIX + id);
                _label.Text = standard.Attribute("Editablestuff").Value;

                _textbox = (TextBox)pnlProcess.FindControl(DEFAULT_EDITABLE_TEXTBOX_ID_PREFIX + id);
                _textbox.Text = standard.Attribute("Editablestuff").Value;

                //Existing Control Effectiveness

                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_EXISTING_CONTROL_DDL_ID_PREFIX + id);
                _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(standard.Attribute("ExistingControlEffectiveness").Value));

                //Likelihood

                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_LIKELIHOOD_DDL_ID_PREFIX + id);
                _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(standard.Attribute("Likelihood").Value));

                //Impact

                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_IMPACT_DDL_ID_PREFIX + id);
                _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(standard.Attribute("Impact").Value));
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void SaveProcess()
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

                    foreach (XElement element in storedQuestionnaire.Root.Elements("Process").Elements("Element"))
                    {
                        SaveSRAProcess(element);
                    }

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
            processTable = new Table();
            processTable.ID = "processTable";
            processTable.ClientIDMode = ClientIDMode.Static;
            pnlProcess.Controls.Add(processTable);

            foreach (XElement element in storedQuestionnaire.Root.Elements("Process").Elements("Element"))
            {
                AddElementsTable(element);
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void SaveSRAProcess(XElement element)
    {
        try
        {
            string elementSequence = element.Attribute("sequence").Value;

            foreach (XElement standard in element.Elements("Standard"))
            {
                string standardId = standard.Attribute("sequence").Value;
                string id = elementSequence + standardId;
                TextBox _textbox;
                DropDownList _dropDownList;
                //Editablest_textboxuff                
                _textbox = (TextBox)pnlProcess.FindControl(DEFAULT_EDITABLE_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                standard.Attribute("Editablestuff").Value = _textbox.Text;

                //ExistingControlEffectiveness               
                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_EXISTING_CONTROL_DDL_ID_PREFIX + id);
                SetFormValue(_dropDownList);
                standard.Attribute("ExistingControlEffectiveness").Value = _dropDownList.SelectedItem.Text != "Select" ? _dropDownList.SelectedItem.Text : string.Empty;

                //Likelihood                
                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_LIKELIHOOD_DDL_ID_PREFIX + id);
                SetFormValue(_dropDownList);
                standard.Attribute("Likelihood").Value = _dropDownList.SelectedItem.Text != "Select" ? _dropDownList.SelectedItem.Text : string.Empty;

                //Impact                
                _dropDownList = (DropDownList)pnlProcess.FindControl(DEFAULT_IMPACT_DDL_ID_PREFIX + id);
                SetFormValue(_dropDownList);
                standard.Attribute("Impact").Value = _dropDownList.SelectedItem.Text != "Select" ? _dropDownList.SelectedItem.Text : string.Empty;

                //Comments
                HiddenField _hdnField = (HiddenField)pnlProcess.FindControl(DEFAULT_COMMENTS_HIDDENFIELD_ID_PREFIX + id);
                SetFormValue(_hdnField);

                if (_hdnField.Value != string.Empty)
                {
                    foreach (string singleComment in _hdnField.Value.Split('|'))
                    {
                        saveCommentsPerStandard(standard, singleComment);
                    }
                }

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
            if (control is DropDownList)
                (control as DropDownList).SelectedValue = value;

            if (control is TextBox)
                (control as TextBox).Text = value;

            if (control is HiddenField)
                (control as HiddenField).Value = value;
        }
    }

    protected string GetCommentsPerStandard(IEnumerable<XElement> privateNotes)
    {
        try
        {
            string comments = string.Empty;
            foreach (XElement notes in privateNotes.Elements("Note"))
            {
                comments += notes.Attribute("user").Value + " ";
                comments += notes.Attribute("date").Value + "<br />";
                comments += notes.Value;
                comments += "<br /><br />";
            }
            return comments;

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void saveCommentsPerStandard(XElement standard, string comments)
    {
        try
        {
            if (comments != string.Empty)
            {
                int lastNoteSequence = 0;

                if (!standard.HasElements)
                {
                    XElement _privateNote = new XElement("PrivateNote");
                    standard.Add(_privateNote);
                }
                else
                {
                    lastNoteSequence = standard.Element("PrivateNote").Elements().Count();
                }

                XElement _note = new XElement("Note");
                standard.Element("PrivateNote").Add(_note);

                string[] commentAttributes = comments.Split(',');
                XAttribute sequence = new XAttribute("sequence", lastNoteSequence + 1);
                XAttribute date = new XAttribute("date", commentAttributes[1]);
                XAttribute user = new XAttribute("user", commentAttributes[0]);

                standard.Element("PrivateNote").Elements("Note").Last().Add(sequence, date, user);
                standard.Element("PrivateNote").Elements("Note").Last().Value = commentAttributes[2];
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private List<SRARiskDetails> GenerateProcessList()
    {
        try
        {
            List<SRARiskDetails> lstSRARiskDetails = new List<SRARiskDetails>();
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                foreach (XElement Element in questionnaire.Root.Elements("Process").Elements("Element"))
                {
                    foreach (XElement standard in Element.Elements("Standard"))
                    {
                        string elementId = Element.Attribute("sequence").Value;
                        string standardId = standard.Attribute("sequence").Value;

                        string existingControl = string.Empty;
                        string exposurePotential = string.Empty;

                        IEnumerable<XElement> peopleProcess = from el in questionnaire.Root.Elements("Screening").Elements("Element").Elements("Standard")
                                                              where (string)el.Attribute("Id") == standard.Attribute("SourceId").Value
                                                              select el;


                        foreach (XElement screeningElement in peopleProcess)
                        {
                            existingControl = screeningElement.Attribute("PeopleOrProcess").Value;
                            exposurePotential = screeningElement.Attribute("Response").Value == "Addressed" ? "lowRisk" : (screeningElement.Attribute("Response").Value == "Partially Addressed" ? "mediumRisk" : screeningElement.Attribute("Response").Value == "Not Addressed" ? "highRisk" : "null");
                        }

                        string likelihood = standard.Attribute("Likelihood").Value;
                        string impact = standard.Attribute("Impact").Value;
                        string riskRating = Util.CalculateRiskRating(likelihood, impact);

                        _sraRiskDetails = new SRARiskDetails();
                        _sraRiskDetails.ElementId = Convert.ToInt32(Element.Attribute("sequence").Value);
                        _sraRiskDetails.ElementName = Element.Attribute("title").Value;

                        _sraRiskDetails.ThreatVulnerability = standard.Attribute("ThreatVulnerability").Value;
                        _sraRiskDetails.RecommendedControlMeasure = standard.Attribute("RecommendedControlMeasure").Value + "\n" + standard.Attribute("Editablestuff").Value;

                        _sraRiskDetails.ExistingControl = existingControl;
                        _sraRiskDetails.ExistingControlEffectiveness = standard.Attribute("ExistingControlEffectiveness").Value;
                        _sraRiskDetails.ExposurePotential = exposurePotential;

                        _sraRiskDetails.Likelihood = likelihood;
                        _sraRiskDetails.Impact = impact;
                        _sraRiskDetails.RiskRating = riskRating;

                        lstSRARiskDetails.Add(_sraRiskDetails);

                    }
                }
            }
            return lstSRARiskDetails;
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
            List<SRARiskDetails> lstSRARiskDetails = GenerateProcessList();

            Logger.PrintInfo("SRA Process Report generation: Process start.");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            Logger.PrintDebug("Declaring parameters to report.");

            SiteBO siteBO = new SiteBO();
            siteBO.GetSiteBySiteId(SiteId);
            string siteName = siteBO.Name;

            UserAccountBO userAccountBO = new UserAccountBO();
            List<string> userDetails = userAccountBO.GetUserInformation(Convert.ToInt32(Session["UserApplicationId"]));

            ReportParameter paramSiteName = new ReportParameter("paramSiteName", siteName);
            //ReportParameter paramDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
            ReportParameter paramDate = new ReportParameter("paramDate", System.DateTime.Now.ToString());
            ReportParameter paramTitle = new ReportParameter("paramTitle", DEFAULT_REPORT_TITLE);

            ReportParameter paramLogo = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter paramHighRisk = new ReportParameter("paramHighRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\highRisk.png");
            ReportParameter paramMediumRisk = new ReportParameter("paramMediumRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\mediumRisk.png");
            ReportParameter paramLowRisk = new ReportParameter("paramLowRisk", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\lowRisk.png");
            ReportParameter paramUserDetails = new ReportParameter("paramUserDetails", userDetails[2] + " " + userDetails[3]);

            
            ReportParameter[] param = { paramSiteName, paramDate, paramTitle, paramLogo, paramHighRisk, paramMediumRisk, paramLowRisk, paramUserDetails };

            // Create Datasource to report
            Logger.PrintDebug("Set data source to report.");

            ReportDataSource rptDataSource = new ReportDataSource("SRARiskDS", lstSRARiskDetails);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rptDataSource);

            viewer.LocalReport.DisplayName = DEFAULT_REPORT_TITLE;
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptSRARisk.rdlc";
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
                path = Util.GetTempPdfPath(PracticeId, "3");

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
