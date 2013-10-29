#region Modification History

//  ******************************************************************************
//  Module        : Dashboard screen for SuperUser and Consultant user
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : To display the current available information against each practice
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad ALi Baig    03-09-2012      Fix Points and Document ASC And DESC Issue
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT.Webforms
{
    public partial class Dashboard : System.Web.UI.Page
    {
        #region CONSTANTS
        private const string DEFAULT_ASC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/asc.png' />";
        private const string DEFAULT_DESC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/desc.png' />";
        private const string DEFAULT_ASC_DESC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/asc-desc.png' />";
        // sorting columns only
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT = "Practice Name";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT = "Site Name";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT = "%Complete";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT = "%Documentation";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT = "Last Activity";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT = "Contact Name";


        private const string SRA_DASHBOARD_HEADER_COLUMN0_TEXT = "Practice Name";
        private const string SRA_DASHBOARD_HEADER_COLUMN1_TEXT = "Site Name";
        private const string SRA_DASHBOARD_HEADER_COLUMN2_TEXT = "Findings Finalized";
        private const string SRA_DASHBOARD_HEADER_COLUMN3_TEXT = "Followup Finalized";
        private const string SRA_DASHBOARD_HEADER_COLUMN4_TEXT = "Last Activity";
        private const string SRA_DASHBOARD_HEADER_COLUMN5_TEXT = "Contact Name";

        private const string DEFAULT_COLUMN_NAME = "PracticeName";
        private const string DEFAULT_ORDER_BY = "Ascending";

        #endregion

        #region VARIABLES
        private Security _security = new Security();
        private PracticeBO _practice = new PracticeBO();

        private int enterpriseId;

        private int startRowIndex;
        private int pageSize;

        private string columnName;
        private string orderBy;

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserApplicationId"] != null || Session["UserType"] != null)
                {
                    int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                    string userType = Session["UserType"].ToString();

                    GetEnterpriseId();

                    // load practice details on pageload
                    if (!Page.IsPostBack)
                    {
                        if (userType == enUserRole.SuperAdmin.ToString())
                        {
                            LoadingProcess();
                            pnlDashBoard.Visible = false;
                            PnlSRA.Visible = false;
                            TblOptions.Visible = false;
                        }
                        else
                        {
                            // reset column view state
                            ResetSortingState();

                            // set starting page;
                            startRowIndex = 0;

                            // set default columnName and order By on page load
                            columnName = DEFAULT_COLUMN_NAME;
                            orderBy = DEFAULT_ORDER_BY;

                            ResetImageOnChange();


                            Session["HOME_DASHBOARD"] = _practice.GetHomeDashboard(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));

                            DDLSelectDashboard.SelectedValue = Session["HOME_DASHBOARD"].ToString();

                            TreeBO treeBO = new TreeBO();
                            if (!treeBO.IsSecurityRiskAssessmentExist(enterpriseId))
                            {
                                DDLSelectDashboard.Items.RemoveAt(1);
                            }

                            if (DDLSelectDashboard.SelectedValue == "0")
                            {

                                GetPracticeDetails();
                                pnlDashBoard.Visible = true;
                                PnlSRA.Visible = false;

                            }
                            else
                            {
                                GetSraData();
                                PnlSRA.Visible = true;
                                pnlDashBoard.Visible = false;
                            }
                            CBHomeDashboard.Checked = true;

                        }
                    }

                }
                else
                {
                    SessionHandling _sessionHandling = new SessionHandling();
                    _sessionHandling.ClearSession();
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void datagridPractice_OnPageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                datagridPractice.CurrentPageIndex = startRowIndex = e.NewPageIndex;
                ResetImageOnChange();
                GetPracticeDetails();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void dataGridSRA_OnPageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dataGridSRA.CurrentPageIndex = startRowIndex = e.NewPageIndex;
                ResetImageOnChange();
                GetSraData();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void datagridPractice_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            try
            {
                startRowIndex = 0;
                datagridPractice.CurrentPageIndex = 0;
                columnName = e.SortExpression.ToString();
                string currentSortingState = string.Empty;

                // Reset column images on page changed
                ResetColumnSortingImage();

                switch (columnName)
                {
                    case "PracticeName":
                        currentSortingState = ViewState["PracticeName"].ToString();

                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["PracticeName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["PracticeName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "SiteName":

                        currentSortingState = ViewState["SiteName"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["SiteName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["SiteName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "Points":

                        currentSortingState = ViewState["Points"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[2].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["Points"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[2].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["Points"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "Documents":

                        currentSortingState = ViewState["Documents"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[3].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["Documents"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[3].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["Documents"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "LastActivity":

                        currentSortingState = ViewState["LastActivity"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["LastActivity"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["LastActivity"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "ContactName":

                        currentSortingState = ViewState["ContactName"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridPractice.Columns[5].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["ContactName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridPractice.Columns[5].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["ContactName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    default:
                        return;

                }

                PracticeBO practiceBO = new PracticeBO();
                practiceBO.UpdateUserPrefer(columnName, orderBy, DDLSelectDashboard.SelectedItem.Text, Convert.ToInt32(Session["UserApplicationId"]));


                // get practiceDetail by order
                GetPracticeDetails();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void dataGridSRA_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            try
            {
                startRowIndex = 0;
                dataGridSRA.CurrentPageIndex = 0;
                columnName = e.SortExpression.ToString();
                string currentSortingState = string.Empty;

                // Reset column images on page changed
                ResetColumnSortingImageSRA();

                switch (columnName)
                {
                    case "PracticeName":
                        currentSortingState = ViewState["PracticeName"].ToString();

                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[0].HeaderText = SRA_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["PracticeName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[0].HeaderText = SRA_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["PracticeName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "SiteName":

                        currentSortingState = ViewState["SiteName"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[1].HeaderText = SRA_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["SiteName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[1].HeaderText = SRA_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["SiteName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "FindingsFinalized":

                        currentSortingState = ViewState["FindingsFinalized"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[2].HeaderText = SRA_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["FindingsFinalized"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[2].HeaderText = SRA_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["FindingsFinalized"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "FollowupFinalized":

                        currentSortingState = ViewState["FollowupFinalized"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[3].HeaderText = SRA_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["FollowupFinalized"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[3].HeaderText = SRA_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["FollowupFinalized"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "LastActivity":

                        currentSortingState = ViewState["LastActivity"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[4].HeaderText = SRA_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["LastActivity"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[4].HeaderText = SRA_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["LastActivity"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    case "ContactName":

                        currentSortingState = ViewState["ContactName"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            dataGridSRA.Columns[5].HeaderText = SRA_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            ViewState["ContactName"] = orderBy = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            dataGridSRA.Columns[5].HeaderText = SRA_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            ViewState["ContactName"] = orderBy = enSortingType.Descending.ToString();
                        }
                        break;

                    default:
                        return;

                }

                PracticeBO practiceBO = new PracticeBO();
                practiceBO.UpdateUserPrefer(columnName, orderBy, DDLSelectDashboard.SelectedItem.Text, Convert.ToInt32(Session["UserApplicationId"]));

                GetSraData();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void ddlEnterprise_OnTextChange(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                {
                    GetEnterpriseId();

                    // reset column view state
                    ResetSortingState();

                    // set starting page;
                    startRowIndex = 0;

                    // set default columnName and order By on page load
                    columnName = DEFAULT_COLUMN_NAME;
                    orderBy = DEFAULT_ORDER_BY;

                    // get practice details
                    //datagridPractice.CurrentPageIndex = 0;
                    //GetPracticeDetails();
                    //pnlDashBoard.Visible = true;

                    TblOptions.Visible = true;
                    Session["HOME_DASHBOARD"] = _practice.GetHomeDashboard(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));
                    DDLSelectDashboard.SelectedValue = Session["HOME_DASHBOARD"].ToString();

                    if (DDLSelectDashboard.SelectedValue == "0")
                    {
                        GetPracticeDetails();
                        pnlDashBoard.Visible = true;
                        PnlSRA.Visible = false;

                    }
                    else
                    {
                        GetSraData();
                        PnlSRA.Visible = true;
                        pnlDashBoard.Visible = false;
                    }
                    CBHomeDashboard.Checked = true;
                }
                else
                {
                    pnlDashBoard.Visible = false;
                    PnlSRA.Visible = false;
                    TblOptions.Visible = false;
                }
            }
        }

        protected void DDLSelectDashboard_SelectedIndexChanged(object sender, EventArgs e)
        {

            GetEnterpriseId();
            if (DDLSelectDashboard.SelectedValue == "0")
            {
                ResetImageOnChange();
                GetPracticeDetails();
                pnlDashBoard.Visible = true;
                PnlSRA.Visible = false;

                if (DDLSelectDashboard.SelectedValue == Session["HOME_DASHBOARD"].ToString())
                    CBHomeDashboard.Checked = true;
                else CBHomeDashboard.Checked = false;

            }
            else
            {
                ResetImageOnChangeSRA();
                GetSraData();
                PnlSRA.Visible = true;
                pnlDashBoard.Visible = false;

                if (DDLSelectDashboard.SelectedValue == Session["HOME_DASHBOARD"].ToString())
                    CBHomeDashboard.Checked = true;
                else
                    CBHomeDashboard.Checked = false;
            }

        }

        protected void CBHomeDashboard_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string userType = Session["UserType"].ToString();
                if (CBHomeDashboard.Checked)
                {
                    if (userType != enUserRole.SuperAdmin.ToString())
                        _practice.SaveHomeDashboard(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]), Convert.ToInt32(DDLSelectDashboard.SelectedValue));
                    else
                        _practice.SaveDashboard(Convert.ToInt32(DDLSelectDashboard.SelectedValue));

                }
                else
                {
                    if (userType != enUserRole.SuperAdmin.ToString())
                        _practice.SaveHomeDashboard(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]), 0);
                    else
                        _practice.SaveDashboard(0);

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        #endregion

        #region FUNCTIONS
        private void GetPracticeDetails()
        {
            try
            {
                string userType = Session["UserType"].ToString();
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                List<PracticeDetails> _practiceDetailList = new List<PracticeDetails>();
                pageSize = datagridPractice.PageSize;
                startRowIndex = (((startRowIndex + 1) * pageSize) - pageSize);

                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.Consultant.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    // get total number of records for paging
                    datagridPractice.VirtualItemCount = _practice.CountPractices(userApplicationId, userType, enterpriseId);
                    // remove paging if not need
                    if (datagridPractice.VirtualItemCount <= datagridPractice.PageSize)
                        datagridPractice.PagerStyle.Visible = false;
                    else
                        datagridPractice.PagerStyle.Visible = true;

                    //get list of practice for current page
                    _practiceDetailList = _practice.GetPracticeDetails(userApplicationId, userType, startRowIndex, pageSize, columnName, orderBy, enterpriseId);

                    foreach (PracticeDetails _practiceDetails in _practiceDetailList)
                    {
                        // encrypt practice Id to make it secure while sending in url
                        _practiceDetails.SecurePracticeId = _security.Encrypt(_practiceDetails.SecurePracticeId);
                        _practiceDetails.SecurePracticeId = Server.UrlEncode(_practiceDetails.SecurePracticeId);//_practiceDetails.SecurePracticeId.Replace("=", "|||");

                        if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.SuperAdmin.ToString())
                        {
                            _practiceDetails.SecureEnterpriseId = _security.Encrypt(ddlEnterprise.SelectedValue.ToString());
                            _practiceDetails.SecureEnterpriseId = Server.UrlEncode(_practiceDetails.SecureEnterpriseId); //_practiceDetails.SecureEnterpriseId.Replace("=", "|||");
                        }
                    }

                    datagridPractice.DataSource = _practiceDetailList;
                    datagridPractice.DataBind();
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        private void GetSraData()
        {
            try
            {
                pageSize = dataGridSRA.PageSize;
                startRowIndex = (((startRowIndex + 1) * pageSize) - pageSize);
                string userType = Session["UserType"].ToString();
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.Consultant.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    // get total number of records for paging
                    dataGridSRA.VirtualItemCount = _practice.CountSRAPractices(userApplicationId, userType, enterpriseId);

                    // remove paging if not need
                    if (dataGridSRA.VirtualItemCount <= dataGridSRA.PageSize)
                        dataGridSRA.PagerStyle.Visible = false;
                    else
                        dataGridSRA.PagerStyle.Visible = true;

                    //get list of practice for current page

                    List<SRAdashboard> list = _practice.GetSRAData(userApplicationId, userType, startRowIndex, pageSize, columnName, orderBy, enterpriseId);
                    foreach (SRAdashboard _sraRecord in list)
                    {
                        _sraRecord.SecurePracticeId = _security.Encrypt(_sraRecord.SecurePracticeId);
                        _sraRecord.SecurePracticeId = Server.UrlEncode(_sraRecord.SecurePracticeId); //_practiceDetails.SecurePracticeId.Replace("=", "|||");

                        if (Session[enSessionKey.UserType.ToString()].ToString() == enUserRole.SuperAdmin.ToString())
                        {
                            _sraRecord.SecureEnterpriseId = _security.Encrypt(ddlEnterprise.SelectedValue.ToString());
                            _sraRecord.SecureEnterpriseId = Server.UrlEncode(_sraRecord.SecureEnterpriseId); //_practiceDetails.SecureEnterpriseId.Replace("=", "|||");
                        }
                    }

                    dataGridSRA.DataSource = list;
                    dataGridSRA.DataBind();
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        private void ResetSortingState()
        {
            // Reset column images on page changed
            ResetColumnSortingImage();
            ResetColumnSortingImageSRA();

            // reset column view state on page changed
            ViewState["PracticeName"] = enSortingType.Normal.ToString();
            ViewState["SiteName"] = enSortingType.Normal.ToString();
            ViewState["Points"] = enSortingType.Normal.ToString();
            ViewState["Documents"] = enSortingType.Normal.ToString();
            ViewState["LastActivity"] = enSortingType.Normal.ToString();
            ViewState["ContactName"] = enSortingType.Normal.ToString();
            ViewState["FindingsFinalized"] = enSortingType.Normal.ToString();
            ViewState["FollowupFinalized"] = enSortingType.Normal.ToString();
        }

        private void ResetColumnSortingImage()
        {
            // Reset column images on page changed
            datagridPractice.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridPractice.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridPractice.Columns[2].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridPractice.Columns[3].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridPractice.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridPractice.Columns[5].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
        }

        private void ResetColumnSortingImageSRA()
        {
            // Reset column images on page changed
            dataGridSRA.Columns[0].HeaderText = SRA_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            dataGridSRA.Columns[1].HeaderText = SRA_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            dataGridSRA.Columns[2].HeaderText = SRA_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            dataGridSRA.Columns[3].HeaderText = SRA_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            dataGridSRA.Columns[4].HeaderText = SRA_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            dataGridSRA.Columns[5].HeaderText = SRA_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
        }

        protected void LoadingProcess()
        {
            string userType = Session["UserType"].ToString();
            if (userType == enUserRole.SuperAdmin.ToString())
            {
                PracticeBO practiceBO = new PracticeBO();
                IQueryable EnterpriseList;
                EnterpriseList = practiceBO.GetEnterprises();
                ddlEnterprise.DataTextField = "Name";
                ddlEnterprise.DataValueField = "ID";
                ddlEnterprise.DataSource = EnterpriseList;
                ddlEnterprise.DataBind();

                //Add Default item in comboBox
                ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
                lblEnterprise.Visible = ddlEnterprise.Visible = true;
            }
            else
            {
                lblEnterprise.Visible = ddlEnterprise.Visible = false;
            }
        }

        protected void ResetImageOnChange()
        {
            try
            {
                string tableType = DDLSelectDashboard.SelectedItem.Text;
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                PracticeBO practiceBO = new PracticeBO();
                XElement xFavSort = practiceBO.GetUserXml(userApplicationId, tableType);
                if (xFavSort != null)
                {
                    columnName = xFavSort.Element("Column").Value;
                    orderBy = xFavSort.Element("Direction").Value;

                    string currentSortingState = orderBy;

                    // Reset column images on page changed
                    ResetColumnSortingImage();

                    switch (columnName)
                    {
                        case "PracticeName":
                            //currentSortingState = ViewState["PracticeName"].ToString();

                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["PracticeName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["PracticeName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "SiteName":

                            //currentSortingState = ViewState["SiteName"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["SiteName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["SiteName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "Points":

                            // currentSortingState = ViewState["Points"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[2].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["Points"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[2].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["Points"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "Documents":

                            //currentSortingState = ViewState["Documents"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[3].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["Documents"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[3].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["Documents"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "LastActivity":

                            //currentSortingState = ViewState["LastActivity"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["LastActivity"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["LastActivity"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "ContactName":

                            //currentSortingState = ViewState["ContactName"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                datagridPractice.Columns[5].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["ContactName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                datagridPractice.Columns[5].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["ContactName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        default:
                            return;

                    }


                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }


        protected void ResetImageOnChangeSRA()
        {
            try
            {
                string tableType = DDLSelectDashboard.SelectedItem.Text;
                PracticeBO practiceBO = new PracticeBO();
                int userApplicationId = Convert.ToInt32(Session["UserApplicationId"]);
                XElement xFavSort = practiceBO.GetUserXml(userApplicationId, tableType);
                if (xFavSort != null)
                {
                    columnName = xFavSort.Element("Column").Value;
                    orderBy = xFavSort.Element("Direction").Value;

                    string currentSortingState = orderBy;

                    // Reset column images on page changed
                    ResetColumnSortingImage();

                    switch (columnName)
                    {
                        case "PracticeName":
                            //currentSortingState = ViewState["PracticeName"].ToString();

                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[0].HeaderText = SRA_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["PracticeName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[0].HeaderText = SRA_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["PracticeName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "SiteName":

                            //currentSortingState = ViewState["SiteName"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[1].HeaderText = SRA_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["SiteName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[1].HeaderText = SRA_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["SiteName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "FindingsFinalized":

                            //currentSortingState = ViewState["FindingsFinalized"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[2].HeaderText = SRA_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["FindingsFinalized"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[2].HeaderText = SRA_DASHBOARD_HEADER_COLUMN2_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["FindingsFinalized"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "FollowupFinalized":

                            //currentSortingState = ViewState["FollowupFinalized"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[3].HeaderText = SRA_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["FollowupFinalized"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[3].HeaderText = SRA_DASHBOARD_HEADER_COLUMN3_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["FollowupFinalized"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "LastActivity":

                            //currentSortingState = ViewState["LastActivity"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[4].HeaderText = SRA_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["LastActivity"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[4].HeaderText = SRA_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["LastActivity"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        case "ContactName":

                            //currentSortingState = ViewState["ContactName"].ToString();
                            if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Ascending.ToString()))
                            {
                                dataGridSRA.Columns[5].HeaderText = SRA_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_ASC_IMAGE_HTML;
                                ViewState["ContactName"] = orderBy = enSortingType.Ascending.ToString();
                            }
                            else
                            {
                                dataGridSRA.Columns[5].HeaderText = SRA_DASHBOARD_HEADER_COLUMN5_TEXT + DEFAULT_DESC_IMAGE_HTML;
                                ViewState["ContactName"] = orderBy = enSortingType.Descending.ToString();
                            }
                            break;

                        default:
                            return;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        private void GetEnterpriseId()
        {
            try
            {
                string userType = Session["UserType"].ToString();
                userType = Session[enSessionKey.UserType.ToString()].ToString();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    if (!string.IsNullOrEmpty(ddlEnterprise.SelectedValue))
                    {
                        enterpriseId = Convert.ToInt32(ddlEnterprise.SelectedValue);
                    }
                }
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