using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;


public partial class ConfigureProjectTemplate : System.Web.UI.UserControl
{
    #region PROPERTY
    public int PracticeId { get; set; }
    public string UserType { get; set; }
    public int EnterpriseId { get; set; }
    #endregion

    #region CONTROL
    private ProjectTemplateBO _template = new ProjectTemplateBO();
    #endregion

    #region CONSTANT
    private int FIRST_COL_INDEX = 0;
    private int TEMP_ID_NONE = 0;
    private int TOPROWINDEX = 0;
    private int NEXTORPREVIOUS = 1;
    private int MAXCOLFORSWAP = 5;
    private int PROJECT_COL_INDEX = 4;
    private int CORPORATE_COL_INDEX = 5;
    public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";
    #endregion

    #region VARIABLES
    private int lastRowIndex;
    private List<int> projectIds;
    private bool isChecked;
    private int rowIndex;
    private int currentRowId;
    private int columnIndex;
    private int tempDataKey;
    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetProject();
            GetSelectedProject();
            msgConfigureProject.Clear("");
            GetSiteByPracticeId();

        }
        upnlConfigureProjects.Update();
    }

    protected void gvConfigureProjectTemplate_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            msgConfigureProject.Clear("");
            if (e.CommandName == "Up")
            {
                if (Page.IsPostBack)
                {
                    int currentRowId = Convert.ToInt32(e.CommandArgument);
                    // Get the last name of the selected author from the appropriate
                    // cell in the GridView control.
                    if (currentRowId > TOPROWINDEX) // Rowid == 0 indicates that the top most row is reached
                        Swap(currentRowId, "Up");
                }
            }
            else if (e.CommandName == "Down")
            {
                if (Page.IsPostBack)
                {
                    currentRowId = Convert.ToInt32(e.CommandArgument);
                    if (currentRowId != gvConfigureProjectTemplate.Rows.Count - 1)	// the bottom most row
                        // is reached
                        Swap(currentRowId, "Down");
                }
            }
        }

        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnSelectProject_Click(Object sender, EventArgs e)
    {
        try
        {
            projectIds = new List<int>();
            gvPracTemp.AllowPaging = false;
            GetProject();
            RePopulateCheckBoxes();
            for (rowIndex = TOPROWINDEX; rowIndex < gvPracTemp.Rows.Count; rowIndex++)
            {
                CheckBox chktempId = (CheckBox)gvPracTemp.Rows[rowIndex].FindControl("chkProjects");
                if (chktempId.Checked)
                {
                    projectIds.Add(Convert.ToInt32(gvPracTemp.DataKeys[rowIndex].Value));
                }
            }
            if (projectIds.Count == TEMP_ID_NONE)
            {
                msgConfigureProject.Warning("No project is selected to Save to My Selected Projects.");
            }
            else if (AddPracticeTemplate(projectIds, PracticeId))
            {
                msgConfigureProject.Success("Projects are Selected successfully.");
            }
            else
            {
                msgConfigureProject.Error("Error occur while saving.");
            }
            gvPracTemp.AllowPaging = true;
            GetProject();
            GetSelectedProject();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnSaveProject_Click(Object sender, EventArgs e)
    {
        try
        {
            projectIds = new List<int>();
            for (rowIndex = TOPROWINDEX; rowIndex < gvConfigureProjectTemplate.Rows.Count; rowIndex++)
            {
                CheckBox chprojects = (CheckBox)gvConfigureProjectTemplate.Rows[rowIndex].FindControl("chkMyProjects");
                if (chprojects.Checked)
                {
                    projectIds.Add(Convert.ToInt32(gvConfigureProjectTemplate.Rows[rowIndex].Cells[TOPROWINDEX].Text));
                }
            }
            if (SavePracticeTemplate(projectIds, PracticeId))
            {
                msgConfigureProject.Success("Projects are successfully added in your practice.");
                if (projectIds.Count == TEMP_ID_NONE)
                {
                    msgConfigureProject.Warning("All projects are deleted from My Selected Projects.");
                }
            }
            else
            {
                msgConfigureProject.Error("Error occur while saving.");
            }
            GetSelectedProject();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancelProject_Click(object sender, EventArgs e)
    {
        try
        {
            GetSelectedProject();
            msgConfigureProject.Clear("");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvPracTemp_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            msgConfigureProject.Clear("");
            gvPracTemp.PageIndex = e.NewPageIndex;
            GetProject();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnRefreshPage_Click(object sender, EventArgs e)
    {
        try
        {
            GetProject();
            GetSelectedProject();
            msgConfigureProject.Clear("");
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void chkMyProjects_OnCheckedChanged(Object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvPracTemp.Rows)
            {
                CheckBox chkBox = (CheckBox)row.FindControl("chkProjects");
                int index = int.Parse(gvPracTemp.DataKeys[row.RowIndex].Value.ToString());
                if (chkBox.Checked)
                {
                    PersistRowIndex(index);
                }
                else
                {
                    RemoveRowIndex(index);
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void getPracticeSite_Click(object sender, EventArgs e)
    {
        try
        {
            if (hiddenTemplateId.Value != null)
                GetCorporateSubmission(PracticeId, Convert.ToInt32(hiddenTemplateId.Value.ToString()));
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    #endregion

    #region FUNCTIONS
    private void Swap(int currentRowId, string option)
    {

        if (option == "Up")
        {
            tempDataKey = Convert.ToInt32(gvConfigureProjectTemplate.DataKeys[currentRowId - NEXTORPREVIOUS].Value);
            for (columnIndex = FIRST_COL_INDEX; columnIndex <= MAXCOLFORSWAP; columnIndex++)
            {
                string temp = gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].Text;
                if (temp != "")
                {
                    gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].Text =
                    gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].Text;
                    gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].Text = temp;
                }
                else if (columnIndex == PROJECT_COL_INDEX)
                {
                    CheckBox tempNext = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkMyProjects");
                    if (tempNext.Checked)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                    }
                    CheckBox prevProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkMyProjects");
                    if (prevProjects.Checked)
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkMyProjects");
                        nextProjects.Checked = true;
                    }
                    else
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkMyProjects");
                        nextProjects.Checked = false;
                    }
                    CheckBox preProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkMyProjects");
                    preProjects.Checked = isChecked;
                }
                else if (columnIndex == CORPORATE_COL_INDEX)
                {
                    CheckBox tempNext = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkCorporate");
                    if (tempNext.Checked)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                    }
                    CheckBox prevProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkCorporate");
                    if (prevProjects.Checked)
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkCorporate");
                        nextProjects.Checked = true;
                        nextProjects.Attributes.Remove("disabled");
                        nextProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId].Cells[0].Text.ToString()) + ")");
                    }
                    else
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkCorporate");
                        nextProjects.Checked = false;
                        if (_template.IsAvailableForCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId].Cells[0].Text.ToString())))
                        {
                            nextProjects.Attributes.Remove("disabled");
                            nextProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId].Cells[0].Text.ToString()) + ")");
                        }
                        else
                        {
                            nextProjects.Attributes.Add("disabled", "disabled");
                            nextProjects.Attributes.Add("onclick", "return false");
                        }
                    }
                    CheckBox preProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkCorporate");
                    preProjects.Checked = isChecked;
                    if (_template.IsAvailableForCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[0].Text.ToString())))
                    {
                        preProjects.Attributes.Remove("disabled");
                        preProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId - NEXTORPREVIOUS].Cells[0].Text.ToString()) + ")");
                    }
                    else
                    {
                        preProjects.Attributes.Add("disabled", "disabled");
                        preProjects.Attributes.Add("onclick", "return false");
                    }
                }
            }
        }
        else
        {
            for (int columnIndex = FIRST_COL_INDEX; columnIndex <= MAXCOLFORSWAP; columnIndex++)
            {
                string temp = gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].Text;
                if (temp != "")
                {
                    gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].Text =
                    gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].Text;
                    gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].Text = temp;
                }
                else if (columnIndex == PROJECT_COL_INDEX)
                {
                    CheckBox tempNext = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkMyProjects");
                    if (tempNext.Checked)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                    }
                    CheckBox prevProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkMyProjects");
                    if (prevProjects.Checked)
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkMyProjects");
                        nextProjects.Checked = true;
                    }
                    else
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkMyProjects");
                        nextProjects.Checked = false;
                    }
                    CheckBox preProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkMyProjects");
                    preProjects.Checked = isChecked;
                }
                else if (columnIndex == CORPORATE_COL_INDEX)
                {
                    CheckBox tempNext = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkCorporate");
                    if (tempNext.Checked)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                    }
                    CheckBox prevProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkCorporate");
                    if (prevProjects.Checked)
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkCorporate");
                        nextProjects.Checked = true;
                        nextProjects.Attributes.Remove("disabled");
                        nextProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[0].Text.ToString()) + ")");
                    }
                    else
                    {
                        CheckBox nextProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[columnIndex].FindControl("chkCorporate");
                        nextProjects.Checked = false;
                        if (_template.IsAvailableForCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[0].Text.ToString())))
                        {
                            nextProjects.Attributes.Remove("disabled");
                            nextProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId + NEXTORPREVIOUS].Cells[0].Text.ToString()) + ")");
                        }
                        else
                        {
                            nextProjects.Attributes.Add("disabled", "disabled");
                            nextProjects.Attributes.Add("onclick", "return false");
                        }
                    }
                    CheckBox preProjects = (CheckBox)gvConfigureProjectTemplate.Rows[currentRowId].Cells[columnIndex].FindControl("chkCorporate");
                    preProjects.Checked = isChecked;
                    if (_template.IsAvailableForCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId].Cells[0].Text.ToString())))
                    {
                        preProjects.Attributes.Remove("disabled");
                        preProjects.Attributes.Add("onclick", "OnCheckCorporate(" + Convert.ToInt32(gvConfigureProjectTemplate.Rows[currentRowId].Cells[0].Text.ToString()) + ")");
                    }
                    else
                    {
                        preProjects.Attributes.Add("disabled", "disabled");
                        preProjects.Attributes.Add("onclick", "return false");
                    }
                }
            }
        }
    }

    public void GetProject()
    {
        try
        {
            IQueryable _templateList;
            _templateList = _template.GetProject(PracticeId, UserType, EnterpriseId);

            gvPracTemp.DataSource = _templateList;
            gvPracTemp.DataBind();


            if (gvPracTemp.Rows.Count == 0)
                btnSelectProject.Attributes.Add("disabled", "disabled");
            else
                btnSelectProject.Attributes.Remove("disabled");
            RePopulateCheckBoxes();

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void GetSelectedProject()
    {
        try
        {
            IQueryable _templateList;
            _templateList = _template.GetSelectedProject(PracticeId);

            gvConfigureProjectTemplate.DataSource = _templateList;
            gvConfigureProjectTemplate.DataBind();
            if (gvConfigureProjectTemplate.Rows.Count == 0)
            {
                btnSaveProject.Attributes.Add("disabled", "disabled");
                btnCancelProject.Attributes.Add("disabled", "disabled");
            }
            else
            {
                btnSaveProject.Attributes.Remove("disabled");
                btnCancelProject.Attributes.Remove("disabled");
            }

            if (gvConfigureProjectTemplate.Rows.Count != 0)
            {
                ImageButton img = (ImageButton)gvConfigureProjectTemplate.Rows[TOPROWINDEX].FindControl("btnUp");
                img.Attributes.Add("disabled", "disabled");
                img.ImageUrl = "~/Themes/Images/arrow-up-disable.png";

                lastRowIndex = gvConfigureProjectTemplate.Rows.Count - 1;

                ImageButton imgBack = (ImageButton)gvConfigureProjectTemplate.Rows[lastRowIndex].FindControl("btnDown");
                imgBack.Attributes.Add("disabled", "disabled");
                imgBack.ImageUrl = "~/Themes/Images/arrow-down-disable.png";
            }
            for (rowIndex = TOPROWINDEX; rowIndex < gvConfigureProjectTemplate.Rows.Count; rowIndex++)
            {
                CheckBox chprojects = (CheckBox)gvConfigureProjectTemplate.Rows[rowIndex].FindControl("chkMyProjects");
                if (_template.IsVisible(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[rowIndex].Cells[0].Text.ToString())))
                    chprojects.Checked = true;
                else
                    chprojects.Checked = false;
            }
            //for (rowIndex = TOPROWINDEX; rowIndex < gvConfigureProjectTemplate.Rows.Count; rowIndex++)
            //{
            //    CheckBox chprojects = (CheckBox)gvConfigureProjectTemplate.Rows[rowIndex].FindControl("chkCorporate");
            //    if (_template.IsAvailableForCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[rowIndex].Cells[0].Text.ToString())))
            //    {
            //        if (_template.AlreadyCorporate(PracticeId, Convert.ToInt32(gvConfigureProjectTemplate.Rows[rowIndex].Cells[0].Text.ToString())))
            //        {
            //            chprojects.Checked = true;
            //        }
            //        else
            //        {
            //            chprojects.Checked = false;
            //        }
            //    }
            //    else
            //    {
            //        chprojects.Attributes.Add("disabled", "disabled");
            //        chprojects.Attributes.Add("onclick", "return false");
            //    }
            //}
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    private bool SavePracticeTemplate(List<int> projectIds, int pracitceId)
    {
        try
        {
            if (_template.SavePracticeTemplate(projectIds, pracitceId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool AddPracticeTemplate(List<int> projectIds, int pracitceId)
    {
        try
        {
            if (_template.AddPracticeTemplate(projectIds, pracitceId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PersistRowIndex(int index)
    {
        if (!SelectedCustomersIndex.Exists(i => i == index))
        {
            SelectedCustomersIndex.Add(index);
        }
    }

    private List<Int32> SelectedCustomersIndex
    {
        get
        {
            if (ViewState[SELECTED_CUSTOMERS_INDEX] == null)
            {
                ViewState[SELECTED_CUSTOMERS_INDEX] = new List<Int32>();
            }
            return (List<Int32>)ViewState[SELECTED_CUSTOMERS_INDEX];
        }
    }

    private void RemoveRowIndex(int index)
    {
        SelectedCustomersIndex.Remove(index);
    }

    private void RePopulateCheckBoxes()
    {
        foreach (GridViewRow row in gvPracTemp.Rows)
        {
            int index = int.Parse(gvPracTemp.DataKeys[row.RowIndex].Value.ToString());
            CheckBox chkBox = (CheckBox)row.FindControl("chkProjects");

            if (SelectedCustomersIndex != null)
            {
                if (SelectedCustomersIndex.Exists(i => i == index))
                {
                    chkBox.Checked = true;
                }
            }
        }
    }

    private void GetSiteByPracticeId()
    {
        SiteBO _site = new SiteBO();
        IQueryable _practiceSiteList;
        _site.GetSitesByPracticeId(PracticeId);
        _practiceSiteList = _site.PracticeSiteList;
        ddlPracSiteName.DataTextField = "Name";
        ddlPracSiteName.DataValueField = "PracticeSiteId";
        ddlPracSiteName.DataSource = _practiceSiteList;
        ddlPracSiteName.DataBind();
        ddlPracSiteName.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void GetCorporateSubmission(int practiceId, int templateId)
    {
        //PracticeTemplate _practice = new PracticeTemplate();
        //_practice = _template.GetTemplateCorporateSubmission(practiceId, templateId);
        //if (_practice.IsCorporate != null)
        //{
        //    if ((bool)_practice.IsCorporate)
        //    {
        //        corporateTypeYes.Checked = true;
        //        corporateTypeNo.Checked = false;
        //        ddlPracSiteName.Enabled = true;
        //        CorpMessage.Style.Add("display", "block");
        //        ddlPracSiteName.SelectedIndex = ddlPracSiteName.Items.IndexOf(ddlPracSiteName.Items.FindByValue(Convert.ToString(_practice.PracticeSiteId)));
        //    }
        //    else
        //    {
        //        corporateTypeYes.Checked = false;
        //        corporateTypeNo.Checked = true;
        //        ddlPracSiteName.Enabled = false;
        //        CorpMessage.Style.Add("display", "none");
        //        ddlPracSiteName.SelectedIndex = -1;
        //    }

        //}
        //else
        //{
        //    corporateTypeYes.Checked = false;
        //    corporateTypeNo.Checked = true;
        //    ddlPracSiteName.Enabled = false;
        //    CorpMessage.Style.Add("display", "none");
        //    ddlPracSiteName.SelectedIndex = -1;
        //}
        //upnlCorporate.Update();
    }
    #endregion

}

