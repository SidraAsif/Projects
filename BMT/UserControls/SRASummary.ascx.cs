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
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class SRASummary : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectUsageId { get; set; }
    public int SectionId { get; set; }
    public int PracticeId { get; set; }
    public int SiteId { get; set; }

    #endregion

    #region Variables
    private XDocument questionnaire;
    private XDocument storedQuestionnaire;

    private int userApplicationId;
    private string userType;
    private int practiceId;

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 6;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Task";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Items";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Items Completed";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Percent Complete";
    private const string DEFAULT_HEADER_PARENT_POS5 = "Review Notes";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Status";

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "SRA Summary Status Report";
    private static string[] TASK = { "Process", "Technology", "Findings", "Followup" };
    #endregion

    #region CONTROLS
    private Table summaryTable;
    private TableRow _tableRow;
    private TableCell _tableCell;

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

    protected void btnPrintSummary_Click(object sender, EventArgs e)
    {
        try
        {
            //GenerateReport(false);
            GenerateSRACopy();
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
              
                bool isLock = Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0 ? questionnaire.Root.Element("Findings").Attribute("Finalize").Value : "false")
                    || Convert.ToBoolean(questionnaire.Root.Elements("Followup").Attributes().Count() > 0 ? questionnaire.Root.Element("Followup").Attribute("Finalize").Value : "false");
                pnlTable.Enabled = !isLock;

                bool isFinalize = Convert.ToBoolean(questionnaire.Root.Elements("Findings").Attributes().Count() > 0 ? questionnaire.Root.Element("Findings").Attribute("Finalize").Value : "false")
                    && Convert.ToBoolean(questionnaire.Root.Elements("Followup").Attributes().Count() > 0 ? questionnaire.Root.Element("Followup").Attribute("Finalize").Value : "false");
                btnNewAssessment.Disabled = !isFinalize;

                LoadContributors();

                pnlSummary.Controls.Clear();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                lblSiteName.Text = siteBO.Name;
                lblsiteDescription.Text = siteBO.Name + ", " + siteBO.GetSiteAddressBySiteId(SiteId);

                summaryTable = new Table();
                summaryTable.ID = "summaryTable";
                summaryTable.ClientIDMode = ClientIDMode.Static;
                pnlSummary.Controls.Add(summaryTable);

                LoadHeader();
                GenerateSummary();
                AddSummaryRows();

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
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        break;
                    case 5:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS5;
                        break;

                    case 6:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS6;
                        break;
                    default:
                        break;
                }

                _tableRow.Controls.Add(_tableCell);
            }

            summaryTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public List<SRASummaryDetails> GenerateSummary()
    {
        try
        {
            List<SRASummaryDetails> lstSRASummary = new List<SRASummaryDetails>();
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                foreach (string task in TASK)
                {
                    XElement root = questionnaire.Root;
                    string reviewNotes = string.Empty;
                    string status = string.Empty;

                    int totalItems = 0;
                    int itemsCompleted = 0;
                    double percentComplete = 0;

                    if (task == "Findings" || task == "Followup")
                    {
                        totalItems = root.Elements(task).Elements("Standard").Count();

                        foreach (XElement standardElement in root.Elements(task).Elements("Standard"))
                        {
                            
                                string sourceId = standardElement.Attribute("SourceId").Value.ToString();
                                if (sourceId.Substring(0, 1) == "2")//Low Risk starts with source id 2
                                {
                                    totalItems--;
                                }
                                else
                                {
                                    string owner = standardElement.Attribute("Owner").Value.ToString();
                                    string remediationSteps = standardElement.Attribute("RemediationSteps").Value.ToString();
                                    string date = standardElement.Attribute("TargetDate").Value.ToString();

                                    if (owner != string.Empty && remediationSteps != string.Empty && date != string.Empty)
                                        itemsCompleted++;
                                }
                        }
                    }
                    else
                    {
                        totalItems = root.Elements(task).Elements("Element").Elements("Standard").Count();
                        foreach (XElement standardElement in root.Elements(task).Elements("Element").Elements("Standard"))
                        {
                            string existingControlEffectiveness = standardElement.Attribute("ExistingControlEffectiveness").Value.ToString();
                            string likelihood = standardElement.Attribute("Likelihood").Value.ToString();
                            string impact = standardElement.Attribute("Impact").Value.ToString();

                            if (existingControlEffectiveness != string.Empty && likelihood != string.Empty && impact != string.Empty)
                                itemsCompleted++;

                            if (standardElement.Elements("PrivateNote").Elements("Note").Count() > 0)
                                reviewNotes = "Yes";
                        }
                    }


                    if (totalItems != 0)
                        percentComplete = (Convert.ToDouble(itemsCompleted) * 100) / Convert.ToDouble(totalItems);

                    if (percentComplete == 100)
                        status = "Complete";
                    else if (itemsCompleted == 0 && totalItems > 0)
                        status = "Not Started";
                    else if (itemsCompleted != 0 && totalItems != 0)
                        status = "Open";

                    lstSRASummary.Add(new SRASummaryDetails(task, totalItems.ToString(), itemsCompleted.ToString(), String.Format("{0:0.00}%", percentComplete), reviewNotes, status));


                }
            }
            return lstSRASummary;
        }
        catch (Exception exception)
        {
            message.Error("An error occured while generating the security risk assessment summary. Please try again/Contact Site Administrator.");
            throw exception;
        }
    }

    protected void AddSummaryRows()
    {
        try
        {
            List<SRASummaryDetails> lstSRASummary = GenerateSummary();

            foreach (SRASummaryDetails element in lstSRASummary)
            {
                _tableRow = new TableRow();
                summaryTable.Controls.Add(_tableRow);

                //Task
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-task";
                _tableCell.Text = element.task;
                _tableRow.Controls.Add(_tableCell);

                //Total Items
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-details";
                _tableCell.Text = element.items;
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableRow.Controls.Add(_tableCell);

                //Item Completed
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-details";
                _tableCell.Text = element.itemCompleted;
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableRow.Controls.Add(_tableCell);


                // Percent Complete  
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-details";
                _tableCell.Text = element.percentComplete;
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableRow.Controls.Add(_tableCell);

                //Review Notes
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-details";
                _tableCell.Text = element.reviewNotes;
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableRow.Controls.Add(_tableCell);

                //Status
                _tableCell = new TableCell();
                _tableCell.ClientIDMode = ClientIDMode.Static;
                _tableCell.CssClass = "summary-details";
                _tableCell.Text = element.status;
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableRow.Controls.Add(_tableCell);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadContributors()
    {
        try
        {
            XElement root = questionnaire.Root;
            XElement practice = root.Elements("Summary").Elements("Contributors").First();
            XElement itConsultant = root.Elements("Summary").Elements("Contributors").Last();

            txtPCName.Text = practice.Attribute("name").Value.ToString();
            txtPCPhone.Text = practice.Attribute("phone").Value.ToString();
            txtPCEmail.Text = practice.Attribute("email").Value.ToString();
            txtITName.Text = itConsultant.Attribute("name").Value.ToString();
            txtITPhone.Text = itConsultant.Attribute("phone").Value.ToString();
            txtITEmail.Text = itConsultant.Attribute("email").Value.ToString();

        }
        catch (Exception exception)
        {
            message.Error("An error occured while loading contributors. Please try again/Contact Site Administrator.");
            throw exception;
        }
    }

    public void SaveSummary()
    {
        try
        {
            if (Page.IsPostBack)
            {
                if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
                {
                    string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                    storedQuestionnaire = XDocument.Parse(RecievedQuestionnaire);

                    XElement root = storedQuestionnaire.Root;
                    XElement practice = root.Elements("Summary").Elements("Contributors").First();
                    XElement itConsultant = root.Elements("Summary").Elements("Contributors").Last();

                    practice.Attribute("name").Value = txtPCName.Text;
                    practice.Attribute("phone").Value = txtPCPhone.Text;
                    practice.Attribute("email").Value = txtPCEmail.Text;
                    itConsultant.Attribute("name").Value = txtITName.Text;
                    itConsultant.Attribute("phone").Value = txtITPhone.Text;
                    itConsultant.Attribute("email").Value = txtITEmail.Text;

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

    public void GenerateReport(bool isSRACopy)
    {
        try
        {
            Logger.PrintInfo("SRA Summary Report generation: Process start.");

            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                List<SRASummaryDetails> lstSRASummary = GenerateSummary();

                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                XElement practice = questionnaire.Root.Elements("Summary").Elements("Contributors").First();
                XElement itConsultant = questionnaire.Root.Elements("Summary").Elements("Contributors").Last();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                string siteName = siteBO.Name;
                string siteAddress = siteBO.Name + ", " + siteBO.GetSiteAddressBySiteId(SiteId);

                UserAccountBO userAccountBO = new UserAccountBO();
                List<string> userDetails = userAccountBO.GetUserInformation(Convert.ToInt32(Session["UserApplicationId"]));

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                Logger.PrintDebug("Declaring parameters to report.");

                ReportParameter paramSiteName = new ReportParameter("paramSiteName", siteName);
                ReportParameter paramSiteAddress = new ReportParameter("paramSiteAddress", siteAddress);
                //ReportParameter paramDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
                ReportParameter paramDate = new ReportParameter("paramDate", System.DateTime.Now.ToString());
                ReportParameter paramLogo = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");

                ReportParameter paramUserDetails = new ReportParameter("paramUserDetails", userDetails[2] + " " + userDetails[3]);

                ReportParameter paramPCName = new ReportParameter("paramPCName", practice.Attribute("name").Value);
                ReportParameter paramPCPhone = new ReportParameter("paramPCPhone", practice.Attribute("phone").Value);
                ReportParameter paramPCEmail = new ReportParameter("paramPCEmail", practice.Attribute("email").Value);
                ReportParameter paramITName = new ReportParameter("paramITName", itConsultant.Attribute("name").Value);
                ReportParameter paramITPhone = new ReportParameter("paramITPhone", itConsultant.Attribute("phone").Value);
                ReportParameter paramITEmail = new ReportParameter("paramITEmail", itConsultant.Attribute("email").Value);

                ReportParameter[] param = { paramSiteName, paramSiteAddress, paramDate, paramLogo, paramPCName, paramPCPhone, paramPCEmail,
                                      paramITName, paramITPhone, paramITEmail, paramUserDetails};

                // Create Datasource to report
                Logger.PrintDebug("Set data source to report.");

                ReportDataSource rptDataSource = new ReportDataSource("SRASummaryDS", lstSRASummary);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rptDataSource);

                viewer.LocalReport.DisplayName = "SRA Summary";
                viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptSRASummary.rdlc";
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
                string path = Util.GetPdfPath(PracticeId, DEFAULT_REPORT_TITLE + siteNameMark, false);

                if (isSRACopy)
                    path = Util.GetTempPdfPath(PracticeId, "0");

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
                    print.SaveSRAData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, userApplicationId, PracticeId, DEFAULT_DOC_CONTENT_TYPE,SectionId,ProjectUsageId);
                    Session["FilePath"] = savingPath;
                }

                Logger.PrintInfo("NCQA Summary Report generation: Process end.");
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void GenerateSRACopy()
    {
        try
        {
            SRASummary sraSummary = (SRASummary)Page.LoadControl("~/UserControls/SRASummary.ascx");
            sraSummary.SiteId = SiteId;
            sraSummary.ProjectUsageId = ProjectUsageId;
            sraSummary.PracticeId = PracticeId;
            sraSummary.GenerateReport(true);

            SRAInventory sraInventory = (SRAInventory)Page.LoadControl("~/UserControls/SRAInventory.ascx");
            sraInventory.SiteId = SiteId;
            sraInventory.ProjectUsageId = ProjectUsageId;
            sraInventory.PracticeId = PracticeId;
            sraInventory.GenerateReport(true);

            SRAScreening sraScreening = (SRAScreening)Page.LoadControl("~/UserControls/SRAScreening.ascx");
            sraScreening.SiteId = SiteId;
            sraScreening.ProjectUsageId = ProjectUsageId;
            sraScreening.PracticeId = PracticeId;
            sraScreening.GenerateReport(true);

            SRAProcess sraProcess = (SRAProcess)Page.LoadControl("~/UserControls/SRAProcess.ascx");
            sraProcess.SiteId = SiteId;
            sraProcess.ProjectUsageId = ProjectUsageId;
            sraProcess.PracticeId = PracticeId;
            sraProcess.GenerateReport(true);

            SRATechnology sraTechnology = (SRATechnology)Page.LoadControl("~/UserControls/SRATechnology.ascx");
            sraTechnology.SiteId = SiteId;
            sraTechnology.ProjectUsageId = ProjectUsageId;
            sraTechnology.PracticeId = PracticeId;
            sraTechnology.GenerateReport(true);

            SRAFindings sraFindings = (SRAFindings)Page.LoadControl("~/UserControls/SRAFindings.ascx");
            sraFindings.SiteId = SiteId;
            sraFindings.ProjectUsageId = ProjectUsageId;
            sraFindings.PracticeId = PracticeId;
            sraFindings.GenerateReport(true);

            SRAFollowup sraFollowup = (SRAFollowup)Page.LoadControl("~/UserControls/SRAFollowup.ascx");
            sraFollowup.SiteId = SiteId;
            sraFollowup.ProjectUsageId = ProjectUsageId;
            sraFollowup.PracticeId = PracticeId;
            sraFollowup.GenerateReport(true);

            SiteBO siteBO = new SiteBO();
            siteBO.GetSiteBySiteId(SiteId);
            string siteName = siteBO.Name;

            siteName = siteName.Replace(",", "").Replace("//", "/").Replace("/", "");
            string siteNameMark = " " + "(" + siteName + ")";

            string path = Util.GetTempDestinationPath(PracticeId);

            string[] pathList = path.Split(',');
            string tempDestinationPath = pathList[0]; /*0= Destination path to store the pdf on server*/
            string savingPath = pathList[1]; /*1= saving path to store the file location in database */

            string destinationPath = tempDestinationPath.Replace("Temp/", DEFAULT_REPORT_TITLE + siteNameMark + ".pdf");
            savingPath += DEFAULT_REPORT_TITLE + siteNameMark + ".pdf";

            string[] sourceFiles = Directory.GetFiles(tempDestinationPath);

            CombineMultiplePDFs(sourceFiles, destinationPath);

            
            
            PrintBO print = new PrintBO();
            print.SaveSRAData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, Convert.ToInt32(Session["UserApplicationId"]), PracticeId, DEFAULT_DOC_CONTENT_TYPE,SectionId,ProjectUsageId);
            Session["FilePath"] = savingPath;
            //PrintBO print = new PrintBO();
            //print.SaveSRAData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, Convert.ToInt32(Session["UserApplicationId"]), PracticeId, DEFAULT_DOC_CONTENT_TYPE);

            Directory.Delete(tempDestinationPath, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    public static void CombineMultiplePDFs(string[] sourceFiles, string outFile)
    {
        try
        {
            int pageOffset = 0;
            int fileIndex = 0;

            ArrayList master = new ArrayList();

            Document document = null;
            PdfCopy writer = null;

            while (fileIndex < sourceFiles.Length)
            {
                PdfReader reader = new PdfReader(sourceFiles[fileIndex]);
                reader.ConsolidateNamedDestinations();

                int numberOfPages = reader.NumberOfPages;

                pageOffset += numberOfPages;
                if (fileIndex == 0)
                {
                    document = new Document(reader.GetPageSizeWithRotation(1));
                    writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                    document.Open();
                }

                for (int index = 0; index < numberOfPages; )
                {
                    if (writer != null)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, ++index);
                        writer.AddPage(page);
                    }
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null && writer != null)
                {
                    writer.CopyAcroForm(reader);
                }

                fileIndex++;
            }

            if (document != null)
            {
                document.Close();
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }
    #endregion
}
