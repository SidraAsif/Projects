#region Modification History

//  ******************************************************************************
//  Module        : Practice
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Adil                                        Enhance functionality for user Administration screen (change the behaviour of provider type)
//  Faizan                  01/30/2012          (Remove duplicate site name)
//  Waqas                   02/22/2012          Email logic
//  Mirza Fahad Ali Baig    02/27/2012          Optimize the code. fix email logic. Remove extra and un-necessary code etc...
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;
using Microsoft;
using System.Text;
using System.Text.RegularExpressions;

namespace BMT.Webforms
{
    public partial class Settings : System.Web.UI.Page
    {
        #region CONSTANTS
        private const int MEDICAL_GROUP_ID = 1;
        private const int DEFAULT_ROLE_ID = (int)enUserRole.User;

        #endregion

        #region VARIABLES
        private PracticeBO _practice = new PracticeBO();
        private AddressBO _address = new AddressBO();
        private SiteBO _site = new SiteBO();
        private UserBO _userAccount = new UserBO();
        private Security _security = new Security();
        private IQueryable _practiceSiteList;
        private List<PracticeSite> _siteList;
        private int enterpriseId;
        private int siteAddressId;
        private int siteId;
        private int userId;
        private string parameter = string.Empty;
        private static System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\s{1,}"); /*1= Number of spaces*/

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int userApplicationId = 0;
                string userType = string.Empty;
                int practiceId = 0;
                if (Session["UserApplicationId"] != null ||
                   Session["UserType"] != null ||
                   Session["PracticeId"] != null ||
                   Session["EnterpriseId"] != null)
                {
                    userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                    userType = Session["UserType"].ToString();
                    practiceId = Convert.ToInt32(Session["PracticeId"]);
                    enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                    CreateEditProject.EnterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                    CreateEditProject.PracticeId = Convert.ToInt32(Session["PracticeId"]);
                    CreateEditProject.MedicalGroupId = Convert.ToInt32(Session["MedicalGroupId"]);
                    CreateEditProject.UserType = userType;

                    CreateEditTemplate.PracticeId = practiceId;
                    CreateEditTemplate.UserType = userType;

                    EditTemp.UserType = userType;

                    ConfigProj.PracticeId = practiceId;
                    ConfigProj.UserType = userType;
                    ConfigProj.EnterpriseId = enterpriseId;
                }
                else
                {
                    SessionHandling _sessionHandling = new SessionHandling();
                    _sessionHandling.ClearSession();
                    Response.Redirect("~/Account/Login.aspx");
                }

                if (Session["TemplateId"] != null)
                {
                    //int templateId = Convert.ToInt32(Session["TemplateId"].ToString());
                    //EditTemp.GetHeader(templateId);
                    //EditTemp.LoadingGrids();

                }

                if (!Page.IsPostBack)
                {

                    if (practiceId != 0)
                    {
                        // ByDefault: Hide all panels
                        pnlChangepwdlink.Style.Add("display", "none");
                        btnAddUser.Style.Add("display", "none");
                        btnAddNewSite.Style.Add("display", "none");
                        btnCreateMORe.Style.Add("display", "none");
                        btncreateProjectMORe.Style.Add("display", "none");
                        // Get static Data from db
                        GetById();

                        // Get practice detail
                        GetPracticeByPracticeId(practiceId);

                        // Get list of available sites
                        GetSitesByPracticeId(practiceId);

                        // Get list of users in current practice
                        GetUserByPracticeId();
                        BindAvailabeSiteList();

                        // Add Default item in DropDownlist "--Select--"
                        AddDefaultItem();
                    }
                    if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                    {
                        // TODO: Remove required validation on current password in Non-SuperUser case
                        rfvtxtcurrentPassword.ValidationGroup = "SuperUser";
                        txtcurrentPassword.CausesValidation = false;
                        txtcurrentPassword.Enabled = false;
                        CreateEditProjectHide.Visible = true;

                        GetPractices();

                        string selectedPractice = ddlPractices.SelectedValue;
                        if (selectedPractice != "0") { pnlSettings.Style.Add("display", "visible"); }
                    }
                    else if (userType == enUserRole.Consultant.ToString() || userType == enUserRole.Consultant.ToString())
                    {
                        // TODO: Add required validation on current password in Non-SuperUser case
                        rfvtxtcurrentPassword.ValidationGroup = "changePassword";
                        txtcurrentPassword.CausesValidation = true;
                        txtcurrentPassword.Enabled = true;

                        hideForConsultant.Style.Add("display", "none");
                        practiceContainer.Style.Add("display", "none");
                        CreateEditProjectHide.Visible = false;

                        GetPractices();

                        string selectedPractice = ddlPractices.SelectedValue;
                        if (selectedPractice != "0") { pnlSettings.Style.Add("display", "visible"); }
                    }
                    else
                    {
                        // TODO: Add required validation on current password in Non-SuperUser case
                        rfvtxtcurrentPassword.ValidationGroup = "changePassword";
                        txtcurrentPassword.CausesValidation = true;
                        txtcurrentPassword.Enabled = true;
                        CreateEditProjectHide.Visible = false;
                        CreateEditProjectsHide.Visible = false;

                        pnlSettings.Style.Add("display", "visible");
                    }
                }
                else
                {

                    if (Request.Params.Get("__EVENTARGUMENT") != null)
                        IsQueryString.Value = Request.Params.Get("__EVENTARGUMENT");
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void ddlPractices_OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    Session["PracticeId"] = ddlPractices.SelectedValue;
                    int practiceId = Convert.ToInt32(Session["PracticeId"]);
                    if (practiceId != 0)
                    {
                        // ByDefault: Hide all panels
                        pnlChangepwdlink.Style.Add("display", "none");
                        btnAddUser.Style.Add("display", "none");
                        btnAddNewSite.Style.Add("display", "none");
                        btnCreateMORe.Style.Add("display", "none");
                        // Get static Data from db
                        GetById();

                        // Get practice detail
                        GetPracticeByPracticeId(practiceId);

                        // Get list of available sites
                        GetSitesByPracticeId(practiceId);

                        // Get list of users in current practice
                        GetUserByPracticeId();
                        BindAvailabeSiteList();

                        // Add Default item in DropDownlist "--Select--"
                        AddDefaultItem();

                        // Show form if SuperUser select the valid practice from the list 
                        pnlSettings.Style.Add("display", "visible");
                        Response.Redirect("~/Webforms/Settings.aspx", false);
                    }
                    else
                    { pnlSettings.Style.Add("display", "none"); }
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
                if (Page.IsPostBack)
                {
                    int practiceId = 0;
                    if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                    {
                        ddlPractices.Enabled = true;
                        GetPracticeByEnterpriseId(Convert.ToInt32(ddlEnterprise.SelectedValue));

                        Session["PracticeId"] = ddlPractices.SelectedValue;
                        practiceId = Convert.ToInt32(Session["PracticeId"]);

                        Session["EnterpriseId"] = ddlEnterprise.SelectedValue;
                        enterpriseId = Convert.ToInt32(Session["EnterpriseId"]);
                    }
                    else
                    {
                        Session["PracticeId"] = practiceId = 0;
                        ddlPractices.SelectedIndex = -1;
                        ddlPractices.Enabled = false;
                    }


                    if (practiceId != 0)
                    {
                        // ByDefault: Hide all panels
                        pnlChangepwdlink.Style.Add("display", "none");
                        btnAddUser.Style.Add("display", "none");
                        btnAddNewSite.Style.Add("display", "none");
                        btnCreateMORe.Style.Add("display", "none");
                        // Get static Data from db
                        GetById();

                        // Get practice detail
                        GetPracticeByPracticeId(practiceId);

                        // Get list of available sites
                        GetSitesByPracticeId(practiceId);

                        // Get list of users in current practice
                        GetUserByPracticeId();
                        BindAvailabeSiteList();

                        // Add Default item in DropDownlist "--Select--"
                        AddDefaultItem();

                        // Show form if SuperUser select the valid practice from the list 
                        pnlSettings.Style.Add("display", "visible");
                        Response.Redirect("~/Webforms/Settings.aspx", false);
                    }
                    else
                    {
                        pnlSettings.Style.Add("display", "none");
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void lbPractice_Click(object sender, EventArgs e)
        {

            SettingTreeNodeChange(enSettingTree.Practice);
        }

        protected void btnPracticeDiscardChanges_Click(object sender, EventArgs e)
        {

            try
            {
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                PracticeMessage.Clear("");
                GetPracticeByPracticeId(practiceId);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void lbUser_Click(object sender, EventArgs e)
        {
            SettingTreeNodeChange(enSettingTree.User);
        }

        protected void lbSite_Click(object sender, EventArgs e)
        {
            SettingTreeNodeChange(enSettingTree.Site);
        }

        protected void lbCreateMORe_Click(object sender, EventArgs e)
        {
            SettingTreeNodeChange(enSettingTree.CreateEditProject);
        }

        protected void lbProjectMORe_Click(object sender, EventArgs e)
        {
            SettingTreeNodeChange(enSettingTree.CreateProject);
        }

        protected void btnAddNewSite_Click(object sender, EventArgs e)
        {
            hdnSiteId.Value = "0";
            hdnsiteAddressId.Value = "0";

            siteId = 0;
            siteAddressId = 0;

            Session["_IsSiteEdit"] = false;
            ClearSiteFields();
            btnAddUser.Style.Add("display", "none");
            btnAddNewSite.Style.Add("display", "visible");
            btnCreateMORe.Style.Add("display", "none");
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {

            userId = 0;
            hdnUserId.Value = userId.ToString();
            Session["_isUserEdit"] = false;
            clearUserFields();
            btnAddNewSite.Style.Add("display", "none");
            btnAddUser.Style.Add("display", "visible");
            btnCreateMORe.Style.Add("display", "none");
        }

        protected void btnSavePractice_Click(object sender, EventArgs e)
        {

            try
            {
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                _address = new AddressBO();
                string primaryAddress = txtPracticeAddress1.Text.Trim();
                string secondaryAddress = txtPracticeAddress2.Text.Trim();
                string city = txtPracticeCity.Text.Trim();
                string state = txtPracticeState.Text.Trim();
                string zipCode = txtPracticeZipCode.Text.Trim();
                string phone = txtPracticePhone.Text.Trim();
                string mobile = string.Empty;
                string fax = txtPracticeFax.Text.Trim();
                string email = txtPracticeContactEmail.Text.Trim().ToLower().ToString(); ;

                // set properties
                _address.AddressId = Convert.ToInt32(Session["practiceAddressId"].ToString());
                _address.PrimaryAddress = primaryAddress;
                _address.SecondaryAddress = secondaryAddress;
                _address.City = city;
                _address.State = state;
                _address.ZipCode = zipCode;
                _address.Phone = phone;
                _address.Mobile = mobile;
                _address.Fax = fax;
                _address.Email = email;

                // Save/Update User's practice Address
                Session["practiceAddressId"] = _address.SaveAddress();

                _practice = new PracticeBO();
                string practiceName = txtPracticeName.Text.Trim();
                int practiceSizeId = Convert.ToInt32(ddlPracticeSize.SelectedValue);
                int practicePrimarySpecialityId = Convert.ToInt32(ddlPracticePrimarySpeciality.SelectedValue);
                DateTime createdDate = System.DateTime.Now;
                int createdBy = userApplicationId;
                string contactName = txtPracticeContactName.Text.Trim();
                bool isCorporate = corporateTypeYes.Checked;
                int practiceSiteId = Convert.ToInt32(ddlSiteName.SelectedValue);
                if (corporateTypeYes.Checked)
                {
                    ddlSiteName.Enabled = true;
                }
                else
                {
                    ddlSiteName.Enabled = false;
                }
                // set properties
                _practice.PracticeId = practiceId;
                _practice.MedicalGroupId = _practice.GetMedicalGroupIdByPracticeId(practiceId);
                _practice.AddressId = Convert.ToInt32(Session["practiceAddressId"].ToString());
                _practice.Name = practiceName;
                _practice.PracticeSizeId = practiceSizeId;
                _practice.SpecialityId = practicePrimarySpecialityId;
                _practice.CreatedDate = createdDate;
                _practice.CreatedBy = userApplicationId;
                _practice.ContactName = contactName;
                _practice.PracticeSiteId = practiceSiteId;
                hdnPracticeSiteId.Value = practiceSiteId.ToString();
                _practice.IsCorporate = isCorporate;

                // Save/update practice
                practiceId = _practice.SavePractice();

                PracticeMessage.Success("Practice information saved successfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                PracticeMessage.Error("Practice information couldn't be saved.");
            }

        }

        protected void gvSites_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {
                        pnlSiteForm.Style.Add("display", "visible");
                        ClearSiteFields();
                        Session["_IsSiteEdit"] = true;
                        int index = Convert.ToInt32(e.CommandArgument);
                        siteId = Convert.ToInt32(gvSites.DataKeys[index].Value);
                        hdnSiteId.Value = siteId.ToString();
                        GetSiteBySiteId(siteId);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void btnSiteSave_Click(object sender, EventArgs e)
        {
            try
            {
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                _address = new AddressBO();
                string primaryAddress = txtSiteAddress1.Text.Trim();
                string secondaryAddress = txtSiteAddress2.Text.Trim();
                string city = txtSiteCity.Text.Trim();
                string state = txtSiteState.Text.Trim();
                string zipCode = txtSiteZipCode.Text.Trim();
                string phone = txtSitePhone.Text.Trim();
                string mobile = string.Empty;
                string fax = txtSiteFax.Text.Trim();
                string email = txtSiteContactEmail.Text.Trim().ToLower().ToString();

                // set properties
                siteAddressId = hdnsiteAddressId.Value != string.Empty ? Convert.ToInt32(hdnsiteAddressId.Value) : 0;
                _address.AddressId = siteAddressId;
                _address.PrimaryAddress = primaryAddress;
                _address.SecondaryAddress = secondaryAddress;
                _address.City = city;
                _address.State = state;
                _address.ZipCode = zipCode;
                _address.Phone = phone;
                _address.Mobile = mobile;
                _address.Fax = fax;
                _address.Email = email;

                // save/update site address
                siteAddressId = _address.SaveAddress();
                hdnsiteAddressId.Value = siteAddressId.ToString();

                _site = new SiteBO();
                string name = txtSiteName.Text.Trim();
                int sitePrimarySpecialityId = Convert.ToInt32(ddlSitePrimarySpeciality.SelectedValue);
                string siteGroupNPI = txtSiteNPI.Text.Trim();
                int siteNumberofProvider = Convert.ToInt32(txtSiteNumberOfProvider.Text);
                bool isMainSite = chkboxMainSite.Checked;
                DateTime createdDate = System.DateTime.Now;
                int createdBy = userApplicationId;
                string contactName = txtSiteContactName.Text.Trim();

                // set properties
                siteId = hdnSiteId.Value != string.Empty ? Convert.ToInt32(hdnSiteId.Value) : 0;
                _site.SiteId = Convert.ToInt32(siteId);
                _site.PracticeId = practiceId;
                _site.AddressId = Convert.ToInt32(hdnsiteAddressId.Value);
                _site.Name = name;
                _site.SitePrimarySpecialityId = sitePrimarySpecialityId;
                _site.SiteGroupNPI = siteGroupNPI;
                _site.NumberOfProvider = siteNumberofProvider;
                _site.IsMainSite = isMainSite;
                _site.CreatedDate = createdDate;
                _site.CreatedBy = createdBy;
                _site.ContactName = contactName;

                bool siteResponse = _site.SavePracticeSite();
                if (!siteResponse)
                {
                    SiteMessge.Error("Site name already exists");
                    return;
                }
                else
                {
                    GetSitesByPracticeId(practiceId);
                    BindAvailabeSiteList();

                    if (!(bool)Session["_IsSiteEdit"])
                        SiteMessge.Success("Site added succcessfully.");
                    else
                        SiteMessge.Success("Site updated succcessfully.");

                    hdnSiteId.Value = "0";
                    hdnsiteAddressId.Value = "0";

                    siteId = 0;
                    siteAddressId = 0;

                    Session["_IsSiteEdit"] = false;
                    ClearSiteFields();

                    pnlSiteForm.Style.Add("display", "none");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                SiteMessge.Error("Practice Site conuldn't be saved.");
            }
        }

        protected void btnSiteDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                int response = (int)enSiteCheck.Invalid;
                if ((bool)Session["_IsSiteEdit"])
                {
                    siteId = hdnSiteId.Value != string.Empty ? Convert.ToInt32(hdnSiteId.Value) : 0;
                    if (IsSiteNotCorpDelete(siteId))
                    {
                        if (IsDeletingCorporateSite(siteId))
                        {
                            SiteMessge.Error("This site is selected as corporate.Select the site as non coroprate for deleting the sites.");
                            return;
                        }
                        else
                        {
                           response = DeleteSiteBySiteId(siteId);
         
                        }
                    }
                    else
                    {
                        SiteMessge.Error("This practice is selected as corporate.Select the practice as non coroprate for deleting the sites.");
                        return;
                    }
                }
                else
                {
                    SiteMessge.Info("No site selected. Please select the site and try again.");
                    return;
                }

                if (response == (int)enSiteCheck.Pass)
                {
                    SiteMessge.Success("Site deleted successfully.");
                    siteId = 0;
                    siteAddressId = 0;

                    hdnSiteId.Value = "0";
                    hdnsiteAddressId.Value = "0";

                    Session["_IsSiteEdit"] = false;
                    ClearSiteFields();
                    GetSitesByPracticeId(practiceId);
                }
                else if (response == (int)enSiteCheck.HasParent)
                {
                    SiteMessge.Warning("Site couldn't be deleted because it is in use.");
                    return;
                }
                else if (response == (int)enSiteCheck.NoMultiple)
                {
                    SiteMessge.Warning("You must have more than one site to delete the selected site");
                    return;
                }
                else
                {
                    SiteMessge.Warning("Site couldn't be deleted, Please select the site and try again.");
                    return;
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);

                // HACK: Temporary fix to fix the Insert/update feature in case of deletion faliure.
                SiteMessge.Warning("Site couldn't be deleted because it is in use.");
                siteId = 0;
                siteAddressId = 0;

                hdnSiteId.Value = "0";
                hdnsiteAddressId.Value = "0";

                Session["_IsSiteEdit"] = false;
                ClearSiteFields();
                pnlSiteForm.Style.Add("display", "none");
            }
        }

        protected void btnSiteDiscardChanges_Click(object sender, EventArgs e)
        {
            SiteMessge.Clear("");
            ClearSiteFields();
            siteId = 0;
            hdnSiteId.Value = siteId.ToString();

            siteAddressId = 0;
            hdnsiteAddressId.Value = siteAddressId.ToString();

            Session["_IsSiteEdit"] = false;
            pnlSiteForm.Style.Add("display", "none");
        }

        protected void btnUserDiscardChanges_Click(object sender, EventArgs e)
        {
            UserMessage.Clear("");
            clearUserFields();
            userId = 0;
            hdnUserId.Value = userId.ToString();
            Session["_isUserEdit"] = false;
            pnlUserForm.Style.Add("display", "none");
        }

        protected void btnUserSave_Click(object sender, EventArgs e)
        {
            try
            {
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                Email _emailSender = new Email();
                // Get MaxId to make username unqiue
                int userMaxId = GetMaxId();

                _userAccount = new UserBO();
                string password = Session["userOldPassword"].ToString();
                string orignolPassword = password;
                if (password == string.Empty)
                    password = orignolPassword = _security.GenerateRandomPassword();

                password = _security.Encrypt(password);

                string userFirstPart = string.Empty;
                string firstName = txtUserFirstName.Text.Trim();
                string lastName = txtUserLastName.Text.Trim();
                string userSecondPart = lastName;

                if (firstName.Length == 1)
                    userFirstPart = firstName;
                else
                    userFirstPart = firstName.Remove(1);

                string email = txtUserEmail.Text.Trim().ToLower().ToString();
                string username = userFirstPart + userSecondPart + Convert.ToString(userMaxId);
                username = regex.Replace(username.Trim(), "");

                bool isActive = Convert.ToInt32(rblUserDeactivate.SelectedValue) == 1 ? true : false;
                DateTime lastActivityDate = System.DateTime.Now;

                int primaryPracticeSiteId = Convert.ToInt32(ddlUserPrimarySite.SelectedValue);
                int providerTypeId = Convert.ToInt32(rblProviderType.SelectedValue);

                int? credentialId = null;
                int? specialityId = null;

                if (rblProviderType.SelectedItem.Text == "PCP" || rblProviderType.SelectedItem.Text == "Specialist")
                {
                    if (ddlUserCredential.SelectedIndex != 0 || ddlUserSpeciality.SelectedIndex != 0)
                    {
                        ddlUserCredential.Enabled = ddlUserSpeciality.Enabled = true;
                        credentialId = Convert.ToInt32(ddlUserCredential.SelectedValue);
                        specialityId = Convert.ToInt32(ddlUserSpeciality.SelectedValue);
                    }
                    else
                    {
                        if (ddlUserCredential.SelectedIndex == 0 || ddlUserSpeciality.SelectedIndex == 0)
                        {
                            ddlUserCredential.Enabled = ddlUserSpeciality.Enabled = true;
                            credentialId = Convert.ToInt32(null);
                            specialityId = Convert.ToInt32(null);
                        }
                    }
                }
                else
                {
                    if (rblProviderType.SelectedItem.Text == "Non Provider")
                    {
                        if (ddlUserCredential.SelectedIndex == 0 || ddlUserSpeciality.SelectedIndex == 0)
                        {
                            ddlUserCredential.Enabled = false;
                            ddlUserSpeciality.Enabled = false;
                            credentialId = Convert.ToInt32(null);
                            specialityId = Convert.ToInt32(null);
                        }
                        else
                        {
                            ddlUserCredential.Enabled = false;
                            ddlUserSpeciality.Enabled = false;
                            credentialId = Convert.ToInt32(null);
                            specialityId = Convert.ToInt32(null);
                        }
                    }

                }
                int practiceRoleId = Convert.ToInt32(ddlUserRole.SelectedValue);

                DateTime BPRPExpireDate = txtUserBPRP.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(txtUserBPRP.Text);
                DateTime DRPExpireDate = txtUserDRP.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(txtUserDRP.Text);
                DateTime HSRPExpireDate = txtUserHSRP.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(txtUserHSRP.Text);
                DateTime createdDate = System.DateTime.Now;

                // set properties
                userId = hdnUserId.Value != string.Empty ? Convert.ToInt32(hdnUserId.Value) : 0;
                _userAccount.UserId = userId;
                _userAccount.UserName = username;
                _userAccount.Password = password;
                _userAccount.Email = email;
                _userAccount.IsActive = isActive;
                _userAccount.LastActivityDate = lastActivityDate;
                _userAccount.RoleId = DEFAULT_ROLE_ID;
                _userAccount.FistName = firstName;
                _userAccount.LastName = lastName;
                _userAccount.ProviderTypeId = providerTypeId;
                _userAccount.CredentialId = credentialId;
                _userAccount.SpecialityId = specialityId;
                _userAccount.PracticeId = practiceId;
                _userAccount.CreatedDate = createdDate;
                _userAccount.CreatedBy = userApplicationId;
                _userAccount.BPRPExpireDate = BPRPExpireDate;
                _userAccount.DRPExpireDate = DRPExpireDate;
                _userAccount.HSRPExpireDate = HSRPExpireDate;
                _userAccount.PracticeSiteId = primaryPracticeSiteId;
                _userAccount.PracticeRoleId = practiceRoleId;

                bool response = SaveUser();

                if (!response)
                {
                    UserMessage.Error("This email is already in use.");
                    return;
                }
                else
                {
                    GetUserByPracticeId();

                    if (!(bool)Session["_isUserEdit"])
                    {
                        string msg = "User added successfully.Login details has been sent to " + email + ".";
                        UserMessage.Success(msg);
                        try
                        {
                            string projectName = Util.GetSystemProjectName(enterpriseId);
                            string companyName = Util.GetSystemCompany(enterpriseId);
                            string body = Util.GetSystemBody(enterpriseId);
                            _emailSender.SendUserCredentialDetails(email, username, orignolPassword, projectName, companyName, body, userId);
                        }
                        catch
                        {

                        }
                    }
                    else
                        UserMessage.Success("User updated successfully.");

                    Session["_isUserEdit"] = false;
                    userId = 0;
                    hdnUserId.Value = userId.ToString();
                    clearUserFields();

                    pnlUserForm.Style.Add("display", "none");
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                UserMessage.Error("User information couldn't be saved");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string currentPassord = txtcurrentPassword.Text;
                string userType = Session["UserType"].ToString();
                if (currentPassord != Session["userOldPassword"].ToString() && !(userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString()))
                {
                    changePwdMessage.Error("Current password is incorrect.");
                    return;
                }

                string password = txtNewPassword.Text;
                password = _security.Encrypt(password);
                userId = hdnUserId.Value != string.Empty ? Convert.ToInt32(hdnUserId.Value) : 0;
                ChangePassword(userId, password);
                changePwdMessage.Success("Password changed successfully.");
                Session["userOldPassword"] = txtNewPassword.Text;
                return;

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                changePwdMessage.Error("Password couldn't be changed.");
                return;
            }
        }

        protected void gvUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {
                        pnlUserForm.Style.Add("display", "visible");

                        clearUserFields();
                        Session["_isUserEdit"] = true;
                        BindAvailabeSiteList();
                        int index = Convert.ToInt32(e.CommandArgument);
                        userId = Convert.ToInt32(gvUsers.DataKeys[index].Value);
                        hdnUserId.Value = userId.ToString();
                        GetUserByUserId(userId);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void btnCancelChangePwd_Click(object sender, EventArgs e)
        {
            try
            {
                changePwdMessage.Clear("");
                txtcurrentPassword.Text = txtNewPassword.Text = txtConfirmNewPassword.Text = string.Empty;

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void gvSites_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                gvSites.PageIndex = e.NewPageIndex;
                GetSitesByPracticeId(practiceId);
                BindAvailabeSiteList();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                gvUsers.PageIndex = e.NewPageIndex;
                GetUserByPracticeId();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnCreateMORe_Click(object sender, EventArgs e)
        {
            userId = 0;
            hdnUserId.Value = userId.ToString();
            btnAddNewSite.Style.Add("display", "none");
            btnAddUser.Style.Add("display", "none");
            btnCreateMORe.Style.Add("display", "visible");
            btncreateProjectMORe.Style.Add("display", "none");
            CreateEditTemplate.ClearTemplateFields();
            //((HiddenField)(CreateEditTemplate.FindControl("hdnIsCreate"))).Value = "true";
            CreateEditTemplate.IsCreate = "true";
        }


        protected void btncreateProjectMORe_Click(object sender, EventArgs e)
        {

            userId = 0;
            hdnUserId.Value = userId.ToString();
            btnAddNewSite.Style.Add("display", "none");
            btnAddUser.Style.Add("display", "none");
            btnCreateMORe.Style.Add("display", "none");
            btncreateProjectMORe.Style.Add("display", "visible");
            CreateEditProject.ClearTemplateFields();
            CreateEditProject.IsCreate = "True";
        }

        protected void lbConfigureMORe_Click(object sender, EventArgs e)
        {
            SettingTreeNodeChange(enSettingTree.MyProject);
        }

        #endregion

        #region FUNCTIONS
        private void GetPractices()
        {
            try
            {
                IQueryable _enterpriseList;
                IQueryable _practiceList;
                string userType = Session["UserType"].ToString();
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.Consultant.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    if (userType == enUserRole.SuperAdmin.ToString())
                    {
                        lblEnterprise.Visible = ddlEnterprise.Visible = lblPractices.Visible = ddlPractices.Visible = true;

                        _practice = new PracticeBO();
                        _enterpriseList = _practice.GetEnterprises();
                        ddlEnterprise.DataTextField = "Name";
                        ddlEnterprise.DataValueField = "ID";
                        ddlEnterprise.DataSource = _enterpriseList;
                        ddlEnterprise.DataBind();

                        //Add Default item in comboBox
                        ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Enabled = false;

                        if (enterpriseId > 0)
                        {
                            ddlEnterprise.SelectedIndex = ddlEnterprise.Items.IndexOf(ddlEnterprise.Items.FindByValue(enterpriseId.ToString()));
                            if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                            {
                                ddlPractices.Enabled = true;
                                GetPracticeByEnterpriseId(Convert.ToInt32(ddlEnterprise.SelectedValue));
                            }
                        }
                    }
                    else
                    {
                        lblPractices.Visible = ddlPractices.Visible = true;
                        _practice = new PracticeBO();
                        _practiceList = _practice.GetPractices(userApplicationId, userType, enterpriseId);
                        ddlPractices.DataTextField = "Name";
                        ddlPractices.DataValueField = "ID";
                        ddlPractices.DataSource = _practiceList;
                        ddlPractices.DataBind();

                        // Add Default item in comboBox
                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue(practiceId.ToString()));
                    }



                }
                else { ddlPractices.Items.Clear(); lblPractices.Visible = ddlPractices.Visible = false; }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                PracticeMessage.Error("System failed to load the practices. Please contact your site administrator.");
            }
        }

        private void GetById()
        {
            try
            {
                // Fetching static data from Database
                // Practice Role list
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                PracticeRoleBO _practiceRole = new PracticeRoleBO();
                PracticeSizeBO _practiceSize = new PracticeSizeBO();
                SpecialityBO _specialty = new SpecialityBO();
                CredentialBO _credntial = new CredentialBO();
                ProviderTypeBO _providerType = new ProviderTypeBO();
                IQueryable _practiceSizeList;
                IQueryable _specialityList;
                IQueryable _credentialList;
                IQueryable _practiceRoleList;
                IQueryable _providerTypeList;
                _practiceRole.GetPracticeRoleById();
                _practiceRoleList = _practiceRole.PracticeRoleList;

                // Practice Size list
                _practiceSize.GetPracticeSizeById();
                _practiceSizeList = _practiceSize.PracticeSizeList;

                // Speciality list
                _specialty.GetSpecialityById();
                _specialityList = _specialty.SpecialityList;

                // List of available credentials
                _credntial.GetCredentialById();
                _credentialList = _credntial.CredentialList;

                // Available provider type
                _providerType.GetProviderTypeById();
                _providerTypeList = _providerType.ProviderTypeList;

                _site.GetSitesByPracticeId(practiceId);
                _practiceSiteList = _site.PracticeSiteList;

                // Binding Data
                ddlPracticeSize.DataTextField = ddlPracticePrimarySpeciality.DataTextField =
                    ddlSitePrimarySpeciality.DataTextField = ddlUserSpeciality.DataTextField = ddlUserCredential.DataTextField =
                   rblProviderType.DataTextField = ddlUserRole.DataTextField = ddlSiteName.DataTextField = "Name";

                ddlPracticeSize.DataValueField = ddlPracticePrimarySpeciality.DataValueField =
                    ddlSitePrimarySpeciality.DataValueField = ddlUserSpeciality.DataValueField = ddlUserCredential.DataValueField =
                    rblProviderType.DataValueField = ddlUserRole.DataValueField = "ID";

                ddlSiteName.DataValueField = "PracticeSiteId";

                ddlPracticeSize.DataSource = _practiceSizeList;
                ddlPracticePrimarySpeciality.DataSource = ddlSitePrimarySpeciality.DataSource = ddlUserSpeciality.DataSource = _specialityList;
                ddlUserCredential.DataSource = _credentialList;
                rblProviderType.DataSource = _providerTypeList;
                ddlUserRole.DataSource = _practiceRoleList;
                ddlSiteName.DataSource = _practiceSiteList;

                ddlPracticeSize.DataBind();
                ddlPracticePrimarySpeciality.DataBind();
                ddlSitePrimarySpeciality.DataBind();
                ddlUserSpeciality.DataBind();
                ddlUserCredential.DataBind();
                rblProviderType.DataBind();
                ddlUserRole.DataBind();
                ddlSiteName.DataBind();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void BindAvailabeSiteList()
        {
            // Bind Site list in dropdownlist
            ddlUserPrimarySite.DataTextField = "Name";
            ddlUserPrimarySite.DataValueField = "PracticeSiteId";
            int PracticeId = Convert.ToInt32(Session["PracticeId"]);
            _siteList = _site.GetSiteListByPracticeId(PracticeId);
            ddlUserPrimarySite.DataSource = _siteList;
            ddlUserPrimarySite.DataBind();

            string option = "0,--Select--|";
            foreach (PracticeSite site in _siteList)
            {
                option += site.PracticeSiteId + "," + site.Name + "|";
            }
            hiddenPracticeSitesList.Value = option;

            ddlUserPrimarySite.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void AddDefaultItem()
        {
            ddlPracticePrimarySpeciality.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPracticeSize.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlSitePrimarySpeciality.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlUserCredential.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlUserSpeciality.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlUserRole.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlSiteName.Items.Insert(0, new ListItem("--Select--", "0"));

        }

        private void GetPracticeByPracticeId(int PracticeId)
        {
            try
            {
                // Get practice detail by practice Id
                _practice = null;
                _practice = new PracticeBO();
                _practice.GetPracticeByPracticeId(PracticeId);

                PracticeId = _practice.PracticeId;
                txtPracticeName.Text = _practice.Name;
                Session["practiceAddressId"] = _practice.AddressId;

                txtPracticeAddress1.Text = _practice.PrimaryAddress;
                txtPracticeAddress2.Text = _practice.SecondaryAddress;
                txtPracticeCity.Text = _practice.City;
                txtPracticeState.Text = _practice.State;
                txtPracticeZipCode.Text = _practice.ZipCode;
                txtPracticePhone.Text = _practice.Phone;
                txtPracticeFax.Text = _practice.Fax;
                txtPracticeContactEmail.Text = txtConfirmPracticeContactEmail.Text = _practice.Email;
                txtPracticeContactName.Text = _practice.ContactName;
                if (_practice.IsCorporate)
                {
                    corporateTypeYes.Checked = true;
                    corporateTypeNo.Checked = false;
                    ddlSiteName.Enabled = true;
                    CorpMessage.Style.Add("display", "block");
                }
                else
                {
                    corporateTypeYes.Checked = false;
                    corporateTypeNo.Checked = true;
                    ddlSiteName.Enabled = false;
                    CorpMessage.Style.Add("display", "none");
                }

                ddlPracticeSize.SelectedIndex = ddlPracticeSize.Items.IndexOf(ddlPracticeSize.Items.FindByValue(Convert.ToString(_practice.PracticeSizeId)));
                ddlSiteName.SelectedIndex = ddlSiteName.Items.IndexOf(ddlSiteName.Items.FindByValue(Convert.ToString(_practice.PracticeSiteId)));
                ddlPracticePrimarySpeciality.SelectedIndex = ddlPracticePrimarySpeciality.Items.IndexOf(ddlPracticePrimarySpeciality.Items.FindByValue(Convert.ToString(_practice.SpecialityId)));
            }
            catch (Exception exception)
            {
                string exceptionMessage = exception.StackTrace;
            }
        }

        private void GetSitesByPracticeId(int PracticeId)
        {
            try
            {
                _siteList = _site.GetSiteListByPracticeId(PracticeId);
                if (_siteList.Count > 2)
                {
                    corporateType.Visible = true;
                    siteSelector.Visible = true;
                    btnCorporateSave.Attributes.Remove("class");
                    btnPracticeSave.Attributes.Add("class", "practiceSave");
                }
                else
                {
                    corporateType.Visible = false;
                    siteSelector.Visible = false;
                    btnPracticeSave.Attributes.Remove("class");
                    btnCorporateSave.Attributes.Add("class", "practiceSave");

                }
                _site.GetSitesByPracticeId(PracticeId);
                _practiceSiteList = _site.PracticeSiteList;

                gvSites.DataSource = _practiceSiteList;
                gvSites.DataBind();

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        private void GetSiteBySiteId(int SiteId)
        {
            _site = null;
            _site = new SiteBO();

            _site.GetSiteBySiteId(SiteId);

            SiteId = _site.SiteId;
            siteAddressId = _site.AddressId;
            hdnsiteAddressId.Value = siteAddressId.ToString();

            txtSiteName.Text = _site.Name;
            txtSiteNumberOfProvider.Text = Convert.ToString(_site.NumberOfProvider);
            txtSiteNPI.Text = _site.SiteGroupNPI;
            chkboxMainSite.Checked = _site.IsMainSite;

            txtSiteAddress1.Text = _site.PrimaryAddress;
            txtSiteAddress2.Text = _site.SecondaryAddress;
            txtSiteCity.Text = _site.City;
            txtSiteState.Text = _site.State;
            txtSiteZipCode.Text = _site.ZipCode;
            txtSitePhone.Text = _site.Phone;
            txtSiteFax.Text = _site.Fax;
            txtSiteContactEmail.Text = txtConfrmSiteContactEmail.Text = _site.Email;
            txtSiteContactName.Text = _site.ContactName;

            ddlSitePrimarySpeciality.SelectedIndex =
                ddlSitePrimarySpeciality.Items.IndexOf(ddlSitePrimarySpeciality.Items.FindByValue(Convert.ToString(_site.SitePrimarySpecialityId)));

        }

        private int DeleteSiteBySiteId(int siteId)
        {

            try
            {
                int response = _site.DeleteSiteBySiteId(siteId);
                return response;

            }
            catch (Exception exception)
            {
                throw exception;

            }
        }

        protected int GetMaxId()
        {
            try
            {
                // Get Max Id to add new User
                int userMaxId = _userAccount.GetMaxId();
                return userMaxId;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        protected void GetUserByPracticeId()
        {

            try
            {
                // Get users list by practice Id
                int practiceId = Convert.ToInt32(Session["PracticeId"]);
                IQueryable _userList;
                _userAccount.GetUserByPracticeId(practiceId);
                _userList = _userAccount.UserList;

                gvUsers.DataSource = _userList;
                gvUsers.DataBind();
            }
            catch (Exception exception) { throw exception; }
        }

        protected void GetUserByUserId(int _userId)
        {
            try
            {
                _userAccount = null;
                _userAccount = new UserBO();

                pnlChangepwdlink.Style.Add("display", "visible");

                // Get User Detail by Id
                _userAccount.GetUserByUserId(_userId);

                // Fetching information to display
                txtUserName.Text = _userAccount.UserName;
                Session["userOldPassword"] = _security.Decrypt(_userAccount.Password);
                txtUserPassword.Text = "***********"; // Only for display
                txtUserEmail.Text = txtConfrmUserEmail.Text = _userAccount.Email;
                rblUserDeactivate.SelectedValue = _userAccount.IsActive.ToString();

                txtUserFirstName.Text = _userAccount.FistName;
                txtUserLastName.Text = _userAccount.LastName;

                ddlUserCredential.SelectedIndex =
                ddlUserCredential.Items.IndexOf(ddlUserCredential.Items.FindByValue(Convert.ToString(_userAccount.CredentialId)));

                ddlUserPrimarySite.SelectedIndex =
                ddlUserPrimarySite.Items.IndexOf(ddlUserPrimarySite.Items.FindByValue(Convert.ToString(_userAccount.PracticeSiteId)));

                ddlUserSpeciality.SelectedIndex =
                ddlUserSpeciality.Items.IndexOf(ddlUserSpeciality.Items.FindByValue(Convert.ToString(_userAccount.SpecialityId)));

                rblUserDeactivate.SelectedIndex = _userAccount.IsActive == true ? 0 : 1;

                rblProviderType.SelectedIndex =
                rblProviderType.Items.IndexOf(rblProviderType.Items.FindByValue(Convert.ToString(_userAccount.ProviderTypeId)));

                txtUserBPRP.Text = String.Format("{0:MM/dd/yyyy}", _userAccount.BPRPExpireDate) == "01/01/0001" ? "" : String.Format("{0:MM/dd/yyyy}", _userAccount.BPRPExpireDate);
                txtUserDRP.Text = String.Format("{0:MM/dd/yyyy}", _userAccount.DRPExpireDate) == "01/01/0001" ? "" : String.Format("{0:MM/dd/yyyy}", _userAccount.DRPExpireDate);
                txtUserHSRP.Text = String.Format("{0:MM/dd/yyyy}", _userAccount.HSRPExpireDate) == "01/01/0001" ? "" : String.Format("{0:MM/dd/yyyy}", _userAccount.HSRPExpireDate);

                if (txtUserBPRP.Text != "")
                    chkBoxUserBPRP.Checked = true;
                if (txtUserDRP.Text != "")
                    chkBoxUserDRP.Checked = true;
                if (txtUserHSRP.Text != "")
                    chkBoxUserHSRP.Checked = true;

                ddlUserRole.SelectedIndex =
                ddlUserRole.Items.IndexOf(ddlUserRole.Items.FindByValue(Convert.ToString(_userAccount.PracticeRoleId)));

            }
            catch (Exception exception) { throw exception; }

        }

        protected bool SaveUser()
        {
            try
            {
                bool IsEmailValid = _userAccount.IsEmailValid(enterpriseId);

                if (IsEmailValid)
                    return _userAccount.SaveUser();
                else
                    return false;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        protected void SettingTreeNodeChange(enSettingTree SettingTreeNode)
        {

            try
            {
                // Clear All Message on Node selection
                UserMessage.Clear("");
                PracticeMessage.Clear("");
                SiteMessge.Clear("");

                // Remove active node class
                lbPractice.CssClass = lbSite.CssClass = lbUser.CssClass = "";

                // Apply active class on current Node
                if (SettingTreeNode == enSettingTree.Practice)
                    lbPractice.CssClass = "active";
                else if (SettingTreeNode == enSettingTree.User)
                    lbUser.CssClass = "active";
                else if (SettingTreeNode == enSettingTree.Site)
                    lbSite.CssClass = "active";
                else if (SettingTreeNode == enSettingTree.CreateEditProject)
                    lbCreateMORe.CssClass = "active";
                else if (SettingTreeNode == enSettingTree.MyProject)
                    lbConfigureMORe.CssClass = "active";
                else
                    lbPractice.CssClass = "active";

                // Disable edit mode
                Session["_IsSiteEdit"] = false;
                Session["_isUserEdit"] = false;
                siteId = userId = 0;

            }
            catch
            { }

        }

        protected void ClearSiteFields()
        {

            txtSiteAddress1.Text = txtSiteAddress2.Text = txtSiteCity.Text = txtSiteContactEmail.Text = txtConfrmSiteContactEmail.Text =
               txtSiteFax.Text = txtSiteName.Text = txtSiteNPI.Text = txtSitePhone.Text = txtSiteState.Text = txtSiteZipCode.Text =
               txtSiteNumberOfProvider.Text = txtSiteContactName.Text = string.Empty;
            int id = 0;
            ddlSitePrimarySpeciality.SelectedValue = id.ToString();
        }

        protected void clearUserFields()
        {

            txtUserName.Text = txtUserPassword.Text = txtUserBPRP.Text = txtUserDRP.Text =
                txtUserEmail.Text = txtConfrmUserEmail.Text = txtUserFirstName.Text = txtUserHSRP.Text = txtUserLastName.Text = string.Empty;
            Session["userOldPassword"] = "";
            chkBoxUserBPRP.Checked = chkBoxUserDRP.Checked = chkBoxUserHSRP.Checked = false;
            pnlChangepwdlink.Style.Add("display", "none");
        }

        protected void ChangePassword(int _userId, string password)
        {
            try
            {
                _userAccount.changePassword(_userId, password);
            }
            catch (Exception exception) { throw exception; }
        }

        private void GetPracticeByEnterpriseId(int enterpriseId)
        {
            int practiceId = Convert.ToInt32(Session["PracticeId"]);
            _practice = new PracticeBO();
            IQueryable _practiceList;
            _practiceList = _practice.GetPracticesByEnterpriseId(enterpriseId);

            ddlPractices.DataTextField = "Name";
            ddlPractices.DataValueField = "ID";
            ddlPractices.DataSource = _practiceList;
            ddlPractices.DataBind();

            ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue(practiceId.ToString()));
        }

        public bool IsSiteNotCorpDelete(int SiteId)
        {
            try
            {
                CorporateElementSubmissionBO _corpSite = new CorporateElementSubmissionBO();
                return _corpSite.IsSiteNotCorpDelete(SiteId);
            }
            catch (Exception exception) { throw exception; }
        }

        public bool IsDeletingCorporateSite(int siteId)
        {
            try
            {
                CorporateElementSubmissionBO _corpSite = new CorporateElementSubmissionBO();
                return _corpSite.IsDeletingCorporateSite(siteId);
            }
            catch (Exception exception) { throw exception; }
        }

        //public bool IsMOReSiteNotCorpDelete(int SiteId)
        //{
        //    try
        //    {
        //        CorporateElementSubmissionBO _corpSite = new CorporateElementSubmissionBO();
        //        return _corpSite.IsMOReSiteNotCorpDelete(SiteId);
        //    }
        //    catch (Exception exception) { throw exception; }
        //}

        //public bool IsDeletingMOReCorporateSite(int siteId)
        //{
        //    try
        //    {
        //        CorporateElementSubmissionBO _corpSite = new CorporateElementSubmissionBO();
        //        return _corpSite.IsDeletingMOReCorporateSite(siteId);
        //    }
        //    catch (Exception exception) { throw exception; }
        //}

        #endregion
    }
}