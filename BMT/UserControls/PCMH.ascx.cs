#region MODIFICATION_HISTORY
//  ******************************************************************************
//  Module        : AddressDetails
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/26/2011 -12/30/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    02-16-2012      Add conditional images for notes icons (Alter the appearance of all Notes icons if there is content entered. Maybe make them black-and-white or something?) 
//  *******************************************************************************

#endregion

using System;
using System.Collections;
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

public partial class PCMH : System.Web.UI.UserControl
{
    #region PPROPERTIES
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    public int ProjectId { get; set; }
    public string Node { get; set; }
    public string SiteName { get; set; }
    public int SiteId { get; set; }
    public enQuestionSubType PCMHType { get; set; }

    #endregion

    #region CONSTANTS

    #region ********************************WARNING**********************************
    // BE CARE FULL IF YOU WANT TO CHANGE THE VALUE OF CONSTATNS!
    // CONSTANT VALUES ARE ALSO USING IN PCMH SCRIPT FILE
    #endregion

    private const string CONTROL_NAME = "NCQARequirements";
    private const string DEFAULT_QUESTIONNAIRE_TYPE = "NCQA";
    private const string DEFAULT_HEADER_PARENT_POS1 = "#";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Factor";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Factor Met";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Policies/ Process";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Reports/ Logs";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Screenshots/ Examples";
    private const string DEFAULT_HEADER_PARENT_POS7 = "RRWB";
    private const string DEFAULT_HEADER_PARENT_POS8 = "Extra";
    private const string DEFAULT_HEADER_PARENT_POS9 = "Notes";
    private const string DEFAULT_HEADER_PARENT_POS10 = "File";
    private const int DEFAULT_HEADER_CHILD_START = 4;
    private const int DEFAULT_HEADER_CHILD_END = 12;
    private const string DEFAULT_HEADER_CHILD_POS1 = "Req.";
    private const string DEFAULT_HEADER_CHILD_POS2 = "Up.";
    private const int DEFAULT_TOTAL_FACTOR_COLUMNS = 15;

    private readonly string[] DEFAULT_FACTOR_OPTIONS = { "No", "Yes", "NA" };

    private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "tableElement";

    private const string DEFAULT_FACTOR_TABLE_PARENT_ROW_CSS_CLASS = "factor-parentRow";
    private const string DEFAULT_FACTOR_TABLE_ROW_NOTICE_CSS_CLASS = "factor-notice";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "factor-table";
    private const string DEFAULT_FACTOR_TEXTBOX_CSS_CLASS = "factor-textbox";

    private const string DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER = "lblHeader";
    private const string DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_CHILD_HEADER = "lblChildHeader";
    private const string DEFAULT_LABEL_ID_PREFIX_FOR_FACTOR_NOTICE = "lblFactorNotice";

    private const string DEFAULT_LABLE_ID_PREFIX_FOR_FACTOR_DOC = "lblfactorDoc";
    private const string DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC = "txtfactorDoc";
    private const string DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL = "factor-control";

    private const string DEFAULT_NOTE_CHECKBOX_ID_PREFIX = "chkBoxNote";
    private const string DEFAULT_NOTE_TEXTBOX_ID_PREFIX = "txtNote";

    private const string DEFAULT_HYPERLINK_TAG_CSSCLASS = "link";
    private const string HYPERLINK_TAG_CSSCLASS= "unlink";
    private const string DEFAULT_CRITIAL_FACTOR_TOOLTIP = " has been identified as a critical factor and must be met for practices to score higher than 25 percent on this element";
    private const string DEFAULT_FACTOR_TOOLTIP = "has been identified as a critical factor and is required for practices to receive ";
    private const string DEFAULT_FACTOR = " has been designated as a critical factor required to receive more than 25 percent of the available points for this element";
    private const int ELEMENT_SPLIT_INDEX = 1;

    private CorporateElementSubmissionBO corporateElementSubmissionBO;
    #endregion

    #region VARIABLES
    private QuestionBO _questionBO;
    private XDocument _questionnaire;
    private string defaultNCQADescriptionInfo;
    private int questionnaireId;
    private int userId;
    private string calculationRules = string.Empty;
    #endregion

    #region CONTROL
    private Label _label;
    private TextBox _textBox;
    private CheckBox _checkbox;
    private Image _image;
    private HiddenField _hiddenField;
    private HyperLink _hyperLink;

    private Table _PCMHTable;
    #endregion

    #region EVENTS
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            base.Render(writer);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == CONTROL_NAME)
            {
                if (!Page.IsPostBack)
                    Session["hostPath"] = Util.GetHostPath();

                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut");

                GetQuestionnaireByType();
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("An error while loading the Questionnaire.");
        }
    }

    protected void gvFN_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (Page.IsPostBack)
                {
                    //int index = Convert.ToInt32(e.CommandArgument);
                    //UserId = Convert.ToInt32(gvUsers.DataKeys[index].Value);
                    //GetUserByUserId(UserId);
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsPostBack)
            {
                if (Session["NCQAIsEdit"] != null)
                    SavingQuestionnaire();
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void btnElementSave_Click(object sender, EventArgs e)
    {
        try
        {
            SavingEvaluationNotes();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void btnFCNSave_Click(object sender, EventArgs e)
    {
        try
        {
            SavingFactorCriticalNotes();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnFNSave_Click(object sender, EventArgs e)
    {
        try
        {
            SavingFactorPrivateNote();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    #endregion

    #region FUNCTIONS
    public void GetQuestionnaireByType()
    {
        try
        {
            string recievedQuestionnaire = Session["NCQAQuestionnaire"].ToString();

            _questionnaire = XDocument.Parse(recievedQuestionnaire);

            questionnaireId = Convert.ToInt32(Session["NCQAQuestionnaireId"]);

            userId = Convert.ToInt32(Session["UserApplicationId"]);

            GenerateQuestionnaireByType(_questionnaire);

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void GenerateQuestionnaireByType(XDocument QuestionnaireXML)
    {

        try
        {
            if (ProjectId != 0)
            {
                HiddenField hiddenRules = new HiddenField();
                hiddenRules.ID = "hiddenRules" + (int)PCMHType;
                hiddenRules.ClientIDMode = ClientIDMode.Static;

                calculationRules = string.Empty;

                lblSiteInfo.Text = SiteName;
                lblSiteInfo.CssClass = "project-title";

                XElement parentElement = QuestionnaireXML.Root;

                // clear control if exists
                pnlNCQARequirements.Controls.Clear();

                // Add Master table
                _PCMHTable = new Table();
                _PCMHTable.ID = "NCQAPCMHTable";
                _PCMHTable.ClientIDMode = ClientIDMode.Static;

                pnlNCQARequirements.Controls.Add(_PCMHTable);

                string questionnaireType = parentElement.Name.ToString();
                if (questionnaireType == enQuestionnaireType.DetailedQuestionnaire.ToString())
                {
                    int Type = (int)PCMHType;
                    IEnumerable<XElement> Standard = from element in parentElement.Descendants("Standard")
                                                     where (string)element.Attribute("sequence") == Convert.ToString(Type)
                                                     select element;



                    foreach (XElement standard in Standard)
                    {
                        /*Add standards*/
                        string standardTitle = standard.Attribute("title").Value;
                        string standardSequence = standard.Attribute("sequence").Value;
                        AddStandards(standardTitle, standardSequence);

                        /*fetching list of elements*/
                        foreach (XElement element in standard.Elements())
                        {
                            /*Add Elements*/
                            AddElements(element, standardSequence);
                        }
                        
                        break;

                    }
                }
                else { message.Error("System rejected the Questionnaire. Please contact your site Administrator."); }
                hiddenRules.Value = calculationRules;

                // Store Rules against selected PCMH
                TableCell _tableCell = new TableCell();
                TableRow _tableRow = new TableRow();
                _tableCell.Controls.Add(hiddenRules);
                _tableRow.Controls.Add(_tableCell);
                _PCMHTable.Controls.Add(_tableRow);

                Session["NCQAIsEdit"] = "true";
            }
            else
                return;

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void AddStandards(string Title, string Sequence)
    {
        try
        {
            string[] standardTitle = Title.Split(':');

            TableRow _tableRow = new TableRow();
            _tableRow.CssClass = "standard-title";
            TableCell _tableCell = new TableCell();

            _label = new Label();
            _label.ID = Sequence;
            _label.Text = "<div class='standard-title-head'>" + standardTitle[0] + ":</div>" + "<div class='standard-title-desc'>" +
                standardTitle[1] + "</div>";

            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);
            _PCMHTable.Controls.Add(_tableRow);

        }
        catch (Exception exception)
        {

            throw exception;
        }

    }

    protected void AddElements(XElement Element,string StandardSequence)
    {
        try
        {
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();
            Literal _literal = new Literal();

            string docInfoLink = GetBookmark(Element);

            // fetching Element Attribute Values
            string elementTitle = Element.Attribute("title").Value;
            string[] elementSplit = elementTitle.Split(new char[] { ' ',':' });
            string elementId = elementSplit[ELEMENT_SPLIT_INDEX];
            string standardSequence = StandardSequence;
            string enableDisableFactorName = "PCMH" + " " + standardSequence + " " + elementId;
            string elementSequence = Element.Attribute("sequence").Value;
            string elementMustPass = Element.Attribute("mustPass").Value;
            string elementComplete = Element.Attribute("complete").Value;

            // create Element Header
            string functionParams = "('tableElement" + elementSequence + "', 'imgElement" + elementSequence + "');";
            _literal.ID = "literalElement" + elementSequence;
            _literal.Text = "<div id='divElement" + elementSequence + "'>";
            _literal.Text += "<div id='divElementText" + elementSequence + "' class='element-title'>";

            // element note image
            _literal.Text += "<a id='imgElement" + elementSequence + "' onClick=\"toggleElement('" + elementSequence + "');\">";
            _literal.Text += "<img class='toggle-img' src='../Themes/Images/Plus.png'></a>&nbsp;&nbsp;";

            //element info image
            _literal.Text += "<a href=\"" + docInfoLink + "\" \" target=\"_blank\">";
            _literal.Text += "<img src='../Themes/Images/ncqa-info-ico.png' alt='Info' title ='Info'></a>&nbsp;&nbsp;";

            // write Element Title
            if (elementMustPass != "Yes")
                _literal.Text += elementTitle;
            else
                _literal.Text += elementTitle + " [MUST PASS]";

            // create PopUp Title
            string elementPopUpTitle = "Evaluation Note for PCMH " + Convert.ToString((int)PCMHType) + " - Element " +
                DEFAULT_LETTERS[Convert.ToInt32(elementSequence) - 1];

            #region FETCHING_EvaluationNote
            string evaluationNotesText = string.Empty;

            foreach (XElement EvaluationNotes in Element.Elements("EvaluationNotes"))
            {
                evaluationNotesText = EvaluationNotes.Value;
            }
            _hiddenField = new HiddenField();
            _hiddenField.ID = "hiddenelement" + elementSequence + Convert.ToString((int)PCMHType);
            _hiddenField.Value = evaluationNotesText.Trim();
            _hiddenField.ClientIDMode = ClientIDMode.Static;
            _tableCell.Controls.Add(_hiddenField);

            #endregion

            #region ELEMENT_NOTE_PICTURE
            // set image Position
            _literal.Text += "&nbsp;&nbsp;<a id='imgElementNote" + elementSequence;

            // pass the values against current element into popup windows when user click on icon
            corporateElementSubmissionBO = new CorporateElementSubmissionBO();
            if (corporateElementSubmissionBO.IsPracticeCorporate(PracticeId))
            {
                if (corporateElementSubmissionBO.IsSiteCorporate(PracticeId, SiteId))
                {
                    if (corporateElementSubmissionBO.IsNotSubmittedCorporateElement(PracticeId, SiteId, enableDisableFactorName))
                    {
                        _literal.Text += "' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)PCMHType);
                        _literal.Text += "','" + _hiddenField.ID + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)PCMHType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                    else
                    {
                        _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)PCMHType);
                        _literal.Text += "','" + _hiddenField.ID + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)PCMHType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                }
                else
                {
                    if (corporateElementSubmissionBO.IsCorporateElementofNonCorporateSite(PracticeId, SiteId, enableDisableFactorName))
                    {
                        _literal.Text += "' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)PCMHType);
                        _literal.Text += "','" + _hiddenField.ID + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)PCMHType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                    else
                    {
                        _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)PCMHType);
                        _literal.Text += "','" + _hiddenField.ID + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)PCMHType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                }
            }
            else
            {
                _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                _literal.Text += elementSequence + "','" + Convert.ToString((int)PCMHType);
                _literal.Text += "','" + _hiddenField.ID + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)PCMHType) + "' ";
                _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
            }

            if (evaluationNotesText == string.Empty)
                _literal.Text += " src='../Themes/Images/element-note-empty.png'></a>"; // set image for current popup (when no content found)
            else
                _literal.Text += " src='../Themes/Images/element-note.png'></a>"; // set image for current popup (when content found)                       

            #endregion

            _literal.Text += "</div></div><br />";

            _tableCell.Controls.Add(_literal);
            _tableRow.Controls.Add(_tableCell);
            _PCMHTable.Controls.Add(_tableRow);

            // creating New table to display factors of each element 
            Table elementTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + elementSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;
            elementTable.CssClass = DEFAULT_ELEMENT_TABLE_CSS_CLASS;

            // Enable/Disable factor according to Corporate Site
            corporateElementSubmissionBO = new CorporateElementSubmissionBO();
            if (corporateElementSubmissionBO.IsPracticeCorporate(PracticeId))
            {
                if (corporateElementSubmissionBO.IsSiteCorporate(PracticeId, SiteId))
                {
                    if (corporateElementSubmissionBO.IsNotSubmittedCorporateElement(PracticeId, SiteId, enableDisableFactorName))
                    {
                        elementTable.Enabled = false;
                        elementTable.Style.Add("Color", "Gray");
                    }
                }
                else
                {
                    if (corporateElementSubmissionBO.IsCorporateElementofNonCorporateSite(PracticeId, SiteId, enableDisableFactorName))
                    {
                        elementTable.Enabled = false;
                        elementTable.Style.Add("Color", "Gray");
                    }
                }
            }
           
            // Add Factor Table below the Element Row
            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            _PCMHTable.Controls.Add(_tableRow);

            // Add Factors
            AddFactors(Element, elementTable, standardSequence);

            // Mark as Complete status & reviewer note
            string checkBoxControlId = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)PCMHType + elementSequence;
            CheckBox chkBoxElementControl = (CheckBox)pnlNCQARequirements.FindControl(checkBoxControlId);

            string reviewerNotes = string.Empty;
            foreach (XElement reviewerNote in Element.Elements("ReviewerNotes"))
            {
                reviewerNotes = reviewerNote.Value;
            }

            string reviewerNotesControlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)PCMHType + elementSequence;
            TextBox txtreviewerNotes = (TextBox)pnlNCQARequirements.FindControl(reviewerNotesControlId);

            if (txtreviewerNotes != null)
                txtreviewerNotes.Text = reviewerNotes;

            if (chkBoxElementControl != null)
            {
                if (elementComplete == "Yes")
                    chkBoxElementControl.Checked = true;
                else
                    chkBoxElementControl.Checked = false;
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddFactors(XElement Element, Table ElementTable, string StandardSequence)
    {
        try
        {
            string standardSequence = StandardSequence;
            string elementSequence = Element.Attribute("sequence").Value;

            // Row to add the document viewer link
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();
            _tableCell.ColumnSpan = 15;
            _tableCell.HorizontalAlign = HorizontalAlign.Right;
            _tableCell.VerticalAlign = VerticalAlign.Middle;

            // DocViewer link
            string lblDocViewerTitle = "Uploaded Document For PCMH " + (int)PCMHType + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequence) - 1];
            _hyperLink = new HyperLink();
            _hyperLink.ID = "hypDocViewer" + (int)PCMHType + elementSequence;
            _hyperLink.Text = "View Uploaded Documents";
            if (ElementTable.Enabled)
                _hyperLink.CssClass = DEFAULT_HYPERLINK_TAG_CSSCLASS;   
            else
                _hyperLink.CssClass = HYPERLINK_TAG_CSSCLASS;

                _hyperLink.NavigateUrl = "javascript:UploadedDocViewer('" + (int)PCMHType + "','" + elementSequence + "','" + lblDocViewerTitle + "','" +
                    PracticeId + "','" + SiteId + "','" + ProjectId + "');";
         
            _tableCell.Controls.Add(_hyperLink);
            _tableRow.Controls.Add(_tableCell);
            ElementTable.Controls.Add(_tableRow);

            // Row To Create Parent_Header
            _tableRow = new TableRow();
            _tableRow.CssClass = DEFAULT_FACTOR_TABLE_PARENT_ROW_CSS_CLASS;

            // add parent factor header
            AddParentFactorHeader(ElementTable, _tableRow, elementSequence);

            // Add Notice for factors
            _tableRow = new TableRow();
            _tableRow.CssClass = DEFAULT_FACTOR_TABLE_ROW_NOTICE_CSS_CLASS;

            _tableCell = new TableCell();
            _tableCell.ColumnSpan = DEFAULT_TOTAL_FACTOR_COLUMNS;

            _label = new Label();
            _label.ID = DEFAULT_LABEL_ID_PREFIX_FOR_FACTOR_NOTICE + elementSequence;
            _label.Text = Element.Attribute("description").Value;

            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            ElementTable.Controls.Add(_tableRow);

            // fetching Factors from elements
            int TotalFactors = 0;
            int PresentFactors = 0;

            foreach (XElement elements_child in Element.Elements())
            {
                _tableRow = new TableRow();

                string currentElement = elements_child.Name.ToString();
                if (currentElement == enQuestionnaireElements.Factor.ToString())
                {
                    TotalFactors = TotalFactors + 1; //It will calculate the total available factors against selected element!

                    if (elements_child.Attribute("answer").Value != "No")
                        PresentFactors = PresentFactors + 1;

                    string factorTitle = elements_child.Attribute("title").Value;
                    string factorSequence = elements_child.Attribute("sequence").Value;

                    AddFactorRow(elements_child, ElementTable, _tableRow, factorTitle, standardSequence, elementSequence, factorSequence);
                }
                else if (currentElement == enQuestionnaireElements.Summary.ToString())
                {
                    // Overwrite total factors by getting available maxpoint value
                    TotalFactors = Convert.ToInt32(Element.Attribute("maxPoints").Value);
                    AddSummary(ElementTable, elements_child, elementSequence, TotalFactors, PresentFactors);
                }
                else if (currentElement == enQuestionnaireElements.ReviewerNotes.ToString())
                {
                    string ControlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)PCMHType + elementSequence;
                    TextBox textBoxReviewerNotes = (TextBox)pnlNCQARequirements.FindControl(ControlId);
                    textBoxReviewerNotes.Text = elements_child.Value.ToString();
                }
                else if (currentElement == enQuestionnaireElements.Calculation.ToString())
                {
                    #region UPDATE_RESULT_VALUE
                    int maxPoint = Convert.ToInt32(Element.Attribute("maxPoints").Value);

                    /*Update Summary Score Point value*/
                    string ControlId = "hiddenSummarycell2" + (int)PCMHType + elementSequence + "3";
                    HiddenField hiddenSummaryCell2 = (HiddenField)pnlNCQARequirements.FindControl(ControlId);
                    hiddenSummaryCell2.Value = elements_child.Attribute("defaultScore").Value;
                    string percentage = (elements_child.Attribute("defaultScore").Value);
                    percentage = percentage.Trim() == string.Empty ? "0%" : percentage;
                    int elementScore = Convert.ToInt32(percentage.Replace("%", ""));

                    ControlId = "lblSummarycell2" + (int)PCMHType + elementSequence + "3";
                    Label lblSummaryCell2 = (Label)pnlNCQARequirements.FindControl(ControlId);
                    lblSummaryCell2.Text = elements_child.Attribute("defaultScore").Value;

                    /*Update Total PCMH points*/
                    ControlId = "lblSummarycell2" + (int)PCMHType + elementSequence + "4";
                    Label lblSummaryPoint = (Label)pnlNCQARequirements.FindControl(ControlId);
                    lblSummaryPoint.Text = Convert.ToString(((Convert.ToDecimal(elementScore) / 100) * maxPoint)); ;

                    /*Update Mustpass value*/
                    string mustPass = Element.Attribute("mustPass").Value;
                    if (mustPass == "Yes")
                    {
                        ControlId = "lblSummarycell2" + (int)PCMHType + elementSequence + "5";
                        Label lblSummarymustPass = (Label)pnlNCQARequirements.FindControl(ControlId);
                        lblSummarymustPass.Text = elementScore >= 50 ? "Yes" : "No";
                    }

                    #endregion

                    calculationRules += elementSequence + ":";
                    foreach (XElement rules in elements_child.Elements())
                    {

                        calculationRules += rules.Attribute("score").Value + ",";
                        calculationRules += rules.Attribute("minYesFactors").Value + ",";
                        calculationRules += rules.Attribute("maxYesFactors").Value + ",";
                        IEnumerable<XAttribute> mustPresentFactor = from attributes in rules.Attributes()
                                                                    where (XName)attributes.Name == "mustPresentFactorSequence"
                                                                    select attributes;
                        IEnumerable<XAttribute> mustPassFactor = from attributes in rules.Attributes()
                                                                 where (XName)attributes.Name == "mustPassFactorSequence"
                                                                 select attributes;
                        IEnumerable<XAttribute> absentFactor = from attributes in rules.Attributes()
                                                               where (XName)attributes.Name == "absentFactorSequence"
                                                               select attributes;
                        if (mustPresentFactor.Count() > 0)
                        {
                            string tempValue = rules.Attribute("mustPresentFactorSequence").Value;
                            string[] presentFactorList = tempValue.Split(',');

                            if (presentFactorList.Length > 1)
                            {
                                foreach (string value in presentFactorList)
                                {
                                    calculationRules += value + "|";
                                }
                                calculationRules = calculationRules.Remove(calculationRules.Length - 1, 1);
                            }
                            else
                                calculationRules += presentFactorList[0];

                            calculationRules += ",";
                        }
                        else if (mustPassFactor.Count() > 0)
                        {
                            string tempValue = rules.Attribute("mustPassFactorSequence").Value;
                            string[] passFactorList = tempValue.Split(',');

                            if (passFactorList.Length > 1)
                            {
                                foreach (string value in passFactorList)
                                {
                                    calculationRules += value + "|";
                                }
                                calculationRules = calculationRules.Remove(calculationRules.Length - 1, 1);
                            }
                            else
                                calculationRules += passFactorList[0];

                            calculationRules += ",";
                        }
                        else
                            calculationRules += "0,";

                        if (absentFactor.Count() > 0)
                        {
                            string tempValue = rules.Attribute("absentFactorSequence").Value;
                            string[] absentFactorList = tempValue.Split(',');
                            if (absentFactorList.Length > 1)
                            {
                                foreach (string value in absentFactorList)
                                {
                                    calculationRules += value + "|";
                                }
                                calculationRules = calculationRules.Remove(calculationRules.Length - 1, 1);
                            }
                            else
                                calculationRules += absentFactorList[0];

                            calculationRules += "#";
                        }
                        else
                            calculationRules += "0#";
                    }
                    calculationRules += "&";
                }
                else
                    break;
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void AddParentFactorHeader(Table FactorTable, TableRow FactorRow, string ElementSequence)
    {
        try
        {
            int ColumnIndex = 1;

            // Add Header Row 1(Please check the prototype)
            #region PARENT_HEADER
            for (ColumnIndex = 1; ColumnIndex <= DEFAULT_TOTAL_FACTOR_COLUMNS; ColumnIndex++)
            {
                _label = new Label();
                _checkbox = new CheckBox();
                _textBox = new TextBox();
                TableCell _tableCell = new TableCell();
                switch (ColumnIndex)
                {
                    case 1:
                        _tableCell = new TableCell();

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS1;

                        _tableCell.CssClass = "factor-header01";
                        _tableCell.Controls.Add(_label);
                        break;

                    case 2:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header02";

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS2;

                        _tableCell.Controls.Add(_label);
                        break;

                    case 3:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header03";

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS3;

                        _tableCell.Controls.Add(_label);
                        break;

                    case 4:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header04";
                        _tableCell.ColumnSpan = 2;

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS4;

                        _tableCell.Controls.Add(_label);
                        ColumnIndex = ColumnIndex + 1;
                        break;
                    case 6:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header06";
                        _tableCell.ColumnSpan = 2;

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS5;

                        _tableCell.Controls.Add(_label);
                        ColumnIndex = ColumnIndex + 1;
                        break;

                    case 8:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header08";
                        _tableCell.ColumnSpan = 2;

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS6;

                        _tableCell.Controls.Add(_label);
                        ColumnIndex = ColumnIndex + 1;
                        break;

                    case 10:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header10";
                        _tableCell.ColumnSpan = 2;

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS7;

                        _tableCell.Controls.Add(_label);
                        ColumnIndex = ColumnIndex + 1;
                        break;

                    case 12:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header12";
                        _tableCell.ColumnSpan = 2;

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS8;

                        _tableCell.Controls.Add(_label);
                        ColumnIndex = ColumnIndex + 1;
                        break;

                    case 14:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header14";

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS9;

                        _tableCell.Controls.Add(_label);
                        break;
                    case 15:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "factor-header15";

                        _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_HEADER + ElementSequence + ColumnIndex;
                        _label.Text = DEFAULT_HEADER_PARENT_POS10;

                        _tableCell.Controls.Add(_label);
                        break;
                    default:
                        break;

                }

                FactorRow.Controls.Add(_tableCell);

            }
            FactorTable.Controls.Add(FactorRow);

            #endregion

            // Add Header Row2 (Please check the protype)
            #region CHILD_HEADER

            //Row to Create Child_Header
            TableRow _tableRow = new TableRow();
            _tableRow.CssClass = DEFAULT_FACTOR_TABLE_PARENT_ROW_CSS_CLASS;

            int ChildHeaderColumnIndex = 1;

            for (ChildHeaderColumnIndex = 1; ChildHeaderColumnIndex <= DEFAULT_TOTAL_FACTOR_COLUMNS; ChildHeaderColumnIndex++)
            {
                TableCell _tableCell = new TableCell();
                _label = new Label();
                _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_CHILD_HEADER + (int)PCMHType + ElementSequence + ChildHeaderColumnIndex;

                if ((ChildHeaderColumnIndex >= 1 && ChildHeaderColumnIndex <= 3) || (ChildHeaderColumnIndex >= 14 && ChildHeaderColumnIndex <= 15))
                    _label.Text = string.Empty;

                else if (ChildHeaderColumnIndex % 2 == 0)
                    _label.Text = "Req.";

                else
                    _label.Text = "Up.";

                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);
            }

            FactorTable.Controls.Add(_tableRow);

            #endregion

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddFactorRow(XElement Factor, Table FactorTable, TableRow FactorRow, string FactorTitle, string StandardSequence, string ElementSequence, string FactorSequence)
    {
        try
        {
            int columnIndex = 1;
            int uploadedDocs = 0;
            int requiredDocs = 0;

            FactorRow.ID = "tr" + ElementSequence + FactorSequence;
            FactorRow.ClientIDMode = ClientIDMode.Static;

            // Add appropriate control on specific position (to varify please check the prototype)
            for (columnIndex = 1; columnIndex <= DEFAULT_TOTAL_FACTOR_COLUMNS; columnIndex++)
            {
                _label = new Label();
                _checkbox = new CheckBox();
                _textBox = new TextBox();
                _image = new Image();
                TableCell _tableCell = new TableCell();

                // Set TextBox & labels ID, Length & cssClass
                _textBox.ID = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                _textBox.ClientIDMode = ClientIDMode.Static;
                _textBox.CssClass = DEFAULT_FACTOR_TEXTBOX_CSS_CLASS;
                _textBox.MaxLength = 2;

                string commentExists = Factor.Attribute("comment").Value;
                switch (columnIndex)
                {
                    case 1:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "cell-sequence";
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        _label.ID = "lblfactor" + ElementSequence + FactorSequence + (int)PCMHType;
                        _label.Text = FactorSequence;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 2:
                        string docInfoLink = string.Empty;
                        docInfoLink = GetBookmark(Factor);

                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        _label.ID = "lblfactorTitle" + ElementSequence + FactorSequence + (int)PCMHType;

                        _label.Text = FactorTitle;

                        if (StandardSequence == "1" && ElementSequence == "1" && FactorSequence == "1")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + " has been identified as a critical factor and must be met for practices to receive any score on the element";
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "1" && ElementSequence == "2" && FactorSequence == "3")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_CRITIAL_FACTOR_TOOLTIP;
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "1" && ElementSequence == "7" && FactorSequence == "2")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_CRITIAL_FACTOR_TOOLTIP;
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "3" && ElementSequence == "1" && FactorSequence == "3")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + " has been identified as a critical factor and must be met for practices to receive a 50% or 100% score";
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "3" && ElementSequence == "4" && FactorSequence == "1")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_FACTOR_TOOLTIP + " any score on the element";
                            _label.Style.Add("font-weight", "bold");
                        } 
                        else if (StandardSequence == "3" && ElementSequence == "5" && FactorSequence == "2")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_FACTOR;
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "4" && ElementSequence == "1" && FactorSequence == "3")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_FACTOR_TOOLTIP +"more than 25 percent of the available points in this element";
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "5" && ElementSequence == "1" && FactorSequence == "1")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_FACTOR_TOOLTIP + "any credit for this element";
                            _label.Style.Add("font-weight", "bold");
                        }
                        else if (StandardSequence == "5" && ElementSequence == "1" && FactorSequence == "2")
                        {
                            _label.ToolTip = "Factor " + FactorSequence + DEFAULT_FACTOR_TOOLTIP +"any credit for this element";
                            _label.Style.Add("font-weight", "bold");
                        }

                        _tableCell.Controls.Add(_label);
                        break;

                    case 3:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = "factor-control-column03";

                        string factoraAvailable = Factor.Attribute("naAvailable").Value;

                        DropDownList _dropDownList = new DropDownList();
                        _dropDownList.ID = "ddlFactorAnswer" + ElementSequence + FactorSequence;
                        _dropDownList.ClientIDMode = ClientIDMode.Static;

                        int count = 0;
                        foreach (string item in DEFAULT_FACTOR_OPTIONS)
                        {
                            if (count < 2 || factoraAvailable != "No")
                            {
                                _dropDownList.Items.Add(new ListItem(item, count.ToString()));
                                count++;
                            }
                        }

                        #region CALCULATE_SUMMARY_ONCLIENTSIDE

                        // hidden field to store the value
                        HiddenField hiddenOldAnswer = new HiddenField();
                        hiddenOldAnswer.ID = "hiddenOldAnswer" + ElementSequence + FactorSequence;
                        hiddenOldAnswer.ClientIDMode = ClientIDMode.Static;

                        // Depends on Summary label Id value
                        string mustPass = Factor.Parent.Attribute("mustPass").Value;

                        string factorControlPrefix = "ddlFactorAnswer";
                        string SummaryCell2Id = "lblSummarycell2" + (int)PCMHType + ElementSequence;

                        _dropDownList.Attributes.Add("onchange", "calculateSummary('" + ElementSequence + "','"
                            + Convert.ToString((int)PCMHType) + "','" + SummaryCell2Id + "','" + _dropDownList.ID + "','" +
                            factorControlPrefix + "','" + mustPass + "','" + hiddenOldAnswer.ID + "');");

                        #endregion

                        #region FACTOR_ANSWER
                        string markeAnswer = Factor.Attribute("answer").Value;
                        _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(markeAnswer));
                        hiddenOldAnswer.Value = markeAnswer;

                        #endregion

                        _tableCell.Controls.Add(hiddenOldAnswer);
                        _tableCell.Controls.Add(_dropDownList);

                        string commentExist = Factor.Attribute("comment").Value;
                        string commentNote = Factor.Attribute("note").Value;

                        if (commentExist == "Yes")
                        {
                            // To Add Critical Notes
                            _image.ID = "imgFactorCriticalNote" + ElementSequence + FactorSequence + Convert.ToString((int)PCMHType); ;
                            _image.CssClass = "factorCriticalNotePopUp";
                            _image.AlternateText = "Add Comment";
                            _image.ToolTip = "Add Comment";

                            #region FactorPresentOrNotAvailable
                            // Capture values against selected PopUp
                            string FCNPopUpTitle = "Comment for PCMH " + Convert.ToString((int)PCMHType) +
                                " - ELEMENT " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1] +
                                " - Factor " + FactorSequence;

                            #region FACTOR_COMMENTS
                            string comment = string.Empty;

                            foreach (XElement comments in Factor.Elements("Comment"))
                            {
                                comment = comments.Value.Trim();
                            }

                            _hiddenField = new HiddenField();
                            _hiddenField.ID = "hiddenfcn" + ElementSequence + FactorSequence + Convert.ToString((int)PCMHType);
                            _hiddenField.Value = comment;
                            _hiddenField.ClientIDMode = ClientIDMode.Static;
                            _tableCell.Controls.Add(_hiddenField);

                            #endregion

                            _image.Attributes.Add("onclick", "fcnTracking('" + FCNPopUpTitle + "','"
                                + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)PCMHType) + "','" + _hiddenField.ID
                                + "','" + commentNote + "');");


                            #endregion

                            if (comment == string.Empty)
                                _image.ImageUrl = "~/Themes/Images/factor-note-empty.png"; // set image for current popup (when no content found)
                            else
                                _image.ImageUrl = "~/Themes/Images/factor-note.png"; // set image for current popup (when content found)

                            _tableCell.Controls.Add(_image);
                        }
                        break;

                    case 4:
                        requiredDocs = uploadedDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        // fetching last value from previous save (if exist)
                        IEnumerable<XElement> factorPolicies = from Policies in Factor.Descendants("Policies")
                                                               select Policies;

                        #region receivedPolicies
                        foreach (XElement receivedPolices in factorPolicies)
                        {
                            _textBox.Text = receivedPolices.Attribute("required").Value;

                            if (_textBox.Text.Trim() != string.Empty)
                                requiredDocs = Convert.ToInt32(receivedPolices.Attribute("required").Value);

                            foreach (XElement uploaddocs in receivedPolices.Elements())
                            {
                                uploadedDocs = uploadedDocs + 1;
                            }
                        }
                        #endregion

                        _tableCell.Controls.Add(_textBox);
                        break;
                    case 6:
                        requiredDocs = uploadedDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        // fetching last value from previous save (if exist)
                        IEnumerable<XElement> factorReports = from Policies in Factor.Descendants("Reports")
                                                              select Policies;

                        #region receivedReports
                        foreach (XElement receivedReports in factorReports)
                        {
                            _textBox.Text = receivedReports.Attribute("required").Value;
                            if (_textBox.Text.Trim() != string.Empty)
                                requiredDocs = Convert.ToInt32(receivedReports.Attribute("required").Value);

                            foreach (XElement uploaddocs in receivedReports.Elements())
                            {
                                uploadedDocs = uploadedDocs + 1;
                            }
                        }
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 8:
                        requiredDocs = uploadedDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        // fetching last value from previous save (if exist)
                        IEnumerable<XElement> factorScreenShots = from Policies in Factor.Descendants("Screenshots")
                                                                  select Policies;

                        #region receivedScreenShots
                        foreach (XElement receivedScreenShots in factorScreenShots)
                        {
                            _textBox.Text = receivedScreenShots.Attribute("required").Value;

                            if (_textBox.Text.Trim() != string.Empty)
                                requiredDocs = Convert.ToInt32(receivedScreenShots.Attribute("required").Value);

                            foreach (XElement uploaddocs in receivedScreenShots.Elements())
                            {
                                uploadedDocs = uploadedDocs + 1;
                            }
                        }
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 10:
                        requiredDocs = uploadedDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        // fetching last value from previous save (if exist)
                        IEnumerable<XElement> factorLogsOrTools = from Policies in Factor.Descendants("LogsOrTools")
                                                                  select Policies;

                        #region receivedLogsOrTools
                        foreach (XElement receivedLogsOrTools in factorLogsOrTools)
                        {
                            _textBox.Text = receivedLogsOrTools.Attribute("required").Value;

                            if (_textBox.Text.Trim() != string.Empty)
                                requiredDocs = Convert.ToInt32(receivedLogsOrTools.Attribute("required").Value);

                            foreach (XElement uploaddocs in receivedLogsOrTools.Elements())
                            {
                                uploadedDocs = uploadedDocs + 1;
                            }
                        }
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 12:
                        requiredDocs = uploadedDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        // fetching last value from previous save (if exist)
                        IEnumerable<XElement> factorOtherDocs = from Policies in Factor.Descendants("OtherDocs")
                                                                select Policies;

                        #region receivedOtherDocs
                        foreach (XElement receivedOtherDocs in factorOtherDocs)
                        {
                            _textBox.Text = receivedOtherDocs.Attribute("required").Value;

                            if (_textBox.Text.Trim() != string.Empty)
                                requiredDocs = Convert.ToInt32(receivedOtherDocs.Attribute("required").Value);

                            foreach (XElement uploaddocs in receivedOtherDocs.Elements())
                            {
                                uploadedDocs = uploadedDocs + 1;
                            }
                        }
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;

                    case 5:
                    case 7:
                    case 9:
                    case 11:
                    case 13:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        if (requiredDocs > uploadedDocs)
                            _tableCell.CssClass = "factor-control-hightlight";
                        else
                            _tableCell.CssClass = "factor-control-important";

                        _label.ID = DEFAULT_LABLE_ID_PREFIX_FOR_FACTOR_DOC + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _label.Text = uploadedDocs.ToString();
                        _label.CssClass = "factor-label";
                        _label.ClientIDMode = ClientIDMode.Static;

                        _tableCell.Controls.Add(_label);
                        break;

                    case 14:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.HorizontalAlign = HorizontalAlign.Center;
                        if (FactorTable.Enabled)
                        {
                            _tableCell.CssClass = "factorNotePopUp";
                        }
                            _image.ID = "imgFactorNote" + ElementSequence + FactorSequence + Convert.ToString((int)PCMHType);
                            _image.ClientIDMode = ClientIDMode.Static;
                            _image.AlternateText = "Add/Edit Private Notes";
                            _image.ToolTip = "Add/Edit Private Notes";

                            // capture values against selected PopUp
                            string fnPopUpTitle = "Private Note for PCMH " + Convert.ToString((int)PCMHType);
                            fnPopUpTitle += " - ELEMENT " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1];
                            fnPopUpTitle += " - Factor " + FactorSequence;

                            #region FACTOR_PRIVATECOMMENT
                            string Notes = string.Empty;

                            foreach (XElement comments in Factor.Elements("PrivateNote"))
                            {
                                foreach (XElement note in comments.Elements("Note"))
                                {
                                    Notes += note.Attribute("sequence").Value + ",";
                                    Notes += note.Attribute("date").Value + ",";
                                    Notes += note.Attribute("user").Value + ",";
                                    Notes += note.Value + "@";
                                    Notes += note.Value.Replace(",", "~~~~~").Replace("@", "|||||") + "@";
                                }
                            }

                                _hiddenField = new HiddenField();
                                _hiddenField.ID = "hiddenPrivateNote" + ElementSequence + FactorSequence + Convert.ToString((int)PCMHType);
                                _hiddenField.Value = Notes;
                                /* hidden.Value = Privatecomment;*/
                                _hiddenField.ClientIDMode = ClientIDMode.Static;
                                _tableCell.Controls.Add(_hiddenField);
                           
                            #endregion

                            _image.Attributes.Add("onclick", "fnTracking('" + fnPopUpTitle + "','"
                                + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)PCMHType) + "','" +
                                _hiddenField.ID + "');");

                            if (Notes.Trim() == string.Empty)
                                _image.ImageUrl = "~/Themes/Images/note-empty.png"; // set image for current popup (when no content found)
                            else
                                _image.ImageUrl = "~/Themes/Images/note.png"; // set image for current popup (when content found)                        

                            _tableCell.Controls.Add(_image);
                        
                        break;

                    case 15:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + (int)PCMHType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        if (FactorTable.Enabled)
                        {
                            _tableCell.CssClass = "uploadPopUp";
                        }
                        // setting image attributes
                        _image.ID = "imgFactorUpload" + ElementSequence + FactorSequence + Convert.ToString((int)PCMHType);
                        _image.ImageUrl = "~/Themes/Images/upload.png";
                        _image.ClientIDMode = ClientIDMode.Static;
                        _image.AlternateText = "Upload File";
                        _image.ToolTip = "Upload File";

                        // Capture values against selected PopUp
                        string FUDPopUpTitle = "Uploading file for PCMH " + Convert.ToString((int)PCMHType) +
                            " - ELEMENT " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1] +
                            " - Factor " + FactorSequence;

                        _image.Attributes.Add("onclick", "fudTracking('" + FUDPopUpTitle + "','"
                            + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)PCMHType) + "','" + ProjectId +
                            "','" + System.Web.HttpUtility.JavaScriptStringEncode(PracticeName) + "','" + System.Web.HttpUtility.JavaScriptStringEncode(SiteName) + "','" + Node + "','" + PracticeId + "','" + SiteId + "');");

                        _tableCell.Controls.Add(_image);
                        break;

                    default:
                        break;

                }
                FactorRow.Controls.Add(_tableCell);

            }
            FactorTable.Controls.Add(FactorRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddSummary(Table ElementTable, XElement Element, string ElementSequence, int TotalFactors, int PresentFactor)
    {

        try
        {
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();

            // To remove the difference between new table and current table
            _tableCell.ColumnSpan = DEFAULT_TOTAL_FACTOR_COLUMNS;

            // To Create Summary Add New table in Factor table (element table)
            Table summaryTable = new Table();
            summaryTable.CssClass = "summary";
            _tableCell.Controls.Add(summaryTable);

            _tableRow.Controls.Add(_tableCell);
            ElementTable.Controls.Add(_tableRow);

            int CurrentSummary = 0;

            // Add summary against the current Element
            foreach (XElement summary in Element.Elements())
            {
                string currentElement = summary.Name.ToString();

                if (currentElement == enQuestionnaireElements.SummaryItem.ToString())
                {
                    CurrentSummary = CurrentSummary + 1;
                    _tableRow = new TableRow();

                    string SummaryTitle = summary.Attribute("title").Value.ToString();
                    string SummarySequence = summary.Attribute("Sequence").Value.ToString();


                    // First Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-cell";
                    _label = new Label();
                    _label.ID = "lblSummarycell1" + (int)PCMHType + ElementSequence + SummarySequence;
                    _label.Text = SummaryTitle;

                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);

                    // Second Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-point";
                    _label = new Label();
                    _label.ID = "lblSummarycell2" + (int)PCMHType + ElementSequence + SummarySequence;
                    _label.Text = CurrentSummary == 1 ? TotalFactors.ToString() : CurrentSummary == 2 ? PresentFactor.ToString() : " 0 ";
                    _label.ClientIDMode = ClientIDMode.Static;

                    _hiddenField = new HiddenField();
                    _hiddenField.ID = "hiddenSummarycell2" + (int)PCMHType + ElementSequence + SummarySequence;
                    _hiddenField.ClientIDMode = ClientIDMode.Static;

                    _tableCell.Controls.Add(_hiddenField);
                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);


                    if (CurrentSummary == 1)
                    {
                        // Add 3rd Cell in the begining of First Row
                        AddReviewerNotes(_tableRow, ElementSequence);
                    }

                    // Add Summary Row
                    summaryTable.Controls.Add(_tableRow);

                }
                else
                {
                    message.Warning("Summary couldn't be loaded successfully due to invalid details.");
                    break;
                }
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddReviewerNotes(TableRow tableRow, string elementSequence)
    {

        try
        {
            TableCell _tableCell = new TableCell();
            _tableCell.ColumnSpan = 13;
            _tableCell.RowSpan = 5;
            _tableCell.CssClass = "summary-title-notes";

            // Add Panel to design Reviewer Notes Area
            Panel _panel = new Panel();
            _panel.ID = "pnl" + elementSequence;
            _panel.CssClass = "ReviewerNotes";
            _tableCell.Controls.Add(_panel);
            tableRow.Controls.Add(_tableCell);

            // Add control in above panel
            Table noteTable = new Table();
            noteTable.CssClass = "note-Area";

            _panel.Controls.Clear();
            _panel.Controls.Add(noteTable);

            TableRow noteRow;
            TableCell noteCell;

            // First Row
            noteRow = new TableRow();

            // Add lable
            noteCell = new TableCell();
            noteCell.CssClass = "note-label";

            _label = new Label();
            _label.ID = "lblNote" + (int)PCMHType + elementSequence;
            _label.Text = "Reserved for Reviewer Notes:";

            noteCell.Controls.Add(_label);
            noteRow.Controls.Add(noteCell);

            // Add CheckBox
            noteCell = new TableCell();
            noteCell.CssClass = "note-checkbox";

            _label = new Label();
            _label.ID = "lblNoteText" + (int)PCMHType + elementSequence;
            _label.Text = "Mark as complete";

            noteCell.Controls.Add(_label);

            _checkbox = new CheckBox();
            _checkbox.ID = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)PCMHType + elementSequence;
            _checkbox.ClientIDMode = ClientIDMode.Static;

            // To enable/disable the Element complete CheckBox
            _checkbox.Visible = true;
            noteCell.Controls.Add(_checkbox);
            noteRow.Controls.Add(noteCell);

            //Add First Row in note Table
            noteTable.Controls.Add(noteRow);

            // Second Row
            noteRow = new TableRow();

            noteCell = new TableCell();
            noteCell.ColumnSpan = 2;

            _textBox = new TextBox();
            _textBox.ID = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)PCMHType + elementSequence;
            _textBox.ClientIDMode = ClientIDMode.Static;
            _textBox.CssClass = "note-textbox";
            _textBox.Text = "";
            _textBox.ToolTip = "Enter Notes";
            _textBox.TextMode = TextBoxMode.MultiLine;

            noteCell.Controls.Add(_textBox);
            noteRow.Controls.Add(noteCell);

            // Add Second Row in note table
            noteTable.Controls.Add(noteRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected string GetBookmark(XElement element)
    {
        string link = string.Empty;

        foreach (XElement docElement in element.Descendants(enQuestionnaireElements.Info.ToString()))
        {
            link = Session["hostPath"] + docElement.Element(enQuestionnaireElements.Doc.ToString()).Attribute("link").Value;
        }

        return link;
    }

    protected void SavingQuestionnaire()
    {
        try
        {

            string storedQuestionnaire = Session["NCQAQuestionnaire"].ToString();

            XDocument StoreDocument = XDocument.Parse(storedQuestionnaire);
            XElement StoreElement = StoreDocument.Root;

            string elementType = StoreElement.Name.ToString();
            if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
            {

                int Type = (int)PCMHType;
                IEnumerable<XElement> Standard = from element in StoreElement.Descendants("Standard")
                                                 where (string)element.Attribute("sequence") == Convert.ToString(Type)
                                                 select element;
                string controlId;
                string value;
                string newElement;

                controlId = value = newElement = string.Empty;

                foreach (XElement element in Standard.Elements())
                {
                    string elementSequence = element.Attribute("sequence").Value;
                    string maxPoints = element.Attribute("maxPoints").Value;
                    string mustPass = element.Attribute("mustPass").Value;


                    #region MarkAsComplete_HANDLING
                    string checkBoxControlId = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)PCMHType + elementSequence;
                    CheckBox chkBoxElementControl = (CheckBox)pnlNCQARequirements.FindControl(checkBoxControlId);
                    if (chkBoxElementControl != null)
                    { element.Attribute("complete").Value = chkBoxElementControl.Checked ? "Yes" : "No"; }

                    #endregion

                    controlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)PCMHType + elementSequence;
                    TextBox textBoxReviewerNotes = (TextBox)pnlNCQARequirements.FindControl(controlId);
                    value = textBoxReviewerNotes == null ? "0" : textBoxReviewerNotes.Text;

                    IEnumerable<XElement> reviewerNotes = from notes in element.Descendants("ReviewerNotes")
                                                          select notes;

                    #region ReviewerNotes
                    if (reviewerNotes.Count() == 0)
                    {
                        newElement = @"<ReviewerNotes>" + value + "</ReviewerNotes>";
                        /*Add Answer Element*/
                        XElement reviewerComments = XElement.Parse(newElement);
                        element.Add(reviewerComments);
                    }
                    else
                    {
                        foreach (XElement requiredElement in reviewerNotes)
                        {
                            requiredElement.Value = value;
                        }
                    }

                    #endregion

                    foreach (XElement factor in element.Elements())
                    {
                        string currentElement = factor.Name.ToString();

                        if (currentElement == enQuestionnaireElements.Factor.ToString())
                        {

                            string factorSequence = factor.Attribute("sequence").Value;

                            #region FactorAnswer

                            controlId = "ddlFactorAnswer" + elementSequence + factorSequence;
                            DropDownList ddlFactorAnswer = (DropDownList)pnlNCQARequirements.FindControl(controlId);
                            factor.Attribute("answer").Value = ddlFactorAnswer.SelectedItem.ToString();


                            #endregion

                            int columnIndex = DEFAULT_HEADER_CHILD_START;
                            for (columnIndex = DEFAULT_HEADER_CHILD_START; columnIndex <= DEFAULT_HEADER_CHILD_END; columnIndex =
                                columnIndex + 2)
                            {
                                switch (columnIndex)
                                {

                                    case 4:
                                        controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + elementSequence + factorSequence + (int)PCMHType + columnIndex;
                                        TextBox textBoxPloicy = (TextBox)pnlNCQARequirements.FindControl(controlId);

                                        value = textBoxPloicy == null ? "0" : textBoxPloicy.Text;

                                        IEnumerable<XElement> factorPolicies = from Policies in factor.Descendants("Policies")
                                                                               select Policies;

                                        #region RequiredPolicies
                                        if (factorPolicies.Count() == 0)
                                        {
                                            newElement = @"<Policies required='" + value + "'></Policies>";

                                            /*Add Answer Element*/
                                            XElement AnswerFactor = XElement.Parse(newElement);
                                            factor.Add(AnswerFactor);
                                        }
                                        else
                                        {
                                            foreach (XElement requiredElement in factorPolicies)
                                            {
                                                requiredElement.Attribute("required").Value = value;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 6:
                                        controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + elementSequence + factorSequence + (int)PCMHType + columnIndex; ;
                                        TextBox textBoxReport = (TextBox)pnlNCQARequirements.FindControl(controlId);

                                        value = textBoxReport == null ? "0" : textBoxReport.Text;

                                        IEnumerable<XElement> factorReports = from Policies in factor.Descendants("Reports")
                                                                              select Policies;

                                        #region RequiredReports
                                        if (factorReports.Count() == 0)
                                        {
                                            newElement = @"<Reports required='" + value + "'></Reports>";

                                            /*Add Answer Element*/
                                            XElement AnswerFactor = XElement.Parse(newElement);
                                            factor.Add(AnswerFactor);
                                        }
                                        else
                                        {
                                            foreach (XElement requiredElement in factorReports)
                                            {
                                                int uploadeddocs = 0;
                                                foreach (XElement uploadedElement in factorReports.Elements())
                                                {
                                                    uploadeddocs = uploadeddocs + 1;
                                                }

                                                requiredElement.Attribute("required").Value = value;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 8:
                                        controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + elementSequence + factorSequence + (int)PCMHType + columnIndex; ;
                                        TextBox textBoxrequiredScrnShts = (TextBox)pnlNCQARequirements.FindControl(controlId);

                                        value = textBoxrequiredScrnShts == null ? "0" : textBoxrequiredScrnShts.Text;

                                        IEnumerable<XElement> factorScrnshots = from Policies in factor.Descendants("Screenshots")
                                                                                select Policies;

                                        #region RequiredScreenShots
                                        if (factorScrnshots.Count() == 0)
                                        {
                                            newElement = @"<Screenshots required='" + value + "'></Screenshots>";

                                            /*Add Answer Element*/
                                            XElement AnswerFactor = XElement.Parse(newElement);
                                            factor.Add(AnswerFactor);
                                        }
                                        else
                                        {
                                            foreach (XElement requiredElement in factorScrnshots)
                                            {
                                                int uploadeddocs = 0;
                                                foreach (XElement uploadedElement in factorScrnshots.Elements())
                                                {
                                                    uploadeddocs = uploadeddocs + 1;
                                                }

                                                requiredElement.Attribute("required").Value = value;
                                            }
                                        }

                                        #endregion
                                        break;
                                    case 10:
                                        controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + elementSequence + factorSequence + (int)PCMHType + columnIndex; ;
                                        TextBox textBoxrequiredLog = (TextBox)pnlNCQARequirements.FindControl(controlId);

                                        value = textBoxrequiredLog == null ? "0" : textBoxrequiredLog.Text;

                                        IEnumerable<XElement> factorLogsOrTools = from Policies in factor.Descendants("LogsOrTools")
                                                                                  select Policies;

                                        #region RequiredLogsOrTools
                                        if (factorLogsOrTools.Count() == 0)
                                        {
                                            newElement = @"<LogsOrTools required='" + value + "'></LogsOrTools>";

                                            /*Add Answer Element*/
                                            XElement AnswerFactor = XElement.Parse(newElement);
                                            factor.Add(AnswerFactor);
                                        }
                                        else
                                        {
                                            foreach (XElement requiredElement in factorLogsOrTools)
                                            {
                                                int uploadeddocs = 0;
                                                foreach (XElement uploadedElement in factorLogsOrTools.Elements())
                                                {
                                                    uploadeddocs = uploadeddocs + 1;
                                                }

                                                requiredElement.Attribute("required").Value = value;
                                            }
                                        }

                                        #endregion
                                        break;
                                    case 12:
                                        controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + elementSequence + factorSequence + (int)PCMHType + columnIndex; ;
                                        TextBox textBoxOtherDocs = (TextBox)pnlNCQARequirements.FindControl(controlId);

                                        value = textBoxOtherDocs == null ? "0" : textBoxOtherDocs.Text;

                                        IEnumerable<XElement> factorOtherDocs = from Policies in factor.Descendants("OtherDocs")
                                                                                select Policies;

                                        #region RequiredOtherDocs
                                        if (factorOtherDocs.Count() == 0)
                                        {
                                            newElement = @"<OtherDocs required='" + value + "'></OtherDocs>";

                                            /*Add Answer Element*/
                                            XElement AnswerFactor = XElement.Parse(newElement);
                                            factor.Add(AnswerFactor);
                                        }
                                        else
                                        {
                                            foreach (XElement requiredElement in factorOtherDocs)
                                            {
                                                int uploadeddocs = 0;
                                                foreach (XElement uploadedElement in factorOtherDocs.Elements())
                                                {
                                                    uploadeddocs = uploadeddocs + 1;
                                                }

                                                requiredElement.Attribute("required").Value = value;
                                            }
                                        }

                                        #endregion
                                        break;
                                }
                            }

                        }
                        if (currentElement == enQuestionnaireElements.Calculation.ToString())
                        {
                            // To update Score
                            string hiddenSummaryScoreId = "hiddenSummarycell2" + (int)PCMHType + elementSequence + "3";
                            HiddenField hiddenElementScore = (HiddenField)pnlNCQARequirements.FindControl(hiddenSummaryScoreId);
                            factor.Attribute("defaultScore").Value = hiddenElementScore.Value != string.Empty ? hiddenElementScore.Value : "0%";
                        }
                    }
                }

            }
            string finalizedDoc = Convert.ToString(StoreDocument.Root);
            _questionBO = new QuestionBO();
            _questionBO.SaveFilledQuestionnaire(questionnaireId, ProjectId, StoreDocument.Root, userId);
            Session["NCQAQuestionnaire"] = StoreDocument.Root.ToString();
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void SavingEvaluationNotes()
    {
        try
        {
            string StoredQuestionnaire = Session["NCQAQuestionnaire"].ToString();
            XDocument StoreDocument = XDocument.Parse(StoredQuestionnaire);
            XElement StoreElement = StoreDocument.Root;

            string elementType = StoreElement.Name.ToString();
            if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
            {

                int Type = (int)PCMHType;
                IEnumerable<XElement> Standard = from element in StoreElement.Descendants("Standard")
                                                 where (string)element.Attribute("sequence") == Convert.ToString(Type)
                                                 select element;

                IEnumerable<XElement> Elements = from element in Standard.Descendants("Element")
                                                 where (string)element.Attribute("sequence") == hiddenElementId.Value
                                                 select element;

                IEnumerable<XElement> EvaluationNotes = from element in Elements.Descendants("EvaluationNotes")
                                                        select element;
                if (EvaluationNotes.Count() == 0)
                {
                    foreach (XElement element in Elements)
                    {
                        string value = txtElementComments.Text;
                        string NewElement = @"<EvaluationNotes>" + value.Replace("&", "&amp;").Replace("<", "~~~~~").Replace(">", "|||||") + "</EvaluationNotes>";
                        /*Add Answer Element*/
                        XElement reviewerComments = XElement.Parse(NewElement);
                        element.Add(reviewerComments);
                        break;
                    }
                }
                else
                {
                    foreach (XElement element in EvaluationNotes)
                    {
                        string value = txtElementComments.Text;
                        element.Value = value;

                        break;
                    }
                }

                string FinalizedDoc = Convert.ToString(StoreDocument.Root);

                _questionBO = new QuestionBO();
                _questionBO.SaveFilledQuestionnaire(questionnaireId, ProjectId, StoreDocument.Root, userId);

                Session["NCQAQuestionnaire"] = StoreDocument.Root.ToString();

                // Update Comment In Hidden Field
                string controlId = "hiddenelement" + hiddenElementId.Value + hiddenElementPCMH.Value;

                HiddenField ElementcommentField = (HiddenField)pnlNCQARequirements.FindControl(controlId);
                ElementcommentField.Value = txtElementComments.Text;

            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void SavingFactorCriticalNotes()
    {
        try
        {
            string value = txtFCNComments.Text.Trim();

            string storedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
            XDocument storeDocument = XDocument.Parse(storedQuestionnaire);
            XElement storeElement = storeDocument.Root;

            string ElementType = storeElement.Name.ToString();

            if (ElementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
            {

                int Type = (int)PCMHType;
                IEnumerable<XElement> Standard = from element in storeElement.Descendants("Standard")
                                                 where (string)element.Attribute("sequence") == Convert.ToString(Type)
                                                 select element;

                IEnumerable<XElement> Elements = from element in Standard.Descendants("Element")
                                                 where (string)element.Attribute("sequence") == hiddenFCNElementId.Value
                                                 select element;
                IEnumerable<XElement> Factors = from factor in Elements.Descendants("Factor")
                                                where (string)factor.Attribute("sequence") == hiddenFCNFactorId.Value
                                                select factor;

                IEnumerable<XElement> comment = from comments in Factors.Descendants("Comment")
                                                select comments;
                if (comment.Count() == 0)
                {
                    foreach (XElement element in Factors)
                    {
                        string NewElement = @"<Comment>" + value + "</Comment>";

                        // Add Answer Element
                        XElement reviewerComments = XElement.Parse(NewElement);
                        element.Add(reviewerComments);
                        break;
                    }
                }
                else
                {
                    foreach (XElement element in comment)
                    {
                        element.Value = value;
                        break;
                    }
                }

                string FinalizedDoc = Convert.ToString(storeDocument.Root);
                _questionBO = new QuestionBO();
                _questionBO.SaveFilledQuestionnaire(questionnaireId, ProjectId, storeDocument.Root, userId);

                Session["NCQAQuestionnaire"] = storeDocument.Root.ToString();

                // Update Comment In Hidden Field
                string controlId = "hiddenfcn" + hiddenFCNElementId.Value + hiddenFCNFactorId.Value + hiddenFCNPCMH.Value;

                HiddenField fcncommentField = (HiddenField)pnlNCQARequirements.FindControl(controlId);
                fcncommentField.Value = txtFCNComments.Text;

                string fcnImageControlId = "imgFactorCriticalNote" + hiddenFCNElementId.Value + hiddenFCNFactorId.Value + hiddenFCNPCMH.Value;
                Image fcnImage = (Image)pnlNCQARequirements.FindControl(fcnImageControlId);

                if (txtFCNComments.Text.Trim() == string.Empty)
                    fcnImage.ImageUrl = "~/Themes/Images/factor-note-empty.png"; // set image for current popup (when no content found)
                else
                    fcnImage.ImageUrl = "~/Themes/Images/factor-note.png"; // set image for current popup (when content found)    
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void SavingFactorPrivateNote()
    {

        try
        {
            string value = txtFNComments.Text.Trim();
            if (value != string.Empty)
            {
                string storedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
                string newNote = "";

                XDocument StoreDocument = XDocument.Parse(storedQuestionnaire);
                XElement StoreElement = StoreDocument.Root;

                string elementType = StoreElement.Name.ToString();
                if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
                {

                    int Type = (int)PCMHType;
                    IEnumerable<XElement> Standard = from element in StoreElement.Descendants("Standard")
                                                     where (string)element.Attribute("sequence") == Convert.ToString(Type)
                                                     select element;

                    IEnumerable<XElement> Elements = from element in Standard.Descendants("Element")
                                                     where (string)element.Attribute("sequence") == hiddenFNElementId.Value
                                                     select element;
                    IEnumerable<XElement> Factors = from factor in Elements.Descendants("Factor")
                                                    where (string)factor.Attribute("sequence") == hiddenFNFactorId.Value
                                                    select factor;

                    IEnumerable<XElement> comment = from comments in Factors.Descendants("PrivateNote")
                                                    select comments;
                    IEnumerable<XElement> note = from notes in comment.Descendants("Note")
                                                 select notes;

                    if (comment.Count() == 0)
                    {
                        foreach (XElement element in Factors)
                        {
                            XElement PrivateNote = new XElement("PrivateNote", new XElement("Note", new XAttribute("sequence", Convert.ToString(comment.Count() + 1)),
                                                                              new XAttribute("date", String.Format("{0:MM/dd/yy}", System.DateTime.Now)),
                                                                              new XAttribute("user", Session["UserName"]), value));

                            newNote = Convert.ToString(comment.Count() + 1) + ",";
                            newNote += String.Format("{0:MM/dd/yy}", System.DateTime.Now) + ",";
                            newNote += Session["UserName"].ToString() + ",";
                            newNote += value.Replace(",", "~~~~~").Replace("@", "|||||") + "@";
                            element.Add(PrivateNote);
                            break;
                        }
                    }
                    else
                    {
                        foreach (XElement element in comment)
                        {
                            XElement PrivateNote = new XElement("Note", new XAttribute("sequence", Convert.ToString(note.Count() + 1)),
                                                                              new XAttribute("date", String.Format("{0:MM/dd/yy}", System.DateTime.Now)),
                                                                              new XAttribute("user", Session["UserName"]), value);

                            newNote = Convert.ToString(note.Count() + 1) + ",";
                            newNote += String.Format("{0:MM/dd/yy}", System.DateTime.Now) + ",";
                            newNote += Session["UserName"].ToString() + ",";
                            newNote += value.Replace(",", "~~~~~").Replace("@", "|||||") + "@";
                            element.Add(PrivateNote);

                            break;
                        }
                    }
                    string FinalizedDoc = Convert.ToString(StoreDocument.Root);
                    _questionBO = new QuestionBO();
                    _questionBO.SaveFilledQuestionnaire(questionnaireId, ProjectId, StoreDocument.Root, userId);
                    Session["NCQAQuestionnaire"] = StoreDocument.Root.ToString();

                    // Update Comment In Hidden Field
                    string ControlId = "hiddenPrivateNote" + hiddenFNElementId.Value + hiddenFNFactorId.Value + hiddenFNPCMH.Value;
                    HiddenField fncommentField = (HiddenField)pnlNCQARequirements.FindControl(ControlId);
                    fncommentField.Value = fncommentField.Value + newNote;

                    string privateNoteImageControlId = "imgFactorNote" + hiddenFNElementId.Value + hiddenFNFactorId.Value + hiddenFNPCMH.Value;
                    Image privateNoteImage = (Image)pnlNCQARequirements.FindControl(privateNoteImageControlId);

                    if (newNote.Trim() == string.Empty)
                        privateNoteImage.ImageUrl = "~/Themes/Images/note-empty.png"; // set image for current popup (when no content found)
                    else
                        privateNoteImage.ImageUrl = "~/Themes/Images/note.png"; // set image for current popup (when content found)         

                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    #endregion

}
