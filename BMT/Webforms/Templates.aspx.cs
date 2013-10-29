using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BMTBLL;
using BMT.WEB;
using System.Data;
using BMTBLL.Enumeration;
using System.Text.RegularExpressions;


namespace BMT.Webforms
{
    public partial class Templates : System.Web.UI.Page
    {
        #region VARIABLES
        private TemplatesBO _template;
        private IQueryable _enterpriseList;
        private int enterpriseId;
        /*private int templateId;*/
        private int userId;
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string userType;
                userType = Session["UserType"].ToString();
                if (Session[enSessionKey.UserApplicationId.ToString()] != null)
                {
                    LoadingProcess();

                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        
        protected void btnUpload_Sheet(object sender, EventArgs e)
        {

            LoadDataIntoGridView();
        }

        protected void btnSave_Template(object sender, EventArgs e)
        {
			if (!Regex.IsMatch(txtTemplateName.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,]+$"))
            {
                Message.Error("Template name is invalid. Please try again!");
                return;
            }            

            _template = new TemplatesBO();
            if (!_template.IsTemplateNameAvailable(txtTemplateName.Text, enterpriseId))
            {
                Message.Error("Template Already Exists. Please Create Template With Different Name.");
                txtTemplateName.Text = "";
                txtShortName.Text = "";
                txtDescription.Text = "";
                return;
            }

            string templateName = txtTemplateName.Text;
            string shortName = txtShortName.Text;
            string description = txtDescription.Text;
            string templateAccessId = ddTemplateAccess.SelectedValue;

            if (_template.SaveTemplate(templateName, shortName, description, userId, enterpriseId, Convert.ToInt32(templateAccessId)))
            {
                Message.Success("Template Saved Successfully.");
                txtTemplateName.Text = "";
                txtShortName.Text = "";
                txtDescription.Text = "";
            }
            else
                Message.Error("Template Cannot be Saved.");

            GetTemplates(enterpriseId);
        }

        public void LoadDataIntoGridView()
        {
            string destinationPath = Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString() + "\\" + uploadFile.FileName;
            int templateId = Convert.ToInt32(Session["TemplateId"].ToString());
            string strExtension = Path.GetExtension(uploadFile.FileName).ToLower();

            uploadFile.SaveAs(destinationPath);
            _template = new TemplatesBO();
            if (_template.ImportFromExcel(destinationPath, templateId, userId))
                Message.Success("Data Imported Successfully.");
            else
                Message.Error("Data Cannot Be Imported! Please Try Again.");

            BindHeaders();

            System.IO.File.Delete(destinationPath);


        }

        protected void grdHeader_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdHeader.PageIndex = e.NewPageIndex;
                BindHeaders();
                grdSubHeader.Visible = false;
                btnAddSubHeader.Visible = false;
                grdQuestion.Visible = false;
                btnAddQuestion.Visible = false;
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdSubHeader_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdSubHeader.PageIndex = e.NewPageIndex;
                BindSubHeaders(Convert.ToInt32(Session["headerId"]));
                grdQuestion.Visible = false;
                btnAddQuestion.Visible = false;
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdQuestion.PageIndex = e.NewPageIndex;
                BindQuestions(Convert.ToInt32(Session["subHeaderId"]));
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
                    enterpriseId = Convert.ToInt32(ddlEnterprise.SelectedValue);
                    if (enterpriseId > 0)
                    {
						templateName.InnerText = "";
                        Session[enSessionKey.EnterpriseId.ToString()] = enterpriseId;                        
                        btnTemplate.Enabled = true;
                        btnTemplate.CssClass = "top-button-yollaw";
                        ddlTemplates.Enabled = true;
                        grdHeader.Visible = false;
                        grdSubHeader.Visible = false;
                        btnAddHeader.Visible = false;
                        btnAddQuestion.Visible = false;
                        btnAddSubHeader.Visible = false;
                        grdQuestion.Visible = false;
                        GetTemplates(enterpriseId);
                    }
                    else
                    {
						templateName.InnerText = "";
                        btnTemplate.Enabled = false;
                        btnTemplate.CssClass = "top-button-yollaw-disable";
                        grdHeader.Visible = false;
                        grdSubHeader.Visible = false;
                        btnAddHeader.Visible = false;
                        btnAddQuestion.Visible = false;
                        btnAddSubHeader.Visible = false;
                        grdQuestion.Visible = false;
                        ddlTemplates.SelectedIndex = -1;
                        ddlTemplates.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void ddlTemplates_OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    if (Convert.ToInt32(ddlTemplates.SelectedValue) > 0)
                    {
                        btnTemplate.Enabled = false;
                        btnTemplate.CssClass = "top-button-yollaw-disable";
                        Session["TemplateId"] = ddlTemplates.SelectedValue;
                        int templateId = Convert.ToInt32(Session["TemplateId"]);
                        _template = new TemplatesBO();
						templateName.InnerText =  _template.GetTemplateNameByTemplateId(templateId);
                        grdHeader.DataSource = _template.GetHeadersTemplate(templateId);

                        grdSubHeader.Visible = false;
                        btnAddSubHeader.Visible = false;
                        grdQuestion.Visible = false;
                        btnAddQuestion.Visible = false;
                        grdHeader.Visible = true;
                        btnAddHeader.Visible = true;

                        grdHeader.Columns[1].Visible = true;
                        grdHeader.EditIndex = -1;
                        grdHeader.DataBind();
                        btnAddHeader.Visible = true;

                        if (grdHeader.Rows.Count == 0)
                        {btnUploadSheet.Enabled = true;
                            btnUploadSheet.CssClass = "top-button-yollaw";
                        }
                        else
                        {btnUploadSheet.Enabled = false;
                            btnUploadSheet.CssClass = "top-button-yollaw-disable";
                        }
                    }
                    else
                    {
						templateName.InnerText = "";
                        btnTemplate.Enabled = true;
                        btnTemplate.CssClass = "top-button-yollaw";
                        btnUploadSheet.Enabled = false;
                        btnUploadSheet.CssClass = "top-button-yollaw-disable";
                        grdHeader.Visible = false;
                        grdSubHeader.Visible = false;
                        btnAddHeader.Visible = false;
                        btnAddQuestion.Visible = false;
                        btnAddSubHeader.Visible = false;
                        grdQuestion.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        public void grdHeader_RowSelecting(object sender, GridViewCommandEventArgs e)
        {
            int templateId = Convert.ToInt32(Session["TemplateId"]);
            if (e.CommandName == "Select")
            {
                int headerId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["headerId"] = headerId;                
                BindSubHeaders(headerId);
                grdSubHeader.Visible = true;
                btnAddSubHeader.Visible = true;
                grdQuestion.Visible = false;
                btnAddQuestion.Visible = false;
                grdHeader.EditIndex = -1;
                BindHeaders();
                Message.Clear("");
            }

            else if (e.CommandName == "Insert")
            {
                TemplatesBO templates = new TemplatesBO();
                TextBox headerText = (TextBox)grdHeader.FooterRow.FindControl("txtAddHeader");
                if (!Regex.IsMatch(headerText.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Header name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsHeaderAllRedayExist(headerText.Text, templateId))
                {
                    Message.Error("Header Name Already Exist. Please Add New Header With Different Name.");
                    return;
                }

                templates.AddHeader(headerText.Text, templateId);
                BindHeaders();

                Message.Success("Header Inserted Successfully.");
            }
        }

        protected void grdHeader_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                TemplatesBO templates = new TemplatesBO();
                int headerId = Convert.ToInt32(grdHeader.DataKeys[e.RowIndex].Values[0].ToString());
                TextBox headerText = (TextBox)grdHeader.Rows[e.RowIndex].FindControl("txtEditHeader");

                if (!Regex.IsMatch(headerText.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Header name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsHeaderExist(headerText.Text, templateId, headerId))
                {
                    Message.Error("Header Name Already Exist. Please Add New Header With Different Name.");
                    return;
                }
                
                templates.UpdateHeaders(headerId, headerText.Text);
                grdHeader.EditIndex = -1;
                BindHeaders();
                Message.Success("Header Updated Succesfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Header Cannot be Updated. Please Try Again.");
            }
        }

        public void grdSubHeader_RowSelecting(Object sender, GridViewCommandEventArgs e)
        {
            int headerId = Convert.ToInt32(Session["headerId"]);
            int templateId = Convert.ToInt32(Session["TemplateId"]);
            if (e.CommandName == "Select")
            {
                int subHeaderId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["subHeaderId"] = subHeaderId;
                BindQuestions(subHeaderId);
                grdQuestion.Visible = true;
                btnAddQuestion.Visible = true;
                grdHeader.EditIndex = -1;
                grdSubHeader.EditIndex = -1;
                BindHeaders();
                BindSubHeaders(headerId);
                Message.Clear("");
            }
            else if (e.CommandName == "Insert")
            {                
                TemplatesBO templates = new TemplatesBO();
                TextBox subHeaderName = (TextBox)grdSubHeader.FooterRow.FindControl("txtAddSubHeader");

                if (!Regex.IsMatch(subHeaderName.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Sub-Header name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsSubHeaderAllReadyExist(subHeaderName.Text, templateId, headerId))
                {
                    Message.Error("Sub-Header Name Already Exist. Please Add New Sub-Header With Different Name.");
                    return;
                }
                templates.AddSubHeader(headerId, subHeaderName.Text, templateId);
                BindSubHeaders(headerId);

                Message.Success("Sub-Header Inserted Successfully.");
            }
        }

        public void grdQuestion_RowSelecting(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                TemplatesBO templates = new TemplatesBO();
                TextBox questionName = (TextBox)grdQuestion.FooterRow.FindControl("txtAddQuestion");
                int subHeaderId = Convert.ToInt32(Session["subHeaderId"]);

                if (!Regex.IsMatch(questionName.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Question name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsAllReadyQuestionExist(questionName.Text, templateId, subHeaderId))
                {
                    Message.Error("Question Name Already Exist. Please Add New Question With Different Name.");
                    return;
                }

                templates.AddQuestion(subHeaderId, questionName.Text, templateId);
                BindQuestions(subHeaderId);
                Message.Success("Question Inserted Successfully.");
            }
        }

        public void grdHeader_NewRow(object sender, EventArgs e)
        {
            if (grdHeader.FooterRow == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("ParentKnowledgeBaseId");
                dt.Columns.Add("Name");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                grdHeader.DataSource = dt;
                grdHeader.DataBind();
                grdHeader.Rows[0].Visible = false;
                grdHeader.Rows[0].Controls.Clear();
            }
            grdHeader.FooterRow.Visible = true;
            grdHeader.Columns[1].Visible = true;
            int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
            grdSubHeader.EditIndex = -1;
            BindSubHeaders(parentKnowledgeBaseId);

            int subHeaderBaseId = Convert.ToInt32(Session["subHeaderId"]);
            grdQuestion.EditIndex = -1;
            BindQuestions(subHeaderBaseId);
            
        }

        public void grdSubHeader_NewRow(object sender, EventArgs e)
        {
            if (grdSubHeader.FooterRow == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("ParentKnowledgeBaseId");
                dt.Columns.Add("Name");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                grdSubHeader.DataSource = dt;
                grdSubHeader.DataBind();

                grdSubHeader.Rows[0].Visible = false;
                grdSubHeader.Rows[0].Controls.Clear();
            }

            grdSubHeader.FooterRow.Visible = true;
            grdSubHeader.Columns[1].Visible = true;
            grdHeader.EditIndex = -1;
            BindHeaders();
            int parentKnowledgeBaseId = Convert.ToInt32(Session["subHeaderId"]);
            grdQuestion.EditIndex = -1;
            BindQuestions(parentKnowledgeBaseId);
        }

        protected void grdHeader_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdHeader.EditIndex = e.NewEditIndex;
                BindHeaders();

                int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
                grdSubHeader.EditIndex = -1;
                BindSubHeaders(parentKnowledgeBaseId);

                int subHeaderId = Convert.ToInt32(Session["subHeaderId"]);
                grdQuestion.EditIndex = -1;
                BindQuestions(subHeaderId);
                
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int headerId = Convert.ToInt32(grdHeader.DataKeys[e.RowIndex].Values[0].ToString());
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                TemplatesBO templates = new TemplatesBO();
                templates.DeleteHeader(headerId, templateId);
                grdSubHeader.Visible = false;
                btnAddSubHeader.Visible = false;
                grdQuestion.Visible = false;
                btnAddQuestion.Visible = false;
                BindHeaders();

                Message.Success("Header Deleted Successfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Header Cannot be Deleted. Please Try Again.");
            }
        }

        protected void grdHeader_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdHeader.EditIndex = -1;
                BindHeaders();
                Message.Clear("");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdSubHeader_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
                grdSubHeader.EditIndex = e.NewEditIndex;

                grdHeader.EditIndex = -1;
                BindHeaders();

                BindSubHeaders(parentKnowledgeBaseId);

                int subHeaderId = Convert.ToInt32(Session["subHeaderId"]);
                grdQuestion.EditIndex = -1;
                BindQuestions(subHeaderId);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdSubHeader_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
                grdSubHeader.EditIndex = -1;

                BindSubHeaders(parentKnowledgeBaseId);
                Message.Clear("");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdSubHeader_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TemplatesBO templates = new TemplatesBO();
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                int headerId = Convert.ToInt32(Session["headerId"]);
                int subHeaderId = Convert.ToInt32(grdSubHeader.DataKeys[e.RowIndex].Values[0].ToString());                
                TextBox subHeaderText = (TextBox)grdSubHeader.Rows[e.RowIndex].FindControl("txtEditSubHeader");

                if (!Regex.IsMatch(subHeaderText.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Sub-Header name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsSubHeaderExist(subHeaderText.Text, templateId, headerId,subHeaderId))
                {
                    Message.Error("Sub-Header Name Already Exist. Please Add New Sub-Header With Different Name.");
                    return;
                }
                
                templates.UpdateSubHeaders(subHeaderId, subHeaderText.Text);
                grdSubHeader.EditIndex = -1;

                BindSubHeaders(headerId);
                Message.Success("Sub-Header Updated Successfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Sub-Header Cannot be Updated. Please Try Again.");
            }
        }

        protected void grdSubHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int subHeaderId = Convert.ToInt32(grdSubHeader.DataKeys[e.RowIndex].Values[0].ToString());
				int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
                TemplatesBO templates = new TemplatesBO();
                templates.DeleteSubHeader(subHeaderId);
                grdQuestion.Visible = false;
                btnAddQuestion.Visible = false;
                BindSubHeaders(parentKnowledgeBaseId);
                Message.Success("Sub-Header Deleted Successfully.");

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Sub-Header Cannot be Deleted. Please Try Again.");
            }
        }

        public void grdQuestion_NewRow(object sender, EventArgs e)
        {
            if (grdQuestion.FooterRow == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("ParentKnowledgeBaseId");
                dt.Columns.Add("Name");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                grdQuestion.DataSource = dt;
                grdQuestion.DataBind();
                grdQuestion.Rows[0].Visible = false;
                grdQuestion.Rows[0].Controls.Clear();
            }

            grdQuestion.FooterRow.Visible = true;
            grdQuestion.Columns[1].Visible = true;

            grdHeader.EditIndex = -1;
            BindHeaders();
            int parentKnowledgeBaseId = Convert.ToInt32(Session["headerId"]);
            grdSubHeader.EditIndex = -1;
            BindSubHeaders(parentKnowledgeBaseId);

        }

        protected void grdQuestion_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int parentKnowledgeBaseId = Convert.ToInt32(Session["subHeaderId"]);
                grdQuestion.EditIndex = e.NewEditIndex;
                BindQuestions(parentKnowledgeBaseId);

                grdHeader.EditIndex = -1;
                BindHeaders();
                int HeaderId = Convert.ToInt32(Session["headerId"]);
                grdSubHeader.EditIndex = -1;
                BindSubHeaders(HeaderId);

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdQuestion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                int parentKnowledgeBaseId = Convert.ToInt32(Session["subHeaderId"]);
                grdQuestion.EditIndex = -1;
                BindQuestions(parentKnowledgeBaseId);
                Message.Clear("");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void grdQuestion_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TemplatesBO templates = new TemplatesBO();
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                int subHeaderId = Convert.ToInt32(Session["subHeaderId"]);
                int questionId = Convert.ToInt32(grdQuestion.DataKeys[e.RowIndex].Values[0].ToString());

                TextBox questionText = (TextBox)grdQuestion.Rows[e.RowIndex].FindControl("txtEditQuestion");

                if (!Regex.IsMatch(questionText.Text, @"^[a-zA-Z0-9\-\+\\*\\>\\<\\'\\_\\/\\.\s\\(\\)\\:\\,\[\]\’]+$"))
                {
                    Message.Error("Question name is invalid. Please try again!");
                    return;
                }

                if (!templates.IsQuestionExist(questionText.Text, templateId, subHeaderId, questionId))
                {
                    Message.Error("Question Name Already Exist. Please Add New Question With Different Name.");
                    return;
                }
                
                templates.UpdateQuestions(questionId, questionText.Text);
                grdQuestion.EditIndex = -1;
				BindQuestions(subHeaderId);

                Message.Success("Question Updated Successfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Question Cannot be Updated. Please Try Again.");
            }
        }

        protected void grdQuestion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int questionId = Convert.ToInt32(grdQuestion.DataKeys[e.RowIndex].Values[0].ToString());
				int parentKnowledgeBaseId = Convert.ToInt32(Session["subHeaderId"]);

                TemplatesBO templates = new TemplatesBO();
                templates.DeleteQuestion(questionId);
                BindQuestions(parentKnowledgeBaseId);

                Message.Success("Question Deleted Successfully.");
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                Message.Error("Question Cannot be Deleted. Please Try Again.");
            }
        }

        #endregion

        #region FUNCTIONS

        public void LoadingEnterprise()
        {

            ddTemplateAccess.Enabled = true;
            PracticeBO practiceBO = new PracticeBO();
            _enterpriseList = practiceBO.GetEnterprises();
            ddlEnterprise.DataTextField = "Name";
            ddlEnterprise.DataValueField = "ID";
            ddlEnterprise.DataSource = _enterpriseList;
            ddlEnterprise.DataBind();

            //Add Default item in comboBox
            ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlTemplates.Items.Insert(0, new ListItem("--Select--", "0"));

            ddlTemplates.Enabled = false;
            lblEnterprise.Visible = ddlEnterprise.Visible = true;

        }

        public void LoadingProcess()
        {
            enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);

            if (Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                string ContentType = Request.QueryString["NodeContentType"] != null ? Request.QueryString["NodeContentType"] : string.Empty;
                int SectionId = Request.QueryString["sectionId"] != null && Request.QueryString["sectionId"].ToString() != string.Empty ? Convert.ToInt32(Request.QueryString["sectionId"]) : 0;

                userId = Session[enSessionKey.UserApplicationId.ToString()] != null ? Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]) : 0;
            }
            else
            {
                SessionHandling _sessionHandling = new SessionHandling();
                _sessionHandling.ClearSession();
                Response.Redirect("~/Account/Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                string userType = Session["UserType"].ToString();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    btnTemplate.Enabled = false;
                    btnTemplate.CssClass = "top-button-yollaw-disable";
                    LoadingEnterprise();
                }
                else
                    lblEnterprise.Visible = ddlEnterprise.Visible = false;

                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.Consultant.ToString())
                {
                    GetTemplates(enterpriseId);
                }

            }
            Message.Clear("");

        }

        public void GetTemplates(int enterpriseId)
        {
            try
            {
                lblTemplates.Visible = ddlTemplates.Visible = true;

                _template = new TemplatesBO();
                IQueryable _templateList;
                _templateList = _template.GetTemplate(Session["UserType"].ToString(), enterpriseId);

                ddlTemplates.DataTextField = "Name";
                ddlTemplates.DataValueField = "TemplateId";
                ddlTemplates.DataSource = _templateList;
                ddlTemplates.DataBind();

                ddlTemplates.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        public void BindHeaders()
        {
            try
            {
                TemplatesBO header = new TemplatesBO();
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                grdHeader.DataSource = header.GetAllHeaders(templateId);
                grdHeader.DataBind();

                if (grdHeader.Rows.Count > 0)
                {
                    btnUploadSheet.Enabled = false;
                    btnUploadSheet.CssClass = "top-button-yollaw-disable";
                }
                else
                {
                    btnUploadSheet.Enabled = true;
                    btnUploadSheet.CssClass = "top-button-yollaw";
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void BindSubHeaders(int parentKnowledgeBaseId)
        {
            try
            {
                TemplatesBO header = new TemplatesBO();
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                grdSubHeader.DataSource = header.GetAllSubHeaders(parentKnowledgeBaseId, templateId);
                //grdSubHeader.EditIndex = -1;
                grdSubHeader.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void BindQuestions(int parentKnowledgeBaseId)
        {
            try
            {
                TemplatesBO header = new TemplatesBO();
                int templateId = Convert.ToInt32(Session["TemplateId"]);
                grdQuestion.DataSource = header.GetAllQuestions(parentKnowledgeBaseId, templateId);
                //grdQuestion.EditIndex = -1;
                grdQuestion.DataBind();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}