using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class CreateEditTemplate : System.Web.UI.UserControl
{
    #region PPROPERTIES
    public int PracticeId { get; set; }
    public string UserType { get; set; }
    public int TemplateId { get; set; }
    public string IsCreate
    {
        get
        {
            return hdnIsCreate.Value;
        }
        set
        {
            hdnIsCreate.Value = value;
        }
    }
    #endregion

    #region CONSTANTS
    private char[] DELIMITATORS = new char[] { ',' };
    #endregion

    #region VARIABLES
    private ProjectTemplateBO _template = new ProjectTemplateBO();
    private int templateId;
    private int tempCatCount;
    private int tempNameCount;

    private string[] existingTempName;
    private string existingTemplateName;
    private string templateName;
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            MOReMessage.Clear("");

            ClearTemplateFields();

            GetTemplateType();

            GetTemplateCategory();

            GetAllowAccess();

            GetSubmittedList();

            GetTemplate();

            GetStandardFolder();

            GetToolLevel();

            hdnUserType.Value = UserType;


        }
        TemplatePopUp();
        TemplateDocumentPopUp();
        txtCopyFromExisting.Attributes.Add("readonly", "readonly");
        txtAnotherTemp.Attributes.Add("readonly", "readonly");
        if (UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.User.ToString())
        {
            ddlAllowAccess.Attributes.Add("disabled", "disabled");
            ddlAllowTempAccess.Attributes.Add("disabled", "disabled");
        }
        else if (UserType == enUserRole.SuperUser.ToString())
        {
            ListItem hidePublicFromDropDown = ddlAllowAccess.Items.FindByText("Public");
            ddlAllowAccess.Items.Remove(hidePublicFromDropDown);
            ddlAllowTempAccess.Items.Remove(hidePublicFromDropDown);
        }
    }

    protected void gvCreateMORe_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (Page.IsPostBack)
                {
                    pnlCreateTempForm.Style.Add("display", "none");
                    pnlEditTemplate.Style.Add("display", "visible");
                    ClearTemplateFields();
                    Session["TemplateId"] = templateId = Convert.ToInt32(e.CommandArgument.ToString());
                    TemplateId = templateId;
                    hdnTemplateId.Value = templateId.ToString();
                    GetTemplateByTemplateId(templateId);
                    hdnTemplateName.Value = GetTemplateNameByTemplateId(templateId);
                    hdnIsEdit.Value = "true";
                    MOReMessage.Clear("");
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void gvCreateMORe_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            MOReMessage.Clear("");
            gvCreateMORe.PageIndex = e.NewPageIndex;
            //if (hdnIsCreate.Value == "true")
            //{
            //    pnlCreateTempForm.Style.Add("display", "visible");
            //}
            //else
            pnlCreateTempForm.Style.Add("display", "none");
            if (hdnIsEdit.Value == "true")
            {
                pnlEditTemplate.Style.Add("display", "visible");
            }
            else
                pnlEditTemplate.Style.Add("display", "none");

            GetTemplate();
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnCancelTemplate_Click(object sender, EventArgs e)
    {
        pnlCreateTempForm.Style.Add("display", "none");
        pnlEditTemplate.Style.Add("display", "none");
        MOReMessage.Clear("");
        ClearTemplateFields();
    }

    protected void btnDiscardChanges_Click(object sender, EventArgs e)
    {

        MOReMessage.Clear("");
        pnlCreateTempForm.Style.Add("display", "none");
        pnlEditTemplate.Style.Add("display", "none");
    }

    protected void btnSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            string name = txtTemplateName.Text.Trim();
            string shortName = txtTemplateShortName.Text.Trim();
            string description = txtTemplateDescription.InnerText.Trim();
            int templateTypeId = Convert.ToInt32(ddlTemplateType.SelectedValue);
            int templateCategoryId = Convert.ToInt32(ddlTemplateCategory.SelectedValue);
            int allowAccessId = Convert.ToInt32(ddlAllowAccess.SelectedValue);
            DateTime createdOn = System.DateTime.Now;
            DateTime lastUpdatedDate = System.DateTime.Now;
            int submittedActionId = Convert.ToInt32(ddlTemplateSubmittedTo.SelectedValue);
            string tempDocName = txtTemplateDocument.Text.Trim();
            bool hasStandardDocument = rbYes.Checked;
            string docStoreName = txtStoreName.Text.Trim();
            bool hasDocumentStore = rbDocStoreYes.Checked;
            int standardFolderId = Convert.ToInt32(ddlStandardFolder.SelectedValue);
            int toolLevelId = Convert.ToInt32(ddlToolLevel.SelectedValue);


            _template = new ProjectTemplateBO();
            _template.Name = name;
            _template.ShortName = shortName;
            _template.Description = description;
            _template.TemplateTypeId = templateTypeId;
            _template.TemplateCategoryId = templateCategoryId;
            _template.TemplateAccessId = allowAccessId;
            _template.CreatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _template.CreatedDate = createdOn;
            _template.LastUpdatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _template.LastUpdatedDate = lastUpdatedDate;
            _template.SubmittedActionId = submittedActionId;
            _template.IsActive = true;
            _template.PracticeId = PracticeId;
            _template.EnterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
            _template.DocPath = Util.GetStDocsFilePath(tempDocName);
            _template.HasStandardFolder = hasStandardDocument;
            _template.HasDocumentStore = hasDocumentStore;
            _template.DocumentStoreName = docStoreName;
            _template.StandardFolderId = standardFolderId;
            _template.ToolLevelId = toolLevelId;

            if (blankTemplate.Checked)
            {
                if (_template.IsTemplateNameAvailable())
                {
                    bool save = _template.SaveTemplate();
                    if (save)
                    {
                        upnlCreateMORe.Update();
                        MOReMessage.Success("Template saved successfully.");
                        pnlEditTemplate.Style.Add("display", "none");
                        pnlCreateTempForm.Style.Add("display", "none");
                        GetTemplate();
                        ClearTemplateFields();
                    }
                    else
                    {
                        upnlCreateMORe.Update();
                        MOReMessage.Error("Template couldn't be saved.");
                        pnlEditTemplate.Style.Add("display", "none");
                        pnlCreateTempForm.Style.Add("display", "none");
                        ClearTemplateFields();
                    }
                }
                else
                {
                    upnlCreateMORe.Update();
                    MOReMessage.Error("Template Name Already Exist.");
                    pnlEditTemplate.Style.Add("display", "none");
                    pnlCreateTempForm.Style.Add("display", "visible");
                }
            }
            else if (copyFromExisting.Checked)
            {
                if (_template.IsTemplateNameAvailable())
                {
                    existingTemplateName = txtCopyFromExisting.Text;
                    if (existingTemplateName != "")
                    {

                        existingTempName = existingTemplateName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
                        bool save = _template.AddTemplateWithCopyExistingTemplate(existingTempName);
                        if (save)
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Success("Template saved successfully.");
                            pnlEditTemplate.Style.Add("display", "none");
                            pnlCreateTempForm.Style.Add("display", "none");
                            GetTemplate();
                            ClearTemplateFields();
                        }
                        else
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Error("Template couldn't be saved.");
                            pnlEditTemplate.Style.Add("display", "none");
                            pnlCreateTempForm.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                    }
                    else
                    {
                        upnlCreateMORe.Update();
                        MOReMessage.Error("Select Existing Template.");
                        pnlEditTemplate.Style.Add("display", "none");
                        pnlCreateTempForm.Style.Add("display", "visible");
                    }
                }
                else
                {
                    upnlCreateMORe.Update();
                    MOReMessage.Error("Template Name Already Exist.");
                    pnlEditTemplate.Style.Add("display", "none");
                    pnlCreateTempForm.Style.Add("display", "visible");
                }
            }
            else
            {
                upnlCreateMORe.Update();
                MOReMessage.Error("Select Create Blank Or Copy from Exiting Template.");
                pnlCreateTempForm.Style.Add("display", "visible");
                pnlEditTemplate.Style.Add("display", "none");
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            upnlCreateMORe.Update();
            MOReMessage.Error("Template couldn't be saved.");
            pnlEditTemplate.Style.Add("display", "none");
            pnlCreateTempForm.Style.Add("display", "none");
        }
    }

    protected void btnUpdateTemplate_Click(object sender, EventArgs e)
    {
        try
        {

            templateId = Convert.ToInt32(hdnTemplateId.Value);
            string name = txtTempName.Text.Trim();
            string shortName = txtTempShortName.Text.Trim();
            string description = txtTempDescription.InnerText.Trim();
            int templateTypeId = Convert.ToInt32(ddlTempType.SelectedValue);
            int templateCategoryId = Convert.ToInt32(ddlTempCategory.SelectedValue);
            int allowAccessId = Convert.ToInt32(ddlAllowTempAccess.SelectedValue);
            DateTime lastUpdatedDate = System.DateTime.Now;
            int submittedActionId = Convert.ToInt32(ddlSubmittedTo.SelectedValue);
            string tempDocName = txtTempDocs.Text.Trim();
            bool hasStanDocument = rbEditYes.Checked;
            int stanFolderId = Convert.ToInt32(ddlStanFolder.SelectedValue);
            string docStoName = txtStoName.Text.Trim();
            bool hasDocStore = rbDocyes.Checked;
            int toolLevelId = Convert.ToInt32(ddlTools.SelectedValue);

            if (active.Checked)
            { _template.IsActive = true; }

            _template = new ProjectTemplateBO();

            _template.TemplateId = templateId;
            _template.Name = name;
            _template.ShortName = shortName;
            _template.Description = description;
            _template.TemplateTypeId = templateTypeId;
            _template.TemplateCategoryId = templateCategoryId;
            _template.TemplateAccessId = allowAccessId;
            _template.LastUpdatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _template.LastUpdatedDate = lastUpdatedDate;
            _template.SubmittedActionId = submittedActionId;
            _template.PracticeId = PracticeId;
            _template.EnterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
            _template.DocPath = Util.GetStDocsFilePath(tempDocName);
            _template.HasStandardFolder = hasStanDocument;
            _template.StandardFolderId = stanFolderId;
            _template.DocumentStoreName = docStoName;
            _template.HasDocumentStore = hasDocStore;
            _template.ToolLevelId = toolLevelId;
            if (active.Checked)
            { _template.IsActive = true; }
            else
            {
                _template.IsActive = false;

            }

            if (_template.IsTemplateNameAvailableForEdit())
            {
                if (txtAnotherTemp.Text == "")
                {
                    bool IsUsed = _template.IsUsedTemplate(templateId);
                    if (IsUsed)
                    {
                        bool update = _template.UpdateTemplate(templateId);
                        if (update)
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Success("Template updated successfully.");
                            GetTemplate();
                            pnlEditTemplate.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                        else if (!update)
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Warning("This template is used in a project. You cannot turn this template to inactive.");
                            pnlEditTemplate.Style.Add("display", "none");
                        }
                        else
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Error("Template couldn't be updated.");
                            pnlEditTemplate.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        upnlCreateMORe.Update();
                        MOReMessage.Warning("This template is used in a project. You cannot change this template.");
                        pnlEditTemplate.Style.Add("display", "none");
                    }
                }
                else
                {
                    existingTemplateName = txtCopyFromExisting.Text;
                    existingTempName = existingTemplateName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
                    bool IsUsed = _template.IsUsedTemplate(templateId);
                    if (IsUsed)
                    {
                        bool update = _template.UpdateWithMergeTemplate(templateId, existingTempName);
                        if (update)
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Success("Template updated successfully.");
                            GetTemplate();
                            pnlEditTemplate.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                        else if (!update)
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Warning("This template is used by some practice. You cannot turn this template to inactive.");
                            pnlEditTemplate.Style.Add("display", "none");
                        }
                        else
                        {
                            upnlCreateMORe.Update();
                            MOReMessage.Error("Template couldn't be updated.");
                            pnlEditTemplate.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                    }
                    else
                    {
                        upnlCreateMORe.Update();
                        MOReMessage.Warning("This template is used by some practice. You cannot change this template.");
                        pnlEditTemplate.Style.Add("display", "none");
                    }
                }
            }
            else
            {
                upnlCreateMORe.Update();
                MOReMessage.Error("Template Name is already Exists.");
                pnlEditTemplate.Style.Add("display", "visible");
                pnlCreateTempForm.Style.Add("display", "none");
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            upnlCreateMORe.Update();
            MOReMessage.Error("Template couldn't be saved.");
        }
    }
    #endregion

    #region FUNCTIONS

    private void GetStandardFolder()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _templateStandardList;
            ddlStandardFolder.DataTextField = "Name";
            ddlStandardFolder.DataValueField = "StandardFolderId";
            ddlStanFolder.DataTextField = "Name";
            ddlStanFolder.DataValueField = "StandardFolderId";
            _templateStandardList = _template.GetStandardFolder();
            ddlStandardFolder.DataSource = _templateStandardList;
            ddlStanFolder.DataSource = _templateStandardList;
            ddlStandardFolder.DataBind();
            ddlStanFolder.DataBind();
            ddlStandardFolder.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlStanFolder.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {

            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Standard Folder. Please contact your site administrator.");
        }
    }

    private void GetTemplateType()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _templateTypeList;
            ddlTemplateType.DataTextField = "TemplateType1";
            ddlTemplateType.DataValueField = "TemplateTypeId";
            ddlTempType.DataTextField = "TemplateType1";
            ddlTempType.DataValueField = "TemplateTypeId";
            _templateTypeList = _template.GetTemplateType();
            ddlTemplateType.DataSource = _templateTypeList;
            ddlTempType.DataSource = _templateTypeList;
            ddlTemplateType.DataBind();
            ddlTempType.DataBind();
            ddlTemplateType.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlTempType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Template Type. Please contact your site administrator.");
        }
    }

    private void GetTemplateCategory()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _templateCategoryList;
            ddlTemplateCategory.DataTextField = "TemplateCategory1";
            ddlTemplateCategory.DataValueField = "TemplateCategoryId";
            ddlTempCategory.DataTextField = "TemplateCategory1";
            ddlTempCategory.DataValueField = "TemplateCategoryId";
            _templateCategoryList = _template.GetTemplateCategory();
            ddlTemplateCategory.DataSource = _templateCategoryList;
            ddlTempCategory.DataSource = _templateCategoryList;
            ddlTemplateCategory.DataBind();
            ddlTempCategory.DataBind();
            ddlTemplateCategory.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlTempCategory.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Template Category. Please contact your site administrator.");
        }
    }

    private void GetAllowAccess()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _templateAllowAccessList;
            ddlAllowAccess.DataTextField = "AccessLevelName";
            ddlAllowAccess.DataValueField = "AccessLevelId";
            ddlAllowTempAccess.DataTextField = "AccessLevelName";
            ddlAllowTempAccess.DataValueField = "AccessLevelId";
            _templateAllowAccessList = _template.GetAllowAccess();
            ddlAllowAccess.DataSource = _templateAllowAccessList;
            ddlAllowTempAccess.DataSource = _templateAllowAccessList;
            ddlAllowAccess.DataBind();
            ddlAllowTempAccess.DataBind();
            ddlAllowAccess.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlAllowTempAccess.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Allow Access. Please contact your site administrator.");
        }
    }

    private void GetSubmittedList()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _submittedToList;
            ddlTemplateSubmittedTo.DataTextField = "SubmitTo";
            ddlTemplateSubmittedTo.DataValueField = "TemplateSubmitActionId";
            ddlSubmittedTo.DataTextField = "SubmitTo";
            ddlSubmittedTo.DataValueField = "TemplateSubmitActionId";
            _submittedToList = _template.GetSubmittedToList();
            ddlTemplateSubmittedTo.DataSource = _submittedToList;
            ddlSubmittedTo.DataSource = _submittedToList;
            ddlTemplateSubmittedTo.DataBind();
            ddlSubmittedTo.DataBind();
            ddlTemplateSubmittedTo.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlSubmittedTo.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Submitted To. Please contact your site administrator.");
        }
    }

    private void GetToolLevel()
    {
        try
        {
            _template = new ProjectTemplateBO();
            IQueryable _toolLevel;
            ddlToolLevel.DataTextField = "ToolLevelName";
            ddlToolLevel.DataValueField = "ToolLevelId";
            ddlTools.DataTextField = "ToolLevelName";
            ddlTools.DataValueField = "ToolLevelId";
            _toolLevel = _template.GetToolLevel();
            ddlToolLevel.DataSource = _toolLevel;
            ddlTools.DataSource = _toolLevel;
            ddlToolLevel.DataBind();
            ddlTools.DataBind();
            ddlToolLevel.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlTools.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
            MOReMessage.Error("System failed to load the Submitted To. Please contact your site administrator.");
        }
    }

    private void GetTemplate()
    {
        try
        {
            IQueryable _templateList;
            _templateList = _template.GetTemplate(PracticeId, UserType, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            gvCreateMORe.DataSource = _templateList;
            gvCreateMORe.DataBind();
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    public void GetTemplateByTemplateId(int templateId)
    {
        try
        {
            _template = new ProjectTemplateBO();
            Template _templateById;
            _templateById = _template.GetTemplateByTemplateId(templateId);

            // Fetching information to display
            txtTempName.Text = _templateById.Name;
            txtStoName.Text = _templateById.DocumentStoreName;
            txtTempShortName.Text = _templateById.ShortName;
            txtTempDescription.InnerText = _templateById.Description;

            if (_templateById.HasStandardFolder == true)
            {
                rbEditYes.Checked = true;
            }

            else
            {

                rbEditNo.Checked = true;
                ddlStanFolder.SelectedIndex = -1;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Script", "ShowDropDown();", true);

            }

            if (_templateById.HasDocumentStore == true)
            {
                rbDocyes.Checked = true;
            }
            else
            {
                rbDocNo.Checked = true;
                txtStoName.Text = string.Empty;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Script1", "ShowDocumentStoreTextBox();", true);

            }

            if (_templateById.IsActive)
            {
                active.Checked = true;
                inactive.Checked = false;
            }
            else
            {
                inactive.Checked = true;
                active.Checked = false;
            }

            ddlTempType.SelectedIndex =
                ddlTempType.Items.IndexOf(ddlTempType.Items.FindByValue(Convert.ToString(_templateById.TemplateTypeId)));

            ddlTempCategory.SelectedIndex =
                ddlTempCategory.Items.IndexOf(ddlTempCategory.Items.FindByValue(Convert.ToString(_templateById.TemplateCategoryId)));
            if (UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.User.ToString())
                ddlAllowTempAccess.SelectedIndex = 3;//for practice level
            else
                ddlAllowTempAccess.SelectedIndex = ddlAllowTempAccess.Items.IndexOf(ddlAllowTempAccess.Items.FindByValue(Convert.ToString(_templateById.AccessLevelId)));

            ddlSubmittedTo.SelectedIndex =
                ddlSubmittedTo.Items.IndexOf(ddlSubmittedTo.Items.FindByValue(Convert.ToString(_templateById.SubmitActionId)));

            txtTempDocs.Text = Util.GetFileNameByPath(_templateById.DocPath);

            ddlTools.SelectedIndex =
            ddlTools.Items.IndexOf(ddlTools.Items.FindByValue(Convert.ToString(_templateById.ToolLevelId)));

            ddlStanFolder.SelectedIndex =
                ddlStanFolder.Items.IndexOf(ddlStanFolder.Items.FindByValue(Convert.ToString(_templateById.StandardFolderId)));


        }
        catch (Exception exception)
        { throw exception; }
    }

    public string GetTemplateNameByTemplateId(int templateId)
    {
        try
        {
            _template = new ProjectTemplateBO();
            templateName = _template.GetTemplateNameByTemplateId(templateId);
            return templateName;
        }
        catch (Exception exception)
        { throw exception; }
    }

    public void ClearTemplateFields()
    {
        txtTemplateName.Text = txtTemplateShortName.Text = txtTemplateDescription.InnerText = txtStoreName.Text =
            txtCopyFromExisting.Text = txtAnotherTemp.Text = "";
        ddlTemplateCategory.SelectedIndex = ddlTemplateSubmittedTo.SelectedIndex = ddlTemplateType.SelectedIndex = 0;
        ddlStandardFolder.SelectedIndex = -1;
        if (UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.User.ToString())
            ddlAllowAccess.SelectedIndex = 3;//for practice level
        else
            ddlAllowAccess.SelectedIndex = 0;
        //if (_template.HasStandardFolder == true)
        //{
        //rbYes.Checked = false;
        //}

        //else
        //{

        //rbNo.Checked = false;
        //ddlStanFolder.SelectedIndex = -1;
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script2", "ShowTextBoxes();", true);
        //}

        //if (_template.HasDocumentStore == true)
        //{
        //rbDocStoreYes.Checked = false;
        //}
        //else
        //{
        //rbDocStoreNo.Checked = false;
        //txtStoName.Text = string.Empty;
        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script3", "ShowDocumentStore();", true);

        //}

    }

    private void TemplatePopUp()
    {

        Table TemplateTable = new Table();
        TemplateTable.ID = "TemplateTable";
        TemplateTable.ClientIDMode = ClientIDMode.Static;
        List<string> tempCat = _template.GetCategory();
        for (tempCatCount = 0; tempCatCount < tempCat.Count(); tempCatCount++)
        {
            TableRow TempCatRow = new TableRow();

            TableCell imgCell = new TableCell();

            HyperLink link = new HyperLink();
            link.ID = "TemplatePopupElement" + tempCatCount;
            link.ClientIDMode = ClientIDMode.Static;

            Image img = new Image();
            img.ClientIDMode = ClientIDMode.Static;
            img.ImageUrl = "../Themes/Images/Plus.png";
            img.ID = "imgStandard" + tempCatCount;
            img.CssClass = "toggle-img";

            link.Attributes.Add("onclick", "TemplateList('" + tempCatCount + "');");
            link.Controls.Add(img);

            imgCell.Controls.Add(link);

            LinkButton lbTempCatName = new LinkButton();
            lbTempCatName.Text = " " + tempCat[tempCatCount];
            lbTempCatName.Attributes.Add("class", "TemplateTree");
            lbTempCatName.Attributes.Add("onclick", "TemplateList('" + tempCatCount + "'); return false;");

            imgCell.Controls.Add(lbTempCatName);
            TempCatRow.Cells.Add(imgCell);
            TemplateTable.Controls.Add(TempCatRow);

            TableRow tempNameRow = new TableRow();
            TableCell tempNameCell = new TableCell();

            Table TemplateElement = new Table();
            TemplateElement.ID = "TemplateElement" + tempCatCount;
            TemplateElement.ClientIDMode = ClientIDMode.Static;
            TemplateElement.Style.Add("display", "none");

            List<string> templateName = _template.GetTemplateByTemplateCategory(tempCat[tempCatCount], PracticeId, UserType, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

            for (tempNameCount = 0; tempNameCount < templateName.Count; tempNameCount++)
            {
                TableRow tplName = new TableRow();
                TableCell chkbxtemp = new TableCell();

                CheckBox chk = new CheckBox();
                chk.ID = "listOfTempName" + tempCatCount + tempNameCount;
                chk.ClientIDMode = ClientIDMode.Static;
                chk.Text = templateName[tempNameCount];
                chk.Attributes.Add("class", "TempLink");
                chkbxtemp.Controls.Add(chk);
                tplName.Controls.Add(chkbxtemp);

                TemplateElement.Controls.Add(tplName);
            }
            tempNameCell.Controls.Add(TemplateElement);
            tempNameRow.Cells.Add(tempNameCell);
            TemplateTable.Controls.Add(tempNameRow);
        }

        ProjectTemplatePopUp.Controls.Add(TemplateTable);
        upnlProjectTempalte.Update();
    }

    private void TemplateDocumentPopUp()
    {
        Table TemplateDocTable = new Table();
        TemplateDocTable.ID = "TemplateDocTable";
        TemplateDocTable.ClientIDMode = ClientIDMode.Static;
        List<string> tempDoc = Util.GetTempDoc();
        for (tempCatCount = 0; tempCatCount < tempDoc.Count(); tempCatCount++)
        {
            TableRow TempCatRow = new TableRow();

            TableCell imgCell = new TableCell();

            RadioButton rdo = new RadioButton();
            rdo.ID = "TempName" + tempCatCount + tempNameCount;
            rdo.ClientIDMode = ClientIDMode.Static;
            rdo.GroupName = "tempDocs";
            rdo.Text = tempDoc[tempCatCount];

            imgCell.Controls.Add(rdo);
            TempCatRow.Cells.Add(imgCell);
            TemplateDocTable.Controls.Add(TempCatRow);
        }

        pnlTempDocs.Controls.Add(TemplateDocTable);
        upnlTempDocs.Update();
    }
    #endregion
}