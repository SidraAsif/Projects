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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class MORe : System.Web.UI.UserControl
{
    #region PPROPERTIES
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    //public int ProjectId { get; set; }
    public int ProjectUsageId { get; set; }
    public string Node { get; set; }
    public string SiteName { get; set; }
    public int SiteId { get; set; }
    public int MOReType { get; set; }
    public int TemplateId { get; set; }
    public int HeaderId { get; set; }

    #endregion

    #region CONSTANTS

    #region ********************************WARNING**********************************
    // BE CARE FULL IF YOU WANT TO CHANGE THE VALUE OF CONSTATNS!
    // CONSTANT VALUES ARE ALSO USING IN PCMH SCRIPT FILE
    #endregion

    private const string CONTROL_NAME = "Template";
    private const string DEFAULT_QUESTIONNAIRE_TYPE = "NCQA";
    private const string DEFAULT_HEADER_PARENT_POS1 = "#";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Question";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Question Met";
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

    private readonly string[] DEFAULT_SUMMARY_TEXT = {"Total Possible Points for this Element:","Total # of Factors with Yes for this Element:",
                                                     "% Points Received for this Element:","Total # of Points Recieved for this Element:"};

    private readonly string[] DEFAULT_SUMMARY_TEXT_MUSTPASS = {"Total Possible Points for this Element:","Total # of Factors with Yes for this Element:",
                                                     "% Points Received for this Element:","Total # of Points Recieved for this Element:","MUST PASS Element - Passed at 50% Level?"};

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
    private const string DEFAULT_FACTOR_HIDDEN_ID = "factorHiddenId";

    private const string DEFAULT_NOTE_CHECKBOX_ID_PREFIX = "chkBoxNote";
    private const string DEFAULT_NOTE_TEXTBOX_ID_PREFIX = "txtNote";

    private const string DEFAULT_HYPERLINK_TAG_CSSCLASS = "link";
    private const string HYPERLINK_TAG_CSSCLASS = "unlink";
    private const string DEFAULT_CRITIAL_FACTOR_TOOLTIP = " has been identified as a critical factor and must be met for practices to score higher than 25 percent on this element";
    private const string DEFAULT_FACTOR_TOOLTIP = "has been identified as a critical factor and is required for practices to receive ";
    private const string DEFAULT_FACTOR = " has been designated as a critical factor required to receive more than 25 percent of the available points for this element";
    private const int ELEMENT_SPLIT_INDEX = 1;
    private const int FIRST_INDEX = 0;

    #endregion

    #region VARIABLES
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
    private MOReBO _MORe = new MOReBO();
    private CorporateElementSubmissionBO corporateElementSubmissionBO;
    DataTable kbTemp = new DataTable();
    DataTable Questions = new DataTable();
    GetKnowledgebaseADO _KBMORe = new GetKnowledgebaseADO();

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
            Logger.PrintError(exception);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == CONTROL_NAME)
            {
                if (!Page.IsPostBack)
                {
                    string hostPath = Util.GetHostPath();
                }
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut");

                GetQuestionnaireByType();
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
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
                if (IsEdit.Value == "true")
                    SavingQuestionnaire();

                // raise a bubble event of reload so that summary tab can be reloaded
                CommandEventArgs args = new CommandEventArgs("Reload", null);
                RaiseBubbleEvent(null, args);
                if (Convert.ToInt32(Session["NewHeaderId"].ToString()) == HeaderId)
                {
                    // raise a bubble event of reload so that MORe tab can be reloaded
                    CommandEventArgs arg = new CommandEventArgs("ReloadMORe", null);
                    RaiseBubbleEvent(null, arg);
                }

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
            SavingQuestionnaire();
            SavingEvaluationNotes();

            // raise a bubble event of reload so that MORe tab can be reloaded
            CommandEventArgs args = new CommandEventArgs("ReloadMORe", null);
            RaiseBubbleEvent(null, args);
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
            SavingQuestionnaire();
            SavingFactorCriticalNotes();
            // raise a bubble event of reload so that MORe tab can be reloaded
            CommandEventArgs args = new CommandEventArgs("ReloadMORe", null);
            RaiseBubbleEvent(null, args);
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
            SavingQuestionnaire();
            SavingFactorPrivateNote();
            // raise a bubble event of reload so that MORe tab can be reloaded
            CommandEventArgs args = new CommandEventArgs("ReloadMORe", null);
            RaiseBubbleEvent(null, args);
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
            if (Session["TemplateHeaderId"] != null)
            {
                userId = Convert.ToInt32(Session["UserApplicationId"]);

                GenerateLayout();
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void GenerateLayout()
    {
        try
        {
            if (ProjectUsageId != 0)
            {


                Session["ProjectUsageId"] = ProjectUsageId;// = PracticeId = 6;//test

                List<int> UnFilledSubHeaders = _MORe.CheckFilledAnswers(TemplateId, ProjectUsageId, SiteId, HeaderId);
                Session["NewHeaderId"] = HeaderId;
                if (UnFilledSubHeaders.Count > 0)
                {

                    foreach (int id in UnFilledSubHeaders)
                    {
                        _MORe.SaveFilledAnswerForBlankTemplates(TemplateId, ProjectUsageId, SiteId, id, true);
                    }
                }

                List<int> UnFilledQuestions = _MORe.CheckFilledAnswersQuestions(TemplateId, ProjectUsageId, SiteId, HeaderId);

                if (UnFilledQuestions.Count > 0)
                {

                    foreach (int id in UnFilledQuestions)
                    {
                        _MORe.SaveFilledAnswerForBlankTemplates(TemplateId, ProjectUsageId, SiteId, id, false);
                    }
                }

                kbTemp = _KBMORe.GetAllKnowledgeBaseOfTemplate(TemplateId, ProjectUsageId, SiteId, HeaderId);

                HiddenField hiddenRules = new HiddenField();
                hiddenRules.ID = "hiddenRules" + (int)MOReType;
                hiddenRules.ClientIDMode = ClientIDMode.Static;

                calculationRules = string.Empty;

                lblSiteInfo.Text = SiteName;
                lblSiteInfo.CssClass = "project-title";

                // clear control if exists
                pnlMOReRequirements.Controls.Clear();

                // Add Master table
                _PCMHTable = new Table();
                _PCMHTable.ID = "NCQAPCMHTable";
                _PCMHTable.ClientIDMode = ClientIDMode.Static;

                pnlMOReRequirements.Controls.Add(_PCMHTable);

                KnowledgeBase Header = _MORe.GetKnowledgeBaseHeadersById(HeaderId, TemplateId);

                AddHeader(Header.Name);
                int SubHeaderSequence = 0;

                foreach (DataRow kb in kbTemp.Rows)
                {
                    SubHeaderSequence++;

                    AddSubHeader(kb, SubHeaderSequence);

                }


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

    protected void AddHeader(string Header)
    {
        try
        {
            string[] standardTitle = Header.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            TableRow _tableRow = new TableRow();
            _tableRow.CssClass = "standard-title";
            TableCell _tableCell = new TableCell();

            _label = new Label();

            if (standardTitle.Count() > 1)
            {
                _label.Text = "<div class='standard-title-head'>" + standardTitle[0] + ":</div>" + "<div class='standard-title-desc'>" +
                    standardTitle[1] + "</div>";
            }
            else
                _label.Text = "<div class='standard-title-head'>" + standardTitle[0] + "</div>";

            Session["standardSequence"] = standardTitle[FIRST_INDEX];

            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);
            _PCMHTable.Controls.Add(_tableRow);

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void AddSubHeader(DataRow Kb, int SubHeaderSequence)
    {
        try
        {
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();
            Literal _literal = new Literal();

            string subPath = _MORe.GetTemplateStDoc(TemplateId);
            string hostPath = Util.GetHostPath();
            string docInfoLink = "";
            if (subPath != null)
                docInfoLink = hostPath + subPath + AddReferencePages(subPath, Convert.ToInt32(Kb["ReferencePages"].ToString()));

            // fetching Element Attribute Values
            string elementTitle = Kb["Name"].ToString();
            string[] elementSplit = elementTitle.Split(new char[] { ' ', ':' });
            string elementId = string.Empty;
            if (elementSplit.Count() > 1)

                elementId = elementSplit[ELEMENT_SPLIT_INDEX];

            else
                elementId = SubHeaderSequence.ToString();

            string standardSequence = Session["headerSequence"].ToString();
            string enableDisableFactorName = Session["standardSequence"].ToString() + " " + elementId;
            string elementSequence = SubHeaderSequence.ToString();
            string elementMustPass = Kb["MustPass"].ToString();

            // create Element Header
            string functionParams = "('tableElement" + elementSequence + "', 'imgElement" + elementSequence + "');";
            _literal.ID = "literalElement" + elementSequence;
            _literal.Text = "<div id='divElement" + elementSequence + "'>";
            _literal.Text += "<div id='divElementText" + elementSequence + "' class='element-title'>";

            // element note image
            _literal.Text += "<a id='imgElement" + elementSequence + "' onClick=\"toggleElement('" + elementSequence + "');\">";
            _literal.Text += "<img class='toggle-img' src='../Themes/Images/Plus.png'></a>&nbsp;&nbsp;";
            if (Kb["IsInfoDocEnable"] != null)
            {
                if ((bool)Kb["IsInfoDocEnable"])
                {
                    //element info image
                    _literal.Text += "<a href=\"" + docInfoLink + "\" \" target=\"_blank\">";
                    _literal.Text += "<img src='../Themes/Images/ncqa-info-ico.png' alt='Info' title ='Info'></a>&nbsp;&nbsp;";
                }
            }

            // write Element Title
            if (elementMustPass != "True")
                _literal.Text += elementTitle;
            else
                _literal.Text += elementTitle + " [MUST PASS]";

            // create PopUp Title
            string elementPopUpTitle = "Evaluation Note for Header " + Convert.ToString((int)MOReType) + " - SubHeader " +
                DEFAULT_LETTERS[Convert.ToInt32(elementSequence) - 1];

            #region FETCHING_EvaluationNote
            string evaluationNotesText = string.Empty;

            if (Kb["FilledAnswersId"] != null)
                evaluationNotesText = (Kb["EvaluationNotes"].ToString() == null ? "" : Kb["EvaluationNotes"].ToString());//_MORe.GetEvaluationNotesBySubHeaderId(Kb.KnowledgeBaseId, TemplateId, ProjectId);
            else
                evaluationNotesText = "";

            if (evaluationNotesText != null)
            {

                _hiddenField = new HiddenField();
                _hiddenField.ID = "hiddenelement" + elementSequence + Convert.ToString((int)MOReType);
                _hiddenField.Value = evaluationNotesText.Trim();
                _hiddenField.ClientIDMode = ClientIDMode.Static;
                _tableCell.Controls.Add(_hiddenField);
            }

            #endregion

            #region ELEMENT_NOTE_PICTURE
            // set image Position
            _literal.Text += "&nbsp;&nbsp;<a id='imgElementNote" + elementSequence;

            // pass the values against current element into popup windows when user click on icon
            corporateElementSubmissionBO = new CorporateElementSubmissionBO();
            if (corporateElementSubmissionBO.IsPracticeCorporateMORe(PracticeId, TemplateId))
            {
                if (corporateElementSubmissionBO.IsSiteCorporateMORe(PracticeId, SiteId))
                {
                    if (corporateElementSubmissionBO.IsNotSubmittedMOReCorporateElement(PracticeId, SiteId, TemplateId, enableDisableFactorName))
                    {
                        _literal.Text += "' onclick=\"elementNoteTrack('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)MOReType);
                        _literal.Text += "','" + _hiddenField.ID + "','" + Kb["KnowledgeBaseId"] + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)MOReType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                    else
                    {
                        _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTrack('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)MOReType);
                        _literal.Text += "','" + _hiddenField.ID + "','" + Kb["KnowledgeBaseId"] + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)MOReType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                }
                else
                {
                    if (corporateElementSubmissionBO.IsCorporateElementofMOReNonCorporateSite(PracticeId, SiteId, TemplateId, enableDisableFactorName))
                    {
                        _literal.Text += "' onclick=\"elementNoteTracking('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)MOReType);
                        _literal.Text += "','" + _hiddenField.ID + "','" + Kb["KnowledgeBaseId"] + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)MOReType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                    else
                    {
                        _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTrack('" + elementPopUpTitle + "','";
                        _literal.Text += elementSequence + "','" + Convert.ToString((int)MOReType);
                        _literal.Text += "','" + _hiddenField.ID + "','" + Kb["KnowledgeBaseId"] + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)MOReType) + "' ";
                        _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
                    }
                }
            }
            else
            {

                _literal.Text += "' class='elementNotePopUp' onclick=\"elementNoteTrack('" + elementPopUpTitle + "','";
                _literal.Text += elementSequence + "','" + Convert.ToString((int)MOReType);
                _literal.Text += "','" + _hiddenField.ID + "','" + Kb["KnowledgeBaseId"] + "');\"><img id ='imgElementNote" + elementSequence + Convert.ToString((int)MOReType) + "' ";
                _literal.Text += "  class='note-elementlevel' alt='Add Evaluation Note' title='Add Evaluation Note' ";
            }

            if (evaluationNotesText == string.Empty || evaluationNotesText == null)
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
            if (corporateElementSubmissionBO.IsPracticeCorporateMORe(PracticeId, TemplateId))
            {
                if (corporateElementSubmissionBO.IsSiteCorporateMORe(PracticeId, SiteId))
                {
                    if (corporateElementSubmissionBO.IsNotSubmittedMOReCorporateElement(PracticeId, SiteId, TemplateId, enableDisableFactorName))
                    {
                        elementTable.Enabled = false;
                        elementTable.Style.Add("Color", "Gray");
                    }
                }
                else
                {
                    if (corporateElementSubmissionBO.IsCorporateElementofMOReNonCorporateSite(PracticeId, SiteId, TemplateId, enableDisableFactorName))
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


            AddQuestion(Kb, elementTable, standardSequence, SubHeaderSequence.ToString());

            // Mark as Complete status & reviewer note
            string checkBoxControlId = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)MOReType + elementSequence;
            CheckBox chkBoxElementControl = (CheckBox)pnlMOReRequirements.FindControl(checkBoxControlId);

            string reviewerNotes = string.Empty;

            reviewerNotes = (Kb["ReviewNotes"].ToString() == "" ? "" : Kb["ReviewNotes"].ToString());

            string reviewerNotesControlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)MOReType + elementSequence;
            TextBox txtreviewerNotes = (TextBox)pnlMOReRequirements.FindControl(reviewerNotesControlId);

            if (txtreviewerNotes != null)
                txtreviewerNotes.Text = reviewerNotes;

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void AddQuestion(DataRow SubHeader, Table ElementTable, string StandardSequence, string SubHeaderSequence)
    {
        try
        {
            // Row to add the document viewer link
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();
            _tableCell.ColumnSpan = 15;
            _tableCell.HorizontalAlign = HorizontalAlign.Right;
            _tableCell.VerticalAlign = VerticalAlign.Middle;

            // DocViewer link
            string lblDocViewerTitle = "Uploaded Document For Header " + (int)MOReType + " - " + "SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(SubHeaderSequence) - 1];
            _hyperLink = new HyperLink();
            _hyperLink.ID = "hypDocViewer" + (int)MOReType + SubHeaderSequence;
            _hyperLink.Text = "View Uploaded Documents";
            if (ElementTable.Enabled)
                _hyperLink.CssClass = DEFAULT_HYPERLINK_TAG_CSSCLASS;
            else
                _hyperLink.CssClass = HYPERLINK_TAG_CSSCLASS;

            _hyperLink.NavigateUrl = "javascript:UploadedDocViewer('" + (int)MOReType + "','" + SubHeaderSequence + "','" + lblDocViewerTitle + "','" +
                PracticeId + "','" + SiteId + "','" + ProjectUsageId + "','" + TemplateId + "');";

            _tableCell.Controls.Add(_hyperLink);
            _tableRow.Controls.Add(_tableCell);
            ElementTable.Controls.Add(_tableRow);

            // Row To Create Parent_Header
            _tableRow = new TableRow();
            _tableRow.CssClass = DEFAULT_FACTOR_TABLE_PARENT_ROW_CSS_CLASS;

            // add parent factor header
            AddParentFactorHeader(ElementTable, _tableRow, SubHeaderSequence);

            // Add Notice for factors
            _tableRow = new TableRow();
            _tableRow.CssClass = DEFAULT_FACTOR_TABLE_ROW_NOTICE_CSS_CLASS;

            _tableCell = new TableCell();
            _tableCell.ColumnSpan = DEFAULT_TOTAL_FACTOR_COLUMNS;

            _label = new Label();
            _label.ID = DEFAULT_LABEL_ID_PREFIX_FOR_FACTOR_NOTICE + SubHeaderSequence;
            _label.Text = SubHeader["InstructionText"].ToString();

            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            ElementTable.Controls.Add(_tableRow);

            // fetching Factors from elements
            int TotalFactors = 0;
            int PresentFactors = 0;

            Questions = _KBMORe.GetAllKnowledgeBaseOfTemplate(TemplateId, ProjectUsageId, SiteId, Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()));
            int index = 0;

            foreach (DataRow _question in Questions.Rows)
            {

                index++;
                _tableRow = new TableRow();

                string currentElement = SubHeader["DisplayName"].ToString();

                TotalFactors = TotalFactors + 1; //It will calculate the total available factors against selected element!

                if (_question["AnswerTypeEnumId"].ToString() != "")
                {
                    int weightage = _MORe.GetAnswerWeightageByEnumId((_question["AnswerTypeEnumId"].ToString() != "" ? Convert.ToInt32(_question["AnswerTypeEnumId"]) : 1), Convert.ToInt32(_question["KnowledgeBaseTemplateId"].ToString()), ProjectUsageId, SiteId);
                    PresentFactors = PresentFactors + weightage;
                }


                string factorTitle = _question["Name"].ToString();
                string factorSequence = index.ToString();

                AddQuestionRow(_question, SubHeader, ElementTable, _tableRow, factorTitle, StandardSequence, SubHeaderSequence, factorSequence);
            }


            TotalFactors = (int)SubHeader["MaxPoints"];

            AddSummary(ElementTable, SubHeader, SubHeaderSequence, TotalFactors, PresentFactors, index.ToString());

            string ControlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)MOReType + SubHeaderSequence;
            TextBox textBoxReviewerNotes = (TextBox)pnlMOReRequirements.FindControl(ControlId);

            string text = SubHeader["ReviewNotes"].ToString();

            if (textBoxReviewerNotes != null)
                textBoxReviewerNotes.Text = (text == null ? "" : text);

            #region UPDATE_RESULT_VALUE
            int maxPoint = TotalFactors;// _MORe.GetMaxPointsBySubHeaderId(SubHeader.KnowledgeBaseId, TemplateId);

            /*Update Summary Score Point value*/
            string ControlsId = "hiddenSummarycell2" + (int)MOReType + SubHeaderSequence + "3";
            HiddenField hiddenSummaryCell2 = (HiddenField)pnlMOReRequirements.FindControl(ControlsId);
            hiddenSummaryCell2.Value = (SubHeader["FilledAnswersId"].ToString() != null ? SubHeader["DefaultScore"].ToString() : "0%");
            string percentage = (SubHeader["FilledAnswersId"].ToString() != null ? SubHeader["DefaultScore"].ToString() : null);
            percentage = percentage == null || percentage.Trim() == string.Empty ? "0%" : percentage;
            string[] temp = percentage.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            int elementScore = Convert.ToInt32(temp[0]);

            ControlsId = "lblSummarycell2" + (int)MOReType + SubHeaderSequence + "3";
            Label lblSummaryCell2 = (Label)pnlMOReRequirements.FindControl(ControlsId);
            lblSummaryCell2.Text = (SubHeader["FilledAnswersId"].ToString() != null ? SubHeader["DefaultScore"].ToString() : "0%");

            /*Update Total PCMH points*/
            ControlsId = "lblSummarycell2" + (int)MOReType + SubHeaderSequence + "4";
            Label lblSummaryPoint = (Label)pnlMOReRequirements.FindControl(ControlsId);
            lblSummaryPoint.Text = Convert.ToString(((Convert.ToDecimal(elementScore) / 100) * maxPoint));

            #endregion

            calculationRules += SubHeaderSequence + ":";

            List<ScoringRule> rules = _MORe.GetScoringRulesBySubHeaderId(Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()), TemplateId);

            foreach (ScoringRule rule in rules)
            {

                calculationRules += rule.Score + ",";
                calculationRules += rule.MinYesFactor + ",";
                calculationRules += rule.MaxYesFactor + ",";

                string mustPresentFactor = rule.MustPresentFactorSequence == null ? "" : rule.MustPresentFactorSequence;
                string mustPassFactor = rule.MustPassFactorSequence == null ? "" : rule.MustPassFactorSequence;
                string absentFactor = rule.AbsentFactorSequence == null ? "" : rule.AbsentFactorSequence;

                if (mustPresentFactor.Count() > 0)
                {
                    if (mustPresentFactor != "-1")
                    {

                        string tempValue = rule.MustPresentFactorSequence;
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

                }
                else if (mustPassFactor.Count() > 0)
                {
                    if (mustPassFactor != "-1")
                    {

                        string tempValue = rule.MustPassFactorSequence;
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

                }
                else
                    calculationRules += "0,";

                if (absentFactor.Count() > 0)
                {
                    if (absentFactor != "-1")
                    {

                        string tempValue = rule.AbsentFactorSequence;
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

                }
                else
                    calculationRules += "0#";
            }
            calculationRules += "&";

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
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
                _label.ID = DEFAULT_LABEL_ID_PREFIX_OF_FACTOR_CHILD_HEADER + (int)MOReType + ElementSequence + ChildHeaderColumnIndex;

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
            Logger.PrintError(exception);
        }
    }

    protected void AddQuestionRow(DataRow Question, DataRow SubHeader, Table FactorTable, TableRow FactorRow, string FactorTitle, string StandardSequence, string ElementSequence, string FactorSequence)
    {
        try
        {

            int columnIndex = 1;
            int uploadedDocs = 0;
            int requiredDocs = 0;

            List<TemplateDocument> UploadedDocsCount = _MORe.GetDocumentUplodedCountByQuestionId(Convert.ToInt32(Question["KnowledgeBaseId"].ToString()), TemplateId, ProjectUsageId, SiteId);

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
                _textBox.ID = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + ElementSequence + FactorSequence + (int)MOReType + columnIndex;
                _textBox.ClientIDMode = ClientIDMode.Static;
                _textBox.CssClass = DEFAULT_FACTOR_TEXTBOX_CSS_CLASS;
                _textBox.MaxLength = 2;

                switch (columnIndex)
                {
                    case 1:
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "cell-sequence";
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        _label.ID = "lblfactor" + ElementSequence + FactorSequence + (int)MOReType;
                        _label.Text = FactorSequence;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 2:
                        string docInfoLink = string.Empty;
                        docInfoLink = "~/";//GetBookmark(Factor);

                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        _label.ID = "lblfactorTitle" + ElementSequence + FactorSequence + (int)MOReType;

                        _label.Text = FactorTitle;
                        _tableCell.Style.Add("vertical-align", "top");

                        if (Question["IsCritical"].ToString() != "")
                        {
                            if ((bool)Question["IsCritical"] == true)
                            {
                                _label.Style.Add("font-weight", "bold");
                                _label.ToolTip = (Question["CriticalTooltip"].ToString() != null && Question["CriticalTooltip"].ToString() != "" ? Question["CriticalTooltip"].ToString() : null);
                            }
                        }

                        _tableCell.Controls.Add(_label);
                        break;

                    case 3:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = "factor-control-column03";

                        DropDownList _dropDownList = new DropDownList();
                        _dropDownList.ID = "ddlFactorAnswer" + ElementSequence + FactorSequence;
                        _dropDownList.ClientIDMode = ClientIDMode.Static;

                        IQueryable value = _MORe.GetDropDownWeightageByQuestion(Convert.ToInt32(Question["KnowledgeBaseId"].ToString()), TemplateId);

                        _dropDownList.DataSource = value;
                        _dropDownList.DataTextField = "Text";
                        _dropDownList.DataValueField = "Value";
                        _dropDownList.DataBind();

                        int count = _MORe.GetDropDownRowsCountByQuestion(Convert.ToInt32(Question["KnowledgeBaseId"].ToString()), TemplateId);

                        if (count == 2)
                            _dropDownList.Items.RemoveAt(2);

                        List<int> newValue = _MORe.GetDropDownWeightageByQuestionId(Convert.ToInt32(Question["KnowledgeBaseId"].ToString()), TemplateId);

                        #region CALCULATE_SUMMARY_ONCLIENTSIDE

                        // hidden field to store the value
                        HiddenField hiddenOldAnswer = new HiddenField();
                        hiddenOldAnswer.ID = "hiddenOldAnswer" + ElementSequence + FactorSequence;
                        hiddenOldAnswer.ClientIDMode = ClientIDMode.Static;

                        // Depends on Summary label Id value
                        string mustPass = SubHeader["MustPass"].ToString();

                        string factorControlPrefix = "ddlFactorAnswer";
                        string SummaryCell2Id = "lblSummarycell2" + (int)MOReType + ElementSequence;


                        if (count == 3)
                        {

                            _dropDownList.Attributes.Add("onchange", "calculateSummaryWithNA('" + ElementSequence + "','"
                                + Convert.ToString((int)MOReType) + "','" + SummaryCell2Id + "','" + _dropDownList.ID + "','" +
                                factorControlPrefix + "','" + mustPass + "','" + hiddenOldAnswer.ID + "','" + newValue[0] + "','" + newValue[1] + "','" + newValue[2] + "');");
                        }
                        else
                        {

                            _dropDownList.Attributes.Add("onchange", "calculateSummaryWithoutNA('" + ElementSequence + "','"
                                + Convert.ToString((int)MOReType) + "','" + SummaryCell2Id + "','" + _dropDownList.ID + "','" +
                                factorControlPrefix + "','" + mustPass + "','" + hiddenOldAnswer.ID + "','" + newValue[0] + "','" + newValue[1] + "');");
                        }



                        #endregion

                        #region FACTOR_ANSWER
                        string markeAnswer = Question["DiscreteValue"].ToString(); //_MORe.GetQuestionAnswerByQuestionId(Convert.ToInt32(Question["KnowledgeBaseId"].ToString()), TemplateId, ProjectId);

                        if (markeAnswer != "")
                        {
                            _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(markeAnswer));
                            hiddenOldAnswer.Value = markeAnswer;
                        }
                        else
                        {
                            _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText("No"));
                            hiddenOldAnswer.Value = "No";
                        }

                        #endregion

                        _tableCell.Controls.Add(hiddenOldAnswer);
                        _tableCell.Controls.Add(_dropDownList);

                        string commentExist = (Question["IsDataBox"].ToString() != "" ? ((bool)Question["IsDataBox"] == true ? "Yes" : "No") : "No");
                        string commentNote = (Question["DataBoxHeader"].ToString() != null ? Question["DataBoxHeader"].ToString() : ""); //_MORe.GetQuestionNoteByQuestionId(Question.KnowledgeBaseId, TemplateId);

                        if (commentExist == "Yes")
                        {
                            // To Add Critical Notes
                            _image.ID = "imgFactorCriticalNote" + ElementSequence + FactorSequence + Convert.ToString((int)MOReType); ;
                            if (FactorTable.Enabled)
                            {
                                _image.CssClass = "factorCriticalNotePopUp";
                            }
                            _image.AlternateText = "Add Comment";
                            _image.ToolTip = "Add Comment";

                            #region FactorPresentOrNotAvailable
                            // Capture values against selected PopUp
                            string FCNPopUpTitle = "Comment for Header " + Convert.ToString((int)MOReType) +
                                " - SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1] +
                                " - Question " + FactorSequence;

                            #region FACTOR_COMMENTS
                            string comment = string.Empty;

                            if (Question["DataBoxComments"] != null)
                                comment = Question["DataBoxComments"].ToString();

                            _hiddenField = new HiddenField();
                            _hiddenField.ID = "hiddenfcn" + ElementSequence + FactorSequence + Convert.ToString((int)MOReType);
                            _hiddenField.Value = comment;
                            _hiddenField.ClientIDMode = ClientIDMode.Static;
                            _tableCell.Controls.Add(_hiddenField);

                            #endregion

                            _image.Attributes.Add("onclick", "fcnTrack('" + FCNPopUpTitle + "','"
                                + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)MOReType) + "','" + _hiddenField.ID
                                + "','" + commentNote + "','" + Question["KnowledgeBaseId"] + "');");


                            #endregion

                            if (comment == string.Empty)
                                _image.ImageUrl = "~/Themes/Images/factor-note-empty.png";
                            else
                                _image.ImageUrl = "~/Themes/Images/factor-note.png";

                            _tableCell.Controls.Add(_image);
                        }
                        break;

                    case 4:
                        uploadedDocs = requiredDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        #region receivedPolicies

                        _textBox.Text = (Question["PoliciesDocumentCount"].ToString() != "" ? (Convert.ToInt32(Question["PoliciesDocumentCount"].ToString()) > 0 ? Question["PoliciesDocumentCount"].ToString() : "0") : "0");
                        requiredDocs = (_textBox.Text == "" ? 0 : Convert.ToInt32(_textBox.Text));
                        uploadedDocs = GetUploadedDocsCount(UploadedDocsCount, enDocType.Policies);
                        _textBox.Text = (_textBox.Text == "0" ? null : _textBox.Text);



                        #endregion

                        _tableCell.Controls.Add(_textBox);
                        break;
                    case 6:
                        uploadedDocs = requiredDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        #region receivedReports

                        _textBox.Text = (Question["ReportsDocumentCount"].ToString() != "" ? (Convert.ToInt32(Question["ReportsDocumentCount"].ToString()) > 0 ? Question["ReportsDocumentCount"].ToString().ToString() : "0") : "0");
                        requiredDocs = (_textBox.Text == "" ? 0 : Convert.ToInt32(_textBox.Text));
                        uploadedDocs = GetUploadedDocsCount(UploadedDocsCount, enDocType.Reports);
                        _textBox.Text = (_textBox.Text == "0" ? null : _textBox.Text);
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 8:
                        uploadedDocs = requiredDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;


                        #region receivedScreenShots

                        _textBox.Text = (Question["ScreenShotsDocumentCount"].ToString() != "" ? (Convert.ToInt32(Question["ScreenShotsDocumentCount"].ToString()) > 0 ? Question["ScreenShotsDocumentCount"].ToString() : "0") : "0");
                        requiredDocs = (_textBox.Text == "" ? 0 : Convert.ToInt32(_textBox.Text));
                        uploadedDocs = GetUploadedDocsCount(UploadedDocsCount, enDocType.Screenshots);
                        _textBox.Text = (_textBox.Text == "0" ? null : _textBox.Text);
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 10:
                        uploadedDocs = requiredDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        #region receivedLogsOrTools

                        _textBox.Text = (Question["LogsOrToolsDocumentCount"].ToString() != "" ? (Convert.ToInt32(Question["LogsOrToolsDocumentCount"].ToString()) > 0 ? Question["LogsOrToolsDocumentCount"].ToString() : "0") : "0");
                        requiredDocs = (_textBox.Text == "" ? 0 : Convert.ToInt32(_textBox.Text));
                        uploadedDocs = GetUploadedDocsCount(UploadedDocsCount, enDocType.LogsOrTools);
                        _textBox.Text = (_textBox.Text == "0" ? null : _textBox.Text);
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;
                    case 12:
                        uploadedDocs = requiredDocs = 0;
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.CssClass = DEFAULT_CELL_ID_PREFIX_FOR_FACTOR_CONTROL;

                        #region receivedOtherDocs

                        _textBox.Text = (Question["OtherDocumentsCount"].ToString() != "" ? (Convert.ToInt32(Question["OtherDocumentsCount"].ToString()) > 0 ? Question["OtherDocumentsCount"].ToString() : "0") : "0");
                        requiredDocs = (_textBox.Text == "" ? 0 : Convert.ToInt32(_textBox.Text));
                        uploadedDocs = GetUploadedDocsCount(UploadedDocsCount, enDocType.OtherDocs);
                        _textBox.Text = (_textBox.Text == "0" ? null : _textBox.Text);
                        #endregion

                        _tableCell.Controls.Add(_textBox);

                        break;

                    case 5:
                    case 7:
                    case 9:
                    case 11:
                    case 13:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;

                        if (requiredDocs > uploadedDocs)
                            _tableCell.CssClass = "factor-control-hightlight";
                        else
                            _tableCell.CssClass = "factor-control-important";

                        _label.ID = DEFAULT_LABLE_ID_PREFIX_FOR_FACTOR_DOC + ElementSequence + FactorSequence + (int)MOReType + columnIndex;
                        _label.Text = uploadedDocs.ToString();
                        _label.CssClass = "factor-label";
                        _label.ClientIDMode = ClientIDMode.Static;

                        _tableCell.Controls.Add(_label);
                        break;

                    case 14:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        _tableCell.HorizontalAlign = HorizontalAlign.Center;
                        if (FactorTable.Enabled)
                        {
                            _tableCell.CssClass = "factorNotePopUp";
                        }
                        _image.ID = "imgFactorNote" + ElementSequence + FactorSequence + Convert.ToString((int)MOReType);
                        _image.ClientIDMode = ClientIDMode.Static;
                        _image.AlternateText = "Add/Edit Private Notes";
                        _image.ToolTip = "Add/Edit Private Notes";

                        // capture values against selected PopUp
                        string fnPopUpTitle = "Private Note for Header " + Convert.ToString((int)MOReType);
                        fnPopUpTitle += " - SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1];
                        fnPopUpTitle += " - Question " + FactorSequence;

                        #region FACTOR_PRIVATECOMMENT
                        string Notes = string.Empty;

                        if (Question["PrivateNotes"].ToString() != null)
                        {
                            char[] historySeparator = new char[] { '~' };
                            string[] HistoryArray = Question["PrivateNotes"].ToString().Split(historySeparator, StringSplitOptions.RemoveEmptyEntries);

                            char[] noteSeparator = new char[] { '@' };

                            if (HistoryArray.Count() > 0)
                            {
                                for (int Cout = 0; Cout < HistoryArray.Length; Cout++)
                                {
                                    string[] noteArray = HistoryArray[Cout].Split(noteSeparator, StringSplitOptions.None);

                                    Notes += (Cout + 1) + ",";
                                    Notes += noteArray[0] + ",";
                                    Notes += noteArray[1] + ",";
                                    Notes += noteArray[2].Replace(",", "~~~~~").Replace("@", "|||||") + "@";

                                }

                            }
                        }


                        _hiddenField = new HiddenField();
                        _hiddenField.ID = "hiddenPrivateNote" + ElementSequence + FactorSequence + Convert.ToString((int)MOReType);
                        _hiddenField.Value = Notes;
                        _hiddenField.ClientIDMode = ClientIDMode.Static;
                        _tableCell.Controls.Add(_hiddenField);

                        #endregion

                        _image.Attributes.Add("onclick", "fTracking('" + fnPopUpTitle + "','"
                            + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)MOReType) + "','" +
                            _hiddenField.ID + "','" + Question["KnowledgeBaseId"] + "');");

                        if (Notes.Trim() == string.Empty)
                            _image.ImageUrl = "~/Themes/Images/note-empty.png"; // set image for current popup (when no content found)
                        else
                            _image.ImageUrl = "~/Themes/Images/note.png"; // set image for current popup (when content found)                        

                        _tableCell.Controls.Add(_image);

                        break;

                    case 15:
                        _tableCell = new TableCell();
                        _tableCell.ID = "CellUploadedDocs" + ElementSequence + FactorSequence + "_" + (int)MOReType + columnIndex;
                        _tableCell.ClientIDMode = ClientIDMode.Static;
                        if (FactorTable.Enabled)
                        {
                            _tableCell.CssClass = "uploadPopUp";
                        }
                        // setting image attributes
                        _image.ID = "imgFactorUpload" + ElementSequence + FactorSequence + Convert.ToString((int)MOReType);
                        _image.ImageUrl = "~/Themes/Images/upload.png";
                        _image.ClientIDMode = ClientIDMode.Static;
                        _image.AlternateText = "Upload File";
                        _image.ToolTip = "Upload File";

                        // Capture values against selected PopUp
                        string FUDPopUpTitle = "Uploading file for Header " + Convert.ToString((int)MOReType) +
                            " - SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(ElementSequence) - 1] +
                            " - Question " + FactorSequence;

                        _image.Attributes.Add("onclick", "fudTracking('" + FUDPopUpTitle + "','"
                            + ElementSequence + "','" + FactorSequence + "','" + Convert.ToString((int)MOReType) + "','" + ProjectUsageId +
                            "','" + System.Web.HttpUtility.JavaScriptStringEncode(PracticeName) + "','" + System.Web.HttpUtility.JavaScriptStringEncode(SiteName) + "','" + Node + "','" + PracticeId + "','" + SiteId + "','" + TemplateId + "');");

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
            Logger.PrintError(exception);
        }
    }

    protected void AddSummary(Table ElementTable, DataRow SubHeader, string ElementSequence, int TotalFactors, int PresentFactor, string FactorSequence)
    {

        try
        {
            List<ScoringRule> Rules = _MORe.GetScoringRulesBySubHeaderId(Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()), TemplateId);
            string defaultScore = _MORe.GetDefaultScore(Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()), TemplateId, ProjectUsageId, SiteId);
            string mustpass;
            string[] defaultScoreArray = defaultScore.Split('%');
            if (Convert.ToInt32(defaultScoreArray[0]) >= 50)
            {
                mustpass = "Yes";
            }
            else
            { 
                mustpass = "No";
            }
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

            if (SubHeader["MustPass"].ToString() == "True")
            {
                foreach (string text in DEFAULT_SUMMARY_TEXT_MUSTPASS)
                {

                    // Add summary against the current Element

                    string currentElement = SubHeader["DisplayName"].ToString();

                    CurrentSummary = CurrentSummary + 1;
                    _tableRow = new TableRow();

                    string SummaryTitle = text;
                    string SummarySequence = CurrentSummary.ToString();


                    // First Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-cell2";
                    _label = new Label();
                    _label.ID = "lblSummarycell1" + (int)MOReType + ElementSequence + SummarySequence;
                    _label.Text = SummaryTitle;

                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);

                    // Second Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-point";
                    _label = new Label();
                    _label.ID = "lblSummarycell2" + (int)MOReType + ElementSequence + SummarySequence;

                    _label.Text = CurrentSummary == 1 ? TotalFactors.ToString() : CurrentSummary == 2 ? PresentFactor.ToString() : CurrentSummary == 3 ? defaultScore : CurrentSummary == 5 ? mustpass : "0";
                    _label.ClientIDMode = ClientIDMode.Static;

                    _hiddenField = new HiddenField();
                    _hiddenField.ID = "hiddenSummarycell2" + (int)MOReType + ElementSequence + SummarySequence;
                    _hiddenField.ClientIDMode = ClientIDMode.Static;

                    _tableCell.Controls.Add(_hiddenField);
                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);


                    if (CurrentSummary == 1)
                    {
                        // Add 3rd Cell in the begining of First Row
                        AddReviewNotes(_tableRow, ElementSequence, Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()), SubHeader["Complete"].ToString());
                    }

                    // Add Summary Row
                    summaryTable.Controls.Add(_tableRow);

                }
            }
            else
            {
                foreach (string text in DEFAULT_SUMMARY_TEXT)
                {

                    // Add summary against the current Element

                    string currentElement = SubHeader["DisplayName"].ToString();

                    CurrentSummary = CurrentSummary + 1;
                    _tableRow = new TableRow();

                    string SummaryTitle = text;
                    string SummarySequence = CurrentSummary.ToString();


                    // First Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-cell2";
                    _label = new Label();
                    _label.ID = "lblSummarycell1" + (int)MOReType + ElementSequence + SummarySequence;
                    _label.Text = SummaryTitle;

                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);

                    // Second Cell of Summary
                    _tableCell = new TableCell();
                    _tableCell.CssClass = "summary-title-point";
                    _label = new Label();
                    _label.ID = "lblSummarycell2" + (int)MOReType + ElementSequence + SummarySequence;

                    _label.Text = CurrentSummary == 1 ? TotalFactors.ToString() : CurrentSummary == 2 ? PresentFactor.ToString() : CurrentSummary == 3 ? defaultScore : "0";
                    _label.ClientIDMode = ClientIDMode.Static;

                    _hiddenField = new HiddenField();
                    _hiddenField.ID = "hiddenSummarycell2" + (int)MOReType + ElementSequence + SummarySequence;
                    _hiddenField.ClientIDMode = ClientIDMode.Static;

                    _tableCell.Controls.Add(_hiddenField);
                    _tableCell.Controls.Add(_label);
                    _tableRow.Controls.Add(_tableCell);


                    if (CurrentSummary == 1)
                    {
                        // Add 3rd Cell in the begining of First Row
                        AddReviewNotes(_tableRow, ElementSequence, Convert.ToInt32(SubHeader["KnowledgeBaseId"].ToString()), SubHeader["Complete"].ToString());
                    }

                    // Add Summary Row
                    summaryTable.Controls.Add(_tableRow);
                }
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void AddReviewNotes(TableRow tableRow, string elementSequence, int SubHeaderId, string IsComplete)
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
            _label.ID = "lblNote" + (int)MOReType + elementSequence;
            _label.Text = "Reserved for Reviewer Notes:";

            noteCell.Controls.Add(_label);
            noteRow.Controls.Add(noteCell);

            // Add CheckBox
            noteCell = new TableCell();
            noteCell.CssClass = "note-checkbox";

            _label = new Label();
            _label.ID = "lblNoteText" + (int)MOReType + elementSequence;
            _label.Text = "Mark as complete";

            noteCell.Controls.Add(_label);

            _checkbox = new CheckBox();
            _checkbox.ID = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)MOReType + elementSequence;
            _checkbox.ClientIDMode = ClientIDMode.Static;
            _checkbox.Attributes.Add("onchange", "changed();");

            // To enable/disable the Element complete CheckBox
            bool value = (IsComplete != "" ? Convert.ToBoolean(IsComplete) : false);
            _checkbox.Visible = true;
            _checkbox.Checked = value;
            noteCell.Controls.Add(_checkbox);
            noteRow.Controls.Add(noteCell);

            //Add First Row in note Table
            noteTable.Controls.Add(noteRow);

            // Second Row
            noteRow = new TableRow();

            noteCell = new TableCell();
            noteCell.ColumnSpan = 2;

            _textBox = new TextBox();
            _textBox.ID = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)MOReType + elementSequence;
            _textBox.ClientIDMode = ClientIDMode.Static;
            _textBox.CssClass = "note-textbox";
            _textBox.Text = "";
            _textBox.ToolTip = "Enter Notes";
            _textBox.TextMode = TextBoxMode.MultiLine;
            _textBox.Attributes.Add("onchange", "changed();");

            noteCell.Controls.Add(_textBox);
            noteRow.Controls.Add(noteCell);

            // Add Second Row in note table
            noteTable.Controls.Add(noteRow);

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }


    protected void SavingQuestionnaire()
    {
        try
        {

            List<KnowledgeBase> subHeader = _MORe.GetKnowledgeBaseSubHeadersByTemplateId(TemplateId, HeaderId);

            int SubHeaderSequence = 1;

            string controlId;
            string value;
            string newElement;

            controlId = value = newElement = string.Empty;

            foreach (KnowledgeBase SubHeader in subHeader)
            {

                string reviewNotes = "";
                string score = string.Empty;
                #region MarkAsComplete_HANDLING
                string checkBoxControlId = DEFAULT_NOTE_CHECKBOX_ID_PREFIX + (int)MOReType + SubHeaderSequence;
                CheckBox chkBoxElementControl = (CheckBox)pnlMOReRequirements.FindControl(checkBoxControlId);
                if (chkBoxElementControl != null)
                { _MORe.SaveCompleteBySubHeaderId(SubHeader.KnowledgeBaseId, TemplateId, chkBoxElementControl.Checked, ProjectUsageId, SiteId); }
                #endregion

                controlId = DEFAULT_NOTE_TEXTBOX_ID_PREFIX + (int)MOReType + SubHeaderSequence;
                TextBox textBoxReviewerNotes = (TextBox)pnlMOReRequirements.FindControl(controlId);
                value = textBoxReviewerNotes == null ? "-1" : textBoxReviewerNotes.Text;

                if (value != "-1")
                {
                    reviewNotes = value;

                }

                List<KnowledgeBase> Questions = _MORe.GetKnowledgeBaseQuestionsBySubHeader(SubHeader.KnowledgeBaseId, TemplateId);

                int QuestionSequence = 1;
                foreach (KnowledgeBase _questions in Questions)
                {

                    #region FactorAnswer

                    controlId = "ddlFactorAnswer" + SubHeaderSequence + QuestionSequence;
                    DropDownList ddlFactorAnswer = (DropDownList)pnlMOReRequirements.FindControl(controlId);
                    if (ddlFactorAnswer.Items.Count != 0)
                        _MORe.SaveAnswers(_questions.KnowledgeBaseId, TemplateId, ddlFactorAnswer.SelectedItem.Text, ProjectUsageId, SiteId);

                    #endregion

                    string policyText, reportText, screenText, logsText, otherText;
                    policyText = reportText = screenText = logsText = otherText = string.Empty;

                    int columnIndex = DEFAULT_HEADER_CHILD_START;
                    for (columnIndex = DEFAULT_HEADER_CHILD_START; columnIndex <= DEFAULT_HEADER_CHILD_END; columnIndex =
                        columnIndex + 2)
                    {
                        switch (columnIndex)
                        {

                            case 4:
                                controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + SubHeaderSequence + QuestionSequence + (int)MOReType + columnIndex;
                                TextBox textBoxPloicy = (TextBox)pnlMOReRequirements.FindControl(controlId);

                                policyText = textBoxPloicy == null || textBoxPloicy.Text == "" ? "0" : textBoxPloicy.Text;

                                break;

                            case 6:
                                controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + SubHeaderSequence + QuestionSequence + (int)MOReType + columnIndex; ;
                                TextBox textBoxReport = (TextBox)pnlMOReRequirements.FindControl(controlId);

                                reportText = textBoxReport == null || textBoxReport.Text == "" ? "0" : textBoxReport.Text;

                                break;

                            case 8:
                                controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + SubHeaderSequence + QuestionSequence + (int)MOReType + columnIndex; ;
                                TextBox textBoxrequiredScrnShts = (TextBox)pnlMOReRequirements.FindControl(controlId);

                                screenText = textBoxrequiredScrnShts == null || textBoxrequiredScrnShts.Text == "" ? "0" : textBoxrequiredScrnShts.Text;

                                break;
                            case 10:
                                controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + SubHeaderSequence + QuestionSequence + (int)MOReType + columnIndex; ;
                                TextBox textBoxrequiredLog = (TextBox)pnlMOReRequirements.FindControl(controlId);

                                logsText = textBoxrequiredLog == null || textBoxrequiredLog.Text == "" ? "0" : textBoxrequiredLog.Text;

                                break;
                            case 12:
                                controlId = DEFAULT_TEXTBOX_ID_PREIFX_FOR_FACTOR_DOC + SubHeaderSequence + QuestionSequence + (int)MOReType + columnIndex; ;
                                TextBox textBoxOtherDocs = (TextBox)pnlMOReRequirements.FindControl(controlId);

                                otherText = textBoxOtherDocs == null || textBoxOtherDocs.Text == "" ? "0" : textBoxOtherDocs.Text;

                                break;
                        }
                    }

                    _MORe.SaveDocumentCounts(_questions.KnowledgeBaseId, TemplateId, ProjectUsageId, SiteId, policyText, reportText, screenText, logsText, otherText);

                    QuestionSequence++;

                }

                string hiddenSummaryScoreId = "hiddenSummarycell2" + (int)MOReType + SubHeaderSequence + "3";
                HiddenField hiddenElementScore = (HiddenField)pnlMOReRequirements.FindControl(hiddenSummaryScoreId);

                score = hiddenElementScore.Value != "" ? hiddenElementScore.Value : "0%";
                _MORe.SaveScoreAndNotes(SubHeader.KnowledgeBaseId, TemplateId, score, reviewNotes, ProjectUsageId, SiteId);

                SubHeaderSequence++;

            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void SavingEvaluationNotes()
    {
        try
        {


            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {

                _MORe.SaveEvaluationNotes(TemplateId, Convert.ToInt32(hiddenSubheaderId.Value), txtElementComments.Text,
                    Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]), ProjectUsageId, SiteId);

            }

            // Update Comment In Hidden Field
            string controlId = "hiddenelement" + hiddenElementId.Value + hiddenElementPCMH.Value;

            HiddenField ElementcommentField = (HiddenField)pnlMOReRequirements.FindControl(controlId);
            ElementcommentField.Value = txtElementComments.Text;

        }


        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void SavingFactorCriticalNotes()
    {
        try
        {
            string value = txtFCNComments.Text.Trim();

            _MORe.SaveFactorNotes(TemplateId, Convert.ToInt32(hiddenQuestionId.Value), value, ProjectUsageId, SiteId);

            // Update Comment In Hidden Field
            string controlId = "hiddenfcn" + hiddenFCNElementId.Value + hiddenFCNFactorId.Value + hiddenFCNPCMH.Value;

            HiddenField fcncommentField = (HiddenField)pnlMOReRequirements.FindControl(controlId);
            fcncommentField.Value = txtFCNComments.Text;

            string fcnImageControlId = "imgFactorCriticalNote" + hiddenFCNElementId.Value + hiddenFCNFactorId.Value + hiddenFCNPCMH.Value;
            Image fcnImage = (Image)pnlMOReRequirements.FindControl(fcnImageControlId);

            if (txtFCNComments.Text.Trim() == string.Empty)
                fcnImage.ImageUrl = "~/Themes/Images/factor-note-empty.png"; // set image for current popup (when no content found)
            else
                fcnImage.ImageUrl = "~/Themes/Images/factor-note.png"; // set image for current popup (when content found)    
        }

        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void SavingFactorPrivateNote()
    {

        try
        {
            string value = txtFNComments.Text.Trim();

            if (value != string.Empty)
            {

                string saveNote = "", showNote = "";
                string comment = _MORe.GetPrivateNotes(TemplateId, Convert.ToInt32(hiddenQuestionId.Value), ProjectUsageId, SiteId);

                if (comment == "" || comment == null || comment == string.Empty)
                {

                    saveNote = String.Format("{0:MM/dd/yy}", System.DateTime.Now) + "@";
                    saveNote += Session["UserName"].ToString() + "@";
                    saveNote += value.Replace(",", "~~~~~").Replace("@", "|||||");

                }
                else
                {

                    saveNote = comment + "~";
                    saveNote += String.Format("{0:MM/dd/yy}", System.DateTime.Now) + "@";
                    saveNote += Session["UserName"].ToString() + "@";
                    saveNote += value.Replace(",", "~~~~~").Replace("@", "|||||");

                }

                _MORe.SavePrivateNotes(TemplateId, Convert.ToInt32(hiddenQuestionId.Value), saveNote, ProjectUsageId, SiteId);

                // Update Comment In Hidden Field
                string ControlId = "hiddenPrivateNote" + hiddenFNElementId.Value + hiddenFNFactorId.Value + hiddenFNPCMH.Value;
                HiddenField fncommentField = (HiddenField)pnlMOReRequirements.FindControl(ControlId);

                if (fncommentField.Value != "")
                {
                    string[] splitComment = fncommentField.Value.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] counter = splitComment[splitComment.Length - 1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int index = Convert.ToInt32(counter[0]) + 1;

                    showNote = index.ToString() + ",";
                    showNote += String.Format("{0:MM/dd/yy}", System.DateTime.Now) + ",";
                    showNote += Session["UserName"].ToString() + ",";
                    showNote += value.Replace(",", "~~~~~").Replace("@", "|||||") + "@";

                    fncommentField.Value = fncommentField.Value + showNote;
                }
                else
                {
                    showNote = "1,";
                    showNote += String.Format("{0:MM/dd/yy}", System.DateTime.Now) + ",";
                    showNote += Session["UserName"].ToString() + ",";
                    showNote += value.Replace(",", "~~~~~").Replace("@", "|||||") + "@";

                    fncommentField.Value = showNote;

                }

                string privateNoteImageControlId = "imgFactorNote" + hiddenFNElementId.Value + hiddenFNFactorId.Value + hiddenFNPCMH.Value;
                Image privateNoteImage = (Image)pnlMOReRequirements.FindControl(privateNoteImageControlId);

                if (saveNote.Trim() == string.Empty)
                    privateNoteImage.ImageUrl = "~/Themes/Images/note-empty.png"; // set image for current popup (when no content found)
                else
                    privateNoteImage.ImageUrl = "~/Themes/Images/note.png"; // set image for current popup (when content found) 


            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected int GetUploadedDocsCount(List<TemplateDocument> Tdoc, enDocType Type)
    {
        int count = 0;

        foreach (TemplateDocument doc in Tdoc)
        {

            if (doc.DocumentType == Type.ToString())
                count++;
        }

        return count;

    }

    protected string AddReferencePages(string path, int pages)
    {
        string value = string.Empty;

        if (path.Contains(".pdf"))
        {
            value = "#page=" + pages;

        }

        return value;

    }

    public void Reload()
    {
        if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == CONTROL_NAME)
        {
            if (!Page.IsPostBack)
            {
                string hostPath = Util.GetHostPath();
            }
            if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut");
            GetQuestionnaireByType();
        }
    }


    #endregion

}
