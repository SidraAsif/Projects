using System;
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
using BMTBLL.Enumeration;
using BMTBLL;
using BMTBLL.Helper;

public partial class NCQASubmission : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectId { get; set; }
    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; }
    public int TemplateId { get; set; }

    #endregion

    #region CONSTANT
    private const string CONTROL_NAME = "DocumentStore";
    private const int DEFAULT_TOTAL_COLUMNS = 7;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Factor";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Type";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Title";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Linked to...";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Edit";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Replace";
    private const string DEFAULT_HEADER_PARENT_POS7 = "Remove";
    private const string DEFAULT_STANDARD_ID_PREFIX = "lblStandard";
    private const string DEFAULT_EDIT_IMAGE_ID_PREFIX = "imgEdit";
    private const string DEFAULT_REPLACE_IMAGE_ID_PREFIX = "imgReplace";
    private const string DEFAULT_DELETE_IMAGE_ID_PREFIX = "imgDelete";
    private const string DEFAULT_STANDARD_IMAGE_ID_PREFIX = "imgStandard";
    private const string DEFAULT_ELEMENT_IMAGE_ID_PREFIX = "imgElement";
    private const string DEFAULT_PCMH_TABLE_ID_PREFIX = "PCMHTable";
    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "elementTable";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "element-table";
    private const string DEFAULT_ELEMENT_FACTOR_TABLE_CSS_CLASS = "element-factortable";
    private const string DEFAULT_ELEMENT_ID_PREFIX = "lblelement";
    private const string DEFAULT_FACTOR_TABLE_ID_PREFIX = "factorTable";
    private const string DEFAULT_FACTOR_ID_PREFIX = "lblfactor";
    private const string DEFAULT_FACTOR_CHILD_TABLE_ID_PREFIX = "factorChildTable";
    private const string DEFAULT_FACTOR_CHILD_ID_PREFIX = "lblfactorChild";
    private const string DEFAULT_FACTOR_CHILD_ATTR_TABLE_ID_PREFIX = "factorChildAttrTable";
    private const string DEFAULT_FACTOR_CHILD_ATTR_ID_PREFIX = "lblfactorChildAttr";
    private const string DEFAULT_UNASSOCIATED_TABLE_ID_PREFIX = "7";
    private static string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
    #endregion

    #region Variables
    DataTable kbTemp = new DataTable();
    DataTable dt = new DataTable();
    GetKnowledgebaseADO _KBMORe = new GetKnowledgebaseADO();
    private static string[] DEFAULT_LETTER = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
    #endregion

    #region CONTROLS
    private Table _NCQASubmissionTable;
    private TableRow _tableRow;
    private TableCell _tableCell;
    MOReBO MORe = new MOReBO();
    private Label _label;
    private Label _whiteSpaceLabel;
    private Table elementTable;
    private Table PCMHTable;
    private Image _image;
    private Literal _literal;

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;
            if (Page.IsPostBack)
            {
                Session["pcmhId"] = null;
                Session["elementId"] = null;
            }

            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == CONTROL_NAME)
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);

                QuestionBO _questionBO = new QuestionBO();
                //_questionBO.QuestionnaireId = (int)enQuestionnaireType.DetailedQuestionnaire;
                _questionBO.ProjectUsageId = ProjectUsageId;

                //int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(PracticeId);
                //string recievedQuestionnaire = _questionBO.GetQuestionnaireByType(medicalGroupId);

                //Session[enSessionKey.NCQAQuestionnaire.ToString()] = recievedQuestionnaire;

                //if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                //{
                //    string RecievedQuestionnaire = Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();
                //    questionnaire = XDocument.Parse(RecievedQuestionnaire);
                //    Session["NCQAQuestionnaireId"] = (int)enQuestionnaireType.DetailedQuestionnaire;

                GenerateLayOut();
                GetPCMHList(_NCQASubmissionTable);
                List<TemplateDocument> tDocs = MORe.GetUnassociatedDocs(ProjectUsageId, SiteId, TemplateId);
                if (tDocs.Count != 0)
                    GetUnAssiciatedRowList(_NCQASubmissionTable);
                message.Clear("");

                //}
                //else
                //    message.Info("Questionnaire against the selected site doesn't exist.");
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }
    #endregion

    #region FUNCTIONS

    protected void GenerateLayOut()
    {
        try
        {
            //if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
            //{
            //    string RecievedQuestionnaire = Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();
            //    questionnaire = XDocument.Parse(RecievedQuestionnaire);

            lblSiteInfo.Text = SiteName;
            _NCQASubmissionTable = new Table();
            _NCQASubmissionTable.ID = "NCQASubmissionTable";
            _NCQASubmissionTable.ClientIDMode = ClientIDMode.Static;
            pnlNCQASummary.Controls.Clear();
            pnlNCQASummary.Controls.Add(_NCQASubmissionTable);

            LoadHeader();
            //}
            //else
            //    message.Info("Questionnaire against the selected site doesn't exist.");

        }
        catch (Exception exception)
        {
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
                _tableCell.ID = "HeaderCell" + Convert.ToSingle(ColumnIndex);
                _tableCell.CssClass = "header";
                _tableCell.ClientIDMode = ClientIDMode.Static;

                switch (ColumnIndex)
                {
                    case 1:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS1;
                        _tableCell.Width = 110;
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Width = 330;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Width = 325;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Width = 160;
                        break;

                    case 5:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS5;
                        _tableCell.Width = 65;
                        break;

                    case 6:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS6;
                        _tableCell.Width = 55;
                        break;

                    case 7:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS7;
                        _tableCell.Width = 55;
                        break;

                    default:
                        break;
                }
                _tableRow.Controls.Add(_tableCell);
            }

            _NCQASubmissionTable.Controls.Add(_tableRow);

            _tableRow = new TableRow();
            _tableRow.ClientIDMode = ClientIDMode.Static;
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 7;
            _tableRow.Controls.Add(_tableCell);

            _NCQASubmissionTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void GetPCMHList(Table masterTable)
    {
        try
        {
            //XElement root = questionnaire.Root;
            hdnTemplateId.Value = TemplateId.ToString();
            Session["TemplateId"] = TemplateId;
            hdnSiteId.Value = SiteId.ToString();
            hiddenPracticeName.Value = PracticeName;
            hiddenSiteName.Value = SiteName;
            List<KnowledgeBase> headers = MORe.GetTemplateHeaders(TemplateId);
            foreach (KnowledgeBase header in headers)
            {

                List<int> UnFilledSubHeaders = MORe.CheckFilledAnswers(TemplateId, ProjectUsageId, SiteId, header.KnowledgeBaseId);
                if (UnFilledSubHeaders.Count > 0)
                {

                    foreach (int id in UnFilledSubHeaders)
                    {
                        MORe.SaveFilledAnswerForBlankTemplates(TemplateId, ProjectUsageId, SiteId, id, true);
                    }
                }
                List<int> UnFilledQuestions = MORe.CheckFilledAnswersQuestions(TemplateId, ProjectUsageId, SiteId, header.KnowledgeBaseId);

                if (UnFilledQuestions.Count > 0)
                {

                    foreach (int id in UnFilledQuestions)
                    {
                        MORe.SaveFilledAnswerForBlankTemplates(TemplateId, ProjectUsageId, SiteId, id, false);
                    }
                }
            }

            dt = _KBMORe.GetAllTemplateDocuments(TemplateId, ProjectUsageId, SiteId);

            string standardSequence = string.Empty;
            int count = 0;

            // Add all available standards
            foreach (KnowledgeBase header in headers)
            {
                count = count + 1;

                standardSequence = count.ToString();
                string standardTitle = header.Name;

                _tableRow = new TableRow();
                _tableCell = new TableCell();
                _tableCell.ColumnSpan = 7;

                _image = new Image();
                _image.Attributes.Add("onclick", "javascript:PCMHtoggle('" + standardSequence + "');");
                _image.ClientIDMode = ClientIDMode.Static;
                _image.ID = DEFAULT_ELEMENT_IMAGE_ID_PREFIX + standardSequence;

                string lastPCMHId = Session["pcmhId"] == null ? string.Empty : Session["pcmhId"].ToString();
                if (lastPCMHId == standardSequence)
                    _image.ImageUrl = "../Themes/Images/Minus-1.png";
                else
                    _image.ImageUrl = "../Themes/Images/Plus-1.png";

                _image.CssClass = "toggle-img";
                _tableCell.Controls.Add(_image);

                _whiteSpaceLabel = new Label();
                _whiteSpaceLabel.Text = "&nbsp;&nbsp;";
                _tableCell.Controls.Add(_whiteSpaceLabel);

                _label = new Label();
                _label.Text = standardTitle;
                _label.Width = 707;
                _label.CssClass = "element-label";
                _tableCell.Controls.Add(_label);

                _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
                _tableCell.Height = 20;
                _tableRow.Controls.Add(_tableCell);

                masterTable.Controls.Add(_tableRow);

                AddPCMHTable(header, standardSequence);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddPCMHTable(KnowledgeBase Element, string elementSequence)
    {
        try
        {
            string lastPCMHId = Session["pcmhId"] == null ? string.Empty : Session["pcmhId"].ToString();

            PCMHTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            PCMHTable.ID = DEFAULT_PCMH_TABLE_ID_PREFIX + elementSequence;
            PCMHTable.ClientIDMode = ClientIDMode.Static;

            PCMHTable.CssClass = DEFAULT_ELEMENT_TABLE_CSS_CLASS;

            if (lastPCMHId == elementSequence)
            {
                PCMHTable.Style.Add("Display", "Table");
                Session["pcmhId"] = string.Empty;
            }
            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Controls.Add(PCMHTable);
            _tableRow.Controls.Add(_tableCell);
            _NCQASubmissionTable.Controls.Add(_tableRow);

            AddStandards(Element, elementSequence, PCMHTable);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandards(KnowledgeBase Element, string pcmhSequenceId, Table PCMHTable)
    {
        try
        {
            int count = 0;
            kbTemp = _KBMORe.GetAllKnowledgeBaseOfTemplate(TemplateId, ProjectUsageId, SiteId, Element.KnowledgeBaseId);
            foreach (DataRow standard in kbTemp.Rows)
            {
                count = count + 1;
                string elementSequenceId = count.ToString();
                List<NCQADetails> _listOfNCQADetail = GetDocuments(dt, Convert.ToInt32(standard["knowledgebaseId"].ToString()), pcmhSequenceId, elementSequenceId);
                AddElementsTable(PCMHTable, standard, pcmhSequenceId, elementSequenceId);
                AddStandardRow(_listOfNCQADetail);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsTable(Table PCMHTable, DataRow standard, string pcmhSequenceId, string elementSequenceId)
    {
        try
        {
            string standardSequence = elementSequenceId;
            string standardTitle = standard["Name"].ToString();
            string lastElementId = Session["elementId"] == null ? string.Empty : Session["elementId"].ToString();

            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 7;

            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:PCMHElementtoggle('" + pcmhSequenceId + "','" + standardSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = DEFAULT_STANDARD_IMAGE_ID_PREFIX + pcmhSequenceId + standardSequence;

            if (lastElementId == standardSequence)
                _image.ImageUrl = "../Themes/Images/Minus-1.png";
            else
                _image.ImageUrl = "../Themes/Images/Plus-1.png";

            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp;&nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = standardTitle;
            _label.Width = 690;
            _label.CssClass = "element-elementlabel";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            PCMHTable.Controls.Add(_tableRow);

            elementTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + pcmhSequenceId + standardSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;
            elementTable.CssClass = DEFAULT_ELEMENT_FACTOR_TABLE_CSS_CLASS;

            if (lastElementId == standardSequence)
            {
                elementTable.Style.Add("Display", "Table");
                Session["elementId"] = string.Empty;
            }

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            PCMHTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(List<NCQADetails> _listOfNCQADetail)
    {
        try
        {

            foreach (NCQADetails ncqaDetails in _listOfNCQADetail)
            {
                string sequenceId = ncqaDetails.FactorSequence.ToString();
                string type = ncqaDetails.Type;
                string title = ncqaDetails.Title;
                string linkedTo = ncqaDetails.DocLinkedTo;

                _tableRow = new TableRow();
                _tableCell = new TableCell();

                //SequenceId

                _label = new Label();
                _label.ID = "lblSequence";
                _label.Text = sequenceId;
                _label.Width = 40;
                _label.ClientIDMode = ClientIDMode.Static;
                //_label.CssClass = "screen-topic";

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableCell.Width = 40;
                _tableRow.Controls.Add(_tableCell);


                //Type

                _label = new Label();
                _label.ID = "lblType";
                _label.Text = type;
                _label.Width = 190;
                _label.ClientIDMode = ClientIDMode.Static;
                //_label.CssClass = "screen-topic";

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableCell.Width = 190;
                _tableRow.Controls.Add(_tableCell);

                //Title

                _label = new Label();
                _label.ID = "lblTitle";
                _label.Text = title;
                _label.Width = 193;
                _label.ClientIDMode = ClientIDMode.Static;
                //_label.CssClass = "screen-topic";

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableCell.Width = 193;
                _tableCell.CssClass = "element-cell";
                _tableRow.Controls.Add(_tableCell);

                //Lindked To...

                _label = new Label();
                _label.ID = "lblLinkedTo";
                _label.Text = linkedTo;
                _label.Width = 105;
                _label.ClientIDMode = ClientIDMode.Static;
                //_label.CssClass = "screen-topic";


                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableCell.Width = 105;
                _tableCell.CssClass = "element-cell";
                _tableRow.Controls.Add(_tableCell);


                //Edit
                _tableCell = new TableCell();
                _tableCell.CssClass = "image-table";
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Width = 50;
                _tableRow.Controls.Add(_tableCell);

                if (!(ncqaDetails.Type.Contains("Note") || ncqaDetails.Type.Contains("Comment")))
                {
                    _image = new Image();
                    _image.ClientIDMode = ClientIDMode.Static;
                    _image.ID = DEFAULT_EDIT_IMAGE_ID_PREFIX + sequenceId;
                    _image.ToolTip = "Edit";
                    _image.Attributes.Add("onclick", "javascript:FileMove('" + ncqaDetails.PCMHSequence + "', '" + ncqaDetails.ElementSequence + "', '" + ncqaDetails.FactorSequence + "', '" + ncqaDetails.File + "', '" + ncqaDetails.DocName + "', '" + ncqaDetails.ReferencePage + "', '" + ncqaDetails.RelevancyLevel + "', '" + ncqaDetails.DocType + "','" + ncqaDetails.FactorTitle + "','" + ncqaDetails.ProjectUsageId.ToString() + "');");
                    _image.ImageUrl = "../Themes/Images/edit-16.png";
                    _tableCell.Controls.Add(_image);
                }

                //Replace
                _tableCell = new TableCell();
                _tableCell.CssClass = "image-table";
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Width = 55;
                _tableRow.Controls.Add(_tableCell);
                if (!(ncqaDetails.Type.Contains("Note") || ncqaDetails.Type.Contains("Comment")))
                {
                    _image = new Image();
                    _image.ClientIDMode = ClientIDMode.Static;
                    _image.ID = DEFAULT_REPLACE_IMAGE_ID_PREFIX + sequenceId;
                    _image.ToolTip = "Replace";
                    _image.CssClass = "Replace-popup";
                    _image.ImageUrl = "../Themes/Images/replace.png";
                    string popupTitle = "Replacing file " + ncqaDetails.DocName;
                    _image.Attributes.Add("onclick", "javascript:ProcessReplaceFile('" + ncqaDetails.DocName + "','" + ncqaDetails.ElementSequence + "','" + ncqaDetails.FactorSequence + "','" + ncqaDetails.PCMHSequence + "','" + ProjectUsageId + "','" + "NCQA Submission" +
                             "','" + PracticeId + "','" + SiteId + "','" + ncqaDetails.DocName + "','" + ncqaDetails.ReferencePage + "','" + ncqaDetails.RelevancyLevel + "','" + ncqaDetails.File + "','" + ncqaDetails.DocLinkedTo + "','" + ncqaDetails.Type + "');");
                    _tableCell.Controls.Add(_image);
                }

                //Remove
                _tableCell = new TableCell();
                _tableCell.CssClass = "image-table";
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Width = 55;
                _tableRow.Controls.Add(_tableCell);

                if (!(ncqaDetails.Type.Contains("Note") || ncqaDetails.Type.Contains("Comment")))
                {
                    _image = new Image();
                    _image.ClientIDMode = ClientIDMode.Static;
                    _image.ID = DEFAULT_DELETE_IMAGE_ID_PREFIX + sequenceId;
                    _image.ToolTip = "Remove";
                    _image.Attributes.Add("onclick", "javascript:ProcessDeleteFile('MOReDeleteFiles.aspx?pcmh=" + ncqaDetails.PCMHSequence + "&element=" + ncqaDetails.ElementSequence + "&factor=" + ncqaDetails.FactorSequence + "&file=" + ncqaDetails.File + "|" + ncqaDetails.DocName + "|" + "&project=" + ncqaDetails.ProjectUsageId + "&practiceId=" + ncqaDetails.PracticeId + "&siteId=" + ncqaDetails.SiteId + "&pageNo=2','" + ncqaDetails.PCMHSequence + "','" + ncqaDetails.ElementSequence + "','" + ncqaDetails.FactorSequence + "','" + ncqaDetails.DocType + "','" + ncqaDetails.FactorTitle + "','" + ncqaDetails.DocLinkedTo + "');");
                    _image.ImageUrl = "../Themes/Images/remove-16.png";
                    _tableCell.Controls.Add(_image);
                }



                elementTable.Controls.Add(_tableRow);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void GetUnAssiciatedRowList(Table masterTable)
    {
        try
        {
            string standardSequence = string.Empty;
            List<KnowledgeBase> headers = MORe.GetTemplateHeaders(TemplateId);
            standardSequence = (headers.Count + 1).ToString();
            string standardTitle = "UNASSOCIATED DOCUMENTS";

            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 7;

            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:PCMHtoggle('" + standardSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = DEFAULT_ELEMENT_IMAGE_ID_PREFIX + standardSequence;

            string lastPCMHId = Session["pcmhId"] == null ? string.Empty : Session["pcmhId"].ToString();
            if (lastPCMHId == standardSequence)
                _image.ImageUrl = "../Themes/Images/Minus-1.png";
            else
                _image.ImageUrl = "../Themes/Images/Plus-1.png";

            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp;&nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = standardTitle;
            _label.Width = 707;
            _label.CssClass = "element-label";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);

            masterTable.Controls.Add(_tableRow);

            AddUnAssociatedTable(standardSequence);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddUnAssocitedDocRow(List<TemplateDocument> tdocs, Table PCMHTable, string PCMHSequence)
    {
        try
        {
            int count = 0;
            foreach (TemplateDocument docFile in tdocs)
            {
                count++;
                _tableRow = new TableRow();

                _label = new Label();
                _label.ID = "UnAssocitedDocTitle" + count.ToString();
                _label.Text = "UnAssociated Documents";
                _label.Width = 250;
                _label.ClientIDMode = ClientIDMode.Static;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);
                _tableCell.Width = 250;
                _tableRow.Controls.Add(_tableCell);

                string location = docFile.Path;
                string name = docFile.Name;

                _literal = new Literal();
                _literal.ID = DEFAULT_FACTOR_CHILD_ATTR_ID_PREFIX + count;
                _literal.Text += "<a target='_blank' href='" + location + "'>" + docFile.Name + "</a>";

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_literal);
                _tableCell.Width = 193;
                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Text = "&nbsp";
                _tableCell.Width = 105;
                _tableRow.Controls.Add(_tableCell);


                //Edit
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Text = "&nbsp";
                _tableCell.Width = 35;
                _tableRow.Controls.Add(_tableCell);


                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Text = "&nbsp";
                _tableCell.Width = 35;
                _tableRow.Controls.Add(_tableCell);

                //Remove
                _tableCell = new TableCell();
                _tableCell.CssClass = "image-table";
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Width = 55;
                _tableRow.Controls.Add(_tableCell);

                location = location.Substring(location.LastIndexOf('/') + 1);

                _image = new Image();
                _image.ClientIDMode = ClientIDMode.Static;
                _image.ID = DEFAULT_DELETE_IMAGE_ID_PREFIX + count;
                _image.ToolTip = "Remove";
                _image.Attributes.Add("onclick", "javascript:ProcessDeleteUnAssociatedFile('MOReDeleteFiles.aspx?pcmh=" + PCMHSequence + "&element=0&factor=0&file=" + docFile.Path + "&project=" + ProjectUsageId + "&practiceId=" + PracticeId + "&siteId=" + SiteId + "','" + "7" + count.ToString() + "');");
                _image.ImageUrl = "../Themes/Images/remove-16.png";
                _tableCell.Controls.Add(_image);

                PCMHTable.Controls.Add(_tableRow);
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddUnAssociatedTable(string elementSequence)
    {
        try
        {
            PCMHTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            PCMHTable.ID = DEFAULT_PCMH_TABLE_ID_PREFIX + elementSequence;
            PCMHTable.ClientIDMode = ClientIDMode.Static;

            PCMHTable.CssClass = DEFAULT_ELEMENT_FACTOR_TABLE_CSS_CLASS;

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Controls.Add(PCMHTable);
            _tableRow.Controls.Add(_tableCell);
            _NCQASubmissionTable.Controls.Add(_tableRow);

            List<TemplateDocument> tDocs = MORe.GetUnassociatedDocs(ProjectUsageId, SiteId, TemplateId);

            AddUnAssocitedDocRow(tDocs, PCMHTable, elementSequence);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected List<NCQADetails> GetDocuments(DataTable documents, int knowledgebaseId, string pcmhSequenceId, string elementSequenceId)
    {
        try
        {
            List<NCQADetails> _listOfNCQADoc = new List<NCQADetails>();

            #region VARIABLES
            string factorSequence = string.Empty;
            string location = string.Empty;
            string type = string.Empty;
            string title = string.Empty;
            DateTime? lastUploadedDate = null;
            string docLinkedTo = string.Empty;
            string docName = string.Empty;
            string referencePage = string.Empty;
            string relevancyLevel = string.Empty;
            string docType = string.Empty;
            string factorTitle = string.Empty;
            #endregion

            DataRow[] elementData = documents.Select("KnowledgeBaseId = " + knowledgebaseId);
            DataRow[] factorData = documents.Select("ParentKnowledgeBaseId = " + knowledgebaseId);
            DataView view = documents.DefaultView;
            // set the row filtering on the view...
            view.RowFilter = "ParentKnowledgeBaseId =" + knowledgebaseId;
            DataTable distinctTable = view.ToTable("DistinctTable", true, "KnowledgeBaseTemplateId", "PrivateNotes", "Sequence");
            
            foreach (DataRow Doc in elementData)
            {
                #region AddingEvaluationComments
                string evaluationComments = Doc["EvaluationNotes"].ToString();
                type = "Evaluation Comment";
                title = "<p><a href='#' class='tt'>Evaluation Comment for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTER[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += evaluationComments;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (evaluationComments != string.Empty && evaluationComments != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        PracticeId, SiteId, ProjectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion
            }
            foreach (DataRow Doc in factorData)
            {
                # region AddingDocuments
                location = Doc["DocPath"].ToString().Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                if (location != string.Empty)
                {
                    string[] splitPath = location.Split('/');
                    int lastIndex = splitPath.Count() - 1;
                    string fileName = splitPath[lastIndex];
                    string timeString = fileName.Substring(0, 14);
                    lastUploadedDate = DateTime.ParseExact(timeString, "yyyyMMddHHmmss", null);

                    if (Doc["Name"].ToString() == "" || Doc["Name"].ToString() == null)
                    {
                        int startIndex = Doc["Path"].ToString().LastIndexOf('/') + 1;
                        int length = Doc["Path"].ToString().LastIndexOf('.') - startIndex;
                        docName = Doc["Path"].ToString().Substring(startIndex, length);
                    }
                    else
                        docName = Doc["Name"].ToString();

                    docName = docName.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");

                    title = "<a href='" + location + "' target='_blank'>" + docName + "</a>";
                    docType = Doc["DocumentType"].ToString();
                    MOReBO MORe = new MOReBO();
                    docLinkedTo = MORe.GetDocumentLinks(Doc["DocPath"].ToString(), TemplateId, pcmhSequenceId, elementSequenceId, Doc["Sequence"].ToString(), ProjectUsageId, SiteId);//string.Empty;

                    if (docType == enTemplateDocumentType.PoliciesOrProcess.ToString() || docType == enTemplateDocumentType.ReportsOrLogs.ToString() || docType == enTemplateDocumentType.ScreenshotsOrExamples.ToString() ||
                            docType == enTemplateDocumentType.RRWB.ToString() || docType == enTemplateDocumentType.Extra.ToString())
                    {
                        type = docType = docType.Replace("Or", "/");
                    }
                    else if (docType == enDocType.Policies.ToString() || docType == enDocType.Reports.ToString() || docType == enDocType.Screenshots.ToString() ||
                            docType == enDocType.LogsOrTools.ToString() || docType == enDocType.OtherDocs.ToString())
                    {
                        DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                        type = docType = documentTypeMappingBO.GetDocumentType(docType);
                        type = docType = docType.Replace("Or", "/");

                    }

                    relevancyLevel = Doc["RelevencyLevel"].ToString();
                    referencePage = Doc["ReferencePages"].ToString();

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, Doc["Sequence"].ToString(), location, type, title, lastUploadedDate, docLinkedTo,
                            PracticeId, SiteId, ProjectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }
                #endregion
            }
            foreach (DataRow Doc in factorData)
            {
                #region AddingFactorComments
                if (Doc["DataBoxComments"].ToString() != null)
                {
                    type = "Factor Comment";
                    title = "<p><a href='#' class='tt'>" + Doc["DataBoxHeader"];
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                    title += Doc["DataBoxComments"];
                    title += "</span><span class='bottom'></span></span></a></p>";

                    if (Doc["DataBoxComments"].ToString() != string.Empty)
                    {
                        _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, Doc["Sequence"].ToString(), string.Empty, type, title, null, string.Empty,
                            PracticeId,SiteId, ProjectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));
                    }
                }
                #endregion
            }
            foreach (DataRow Doc in distinctTable.Rows)
            {
                #region AddingPrivateNotes
                string privateNotes = Doc["PrivateNotes"].ToString();

                type = "Note for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1] + " - " + "Factor " + factorSequence;

                title = "<p><a href='#' class='tt'>" + "Note";
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";

                if (privateNotes != null)
                {

                    string[] History = privateNotes.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

                    int notesCount = 1;
                    foreach (string comment in History)
                    {
                        string[] PrivateNotes = comment.Split(new char[] { '@' }, StringSplitOptions.None);

                        title += notesCount.ToString() + ". " + PrivateNotes[2];
                        title += "<span class='notice'>(" + PrivateNotes[1] + " on " + PrivateNotes[0] + ")</span><br />";
                        notesCount++;
                    }

                    title += "</span><span class='bottom'></span></span></a></p>";

                    if (privateNotes != string.Empty)
                        _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, Doc["Sequence"].ToString(), string.Empty, type, title, null, string.Empty,
                            PracticeId, SiteId, ProjectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                }
                #endregion
            }
            foreach (DataRow Doc in elementData)
            {
 
                #region AddingReviwerNotes
                string reviewNotes = Doc["ReviewNotes"].ToString();
                type = "Reviewer Notes";
                title = "<p><a href='#' class='tt'>Reviewer Notes for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += reviewNotes;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (reviewNotes != string.Empty && reviewNotes != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        PracticeId, SiteId, ProjectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion
            }
            return _listOfNCQADoc.ToList();
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    #endregion
}
