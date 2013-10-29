#region Modification History

//  ******************************************************************************
//  Module        : NCQA Summary Report
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
using System.Xml.Linq;
using System.Xml;
using System.Web.UI.HtmlControls;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;

public partial class MOReSummary : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int SiteId { get; set; }
    public int ProjectUsageId { get; set; }
    public int TemplateId { get; set; }
    public string SiteName { get; set; }
    public int SectionId { get; set; }

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 9;
    private const string DEFAULT_HEADER_PARENT_POS1 = "";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Header";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Max Points";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Points Earned";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Must Pass";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Docs Required";
    private const string DEFAULT_HEADER_PARENT_POS7 = "Docs Uploaded";
    private const string DEFAULT_HEADER_PARENT_POS8 = "Notes";
    private const string DEFAULT_HEADER_PARENT_POS9 = "Complete";
    private const string DEFAULT_STANDARD_ID_PREFIX = "lblSummaryStandard";
    private const string DEFAULT_STANDARD_IMAGE_ID_PREFIX = "imgStandard";
    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "elementTable";
    private const string DEFAULT_ELEMENT_ID_PREFIX = "lblSummaryelement";
    private const string DEFAULT_GRANDTOTAL_TABLE_ID_PREFIX = "grandTotalTable";
    private const string DEFAULT_GRANDTOTAL_LABEL_ID_PREFIX = "lblTotal";
    private const string DEFAULT_GRANDTOTAL_LABEL_TEXT = "Total:";
    private const string DEFAULT_REPORT_TITLE = "Summary Status Report - NCQA PCSP 2013";
    private const string DEFAULT_NOTES_DETAILED_REPORT_TITLE = "Notes Details Report";
    private const string DEFAULT_DETAILED_REPORT_TITLE = "Summary Status Report with details";
    private const string DEFAULT_REPORT_DETAIL_TITLE = "Detailed Summary Status Report";

    private const int DEFAULT_LEVEL1_STARTPOINT = 35;
    private const int DEFAULT_LEVEL1_ENDPOINT = 59;
    private const int DEFAULT_LEVEL2_STARTPOINT = 60;
    private const int DEFAULT_LEVEL2_ENDPOINT = 84;
    private const int DEFAULT_LEVEL3_STARTPOINT = 85;
    private const int DEFAULT_LEVEL3_ENDPOINT = 100;

    // PCMH scoring level
    private const string DEFAULT_LEVEL1_TEXT = " (Level 1)";
    private const string DEFAULT_LEVEL2_TEXT = " (Level 2)";
    private const string DEFAULT_LEVEL3_TEXT = " (Level 3)";

    // Default ContentType
    private const string DEFAULT_DOC_CONTENT_TYPE = "UploadedDocuments";

    #endregion

    #region Variables
    private NCQANotesDetails _NCQANotesDetails;

    private PrintBO print;

    private decimal SUM_ELEMENT_EARNED_POINTS;
    private decimal TOTAL_NCQA_POINTS;
    private decimal TOTAL_NCQA_EARNER_POINTS;
    private int TOTAL_NCQA_REQUIRED_DOCS;
    private int TOTAL_NCQA_UPLOADED_DOCS;
    private string FINAL_MUSTPASS_CHECK;
    private string POINTS_PERCENTAGE_Value;
    private string DOCS_PERCENTAGE_Value;
    private int UNIQUE_DOCS;

    private XDocument questionnaire;
    private List<NCQANotesDetails> _lstSRANotesDetails = new List<NCQANotesDetails>();
    private List<NCQASummaryDetail> _ncqaStandardList = new List<NCQASummaryDetail>();

    private int userApplicationId;
    private string userType;
    public int practiceId;

    #endregion

    #region CONTROLS
    private Table _NCQASummaryTable;
    private TableRow _tableRow;
    private TableCell _tableCell;

    private Label _label;
    private Image _image;
    private LinkButton _linkButton;

    MOReBO _MORe = new MOReBO();

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;
            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                userApplicationId = Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]);
                userType = Session[enSessionKey.UserType.ToString()].ToString();
                practiceId = Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]);
                hdnSummaryProjectUsageId.Value = ProjectUsageId.ToString();
                hdnSiteId.Value = SiteId.ToString();
                hdnTemplateId.Value = TemplateId.ToString();
            }
            else
            {
                SessionHandling sessionHandling = new SessionHandling();
                sessionHandling.ClearSession();
                Response.Redirect("~/Account/Login.aspx");
            }

            //if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
            //{
                // Enable/Disable checkboxes
                CheckNCQATemplate();

                // Set MORe Status
                SetSubmissionStatus();

                // Generate Table Layput
                GenerateLayOut();

                // Fetching available PCMH list
                GettingPCMHList(_NCQASummaryTable);

                // Calculate grand total
                CalculateGrandTotal();

                // Add jScript functions
                RegisterJscript();

                //Generate Notes Details
                GenerateNotesList();

                //Add Facilitators
                GetFacilitators();

                message.Clear("");

            //}
            //else
            //    message.Info("Questionnaire against the selected site doesn't exist.");
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void btnPrintNotes_Click(object sender, EventArgs e)
    {
        try
        {
            Session["FilePath"] = string.Empty;
            PrintNotes(DEFAULT_NOTES_DETAILED_REPORT_TITLE);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("Report couldn't be generated. Please try again/ Contact your site Administrator.");
        }
    }

    protected void btnPrintSummary_Click(object sender, EventArgs e)
    {
        try
        {
            Session["FilePath"] = string.Empty;
            PrintSummary(false, DEFAULT_REPORT_TITLE);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("Report couldn't be generated. Please try again/ Contact your site Administrator.");
        }

    }

    protected void btnPrintSummaryDetails_Click(object sender, EventArgs e)
    {
        try
        {
            Session["FilePath"] = String.Empty;
            PrintSummary(true, DEFAULT_REPORT_DETAIL_TITLE);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("Report couldn't be generated. Please try again/ Contact your site Administrator.");
        }

    }

    protected void btnSaveAPICredentials_Click(object sender, EventArgs e)
    {
        try
        {
            //Encrypt password
            Security security = new Security();
            string encryptedPassword = security.Encrypt(txtPassword.Text);

            XElement apiCredentials = new XElement("APICredentials");

            XAttribute issLicenseNumber = new XAttribute("ISSLicenseNumber", txtLicenseNumber.Text);
            apiCredentials.Add(issLicenseNumber);

            XAttribute userName = new XAttribute("UserName", txtUserName.Text);
            apiCredentials.Add(userName);

            XAttribute password = new XAttribute("Password", encryptedPassword);
            apiCredentials.Add(password);

            NCQASubmissionMethod ncqaSubmissionMethod = new NCQASubmissionMethod();
            ncqaSubmissionMethod.InsertAPICredentials(ProjectUsageId,SiteId, TemplateId, apiCredentials);

            Email email = new Email();
            email.SendNCQARequest(ProjectUsageId, txtLicenseNumber.Text, txtUserName.Text, txtPassword.Text, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            bool requestExists = ncqaSubmissionMethod.IsSubmissionRequestExists(ProjectUsageId,SiteId, TemplateId);
            btnRequestSubmission.Disabled = requestExists;
            hdnRequestExists.Value = requestExists.ToString();

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    #endregion

    #region FUNCTIONS

    // this function would be called by parent upon notification that save button on header tab is pressed, so that
    // summary data is updated
    public void Reload()
    {
        //if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
        //{
            if (_NCQASummaryTable != null)
            _NCQASummaryTable.Controls.Clear();

            divTemplateName.Controls.Clear();

            GenerateLayOut();
            // Fetching available PCMH list
            GettingPCMHList(_NCQASummaryTable);

            // Calculate grand total
            CalculateGrandTotal();

        //}
    }

    protected void SetSubmissionStatus()
    {
        try
        {
            PracticeSiteSubmission practiceSiteSubmission = new NCQASubmissionMethod().GetPracticeSiteSubmissionByProjectIdAndTemplateId(ProjectUsageId,SiteId, TemplateId);

            if (practiceSiteSubmission != null)
            {
                string reviewed = "false";
                string submitted = "false";
                string recognized = "false";

                if (practiceSiteSubmission.Reviewed != null)
                    reviewed = practiceSiteSubmission.Reviewed.ToString().ToLower();

                if (practiceSiteSubmission.Submitted != null)
                    submitted = practiceSiteSubmission.Submitted.ToString().ToLower();

                if (practiceSiteSubmission.Recognized != null)
                    recognized = practiceSiteSubmission.Recognized.ToString().ToLower();


                // Reviewed
                chbReviewed.Checked = Convert.ToBoolean(reviewed);
                if (Convert.ToBoolean(reviewed))
                {
                    bool requestExists = new NCQASubmissionMethod().IsSubmissionRequestExists(ProjectUsageId,SiteId, TemplateId);
                    btnRequestSubmission.Disabled = requestExists;
                    hdnRequestExists.Value = requestExists.ToString();
                }


                //Submitted
                chbSubmitted.Checked = Convert.ToBoolean(submitted);

                if (Convert.ToBoolean(submitted))
                    lblSubmitted.Text = "On " + (practiceSiteSubmission.SubmittedOn == null ? string.Empty : Convert.ToDateTime(practiceSiteSubmission.SubmittedOn).ToString("MM-dd-yyyy"));


                //Recognized
                chbRecognized.Checked = Convert.ToBoolean(recognized);

                if (Convert.ToBoolean(recognized))
                {
                    lblRecognized.Text = "On " + (practiceSiteSubmission.RecognizedOn == null ? string.Empty : Convert.ToDateTime(practiceSiteSubmission.RecognizedOn).ToString("MM-dd-yyyy"));
                    lblRecognized.Text += practiceSiteSubmission.RecognizedLevel == null ? string.Empty : practiceSiteSubmission.RecognizedLevel;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void GenerateLayOut()
    {
        try
        {
            Template temp = _MORe.GetTemplateInfo(TemplateId);

            HtmlGenericControl div1 = new HtmlGenericControl("div");
            div1.Attributes.Add("class", "child-title1");
            div1.InnerText = temp.Name;

            divTemplateName.Controls.Add(div1);

            /*Last updated date is not visible in PCSP as per Margalit request*/
            if (temp.Name != "PCSP Recognition")
            {
                HtmlGenericControl div2 = new HtmlGenericControl("div");
                div2.Attributes.Add("class", "child-title12");
                div2.InnerText = "Last Updated: " + temp.LastUpdatedDate.ToShortDateString();
                divTemplateName.Controls.Add(div2);
            }

            // Clear Master table if is already exist
            pnlNCQASummary.Controls.Clear();

            System.Threading.Thread.Sleep(1000);

            // Display Site Name
            lblSiteInfo.Text = SiteName;

            // Create object for master table
            _NCQASummaryTable = new Table();
            _NCQASummaryTable.ID = "masterTable";
            _NCQASummaryTable.ClientIDMode = ClientIDMode.Static;

            // Add Master table
            pnlNCQASummary.Controls.Add(_NCQASummaryTable);

            // Add Header
            AddHeader(_NCQASummaryTable);

        }
        catch (Exception exception)
        {
            message.Error("An error occured while generating the layout. Please try again!");
            throw exception;
        }

    }

    protected void AddHeader(Table masterTable)
    {
        try
        {
            int ColumnIndex = 1;
            _tableRow = new TableRow();
            _tableRow.ClientIDMode = ClientIDMode.Static;

            // Add Data on specific column (to varify please check the prototype)
            #region PARENT_HEADER
            for (ColumnIndex = 1; ColumnIndex <= DEFAULT_TOTAL_COLUMNS; ColumnIndex++)
            {
                _label = new Label();
                _label.ID = "lblHeader" + ColumnIndex;
                _label.ClientIDMode = ClientIDMode.Static;

                _tableCell = new TableCell();
                _tableCell.ID = "headerCell" + Convert.ToString(ColumnIndex);
                _tableCell.CssClass = "header";
                _tableCell.ClientIDMode = ClientIDMode.Static;

                switch (ColumnIndex)
                {
                    case 1:
                        _label.Text = DEFAULT_HEADER_PARENT_POS1;
                        _tableCell.CssClass = "empty-header";
                        _tableCell.Controls.Add(_label);
                        break;

                    case 2:
                        _label.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 3:
                        _label.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 4:
                        _label.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Controls.Add(_label);
                        break;
                    case 5:
                        _label.Text = DEFAULT_HEADER_PARENT_POS5;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 6:
                        _label.Text = DEFAULT_HEADER_PARENT_POS6;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 7:
                        _label.Text = DEFAULT_HEADER_PARENT_POS7;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 8:
                        _label.Text = DEFAULT_HEADER_PARENT_POS8;
                        _tableCell.Controls.Add(_label);
                        break;

                    case 9:
                        _label.Text = DEFAULT_HEADER_PARENT_POS9;
                        _tableCell.Controls.Add(_label);
                        break;
                    default:
                        break;

                }

                _tableRow.Controls.Add(_tableCell);

            }
            masterTable.Controls.Add(_tableRow);
            #endregion



        }
        catch (Exception exception)
        {

            throw exception;
        }

    }

    protected void GettingPCMHList(Table masterTable)
    {
        try
        {
            //XElement root = questionnaire.Root;
            TOTAL_NCQA_POINTS = 0;
            TOTAL_NCQA_EARNER_POINTS = 0;
            TOTAL_NCQA_REQUIRED_DOCS = 0;
            TOTAL_NCQA_UPLOADED_DOCS = 0;
            UNIQUE_DOCS = 0;
            FINAL_MUSTPASS_CHECK = "Yes";

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            UNIQUE_DOCS = knowledgeBaseBO.GetUniqueDocumentsCount(ProjectUsageId,SiteId, TemplateId);
            if (IsTemplateOption(ProjectUsageId,TemplateId,Convert.ToInt32(Session["EnterpriseId"].ToString())))
            {
                if (FilledAnswersExist(ProjectUsageId, SiteId))
                {
                    CopyFilledAns(ProjectUsageId, SiteId);
                }
            }
            List<KnowledgeBase> lstKnowledgeBaseHeader = knowledgeBaseBO.GetKnowledgeBaseByTypeAndTemplateId(enKBType.Header, TemplateId);

            string headerId = string.Empty;
            string headerTitle = string.Empty;
            string headerMaxPoint = string.Empty;

            string mustPassText = string.Empty;
            string noteText = string.Empty;
            string completeText = string.Empty;

            int requiredDocsCount = 0;
            int uploadedDocsCount = 0;
            decimal pointsEarned = 0;

            int index = 2;

            List<string> listMustPass = new List<string>();

            foreach (KnowledgeBase header in lstKnowledgeBaseHeader)
            {
                headerId = header.KnowledgeBaseId.ToString();
                headerTitle = header.Name;
                headerMaxPoint = knowledgeBaseBO.GetHeaderMaxPointByHeaderId(header.KnowledgeBaseId, TemplateId);

                mustPassText = GetMustPassValueByKnowledgeBaseId(header.KnowledgeBaseId);
                listMustPass.Add(mustPassText);
                requiredDocsCount = GetHeaderRequiredDocsCount(header.KnowledgeBaseId);
                uploadedDocsCount = GetHeaderUploadedDocsCount(header.KnowledgeBaseId);
                noteText = GetNotesTextByKnowledgeBaseId(header.KnowledgeBaseId);
                completeText = GetCompleteTextByKnowledgeBaseId(header.KnowledgeBaseId);
                pointsEarned = GetPointEarnedByKnowledgeBaseId(header.KnowledgeBaseId);

                _tableRow = new TableRow();
                _tableCell = new TableCell();
                _tableCell.CssClass = "child-title02";
                _tableCell.ColumnSpan = 2;

                _image = new Image();
                _image.ID = DEFAULT_STANDARD_IMAGE_ID_PREFIX + headerId;
                _image.ImageUrl = "~/Themes/Images/Plus.png";
                _image.CssClass = "img-toggle";
                _image.ClientIDMode = ClientIDMode.Static;
                _image.Attributes.Add("onclick", "standardTracking('" + headerId + "');");
                _tableCell.Controls.Add(_image);

                _linkButton = new LinkButton();
                _linkButton.ID = DEFAULT_STANDARD_ID_PREFIX + headerId;
                _linkButton.ClientIDMode = ClientIDMode.Static;
                _linkButton.OnClientClick = "javascript:updateClickTab(" + index + ");";

                string[] headerTitleText = headerTitle.Split(':');

                if (headerTitleText.Count() == 2)
                    _linkButton.Text = "<div class='child-title02-head'>" + headerTitleText.First() + ":</div>" +
                        "<div class='child-title02-desc'>" + headerTitleText.Last() + "</div>";
                else
                    _linkButton.Text = headerTitle;

                _tableCell.Controls.Add(_linkButton);
                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = "cell";
                _tableCell.Text = headerMaxPoint;

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = "cell";
                _tableCell.Text = pointsEarned.ToString();

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = mustPassText == "No" ? "no-cell" : "cell";
                _tableCell.Text = mustPassText;

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = "cell";
                _tableCell.Text = requiredDocsCount.ToString();

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = "cell";
                _tableCell.Text = uploadedDocsCount.ToString();

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = noteText == "No" ? "no-cell" : "cell";
                _tableCell.Text = noteText;

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.CssClass = completeText == "No" ? "no-cell" : "cell";
                _tableCell.Text = completeText;

                _tableRow.Controls.Add(_tableCell);

                masterTable.Controls.Add(_tableRow);

                // Add Standard Row in List
                _ncqaStandardList.Add(new NCQASummaryDetail(headerId, string.Empty, headerTitle, Convert.ToDecimal(headerMaxPoint), Convert.ToDecimal(pointsEarned),
                                     mustPassText, requiredDocsCount, uploadedDocsCount, noteText, completeText));

                // Populate Sub Header
                PopulateSubHeaders(masterTable, header.KnowledgeBaseId, index);

                // Store Docs Detail for grand total
                TOTAL_NCQA_REQUIRED_DOCS = TOTAL_NCQA_REQUIRED_DOCS + requiredDocsCount;
                TOTAL_NCQA_UPLOADED_DOCS = TOTAL_NCQA_UPLOADED_DOCS + uploadedDocsCount;
                TOTAL_NCQA_POINTS = TOTAL_NCQA_POINTS + Convert.ToDecimal(headerMaxPoint);
                TOTAL_NCQA_EARNER_POINTS = TOTAL_NCQA_EARNER_POINTS + Convert.ToDecimal(SUM_ELEMENT_EARNED_POINTS);
                index++;
            }

            bool isAllMustPass = true;

            foreach (string item in listMustPass)
            {
                if (item == "No")
                {
                    isAllMustPass = false;
                    break;
                }

                if (item == "NA")
                    FINAL_MUSTPASS_CHECK = "NA";
            }

            if (!isAllMustPass)
                FINAL_MUSTPASS_CHECK = "No";

            //NCQA Status
            /*if (questionnaire.Root.Elements("Status").Any())
            {
                if (questionnaire.Root.Elements("Status").Elements("Reviewed").Any())
                {
                    if (questionnaire.Root.Elements("Status").Elements("Reviewed").Attributes().Any())
                    {
                        chbReviewed.Checked = Convert.ToBoolean(questionnaire.Root.Element("Status").Element("Reviewed").Attribute("Status").Value);

                        NCQASubmissionBO ncqaSubmissionBO = new NCQASubmissionBO();
                        bool requestExists = ncqaSubmissionBO.NCQASubmissionRequestExists(ProjectId);
                        btnRequestSubmission.Disabled = requestExists;
                        hdnRequestExists.Value = requestExists.ToString();
                    }
                }

                if (questionnaire.Root.Elements("Status").Elements("Submitted").Any())
                {
                    if (questionnaire.Root.Elements("Status").Elements("Submitted").Attributes().Any())
                    {
                        chbSubmitted.Checked = Convert.ToBoolean(questionnaire.Root.Element("Status").Element("Submitted").Attribute("Status").Value);
                        lblSubmitted.Text = "On " + questionnaire.Root.Element("Status").Element("Submitted").Attribute("Date").Value;
                    }
                }

                if (questionnaire.Root.Elements("Status").Elements("Recognized").Any())
                {
                    if (questionnaire.Root.Elements("Status").Elements("Recognized").Attributes().Any())
                    {
                        chbRecognized.Checked = Convert.ToBoolean(questionnaire.Root.Element("Status").Element("Recognized").Attribute("Status").Value);
                        lblRecognized.Text = "On " + questionnaire.Root.Element("Status").Element("Recognized").Attribute("Date").Value;
                        lblRecognized.Text += questionnaire.Root.Element("Status").Element("Recognized").Attribute("Level").Value;
                    }
                }
            }*/

        }
        catch (Exception exception)
        {
            message.Error("An error occured while generating the Report. Please try again/Contact Site Administrator.");
            throw exception;
        }
    }

    protected void CalculateGrandTotal()
    {
        try
        {
            _tableRow = new TableRow();
            _tableRow.CssClass = "grandtotal";
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 2;
            _tableCell.HorizontalAlign = HorizontalAlign.Right;

            _label = new Label();
            _label.ID = DEFAULT_GRANDTOTAL_LABEL_ID_PREFIX + "1";
            _label.ClientIDMode = ClientIDMode.Static;
            _label.Text = DEFAULT_GRANDTOTAL_LABEL_TEXT;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            // Add Grand total result on specific columns
            // Column 3-9
            string[] resultDetails = {Convert .ToString( TOTAL_NCQA_POINTS),
                                         Convert .ToString(TOTAL_NCQA_EARNER_POINTS),
                                         Convert .ToString(FINAL_MUSTPASS_CHECK),
                                         Convert .ToString(TOTAL_NCQA_REQUIRED_DOCS),
                                         Convert .ToString(TOTAL_NCQA_UPLOADED_DOCS) + " (" + Convert .ToString(UNIQUE_DOCS) + ")",string.Empty,string.Empty };
            int index = 0;
            int columnIndex = 3;
            for (index = 0; index < resultDetails.Length; index++)
            {
                _tableCell = new TableCell();
                _tableCell.CssClass = "cell0" + columnIndex;

                _label = new Label();
                _label.ID = DEFAULT_GRANDTOTAL_LABEL_ID_PREFIX + columnIndex;
                _label.Text = resultDetails[index];

                if (index == 4)
                {
                    _label.ToolTip = "Total Documents: " + TOTAL_NCQA_UPLOADED_DOCS.ToString() + ", Unique Documents: " + UNIQUE_DOCS.ToString();
                }

                if (resultDetails[index] == "No")
                    _label.CssClass = "none";

                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                columnIndex = columnIndex + 1;
            }

            _NCQASummaryTable.Controls.Add(_tableRow);

            DOCS_PERCENTAGE_Value = POINTS_PERCENTAGE_Value = "0";
            POINTS_PERCENTAGE_Value = (TOTAL_NCQA_POINTS != 0 ? String.Format("{0:0.00}", (TOTAL_NCQA_EARNER_POINTS * 100) / TOTAL_NCQA_POINTS) : "0");
            lblTotalPoints.Text = POINTS_PERCENTAGE_Value + " %";


            if (TOTAL_NCQA_REQUIRED_DOCS > 0)
                DOCS_PERCENTAGE_Value = Convert.ToString((TOTAL_NCQA_UPLOADED_DOCS * 100) / TOTAL_NCQA_REQUIRED_DOCS);

            lblTotalDocs.Text = DOCS_PERCENTAGE_Value + " %";

            // Display NCQA Level Under points
            DisplayNCQALevel();

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void RegisterJscript()
    {
        try
        {

            string _registerJScript = string.Empty;

            #region PROGRESSBAR
            _registerJScript = "$(document).ready(function() {if($('#pointsProgress')) { $('#pointsProgress').progressbar({value: " + POINTS_PERCENTAGE_Value + "});}";
            _registerJScript += "if ($('#docsProgress')) { $('#docsProgress').progressbar({value: " + DOCS_PERCENTAGE_Value + "});}});";

            #endregion

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NCQASummaryjs", _registerJScript, true);

        }
        catch (Exception exception)
        {

            throw exception;
        }

    }

    protected void PrintSummary(bool _isDetail, string name)
    {

        try
        {
            Logger.PrintInfo("NCQA Summary Report generation: Process start.");

            List<NCQASummaryDetail> _receivedList = _ncqaStandardList;

            // Remove items from list (without detail case)
            if (!_isDetail)
            {

                Logger.PrintInfo("Remove details from the print list.");
                _receivedList.RemoveAll(item => item.ElementSequence.ToString() != string.Empty);
            }
            else
            {
                name = name + " - NCQA PCSP 2013";
            }

            ReportViewer viewer = new ReportViewer();

            // Set Report processing Mode
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            // Declare parameters for reports
            Logger.PrintDebug("Declaring parameters to report.");
            ReportParameter rpReportTitle = new ReportParameter("paramTitle", name);
            ReportParameter rpTotalMaxPoints = new ReportParameter("paramTotalMaxPoints", TOTAL_NCQA_POINTS.ToString());
            ReportParameter rpTotalEarnedPoints = new ReportParameter("paramTotalEarnedPoints", TOTAL_NCQA_EARNER_POINTS.ToString());
            ReportParameter rpTotalMustPass = new ReportParameter("paramTotalMustPass", FINAL_MUSTPASS_CHECK);
            ReportParameter rpTotalDocsRequired = new ReportParameter("paramTotalDocsRequired", TOTAL_NCQA_REQUIRED_DOCS.ToString());
            ReportParameter rpTotalDocsUploaded = new ReportParameter("paramTotalDocsUploaded", TOTAL_NCQA_UPLOADED_DOCS.ToString());
            ReportParameter rpPracticeName = new ReportParameter("paramSiteName", SiteName);
            ReportParameter rpDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
            ReportParameter rpPointPercentage = new ReportParameter("paramPointPercentage", lblTotalPoints.Text);
            ReportParameter rpDocPercentage = new ReportParameter("paramDocPercentage", DOCS_PERCENTAGE_Value.ToString());
            ReportParameter logoImage = new ReportParameter("logoImage", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter[] param = { rpReportTitle, rpTotalMaxPoints, rpTotalEarnedPoints, rpTotalMustPass, rpTotalDocsRequired,
                                      rpTotalDocsUploaded,rpPracticeName,rpDate,rpPointPercentage,rpDocPercentage, logoImage};

            // Create Datasource to report
            Logger.PrintDebug("Create data source to report.");
            ReportDataSource rptStandarddataSource = new ReportDataSource("NCQAStandardDataSource", _receivedList);

            // Report display name which will be use on export
            viewer.LocalReport.DisplayName = "NCQA Summary";

            // Clear existing datasource*/
            viewer.LocalReport.DataSources.Clear();

            // Assign new Data source to report*/
            Logger.PrintDebug("Add data source.");
            viewer.LocalReport.DataSources.Add(rptStandarddataSource);

            // Set Report path
            Logger.PrintDebug("Set report path.");
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptNCQASummary.rdlc";
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
            byte[] bytes = viewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

            print = new PrintBO();

            // Getting file path to db
            SiteName = SiteName.Replace(",", "").Replace("//", "/").Replace("/", "");

            string siteNameMark = " " + "(" + SiteName + ")";
            string path = "";

            if (_isDetail)
                path = Util.GetPdfPath(practiceId, DEFAULT_DETAILED_REPORT_TITLE + siteNameMark, true);
            else if (!_isDetail)
                path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, true);

            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /*0= Destination path to store the pdf on server*/
            string savingPath = pathList[1]; /*1= saving path to store the file location in database */

            // Save file on server*/
            Logger.PrintInfo("Report saving on " + destinationPath + ".");
            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");

            // Saving location of file in db*/
            if (_isDetail)
            {
                print.SaveDataForMORe(DEFAULT_REPORT_DETAIL_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true,
                    DEFAULT_DOC_CONTENT_TYPE, TemplateId,SectionId,ProjectUsageId);
                Session["FilePath"] = savingPath;
            }
            else if (!_isDetail)
            {
                print.SaveDataForMORe(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true,
                    DEFAULT_DOC_CONTENT_TYPE, TemplateId,SectionId,ProjectUsageId);
                Session["FilePath"] = savingPath;
            }

            Logger.PrintInfo("NCQA Summary Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void DisplayNCQALevel()
    {

        if (Convert.ToDouble(POINTS_PERCENTAGE_Value) >= DEFAULT_LEVEL1_STARTPOINT && Convert.ToDouble(POINTS_PERCENTAGE_Value) < DEFAULT_LEVEL1_ENDPOINT && FINAL_MUSTPASS_CHECK == "Yes")
            lblTotalPoints.Text += hdnLevel.Value = DEFAULT_LEVEL1_TEXT;
        else if (Convert.ToDouble(POINTS_PERCENTAGE_Value) >= DEFAULT_LEVEL2_STARTPOINT && Convert.ToDouble(POINTS_PERCENTAGE_Value) < DEFAULT_LEVEL2_ENDPOINT && FINAL_MUSTPASS_CHECK == "Yes")
            lblTotalPoints.Text += hdnLevel.Value = DEFAULT_LEVEL2_TEXT;
        else if (Convert.ToDouble(POINTS_PERCENTAGE_Value) >= DEFAULT_LEVEL3_STARTPOINT && Convert.ToDouble(POINTS_PERCENTAGE_Value) < DEFAULT_LEVEL3_ENDPOINT && FINAL_MUSTPASS_CHECK == "Yes")
            lblTotalPoints.Text += hdnLevel.Value = DEFAULT_LEVEL3_TEXT;

    }

    protected void GetFacilitators()
    {
        ConsultingUserBO consultingUserBO = new ConsultingUserBO();
        List<ConsultantAdministrationDetail> facilitatorsList = consultingUserBO.GetFacilitators(Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]));

        if (facilitatorsList.Count() > 0)
        {
            Table tblfacilitator = new Table();
            pnlFacilitators.Controls.Add(tblfacilitator);

            foreach (ConsultantAdministrationDetail facilitator in facilitatorsList)
            {
                HyperLink hyperLink = new HyperLink();
                hyperLink.Text = facilitator.FirstName + " " + facilitator.LastName;
                hyperLink.ToolTip = "Click to email";
                hyperLink.NavigateUrl = string.Format("mailto:{0}", facilitator.Email);

                TableCell tableCell = new TableCell();
                tableCell.Controls.Add(hyperLink);

                TableRow tableRow = new TableRow();
                tableRow.Controls.Add(tableCell);

                tblfacilitator.Controls.Add(tableRow);
            }
        }
        else
        {
            hypFacilitator.Visible = false;
        }
    }

    protected void PrintNotes(string name)
    {

        try
        {
            byte[] bytes = GetPDFBytes();

            print = new PrintBO();

            // Getting file path to db
            SiteName = SiteName.Replace(",", "").Replace("//", "/").Replace("/", "");

            string siteNameMark = " " + "(" + SiteName + ")";
            string path = "";


            path = Util.GetPdfPath(practiceId, DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, true);


            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /*0= Destination path to store the pdf on server*/
            string savingPath = pathList[1]; /*1= saving path to store the file location in database */

            // Save file on server*/
            Logger.PrintInfo("Report saving on " + destinationPath + ".");
            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");

            // Saving location of file in db*/

            print.SaveDataForMORe(DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, 
                DEFAULT_DOC_CONTENT_TYPE, TemplateId,SectionId,ProjectUsageId);
            Session["FilePath"] = savingPath;



            Logger.PrintInfo("NCQA Summary Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private List<NCQANotesDetails> GenerateNotesList()
    {
        try
        {
            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();

            List<KnowledgeBase> lstKnowledgeBaseHeader = knowledgeBaseBO.GetKnowledgeBaseByTypeAndTemplateId(enKBType.Header, TemplateId);

            FilledAnswer filledAnswer;

            foreach (KnowledgeBase header in lstKnowledgeBaseHeader)
            {
                List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(header.KnowledgeBaseId, TemplateId);

                foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
                {
                    filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId,subHeader.KnowledgeBaseId, TemplateId);

                    if (filledAnswer != null)
                    {
                        if (!string.IsNullOrEmpty(filledAnswer.EvaluationNotes))
                        {
                            _NCQANotesDetails = new NCQANotesDetails();
                            _NCQANotesDetails.PCMHId = header.KnowledgeBaseId.ToString();

                            _NCQANotesDetails.PCMHTitle = header.DisplayName;
                            _NCQANotesDetails.ElementId = subHeader.KnowledgeBaseId.ToString();

                            _NCQANotesDetails.ElementTitle = subHeader.DisplayName;
                            _NCQANotesDetails.Type = "EvaluationNotes";

                            _NCQANotesDetails.Notes = filledAnswer.EvaluationNotes;
                            _lstSRANotesDetails.Add(_NCQANotesDetails);
                        }
                    }
                }
            }

            return _lstSRANotesDetails;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GenerateNotesReport(string mapPath, Uri uri, string virtualDirectory, int practiceID)
    {
        try
        {
            _lstSRANotesDetails = new List<NCQANotesDetails>();
            _lstSRANotesDetails = GenerateNotesList();

            byte[] bytes = GetPDFBytes();
            print = new PrintBO();

            // Getting file path to db
            SiteName = SiteName.Replace(",", "").Replace("//", "/").Replace("/", "");

            string siteNameMark = " " + "(" + SiteName + ")";
            string path = Util.GetPdfPath(practiceID, DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, true, mapPath, uri, virtualDirectory);

            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /*0= Destination path to store the pdf on server*/
            string savingPath = pathList[1]; /*1= saving path to store the file location in database */

            // Save file on server*/
            Logger.PrintInfo("Report saving on " + destinationPath + ".");
            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");


            // Saving location of file in db
            print.SaveDataForMORe(DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceID, true,
                DEFAULT_DOC_CONTENT_TYPE, TemplateId,SectionId,ProjectUsageId);

            return destinationPath;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private byte[] GetPDFBytes()
    {
        try
        {
            Logger.PrintInfo("NCQA Summary Report generation: Process start.");

            List<NCQANotesDetails> _receivedNotesList = _lstSRANotesDetails;
            ReportViewer viewer = new ReportViewer();

            // Set Report processing Mode
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            // Declare parameters for reports
            Logger.PrintDebug("Declaring parameters to report.");
            //ReportParameter rpReportTitle = new ReportParameter("paramTitle", name);
            //ReportParameter rpTotalMaxPoints = new ReportParameter("paramTotalMaxPoints", TOTAL_NCQA_POINTS.ToString());

            ReportParameter rpPracticeName = new ReportParameter("paramSiteName", SiteName);
            //ReportParameter rpDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
            ReportParameter logoImage = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter[] param = { rpPracticeName, logoImage };

            // Create Datasource to report
            Logger.PrintDebug("Create data source to report.");
            ReportDataSource rptStandarddataSource = new ReportDataSource("NCQANotesDS", _receivedNotesList);

            // Report display name which will be use on export
            viewer.LocalReport.DisplayName = "NCQA Notes";

            // Clear existing datasource*/
            viewer.LocalReport.DataSources.Clear();

            // Assign new Data source to report*/
            Logger.PrintDebug("Add data source.");
            viewer.LocalReport.DataSources.Add(rptStandarddataSource);

            // Set Report path
            Logger.PrintDebug("Set report path.");
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptNCQANotes.rdlc";
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
            byte[] bytes = viewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

            return bytes;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string GetMustPassValueByKnowledgeBaseId(int knowledgeBaseId)
    {
        try
        {
            string mustPass = string.Empty;

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);
            FilledAnswer filledAnswer;
            bool isPass = false;
            List<string> listmPass = new List<string>();

            if (lstKnowledgeBaseSubHeader.Count() > 0)
            {
                foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
                {
                    string isMustPass = subHeader.MustPass == null ? "false" : subHeader.MustPass.ToString().ToLower();

                    if (isMustPass == "true")
                    {
                        isPass = true;
                        filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId);

                        if (filledAnswer != null)
                        {
                            string scoredResult = filledAnswer.DefaultScore;

                            if (scoredResult != null)
                                scoredResult = scoredResult.Trim() != string.Empty ? scoredResult.Replace("%", "") : "0";
                            else
                                scoredResult = "0";

                            if (Convert.ToInt32(scoredResult) >= 50) // Passing criteria is 50%
                                /*mustPass = "Yes";*/
                                listmPass.Add("Yes");
                            else
                                /*mustPass = FINAL_MUSTPASS_CHECK = "No";*/
                                listmPass.Add("No");
                        }
                        else
                            /*mustPass = "No";*/
                            listmPass.Add("No");
                    }


                }

                /*if (!isPass)
                    mustPass = FINAL_MUSTPASS_CHECK = "NA";*/
            }
            else
            {
                /*mustPass = FINAL_MUSTPASS_CHECK = "No";*/
                listmPass.Add("No");
            }

            bool isFalse = false;
            bool isNA = false;

            foreach (string item in listmPass)
            {
                if (item == "No")
                {
                    isFalse = true;
                    break;
                }

            }

            if (listmPass.Count > 0)
            {

                if (!isFalse)
                    mustPass = "Yes";

                else
                    mustPass = FINAL_MUSTPASS_CHECK = "No";
            }

            else
                mustPass = "NA";

            return mustPass;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private int GetHeaderRequiredDocsCount(int knowledgeBaseId)
    {
        try
        {
            int requiredDocsCount = 0;
            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();

            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);

            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                requiredDocsCount += Convert.ToInt32(knowledgeBaseBO.GetSubHeaderRequiredDocsCount(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId).ToString());
            }

            return requiredDocsCount;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private int GetHeaderUploadedDocsCount(int knowledgeBaseId)
    {
        try
        {
            int uploadedDocsCount = 0;
            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();

            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);

            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                uploadedDocsCount += Convert.ToInt32(knowledgeBaseBO.GetUploadedDocsCount(subHeader.KnowledgeBaseId, ProjectUsageId,SiteId, TemplateId).ToString());
            }

            return uploadedDocsCount;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private string GetNotesTextByKnowledgeBaseId(int knowledgeBaseId)
    {
        try
        {
            string NoteExist = string.Empty;

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);
            FilledAnswer filledAnswer;

            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId);

                if (filledAnswer != null)
                {
                    if (filledAnswer.ReviewNotes != null && filledAnswer.ReviewNotes.Trim() != string.Empty)
                        NoteExist = "Yes";
                }
            }


            return NoteExist;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private string GetCompleteTextByKnowledgeBaseId(int knowledgeBaseId)
    {
        try
        {
            string complete = string.Empty;
            string isComplete = string.Empty;
            int counter = 0;

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);
            FilledAnswer filledAnswer;

            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId);

                if (filledAnswer != null)
                {
                    isComplete = filledAnswer.Complete == null ? "false" : filledAnswer.Complete.ToString().ToLower();

                    if (isComplete == "true")
                        counter++;
                }
            }

            if (lstKnowledgeBaseSubHeader.Count == counter)
                complete = "Yes";

            return complete;

        }
        catch (Exception)
        {
            throw;
        }
    }

    private decimal GetPointEarnedByKnowledgeBaseId(int knowledgeBaseId)
    {
        try
        {
            string defaultScore = string.Empty;
            string pointEarned = string.Empty;

            string subHeaderMaxPoint = string.Empty;
            decimal totalPointsEarned = 0;

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);
            FilledAnswer filledAnswer;

            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId,subHeader.KnowledgeBaseId, TemplateId);

                if (filledAnswer != null)
                {
                    subHeaderMaxPoint = knowledgeBaseBO.GetMaxPointByKnowledgeId(subHeader.KnowledgeBaseId, TemplateId);
                    defaultScore = filledAnswer.DefaultScore;
                    defaultScore = defaultScore != null && defaultScore.Trim() != string.Empty ? defaultScore : "0%";
                    string[] temp = defaultScore.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
                    pointEarned = Convert.ToString((Convert.ToDecimal(temp[0]) / 100) * Convert.ToDecimal(subHeaderMaxPoint));
                    totalPointsEarned = totalPointsEarned + Convert.ToDecimal(pointEarned);
                }
            }

            return totalPointsEarned;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void PopulateSubHeaders(Table masterTable, int knowledgeBaseId, int headerSequence)
    {
        try
        {
            SUM_ELEMENT_EARNED_POINTS = 0;

            Table elementTable = new Table();
            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + knowledgeBaseId;
            elementTable.ClientIDMode = ClientIDMode.Static;

            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 9;
            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            masterTable.Controls.Add(_tableRow);

            string subHeaderId = string.Empty;
            string subHeaderTitle = string.Empty;
            string subHeaderMaxPoint = string.Empty;

            string mustPassText = string.Empty;
            string noteText = string.Empty;
            string completeText = string.Empty;

            string defaultScore = string.Empty;
            string pointEarned = string.Empty;

            int requiredDocsCount = 0;
            int uploadedDocsCount = 0;

            KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
            List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(knowledgeBaseId, TemplateId);
            FilledAnswer filledAnswer;

            int index = 1;
            foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
            {
                subHeaderId = subHeader.KnowledgeBaseId.ToString();
                subHeaderTitle = subHeader.Name;
                subHeaderMaxPoint = knowledgeBaseBO.GetMaxPointByKnowledgeId(subHeader.KnowledgeBaseId, TemplateId);
                mustPassText = subHeader.MustPass == null ? "false" : subHeader.MustPass.ToString().ToLower();

                filledAnswer = knowledgeBaseBO.GetFilledAnswerByProjectIdAndKnowledgeBaseId(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId);

                if (filledAnswer != null)
                {

                    defaultScore = filledAnswer.DefaultScore;
                    defaultScore = defaultScore != null && defaultScore.Trim() != string.Empty ? defaultScore : "0";
                    string[] temp = defaultScore.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
                    defaultScore = temp[0];
                    completeText = filledAnswer.Complete == null || filledAnswer.Complete == false ? "No" : "Yes";
                    pointEarned = Convert.ToString((Convert.ToDecimal(temp[0]) / 100) * Convert.ToDecimal(subHeaderMaxPoint));

                    if (mustPassText == "true")
                    {
                        if (Convert.ToInt32(defaultScore) >= 50)
                            mustPassText = "Yes";
                        else
                            mustPassText = "No";
                    }
                    else
                        mustPassText = "NA";

                    if (filledAnswer.ReviewNotes != null && filledAnswer.ReviewNotes.Trim() != string.Empty)
                        noteText = "Yes";
                    else
                        noteText = "No";

                }
                else
                {
                    mustPassText = (mustPassText == "false" ? "NA" : "No");
                    pointEarned = "0";
                    noteText = "No";
                    completeText = "No";
                }

                requiredDocsCount = Convert.ToInt32(knowledgeBaseBO.GetSubHeaderRequiredDocsCount(ProjectUsageId,SiteId, subHeader.KnowledgeBaseId, TemplateId).ToString());
                uploadedDocsCount = knowledgeBaseBO.GetUploadedDocsCount(subHeader.KnowledgeBaseId, ProjectUsageId,SiteId, TemplateId);

                PopulateElementTable(elementTable, knowledgeBaseId, subHeaderId, subHeaderTitle, subHeaderMaxPoint, mustPassText, requiredDocsCount, uploadedDocsCount,
                    noteText, completeText, pointEarned, headerSequence, index);

                SUM_ELEMENT_EARNED_POINTS = SUM_ELEMENT_EARNED_POINTS + (pointEarned != "" ? Convert.ToDecimal(pointEarned) : Convert.ToDecimal(0));

                index++;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void PopulateElementTable(Table elementTable, int headerId, string subHeaderId, string subHeaderTitle, string maxPoint, string mustPassText, int requiredDocsCount,
        int uploadedDocsCount, string noteText, string completeText, string pointEarned, int headerSequence, int subHeaderSequence)
    {
        try
        {
            _tableRow = new TableRow();
            _tableRow.CssClass = "row-elementdesc";
            _tableCell = new TableCell();
            _tableCell.CssClass = "child-title03";

            _linkButton = new LinkButton();
            _linkButton.ID = DEFAULT_ELEMENT_ID_PREFIX + headerSequence + subHeaderSequence;
            _linkButton.ClientIDMode = ClientIDMode.Static;
            _linkButton.OnClientClick = "javascript:OpenStandard(" + headerSequence + "," + subHeaderSequence + ");";

            string[] headerTitleText = subHeaderTitle.Split(':');

            if (headerTitleText.Count() == 2)
                _linkButton.Text = "<div class='child-title03-head'>" + headerTitleText.First() + ":</div>" +
                    "<div class='child-title03-desc'>" + headerTitleText.Last() + "</div>";
            else
                _linkButton.Text = subHeaderTitle;

            _tableCell.Controls.Add(_linkButton);
            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = "cell";
            _tableCell.Text = maxPoint;

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = "cell";
            _tableCell.Text = pointEarned;

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = mustPassText == "No" ? "no-cell" : "cell";
            _tableCell.Text = mustPassText;

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = "cell";
            _tableCell.Text = requiredDocsCount.ToString();

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = "cell";
            _tableCell.Text = uploadedDocsCount.ToString();

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = noteText == "No" ? "no-cell" : "cell";
            _tableCell.Text = noteText;

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.CssClass = completeText == "No" ? "no-cell" : "cell";
            _tableCell.Text = completeText;

            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);

            _ncqaStandardList.Add(new NCQASummaryDetail(headerId.ToString(), subHeaderId, subHeaderTitle, Convert.ToDecimal(maxPoint), (pointEarned != "" ?
                Convert.ToDecimal(pointEarned) : Convert.ToDecimal(0)), mustPassText,
                    requiredDocsCount, uploadedDocsCount, noteText, completeText));
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void CheckNCQATemplate()
    {
        try
        {
            if (!_MORe.CheckNCQASubmission(TemplateId))
            {
                chbRecognized.Visible = chbReviewed.Visible = chbSubmitted.Visible = false;
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
        }

    }

    private bool IsTemplateOption(int projectUsageId,int templateId,int enterpriseId)
    {
        try
        {
            return _MORe.IsTemplateOption(projectUsageId,templateId,enterpriseId);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool FilledAnswersExist(int projectUsageId, int siteId)
    {
        try
        {
            return _MORe.FilledAnswersExist(projectUsageId, siteId);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CopyFilledAns(int projectUsageId, int siteId)
    {
        try
        {
            _MORe.CopyTempalteOption(projectUsageId, siteId);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
}
