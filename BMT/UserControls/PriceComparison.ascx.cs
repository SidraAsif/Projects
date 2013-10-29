#region Modification History

//  ******************************************************************************
//  Module        : EHR Price Calculator Report
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 
//  Description   : EHR/PM Total Cost of Ownership (TOC) - 5 Years Comparison Report
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
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class PriceComparison : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const int DEFAULT_SUBSCRIPTION_QUESTIONNAIRE_ID = (int)enQuestionnaireType.Subscription;
    private const int DEFAULT_LECENSE_QUESTIONNAIRE_ID = (int)enQuestionnaireType.License;

    private readonly string[] DEFAULT_ALLOWED_PROVIDER_TITLE = { "Subscription", "Maintenance" };
    private const string DEFAULT_EHR_INTERFACE_TYPE_TEXT = "Interface";
    private const string DEFAULT_EHR_OTHER_TYPE_TEXT = "Other";

    private const string DEFAULT_REPORT_TITLE = "EHR Total Cost of Ownership (TOC) - 5 Years Comparison";

    private const string DEFAULT_GRAPH_COLUMN1_TITLE = "Year 1";
    private const string DEFAULT_GRAPH_COLUMN2_TITLE = "Year 2";
    private const string DEFAULT_GRAPH_COLUMN3_TITLE = "Year 3";
    private const string DEFAULT_GRAPH_COLUMN4_TITLE = "Year 4";
    private const string DEFAULT_GRAPH_COLUMN5_TITLE = "Year 5";

    // Default ContentType
    private const string DEFAULT_DOC_CONTENT_TYPE = "EHRUploadedDocuments";
    #endregion

    #region PROPERTIES
    public string PracticeName { get; set; }
    public int ProjectUsageId { get; set; }
    public int SectionId { get; set; }
    public string SystemSequence { get; set; }

    #endregion

    #region VARIABLES
    private QuestionBO _questionBO;
    private PrintBO _printBO;
    private SessionHandling _sessionHandling;

    // User information
    private int userId;
    private int practiceId;

    // Qyery string
    private string activeNodeId;
    private string path;
    private string activeVoucherId;

    // filled questionnaires
    private XDocument _filledQuestionnaire;

    // Providers
    private int totalPhysician;
    private int totalMidLevelProvider;
    private int totalPartTimeProvider;
    private int totalDiscounts;
    private int totalOthers;

    // Total Number of providers    
    private Dictionary<string, string> _totalProvidersOfSystem = new Dictionary<string, string>();
    private Dictionary<string, string> _systemProviderInfo = new Dictionary<string, string>();

    // System detail list
    private int id; // Primary key of DataTable

    private string sequence;

    private string systemName;
    private string purchaseModel;

    private double ongoingAmount;
    private double oneTimeAmount;

    private double yearOneCost;
    private double YearTwoToFive;
    private double TOCYearTwo;
    private double TOCYearThree;
    private double TOCYearFour;
    private double TOCYearFive;

    private List<PriceCalculationDetail> _listOfSystemDetails = new List<PriceCalculationDetail>();
    private List<PriceCalculationDetail> _listOfChartValues = new List<PriceCalculationDetail>();

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageLoadingProcess();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void gvPriceDetails_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvPriceDetails.DataSource = _listOfSystemDetails;
            gvPriceDetails.DataBind();

            gvPriceDetails.PageIndex = e.NewPageIndex;

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            PrintPriceComparison(true, DEFAULT_REPORT_TITLE);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnSaveDoc_Click(object sender, EventArgs e)
    {
        try
        {
            PrintPriceComparison(false, DEFAULT_REPORT_TITLE);
            message.Success("Report has been saved successfully.");
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            message.Error("Report couldn't be saved. Please try again/ Contact your site Administrator.");
        }
    }

    #endregion

    #region FUNCTIONS
    private void PageLoadingProcess()
    {

        #region SESSION_CHECK
        if (Session["UserApplicationId"] != null && Session["UserType"] != null && Session["PracticeId"] != null)
        {
            userId = Convert.ToInt32(Session["UserApplicationId"]);
            practiceId = Convert.ToInt32(Session["PracticeId"]);
        }
        else
        {
            _sessionHandling = new SessionHandling();
            _sessionHandling.ClearSession();
            Response.Redirect("~/Account/Login.aspx");
        }

        #endregion


        activeNodeId = Request.QueryString["Active"] != null ? Request.QueryString["Active"] : string.Empty;
        path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;
        activeVoucherId = Request.QueryString["activeVoucherId"] != null ? Request.QueryString["activeVoucherId"] : string.Empty;
        //Session["IfReturn"] = true;


        btnReturn.Attributes.Add("onClick", "javascript:window.location = 'Projects.aspx?NodeContentType=Form&NodeSectionID=" + SectionId + "&NodeProjectUsageId=" + ProjectUsageId.ToString() + "&Active=" + activeNodeId + "&Path=" + System.Web.HttpUtility.JavaScriptStringEncode(path) + "'");

        lblPracticeName.Text = PracticeName;

        // Get questionnaire
        GetQuestionnaire();

        // To generate the summary of type
        GetProviders();

        // Get system details
        GetSystemDetails();

        // Display Price comparison
        DisplayPriceComparison();

    }

    private void GetQuestionnaire()
    {
        string receivedQuestionnaire = string.Empty;
        _questionBO = new QuestionBO();

        // Get Subscription questionnaire
        _questionBO.ProjectUsageId = ProjectUsageId;
        _questionBO.QuestionnaireId = DEFAULT_SUBSCRIPTION_QUESTIONNAIRE_ID;

        receivedQuestionnaire = _questionBO.GetQuestionnaireByTypeEHR();

        _filledQuestionnaire = XDocument.Parse(receivedQuestionnaire);

    }

    private void GetProviders()
    {
        try
        {
            // To calculate the total selection of all types
            GetProviderByType();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    private void GetProviderByType()
    {
        try
        {
            CountProviderByType(_filledQuestionnaire.Root, enQuestionnaireElements.Type); // Filled questionnaire

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    private void CountProviderByType(XElement receivedElement, enQuestionnaireElements questionnaireElement)
    {
        // Loop on each system
        int totalNoOfProviders = 0;
        foreach (XElement systemElement in receivedElement.Descendants(enQuestionnaireElements.System.ToString()))
        {
            // loop on system -> Recurring Fee (Ongoing Fee) -> where Provider exists
            foreach (XElement recurringElement in systemElement.Descendants(enQuestionnaireElements.OngoingFees.ToString()).Descendants(questionnaireElement.ToString()))
            {
                // If title is valid (Provider field title)
                string fieldTitle = recurringElement.Parent.Parent.Attribute(enQuestionnaireAttr.title.ToString()).Value;

                #region PROVIDER_CALCULATION
                foreach (string title in DEFAULT_ALLOWED_PROVIDER_TITLE)
                {
                    if (fieldTitle.Contains(title))
                    {
                        // To Check if quantity exist against current value                        
                        string[] quantity = new string[10000];

                        // Get list of Quantities
                        foreach (XElement quantityElement in recurringElement.Parent.Parent.Parent.Descendants(enQuestionnaireElements.Quantity.ToString()))
                        {
                            quantity = quantityElement.Attribute(enQuestionnaireAttr.value.ToString()).Value.Split(',');
                            break;
                        }

                        string value = recurringElement.Attribute(enQuestionnaireAttr.value.ToString()).Value;
                        if (value != string.Empty)
                        {
                            string[] multiValues = value.Split(',');
                            int count = 1;

                            foreach (string val in multiValues)
                            {
                                int finalValue = 1;

                                if (val.Trim() != string.Empty)
                                {
                                    if (quantity.Length >= count)
                                        if (quantity[count - 1].Trim() != string.Empty)
                                            finalValue = finalValue * Convert.ToInt32(quantity[count - 1]);

                                    switch (questionnaireElement)
                                    {
                                        case enQuestionnaireElements.Type:
                                            if (val == Convert.ToString((int)enEHRProviders.Physician))
                                                totalNoOfProviders += totalPhysician += finalValue;
                                            else if (val == Convert.ToString((int)enEHRProviders.MidLevel))
                                                totalNoOfProviders += totalMidLevelProvider += finalValue;
                                            else if (val == Convert.ToString((int)enEHRProviders.PartTime))
                                                totalNoOfProviders += totalPartTimeProvider += finalValue;
                                            else if (val == Convert.ToString((int)enEHRProviders.Discounted))
                                                totalNoOfProviders += totalDiscounts += finalValue;
                                            else if (val == Convert.ToString((int)enEHRProviders.Other))
                                                totalNoOfProviders += totalOthers += finalValue;
                                            break;
                                    }
                                }
                                count += 1;
                            }
                        }
                    }
                    else
                        continue;
                }

                #endregion
            }

            _totalProvidersOfSystem.Add(systemElement.Attribute(enQuestionnaireAttr.sequence.ToString()).Value, totalNoOfProviders.ToString());
            string infoToolTip = string.Empty;
            infoToolTip += enEHRProviders.Physician.ToString() + " = " + totalPhysician.ToString() + "\n ";
            infoToolTip += enEHRProviders.MidLevel.ToString() + " = " + totalMidLevelProvider.ToString() + "\n ";
            infoToolTip += enEHRProviders.PartTime.ToString() + " = " + totalPartTimeProvider.ToString() + "\n ";
            infoToolTip += enEHRProviders.Discounted.ToString() + " = " + totalDiscounts.ToString() + "\n ";
            infoToolTip += enEHRProviders.Other.ToString() + " = " + totalOthers.ToString() + "\n ";
            infoToolTip += "Total = " + (totalPhysician + totalMidLevelProvider + totalPartTimeProvider + totalDiscounts + totalOthers) + "\n ";
            _systemProviderInfo.Add(systemElement.Attribute(enQuestionnaireAttr.sequence.ToString()).Value, infoToolTip);

            // clear values for next system
            totalPhysician = totalMidLevelProvider = totalPartTimeProvider = totalDiscounts = totalOthers = 0;

            totalNoOfProviders = 0;
        }

    }

    private void GetSystemDetails()
    {
        _listOfSystemDetails.Clear();
        _listOfChartValues.Clear();

        GetAllSystemDetails(_filledQuestionnaire.Root); // Filled questionnaire

    }

    private void GetAllSystemDetails(XElement receivedElement)
    {
        systemName = string.Empty;

        // Get All System Names
        IEnumerable<XElement> systems = (from system in receivedElement.Descendants(enQuestionnaireElements.System.ToString())
                                         select system);
        foreach (XAttribute attr in systems.Attributes())
        {
            if (attr.Name == enQuestionnaireAttr.sequence.ToString())
                sequence = attr.Value.Trim();
            else if (attr.Name == enQuestionnaireAttr.name.ToString())
                systemName = attr.Value.Trim();
            else if (attr.Name == enQuestionnaireAttr.description.ToString())
            {
                // Add new row in list becuase description is last attribute of system element
                purchaseModel = attr.Value.Trim();

                // Ongoing Fee Total
                CalculateFeeTotalByFeeType(receivedElement, enQuestionnaireElements.OngoingFees, sequence);

                // One Time Fee Total
                CalculateFeeTotalByFeeType(receivedElement, enQuestionnaireElements.OneTimeFees, sequence);

                // caclulate 1-5 Year comparison
                CalculateCost();

                // Add Row
                id += 1; // primary key for table
                _listOfSystemDetails.Add(new PriceCalculationDetail(id, sequence, systemName, purchaseModel, yearOneCost, YearTwoToFive,
                    TOCYearTwo, TOCYearThree, TOCYearFour, TOCYearFive, _systemProviderInfo[sequence]));

                // Clear values
                ClearValues();

            }
            else
                continue;
        }

    }

    private void CalculateFeeTotalByFeeType(XElement receivedElement, enQuestionnaireElements questionnaireElement, string currentElementSequence)
    {
        string quantity = string.Empty;
        string curElementAmount = string.Empty;
        string curElementPaymentMethod = string.Empty;

        IEnumerable<XElement> elements = (from elememt in receivedElement.Descendants(questionnaireElement.ToString())
                                          where elememt.Parent.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == currentElementSequence
                                          select elememt);
        foreach (XElement field in elements.Elements().Elements().Elements())
        {
            if (field.Name == enQuestionnaireElements.Quantity.ToString())
                quantity = field.Attribute(enQuestionnaireAttr.value.ToString()).Value.Trim();
            else if (field.Name == enQuestionnaireElements.Amount.ToString())
                curElementAmount = field.Attribute(enQuestionnaireAttr.value.ToString()).Value;
            else if (field.Name == enQuestionnaireElements.PaymentMethod.ToString())
            {
                curElementPaymentMethod = field.Attribute(enQuestionnaireAttr.value.ToString()).Value;
                CalculateAmount(quantity, curElementAmount, curElementPaymentMethod, questionnaireElement, currentElementSequence);
                quantity = curElementAmount = curElementPaymentMethod = string.Empty;
            }
        }

    }

    private void CalculateAmount(string receivedQuantity, string receivedAmount, string receivedPaymentMethod, enQuestionnaireElements questionnaireElement, string currentElementSequence)
    {
        if (receivedAmount.Trim() != string.Empty)
        {
            string[] multiAmountVal = receivedAmount.Split(',');
            string[] multiQuantityVal = receivedQuantity.Split(',');
            string[] multiPaymentMethodValue = receivedPaymentMethod.Split(',');

            for (int index = 0; index < multiAmountVal.Length; index++)
            {
                string tempAmount = multiAmountVal[index].Trim();
                tempAmount = tempAmount != string.Empty ? tempAmount : "0";

                // Only in case of Recurring/Outgoing Fees
                string tempPaymentMethod = multiPaymentMethodValue[index].Trim();
                tempPaymentMethod = tempPaymentMethod != string.Empty ? tempPaymentMethod : "0";

                // fetch number of providers against current system sequence
                int tempNumberOfProvider = 0;
                if (_totalProvidersOfSystem.ContainsKey(currentElementSequence))
                    tempNumberOfProvider = Convert.ToInt32(_totalProvidersOfSystem[currentElementSequence]);

                // if number of provider is 0
                tempNumberOfProvider = tempNumberOfProvider != 0 ? tempNumberOfProvider : 1;

                if (tempPaymentMethod != "0")
                {
                    if (questionnaireElement == enQuestionnaireElements.OngoingFees)
                    {
                        // calculate per year cost if payment method : Per month
                        if (tempPaymentMethod == ((int)enOnGoingPaymentMethod.PerPracticePerMonth).ToString())
                            tempAmount = (Convert.ToDouble(tempAmount) * 12).ToString();
                        else if (tempPaymentMethod == ((int)enOnGoingPaymentMethod.PerProviderPerYear).ToString())
                            tempAmount = (Convert.ToDouble(tempAmount) * tempNumberOfProvider).ToString();
                        else if (tempPaymentMethod == ((int)enOnGoingPaymentMethod.PerProviderPerMonth).ToString())
                            tempAmount = ((Convert.ToDouble(tempAmount) * (tempNumberOfProvider)) * 12).ToString();
                    }
                    else if (questionnaireElement == enQuestionnaireElements.OneTimeFees)
                    {
                        if (tempPaymentMethod == ((int)enOneTimePaymentMethod.PerProvider).ToString())
                            tempAmount = (Convert.ToDouble(tempAmount) * tempNumberOfProvider).ToString();
                    }


                    if (multiAmountVal.Length == multiQuantityVal.Length && receivedQuantity.Trim() != string.Empty)
                    {
                        string tempQunatity = multiQuantityVal[index].Trim();
                        tempQunatity = tempQunatity != string.Empty ? tempQunatity : "0";

                        if (questionnaireElement == enQuestionnaireElements.OngoingFees)
                            ongoingAmount += Convert.ToDouble(Convert.ToDouble(tempAmount) * Convert.ToInt32(tempQunatity));
                        else if (questionnaireElement == enQuestionnaireElements.OneTimeFees)
                            oneTimeAmount += Convert.ToDouble(Convert.ToDouble(tempAmount) * Convert.ToInt32(tempQunatity));
                    }
                    else
                    {
                        if (questionnaireElement == enQuestionnaireElements.OngoingFees)
                            ongoingAmount += Convert.ToDouble(tempAmount);
                        else if (questionnaireElement == enQuestionnaireElements.OneTimeFees)
                            oneTimeAmount += Convert.ToDouble(tempAmount);
                    }
                }
            }
        }

    }

    private void DisplayPriceComparison()
    {


        #region GRAPH
        string param = string.Empty;
        string legend = string.Empty;

        param = "GenerateGraph('";

        _listOfSystemDetails = _listOfSystemDetails.OrderBy(column => column.SystemSequence).ToList();
        foreach (PriceCalculationDetail price in _listOfSystemDetails)
        {
            param += price.YearOne + "@" + price.TOCYearTwo + "@" + price.TOCYearThree + "@" + price.TOCYearFour + "@" + price.TOCYearFive + "@";
            legend += "" + price.SystemName + " - " + price.PurchaseModel + "@";
        }

        param = param.Substring(0, param.LastIndexOf('@'));
        legend = legend.Substring(0, legend.LastIndexOf('@'));

        // for escape character sequence
        legend = System.Web.HttpUtility.JavaScriptStringEncode(legend);

        param += "','" + legend + "');";

        // to generate the graph        
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "jsPriceComparisonGraph" + _listOfSystemDetails.Count(), param, true);

        #endregion

        gvPriceDetails.DataSource = _listOfSystemDetails;
        gvPriceDetails.DataBind();

        id = 0; // clear Id
    }

    private void CalculateCost()
    {
        // year One cost of System
        yearOneCost = ongoingAmount + oneTimeAmount;
        _listOfChartValues.Add(new PriceCalculationDetail(sequence, systemName, purchaseModel, DEFAULT_GRAPH_COLUMN1_TITLE, yearOneCost));

        // year 2-5
        YearTwoToFive = ongoingAmount;

        // Year 2
        TOCYearTwo = oneTimeAmount + (2 * ongoingAmount);
        _listOfChartValues.Add(new PriceCalculationDetail(sequence, systemName, purchaseModel, DEFAULT_GRAPH_COLUMN2_TITLE, TOCYearTwo));

        // Year 3
        TOCYearThree = oneTimeAmount + (3 * ongoingAmount);
        _listOfChartValues.Add(new PriceCalculationDetail(sequence, systemName, purchaseModel, DEFAULT_GRAPH_COLUMN3_TITLE, TOCYearThree));

        // Year 4
        TOCYearFour = oneTimeAmount + (4 * ongoingAmount);
        _listOfChartValues.Add(new PriceCalculationDetail(sequence, systemName, purchaseModel, DEFAULT_GRAPH_COLUMN4_TITLE, TOCYearFour));

        // Year 5
        TOCYearFive = oneTimeAmount + (5 * ongoingAmount);
        _listOfChartValues.Add(new PriceCalculationDetail(sequence, systemName, purchaseModel, DEFAULT_GRAPH_COLUMN5_TITLE, TOCYearFive));
    }

    private void ClearValues()
    {
        ongoingAmount = oneTimeAmount = yearOneCost = YearTwoToFive = TOCYearThree = TOCYearFour = TOCYearFive =
            totalPhysician = totalMidLevelProvider = totalPartTimeProvider = totalDiscounts = totalOthers = 0;

    }

    protected void PrintPriceComparison(bool printOnly, string name)
    {
        try
        {
            Logger.PrintInfo(DEFAULT_REPORT_TITLE + " Report generation: Process start.");

            ReportViewer viewer = new ReportViewer();

            // Set Report processing Mode
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            // Declare parameters for reports
            ReportParameter rpReportTitle = new ReportParameter("paramTitle", name);
            ReportParameter rpPracticeName = new ReportParameter("paramPracticeName", PracticeName);
            ReportParameter rpDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now).ToString());
            ReportParameter logoImage = new ReportParameter("logoImage", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter[] param = { rpReportTitle, rpPracticeName, rpDate, logoImage };

            // Create Datasource to report
            ReportDataSource rptPriceComparisonDS = new ReportDataSource("PriceComparisonDataSource", _listOfSystemDetails);
            ReportDataSource rptPriceComparisonGraphDS = new ReportDataSource("PriceComparisonGraphDataSource", _listOfChartValues);

            // Report display name which will be use on export
            viewer.LocalReport.DisplayName = DEFAULT_REPORT_TITLE;

            // Clear existing datasource
            viewer.LocalReport.DataSources.Clear();

            // Assign new Data source to report
            viewer.LocalReport.DataSources.Add(rptPriceComparisonDS);
            viewer.LocalReport.DataSources.Add(rptPriceComparisonGraphDS);

            // Report path
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptPriceComparison.rdlc";
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();

            // Assign parameter to report after assigned the data source*/
            viewer.LocalReport.SetParameters(param);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Generating PDF Report
            Logger.PrintInfo("Generating PDF report(" + DEFAULT_REPORT_TITLE + ")");
            byte[] bytes = viewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

            _printBO = new PrintBO();

            // Get file path to db
            PracticeName = PracticeName.Replace(",", "").Replace("//", "/").Replace("/", "");

            string siteNameMark = " " + "(" + PracticeName + ")";
            string path = string.Empty;

            if (printOnly)
                path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, printOnly);

            else if (!printOnly)
                path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, printOnly);

            string[] pathList = path.Split(',');
            string destinationPath = pathList[0]; /// 0= Destination path to store the pdf on server
            string savingPath = pathList[1];      /// 1= saving path to sotre the file location in database 

            // Save file on server
            Logger.PrintInfo("Report saving on " + destinationPath + ".");

            FileStream fs = new FileStream(destinationPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            Logger.PrintInfo("Report saved successfully at " + destinationPath + ".");

            // Saving location of file in db*/
            if (printOnly)
            {
                _printBO.SaveData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userId, practiceId, printOnly, DEFAULT_DOC_CONTENT_TYPE,ProjectUsageId,SectionId);
                Session["FilePath"] = savingPath;
            }
            else if (!printOnly)
            {
                _printBO.SaveData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, System.DateTime.Now, userId, practiceId, printOnly, DEFAULT_DOC_CONTENT_TYPE, ProjectUsageId, SectionId);
                Session["FilePath"] = savingPath;
            }
            Logger.PrintInfo(DEFAULT_REPORT_TITLE + " Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion
}
