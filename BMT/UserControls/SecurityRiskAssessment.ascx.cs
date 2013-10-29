using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class SecurityRiskAssessment : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int SectionId { get; set; }
    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public int SiteId { get; set; }

    #endregion

    #region CONSTANT

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "SRA Copy";

    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;
            //Check SRA Disclaimer Required
            SRADisclaimerRequired();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            pnlDisclaimer.Visible = false;
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            UserBO userBO = new UserBO();
            userBO.UpdateSRADisclaimerPassed(Convert.ToInt32(Session["UserApplicationId"]));

            pnlTabs.Visible = pnlSRA.Visible = true;
            pnlDisclaimer.Visible = false;

            LoadControls();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnSRASave_Click(object sender, EventArgs e)
    {
        //TabSaving();
    }

    protected void btnRefreshSRA_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateSRACopy();

            //Delete existing Questionaire
            QuestionBO _questionBO = new QuestionBO();
            _questionBO.DeleteAssessmentByProjectUsageAndSiteId((int)enQuestionnaireType.SRAQuestionnaire, Convert.ToInt32(ProjectUsageId),SiteId);

            // Get New Questionnaire By Type
            _questionBO.QuestionnaireId = (int)enQuestionnaireType.SRAQuestionnaire;

            string recievedQuestionnaire = _questionBO.GetNewQuestionnaire();
            Session[enSessionKey.SRAQuestionnaire.ToString()] = recievedQuestionnaire;

            TabLoading();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    #endregion

    #region FUNCTIONS

    protected void SRADisclaimerRequired()
    {
        try
        {
            if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.User.ToString())
            {
                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                string disclaimerRequired = Util.GetSRADisclaimerRequired(enterpriseId);
                if (disclaimerRequired == "YES")
                {
                    UserBO userBO = new UserBO();
                    bool isDisclaimerPassed = userBO.IsSRADisclaimerPassed(Convert.ToInt32(Session["UserApplicationId"]));

                    if (!isDisclaimerPassed)
                    {
                        pnlTabs.Visible = pnlSRA.Visible = false;
                        pnlDisclaimer.Visible = true;

                        string disclaimerString = Util.GetSRADisclaimerText(enterpriseId);
                        lblDisclaimer.Text = disclaimerString;
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
            else
            {
                LoadControls();
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void LoadControls()
    {
        try
        {
            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == "Form")
            {
                if (Session[enSessionKey.UserApplicationId.ToString()] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);

                Logger.PrintInfo("Start Tab Saving");
                Logger.PrintInfo("SiteId: " + SiteId + ", ProjectUsageId: " + ProjectUsageId + ", PracticeId: " + PracticeId);
                TabSaving();                

                // Get Questionnaire By Type
                QuestionBO _questionBO = new QuestionBO();
                _questionBO.QuestionnaireId = (int)enFormType.SecurityRiskAssessment;
                _questionBO.ProjectUsageId = ProjectUsageId;
                _questionBO.SiteId = SiteId;

                // get questionnaire
                string recievedQuestionnaire = _questionBO.GetQuestionnaireByType();
                Session[enSessionKey.SRAQuestionnaire.ToString()] = recievedQuestionnaire;
               
                TabLoading();     
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void TabSaving()
    {
        try
        {
            int activeTab = Convert.ToInt32(hdnCurrentTab.Value);
            switch (activeTab)
            {
                case 1:
                    SRASummary.SiteId = SiteId;
                    SRASummary.ProjectUsageId = ProjectUsageId;
                    SRASummary.PracticeId = PracticeId;
                    SRASummary.SectionId = SectionId;

                    SRASummary.SaveSummary();
                    pnlSRASummary.Visible = false;
                    break;
                case 2:
                    SRAInventory.SiteId = SiteId;
                    SRAInventory.ProjectUsageId = ProjectUsageId;
                    SRAInventory.PracticeId = PracticeId;
                    SRAInventory.SectionId = SectionId;

                    SRAInventory.SaveInventory();
                    pnlSRAInventory.Visible = false;
                    break;
                case 3:
                    SRAScreening.SiteId = SiteId;
                    SRAScreening.ProjectUsageId = ProjectUsageId;
                    SRAScreening.PracticeId = PracticeId;
                    SRAScreening.SectionId = SectionId;

                    SRAScreening.SaveScreening();
                    pnlSRAScreening.Visible = false;
                    break;
                case 4:
                    SRAProcess.SiteId = SiteId;
                    SRAProcess.ProjectUsageId = ProjectUsageId;
                    SRAProcess.PracticeId = PracticeId;
                    SRAProcess.SectionId = SectionId;

                    SRAProcess.SaveProcess();
                    pnlSRAProcess.Visible = false;
                    break;
                case 5:
                    SRATechnology.SiteId = SiteId;
                    SRATechnology.ProjectUsageId = ProjectUsageId;
                    SRATechnology.PracticeId = PracticeId;
                    SRATechnology.SectionId = SectionId;

                    SRATechnology.SaveTechnology();
                    pnlSRATechnology.Visible = false;
                    break;
                case 6:
                    SRAFindings.SiteId = SiteId;
                    SRAFindings.ProjectUsageId = ProjectUsageId;
                    SRAFindings.PracticeId = PracticeId;
                    SRAFindings.SectionId = SectionId;

                    SRAFindings.SaveFinding();
                    pnlSRAFindings.Visible = false;
                    break;
                case 7:
                    SRAFollowup.SiteId = SiteId;
                    SRAFollowup.ProjectUsageId = ProjectUsageId;
                    SRAFollowup.PracticeId = PracticeId;
                    SRAFollowup.SectionId = SectionId;

                    SRAFollowup.SaveFollowup();
                    pnlSRAFollowup.Visible = false;
                    break;
            }

            if (activeTab == 0)
                hdnCurrentTab.Value = hdnNextTab.Value = "1";
            else
                hdnCurrentTab.Value = hdnNextTab.Value;

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void TabLoading()
    {
        try
        {
            int activeTab = Convert.ToInt32(hdnNextTab.Value);
            switch (activeTab)
            {
                case 1:
                    SRASummary.SiteId = SiteId;
                    SRASummary.ProjectUsageId = ProjectUsageId;
                    SRASummary.PracticeId = PracticeId;
                    SRASummary.SectionId = SectionId;

                    SRASummary.GenerateLayout();
                    pnlSRASummary.Visible = true;

                    break;
                case 2:
                    SRAInventory.SiteId = SiteId;
                    SRAInventory.ProjectUsageId = ProjectUsageId;
                    SRAInventory.PracticeId = PracticeId;
                    SRAInventory.SectionId = SectionId;

                    SRAInventory.GenerateLayout();
                    pnlSRAInventory.Visible = true;

                    break;
                case 3:
                    SRAScreening.SiteId = SiteId;
                    SRAScreening.ProjectUsageId = ProjectUsageId;
                    SRAScreening.PracticeId = PracticeId;
                    SRAScreening.SectionId = SectionId;

                    SRAScreening.GenerateLayout();
                    pnlSRAScreening.Visible = true;

                    break;
                case 4:
                    SRAProcess.SiteId = SiteId;
                    SRAProcess.ProjectUsageId = ProjectUsageId;
                    SRAProcess.PracticeId = PracticeId;
                    SRAProcess.SectionId = SectionId;

                    SRAProcess.GenerateLayout();
                    pnlSRAProcess.Visible = true;

                    break;
                case 5:
                    SRATechnology.SiteId = SiteId;
                    SRATechnology.ProjectUsageId = ProjectUsageId;
                    SRATechnology.PracticeId = PracticeId;
                    SRATechnology.SectionId = SectionId;

                    SRATechnology.GenerateLayout();
                    pnlSRATechnology.Visible = true;
                    break;
                case 6:
                    SRAFindings.SiteId = SiteId;
                    SRAFindings.ProjectUsageId = ProjectUsageId;
                    SRAFindings.PracticeId = PracticeId;
                    SRAFindings.SectionId = SectionId;

                    SRAFindings.GenerateLayout();
                    pnlSRAFindings.Visible = true;
                    break;
                case 7:
                    SRAFollowup.SiteId = SiteId;
                    SRAFollowup.ProjectUsageId = ProjectUsageId;
                    SRAFollowup.PracticeId = PracticeId;
                    SRAFollowup.SectionId = SectionId;

                    SRAFollowup.GenerateLayout();
                    pnlSRAFollowup.Visible = true;
                    break;
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

            Directory.Delete(tempDestinationPath, true);

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
