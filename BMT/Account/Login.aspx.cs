
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using System.IO;

using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;
using System.Text;


namespace BMT.Account
{
    public partial class Login : System.Web.UI.Page
    {

        #region CONSTANTS
        private static int DEFAULT_COOKIE_EXPIRE_DATE = 365;
        private static int DEFAULT_PRACTICE_ID = 0;
        #endregion

        #region VARIABLES
        private SessionHandling _sessionHandling;
        private Security _security;

        private PracticeSizeBO _practiceSizeBO;
        private SpecialityBO _specialityBO;
        private PracticeBO _practiceBO;
        private SiteBO _siteBO;
        private ProjectBO _projectBO;
        private UserBO _userBO;
        private UserAccountBO _userAccountBO;
        private Email _email = new Email();


        private IQueryable _practiceSizeList;
        private IQueryable _specialityList;

        private int userMaxId;
        private bool isEmailValid;
        private static System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\s{1,}"); // 1= Number of spaces

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    int enterpriseId = Util.GetEnterpriseId();
                    if (enterpriseId != 0)
                    {
                        Session[enSessionKey.EnterpriseId.ToString()] = enterpriseId;                        

                        //Display Enterprise Info 
                        lblver.Text = Util.GetSystemVersion(enterpriseId);
                        lblVerSignup.Text = Util.GetSystemVersion(enterpriseId);
                        lblsysteminfo.Text = Util.GetSystemCopyright(enterpriseId);
                        lblsystemsignup.Text = Util.GetSystemCopyright(enterpriseId);

                        string email = Util.GetSystemMailTo(enterpriseId);
                        mailTo.Text = mailToSupport.Text = email;
                        mailTo.NavigateUrl = mailToSupport.NavigateUrl = "mail" + "to:" + email;
                        Page.Title = Util.GetPageTitle(enterpriseId) + " | Log in";

                        // Disable Signup button on click to prevent multiple clicks
                        btnSubmit.Attributes.Add("onclick", "if(Page_ClientValidate('SignUp')){this.disabled=true;} else {this.disabled=false;}" + Page.ClientScript.GetPostBackEventReference(btnSubmit, "onclick").ToString());

                        if (Session["UserApplicationId"] != null)
                            Response.Redirect("~/Webforms/Home.aspx", false);
                        else
                        {
                            HttpCookie cookie = Request.Cookies["Credential"];
                            if (cookie != null)
                            {
                                _userBO = new UserBO();
                                string[] Credential = cookie.Value.ToString().Split('@');
                                _userBO.UserName = Credential[0].ToString(); // 0= UserName
                                _userBO.Password = Credential[1].ToString(); // 1= Password
                                DoLogin(_userBO);
                            }

                            // Get static data
                            GetById();

                            // Load Default settings
                            DefaultSettings();

                            // Reset form
                            ClearField();
                        }

                    }
                    else
                    {
                        LoginMesssage.Error(Util.GetEnumDescription(enErrorMessage.EnterpriseNameIsIncorrect));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void lbTopLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayLogin();
            }
            catch
            { }
        }

        protected void lbTopSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySignUp();
            }
            catch
            { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                string email = txtEmail.Text.Trim().ToLower().ToString();

                /*Create object of UserAccountBO Class*/
                _userAccountBO = new UserAccountBO();

                /*Check if email is available?*/
                isEmailValid = _userAccountBO.EmailExist(email, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

                if (isEmailValid)
                    SignUp();
                else
                    SignUpMessage.Error(Util.GetEnumDescription(enErrorMessage.EmailIsNotAvailable));

            }
            catch (Exception exception)
            {
                string exceptionMessage = exception.StackTrace;
                SignUpMessage.Error(Util.GetEnumDescription(enErrorMessage.UserAccountCouldNotBeCreated));
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtLoginUserName.Text.Trim();
                string password = txtLoginPassword.Text.Trim();

                // Create new object of UserAccountBo Class
                _userBO = new UserBO();
                _userBO.UserName = userName;
                _userBO.Password = password;



                // Encrypt the password
                _security = new Security();
                _userBO.Password = _security.Encrypt(_userBO.Password);

                // Start Login process
                DoLogin(_userBO);


            }
            catch (Exception exception)
            {
                string exceptionMessage = exception.StackTrace;
                LoginMesssage.Error(Util.GetEnumDescription(enErrorMessage.TheServerHasEncounterdAnErrorPleaseTryAgainLater));
            }

        }

        protected void chkBoxAgreement_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxAgreement.Checked)
                    btnSubmit.Enabled = true;
                else
                    btnSubmit.Enabled = false;
            }
            catch { }
        }

        #endregion

        #region FUNCTIONS
        protected void ClearField()
        {
            try
            {
                txtLoginUserName.Text = txtLoginPassword.Text = txtFirstName.Text = txtLastName.Text = txtEmail.Text = txtConfirmEmail.Text =
                    txtPhone.Text = string.Empty;
                ddlPracticeSize.SelectedIndex = 0;
                ddlSpeciality.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        private void DefaultSettings()
        {
            try
            {
                pnlSignUpForm.Visible = false;
                pnlLoginForm.Visible = true;

                LoginMesssage.Clear("");
                SignUpMessage.Clear("");

                lbTopSignUp.CssClass = ""; lbTopLogin.CssClass = "active";
                ddlPracticeSize.Items.Insert(0, new ListItem("--Select--"));
                ddlSpeciality.Items.Insert(0, new ListItem("--Select--"));
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        private void GetById()
        {
            try
            {
                // Fetching static data from Database
                _practiceSizeBO = new PracticeSizeBO();
                _practiceSizeBO.GetPracticeSizeById();
                _practiceSizeList = _practiceSizeBO.PracticeSizeList;

                _specialityBO = new SpecialityBO();
                _specialityBO.GetSpecialityById();
                _specialityList = _specialityBO.SpecialityList;


                // Set DataTextField and DataValueField name
                ddlPracticeSize.DataTextField = ddlSpeciality.DataTextField = "Name";
                ddlPracticeSize.DataValueField = ddlSpeciality.DataValueField = "ID";

                // Assign data source
                ddlPracticeSize.DataSource = _practiceSizeList;
                ddlSpeciality.DataSource = _specialityList;

                // Bind data source
                ddlPracticeSize.DataBind();
                ddlSpeciality.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void DisplayLogin()
        {
            try
            {
                Page.Title = Util.GetPageTitle(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()])) + " | Log in";

                LoginMesssage.Clear("");
                lbTopLogin.CssClass = "active";
                lbTopSignUp.CssClass = "";
                pnlSignUpForm.Visible = false;
                pnlLoginForm.Visible = true;
                ClearField();
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        protected void DisplaySignUp()
        {
            try
            {
                Page.Title = Util.GetPageTitle(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()])) + " | Sign Up";

                chkBoxAgreement.Checked = false;
                btnSubmit.Enabled = false;
                SignUpMessage.Clear("");
                lbTopLogin.CssClass = "";
                lbTopSignUp.CssClass = "active";
                pnlSignUpForm.Visible = true;
                pnlLoginForm.Visible = false;
                ClearField();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        protected void GetMaxId()
        {
            try
            {
                _userBO = new UserBO();
                userMaxId = _userBO.GetMaxId();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void SignUp()
        {

            try
            {
                GetMaxId();

                // store values into variables
                string password = string.Empty;
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string emailAddress = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string firstLetterOfName = string.Empty;

                int practiceSizeId = Convert.ToInt32(ddlPracticeSize.SelectedValue);
                int specialityId = Convert.ToInt32(ddlSpeciality.SelectedValue);

                DateTime createdDate = System.DateTime.Now;

                // Generate Random/Secured password
                _security = new Security();
                password = _security.GenerateRandomPassword();

                //Encrypt password
                password = _security.Encrypt(password);

                //User Account Detail
                _userBO = new UserBO();
                _userBO.FistName = firstName.Trim();
                _userBO.LastName = lastName.Trim();
                _userBO.Email = emailAddress.Trim();
                _userBO.Phone = phone.Trim();

                if (_userBO.FistName.Length == 1)
                    firstLetterOfName = firstName;
                else
                    firstLetterOfName = firstName.Remove(1);

                // create userName
                _userBO.UserName = firstLetterOfName + lastName + userMaxId.ToString();

                // Remove spaces from username
                _userBO.UserName = regex.Replace(_userBO.UserName.Trim(), "");
                _userBO.Password = password;
                _userBO.CreatedDate = createdDate;

                // User Practice Size Detail
                _practiceSizeBO = new PracticeSizeBO();
                _practiceSizeBO.PracriceSizeId = practiceSizeId;

                // User Speciality Detail
                _specialityBO = new SpecialityBO();
                _specialityBO.SpecialityId = specialityId;

                // User Other Details                
                _practiceBO = new PracticeBO(Convert.ToInt32(ConfigurationManager.AppSettings["DefaultMedicalGroupId"]));
                _siteBO = new SiteBO();
                _userAccountBO = new UserAccountBO();
                _projectBO = new ProjectBO();

                int userId = _userAccountBO.SignUp(_userBO, _practiceSizeBO, _specialityBO, _practiceBO, _siteBO, _projectBO, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]), Convert.ToInt32(ConfigurationManager.AppSettings["DefaultMedicalGroupId"]));

                if (userId != 0)
                    CreateSession(userId, true);
                else
                {
                    SignUpMessage.Success(Util.GetEnumDescription(enErrorMessage.UserAccountCouldNotBeCreated));
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                throw exception;
            }
        }

        protected void DoLogin(UserBO userDetail)
        {

            try
            {
                _userAccountBO = new UserAccountBO();
                int userId = _userAccountBO.Login(userDetail, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));

                if (userId != 0)
                {
                    // If Remeber me check is on
                    if (chkBoxRememberMe.Checked)
                    {
                        HttpCookie cookie = new HttpCookie("Credential");
                        cookie.Value = userDetail.UserName + "@" + userDetail.Password;
                        cookie.Expires = System.DateTime.Now.AddDays(DEFAULT_COOKIE_EXPIRE_DATE);
                        Response.Cookies.Add(cookie);
                    }

                    // create Sesssion
                    CreateSession(userId, false);
                }
                else
                    LoginMesssage.Error(Util.GetEnumDescription(enErrorMessage.UsernamePasswordIsIncorrect));

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        protected void CreateSession(int userId, bool isNew)
        {

            try
            {
                if (Session["userType"] != null)
                {
                    _sessionHandling = new SessionHandling();
                    _sessionHandling.ClearSession();
                }

                _userAccountBO = new UserAccountBO();
                List<string> _credentialDetails = _userAccountBO.GetUserDetailById(userId);
                Session["UserApplicationId"] = userId;
                Session["UserName"] = _credentialDetails[0];   //0: UserName
                Session["PracticeId"] = _credentialDetails[1]; //1: PracticeId
                Session["UserType"] = _credentialDetails[2];   //2: User Type
                Session["UserEmail"] = _credentialDetails[3]; //3: Email               

                // TODO: Update Practice Id if SuperUser/Consultant User logged-in
                if (_credentialDetails[2] == enUserRole.SuperUser.ToString() ||
                    _credentialDetails[2] == enUserRole.Consultant.ToString() ||
                    _credentialDetails[2] == enUserRole.SuperAdmin.ToString())
                {
                    Session["PracticeId"] = DEFAULT_PRACTICE_ID.ToString();
                }

                // TODO: Show Welcome message when new user signup
                if (Request.QueryString["LastRequest"] != null)
                    Response.Redirect(Request.QueryString["LastRequest"].ToString(), false);
                else if (isNew)
                    Response.Redirect("~/Webforms/Home.aspx?registered=success", false);
                else
                    Response.Redirect("~/Webforms/Home.aspx", false);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        #endregion

    }
}