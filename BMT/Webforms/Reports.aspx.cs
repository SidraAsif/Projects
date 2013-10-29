using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.Common;
using BMTBLL.Enumeration;
using BMTBLL.Classes;

using BMT.WEB;
using BMTBLL;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;
using System.Collections;


namespace BMT.Webforms
{
    public partial class Reports : System.Web.UI.Page
    {
        #region PROPERTIES
        public int ProjectId { get; set; }
        public string SiteName { get; set; }
        #endregion

        #region CONSTANTS

        private char[] DEFAULT_STANDARD_SEQUENCE = { '1', '2', '3', '4', '5', '6' };
        private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        // Default ContentType
        private const string DEFAULT_DOC_CONTENT_TYPE = "UploadedDocuments";
        private const int DEFAULT_PRACITCE_ID = 1;
        private string DEFAULT_GENERAL_STATUS_REPORT = "General Status Report";
        #endregion

        #region Variable
        private int practiceId;
        private int userApplicationId;
        private XDocument questionnaire;
        private PCMHStatusReport pcmhStatusReport;
        private PrintBO print;
        private CustomReportBO _customReport;
        private List<PCMHStatusReport> _lstPCMHStatusReport = new List<PCMHStatusReport>();
        private List<OverAllProjectStatusDetails> lstOverAllProjectStatusDetails;
        private List<GroupProjectStatusDetails> lstOverAllBarGraphDetails;



        private string userType;
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteName = DEFAULT_GENERAL_STATUS_REPORT;
            practiceId = 1;

            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                if (!IsPostBack)
                {
                    List<NCQADetails> PCMHStandardList = GetNCQAStandards();
                    List<ConsultingUserDetail> consultantList = GetConsultant();
                    List<PracticeSizeBO> practiceSizeList = GetPracticeSize();

                    lstPCMHStandard.DataSource = PCMHStandardList;
                    lstPCMHStandard.DataTextField = "Name";
                    lstPCMHStandard.DataValueField = "Sequence";
                    lstPCMHStandard.SelectedIndex = 0;
                    lstPCMHStandard.DataBind();

                    lstConsultant.DataSource = consultantList;
                    lstConsultant.DataTextField = "UserName";
                    lstConsultant.DataValueField = "UserID";
                    lstConsultant.SelectedIndex = 0;
                    hiddenConsultantId.Value = "0";
                    lstConsultant.DataBind();

                    lstPracticeSize.DataSource = practiceSizeList;
                    lstPracticeSize.DataTextField = "Name";
                    lstPracticeSize.DataValueField = "ID";
                    lstPracticeSize.SelectedIndex = 0;
                    hiddenPracticeSizeId.Value = "0";
                    lstPracticeSize.DataBind();

                    LoadingProcess();
                }
            }
        }

        public void Save_CustomReport(object sender, EventArgs e)
        {
            SaveCustomReport();
        }

        protected void btnPrintReport_Click(object sender, EventArgs e)
        {
            Session["FilePath"] = string.Empty;
            GenerateReportList();
            PrintPCMHGeneralStatusReport(DEFAULT_GENERAL_STATUS_REPORT);
        }

        #endregion

        #region Function

        public List<NCQADetails> GetNCQAStandards()
        {

            List<NCQADetails> _ncqaStandarList = new List<NCQADetails>();
            try
            {
                QuestionBO questionBO = new QuestionBO();
                _ncqaStandarList.Add(new NCQADetails("0", "All Standards"));
                _ncqaStandarList.AddRange(questionBO.GetStandards());

                //QuestionBO questionBO = new QuestionBO();
                //questionBO.QuestionnaireId = (int)enQuestionnaireType.DetailedQuestionnaire;

                //int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);
                //string recievedQuestionnaire = questionBO.GetNewQuestionnaire(medicalGroupId);
                //Session[enSessionKey.NCQAQuestionnaire.ToString()] = recievedQuestionnaire;

                //XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                //IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                //                                  select standardsRecord;

                //_ncqaStandarList.Add(new NCQADetails("0", "All Standards"));
                //foreach (XElement standard in standards)
                //{
                //    _ncqaStandarList.Add(new NCQADetails(standard.Attribute("sequence").Value.ToString(), "PCMH" + " " + standard.Attribute("sequence").Value.ToString()));
                //}


            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaStandarList;

        }

        public List<ConsultingUserDetail> GetConsultant()
        {
            List<ConsultingUserDetail> lstConsultants = new List<ConsultingUserDetail>();
            ConsultingUserBO consultingUser = new ConsultingUserBO();
            lstConsultants = consultingUser.GetConsultants(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
            ConsultingUserDetail allConsultant = new ConsultingUserDetail(0, "All Consultants");
            lstConsultants.Insert(0, allConsultant);

            return lstConsultants;
        }

        public List<PracticeSizeBO> GetPracticeSize()
        {
            List<PracticeSizeBO> lstPracticeSize = new List<PracticeSizeBO>();
            PracticeSizeBO practiceSize = new PracticeSizeBO();
            lstPracticeSize = practiceSize.GetPracticeSizeByGroupId();
            PracticeSizeBO allConsultant = new PracticeSizeBO(0, "All Practices");
            lstPracticeSize.Insert(0, allConsultant);

            return lstPracticeSize;

        }

        public void SaveCustomReport()
        {
            if (Page.IsPostBack)
            {
                string[] selectedStandardId = hiddenPCMHId.Value.Split(',');
                string selectedElements = hiddenElementId.Value;
                string elements = string.Empty;
                if (hiddenElementId.Value == "0")
                    elements = "0,";
                else
                {
                    for (int elementIndex = 1; elementIndex < selectedElements.Length; elementIndex += 3)
                    {
                        elements += selectedElements[elementIndex] + ",";
                    }
                }
                string[] selectedElementsId = elements.Split(',');
                string[] selectedFactorsId = hiddenFactorArray.Value.Split(',');
                string[] selectedStandardTitle = hiddenPCMHTitle.Value.Split(',');
                string[] selectedElementsTitle = hiddenElementTitle.Value.Split(',');
                string[] selectedFactorsTitle = hiddenFactorTitleArray.Value.Split(',');
                string[] selectedConsultantId = hiddenConsultantId.Value.Split(',');
                string[] selectedConsultantTitle = hiddenConsultantName.Value.Split(',');
                string[] selectedPercentCompleteId = hiddenCompleteId.Value.Split(',');
                string[] selectedPercentCompleteText = hiddenCompleteText.Value.Split(',');
                string[] selectedPracticeSizeId = hiddenPracticeSizeId.Value.Split(',');
                string[] selectedPracticeSizeTitle = hiddenPracticeSizeTitle.Value.Split(',');

                XElement root = new XElement("CustomReport", new XAttribute("TitleLine1", txtLine1.Text), new XAttribute("TitleLine2", txtLine2.Text), new XAttribute("Id", hiddenIsNewRport.Value));

                if (hiddenFactorTitleArray.Value == "All Factors")
                {
                    XElement pcmhFactors = new XElement("PCMHFactors", new XAttribute("Title", "All Factors"), new XAttribute("Sequence", "0"));
                    root.Add(pcmhFactors);
                }
                else
                {
                    for (int factorIndex = 0; factorIndex < selectedFactorsId.Length; factorIndex++)
                    {
                        XElement pcmhFactors = new XElement("PCMHFactors", new XAttribute("Title", selectedFactorsTitle[factorIndex]), new XAttribute("Sequence", selectedFactorsId[factorIndex]));
                        root.Add(pcmhFactors);
                    }
                }
                for (int consultantIndex = 0; consultantIndex < selectedConsultantId.Length; consultantIndex++)
                {
                    XElement consultant = new XElement("Consultant", new XAttribute("Name", selectedConsultantTitle[consultantIndex]), new XAttribute("Id", selectedConsultantId[consultantIndex]));
                    root.Add(consultant);
                }
                for (int percentCompleteIndex = 0; percentCompleteIndex < selectedPercentCompleteId.Length; percentCompleteIndex++)
                {
                    XElement percentComplete = new XElement("PercentComplete", new XAttribute("Title", selectedPercentCompleteText[percentCompleteIndex]), new XAttribute("Id", selectedPercentCompleteId[percentCompleteIndex]));
                    root.Add(percentComplete);
                }
                for (int practiceSizeIndex = 0; practiceSizeIndex < selectedPracticeSizeId.Length; practiceSizeIndex++)
                {
                    XElement practiceSize = new XElement("PracticeSize", new XAttribute("Title", selectedPracticeSizeTitle[practiceSizeIndex]), new XAttribute("Id", selectedPracticeSizeId[practiceSizeIndex]));
                    root.Add(practiceSize);
                }
                XElement displayOption = new XElement("DispalyOption", new XAttribute("Title", "Display Option"), new XAttribute("Id", "1"));
                XElement chkSelectedStandard = new XElement("checkboxStandard", new XAttribute("Title", chkPCMHStandard.Text), new XAttribute("Value", chkPCMHStandard.Checked));
                displayOption.Add(chkSelectedStandard);
                XElement chkSelectedElement = new XElement("checkboxElement", new XAttribute("Title", chkElement.Text), new XAttribute("Value", chkElement.Checked));
                displayOption.Add(chkSelectedElement);
                XElement chkSelectedFactor = new XElement("checkboxFactor", new XAttribute("Title", chkFactor.Text), new XAttribute("Value", chkFactor.Checked.ToString()));
                displayOption.Add(chkSelectedFactor);
                XElement chkSelectedConsultant = new XElement("checkboxConsultant", new XAttribute("Title", chkConsultant.Text), new XAttribute("Value", chkConsultant.Checked.ToString()));
                displayOption.Add(chkSelectedConsultant);
                XElement chkSelectedComplete = new XElement("checkboxComplete", new XAttribute("Title", chkComplete.Text), new XAttribute("Value", chkComplete.Checked.ToString()));
                displayOption.Add(chkSelectedComplete);
                XElement chkSelectedPracticeSize = new XElement("checkboxPracticeSize", new XAttribute("Title", chkPracticeSize.Text), new XAttribute("Value", chkPracticeSize.Checked.ToString()));
                displayOption.Add(chkSelectedPracticeSize);
                XElement chkSelectedPracticeSiteName = new XElement("checkboxPracticeSiteName", new XAttribute("Title", chkPracticeSiteName.Text), new XAttribute("Value", chkPracticeSiteName.Checked.ToString()));
                displayOption.Add(chkSelectedPracticeSiteName);
                XElement chkSelectedPointsEarned = new XElement("checkboxPointsEarned", new XAttribute("Title", chkPointsEarned.Text), new XAttribute("Value", chkPointsEarned.Checked.ToString()));
                displayOption.Add(chkSelectedPointsEarned);
                XElement chkSelectedDocumentsUploaded = new XElement("checkboxDocumentsUploaded", new XAttribute("Title", chkDocumentsUploaded.Text), new XAttribute("Value", chkDocumentsUploaded.Checked.ToString()));
                displayOption.Add(chkSelectedDocumentsUploaded);
                XElement chkSelectedMustPassStatus = new XElement("checkboxMustPassStatus", new XAttribute("Title", chkMustPassStatus.Text), new XAttribute("Value", chkMustPassStatus.Checked.ToString()));
                displayOption.Add(chkSelectedMustPassStatus);
                XElement chkSelectedLastActivityDate = new XElement("checkboxLastActivityDate", new XAttribute("Title", chkLastActivityDate.Text), new XAttribute("Value", chkLastActivityDate.Checked.ToString()));
                displayOption.Add(chkSelectedLastActivityDate);
                XElement chkSelectedConvertElements = new XElement("checkboxConvertElement", new XAttribute("Title", chkConvertElements.Text), new XAttribute("Value", chkConvertElements.Checked.ToString()));
                displayOption.Add(chkSelectedConvertElements);
                XElement chkSelectedOverallbarGraph = new XElement("checkboxOverallbarGraph", new XAttribute("Title", chkOverallbarGraph.Text), new XAttribute("Value", chkOverallbarGraph.Checked.ToString()));
                displayOption.Add(chkSelectedOverallbarGraph);
                XElement chkSelectedOverallGraph = new XElement("checkboxOverallGraph", new XAttribute("Title", chkOverallGraph.Text), new XAttribute("Value", chkOverallGraph.Checked.ToString()));
                displayOption.Add(chkSelectedOverallGraph);
                XElement chkSelectedGroupGraphs = new XElement("checkboxGroupGraphs", new XAttribute("Title", chkGroupGraphs.Text), new XAttribute("Value", chkGroupGraphs.Checked.ToString()));
                displayOption.Add(chkSelectedGroupGraphs);

                root.Add(displayOption);

                _customReport = new CustomReportBO();
                int SectionId = Request.QueryString["sectionId"] != null && Request.QueryString["sectionId"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["sectionId"]) : 0;

                _customReport.SaveCustomReport(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]), root, SectionId, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

                hiddenContentType.Value = txtLine1.Text;
                hiddenSectionId.Value = SectionId.ToString();
                string str = "<script>javascript:onsavereport();</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", str, false);
            }
        }

        private void LoadingProcess()
        {
            userType = Session["UserType"].ToString();

            TreeControl.PracticeId = DEFAULT_PRACITCE_ID;

            if (hdnTreeNodeID.Value != "")
            {
                Session["selectedNodeId"] = hdnTreeNodeID.Value;
            }
            else
            {
                if (Session["selectedNodeId"] != null)
                {
                    hdnTreeNodeID.Value = Session["selectedNodeId"].ToString();
                }
            }
            // Load selected control            
            LoadControl();
        }

        protected void LoadControl()
        {
            try
            {
                // Fetching values form Query string to check the informaiton against the selected Node
                string ContentType = Request.QueryString["NodeContentType"] != null ? Request.QueryString["NodeContentType"] : string.Empty;
                int SectionId = Request.QueryString["sectionId"] != null && Request.QueryString["sectionId"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["sectionId"]) : 0;
                string Path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;

                _customReport = new CustomReportBO();

                if (ContentType != string.Empty && SectionId != 0)
                {
                    hiddenContentType.Value = ContentType;
                    Session["ReturnContentType"] = ContentType;
                }
                if (ContentType != string.Empty && SectionId != 0)
                {
                    pnlDynamicControl.Controls.Clear();
                    Session[enSessionKey.currentControl.ToString()] = ContentType;
                    if (ContentType == DEFAULT_GENERAL_STATUS_REPORT)
                    {
                        pnlReports.Visible = true;
                    }
                    else
                    {
                        string xml = _customReport.GetNewXMLCustomReport(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]), SectionId);
                        XDocument xDocument = XDocument.Parse(xml);

                        XElement pcmhStandard = xDocument.Root.Element("PCMHStandard");
                        txtLine1.Text = xDocument.Root.Attribute("TitleLine1").Value;
                        txtLine2.Text = xDocument.Root.Attribute("TitleLine2").Value;
                        hiddenPCMHId.Value = "0";
                        hiddenElementId.Value = "0";
                        hiddenElementTitle.Value = "All Elements";

                        foreach (XElement factorId in xDocument.Root.Elements("PCMHFactors"))
                        {
                            hiddenFactorId.Value = hiddenFactorId.Value + factorId.Attribute("Sequence").Value + ',';
                            hiddenFactorArray.Value = hiddenFactorArray.Value + factorId.Attribute("Sequence").Value + ',';
                            hiddenFactorTitleArray.Value = hiddenFactorTitleArray.Value + factorId.Attribute("Title").Value + ',';
                        }

                        hiddenFactorId.Value = hiddenFactorId.Value.Remove(hiddenFactorId.Value.Length - 1);
                        hiddenFactorArray.Value = hiddenFactorArray.Value.Remove(hiddenFactorArray.Value.Length - 1);
                        hiddenFactorTitleArray.Value = hiddenFactorTitleArray.Value.Remove(hiddenFactorTitleArray.Value.Length - 1);

                        foreach (XElement consultantId in xDocument.Root.Elements("Consultant"))
                        {
                            hiddenConsultantId.Value = hiddenConsultantId.Value + consultantId.Attribute("Id").Value + ',';
                            hiddenConsultantName.Value = hiddenConsultantName.Value + consultantId.Attribute("Name").Value + ',';
                        }
                        hiddenConsultantId.Value = hiddenConsultantId.Value.Remove(hiddenConsultantId.Value.Length - 1);
                        hiddenConsultantName.Value = hiddenConsultantName.Value.Remove(hiddenConsultantName.Value.Length - 1);

                        hiddenCompleteId.Value = xDocument.Root.Element("PercentComplete").Attribute("Id").Value;

                        foreach (XElement practiceSizeId in xDocument.Root.Elements("PracticeSize"))
                        {
                            hiddenPracticeSizeId.Value = hiddenPracticeSizeId.Value + practiceSizeId.Attribute("Id").Value + ',';
                            hiddenPracticeSizeTitle.Value = hiddenPracticeSizeTitle.Value + practiceSizeId.Attribute("Title").Value + ',';
                        }
                        hiddenPracticeSizeId.Value = hiddenPracticeSizeId.Value.Remove(hiddenPracticeSizeId.Value.Length - 1);
                        hiddenPracticeSizeTitle.Value = hiddenPracticeSizeTitle.Value.Remove(hiddenPracticeSizeTitle.Value.Length - 1);

                        chkPCMHStandard.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxStandard").Attribute("Value").Value);
                        chkElement.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxElement").Attribute("Value").Value);
                        chkFactor.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxFactor").Attribute("Value").Value);
                        chkConsultant.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxConsultant").Attribute("Value").Value);
                        chkComplete.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxComplete").Attribute("Value").Value);
                        chkPracticeSize.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxPracticeSize").Attribute("Value").Value);
                        chkPracticeSiteName.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxPracticeSiteName").Attribute("Value").Value);
                        chkPointsEarned.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxPointsEarned").Attribute("Value").Value);
                        chkDocumentsUploaded.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxDocumentsUploaded").Attribute("Value").Value);
                        chkMustPassStatus.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxMustPassStatus").Attribute("Value").Value);
                        chkLastActivityDate.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxLastActivityDate").Attribute("Value").Value);
                        chkConvertElements.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxConvertElement").Attribute("Value").Value);
                        chkOverallbarGraph.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxOverallbarGraph").Attribute("Value").Value);
                        chkOverallGraph.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxOverallGraph").Attribute("Value").Value);
                        chkGroupGraphs.Checked = Convert.ToBoolean(xDocument.Root.Element("DispalyOption").Element("checkboxGroupGraphs").Attribute("Value").Value);

                        pnlReports.Visible = true;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void PrintPCMHGeneralStatusReport(string name)
        {
            try
            {
                Logger.PrintInfo("PCMH General Status Report generation: Process start.");
                List<PCMHStatusReport> _receivedStatusReport = _lstPCMHStatusReport;
                GetOverAllProjectStatusList();

                ReportViewer viewer = new ReportViewer();

                // Set Report processing Mode
                viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                // Declare parameters for reports
                Logger.PrintDebug("Declaring parameters to report.");

                ReportParameter logoImage = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
                ReportParameter reportTitle1 = new ReportParameter("paramReportTitle1", txtLine1.Text);
                ReportParameter reportTitle2 = new ReportParameter("paramReportTitle2", txtLine2.Text);
                ReportParameter pcmhFactor = new ReportParameter("paramFactor", hiddenFactorTitleArray.Value);
                ReportParameter consultant = new ReportParameter("paramConsultant", hiddenConsultantId.Value);
                ReportParameter percentComplete = new ReportParameter("paramComplete", hiddenCompleteId.Value);
                ReportParameter practiceSize = new ReportParameter("paramPracticeSize", hiddenPracticeSizeId.Value);
                ReportParameter displayStandard = new ReportParameter("paramDisplayStandard", chkPCMHStandard.Checked.ToString());
                ReportParameter displayElement = new ReportParameter("paramDisplayElement", chkElement.Checked.ToString());
                ReportParameter displayFactor = new ReportParameter("paramDisplayFactor", chkFactor.Checked.ToString());
                ReportParameter groupByConsultant = new ReportParameter("paramGroupByConsultant", chkConsultant.Checked.ToString());
                ReportParameter groupByComplete = new ReportParameter("paramGroupByComplete", chkComplete.Checked.ToString());
                ReportParameter groupByPracticeSize = new ReportParameter("paramGroupByPracticeSize", chkPracticeSize.Checked.ToString());
                ReportParameter showPracticeSiteName = new ReportParameter("paramShowPracticeSiteName", chkPracticeSiteName.Checked.ToString());
                ReportParameter showPointsEarned = new ReportParameter("paramShowPointsEarned", chkPointsEarned.Checked.ToString());
                ReportParameter showDocumentUploaded = new ReportParameter("paramShowDocumentUploaded", chkDocumentsUploaded.Checked.ToString());
                ReportParameter showMustPassStatus = new ReportParameter("paramShowMustPassStatus", chkMustPassStatus.Checked.ToString());
                ReportParameter showLastActivityDate = new ReportParameter("paramShowLastActivityDate", chkLastActivityDate.Checked.ToString());
                ReportParameter converElementToPercent = new ReportParameter("paramElementToPercent", chkConvertElements.Checked.ToString());
                ReportParameter showOverAllBarGraph = new ReportParameter("paramOverlAllBarGraph", chkOverallbarGraph.Checked.ToString());
                ReportParameter showOverAllGraph = new ReportParameter("paramOverAllGraph", chkOverallGraph.Checked.ToString());
                ReportParameter showGroupGraph = new ReportParameter("showGroupGraph", chkGroupGraphs.Checked.ToString());

                ReportParameter[] param = { logoImage, reportTitle1,reportTitle2,pcmhFactor,consultant,percentComplete,practiceSize,displayStandard,displayElement,displayFactor,
                                              groupByConsultant,groupByComplete,groupByPracticeSize,showPracticeSiteName,showPointsEarned,showDocumentUploaded,showMustPassStatus,
                                          converElementToPercent,showOverAllBarGraph,showOverAllGraph,showGroupGraph,showLastActivityDate};

                // Create Datasource to report
                Logger.PrintDebug("Create data source to report.");
                ReportDataSource rptReportStatusDataSource = new ReportDataSource("DataSet1", _receivedStatusReport);
                ReportDataSource rptOverAllStatusDataSource = new ReportDataSource("DataSet2", lstOverAllProjectStatusDetails);
                ReportDataSource rptGroupProjectStatusDataSource = new ReportDataSource("DataSet4", lstOverAllBarGraphDetails);

                // Report display name which will be use on export
                viewer.LocalReport.DisplayName = "PCMH Status Report";

                // Clear existing datasource*/
                viewer.LocalReport.DataSources.Clear();

                // Assign new Data source to report*/
                Logger.PrintDebug("Add data source.");
                viewer.LocalReport.DataSources.Add(rptReportStatusDataSource);
                viewer.LocalReport.DataSources.Add(rptOverAllStatusDataSource);
                //viewer.LocalReport.DataSources.Add(rptGroupProjectStatusDataSource);

                // Set Report path
                Logger.PrintDebug("Set report path.");
                viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptPCMHStatusReport.rdlc";
                if (chkOverallbarGraph.Checked == true)
                    viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(GetOverAllBarGroupProjectStatusList);
                if (chkGroupGraphs.Checked == true)
                    viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
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

                path = Util.GetPdfPath(practiceId, DEFAULT_GENERAL_STATUS_REPORT + siteNameMark, true);

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

                print.SaveData(DEFAULT_GENERAL_STATUS_REPORT + siteNameMark, savingPath, System.DateTime.Now, userApplicationId, practiceId, true, DEFAULT_DOC_CONTENT_TYPE);
                Session["FilePath"] = savingPath;
                Logger.PrintInfo("PCMH Status Report generation: Process end.");


                Page.ClientScript.RegisterStartupScript(this.GetType(), "click", "print();", true);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            if (e.Parameters.Count > 0)
            {
                int consultantId = Convert.ToInt32(e.Parameters[0].Values[0]);
                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);

                PCMHReportBO pcmhReportBO = new PCMHReportBO();
                List<GroupProjectStatusDetails> lstGroupProjectStatusDetails = pcmhReportBO.GetGroupProjectStatusList(hiddenFactorArray.Value, consultantId.ToString(), hiddenPracticeSizeId.Value, medicalGroupId);

                e.DataSources.Add(new ReportDataSource("DataSet3", lstGroupProjectStatusDetails));
            }
        }

        private List<PCMHStatusReport> GenerateReportList()
        {
            if (Page.IsPostBack)
            {
                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);

                PCMHReportBO pcmhReport = new PCMHReportBO();
                _lstPCMHStatusReport = pcmhReport.GetPCMHReportData(hiddenFactorArray.Value, hiddenConsultantId.Value, hiddenPracticeSizeId.Value, lstComplete.SelectedItem.ToString(), medicalGroupId);
            }
            return _lstPCMHStatusReport;
        }

        private void GetOverAllProjectStatusList()
        {
            try
            {
                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);

                PCMHReportBO pcmhReportBO = new PCMHReportBO();
                lstOverAllProjectStatusDetails = pcmhReportBO.GetOverAllProjectStatusList(hiddenFactorArray.Value, hiddenConsultantId.Value, medicalGroupId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void GetOverAllBarGroupProjectStatusList(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                int medicalGroupId = new PracticeBO().GetMedicalGroupIdByPracticeId(practiceId);

                PCMHReportBO pcmhReportBO = new PCMHReportBO();
                List<OverAllBarProjectStatusDetails> lstOverAllBarGraphDetails = pcmhReportBO.GetOverAllBarProjectStatusList(hiddenFactorArray.Value, hiddenConsultantId.Value, hiddenPracticeSizeId.Value, medicalGroupId);

                e.DataSources.Add(new ReportDataSource("DataSet4", lstOverAllBarGraphDetails));

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

    }
}