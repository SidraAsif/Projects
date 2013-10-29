#region Modification History

//  ******************************************************************************
//  Module        : Editor
//  Created By    : Waqad Amin
//  When Created  : N/A
//  Description   : Allow use to Change the content of configurable page
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    03-06-2012       Load Active content on page load
//  Mirza Fahad Ali Baig    03-06-2012       LoadActiveContentByPageName Add loading in Function
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT.Webforms
{
    public partial class Editor : System.Web.UI.Page
    {
        #region DATA_MEMBER

        #endregion

        #region VARIABLE
        private InsertContentBO _Content = new InsertContentBO();
        private string PageData;
        private string userType;
        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EditorMessage.Clear("");
                userType = Session["UserType"].ToString();

                if (!IsPostBack)
                {
                    if (userType == enUserRole.SuperAdmin.ToString())
                    {
                        LoadingProcess();
                        CMSPanel.Visible = false;
                    }
                    else
                    {
                        IQueryable<BMTBLL.Page> PageList= GetPage(enUserRole.SuperUser, Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
                        if (PageList.Count() > 0)
                        {
                            CMSPanel.Visible = true;
                            LoadActiveContentByPageName();
                        }

                    }
                }
                else
                {
                    PageData = Request.Params.Get("__EVENTTARGET");
                    string cmd = Request.Params.Get("__EVENTARGUMENT");

                    if (cmd == "save")
                        btnSave_Click(null, null);
                    else if (cmd == "load")
                        btnLoad_Click(null, null);
                    else if (cmd == "onchange")
                        ddlEnterprise_OnTextChange(null, null);
                    else if (cmd == "onLogOff")
                    {
                        BMTMaster bmtMaster = (BMTMaster)Page.Master;
                        bmtMaster.btnLogOut_Click(null, null);
                    }
                    else if (cmd == "onChangePassword")
                    {
                        BMTMaster bmtMaster = (BMTMaster)Page.Master;
                        bmtMaster.lbChangepassword_Click(null, null);
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
            if (Page.IsPostBack)
            {
                int pageCount = 0;
                int enterpriseId = Convert.ToInt32(ddlEnterprise.SelectedValue);
                if (enterpriseId > 0)
                {
                    IQueryable<BMTBLL.Page> PageList = GetPage(enUserRole.SuperUser, enterpriseId);
                    LoadActiveContentByPageName();
                    pageCount = PageList.Count();
                }
                else
                {
                    pageCount = 0;
                }

                if (pageCount > 0)
                {
                    CMSPanel.Visible = true;
                }
                else
                {
                    CMSPanel.Visible = false;
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int PageId = Convert.ToInt32(hiddenSelectedId.Value == string.Empty ? cmbPage.SelectedValue : hiddenSelectedId.Value);
                DateTime postDate = Convert.ToDateTime(txtPostdate.Text);
                int pageContentId = _Content.GetPageContentId(PageId, postDate);
                List<string> ListPageContent = _Content.GetPageContent(pageContentId);

                if (ListPageContent.Count != 0)
                {
                    markItUp.Text = ListPageContent[0];
                    postDate = Convert.ToDateTime(ListPageContent[1]);
                }
                else
                    markItUp.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPostdate.Text == string.Empty)
                    EditorMessage.Warning("Insert Postdate.");

                if (PageData != string.Empty)
                {
                    int PageId = Convert.ToInt32(hiddenSelectedId.Value == string.Empty ? cmbPage.SelectedValue : hiddenSelectedId.Value);
                    string PageContent = PageData;
                    DateTime CreatedDate = System.DateTime.Now;
                    int CreatedBy = 1;
                    DateTime LastUpdatedDate = System.DateTime.Now;
                    int LastUpdatedBy = 2;
                    DateTime PostDate = txtPostdate.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(txtPostdate.Text);

                    int PageContentID = _Content.Save(PageId, PageContent, CreatedDate, CreatedBy, LastUpdatedDate, LastUpdatedBy, PostDate);

                    if (PageContentID != 0)
                        EditorMessage.Success("Content has been saved.");
                }
                else
                    EditorMessage.Warning("Please insert content.");

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }

        }

        #endregion

        #region FUNCTIONS
        protected IQueryable<BMTBLL.Page> GetPage(enUserRole userRole, int enterpriseId)
        {
            IQueryable<BMTBLL.Page> PageList;
            try
            {
                _Content.GetPage(userRole, enterpriseId);
                PageList = _Content.PageList;

                cmbPage.DataTextField = "Name";
                cmbPage.DataValueField = "PageId";
                cmbPage.DataSource = _Content.PageList;
                cmbPage.DataBind();
                return _Content.PageList;
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return _Content.PageList;
            }
        }

        protected void LoadingProcess()
        {
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

        protected void LoadActiveContentByPageName()
        {
            try
            {
                int pageId = 0;
                PageContentBO _pageContentBO = new PageContentBO();
                pageId = Convert.ToInt32(cmbPage.SelectedValue);
                List<PageContentBO> _listPageContent = new List<PageContentBO>();
                _listPageContent = _pageContentBO.GetActiveContentDetailByPageId(pageId);

                foreach (var row in _listPageContent)
                {
                    markItUp.Text = row.Content;
                    txtPostdate.Text = String.Format("{0:MM/dd/yyyy}", row.PostedDate);
                    break;
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        #endregion

    }
}