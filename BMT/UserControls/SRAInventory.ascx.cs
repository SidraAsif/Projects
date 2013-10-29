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

public partial class SRAInventory : System.Web.UI.UserControl
{
    #region PROPERTIES

    public int ProjectUsageId { get; set; }
    public int PracticeId { get; set; }
    public int SiteId { get; set; }
    public int SectionId { get; set; }

    #endregion

    #region Variables
    private SRAInventoryDetails _sraInventoryDetails;
    private XDocument questionnaire;
    private XDocument storedQuestionnaire;

    private int userApplicationId;
    private string userType;
    private int practiceId;

    private List<AssetDetails> _assetTypeList;

    #endregion

    #region CONSTANT
    private const int DEFAULT_TOTAL_COLUMNS = 4;
    private const string DEFAULT_HEADER_PARENT_POS1 = "Asset Type";
    private const string DEFAULT_HEADER_PARENT_POS2 = "Asset Description";
    private const string DEFAULT_HEADER_PARENT_POS3 = "Practice Notes";
    private const string DEFAULT_HEADER_PARENT_POS4 = "Review Notes";

    private const string DEFAULT_ELEMENT_TABLE_ID_PREFIX = "tableElement";
    private const string DEFAULT_ELEMENT_TABLE_CSS_CLASS = "element-table";

    private const string DEFAULT_ASSET_DESC_LABEL_ID_PREFIX = "lblAssetDesc";
    private const string DEFAULT_PRACTICE_NOTES_LABEL_ID_PREFIX = "lblPracticeNotes";
    private const string DEFAULT_REVIEW_NOTES_LABEL_ID_PREFIX = "lblReviewNotes";

    private const string DEFAULT_ASSET_DESC_TEXTBOX_ID_PREFIX = "txtboxAssetDesc";
    private const string DEFAULT_PRACTICE_NOTES_TEXTBOX_ID_PREFIX = "txtboxPracticeNotes";
    private const string DEFAULT_REVIEW_NOTES_TEXTBOX_ID_PREFIX = "txtboxReviewNotes";

    private const string DEFAULT_ASSET_TYPE_DDL_ID_PREFIX = "ddlAssetType";
    private const string DEFAULT_ROW_COUNT_HIDDEN_ID_PREFIX = "hdnRowCount";
    private const string DEFAULT_HIDDEN_FIELD_PER_ELEMENT_ID_PREFIX = "hdnFieldElement";

    private const string DEFAULT_DOC_CONTENT_TYPE = "SRACopies";
    private const string DEFAULT_REPORT_TITLE = "Assets Inventory";

    #endregion

    #region CONTROLS
    private Table inventoryTable;
    private Table elementTable;
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

    protected void btnPrintInventory_Click(object sender, EventArgs e)
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
                pnlInventory.Enabled = !isLock;

                pnlInventory.Controls.Clear();

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(SiteId);

                lblSiteName.Text = siteBO.Name;

                inventoryTable = new Table();
                inventoryTable.ID = "inventoryTable";
                inventoryTable.ClientIDMode = ClientIDMode.Static;
                pnlInventory.Controls.Add(inventoryTable);

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
                        _tableCell.Width = 130;
                        break;

                    case 2:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS2;
                        _tableCell.Width = 290;
                        break;

                    case 3:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS3;
                        _tableCell.Width = 160;
                        break;

                    case 4:
                        _tableCell.Text = DEFAULT_HEADER_PARENT_POS4;
                        _tableCell.Width = 160;
                        break;
                    default:
                        break;
                }

                _tableRow.Controls.Add(_tableCell);

            }
            inventoryTable.Controls.Add(_tableRow);
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
            foreach (XElement element in questionnaire.Root.Elements("Inventory").Elements("Element"))
            {
                AddElementsGroup(element);
                AddElementsTable(element);
                AddElementsValues(element);

                TableRow _tableRow = new TableRow();
                TableCell _tableCell = new TableCell();

                _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
                _tableCell.Height = 10;

                _tableRow.Controls.Add(_tableCell);
                inventoryTable.Controls.Add(_tableRow);
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
            Label _label;
            Label _whiteSpaceLabel;
            Image _image;
            string elementTitle = Element.Attribute("title").Value;
            string elementSequence = Element.Attribute("sequence").Value;

            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:SRAtoggle('" + elementSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgElement" + elementSequence;
            _image.ImageUrl = "../Themes/Images/Plus-1.png";
            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "&nbsp; &nbsp;";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = elementSequence + ". " + elementTitle;
            _label.Width = 700;
            _label.CssClass = "element-label";
            _tableCell.Controls.Add(_label);

            _tableCell.ColumnSpan = DEFAULT_TOTAL_COLUMNS;
            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);
            inventoryTable.Controls.Add(_tableRow);

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
            inventoryTable.Controls.Add(_tableRow);

            AddStandards(Element);
            AddGenericControls(Element);

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
            int counter = 1;
            string id = string.Empty;
            string elementSequence = Element.Attribute("sequence").Value;

            int rowCount = Element.Elements("Standard").Count();
            int multipleOfTen = rowCount / 10;

            // Hidden Field Row Count

            HiddenField _hdnField = new HiddenField();
            _hdnField.ID = DEFAULT_ROW_COUNT_HIDDEN_ID_PREFIX + elementSequence;
            _hdnField.ClientIDMode = ClientIDMode.Static;

            TableCell _tableCell = new TableCell();
            _tableCell.Controls.Add(_hdnField);

            TableRow _tableRow = new TableRow();
            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);


            //Hidden Field Per Element

            _hdnField = new HiddenField();
            _hdnField.ID = DEFAULT_HIDDEN_FIELD_PER_ELEMENT_ID_PREFIX + elementSequence;
            _hdnField.ClientIDMode = ClientIDMode.Static;

            _tableCell = new TableCell();
            _tableCell.Controls.Add(_hdnField);

            _tableRow = new TableRow();
            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);


            //Add Standard Rows

            foreach (XElement standard in Element.Elements("Standard"))
            {
                id = elementSequence + standard.Attribute("sequence").Value;
                AddStandardRow(id, elementSequence);

                counter++;
            }


            for (int index = counter; index <= (multipleOfTen + 1) * 10; index++)
            {
                id = elementSequence + index;
                AddStandardRow(id, elementSequence);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(string id, string elementSequence)
    {
        try
        {
            TableRow _tableRow = new TableRow();
            int elementId = Convert.ToInt32(elementSequence);
            Label _label;
            DropDownList _dropDownList;
            TextBox _textbox;
            // Asset Type
            QuestionBO _questionBO = new QuestionBO();
            _assetTypeList = _questionBO.GetAssetTypes(elementId);

            _dropDownList = new DropDownList();
            _dropDownList.ID = DEFAULT_ASSET_TYPE_DDL_ID_PREFIX + id;
            _dropDownList.Attributes.Add("onchange", "javascript:onAssetTypeChange('" + id + "');");
            _dropDownList.ClientIDMode = ClientIDMode.Static;
            _dropDownList.CssClass = "assettype";
            _dropDownList.Width = 122;

            _dropDownList.DataSource = _assetTypeList;
            _dropDownList.DataTextField = "Name";
            _dropDownList.DataValueField = "ValueKey";
            _dropDownList.DataBind();

            _dropDownList.Items.Insert(0, new ListItem("Select", "-1"));

            foreach (ListItem item in _dropDownList.Items)
            {
                if (item.Value.ToString() == "0")
                {
                    //item.Attributes.Add("class", "asset-Parent");
                    item.Attributes.Add("Disabled", "true");
                }
                else if (item.Value.ToString() != "-1")
                {
                    item.Text = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + item.Text;
                }
            }

            TableCell _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_dropDownList);
            _tableRow.Controls.Add(_tableCell);


            //Asset Description

            _label = new Label();
            _label.ID = DEFAULT_ASSET_DESC_LABEL_ID_PREFIX + id;
            _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_ASSET_DESC_TEXTBOX_ID_PREFIX + id + "');");
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "assetdesc";
            _label.Width = 280;
            _label.ToolTip = "Click here to add Asset Description";
            _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);


            _textbox = new TextBox();
            _textbox.CssClass = "assetdesc";
            _textbox.ID = DEFAULT_ASSET_DESC_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 275;
            _textbox.Height = 14;

            _tableCell.Controls.Add(_textbox);
            _tableRow.Controls.Add(_tableCell);

            //Practice Notes

            _label = new Label();
            _label.ID = DEFAULT_PRACTICE_NOTES_LABEL_ID_PREFIX + id;
            _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_PRACTICE_NOTES_TEXTBOX_ID_PREFIX + id + "');");
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "practicenotes";
            _label.Width = 150;
            _label.ToolTip = "Click here to add Practice Notes";
            _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            _textbox = new TextBox();
            _textbox.CssClass = "practicenotes";
            _textbox.ID = DEFAULT_PRACTICE_NOTES_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 145;
            _textbox.Height = 14;

            _tableCell.Controls.Add(_textbox);
            _tableRow.Controls.Add(_tableCell);

            //Review Notes

            _label = new Label();
            _label.ID = DEFAULT_REVIEW_NOTES_LABEL_ID_PREFIX + id;
            _label.Attributes.Add("onclick", "javascript:editLabel('" + _label.ID + "','" + DEFAULT_REVIEW_NOTES_TEXTBOX_ID_PREFIX + id + "');");
            _label.ClientIDMode = ClientIDMode.Static;
            _label.CssClass = "reviewnotes";
            _label.Width = 150;
            _label.ToolTip = "Click here to add Review Notes";
            _label.BackColor = System.Drawing.Color.FromArgb(191, 207, 226);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.Controls.Add(_label);

            _textbox = new TextBox();
            _textbox.CssClass = "reviewnotes";
            _textbox.ID = DEFAULT_REVIEW_NOTES_TEXTBOX_ID_PREFIX + id;
            _textbox.Attributes.Add("onblur", "javascript:setLabel('" + _label.ID + "','" + _textbox.ID + "', '" + _textbox.CssClass + "');");
            _textbox.Style.Add("display", "none");
            _textbox.ClientIDMode = ClientIDMode.Static;
            _textbox.Width = 145;
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
            foreach (XElement element in questionnaire.Root.Elements("Inventory").Elements("Element"))
            {
                string id = string.Empty;
                string elementSequence = Element.Attribute("sequence").Value;

                int rowCount = Element.Elements("Standard").Count();
                int multipleOfTen = rowCount / 10;
                int counter = 1;
                Label _label;
                DropDownList _dropDownList;
                TextBox _textbox;
                // Hidden Field Row Count Value

                HiddenField _hdnField = (HiddenField)pnlInventory.FindControl(DEFAULT_ROW_COUNT_HIDDEN_ID_PREFIX + elementSequence);
                _hdnField.Value = ((multipleOfTen + 1) * 10).ToString() + "," + ((multipleOfTen + 1) * 10).ToString();

                //Standard Row Values

                foreach (XElement standard in Element.Elements("Standard"))
                {
                    id = elementSequence + standard.Attribute("sequence").Value;

                    //Asset Type

                    _dropDownList = (DropDownList)pnlInventory.FindControl(DEFAULT_ASSET_TYPE_DDL_ID_PREFIX + id);
                    _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + standard.Attribute("AssetType").Value));
                    ListItem listItem = _dropDownList.SelectedItem;
                    listItem.Text = listItem.Text.Trim();

                    _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByText(standard.Attribute("AssetType").Value));
                    

                    //Asset Description

                    _label = (Label)pnlInventory.FindControl(DEFAULT_ASSET_DESC_LABEL_ID_PREFIX + id);
                    _label.Text = standard.Attribute("AssetDescription").Value;

                    _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_ASSET_DESC_TEXTBOX_ID_PREFIX + id);
                    _textbox.Text = standard.Attribute("AssetDescription").Value;

                    //Practice Notes

                    _label = (Label)pnlInventory.FindControl(DEFAULT_PRACTICE_NOTES_LABEL_ID_PREFIX + id);
                    _label.Text = standard.Attribute("PracticeNotes").Value;

                    _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_PRACTICE_NOTES_TEXTBOX_ID_PREFIX + id);
                    _textbox.Text = standard.Attribute("PracticeNotes").Value;

                    //Review Notes

                    _label = (Label)pnlInventory.FindControl(DEFAULT_REVIEW_NOTES_LABEL_ID_PREFIX + id);
                    _label.Text = standard.Attribute("ReviewNotes").Value;

                    _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_REVIEW_NOTES_TEXTBOX_ID_PREFIX + id);
                    _textbox.Text = standard.Attribute("ReviewNotes").Value;

                    counter++;
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddGenericControls(XElement element)
    {
        try
        {
            string elementSequence = element.Attribute("sequence").Value;

            HyperLink addMoreHyperLink = new HyperLink();
            addMoreHyperLink.ID = "hypLinkAddMore" + elementSequence;
            addMoreHyperLink.Text = "+ Add 10 More";
            addMoreHyperLink.NavigateUrl = "javascript:GenerateInventoryRows(" + elementSequence + ");";
            addMoreHyperLink.ClientIDMode = ClientIDMode.Static;

            TableCell _tableCell = new TableCell();
            _tableCell.Controls.Add(addMoreHyperLink);

            TableRow _tableRow = new TableRow();
            _tableRow.Controls.Add(_tableCell);

            elementTable.Controls.Add(_tableRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    public void SaveInventory()
    {
        try
        {
            if (Page.IsPostBack)
            {
                int elementPosition = 0;

                if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
                {
                    string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                    storedQuestionnaire = XDocument.Parse(RecievedQuestionnaire);

                    GenerateControlsIds();

                    foreach (XElement element in storedQuestionnaire.Root.Elements("Inventory").Elements("Element"))
                    {
                        //Remove All Standard Before Saving

                        elementPosition = Convert.ToInt32(element.Attribute("sequence").Value) - 1;

                        if (storedQuestionnaire.Root.Elements("Inventory").Elements("Element").ElementAt(elementPosition).Elements("Standard").Count() > 0)
                            storedQuestionnaire.Root.Elements("Inventory").Elements("Element").ElementAt(elementPosition).Elements("Standard").Remove();

                        SaveSRAInventory(element);
                    }

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

    protected void GenerateControlsIds()
    {
        try
        {
            inventoryTable = new Table();
            inventoryTable.ID = "inventoryTable";
            inventoryTable.ClientIDMode = ClientIDMode.Static;
            pnlInventory.Controls.Add(inventoryTable);

            foreach (XElement element in storedQuestionnaire.Root.Elements("Inventory").Elements("Element"))
            {
                AddElementsTable(element);
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void SaveSRAInventory(XElement element)
    {
        try
        {
            int seqCounter = 1;
            int genericFirstIndex = 0;
            int genericLastIndex = 0;
            DropDownList _dropDownList;
            TextBox _textbox;
            string[] hdnRowCount;
            string[] inventoryRowObjects;

            string id = string.Empty;
            string elementSequence = element.Attribute("sequence").Value;
            string title = element.Attribute("title").Value;


            string _assetType = string.Empty;
            string _assetDesc = string.Empty;
            string _practiceNotes = string.Empty;
            string _reviewNotes = string.Empty;

            HiddenField _hdnField = (HiddenField)pnlInventory.FindControl(DEFAULT_ROW_COUNT_HIDDEN_ID_PREFIX + elementSequence);
            SetFormValue(_hdnField);

            hdnRowCount = _hdnField.Value.Split(',');
            genericFirstIndex = Convert.ToInt32(hdnRowCount[0]) + 1;
            genericLastIndex = Convert.ToInt32(hdnRowCount[1]);


            for (int index = 1; index <= genericFirstIndex - 1; index++)
            {
                id = elementSequence + index;

                // Asset Type                
                _dropDownList = (DropDownList)pnlInventory.FindControl(DEFAULT_ASSET_TYPE_DDL_ID_PREFIX + id);
                SetFormValue(_dropDownList);
                _assetType = _dropDownList.SelectedItem.ToString();

                //Asset Description                
                _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_ASSET_DESC_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                _assetDesc = _textbox.Text;

                //Practice Notes                
                _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_PRACTICE_NOTES_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                _practiceNotes = _textbox.Text;

                //Review Notes                
                _textbox = (TextBox)pnlInventory.FindControl(DEFAULT_REVIEW_NOTES_TEXTBOX_ID_PREFIX + id);
                SetFormValue(_textbox);
                _reviewNotes = _textbox.Text;

                if (_assetType != "Select")
                {
                    SaveStandardRow(seqCounter.ToString(), _assetType, _assetDesc, _practiceNotes, _reviewNotes, title, Convert.ToInt32(elementSequence) - 1);
                    seqCounter++;
                }
            }

            if (genericLastIndex >= genericFirstIndex)
            {
                _hdnField = (HiddenField)pnlInventory.FindControl(DEFAULT_HIDDEN_FIELD_PER_ELEMENT_ID_PREFIX + elementSequence);
                SetFormValue(_hdnField);

                if (_hdnField.Value != string.Empty)
                {
                    foreach (string inventoryRow in _hdnField.Value.Split('|'))
                    {
                        if (inventoryRow != string.Empty)
                        {
                            inventoryRowObjects = inventoryRow.Split(',');
                            _assetType = inventoryRowObjects[0];
                            _assetDesc = inventoryRowObjects[1];
                            _practiceNotes = inventoryRowObjects[2];
                            _reviewNotes = inventoryRowObjects[3];

                            if (_assetType != "Select")
                            {
                                SaveStandardRow(seqCounter.ToString(), _assetType, _assetDesc, _practiceNotes, _reviewNotes, title, Convert.ToInt32(elementSequence) - 1);
                                seqCounter++;
                            }
                        }

                    }
                }
            }

        }


        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void SaveStandardRow(string sequence, string assetType, string assetDesc, string practiceNotes, string reviewNotes, string title, int elementPosition)
    {
        try
        {
            XElement _standard = new XElement("Standard");
            storedQuestionnaire.Root.Elements("Inventory").Elements("Element").ElementAt(elementPosition).Add(_standard);

            XAttribute sequenceAttr = new XAttribute("sequence", sequence);
            XAttribute assetTypeAttr = new XAttribute("AssetType", assetType.Trim());
            XAttribute assetDescAttr = new XAttribute("AssetDescription", assetDesc);
            XAttribute practiceNotesAttr = new XAttribute("PracticeNotes", practiceNotes);
            XAttribute reviewNotesAttr = new XAttribute("ReviewNotes", reviewNotes);

            storedQuestionnaire.Root.Elements("Inventory").Elements("Element").ElementAt(elementPosition).Elements("Standard").Last().Add(sequenceAttr, assetTypeAttr, assetDescAttr, practiceNotesAttr, reviewNotesAttr);

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


    public void GenerateReport(bool isSRACopy)
    {
        try
        {
            List<SRAInventoryDetails> lstSRAInventoryDetails = GenerateInventoryList();

            Logger.PrintInfo("Assets Inventory Report generation: Process start.");

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

            ReportParameter[] param = { paramSiteName, paramDate, paramLogo , paramUserDetails };

            // Create Datasource to report
            Logger.PrintDebug("Set data source to report.");

            ReportDataSource rptDataSource = new ReportDataSource("SRAInventoryDS", lstSRAInventoryDetails);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rptDataSource);

            viewer.LocalReport.DisplayName = DEFAULT_REPORT_TITLE;
            viewer.LocalReport.ReportPath = Server.MapPath("~/") + "Reports/rptSRAInventory.rdlc";
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
                path = Util.GetTempPdfPath(PracticeId, "1");

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "print();", true);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }


    private List<SRAInventoryDetails> GenerateInventoryList()
    {
        try
        {
            List<SRAInventoryDetails> lstSRAInventoryDetails = new List<SRAInventoryDetails>();
            if (Session[enSessionKey.SRAQuestionnaire.ToString()] != null)
            {
                string RecievedQuestionnaire = Session[enSessionKey.SRAQuestionnaire.ToString()].ToString();
                questionnaire = XDocument.Parse(RecievedQuestionnaire);

                foreach (XElement element in questionnaire.Root.Elements("Inventory").Elements("Element"))
                {
                    foreach (XElement standard in element.Elements("Standard"))
                    {
                        _sraInventoryDetails = new SRAInventoryDetails();
                        _sraInventoryDetails.AssetCategoryId = Convert.ToInt32(element.Attribute("sequence").Value);
                        _sraInventoryDetails.AssetCategory = element.Attribute("title").Value;
                        _sraInventoryDetails.AssetType = standard.Attribute("AssetType").Value;
                        _sraInventoryDetails.AssetDescription = standard.Attribute("AssetDescription").Value;
                        _sraInventoryDetails.PracticeNotes = standard.Attribute("PracticeNotes").Value;
                        _sraInventoryDetails.ReviewNotes = standard.Attribute("ReviewNotes").Value;

                        lstSRAInventoryDetails.Add(_sraInventoryDetails);
                    }
                }
            }
            return lstSRAInventoryDetails;

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    #endregion

}
