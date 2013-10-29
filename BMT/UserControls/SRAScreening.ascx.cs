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

public partial class SRAScreening : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public int SiteId { get; set; }
    public int SectionId { get; set; }
    #endregion

    #region Variables
    private SRAScreeningQuestions _sraScreeningQuestion;

    private XDocument questionnaire;
    private XDocument storedQuestionnaire;

    private int userApplicationId;
    private string userType;
    private int practiceId;

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 6;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Topic";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Questions";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Response";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Threat Vulnerability";
    private const string DEFAULT_HEADER_PARENT_POS5 = "People/Process";
    private const string DEFAULT_HEADER_PARENT_POS6 = "Technology";

    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "tableElement";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "element-table";
    private const string DEFAULT_RESPONSE_DROPDOWN_ID_PREFIX = "ddlResponse";

    private const string DEFAULT_PEOPLE_PROCESS_LABEL_ID_PREFIX = "lblPeopleOrProcess";
    private const string DEFAULT_PEOPLE_PROCESS_TEXTBOX_ID_PREFIX = "txtboxPeopleOrProcess";

    private const string DEFAULT_TECHNOLOGY_LABEL_ID_PREFIX = "lblTechnology";
    private const string DEFAULT_TECHNOLOGY_TEXTBOX_ID_PREFIX = "txtboxTechnology";

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "Screening Questions";

    private readonly string[] DEFAULT_RESPONSE_OPTIONS = { "Addressed", "Partially Addressed", "Not Addressed" };

    #endregion

    #region CONTROLS
    private Table screeningTable;
    private  Table elementTable;
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

    protected void btnPrintScreening_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateReport(false);
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
                pnlScreening.Enabled = !isLock;

                pnlScreening.Controls.Clear();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                lblSiteName.Text = siteBO.Name;

                screeningTable = new Table();
                screeningTable.ID = "screeningTable";
                screeningTable.ClientIDMode = ClientIDMode.Static;
                pnlScreening.Controls.Add(screeningTable);

                LoadHeader();
                GenerateQuestionaire();

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
            TableRow _tableRow = new TableRow();
            _tableRow.ClientIDMode = ClientIDMode.Static;

            for (int ColumnIndex = 1; ColumnIndex <= DEFAULT_TOTAL_COLUMNS; ColumnIndex++)
            {
                TableCell _tableCell = new TableCell();
                _tableCell.ID = "headerCell" + Convert.ToString(ColumnIndex);
                _tableCell.CssClass = "header";
                _tableCell.ClientIDMode = ClientIDMode.Static;

                switch (ColumnIndex)
                {
                    case 1:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS1;
                        _tableCell.Width = 120;
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Width = 170;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Width = 110;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Width = 115;
                        break;

                    case 5:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS5;
                        _tableCell.Width = 100;
                        break;

                    case 6:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS6;
                        _tableCell.Width = 100;
                        break;
                    default:
                        break;
                }

                _tableRow.Controls.Add(_tableCell);
            }

            screeningTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void GenerateQuestionaire()
    {
        try
        {
            foreach (XElement element in questionnaire.Root.Elements("Screening").Elements("Element"))
            {
                AddElementsGroup(element);
                AddElementsTable(element);
                AddElementsValues(element);

                TableRow _tableRow = new TableRow();
                TableCell _tableCell = new TableCell();

                _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
                _tableCell.Height = 10;

                _tableRow.Controls.Add(_tableCell);
                screeningTable.Controls.Add(_tableRow);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void AddElementsGroup(XElement Element)
    {
        try
        {
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();

            string elementTitle = Element.Attribute("title").Value;
            string elementSequence = Element.Attribute("sequence").Value;

            Image _image = new Image();
            _image.Attributes.Add("onclick", "javascript:SRAtoggle('" + elementSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgElement" + elementSequence;
            _image.ImageUrl = "../Themes/Images/Plus-1.png";
            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            Label _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp; &nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            Label _label = new Label();
            _label.Text = elementSequence + ". " + elementTitle;
            _label.Width = 700;
            _label.CssClass = "element-label";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            screeningTable.Controls.Add(_tableRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsTable(XElement Element)
    {
        try
        {
            string elementSequence = Element.Attribute("sequence").Value;

            elementTable = new Table();
            TableRow _tableRow = new TableRow();
            TableCell _tableCell = new TableCell();

            elementTable.ID = DEFAULT_ELEMENT_TABLE_ID_PREFIX + elementSequence;
            elementTable.ClientIDMode = ClientIDMode.Static;
            elementTable.CssClass = DEFAULT_ELEMENT_TABLE_CSS_CLASS;
            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;

            _tableCell.Controls.Add(elementTable);
            _tableRow.Controls.Add(_tableCell);
            screeningTable.Controls.Add(_tableRow);

            AddStandards(Element);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandards(XElement Element)
    {
        try
        {
            foreach (XElement standard in Element.Elements("Standard"))
            {
                AddStandardRow(Element.Attribute("sequence").Value, standard);
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(string elementId, XElement standard)
    {
        try
        {
            string standardId = standard.Attribute("sequence").Value;
            string topic = standard.Attribute("Topic").Value;

            string questions = standard.Attribute("Questions").Value;
            string threatVulnerability = standard.Attribute("ThreatVulnerability").Value;
            bool isNotApplicable = Convert.ToBoolean(standard.Attribute("IsNotApplicable").Value);

            TableRow _tableRow = new TableRow();
            string id = elementId + standardId;

            //Topic

            Label _label = new Label();
            _label.ID = "lblTopic" + id;
            _label.Text = elementId + "." + standardId + " " + topic;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "screen-topic";
            _label.Width = 115;

            TableCell _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            //Questions

            _label = new Label();
            _label.ID = "lblQuestions" + id;
            _label.Text = questions;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "screen-questions";
            _label.Width = 165;

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            //Response

            DropDownList _dropDownList = new DropDownList();
            _dropDownList.ID = DEFAULT_RESPONSE_DROPDOWN_ID_PREFIX + id;
            _dropDownList.ClientIDMode = ClientIDMode.Static;
            _dropDownList.CssClass = "screen-response";
            _dropDownList.Width = 105;

            _dropDownList.Items.Add(new ListItem("Select", "0"));

            int position = 1;
            foreach (string responseItem in DEFAULT_RESPONSE_OPTIONS)
            {
                _dropDownList.Items.Add(new ListItem(responseItem, position++.ToString()));
            }

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_dropDownList);
            _tableRow.Controls.Add(_tableCell);

            //Threat Vulnerability

            _label = new Label();
            _label.ID = "lblThreatVulnerability" + id;
            _label.Text = threatVulnerability;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "screen-threat";
            _label.Width = 116;

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);
            _tableRow.Controls.Add(_tableCell);

            //People/Process

            _label = new Label();
            _label.ID = DEFAULT_PEOPLE_PROCESS_LABEL_ID_PREFIX + id;
            _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_PEOPLE_PROCESS_TEXTBOX_ID_PREFIX + id + "');");
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "screen-people";
            _label.Width = 97;
            _label.ToolTip = "Click here to add People/Process";
            _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            TextBox _textbox = new TextBox();
            _textbox.CssClass = "screen-people";
            _textbox.ID = DEFAULT_PEOPLE_PROCESS_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 92;
            _textbox.Height = 14;

            _tableCell.Controls.Add(_textbox);
            _tableRow.Controls.Add(_tableCell);

            //Technology

            _label = new Label();
            _label.ID = DEFAULT_TECHNOLOGY_LABEL_ID_PREFIX + id;
            _label.ClientIDMode = ClientIDMode.Static;
            _label.Width = 97;

            if (isNotApplicable)
            {
                _label.CssClass = "screen-technology-disable";
            }
            else
            {
                _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_TECHNOLOGY_TEXTBOX_ID_PREFIX + id + "');");
                _label.CssClass = "screen-technology";
                _label.ToolTip = "Click here to add Technology";
                _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);
            }

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            _textbox = new TextBox();
            _textbox.CssClass = "screen-technology";
            _textbox.ID = DEFAULT_TECHNOLOGY_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 92;
            _textbox.Height = 14;

            _tableCell.Controls.Add(_textbox);
            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsValues(XElement Element)
    {
        try
        {
            foreach (XElement standard in Element.Elements("Standard"))
            {

                string elementId = Element.Attribute("sequence").Value;
                string standardId = standard.Attribute("sequence").Value;

                string id = elementId + standardId;
                bool isNotApplicable = Convert.ToBoolean(standard.Attribute("IsNotApplicable").Value);

                //Response

                DropDownList _dropDownList = (DropDownList)pnlScreening.FindControl(DEFAULT_RESPONSE_DROPDOWN_ID_PREFIX + id);
                _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(standard.Attribute("Response").Value));


                //People/Process

                Label _label = (Label)pnlScreening.FindControl(DEFAULT_PEOPLE_PROCESS_LABEL_ID_PREFIX + id);
                _label.Text = standard.Attribute("PeopleOrProcess").Value;

                TextBox _textbox = (TextBox)pnlScreening.FindControl(DEFAULT_PEOPLE_PROCESS_TEXTBOX_ID_PREFIX + id);
                _textbox.Text = standard.Attribute("PeopleOrProcess").Value;


                //Technology

                _label = (Label)pnlScreening.FindControl(DEFAULT_TECHNOLOGY_LABEL_ID_PREFIX + id);
                if (isNotApplicable)
                    _label.Text = "N/A";
                else
                    _label.Text = standard.Attribute("Technology").Value;

                _textbox = (TextBox)pnlScreening.FindControl(DEFAULT_TECHNOLOGY_TEXTBOX_ID_PREFIX + id);
                _textbox.Text = standard.Attribute("Technology").Value;
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void SaveScreening()
    {
        try
        {
            if (Page.IsPostBack)
            {
                if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
                {
                    string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                    storedQuestionnaire = XDocument.Parse(RecievedQuestionnaire);

                    GenerateControlsIds();

                    foreach (XElement element in storedQuestionnaire.Root.Elements("Screening").Elements("Element"))
                    {
                        SaveSRAScreening(element);
                    }

                    QuestionBO _questionBO = new QuestionBO();
                    _questionBO.SaveFilledQuestionnaire((int)enQuestionnaireType.SRAQuestionnaire, ProjectUsageId, SiteId,storedQuestionnaire.Root, Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void GenerateControlsIds()
    {
        try
        {
            screeningTable = new Table();
            screeningTable.ID = "screeningTable";
            screeningTable.ClientIDMode = ClientIDMode.Static;
            pnlScreening.Controls.Add(screeningTable);

            foreach (XElement element in storedQuestionnaire.Root.Elements("Screening").Elements("Element"))
            {
                AddElementsTable(element);
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void SaveSRAScreening(XElement element)
    {
        try
        {
            string elementSequence = element.Attribute("sequence").Value;

            foreach (XElement standard in element.Elements("Standard"))
            {
                string standardId = standard.Attribute("sequence").Value;
                string id = elementSequence + standardId;

                //Saving SRA Questionaire

                //Response
                DropDownList _dropDownList = (DropDownList)pnlScreening.FindControl(DEFAULT_RESPONSE_DROPDOWN_ID_PREFIX + id);
                SetFormValue(_dropDownList);
                standard.Attribute("Response").Value = _dropDownList.SelectedItem.Text != "Select" ? _dropDownList.SelectedItem.Text : string.Empty;

                //PeopleOrProcess
                TextBox _textbox = (TextBox)pnlScreening.FindControl(DEFAULT_PEOPLE_PROCESS_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                standard.Attribute("PeopleOrProcess").Value = _textbox.Text;


                //Technology
                _textbox = (TextBox)pnlScreening.FindControl(DEFAULT_TECHNOLOGY_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                standard.Attribute("Technology").Value = _textbox.Text;
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private void SetFormValue(Control control)
    {
        if (!Request.Form.HasKeys())
            return;

        string value = Request.Form[control.UniqueID];
        if (value != null)
        {
            if (control is DropDownList)
                (control as DropDownList).SelectedValue = value;

            if (control is TextBox)
                (control as TextBox).Text = value;

            if (control is HiddenField)
                (control as HiddenField).Value = value;
        }
    }

    private List<SRAScreeningQuestions> GenerateScreeningList()
    {
        try
        {
            List<SRAScreeningQuestions> lstSRAScreeningQuestions = new List<SRAScreeningQuestions>();
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                foreach (XElement Element in questionnaire.Root.Elements("Screening").Elements("Element"))
                {
                    foreach (XElement standard in Element.Elements("Standard"))
                    {
                        string elementId = Element.Attribute("sequence").Value;
                        string standardId = standard.Attribute("sequence").Value;
                        bool isNotApplicable = Convert.ToBoolean(standard.Attribute("IsNotApplicable").Value);

                        _sraScreeningQuestion = new SRAScreeningQuestions();
                        _sraScreeningQuestion.ElementId = Convert.ToInt32(elementId);
                        _sraScreeningQuestion.ElementName = Element.Attribute("title").Value;

                        _sraScreeningQuestion.Topic = elementId + "." + standardId + " " + standard.Attribute("Topic").Value;
                        _sraScreeningQuestion.Questions = standard.Attribute("Questions").Value;
                        _sraScreeningQuestion.Response = standard.Attribute("Response").Value;
                        _sraScreeningQuestion.ThreatVulnerability = standard.Attribute("ThreatVulnerability").Value;
                        _sraScreeningQuestion.PeopleOrProcess = standard.Attribute("PeopleOrProcess").Value;

                        if (isNotApplicable)
                            _sraScreeningQuestion.Technology = "N/A";
                        else
                            _sraScreeningQuestion.Technology = standard.Attribute("Technology").Value;

                        lstSRAScreeningQuestions.Add(_sraScreeningQuestion);

                    }
                }
            }
            return lstSRAScreeningQuestions;

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void GenerateReport(bool isSRACopy)
    {
        try
        {
            List<SRAScreeningQuestions> lstSRAScreeningQuestions = GenerateScreeningList();

            Logger.PrintInfo("SRA Screening Report generation: Process start.");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            Logger.PrintDebug("Declaring parameters to report.");

            SiteBO siteBO = new SiteBO();
            siteBO.GetSiteBySiteId(SiteId);
            string siteName = siteBO.Name;

            UserAccountBO userAccountBO = new UserAccountBO();
            List<string> userDetails = userAccountBO.GetUserInformation(Convert.ToInt32(Session["UserApplicationId"]));

            ReportParameter paramSiteName = new ReportParameter("paramSiteName", siteName);
            //ReportParameter paramDate = new ReportParameter("paramDate", String.Format("{0:M/d/yyyy}", System.DateTime.Now));
            ReportParameter paramDate = new ReportParameter("paramDate", System.DateTime.Now.ToString());
            ReportParameter paramLogo = new ReportParameter("paramLogo", @"file:///" + Server.MapPath("~/") + "Themes\\Images\\logo-bizmed.png");
            ReportParameter paramUserDetails = new ReportParameter("paramUserDetails", userDetails[2] + " " + userDetails[3]);

            ReportParameter[] param = { paramSiteName, paramDate, paramLogo,paramUserDetails };

            // Create Datasource to report
            Logger.PrintDebug("Set data source to report.");

            ReportDataSource rptDataSource = new ReportDataSource("SRAScreeningDS", lstSRAScreeningQuestions);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rptDataSource);

            viewer.LocalReport.DisplayName = "Screening Questions";
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptSRAScreening.rdlc";
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
            string path = Util.GetPdfPath(practiceId, DEFAULT_REPORT_TITLE + siteNameMark, false);

            if (isSRACopy)
                path = Util.GetTempPdfPath(PracticeId, "2");

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
                print.SaveSRAData(DEFAULT_REPORT_TITLE + siteNameMark, savingPath, userApplicationId, practiceId, DEFAULT_DOC_CONTENT_TYPE,SectionId,ProjectUsageId);
                Session["FilePath"] = savingPath;
            }


            Logger.PrintInfo("SRA Screening Report generation: Process end.");
            ScriptManager.RegisterStartupScript(this,this.GetType(), "click", "print();", true);
            
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion
}



