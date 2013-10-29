#region Modification History

//  ******************************************************************************
//  Module        : User Administration
//  Created By    : N/A
//  When Created  : N/A
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                         Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig        02-01-2012      Take over consultant screen from Adil
//  Mirza Fahad Ali Baig        02-01-2012      apply pascal/camel casing
//  Mirza Fahad Ali Baig        02-01-2012      Add Consultant user functionality
//  Mirza Fahad Ali Baig        02-01-2012      Add error handling which was missing in current code
//  Mirza Fahad Ali Baig        02-01-2012      Add logging
//  Mirza Fahad Ali Baig        02-01-2012      Add logging
//  Syed Haris Hassan           25-08-2012      Change Everything
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{

    // NOTE: There is no proper naming in variable/ control id declaration. Need to be fix [02-01-2012]
    public partial class ConsultingUser : System.Web.UI.Page
    {
        #region VARIABLE
        private IQueryable EnterpriseList;

        private ConsultingUserBO consultingUser = new ConsultingUserBO();
        private Security _security = new Security();

        private string userType;
        private int enterpriseId;

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    userType = Session["UserType"].ToString();
                    if (userType == enUserRole.SuperAdmin.ToString())
                    {
                        LoadingProcess();
                    }
                    else
                    {
                        BindGrid();
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void ddlEnterprise_OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                {
                    BindGrid();
                    UpdatePanel.Visible = true;
                }
                else
                {
                    UpdatePanel.Visible = false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void GVData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    Message.Clear("");
                    pnlConsultant.Style.Add("display", "block");
                    hypChangePassword.Style.Add("display", "block");
                    LoadConsultantType();
                    ClearFields();
                    int userId = Convert.ToInt32(e.CommandArgument);
                    GetConsultantById(userId);
                    hdnUserId.Value = userId.ToString();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void GVData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVData.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void btnAddConsultant_Click(object sender, EventArgs e)
        {
            try
            {
                pnlConsultant.Style.Add("display", "block");
                hypChangePassword.Style.Add("display", "none");
                LoadConsultantType();
                ClearFields();
                LoadPracticeList();
                hdnUserId.Value = "0";
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
                int[] SelectIndex = lstBoxPractice.GetSelectedIndices();
                for (int index = 0; index < SelectIndex.Length; index++)
                {
                    ListItem AvailableItem = new ListItem();
                    AvailableItem.Value = lstBoxPractice.Items[SelectIndex[index]].Value;
                    AvailableItem.Text = lstBoxPractice.Items[SelectIndex[index]].Text;
                    lstBoxAssignedPractice.Items.Add(AvailableItem);
                }

                for (int index = lstBoxPractice.Items.Count - 1; index > -1; index--)
                {
                    if (lstBoxPractice.Items[index].Selected == true)
                    {
                        lstBoxPractice.Items[index].Selected = false;
                        lstBoxPractice.Items.RemoveAt(index);
                    }
                }

                txtPassword.Attributes.Add("value", txtPassword.Text);
                btnUploadLogo.Attributes.Remove("disabled");
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
                int[] SelectIndex = lstBoxAssignedPractice.GetSelectedIndices();
                for (int index = 0; index < SelectIndex.Length; index++)
                {
                    ListItem SelectedItems = new ListItem();
                    SelectedItems.Value = lstBoxAssignedPractice.Items[SelectIndex[index]].Value;
                    SelectedItems.Text = lstBoxAssignedPractice.Items[SelectIndex[index]].Text;
                    lstBoxPractice.Items.Add(SelectedItems);

                }
                for (int index = lstBoxAssignedPractice.Items.Count - 1; index > -1; index--)
                {
                    if (lstBoxAssignedPractice.Items[index].Selected == true)
                    {
                        lstBoxAssignedPractice.Items[index].Selected = false;
                        lstBoxAssignedPractice.Items.RemoveAt(index);
                    }
                }

                txtPassword.Attributes.Add("value", txtPassword.Text);
                btnUploadLogo.Attributes.Remove("disabled");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                pnlConsultant.Style.Add("display", "none");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string logoPath = Session["FilePath"] != null ? Session["FilePath"].ToString() : string.Empty;
                List<int> lstPractices = new List<int>();
                bool isFeatured = rbListFeatured.SelectedValue == "1" ? true : false;
                bool isActive = rbListStatus.SelectedValue == "1" ? true : false;

                GetEnterpriseId();                

                foreach (ListItem item in lstBoxAssignedPractice.Items)
                {
                    lstPractices.Add(Convert.ToInt32(item.Value));
                }

                if (hdnUserId.Value != "0")
                {
                    if (!consultingUser.IsEmailAvailableForEdit(Convert.ToInt32(hdnUserId.Value), txtEmail.Text, enterpriseId))
                    {
                        Message.Error("Email is not available.");
                        btnUploadLogo.Attributes.Remove("disabled");
                        return;
                    }

                    if (!consultingUser.IsUserNameAvailableForEdit(Convert.ToInt32(hdnUserId.Value), txtUserName.Text, enterpriseId))
                    {
                        Message.Error("UserName is not available.");
                        btnUploadLogo.Attributes.Remove("disabled");
                        return;
                    }

                    if (consultingUser.UpdateRecord(Convert.ToInt32(hdnUserId.Value), txtLastName.Text, txtFirstName.Text, txtUserName.Text, txtEmail.Text, txtPhone.Text,
                        Convert.ToInt32(ddlConsultantType.SelectedValue), txtServiceArea.Text, isFeatured, isActive, txtWebsite.Text, logoPath, txtOrganization.Text,
                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text, txtZip.Text, lstPractices))
                    {

                        Message.Success("Record Updated Successfully.");
                    }
                    else
                        Message.Error("Record cannot be updated.");
                }
                else
                {
                    if (!consultingUser.IsEmailAvailable(txtEmail.Text, enterpriseId))
                    {
                        Message.Error("Email is not available.");
                        btnUploadLogo.Attributes.Remove("disabled");
                        return;
                    }

                    if (!consultingUser.IsUserNameAvailable(txtUserName.Text, enterpriseId))
                    {
                        Message.Error("UserName is not available.");
                        btnUploadLogo.Attributes.Remove("disabled");
                        return;
                    }

                    if (consultingUser.InsertRecord(enterpriseId, txtLastName.Text, txtFirstName.Text, txtUserName.Text,
                        _security.Encrypt(txtPassword.Text), txtEmail.Text, txtPhone.Text, Convert.ToInt32(ddlConsultantType.SelectedValue), txtServiceArea.Text,
                        isFeatured, isActive, txtWebsite.Text, logoPath, txtOrganization.Text, txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text,
                        txtZip.Text, lstPractices, Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()])))
                    {

                        Message.Success("Record Inserted Successfully.");
                    }
                    else
                        Message.Error("Record cannot be Inserted.");
                }

                BindGrid();
                pnlConsultant.Style.Add("display", "none");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnUserId.Value != "0")
                {
                    string password = txtNewPassword.Text;
                    password = _security.Encrypt(password);

                    UserBO _userAccount = new UserBO();
                    _userAccount.changePassword(Convert.ToInt32(hdnUserId.Value), password);

                    changePwdMessage.Success("Password changed successfully.");
                }
                else
                {
                    changePwdMessage.Error("User Not Found.");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        #region METHODS

        protected void BindGrid()
        {
            try
            {
                GetEnterpriseId();

                List<ConsultingUserDetail> lstConsultants = consultingUser.GetConsultants(enterpriseId);
                GVData.DataSource = lstConsultants;
                GVData.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void LoadingProcess()
        {
            try
            {
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    PracticeBO practiceBO = new PracticeBO();
                    EnterpriseList = practiceBO.GetEnterprises();
                    ddlEnterprise.DataTextField = "Name";
                    ddlEnterprise.DataValueField = "ID";
                    ddlEnterprise.DataSource = EnterpriseList;
                    ddlEnterprise.DataBind();

                    //Add Default item in comboBox
                    ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));

                    UpdatePanel.Visible = false;
                    lblEnterprise.Visible = ddlEnterprise.Visible = true;
                }
                else
                {
                    UpdatePanel.Visible = true;
                    lblEnterprise.Visible = ddlEnterprise.Visible = false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void ClearFields()
        {
            try
            {
                txtLastName.Text = txtUserName.Text = txtFirstName.Text = txtPassword.Text = txtEmail.Text = txtConfirmEmail.Text = txtPhone.Text =
                txtServiceArea.Text = txtWebsite.Text = txtOrganization.Text = txtAddress1.Text = txtAddress2.Text = txtCity.Text = txtState.Text =
                txtZip.Text = string.Empty;

                rbListStatus.SelectedValue = rbListFeatured.SelectedValue = null;
                ddlConsultantType.SelectedValue = "0";

                txtPassword.Enabled = true;
                txtPassword.Attributes.Add("value", "");

                lstBoxPractice.Items.Clear();
                lstBoxAssignedPractice.Items.Clear();

                Message.Clear("");
                Session["FilePath"] = null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void GetConsultantById(int _userId)
        {
            try
            {
                ConsultantAdministrationDetail consultantAdministrationDetail = consultingUser.GetConsultantDetailByUserId(_userId);
                txtLastName.Text = consultantAdministrationDetail.LastName;
                txtUserName.Text = consultantAdministrationDetail.UserName;
                txtFirstName.Text = consultantAdministrationDetail.FirstName;

                txtPassword.Attributes.Add("value", "********");
                txtPassword.Enabled = false;
                btnUploadLogo.Attributes.Remove("disabled");

                txtEmail.Text = txtConfirmEmail.Text = consultantAdministrationDetail.Email;
                txtPhone.Text = consultantAdministrationDetail.Telephone;
                txtServiceArea.Text = consultantAdministrationDetail.ServiceArea;

                txtWebsite.Text = consultantAdministrationDetail.Website;
                txtOrganization.Text = consultantAdministrationDetail.Organization;

                txtAddress1.Text = consultantAdministrationDetail.PrimaryAddress;
                txtAddress2.Text = consultantAdministrationDetail.SecondaryAddress;
                txtCity.Text = consultantAdministrationDetail.City;
                txtState.Text = consultantAdministrationDetail.State;
                txtZip.Text = consultantAdministrationDetail.ZipCode;

                rbListStatus.SelectedValue = consultantAdministrationDetail.IsActive ? "1" : "0";
                rbListFeatured.SelectedValue = consultantAdministrationDetail.Featured ? "1" : "0";
                ddlConsultantType.SelectedValue = consultantAdministrationDetail.ConsultantTypeId.ToString();

                LoadAssignPracticeList(_userId);
                LoadPracticeListByUserId(_userId);
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
                GetEnterpriseId();

                lstBoxPractice.DataSource = consultingUser.GetPracticeList(enterpriseId);
                lstBoxPractice.DataTextField = "Name";
                lstBoxPractice.DataValueField = "ID";
                lstBoxPractice.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void LoadAssignPracticeList(int _userId)
        {
            try
            {
                lstBoxAssignedPractice.DataSource = consultingUser.GetPracticeConsultant(_userId);
                lstBoxAssignedPractice.DataTextField = "Name";
                lstBoxAssignedPractice.DataValueField = "PracticeId";
                lstBoxAssignedPractice.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void LoadPracticeListByUserId(int _userId)
        {
            try
            {
                GetEnterpriseId();

                lstBoxPractice.DataSource = consultingUser.GetPracticeListByUserId(_userId, enterpriseId);
                lstBoxPractice.DataTextField = "Name";
                lstBoxPractice.DataValueField = "ID";
                lstBoxPractice.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void LoadConsultantType()
        {
            try
            {
                List<ConsultantType> lstConsultantType = consultingUser.GetConsultantType();
                ddlConsultantType.DataSource = lstConsultantType;
                ddlConsultantType.DataTextField = "Name";
                ddlConsultantType.DataValueField = "ConsultantTypeId";
                ddlConsultantType.DataBind();
                ddlConsultantType.Items.Insert(0, new ListItem("--- Select ---", "0"));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void GetEnterpriseId()
        {
            try
            {
                userType = Session[enSessionKey.UserType.ToString()].ToString();
                if (userType == enUserRole.SuperAdmin.ToString())    
                    enterpriseId = Convert.ToInt32(ddlEnterprise.SelectedValue);                   
                else
                    enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        #endregion
    }
}