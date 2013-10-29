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

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;

public partial class NCQASummary : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectId { get; set; }
    public string SiteName { get; set; }

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 9;
    private const string DEFAULT_HEADER_PARENT_POS1 = "";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Standard";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Max Points";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Points Earned";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Must Pass";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Docs Required";
    private const string DEFAULT_HEADER_PARENT_POS7 = "Docs Uploaded";
    private const string DEFAULT_HEADER_PARENT_POS8 = "Notes";
    private const string DEFAULT_HEADER_PARENT_POS9 = "Complete";
    private const string DEFAULT_STANDARD_ID_PREFIX = "lblSummayStandard";
    private const string DEFAULT_STANDARD_IMAGE_ID_PREFIX = "imgStandard";
    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "elementTable";
    private const string DEFAULT_ELEMENT_ID_PREFIX = "lblSummayelement";
    private const string DEFAULT_GRANDTOTAL_TABLE_ID_PREFIX = "grandTotalTable";
    private const string DEFAULT_GRANDTOTAL_LABEL_ID_PREFIX = "lblTotal";
    private const string DEFAULT_GRANDTOTAL_LABEL_TEXT = "Total:";
    private const string DEFAULT_REPORT_TITLE = "Summary Status Report";
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
    private System.Web.UI.WebControls.Image _image;

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                userApplicationId = Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]);
                userType = Session[enSessionKey.UserType.ToString()].ToString();
                practiceId = Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]);
                hdnSummaryProjectId.Value = ProjectId.ToString();
            }
            else
            {
                SessionHandling sessionHandling = new SessionHandling();
                sessionHandling.ClearSession();
                Response.Redirect("~/Account/Login.aspx");
            }

            if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

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

            }
            else
                message.Info("Questionnaire against the selected site doesn't exist.");
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

    protected void btnSaveNCQACredentials_Click(object sender, EventArgs e)
    {
        try
        {
            //Encrypt password
            Security security = new Security();
            string password = security.Encrypt(txtPassword.Text);

            NCQASubmissionBO ncqaSubmissionBO = new NCQASubmissionBO();
            ncqaSubmissionBO.InsertNCQASubmission(ProjectId, txtLicenseNumber.Text, txtUserName.Text, password);

            Email email = new Email();
            email.SendNCQARequest(ProjectId, txtLicenseNumber.Text, txtUserName.Text, txtPassword.Text, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            bool requestExists = ncqaSubmissionBO.NCQASubmissionRequestExists(ProjectId);
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

    protected void GenerateLayOut()
    {
        try
        {
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
            XElement root = questionnaire.Root;
            TOTAL_NCQA_POINTS = 0;
            TOTAL_NCQA_EARNER_POINTS = 0;
            TOTAL_NCQA_REQUIRED_DOCS = 0;
            TOTAL_NCQA_UPLOADED_DOCS = 0;
            UNIQUE_DOCS = 0;
            FINAL_MUSTPASS_CHECK = "Yes";

            List<string> lstUniqueDocs = (from docRecord in questionnaire.Descendants("DocFile")
                                         select docRecord.Attribute("location").ToString()).Distinct().ToList();

            List<string> lstUnAssociatedDocs = (from docRecord in questionnaire.Descendants("UnAssociatedDoc").Descendants("DocFile")
                                          select docRecord.Attribute("location").ToString()).Distinct().ToList();

            UNIQUE_DOCS = lstUniqueDocs.Count() - lstUnAssociatedDocs.Count();
            
            // Add all available standard
            foreach (XElement standardElement in root.Elements("Standard"))
            {
                string standardSequence = standardElement.Attribute("sequence").Value;
                string standardTitle = standardElement.Attribute("title").Value;
                string maxPoints = standardElement.Attribute("maxPoints").Value;

                // Store MaxPoints for grand total
                TOTAL_NCQA_POINTS = TOTAL_NCQA_POINTS + Convert.ToDecimal(maxPoints);

                string mustPass = string.Empty;

                IEnumerable<XElement> mustPassElement = from element in standardElement.Descendants("Element")
                                                        where (string)element.Attribute("mustPass") == "Yes"
                                                        select element;

                IEnumerable<XElement> Element = from element in standardElement.Descendants("Element")
                                                select element;

                IEnumerable<XElement> CompleteElement = from element in standardElement.Descendants("Element")
                                                        where (string)element.Attribute("complete") == "Yes"
                                                        select element;

                // Check if Standard must pass element: pass/Fail
                if (mustPassElement.Count() > 0)
                {
                    foreach (XElement calculation in mustPassElement.Elements("Calculation"))
                    {
                        string scoredResult = calculation.Attribute("defaultScore").Value;
                        scoredResult = scoredResult.Trim() != string.Empty ? scoredResult.Replace("%", "") : "0";

                        if (Convert.ToInt32(scoredResult) >= 50) // Passing criteria is 50%
                            mustPass = "Yes";
                        else
                            mustPass = FINAL_MUSTPASS_CHECK = "No";
                    }
                }
                else
                    mustPass = FINAL_MUSTPASS_CHECK = "No";

                string[] standardTitleParts = standardTitle.Split(':');

                List<string> _docsDetails = GetDocsDetail(standardElement);

                // store Docs Detail for grand total
                TOTAL_NCQA_REQUIRED_DOCS = TOTAL_NCQA_REQUIRED_DOCS + Convert.ToInt32(_docsDetails[0]);
                TOTAL_NCQA_UPLOADED_DOCS = TOTAL_NCQA_UPLOADED_DOCS + Convert.ToInt32(_docsDetails[1]);

                // if any Reviewer notes are present in any of the sub-Elements
                string NoteExist = string.Empty;
                foreach (XElement reviewerNotes in Element.Elements("ReviewerNotes"))
                {
                    if (reviewerNotes.Value.Trim() != string.Empty)
                        NoteExist = "Yes";
                }

                // if all sub-Elements are marked Complete in the Reviewer checkbox
                string Complete = string.Empty;
                if (Element.Count() == CompleteElement.Count())
                    Complete = "Yes";

                string[] elementDetails = { maxPoints, string.Empty, mustPass, _docsDetails[0], _docsDetails[1], NoteExist, Complete };

                _tableRow = new TableRow();
                _tableCell = new TableCell();
                _tableCell.CssClass = "child-title02";
                _tableCell.ColumnSpan = 2;

                LinkButton linkButton = new LinkButton();

                //_label = new Label();
                linkButton.ID = DEFAULT_STANDARD_ID_PREFIX + standardSequence;

                try
                {
                    linkButton.Text = "<div class='child-title02-head'>" + standardTitleParts[0] + ":</div>" +
                        "<div class='child-title02-desc'>" + standardTitleParts[1] + "</div>";
                }
                catch // If text portion is missing in PCMH Title
                {
                    linkButton.Text = standardTitle;
                }
                finally
                {
                    linkButton.ClientIDMode = ClientIDMode.Static;
                    linkButton.OnClientClick = "javascript:updateClickTab(" + standardSequence + ");";

                    _image = new System.Web.UI.WebControls.Image();
                    _image.ID = DEFAULT_STANDARD_IMAGE_ID_PREFIX + standardSequence;
                    _image.ImageUrl = "~/Themes/Images/Plus.png";
                    _image.CssClass = "img-toggle";
                    _image.ClientIDMode = ClientIDMode.Static;
                    _image.Attributes.Add("onclick", "standardTracking('" + standardSequence + "');");

                    _tableCell.Controls.Add(_image);
                    _tableCell.Controls.Add(linkButton);

                    _tableRow.Controls.Add(_tableCell);

                    int index = 0;
                    int columnIndex = 3;

                    // Add points, docs & other details against the current standard
                    // Column 3-9
                    for (index = 0; index < elementDetails.Length; index++)
                    {
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "cell0" + columnIndex;

                        _label = new Label();
                        _label.ID = DEFAULT_STANDARD_ID_PREFIX + standardSequence + columnIndex;
                        _label.ClientIDMode = ClientIDMode.Static;
                        _label.Text = elementDetails[index];

                        if (elementDetails[index] == "No")
                            _label.CssClass = "none";

                        _tableCell.Controls.Add(_label);
                        _tableRow.Controls.Add(_tableCell);

                        columnIndex = columnIndex + 1;
                    }

                    masterTable.Controls.Add(_tableRow);

                    // Add Standard Row in datasource
                    // Add Standard Row in List
                    _ncqaStandardList.Add(new NCQASummaryDetail(standardSequence,
                                                                        string.Empty,
                                                                        standardTitle,
                                                                        Convert.ToDecimal(maxPoints),
                                                                        Convert.ToDecimal(maxPoints),
                                                                        mustPass,
                                                                        Convert.ToInt32(_docsDetails[0]),
                                                                        Convert.ToInt32(_docsDetails[1]),
                                                                        NoteExist,
                                                                        Complete));

                    // Fetching list of element against current Standard
                    GetELEMENTList(masterTable, standardSequence, standardElement);

                    // Store earned potins for grand total
                    TOTAL_NCQA_EARNER_POINTS = TOTAL_NCQA_EARNER_POINTS + Convert.ToDecimal(SUM_ELEMENT_EARNED_POINTS);
                }

                // SUM_ELEMENT_EARNED_POINTS
                string earnedLableId = DEFAULT_STANDARD_ID_PREFIX + standardSequence + "4";

                try
                {
                    Label lblTotalEarnedPoints = (Label)pnlNCQASummary.FindControl(earnedLableId);
                    lblTotalEarnedPoints.Text = Convert.ToString(SUM_ELEMENT_EARNED_POINTS);
                    foreach (NCQASummaryDetail value in _ncqaStandardList)
                    {
                        if (value.StandardSequence == standardSequence && value.ElementSequence == string.Empty)
                            value.EarnedPoints = SUM_ELEMENT_EARNED_POINTS;
                    }

                }
                catch
                {
                    continue;
                }
            }


            //NCQA Status
            if (questionnaire.Root.Elements("Status").Any())
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
            }

        }
        catch (Exception exception)
        {
            message.Error("An error occured while generating the Report. Please try again/Contact Site Administrator.");
            throw exception;
        }
    }

    protected void GetELEMENTList(Table masterTable, string standardSequence, XElement standardElement)
    {
        try
        {
            SUM_ELEMENT_EARNED_POINTS = 0;

            Table elementTable = new Table();
            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + standardSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;

            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 9;
            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            masterTable.Controls.Add(_tableRow);

            // Add all Elements against the current standard
            foreach (XElement Element in standardElement.Elements("Element"))
            {
                string elementSequence = Element.Attribute("sequence").Value;
                string elementTitle = Element.Attribute("title").Value;
                string maxPoints = Element.Attribute("maxPoints").Value;
                string mustPass = Element.Attribute("mustPass").Value; ;
                string complete = Element.Attribute("complete").Value;
                string[] elementTitlePart = elementTitle.Split(':');
                if (mustPass != "No")
                {
                    foreach (XElement calculation in Element.Elements("Calculation"))
                    {

                        string scoredResult = calculation.Attribute("defaultScore").Value;
                        scoredResult = scoredResult.Trim() != string.Empty ? scoredResult.Replace("%", "") : "0";

                        if (Convert.ToInt32(scoredResult) >= 50)
                            mustPass = "Yes";
                        else
                            mustPass = "No";
                    }
                }
                else
                    mustPass = "";

                // Get Scored Points
                decimal scoredPoints = GetScoredPoints(Element);
                string finalScoredPoints = Convert.ToString((scoredPoints / 100) * Convert.ToDecimal(maxPoints));

                // Sum Element Points
                SUM_ELEMENT_EARNED_POINTS = SUM_ELEMENT_EARNED_POINTS + Convert.ToDecimal(finalScoredPoints);

                // Get Docs Details
                List<string> docsDetails = GetDocsDetail(Element);

                // Get Element Notes & Completion details
                string NoteExist = string.Empty;
                NoteExist = "";

                // Add Reviewer Notes
                foreach (XElement reviewerNotes in Element.Elements("ReviewerNotes"))
                {
                    if (reviewerNotes.Value.Trim() != string.Empty)
                        NoteExist = "Yes";
                }

                string[] ElementDetails = { maxPoints, finalScoredPoints, mustPass, docsDetails[0], docsDetails[1], NoteExist, complete };

                _tableRow = new TableRow();
                _tableRow.CssClass = "row-elementdesc";
                _tableCell = new TableCell();
                _tableCell.CssClass = "child-title03";

                LinkButton linkButton = new LinkButton();
                linkButton.ID = DEFAULT_ELEMENT_ID_PREFIX + standardSequence + elementSequence;

                try
                {
                    linkButton.Text = "<div class='child-title03-head'>" + elementTitlePart[0] + ":</div>" +
                          "<div class='child-title03-desc'>" + elementTitlePart[1] + "</div>";
                }
                catch
                {
                    linkButton.Text = elementTitle;
                }
                finally
                {
                    linkButton.ClientIDMode = ClientIDMode.Static;
                    linkButton.OnClientClick = "javascript:OpenStandard(" + standardSequence + "," + elementSequence + ");";

                    _tableCell.Controls.Add(linkButton);
                    _tableRow.Controls.Add(_tableCell);

                    int index = 0;
                    int columnIndex = 3;

                    /*Add points, docs & other details against the current element*/
                    /* Column 3-9*/
                    for (index = 0; index < ElementDetails.Length; index++)
                    {
                        _tableCell = new TableCell();
                        _tableCell.CssClass = "cell0" + columnIndex;

                        _label = new Label();
                        _label.ID = DEFAULT_ELEMENT_ID_PREFIX + standardSequence + elementSequence + columnIndex;
                        _label.ClientIDMode = ClientIDMode.Static;
                        _label.Text = ElementDetails[index];

                        if (ElementDetails[index] == "No")
                        { _label.CssClass = "none"; }

                        _tableCell.Controls.Add(_label);
                        _tableRow.Controls.Add(_tableCell);
                        columnIndex = columnIndex + 1;
                    }

                    /*Add Element Row in datasource*/
                    _ncqaStandardList.Add(new NCQASummaryDetail(standardSequence,
                                                                       elementSequence,
                                                                       elementTitle,
                                                                       Convert.ToDecimal(maxPoints),
                                                                       Convert.ToDecimal(finalScoredPoints),
                                                                       mustPass,
                                                                       Convert.ToInt32(docsDetails[0]),
                                                                       Convert.ToInt32(docsDetails[1]),
                                                                       NoteExist,
                                                                       complete));

                    /*Add Element Row in table inside the current Standard*/
                    elementTable.Controls.Add(_tableRow);
                }
            }

        }
        catch (Exception exception) { throw exception; }
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
            POINTS_PERCENTAGE_Value = String.Format("{0:0.00}", (TOTAL_NCQA_EARNER_POINTS * 100) / TOTAL_NCQA_POINTS);
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

    protected List<string> GetDocsDetail(XElement element)
    {
        try
        {
            int RequiredDocs = 0;
            int UploadedDocs = 0;
            IEnumerable<XElement> elementFactor = from elementList in element.Descendants("Factor")
                                                  select elementList;
            /*Calculate Required/uploaded docs for All factors Or single factor*/
            foreach (XElement docElement in elementFactor)
            {

                IEnumerable<XElement> DocsElements = from docElementList in docElement.Elements()
                                                     select docElementList;
                foreach (XElement _docElements in DocsElements)
                {
                    if (_docElements.Name.ToString() == enDocType.Policies.ToString() ||
                        _docElements.Name.ToString() == enDocType.Reports.ToString() ||
                        _docElements.Name.ToString() == enDocType.Screenshots.ToString() ||
                        _docElements.Name.ToString() == enDocType.LogsOrTools.ToString() ||
                        _docElements.Name.ToString() == enDocType.OtherDocs.ToString())
                    {
                        string requiredDocsValue = _docElements.Attribute("required").Value.Trim() != string.Empty ? _docElements.Attribute("required").Value.Trim() : "0";

                        RequiredDocs = RequiredDocs + Convert.ToInt32(requiredDocsValue);
                        foreach (XElement _docs in _docElements.Elements())
                        {
                            UploadedDocs = UploadedDocs + 1;
                        }
                    }
                }

            }

            List<string> _docsDetail = new List<string>();
            _docsDetail.Add(RequiredDocs.ToString());
            _docsDetail.Add(UploadedDocs.ToString());

            return _docsDetail;
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    protected decimal GetScoredPoints(XElement element)
    {
        try
        {
            decimal scoredPoints = 0;
            IEnumerable<XElement> CalculationElement = from pointList in element.Descendants("Calculation")
                                                       select pointList;
            /*Calculate scored point in current Element*/
            /*Note: It can be use for all nested "calculation" elements*/
            foreach (XElement _caclulationElement in CalculationElement)
            {
                string elementName = _caclulationElement.Name.ToString();
                if (elementName == enQuestionnaireElements.Calculation.ToString())
                {
                    string DefaultScoreValue = _caclulationElement.Attribute("defaultScore").Value;
                    DefaultScoreValue = DefaultScoreValue.Trim() != string.Empty ? DefaultScoreValue.Replace("%", "") : "0";
                    scoredPoints = scoredPoints + Convert.ToDecimal(DefaultScoreValue);
                }
            }
            return scoredPoints;

        }
        catch (Exception exception) { throw exception; }
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
                name = name + " - NCQA PCMH 2011";
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
                print.SaveData(DEFAULT_REPORT_DETAIL_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, DEFAULT_DOC_CONTENT_TYPE);
                Session["FilePath"] = savingPath;
            }
            else if (!_isDetail)
            {
                print.SaveData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, DEFAULT_DOC_CONTENT_TYPE);
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

            print.SaveData(DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, DEFAULT_DOC_CONTENT_TYPE);
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
        string RecievedQuestionnaire = Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();
        questionnaire = XDocument.Parse(RecievedQuestionnaire);

        foreach (XElement standard in questionnaire.Root.Elements("Standard"))
        {
            foreach (XElement element in standard.Elements("Element"))
            {
                foreach (XElement reviewerNotes in element.Elements("ReviewerNotes"))
                {
                    if (reviewerNotes.Value != string.Empty)
                    {
                        _NCQANotesDetails = new NCQANotesDetails();
                        _NCQANotesDetails.PCMHId = standard.Attribute("sequence").Value;

                        _NCQANotesDetails.PCMHTitle = standard.Attribute("title").Value.Split(':')[0];
                        _NCQANotesDetails.ElementId = element.Attribute("sequence").Value;
                        _NCQANotesDetails.ElementTitle = element.Attribute("title").Value.Split(':')[0];
                        _NCQANotesDetails.Type = reviewerNotes.Name.ToString();
                        _NCQANotesDetails.Notes = reviewerNotes.Value;
                        _lstSRANotesDetails.Add(_NCQANotesDetails);
                    }
                }

                foreach (XElement evaluationNotes in element.Elements("EvaluationNotes"))
                {
                    if (evaluationNotes.Value != string.Empty)
                    {
                        _NCQANotesDetails = new NCQANotesDetails();
                        _NCQANotesDetails.PCMHId = standard.Attribute("sequence").Value;


                        _NCQANotesDetails.PCMHTitle = standard.Attribute("title").Value.Split(':')[0];
                        _NCQANotesDetails.ElementId = element.Attribute("sequence").Value;

                        _NCQANotesDetails.ElementTitle = element.Attribute("title").Value.Split(':')[0];
                        _NCQANotesDetails.Type = evaluationNotes.Name.ToString();

                        _NCQANotesDetails.Notes = evaluationNotes.Value;
                        _lstSRANotesDetails.Add(_NCQANotesDetails);
                    }
                }
            }
        }

        return _lstSRANotesDetails;
    }

    public string GenerateNotesReport(string mapPath, Uri uri, string virtualDirectory, string recievedQuestionnaire)
    {
        try
        {
            Session[enSessionKey.NCQAQuestionnaire.ToString()] = recievedQuestionnaire;

            _lstSRANotesDetails = new List<NCQANotesDetails>();
            _lstSRANotesDetails = GenerateNotesList();

            byte[] bytes = GetPDFBytes();
            print = new PrintBO();

            // Getting file path to db
            SiteName = SiteName.Replace(",", "").Replace("//", "/").Replace("/", "");

            string siteNameMark = " " + "(" + SiteName + ")";
            string path = Util.GetPdfPath(practiceId, DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, true, mapPath, uri, virtualDirectory);

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
            print.SaveData(DEFAULT_NOTES_DETAILED_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, DEFAULT_DOC_CONTENT_TYPE);
            
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

    #endregion
}
