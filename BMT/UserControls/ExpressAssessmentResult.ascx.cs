#region Modification History

//  ******************************************************************************
//  Module        : Express Assessment Report
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;

public partial class ExpressAssessmentResult : System.Web.UI.UserControl
{
    #region PROPERTIES
    public enQuestionnaireType _questionnaireType { get; set; }
    public int ProjectUsageId { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; }
    public int SectionId { get; set; }
    #endregion

    #region CONSTANTS
    private const string DEFAULT_HEADER_TEXT = "Express PCMH Readiness Assessment Report – Generated on ";

    private const int DEFAULT_MAX_POINTS = 150;

    private const string DEFAULT_CRITICAL_MSG_Q1 = "It is extremely difficult to achieve Patient Centered Medical Home " +
        "status without an EHR. EHR adoption decisions should be reconsidered before proceeding with PCMH recognition.";

    private const string DEFAULT_CRITICAL_MSG_Q4 = "Patient Centered Medical Home requires access to providers after business hours. " +
     "Providers should consider willingness and ability to take after hours calls.";

    private const string DEFAULT_CRITICAL_MSG_Q15 = "Patient Centered Medical Home standards may require some additional expenses on your part. " +
     "It would be beneficial for the practice to obtain an accurate financial assessment at this time.";

    // Score=>120
    private const string DEFAULT_EXCELLENT_SCORE_MSG = "Your practice is very well positioned to become a Patient Centered Medical Home.";

    // 75=<Score<120
    private const string DEFAULT_BEST_SCORE_MSG = "Transformation to a Patient Centered Medical Home will require some changes to your practice.";

    // 35=<Score<75
    private const string DEFAULT_GOOD_SCORE_MSG = "Transformation to a Patient Centered Medical Home will require significant effort.";

    // Score  <35
    private const string DEFAULT_POOR_SCORE_MSG = "Transformation to a Patient Centered Medical Home will require major changes to your practice.";

    private const string DEFAULT_REPORT_TITLE = "Express Assessment Report";

    private const string DEFAULT_REPORT_SAVE_TITLE = "Express PCMH Readiness Assessment Report";

    // Default ContentType
    private const string DEFAULT_DOC_CONTENT_TYPE = "UploadedDocuments";

    #endregion

    #region VARIABLE
    List<string> _warningMesasgeList = new List<string>();
    private static List<ExpressAssessmentDetail> _expressAssessmentDetailList = new List<ExpressAssessmentDetail>();
    private static List<ExpressAssessmentDetail> _expressAssessmentWarningList = new List<ExpressAssessmentDetail>();
    int score;
    private int userId;
    private string UserType;
    private int practiceId;
    private Table _resultProgressTable;
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserApplicationId"] != null &&
                  Session["UserType"] != null &&
                  Session["PracticeId"] != null)
            {
                userId = Convert.ToInt32(Session["UserApplicationId"]);
                UserType = Session["UserType"].ToString();
                practiceId = Convert.ToInt32(Session["PracticeId"]);

                _expressAssessmentDetailList.Clear();
                _expressAssessmentWarningList.Clear();

                // Configure Questionnaire Type
                _questionnaireType = enQuestionnaireType.SimpleQuestionnaire;

                // Create Questionnaire
                GetQuestionnaire(_questionnaireType);
            }
            else
            {
                SessionHandling sessionHandling = new SessionHandling();
                sessionHandling.ClearSession();
                Response.Redirect("~/Account/Login.aspx");
            }


        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void btnsSaveDoc_click(object sender, EventArgs e)
    {
        try
        {
            PrintSummary(false, DEFAULT_REPORT_SAVE_TITLE);
            message.Success("Report has been saved successfully");
        }
        catch (Exception exception)
        {

            Logger.PrintError(exception);
            message.Error("Report couldn't be saved. Please try again/ Contact your site Administrator.");
        }

    }

    protected void btnPrintDoc_click(object sender, EventArgs e)
    {
        try
        {
            PrintSummary(true, DEFAULT_REPORT_TITLE);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("Report couldn't be generated. Please try again/ Contact your site Administrator.");
        }

    }

    #endregion

    #region FUNCTIONS

    protected void GetQuestionnaire(enQuestionnaireType questionnaireType)
    {
        try
        {
            score = 0;
            
            _warningMesasgeList.Clear();
            lblSiteName.Text = SiteName;

            QuestionBO _questionBO = new QuestionBO();

            string receivedQuestion = "";
            XDocument filledQuestionnaireDocument = null;
            if (questionnaireType == enQuestionnaireType.SimpleQuestionnaire)
            {
                _questionBO.QuestionnaireId = (int)enQuestionnaireType.SimpleQuestionnaire;
                _questionBO.ProjectUsageId = ProjectUsageId;
                _questionBO.SiteId = SiteId;
                // get questionnaire
                receivedQuestion = _questionBO.GetQuestionnaireByType();

                if (receivedQuestion != string.Empty)
                    filledQuestionnaireDocument = XDocument.Parse(receivedQuestion); // Create XML object using string
                else
                    return;
            }

            // Generate Questionnaire
            GenerateResult(filledQuestionnaireDocument);

        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void GenerateResult(XDocument filledDocument)
    {
        try
        {

            lblTopHeaderText.Text = DEFAULT_HEADER_TEXT + String.Format("{0:MM/dd/yyyy}", System.DateTime.Now);
            _resultProgressTable = new Table();

            pnlResultProgress.Controls.Add(_resultProgressTable);

            XElement FilledQuestionniareElement = filledDocument.Root;


            foreach (XElement _question in FilledQuestionniareElement.Elements())
            {
                string lastChoiceType = _question.Attribute("type").Value.ToString();

                if (lastChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    CreateQuestion(_question, enQuestionChoiceType.SingleChoice);

                else if (lastChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    CreateQuestion(_question, enQuestionChoiceType.MultiChoice);

                else
                    break;

            }

            CreateButton();
            CalculateResult();
            FetchingErrorMessage();


        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void CreateQuestion(XElement question, enQuestionChoiceType questionChoiceType)
    {

        try
        {
            // variables to store values for report start here
            string CurrentQuestion;
            string CurrentAnswer;
            string critical;

            // variables to store values for report close here
            TableRow _resultRow = new TableRow();
            TableCell _resultcell = new TableCell();
            Label _label = new Label();

            #region Single-ChoiceType_Answer
            if (questionChoiceType == enQuestionChoiceType.SingleChoice)
            {
                IEnumerable<XElement> AnswerList = from answerRecord in question.Descendants("Answer")
                                                   select answerRecord;
                string selectedSequence = string.Empty;
                string SelectedAnswer = string.Empty;
                string answerPoint = string.Empty;

                if (AnswerList.Count() > 0)
                {
                    // Fetching Selected Answer
                    foreach (XElement selectedChoice in AnswerList.Elements())
                    {
                        selectedSequence = selectedChoice.Attribute("sequence").Value.ToString();
                        break;
                    }
                }

                // Fetching Selected Answer value
                foreach (XElement availableChoices in question.Elements())
                {
                    string nestedElement = availableChoices.Name.ToString();

                    if (nestedElement != enQuestionnaireElements.Answer.ToString())
                    {

                        string currentSequence = availableChoices.Attribute("sequence").Value.ToString();
                        if (currentSequence == selectedSequence)
                        {
                            SelectedAnswer = availableChoices.Attribute("option").Value.ToString();
                            continue;
                        }
                        else
                            continue;
                    }
                    // getting Point against the selected Question
                    else if (nestedElement == enQuestionnaireElements.Answer.ToString())
                    {
                        answerPoint = availableChoices.Attribute("points").Value.ToString();
                        score = score + Convert.ToInt32(answerPoint); break;
                    }
                }

                // Creating Question
                CurrentQuestion = question.Attribute("sequence").Value + ". " + question.Attribute("title").Value;
                CurrentAnswer = SelectedAnswer;

                _label.ID = "lblQuestion" + question.Attribute("sequence").Value.ToString();
                _label.Text = CurrentQuestion;
                _label.CssClass = "question-label";
                _resultcell.Controls.Add(_label);
                _resultRow.Cells.Add(_resultcell);

                // Write Answers
                _resultcell = new TableCell();
                _label = new Label();
                _label.ID = "lblAnswer" + question.Attribute("sequence").Value.ToString();
                _label.Text = SelectedAnswer;

                if (answerPoint == "0")
                {
                    _label.CssClass = "critical-deficiencies";
                    critical = "Yes";
                }
                else
                {
                    _label.CssClass = "answer-label";
                    critical = "No";
                }

                AddWarningMessage(answerPoint, question.Attribute("sequence").Value.ToString());

                _resultcell.Controls.Add(_label);
                _resultRow.Cells.Add(_resultcell);

                // store Row in list
                _expressAssessmentDetailList.Add(new ExpressAssessmentDetail(CurrentQuestion, CurrentAnswer, critical));
            }

            #endregion

            #region MULTI_CHOICE_ANSWER
            else if (questionChoiceType == enQuestionChoiceType.MultiChoice)
            {
                IEnumerable<XElement> AnswerList = from answerRecord in question.Descendants("Answer")
                                                   select answerRecord;
                List<string> selectedSequence = new List<string>();
                List<string> SelectedAnswer = new List<string>();
                string answerPoint = string.Empty;
                if (AnswerList.Count() > 0)
                {
                    /*Fetching selected Answer*/
                    foreach (XElement selectedChoice in AnswerList.Elements())
                    {
                        selectedSequence.Add(selectedChoice.Attribute("sequence").Value.ToString());
                    }
                }

                /*Extracting List of selected choices*/
                foreach (XElement availableChoices in question.Elements())
                {
                    string nestedElement = availableChoices.Name.ToString();

                    if (nestedElement != enQuestionnaireElements.Answer.ToString())
                    {

                        string currentSequence = availableChoices.Attribute("sequence").Value.ToString();

                        foreach (string index in selectedSequence)
                        {

                            if (currentSequence == index)
                            {
                                SelectedAnswer.Add(availableChoices.Attribute("option").Value.ToString()); continue;
                            }
                            else { continue; }
                        }
                    }
                    /*getting point against the selected choices*/
                    else if (nestedElement == enQuestionnaireElements.Answer.ToString())
                    {
                        answerPoint = availableChoices.Attribute("points").Value.ToString();
                        score = score + Convert.ToInt32(answerPoint); break;
                    }
                }

                /*Writing Question*/
                CurrentQuestion = question.Attribute("sequence").Value + ". " + question.Attribute("title").Value;

                _label.ID = "lblQuestion" + question.Attribute("sequence").Value.ToString();
                _label.Text = CurrentQuestion;
                _label.CssClass = "question-label";
                _resultcell.Controls.Add(_label);
                _resultRow.Cells.Add(_resultcell);

                /*Writing Answer*/
                _resultcell = new TableCell();
                _label = new Label();
                _label.ID = "lblAnswer" + question.Attribute("sequence").Value.ToString();
                string AnswerText = string.Empty;

                int answerCurrentIndex = 0;

                /* Extracting each selected answer*/
                foreach (string answer in SelectedAnswer)
                {
                    AnswerText += answer;
                    answerCurrentIndex = answerCurrentIndex + 1;
                    if (answerCurrentIndex != SelectedAnswer.Count())
                    {
                        AnswerText += ", ";
                    }

                }

                _label.Text = AnswerText;
                CurrentAnswer = AnswerText;

                if (answerPoint == "0")
                {
                    _label.CssClass = "critical-deficiencies";
                    critical = "Yes";
                }
                else
                { _label.CssClass = "answer-label"; critical = "No"; }

                AddWarningMessage(answerPoint, question.Attribute("sequence").Value.ToString());

                _resultcell.Controls.Add(_label);
                _resultRow.Cells.Add(_resultcell);

                /*Store Row in list*/
                _expressAssessmentDetailList.Add(new ExpressAssessmentDetail(CurrentQuestion, CurrentAnswer, critical));
            }

            #endregion

            _resultProgressTable.Rows.Add(_resultRow);

        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    protected void CreateButton()
    {

        try
        {
            Table _resultActionTable = new Table();

            pnlResultAction.Controls.Clear();
            pnlResultAction.Controls.Add(_resultActionTable);

            TableRow _resultRow = new TableRow();

            string[] _buttonNames = new string[3] { "btnPrint", "btnsSaveDoc", "btnReturn" };

            // Create Cell for each Button
            foreach (string value in _buttonNames)
            {
                Button _button = new Button();
                TableCell _resultcell = new TableCell();

                if (value == "btnPrint")
                {
                    _button.ID = "btnPrint";
                    _button.Text = "Print";
                    //_button.Attributes.Add("onClick", "javascript:print();");
                    _button.Click += new System.EventHandler(this.btnPrintDoc_click);

                    _resultcell.Controls.Add(_button);
                    _button.CausesValidation = false;
                    _resultRow.Cells.Add(_resultcell); continue;
                }
                else if (value == "btnsSaveDoc")
                {
                    _button.ID = "btnsSaveDoc";
                    _button.Text = "Save to My Documents";
                    _button.Click += new System.EventHandler(this.btnsSaveDoc_click);
                    _button.CausesValidation = false;

                    _resultcell.Controls.Add(_button);
                    _resultRow.Cells.Add(_resultcell); continue;
                }
                else if (value == "btnReturn")
                {
                    _button.ID = "btnReturn";
                    _button.Text = "Return to Assessment";

                    string Active = Request.QueryString["Active"] != null ? Request.QueryString["Active"] : string.Empty;
                    string Path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;

                    _button.Attributes.Add("onClick", "window.location = 'Projects.aspx?NodeContentType=Form&NodeSectionID=" + SectionId + "&NodeProjectUsageId=" + ProjectUsageId.ToString() + "&Active=" + Active + "&SiteId=" + SiteId + "&Path=" + System.Web.HttpUtility.JavaScriptStringEncode(Path) + "'");

                    _resultcell.Controls.Add(_button);
                    _button.CausesValidation = false;
                    _resultRow.Cells.Add(_resultcell); continue;
                }
                else
                    break;

            }

            // Creating new row
            _resultActionTable.Rows.Add(_resultRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddWarningMessage(string point, string sequence)
    {

        try
        {
            if (point == "0" && sequence == "1")
            {
                _warningMesasgeList.Add(DEFAULT_CRITICAL_MSG_Q1);

                // Add warning Message into list
                _expressAssessmentWarningList.Add(new ExpressAssessmentDetail(DEFAULT_CRITICAL_MSG_Q1));
            }
            else if (point == "0" && sequence == "4")
            {
                _warningMesasgeList.Add(DEFAULT_CRITICAL_MSG_Q4);

                //Add warning Message into list
                _expressAssessmentWarningList.Add(new ExpressAssessmentDetail(DEFAULT_CRITICAL_MSG_Q4));
            }
            else if (point == "0" && sequence == "15")
            {
                _warningMesasgeList.Add(DEFAULT_CRITICAL_MSG_Q15);

                // Add warning Message into list
                _expressAssessmentWarningList.Add(new ExpressAssessmentDetail(DEFAULT_CRITICAL_MSG_Q15));
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void CalculateResult()
    {
        try
        {
            #region CALCULATE_FINAL_RESULT
            int FINAL_RESULT_PERCENTAGE = 0;
            int FINAL_RESULT = score;

            if (score != 0)
            {
                FINAL_RESULT_PERCENTAGE = ((score * 100) / DEFAULT_MAX_POINTS);
            }

            if (FINAL_RESULT >= 120)
            {
                lblResultMessage.Text = DEFAULT_EXCELLENT_SCORE_MSG;
            }
            else if (FINAL_RESULT < 120 && FINAL_RESULT >= 75)
            {
                lblResultMessage.Text = DEFAULT_BEST_SCORE_MSG;
            }
            else if (FINAL_RESULT < 75 && FINAL_RESULT >= 35)
            {
                lblResultMessage.Text = DEFAULT_GOOD_SCORE_MSG;
            }
            else
            {
                lblResultMessage.Text = DEFAULT_POOR_SCORE_MSG;
            }

            lblScoringPoint.Text = Convert.ToString(FINAL_RESULT_PERCENTAGE) + "%";
            Session["_expressAssessmentScore"] = Convert.ToString(FINAL_RESULT_PERCENTAGE) + "%";
            Session["_expressAssessmentComment"] = lblResultMessage.Text;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "KeyToIdentifythisScript", "$(function() {$('#progressbar').progressbar({value: " + FINAL_RESULT_PERCENTAGE + "});});", true);

            #endregion
        }
        catch (Exception exception)
        { throw exception; }
    }

    protected void FetchingErrorMessage()
    {
        try
        {
            // To change the height a/c to number of warnings
            if (_warningMesasgeList.Count() == 1)
                pnlresultAlert.Height = Unit.Pixel(160);

            else if (_warningMesasgeList.Count() == 2)
                pnlresultAlert.Height = Unit.Pixel(250);

            else if (_warningMesasgeList.Count() == 3)
                pnlresultAlert.Height = Unit.Pixel(330);

            else
                pnlresultAlert.Height = Unit.Pixel(120);

            if (_warningMesasgeList.Count() > 0)
            {
                int waringId = 0;

                // Clear existing Warning messages
                pnlWarningMessage.Controls.Clear();

                // Creating table for Add warning Message dynamically
                Table _resultActionTable = new Table();

                foreach (string message in _warningMesasgeList)
                {
                    waringId = waringId + 1;

                    TableRow _resultRow = new TableRow();
                    TableCell _resultcell = new TableCell();
                    Literal _literal = new Literal();

                    _literal.ID = "literal" + waringId.ToString();
                    _literal.Text = "<br /> <div id='warning-logo'></div>";
                    _literal.Text += "<div class='warning-text'>";
                    _literal.Text += message;
                    _literal.Text += " </div>";

                    _resultcell.Controls.Add(_literal);
                    _resultRow.Controls.Add(_resultcell);
                    _resultActionTable.Controls.Add(_resultRow);

                }

                pnlWarningMessage.Controls.Add(_resultActionTable);
            }
            else
            {
                // Clear existing Warning messages
                pnlWarningMessage.Controls.Clear();
            }


        }
        catch (Exception exception)
        {

            throw exception;
        }

    }

    protected void PrintSummary(bool printOnly, string name)
    {

        try
        {
            Logger.PrintInfo("Express Assessment Report generation: Process start.");

            ReportViewer viewer = new ReportViewer();

            // Set Report processing Mode
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            // Declare parameters for reports
            Logger.PrintDebug("Declaring parameters to report.");
            ReportParameter rpReportTitle = new ReportParameter("paramTitle", name + " – Generated on " + String.Format("{0:M/d/yyyy}", System.DateTime.Now).ToString());
            ReportParameter rpSiteName = new ReportParameter("paramSiteName", SiteName);
            ReportParameter rpScore = new ReportParameter("paramScore", Session["_expressAssessmentScore"].ToString());
            ReportParameter rpComment = new ReportParameter("paramComment", Session["_expressAssessmentComment"].ToString());
            ReportParameter logoImage = new ReportParameter("logoImage", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter[] param = { rpReportTitle, rpSiteName, rpScore, rpComment, logoImage };

            // Create Datasource to report
            Logger.PrintDebug("Create data source to report.");
            ReportDataSource rptExpressAssessmentdataSource = new ReportDataSource("ExpressAssessmentDataSource", _expressAssessmentDetailList);
            ReportDataSource rptExpressAssessmentWarningdataSource = new ReportDataSource("ExpressAssessmentWarningDataSource", _expressAssessmentWarningList);

            // Report display name which will be use on export
            viewer.LocalReport.DisplayName = DEFAULT_REPORT_TITLE;

            // Clear existing datasource
            viewer.LocalReport.DataSources.Clear();

            // Assign new Data source to report
            Logger.PrintDebug("Add data source.");
            viewer.LocalReport.DataSources.Add(rptExpressAssessmentdataSource);
            viewer.LocalReport.DataSources.Add(rptExpressAssessmentWarningdataSource);

            // Report path
            Logger.PrintDebug("Set report path.");
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptExpressAssessment.rdlc";
            viewer.LocalReport.EnableExternalImages = true;
            

            viewer.LocalReport.Refresh();

            // Assign parameter to report after assigned the data source*/
            Logger.PrintDebug("Set report parameters.");
            viewer.LocalReport.SetParameters(param);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Generating PDF Report
            Logger.PrintDebug("Generating PDF report.");
            byte[] bytes = viewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

            PrintBO print = new PrintBO();

            // Get file path to db*/
            // Getting file path to db
            SiteName = SiteName.Replace(",", "").Replace("//", "/").Replace("/", "");
            string siteNameMark = " " + "(" + SiteName + ")";
            string path = string.Empty;

            if (printOnly)
                path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, printOnly);

            else if (!printOnly)
                path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_SAVE_TITLE + siteNameMark, printOnly);

            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /// 0= Destination path to store the pdf on server
            string savingPath = pathList[1];      /// 1= saving path to sotre the file location in database 

            // Save file on server*/
            Logger.PrintInfo("Report saving on " + destinationPath + ".");

            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");

            // Saving location of file in db*/
            if (printOnly)
            {
                print.SaveData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userId, practiceId, printOnly, DEFAULT_DOC_CONTENT_TYPE, ProjectUsageId, SectionId);
                Session["FilePath"] = savingPath;
            }
            else if (!printOnly)
            {
                print.SaveData(DEFAULT_REPORT_SAVE_TITLE + siteNameMark, savingPath, System.DateTime.Now, userId, practiceId, printOnly, DEFAULT_DOC_CONTENT_TYPE, ProjectUsageId, SectionId);
                Session["FilePath"] = savingPath;
            }
            Logger.PrintInfo("Express Assessment Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion
}
