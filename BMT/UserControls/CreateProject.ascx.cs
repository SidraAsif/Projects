using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class CreateProject : System.Web.UI.UserControl
{
    #region CONSTANTS
    private char[] DELIMITATORS = new char[] { ',' };
    private char[] FOLDER_DELIMITATOR = new char[] { '/' };
    #endregion

    #region PPROPERTIES

    public string UserType { get; set; }
    public int ProjectId { get; set; }
    public int PracticeId { get; set; }
    public int EnterpriseId { get; set; }
    public int MedicalGroupId { get; set; }
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

    #region VARIABLES
    private ProjectBO _project = new ProjectBO();
    private ProjectAssignment _projectAssignment = new ProjectAssignment();
    private int projectId;
    private int templateId;
    private int tempNameCount;
    private int tempCatCount;
    private string projectName;

    #endregion

    #region CONTROLS
    private Table elementTable;
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearTemplateFields();
            GetProjects();
            GetAllowAccess();
            FormPopup();
            LoadMGRList();
            LoadEnterpriseList();
            LoadPracticeList();
            //pnlEditProject.Style.Add("display", "none");
        }
        TemplateDocumentPopUp();
        //DynamicFoldersAdding();
        txtSelectTemplate.Attributes.Add("readonly", "readonly");
    }

    protected void gvCreateProjects_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (Page.IsPostBack)
                {
                    pnlCreateProjectForm.Style.Add("display", "none");
                    pnlEditProject.Style.Add("display", "visible");
                    ClearTemplateFields();
                    Session["ProjectId"] = projectId = Convert.ToInt32(e.CommandArgument.ToString());
                    ProjectId = projectId;
                    hdnProjectId.Value = projectId.ToString();
                    GetProjectByProjectId(projectId);
                    hdnProjectName.Value = GetProjectNameByProjectId(projectId);
                    hdnIsEdit.Value = "true";
                    ProjectMassage.Clear("");
                }
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvCreateProjects_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            ProjectMassage.Clear("");
            gvCreateProjects.PageIndex = e.NewPageIndex;
            pnlCreateProjectForm.Style.Add("display", "none");

            if (hdnIsEdit.Value == "true")
            {
                pnlEditProject.Style.Add("display", "visible");
            }
            else
                pnlEditProject.Style.Add("display", "none");

            GetProjects();

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnCancelProject_Click(object sender, EventArgs e)
    {
        pnlCreateProjectForm.Style.Add("display", "none");
        ProjectMassage.Clear("");
        ClearTemplateFields();
    }

    protected void btnDiscardChanges_Click(object sender, EventArgs e)
    {
        ProjectMassage.Clear("");
        pnlCreateProjectForm.Style.Add("display", "none");
        pnlEditProject.Style.Add("display", "none");
        ClearTemplateFields();
    }

    protected void btnSaveProject_Click(object sender, EventArgs e)
    {
        try
        {
            int practiceId = Convert.ToInt32(Session["PracticeId"]);
            string name = txtProjectName.Text.Trim();
            string description = txtProjectDescription.InnerText.Trim();
            int allowAccessId = Convert.ToInt32(ddlAllowAccessTo.SelectedValue);
            DateTime createdOn = System.DateTime.Now;
            DateTime lastUpdatedDate = System.DateTime.Now;
            string tempName = txtSelectTemplate.Text.Trim();
            List<int> enterpriseList = new List<int>();
            List<int> medicalGroupList = new List<int>();
            List<int> practiceList = new List<int>();
            string[] folderList =  hdnFolderList.Value.ToString().Split(FOLDER_DELIMITATOR, StringSplitOptions.RemoveEmptyEntries);

            string[] existingTempName = tempName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);

            string formName = txtForm.Text.Trim();

            string[] existingFormName = formName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);

            bool addProjectFolder = false;
            if (rdoYes.Checked)
                addProjectFolder = true;
            else
                addProjectFolder = false;

            foreach (ListItem item in lstSelectedMGR.Items)
            {
                medicalGroupList.Add(Convert.ToInt32(item.Value));
            }
            foreach (ListItem item in lstSelectedEnterprises.Items)
            {
                enterpriseList.Add(Convert.ToInt32(item.Value));
            }
            foreach (ListItem item in assignedPrac.Items)
            {
                practiceList.Add(Convert.ToInt32(item.Value));
            }

            _project = new ProjectBO();
            _project.Name = name;
            _project.Description = description;
            _project.AccessLevelId = allowAccessId;
            _project.CreatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _project.CreatedDate = createdOn;
            _project.LastUpdatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _project.LastUpdatedDate = lastUpdatedDate;
            _project.TempName = existingTempName;
            _project.EnterpriseIds = enterpriseList;
            _project.PracticeIds = practiceList;
            _project.FormName = existingFormName;
            _project.MedicalGroupIds = medicalGroupList;
            _project.AddProjectFolder = addProjectFolder;
            _project.FolderList = folderList;

            if (_project.IsProjectNameAvailable())
            {
                bool save = _project.SaveProject();
                if (save)
                {
                    upnlCreateProjects.Update();
                    ProjectMassage.Success("Project saved successfully.");
                    pnlCreateProjectForm.Style.Add("display", "none");
                    pnlEditProject.Style.Add("display", "none");
                    GetProjects();
                    ClearTemplateFields();
                }
                else
                {
                    upnlCreateProjects.Update();
                    ProjectMassage.Error("Project couldn't be saved.");
                    pnlCreateProjectForm.Style.Add("display", "none");
                    pnlEditProject.Style.Add("display", "none");
                    ClearTemplateFields();

                }
            }
            else
            {
                upnlCreateProjects.Update();
                ProjectMassage.Error("Project Name Already Exist.");
                pnlCreateProjectForm.Style.Add("display", "visible");
                pnlEditProject.Style.Add("display", "none");

            }
        }
        catch (Exception exception)
        {

            Logger.PrintError(exception);
            upnlCreateProjects.Update();
            ProjectMassage.Error("Project couldn't be saved.");
            pnlCreateProjectForm.Style.Add("display", "none");
        }
    }

    protected void btnUpdateProject_Click(object sender, EventArgs e)
    {
        try
        {
            int practiceId = Convert.ToInt32(Session["PracticeId"]);
            projectId = Convert.ToInt32(hdnProjectId.Value);
            string name = txtProjName.Text.Trim();
            string description = txtProjDescription.InnerText.Trim();
            int allowAccessId = Convert.ToInt32(ddlAllowProjAccess.SelectedValue);
            DateTime lastUpdatedDate = System.DateTime.Now;
            string tempName = txtSelectTemp.Text.Trim();
            string[] existingTempName = tempName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
            string formName = txtSelectedForm.Text.Trim();
            string[] existingFormName = formName.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
            List<int> enterpriseList = new List<int>();
            List<int> medicalGroupList = new List<int>();
            List<int> practiceList = new List<int>();
            string[] folderList = hdnFolderList.Value.ToString().Split(FOLDER_DELIMITATOR, StringSplitOptions.RemoveEmptyEntries);

            bool addProjectFolder = false;
            if (rdoEditYes.Checked)
                addProjectFolder = true;
            else
                addProjectFolder = false;

            foreach (ListItem item in lstBoxAssignedMGR.Items)
            {
                medicalGroupList.Add(Convert.ToInt32(item.Value));
            }
            foreach (ListItem item in lstAssignedEnt.Items)
            {
                enterpriseList.Add(Convert.ToInt32(item.Value));
            }
            foreach (ListItem item in assignedEditPrac.Items)
            {
                practiceList.Add(Convert.ToInt32(item.Value));
            }

            _project = new ProjectBO();
            _project.ProjectId = projectId;
            _project.Name = name;
            _project.Description = description;
            _project.AccessLevelId = allowAccessId;
            _project.LastUpdatedBy = Convert.ToInt32(Session["UserApplicationId"]);
            _project.LastUpdatedDate = lastUpdatedDate;
            _project.TempName = existingTempName;
            _project.EnterpriseIds = enterpriseList;
            _project.PracticeIds = practiceList;
            _project.AddProjectFolder = addProjectFolder;
            _project.MedicalGroupIds = medicalGroupList;
            _project.FormName = existingFormName;
            _project.FolderList = folderList;

            if ((practiceList.Count != 0 || medicalGroupList.Count != 0 || enterpriseList.Count != 0) && (allowAccessId!=(int)enAccessLevelId.Public))
            {
                if (_project.IsProjectNameAvailableForEdit())
                {
                    bool IsUsed = _project.IsUsedProject(projectId);
                    if (IsUsed)
                    {
                        bool update = _project.UpdateProject(projectId);
                        if (update)
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Success("Project updated successfully.");
                            GetProjects();
                            pnlEditProject.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                        else if (!update)
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Warning("This Project is used by some practice. You cannot turn this Project to inactive.");
                            pnlEditProject.Style.Add("display", "none");
                        }
                        else
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Error("Project couldn't be updated.");
                            pnlEditProject.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        upnlCreateProjects.Update();
                        ProjectMassage.Warning("This Project is used by some practice. You cannot change this project.");
                        pnlEditProject.Style.Add("display", "none");
                    }
                }
                else
                {
                    upnlCreateProjects.Update();
                    ProjectMassage.Error("project Name is already Exists.");
                    pnlEditProject.Style.Add("display", "visible");
                    pnlCreateProjectForm.Style.Add("display", "none");
                }
            }
            else if(allowAccessId==(int)enAccessLevelId.Public)
            {

                if (_project.IsProjectNameAvailableForEdit())
                {
                    bool IsUsed = _project.IsUsedProject(projectId);
                    if (IsUsed)
                    {
                        bool update = _project.UpdateProject(projectId);
                        if (update)
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Success("Project updated successfully.");
                            GetProjects();
                            pnlEditProject.Style.Add("display", "none");
                            ClearTemplateFields();
                        }
                        else if (!update)
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Warning("This Project is used by some practice. You cannot turn this Project to inactive.");
                            pnlEditProject.Style.Add("display", "none");
                        }
                        else
                        {
                            upnlCreateProjects.Update();
                            ProjectMassage.Error("Project couldn't be updated.");
                            pnlEditProject.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        upnlCreateProjects.Update();
                        ProjectMassage.Warning("This Project is used by some practice. You cannot change this project.");
                        pnlEditProject.Style.Add("display", "none");
                    }
                }
                else
                {
                    upnlCreateProjects.Update();
                    ProjectMassage.Error("project Name is already Exists.");
                    pnlEditProject.Style.Add("display", "visible");
                    pnlCreateProjectForm.Style.Add("display", "none");
                }
            }
            else
            {
                upnlCreateProjects.Update();
                if(allowAccessId==(int)enAccessLevelId.Practice)
                    ProjectMassage.Error("You have not selected any Practice.");
                else if (allowAccessId == (int)enAccessLevelId.Enterprise)
                    ProjectMassage.Error("You have not selected any Enterprise.");
                else if (allowAccessId == (int)enAccessLevelId.MedicalGroup)
                    ProjectMassage.Error("You have not selected any Medical Group.");

                pnlEditProject.Style.Add("display", "visible");
                pnlCreateProjectForm.Style.Add("display", "none");
            }
        }


        catch (Exception exception)
        {
            Logger.PrintError(exception);
            upnlCreateProjects.Update();
            ProjectMassage.Error("Project couldn't be saved.");
        }
    }

    protected void ddlAllowAccessChanged_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlAllowAccessTo.SelectedItem.Text == "Enterprise")
            {
                divLstENT.Style.Add("display", "visible");
                divLstMGR.Style.Add("display", "none");
                divLstPractice.Style.Add("display", "none");
            }
            else if (ddlAllowAccessTo.SelectedItem.Text == "Medical Group")
            {
                divLstMGR.Style.Add("display", "visible");
                divLstENT.Style.Add("display", "none");
                divLstPractice.Style.Add("display", "none");
            }
            else if (ddlAllowAccessTo.SelectedItem.Text == "Practice")
            {
                divLstMGR.Style.Add("display", "none");
                divLstENT.Style.Add("display", "none");
                divLstPractice.Style.Add("display", "visible");
            }
            else
            {
                divLstMGR.Style.Add("display", "none");
                divLstENT.Style.Add("display", "none");
                divLstPractice.Style.Add("display", "none");
            }
            pnlCreateProjectForm.Style.Add("display", "visible");
            pnlEditProjMessage.Style.Add("display", "none");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlAllowProjAccessChange_Click(object sender, EventArgs e)
    {
        try
        {
            projectId = Convert.ToInt32(hdnProjectId.Value);
            if (ddlAllowProjAccess.SelectedItem.Text == "Enterprise")
            {
                LoadEditEnterpriseList(projectId);
                divEditEnterprise.Style.Add("display", "visible");
                divEditMGR.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
            }
            else if (ddlAllowProjAccess.SelectedItem.Text == "Medical Group")
            {
                LoadEditMGRList(projectId);
                divEditMGR.Style.Add("display", "visible");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
            }
            else if (ddlAllowProjAccess.SelectedItem.Text == "Practice")
            {
                LoadEditPracticeList(projectId);
                divEditMGR.Style.Add("display", "none");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "visible");
            }
            else
            {
                divEditMGR.Style.Add("display", "none");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
            }
            pnlCreateProjectForm.Style.Add("display", "none");
            pnlEditProjMessage.Style.Add("display", "visible");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnAddMGR_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstAvailableMGR.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstAvailableMGR.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstAvailableMGR.Items[SelectIndex[index]].Text;
                lstSelectedMGR.Items.Add(AvailableItem);
            }

            for (int index = lstAvailableMGR.Items.Count - 1; index > -1; index--)
            {
                if (lstAvailableMGR.Items[index].Selected == true)
                {
                    lstAvailableMGR.Items[index].Selected = false;
                    lstAvailableMGR.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnRemoveMGR_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstSelectedMGR.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = lstSelectedMGR.Items[SelectIndex[index]].Value;
                SelectedItems.Text = lstSelectedMGR.Items[SelectIndex[index]].Text;
                lstAvailableMGR.Items.Add(SelectedItems);

            }
            for (int index = lstSelectedMGR.Items.Count - 1; index > -1; index--)
            {
                if (lstSelectedMGR.Items[index].Selected == true)
                {
                    lstSelectedMGR.Items[index].Selected = false;
                    lstSelectedMGR.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstBoxMGR.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstBoxMGR.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstBoxMGR.Items[SelectIndex[index]].Text;
                lstBoxAssignedMGR.Items.Add(AvailableItem);
            }

            for (int index = lstBoxMGR.Items.Count - 1; index > -1; index--)
            {
                if (lstBoxMGR.Items[index].Selected == true)
                {
                    lstBoxMGR.Items[index].Selected = false;
                    lstBoxMGR.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstBoxAssignedMGR.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = lstBoxAssignedMGR.Items[SelectIndex[index]].Value;
                SelectedItems.Text = lstBoxAssignedMGR.Items[SelectIndex[index]].Text;
                lstBoxMGR.Items.Add(SelectedItems);

            }
            for (int index = lstBoxAssignedMGR.Items.Count - 1; index > -1; index--)
            {
                if (lstBoxAssignedMGR.Items[index].Selected == true)
                {
                    lstBoxAssignedMGR.Items[index].Selected = false;
                    lstBoxAssignedMGR.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnAddEnt_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstAvailableEnterprises.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstAvailableEnterprises.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstAvailableEnterprises.Items[SelectIndex[index]].Text;
                lstSelectedEnterprises.Items.Add(AvailableItem);
            }

            for (int index = lstAvailableEnterprises.Items.Count - 1; index > -1; index--)
            {
                if (lstAvailableEnterprises.Items[index].Selected == true)
                {
                    lstAvailableEnterprises.Items[index].Selected = false;
                    lstAvailableEnterprises.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnRemoveEnt_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstSelectedEnterprises.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = lstSelectedEnterprises.Items[SelectIndex[index]].Value;
                SelectedItems.Text = lstSelectedEnterprises.Items[SelectIndex[index]].Text;
                lstAvailableEnterprises.Items.Add(SelectedItems);

            }
            for (int index = lstSelectedEnterprises.Items.Count - 1; index > -1; index--)
            {
                if (lstSelectedEnterprises.Items[index].Selected == true)
                {
                    lstSelectedEnterprises.Items[index].Selected = false;
                    lstSelectedEnterprises.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnEditAddEnt_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstEnt.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstEnt.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstEnt.Items[SelectIndex[index]].Text;
                lstAssignedEnt.Items.Add(AvailableItem);
            }

            for (int index = lstEnt.Items.Count - 1; index > -1; index--)
            {
                if (lstEnt.Items[index].Selected == true)
                {
                    lstEnt.Items[index].Selected = false;
                    lstEnt.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnEditRemoveEnt_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstAssignedEnt.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = lstAssignedEnt.Items[SelectIndex[index]].Value;
                SelectedItems.Text = lstAssignedEnt.Items[SelectIndex[index]].Text;
                lstEnt.Items.Add(SelectedItems);

            }
            for (int index = lstAssignedEnt.Items.Count - 1; index > -1; index--)
            {
                if (lstAssignedEnt.Items[index].Selected == true)
                {
                    lstAssignedEnt.Items[index].Selected = false;
                    lstAssignedEnt.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnAddPrac_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstPrac.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstPrac.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstPrac.Items[SelectIndex[index]].Text;
                assignedPrac.Items.Add(AvailableItem);
            }

            for (int index = lstPrac.Items.Count - 1; index > -1; index--)
            {
                if (lstPrac.Items[index].Selected == true)
                {
                    lstPrac.Items[index].Selected = false;
                    lstPrac.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnRemovePrac_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = assignedPrac.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = assignedPrac.Items[SelectIndex[index]].Value;
                SelectedItems.Text = assignedPrac.Items[SelectIndex[index]].Text;
                lstPrac.Items.Add(SelectedItems);

            }
            for (int index = assignedPrac.Items.Count - 1; index > -1; index--)
            {
                if (assignedPrac.Items[index].Selected == true)
                {
                    assignedPrac.Items[index].Selected = false;
                    assignedPrac.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnEditAddPrac_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = lstEditPrac.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem AvailableItem = new ListItem();
                AvailableItem.Value = lstEditPrac.Items[SelectIndex[index]].Value;
                AvailableItem.Text = lstEditPrac.Items[SelectIndex[index]].Text;
                assignedEditPrac.Items.Add(AvailableItem);
            }

            for (int index = lstEditPrac.Items.Count - 1; index > -1; index--)
            {
                if (lstEditPrac.Items[index].Selected == true)
                {
                    lstEditPrac.Items[index].Selected = false;
                    lstEditPrac.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnEditRemovePrac_Click(object sender, EventArgs e)
    {
        try
        {
            int[] SelectIndex = assignedEditPrac.GetSelectedIndices();
            for (int index = 0; index < SelectIndex.Length; index++)
            {
                ListItem SelectedItems = new ListItem();
                SelectedItems.Value = assignedEditPrac.Items[SelectIndex[index]].Value;
                SelectedItems.Text = assignedEditPrac.Items[SelectIndex[index]].Text;
                lstEditPrac.Items.Add(SelectedItems);

            }
            for (int index = assignedEditPrac.Items.Count - 1; index > -1; index--)
            {
                if (assignedEditPrac.Items[index].Selected == true)
                {
                    assignedEditPrac.Items[index].Selected = false;
                    assignedEditPrac.Items.RemoveAt(index);
                }
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void btnRefreshList_Click(object sender, EventArgs e)
    {
        try
        {
            LoadMGRList();
            LoadEnterpriseList();
            LoadPracticeList();
            lstSelectedEnterprises.Items.Clear();
            lstSelectedMGR.Items.Clear();
            assignedPrac.Items.Clear();
            ProjectMassage.Clear("");
            pnlEditProject.Style.Add("display", "none");
            pnlCreateProjectForm.Style.Add("display", "visible");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region FUNCTIONS
    public void ClearTemplateFields()
    {
        txtProjectName.Text = txtProjectDescription.InnerText = txtSelectTemplate.Text = String.Empty;
        upnlCreateProject.Update();
        if (UserType == enUserRole.Consultant.ToString() || UserType == enUserRole.User.ToString())
            ddlAllowAccessTo.SelectedIndex = 3;//for practice level
        else
            ddlAllowAccessTo.SelectedIndex = -1;

    }

    private void GetProjects()
    {
        try
        {
            int practiceId = Convert.ToInt32(Session["PracticeId"]);
            IQueryable _projectList;
            MedicalGroupId = _project.GetMedicalGroupIdByPracticeId(practiceId);
            _projectList = _project.GetProjects(EnterpriseId, MedicalGroupId, PracticeId,UserType);
            gvCreateProjects.DataSource = _projectList;
            gvCreateProjects.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void GetAllowAccess()
    {
        try
        {
            _project = new ProjectBO();
            IQueryable _templateAllowAccessList;
            ddlAllowAccessTo.DataTextField = "AccessLevelName";
            ddlAllowAccessTo.DataValueField = "AccessLevelId";
            ddlAllowProjAccess.DataTextField = "AccessLevelName";
            ddlAllowProjAccess.DataValueField = "AccessLevelId";
            _templateAllowAccessList = _project.GetAllowAccess();
            ddlAllowAccessTo.DataSource = _templateAllowAccessList;
            ddlAllowProjAccess.DataSource = _templateAllowAccessList;
            ddlAllowAccessTo.DataBind();
            ddlAllowProjAccess.DataBind();
            ddlAllowAccessTo.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlAllowProjAccess.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception)
        {

            throw;
        }
    }

    private void TemplateDocumentPopUp()
    {
        if (EnterpriseId != 0 && PracticeId != 0)
        {
            _project.EnterpriseId = EnterpriseId;
            _project.PracticeId = PracticeId;
            _project.MedicalGroupId = _project.GetMedicalGroupIdByPracticeId(PracticeId);
            Table TemplateDocTable = new Table();
            TemplateDocTable.ID = "TemplateDocTable";
            TemplateDocTable.ClientIDMode = ClientIDMode.Static;
            List<string> tempDoc = _project.GetTemplateName();
            for (tempCatCount = 0; tempCatCount < tempDoc.Count(); tempCatCount++)
            {
                TableRow TempCatRow = new TableRow();

                TableCell imgCell = new TableCell();

                CheckBox chk = new CheckBox();
                chk.ID = "TempName" + tempCatCount + tempNameCount;
                chk.ClientIDMode = ClientIDMode.Static;
                //rdo.GroupName = "tempDocs";
                chk.Text = tempDoc[tempCatCount];
                //if (ProjectId != 0)
                //{
                //    List<Template> temps = _project.GetProjectTemplates(ProjectId);
                //    foreach (Template temp in temps)
                //    {
                //        if (temp.Name == tempDoc[tempCatCount])
                //            chk.Checked = true;
                //    }
                //}

                imgCell.Controls.Add(chk);
                TempCatRow.Cells.Add(imgCell);
                TemplateDocTable.Controls.Add(TempCatRow);
            }

            pnlTemp.Controls.Add(TemplateDocTable);
            upnlTemp.Update();
        }
    }

    public void GetProjectByProjectId(int projectId)
    {
        try
        {
            _project = new ProjectBO();
            Project _projectById;
            _projectById = _project.GetProjectByProjectId(projectId);

            // Fetching Data to display
            txtProjName.Text = _projectById.Name;
            txtProjDescription.InnerText = _projectById.Description;

            string selectedTemplate = _project.GetTemplateProjectSectionByProjectId(projectId);
            txtSelectTemp.Text = selectedTemplate;

            string selectedForm = _project.GetFormProjectSectionByProjectId(projectId);
            txtSelectedForm.Text = selectedForm;

            ddlAllowProjAccess.SelectedIndex = ddlAllowProjAccess.Items.IndexOf(ddlAllowProjAccess.Items.FindByValue(Convert.ToString(_projectById.AccessLevelId)));

            if (ddlAllowProjAccess.SelectedItem.Text == "Enterprise")
            {
                divEditEnterprise.Style.Add("display", "visible");
                divEditMGR.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
                LoadEditEnterpriseList(projectId);
                LoadAssignENTList(projectId);
            }
            else if (ddlAllowProjAccess.SelectedItem.Text == "Medical Group")
            {
                divEditMGR.Style.Add("display", "visible");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
                LoadEditMGRList(projectId);
                LoadAssignMGRList(projectId);
            }
            else if (ddlAllowProjAccess.SelectedItem.Text == "Practice")
            {
                divEditMGR.Style.Add("display", "none");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "visible");
                LoadEditPracticeList(projectId);
                LoadAssignPracList(projectId);
            }
            else
            {
                divEditMGR.Style.Add("display", "none");
                divEditEnterprise.Style.Add("display", "none");
                divEditPrac.Style.Add("display", "none");
            }

            if (_project.GetProjectFolder(_projectById.ProjectId))
            {
                rdoEditNo.Checked = false;
                rdoEditYes.Checked = true;
            }
            else
            {
                rdoEditNo.Checked = true;
                rdoEditYes.Checked = false;
            }

            hdnFolderList.Value = string.Empty;
            hdnFolderList.Value = _project.GetOtherFolders(projectId);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public string GetProjectNameByProjectId(int projectId)
    {
        try
        {
            _project = new ProjectBO();
            projectName = _project.GetProjectNameByProjectID(projectId);
            return projectName;

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private void FormPopup()
    {
        try
        {
            if (EnterpriseId != 0 && PracticeId != 0)
            {
                _project.EnterpriseId = EnterpriseId;
                _project.PracticeId = PracticeId;
                _project.MedicalGroupId = _project.GetMedicalGroupIdByPracticeId(PracticeId);
                Table FormDocTable = new Table();
                FormDocTable.ID = "FormTable";
                FormDocTable.ClientIDMode = ClientIDMode.Static;
                List<string> tempDoc = _project.GetFormList();
                for (tempCatCount = 0; tempCatCount < tempDoc.Count(); tempCatCount++)
                {
                    TableRow FormRow = new TableRow();

                    TableCell imgFormCell = new TableCell();

                    CheckBox chk = new CheckBox();
                    chk.ID = "FormName" + tempCatCount + tempNameCount;
                    chk.ClientIDMode = ClientIDMode.Static;
                    //rdo.GroupName = "FormDocs";
                    chk.Text = tempDoc[tempCatCount];

                    //if (ProjectId != 0)
                    //{
                    //    List<Form> forms = _project.GetProjectForms(ProjectId);
                    //    foreach (Form form in forms)
                    //    {
                    //        if (form.Name == tempDoc[tempCatCount])
                    //            chk.Checked = true;
                    //    }
                    //}
                    imgFormCell.Controls.Add(chk);
                    FormRow.Cells.Add(imgFormCell);
                    FormDocTable.Controls.Add(FormRow);
                }

                pnlForm.Controls.Add(FormDocTable);
                upnlForm.Update();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void LoadMGRList()
    {
        try
        {
            lstAvailableMGR.DataSource = _project.GetAllMedicalGroups(EnterpriseId);
            lstAvailableMGR.DataTextField = "Name";
            lstAvailableMGR.DataValueField = "MedicalGroupId";
            lstAvailableMGR.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadAssignMGRList(int projectId)
    {
        try
        {
            lstBoxAssignedMGR.DataSource = _project.GetMedicalGroupsOfProject(projectId);
            lstBoxAssignedMGR.DataTextField = "Name";
            lstBoxAssignedMGR.DataValueField = "MedicalGroupId";
            lstBoxAssignedMGR.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadAssignENTList(int projectId)
    {
        try
        {
            lstAssignedEnt.DataSource = _project.GetEnterpriseOfProject(projectId);
            lstAssignedEnt.DataTextField = "Name";
            lstAssignedEnt.DataValueField = "EnterpriseId";
            lstAssignedEnt.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadEnterpriseList()
    {
        try
        {
            lstAvailableEnterprises.DataSource = _project.GetEnterpriseOfUser(UserType, EnterpriseId);
            lstAvailableEnterprises.DataTextField = "Name";
            lstAvailableEnterprises.DataValueField = "EnterpriseId";
            lstAvailableEnterprises.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadPracticeList()
    {
        try
        {

            lstPrac.DataSource = _project.GetPracticeOfUser(UserType, EnterpriseId);
            lstPrac.DataTextField = "Name";
            lstPrac.DataValueField = "PracticeId";
            lstPrac.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadAssignPracList(int projectId)
    {
        try
        {
            assignedEditPrac.DataSource = _project.GetPracticeOfProject(projectId);
            assignedEditPrac.DataTextField = "Name";
            assignedEditPrac.DataValueField = "PracticeId";
            assignedEditPrac.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadEditMGRList(int projectId)
    {
        try
        {
            lstBoxMGR.DataSource = _project.GetAllMedicalGroupsOfProject(EnterpriseId, projectId);
            lstBoxMGR.DataTextField = "Name";
            lstBoxMGR.DataValueField = "MedicalGroupId";
            lstBoxMGR.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadEditEnterpriseList(int projectId)
    {
        try
        {
            lstEnt.DataSource = _project.GetEnterpriseListOfProject(UserType, EnterpriseId, projectId);
            lstEnt.DataTextField = "Name";
            lstEnt.DataValueField = "EnterpriseId";
            lstEnt.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadEditPracticeList(int projectId)
    {
        try
        {
            lstEditPrac.DataSource = _project.GetPracticeUserListOfProject(UserType, EnterpriseId, projectId);
            lstEditPrac.DataTextField = "Name";
            lstEditPrac.DataValueField = "PracticeId";
            lstEditPrac.DataBind();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void DynamicFoldersAdding()
    {

        elementTable = new Table();
        elementTable.ID = "tableElement";
        elementTable.ClientIDMode = ClientIDMode.Static;
        Label AddFolder = new Label();
        AddFolder.Text = "Add Folder:";
        TableCell _tablecell = new TableCell();
        _tablecell.Width = 125;
        _tablecell.Controls.Add(AddFolder);

        TableRow _tableRow1 = new TableRow();
        _tableRow1.Controls.Add(_tablecell);

        TextBox txt = new TextBox();
        _tablecell = new TableCell();
        txt.CssClass = "bodytxt-field";
        txt.ID = "Folder1";
        txt.ClientIDMode = ClientIDMode.Static;
        _tablecell.Controls.Add(txt);
        _tableRow1.Cells.Add(_tablecell);

        _tablecell = new TableCell();
        HyperLink addSubFolderHyperLink = new HyperLink();
        addSubFolderHyperLink.ID = "hypSubAddMore" + 1;
        addSubFolderHyperLink.Text = "+ Add Child Folder";
        addSubFolderHyperLink.NavigateUrl = "javascript:GenerateSubFolderRows(" + 1 + ");";
        addSubFolderHyperLink.ClientIDMode = ClientIDMode.Static;
        _tablecell.Controls.Add(addSubFolderHyperLink);
        _tableRow1.Controls.Add(_tablecell);

        elementTable.Controls.Add(_tableRow1);

        Table SubFolderTable = new Table();
        SubFolderTable.ID = "SubFolderTable1";
        SubFolderTable.ClientIDMode = ClientIDMode.Static;
        TableRow SubFolderRow = new TableRow();
        //TableCell SubFolderCell = new TableCell();
        //Label addSubFolder = new Label();
        //addSubFolder.Text = "Sub Folder Name:";
        //SubFolderCell.Controls.Add(addSubFolder);
        //SubFolderCell = new TableCell();
        SubFolderTable.Controls.Add(SubFolderRow);

        _tableRow1 = new TableRow();
        _tablecell = new TableCell();
        _tablecell.ColumnSpan = 3;
        _tablecell.Controls.Add(SubFolderTable);
        _tableRow1.Controls.Add(_tablecell);

        elementTable.Controls.Add(_tableRow1);

        HyperLink addMoreHyperLink = new HyperLink();
        addMoreHyperLink.ID = "hypLinkAddMore" + 1;
        addMoreHyperLink.Text = "+ Add Another Folder";
        addMoreHyperLink.NavigateUrl = "javascript:GenerateFolderRows(" + 1 + ");";
        addMoreHyperLink.ClientIDMode = ClientIDMode.Static;

        TableCell _tableCell = new TableCell();
        _tableCell.ColumnSpan = 2;
        _tableCell.Controls.Add(addMoreHyperLink);

        TableRow _tableRow = new TableRow();
        _tableRow.Controls.Add(_tableCell);

        elementTable.Controls.Add(_tableRow);
        tableDiv.Controls.Add(elementTable);

    }
    #endregion
}