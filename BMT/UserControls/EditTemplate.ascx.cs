using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using BMTBLL;
using BMT.WEB;
using System.Data;
using BMTBLL.Enumeration;

public partial class EditTemplate : System.Web.UI.UserControl
{
    #region PROPERTIES
    public string UserType { get; set; }
    public ArrayList KbIdList = new ArrayList();
    #endregion

    #region CONSTANTS
    private int EDITCOLINDEX = 1;
    private int FIRSTROWINDEX = 0;
    private int KEYINDEX = 0;
    private int HEADERPARENTID = 0;
    #endregion

    #region VARIABLES
    private KnowledgeBaseBO _knowledgeBase = new KnowledgeBaseBO();
    private int tempId;
    private int kbId;
    private int knowledgeBaseId;
    private int subHeaderParendId;
    private int subHeaderParendIdBack;
    private int questionParentId;
    private int questionParentIdBack;
    private int parentHeaderId;
    private int parentSubHeaderId;
    private int parentId;
    private int grandParentId;
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadingGrids();
    }

    protected void gvHeader_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                Session["SubHeaderParentId"] = subHeaderParendId = Convert.ToInt32(e.CommandArgument.ToString());
                hiddenParentId.Value= hdnHeaderId.Value = hiddenSubHeaderParent.Value = Session["SubHeaderParentId"].ToString();
                gvSubHeader.Visible = true;
                hdnSubHeaderId.Value = "";
                BindSubHeader(subHeaderParendId);
                for (int row = 0; row < gvHeader.Rows.Count; row++)
                {
                    if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                    {
                        gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        if (row % 2 == 0)
                        {
                            gvHeader.Rows[row].BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            gvHeader.Rows[row].BackColor = System.Drawing.Color.FromName("#F2F2F2");
                        }
                    }
                }
            }
            if (e.CommandName == "Add")
            {
                Session["isEditOrAdd"] = "Edit";
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void gvHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            knowledgeBaseId = Convert.ToInt32(gvHeader.DataKeys[e.RowIndex].Values[KEYINDEX].ToString());
            _knowledgeBase = new KnowledgeBaseBO();
            _knowledgeBase.DeleteKBElement(knowledgeBaseId, tempId);
            GetHeader(UserType);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvHeader_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvHeader.PageIndex = e.NewPageIndex;
            hdnHeaderId.Value = "";
            GetHeader(UserType);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvSubHeader_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                Session["QuestionParentId"] = questionParentId = Convert.ToInt32(e.CommandArgument.ToString());
                hiddenParentId.Value = hdnSubHeaderId.Value = hiddenQuestionParent.Value = Session["QuestionParentId"].ToString();
                gvSubHeader.Visible = true;
                gvQuestion.Visible = true;
                BindQuestion(questionParentId);
                for (int row = 0; row < gvHeader.Rows.Count; row++)
                {
                    subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                    if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                    {
                        BindSubHeader(subHeaderParendId);
                        gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                for (int row = 0; row < gvSubHeader.Rows.Count; row++)
                {
                    if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentId)
                    {
                        gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
            }
            if (e.CommandName == "Add")
            {
                Session["isEditOrAdd"] = "Edit";
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void gvSubHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            knowledgeBaseId = Convert.ToInt32(gvSubHeader.DataKeys[e.RowIndex].Values[KEYINDEX].ToString());
            _knowledgeBase = new KnowledgeBaseBO();
            _knowledgeBase.DeleteKBElement(knowledgeBaseId, tempId);
            gvSubHeader.Visible = true;
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    BindSubHeader(subHeaderParendId);
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvSubHeader_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvSubHeader.PageIndex = e.NewPageIndex;
            gvSubHeader.Visible = true;
            hdnSubHeaderId.Value = "";
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    BindSubHeader(subHeaderParendId);
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvQuestion_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                gvSubHeader.Visible = true;
                gvQuestion.Visible = true;
                int QuesId = Convert.ToInt32(e.CommandArgument.ToString());
                for (int row = 0; row < gvHeader.Rows.Count; row++)
                {
                    subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                    if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                    {
                        gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                for (int row = 0; row < gvSubHeader.Rows.Count; row++)
                {
                    questionParentId = Convert.ToInt32(hiddenQuestionParent.Value);
                    if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentId)
                    {
                        BindQuestion(questionParentId);
                        gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                for (int row = 0; row < gvQuestion.Rows.Count; row++)
                {
                    if (Convert.ToInt32(gvQuestion.DataKeys[row].Value) == QuesId)
                    {
                        gvQuestion.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
            }
            if (e.CommandName == "Add")
            {
                Session["isEditOrAdd"] = "Edit";
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected void gvQuestion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            knowledgeBaseId = Convert.ToInt32(gvQuestion.DataKeys[e.RowIndex].Values[KEYINDEX].ToString());
            _knowledgeBase = new KnowledgeBaseBO();
            _knowledgeBase.DeleteKBElement(knowledgeBaseId, tempId);
            gvSubHeader.Visible = true;
            gvQuestion.Visible = true;
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
            for (int row = 0; row < gvSubHeader.Rows.Count; row++)
            {
                questionParentId = Convert.ToInt32(hiddenQuestionParent.Value);
                if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentId)
                {
                    BindQuestion(questionParentId);
                    gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvQuestion_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvQuestion.PageIndex = e.NewPageIndex;
            gvSubHeader.Visible = true;
            gvQuestion.Visible = true;
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
            for (int row = 0; row < gvSubHeader.Rows.Count; row++)
            {
                questionParentId = Convert.ToInt32(hiddenQuestionParent.Value);
                if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentId)
                {
                    BindQuestion(questionParentId);
                    gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void btnSaveHeader_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (hdnKBType.Value == "Header")
            {
                SaveHeaderInTemplate();
            }
            else if (hdnKBType.Value == "SubHeader")
            {
                SaveSubHeaderInTemplate();
            }
            else if (hdnKBType.Value == "Question")
            {
                SaveQuestionInTemplate();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnSaveAll_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (hdnKBType.Value == "Header")
            {
                if (((CheckBox)gvHeader.HeaderRow.FindControl("chkHeader")).Checked)
                {
                    for (int row = 0; row < gvHeader.Rows.Count; row++)
                    {
                        ((CheckBox)gvHeader.Rows[row].FindControl("chkHeaderName")).Checked = true;
                    }
                    ((CheckBox)gvHeader.HeaderRow.FindControl("chkHeader")).Checked = true;
                }
                else
                {
                    for (int row = 0; row < gvHeader.Rows.Count; row++)
                    {
                        ((CheckBox)gvHeader.Rows[row].FindControl("chkHeaderName")).Checked = false;
                    }
                    ((CheckBox)gvHeader.HeaderRow.FindControl("chkHeader")).Checked = false;
                }
                SaveHeaderInTemplate();
            }
            else if (hdnKBType.Value == "SubHeader")
            {
                if (((CheckBox)gvSubHeader.HeaderRow.FindControl("chkSubHeader")).Checked)
                {
                    for (int row = 0; row < gvSubHeader.Rows.Count; row++)
                    {
                        ((CheckBox)gvSubHeader.Rows[row].FindControl("chkSubHeaderName")).Checked = true;
                    }
                    ((CheckBox)gvSubHeader.HeaderRow.FindControl("chkSubHeader")).Checked = true;
                }
                else
                {
                    for (int row = 0; row < gvSubHeader.Rows.Count; row++)
                    {
                        ((CheckBox)gvSubHeader.Rows[row].FindControl("chkSubHeaderName")).Checked = false;
                    }
                    ((CheckBox)gvSubHeader.HeaderRow.FindControl("chkSubHeader")).Checked = false;
                }
                SaveSubHeaderInTemplate();
            }
            else if (hdnKBType.Value == "Question")
            {
                if (((CheckBox)gvQuestion.HeaderRow.FindControl("chkQuestion")).Checked)
                {
                    for (int row = 0; row < gvQuestion.Rows.Count; row++)
                    {
                        ((CheckBox)gvQuestion.Rows[row].FindControl("chkQuestionName")).Checked = true;
                    }
                    ((CheckBox)gvQuestion.HeaderRow.FindControl("chkQuestion")).Checked = true;
                }
                else
                {
                    for (int row = 0; row < gvQuestion.Rows.Count; row++)
                    {
                        ((CheckBox)gvQuestion.Rows[row].FindControl("chkQuestionName")).Checked = false;
                    }
                    ((CheckBox)gvQuestion.HeaderRow.FindControl("chkQuestion")).Checked = false;
                }
                SaveQuestionInTemplate();
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    #endregion

    #region FUNCTIONS
    public void GetHeader(string userType)
    {
        try
        {
            _knowledgeBase = new KnowledgeBaseBO();

            IQueryable _headerList = _knowledgeBase.GetHeader(userType);


            gvHeader.DataSource = _headerList;
            gvHeader.DataBind();

            if (gvHeader.Rows.Count == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("Name");
                dt.Columns.Add("KnowledgeBaseTypeId");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                gvHeader.DataSource = dt;
                gvHeader.Columns[EDITCOLINDEX].Visible = false;
                gvHeader.DataBind();
                gvHeader.Rows[FIRSTROWINDEX].Visible = false;
            }
            else
            {
                gvHeader.Rows[FIRSTROWINDEX].Visible = true;
                gvHeader.Columns[EDITCOLINDEX].Visible = true;
                for (int index = 0; index < gvHeader.Rows.Count; index++)
                {
                    int kbId = Convert.ToInt32(gvHeader.DataKeys[index].Value);
                    CheckBox chsubmit = (CheckBox)gvHeader.Rows[index].FindControl("chkHeaderName");
                    parentId = 0;
                    grandParentId = 0;
                    if (_knowledgeBase.IsKBExist(tempId, kbId, parentId, grandParentId))
                    {
                        chsubmit.Checked = true;
                        ImageButton img = (ImageButton)gvHeader.Rows[index].FindControl("btnEdit");
                        img.Attributes.Remove("diabled");
                        img.ImageUrl = "~/Themes/Images/Edit-16.png";
                    }
                    else
                    {
                        chsubmit.Checked = false;
                        ImageButton img = (ImageButton)gvHeader.Rows[index].FindControl("btnEdit");
                        img.Attributes.Add("disabled", "disabled");
                        img.ImageUrl = "~/Themes/Images/edit-Disabled-16.png";
                    }
                }
                if (hdnHeaderId.Value.ToString() != "")
                {
                    subHeaderParendIdBack = Convert.ToInt32(hdnHeaderId.Value);
                    for (int row = 0; row < gvHeader.Rows.Count; row++)
                    {
                        if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendIdBack)
                        {
                            BindSubHeader(subHeaderParendIdBack);
                            gvSubHeader.Visible = true;
                            gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                        }
                    }
                }
                else
                {
                    gvSubHeader.Visible = false;
                    gvQuestion.Visible = false;
                }
            }
        }
        catch (Exception exception)
        { throw exception; }
    }

    public void BindSubHeader(int parentId)
    {
        try
        {
            _knowledgeBase = new KnowledgeBaseBO();
            tempId = Convert.ToInt32(Session["TemplateId"].ToString());

            IQueryable _bindsubHeaderList = _knowledgeBase.BindSubHeader(parentId, UserType);

            gvSubHeader.DataSource = _bindsubHeaderList;
            gvSubHeader.DataBind();

            if (gvSubHeader.Rows.Count == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("Name");
                dt.Columns.Add("KnowledgeBaseTypeId");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                gvSubHeader.DataSource = dt;

                gvSubHeader.Columns[EDITCOLINDEX].Visible = false;
                gvSubHeader.DataBind();
                gvSubHeader.Rows[FIRSTROWINDEX].Visible = false;
                gvQuestion.Visible = false;
            }
            else
            {
                gvSubHeader.Columns[EDITCOLINDEX].Visible = true;
                for (int index = 0; index < gvSubHeader.Rows.Count; index++)
                {
                    int kbId = Convert.ToInt32(gvSubHeader.DataKeys[index].Value);
                    CheckBox chsubmit = (CheckBox)gvSubHeader.Rows[index].FindControl("chkSubHeaderName");
                    parentId = Convert.ToInt32(Session["SubHeaderParentId"].ToString());
                    grandParentId = 0;
                    if (_knowledgeBase.IsKBExist(tempId, kbId, parentId, grandParentId))
                    {
                        chsubmit.Checked = true;
                        ImageButton img = (ImageButton)gvSubHeader.Rows[index].FindControl("btnEdit");
                        img.Attributes.Remove("diabled");
                        img.ImageUrl = "~/Themes/Images/Edit-16.png";
                    }
                    else
                    {
                        chsubmit.Checked = false;
                        ImageButton img = (ImageButton)gvSubHeader.Rows[index].FindControl("btnEdit");
                        img.Attributes.Add("disabled", "disabled");
                        img.ImageUrl = "~/Themes/Images/edit-Disabled-16.png";
                    }
                }
                if (hdnSubHeaderId.Value.ToString() != "")
                {
                    questionParentIdBack = Convert.ToInt32(hdnSubHeaderId.Value);
                    for (int row = 0; row < gvSubHeader.Rows.Count; row++)
                    {
                        if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentIdBack)
                        {
                            BindQuestion(questionParentIdBack);
                            gvSubHeader.Visible = true;
                            gvQuestion.Visible = true;
                            gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                        }
                    }
                }
                else
                {
                    gvQuestion.Visible = false;
                }
            }
        }
        catch (Exception exception)
        { throw exception; }
    }

    public void BindQuestion(int parentId)
    {
        try
        {
            _knowledgeBase = new KnowledgeBaseBO();
            tempId = Convert.ToInt32(Session["TemplateId"].ToString());

            IQueryable _bindquestionList = _knowledgeBase.BindQuestion(parentId, UserType);

            gvQuestion.DataSource = _bindquestionList;
            gvQuestion.DataBind();

            if (gvQuestion.Rows.Count == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgeBaseId");
                dt.Columns.Add("Name");
                dt.Columns.Add("KnowledgeBaseTypeId");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                gvQuestion.DataSource = dt;
                gvQuestion.Columns[EDITCOLINDEX].Visible = false;
                gvQuestion.DataBind();
                gvQuestion.Rows[FIRSTROWINDEX].Visible = false;
            }
            else
            {
                gvQuestion.Columns[EDITCOLINDEX].Visible = true;
                for (int index = 0; index < gvQuestion.Rows.Count; index++)
                {
                    int kbId = Convert.ToInt32(gvQuestion.DataKeys[index].Value);
                    CheckBox chsubmit = (CheckBox)gvQuestion.Rows[index].FindControl("chkQuestionName");
                    parentId = Convert.ToInt32(Session["QuestionParentId"].ToString());
                    grandParentId = Convert.ToInt32(Session["SubHeaderParentId"].ToString());
                    if (_knowledgeBase.IsKBExist(tempId, kbId, parentId, grandParentId))
                    {
                        chsubmit.Checked = true;
                        ImageButton img = (ImageButton)gvQuestion.Rows[index].FindControl("btnEdit");
                        img.Attributes.Remove("diabled");
                        img.ImageUrl = "~/Themes/Images/Edit-16.png";
                    }
                    else
                    {
                        chsubmit.Checked = false;
                        ImageButton img = (ImageButton)gvQuestion.Rows[index].FindControl("btnEdit");
                        img.Attributes.Add("disabled", "disabled");
                        img.ImageUrl = "~/Themes/Images/edit-Disabled-16.png";
                    }
                }
            }
            upnlEditTemp.Update();
        }
        catch (Exception exception)
        { throw exception; }
    }

    public void SaveHeaderInTemplate()
    {
        try
        {
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                if (gvHeader.DataKeys[row].Value.ToString() != "")
                {
                    if (((CheckBox)gvHeader.Rows[row].FindControl("chkHeaderName")).Checked)
                    {
                        kbId = Convert.ToInt32(gvHeader.DataKeys[row].Value);
                        _knowledgeBase.SaveHeaderInKBTemplate(tempId, kbId);
                    }
                    else
                    {
                        kbId = Convert.ToInt32(gvHeader.DataKeys[row].Value);
                        _knowledgeBase.DeleteKBFromTemplate(tempId, kbId, HEADERPARENTID);
                    }
                }
            }
            hdnNotEditTempLoads.Value = "false";
            GetHeader(UserType);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public void SaveSubHeaderInTemplate()
    {
        try
        {
            parentHeaderId = Convert.ToInt32(Session["SubHeaderParentId"].ToString());
            for (int row = 0; row < gvSubHeader.Rows.Count; row++)
            {
                if (gvSubHeader.DataKeys[row].Value.ToString() !="")
                {
                    if (((CheckBox)gvSubHeader.Rows[row].FindControl("chkSubHeaderName")).Checked)
                    {
                        kbId = Convert.ToInt32(gvSubHeader.DataKeys[row].Value);

                        if (!_knowledgeBase.SaveSubHeaderInKBTemplate(tempId, kbId, parentHeaderId))
                        {
                            messageEditTemplate.Error("You cannot select this Sub-Header because this is already selected in this template.");
                        }
                    }
                    else
                    {
                        kbId = Convert.ToInt32(gvSubHeader.DataKeys[row].Value);
                        _knowledgeBase.DeleteKBFromTemplate(tempId, kbId, parentHeaderId);
                    }
                }
            }
            hdnNotEditTempLoads.Value = "false";
            GetHeader(UserType);
            gvSubHeader.Visible = true;
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    BindSubHeader(subHeaderParendId);
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public void SaveQuestionInTemplate()
    {
        try
        {
            parentHeaderId = Convert.ToInt32(Session["SubHeaderParentId"].ToString());
            parentSubHeaderId = Convert.ToInt32(Session["QuestionParentId"].ToString());

            for (int row = 0; row < gvQuestion.Rows.Count; row++)
            {
                if (gvQuestion.DataKeys[row].Value.ToString() != "")
                {
                    if (((CheckBox)gvQuestion.Rows[row].FindControl("chkQuestionName")).Checked)
                    {
                        kbId = Convert.ToInt32(gvQuestion.DataKeys[row].Value);
                        if (!_knowledgeBase.SaveQuestionInKBTemplate(tempId, kbId, parentSubHeaderId, parentHeaderId))
                        {
                            messageEditTemplate.Error("You cannot select this Question because this is already selected in this template.");
                        }
                    }
                    else
                    {
                        kbId = Convert.ToInt32(gvQuestion.DataKeys[row].Value);
                        _knowledgeBase.DeleteKBFromTemplate(tempId, kbId, parentSubHeaderId);
                    }
                }
            }
            hdnNotEditTempLoads.Value = "false";
            GetHeader(UserType);
            for (int row = 0; row < gvHeader.Rows.Count; row++)
            {
                subHeaderParendId = Convert.ToInt32(hiddenSubHeaderParent.Value);
                if (Convert.ToInt32(gvHeader.DataKeys[row].Value) == subHeaderParendId)
                {
                    BindSubHeader(subHeaderParendId);
                    gvSubHeader.Visible = true;
                    gvHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }
            for (int row = 0; row < gvSubHeader.Rows.Count; row++)
            {
                questionParentId = Convert.ToInt32(hiddenQuestionParent.Value);
                if (Convert.ToInt32(gvSubHeader.DataKeys[row].Value) == questionParentId)
                {
                    BindQuestion(questionParentId);
                    gvQuestion.Visible = true;
                    gvSubHeader.Rows[row].BackColor = System.Drawing.Color.LightYellow;
                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public string GetTemplateName(int tempId)
    {
        try
        {
            _knowledgeBase = new KnowledgeBaseBO();
            return _knowledgeBase.GetTemplateName(tempId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public void LoadingGrids()
    {
        if (Session["TemplateId"] != null)
        {
            tempId = Convert.ToInt32(Session["TemplateId"].ToString());
            hdnTempId.Value = tempId.ToString();
            messageEditTemplate.Clear("");
            tempName.Text = GetTemplateName(tempId);
            gvSubHeader.Columns[EDITCOLINDEX].Visible = true;
            gvQuestion.Columns[EDITCOLINDEX].Visible = true;
            upnlEditTemp.Update();
            gvSubHeader.Visible = false;
            gvQuestion.Visible = false;
            if (hdnNotEditTempLoads.Value.ToString() != "true")
            {
                GetHeader(UserType);
            }
        }
    }

    #endregion

}
