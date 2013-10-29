using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMTBLL;
using BMT.WEB;
using System.Data;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class KnowledgeBaseViewer : System.Web.UI.Page
    {
        #region VARIABLES
        private KnowledgeBaseBO _knowledgeBase = new KnowledgeBaseBO();
        private IQueryable _List;
        private int knowledgebaseId;
        private int templateId;
        private int had;
        private string userType;
        private int userId;
        private int medicalGroupId;
        private int practiceId;
        private int numLength;
        private int kbAnswerEdit;
        private KnowledgeBaseTemplate kbtempForEdit;
        private KnowledgeBase kbForEdit;
        #endregion

        #region CONSTANT
        private int TOTAL_LENGHT_OF_DESCRIPTION = 275;
        private int FIRSTCOLINDEX = 0;
        private int SECONDCOLINDEX = 1;
        private int THIRDCOLINDEX = 2;
        private int FOURTHCOLINDEX = 3;
        private int FIFTHCOLINDEX = 4;
        private int ADDKBID = 0;
        private int FIRSTROWINDEXVAL = 0;
        private int NOPARENT = 0;
        private int NOGRANDPARENT = 0;
        #endregion

        # region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["TemplateId"].ToString() != "")
                {
                    hdnTemplateId.Value = Session["TemplateId"].ToString();
                    templateId = Convert.ToInt32(Session["TemplateId"].ToString());
                }
                if (Session["UserApplicationId"].ToString() != "")
                {
                    hdnUserId.Value = Session["UserApplicationId"].ToString();
                }
                knowledgebaseId = Convert.ToInt32(Request.QueryString["kbId"]);

                hdnkbId.Value = knowledgebaseId.ToString();
                had = Convert.ToInt32(Request.QueryString["KnowledgebaseType"]);

                CheckForEditingCriteria(knowledgebaseId, templateId);

                if (had == (int)enKBType.Header)
                {
                    GetHeaderInfo(knowledgebaseId, templateId);
                    MustPass.Style.Add("display", "none");
                    Critical.Style.Add("display", "none");
                    infoDocument.Style.Add("display", "none");
                    CriticalToolTip.Style.Add("display", "none");
                    pageReference.Style.Add("display", "none");
                    right.Style.Add("display", "none");
                    Tab.Style.Add("display", "visible");
                    hdnkbTypeId.Value = "Header";
                    hdnParentId.Value = null;
                }
                else if (had == (int)enKBType.SubHeader)
                {
                    GetSubHeaderInfo(knowledgebaseId, templateId);
                    MustPass.Style.Add("display", "visible");
                    Critical.Style.Add("display", "none");
                    CriticalToolTip.Style.Add("display", "none");
                    infoDocument.Style.Add("display", "visible");
                    pageReference.Style.Add("display", "visible");
                    right.Style.Add("display", "none");
                    Tab.Style.Add("display", "none");
                    hdnkbTypeId.Value = "SubHeader";
                    if (Session["SubHeaderParentId"] != null)
                    {
                        hdnParentId.Value = Session["SubHeaderParentId"].ToString();
                    }
                }
                else if (had == (int)enKBType.Question)
                {
                    GetQuestionInfo(knowledgebaseId, templateId);
                    MustPass.Style.Add("display", "none");
                    Critical.Style.Add("display", "visible");
                    CriticalToolTip.Style.Add("display", "visible");
                    pageReference.Style.Add("display", "none");
                    infoDocument.Style.Add("display", "none");
                    right.Style.Add("display", "visible");
                    Tab.Style.Add("display", "none");
                    hdnkbTypeId.Value = "Question";
                    if (Session["QuestionParentId"] != null)
                    {
                        hdnGrandParentId.Value = Session["SubHeaderParentId"].ToString();
                    }
                    if (Session["QuestionParentId"] != null)
                    {
                        hdnParentId.Value = Session["QuestionParentId"].ToString();
                    }
                }
                if (knowledgebaseId == ADDKBID)
                {
                    databoxHeader.Attributes.Add("disabled", "disabled");
                    CriticalToolTip.Attributes.Add("disabled", "disabled");
                    txtDataBoxHeader.Attributes.Add("disabled", "disabled");
                    txtToolTip.Attributes.Add("disabled", "disabled");
                    pageReference.Attributes.Add("disabled", "disabled");
                    txtPageReference.Attributes.Add("disabled", "disabled");
                    EnterKnowledgebaseElement(templateId, had);
                    headingText.InnerHtml = "Enter Knowledge Base Information";
                    knowledgebaseForm.InnerHtml = "Configure for Current Template";
                    hdnIsEditOrAdd.Value = "Add";
                }
                else
                {
                    databoxHeader.Attributes.Add("disabled", "disabled");
                    CriticalToolTip.Attributes.Add("disabled", "disabled");
                    txtDataBoxHeader.Attributes.Add("disabled", "disabled");
                    txtToolTip.Attributes.Add("disabled", "disabled");
                    pageReference.Attributes.Add("disabled", "disabled");
                    txtPageReference.Attributes.Add("disabled", "disabled");
                    GetHeaderByKnowledgebaseId(knowledgebaseId, templateId);
                    headingText.InnerHtml = "Knowledge Base Information";
                    knowledgebaseForm.InnerHtml = "Edit for Current Template";
                    hdnIsEditOrAdd.Value = "Edit";
                }

            }
        }

        protected void gvkbInfo_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {

                        int index = Convert.ToInt32(e.CommandArgument);

                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void gvkbInfo_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                gvkbInfo.PageIndex = e.NewPageIndex;
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void HeaderInfo_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {

                        int index = Convert.ToInt32(e.CommandArgument);

                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void HeaderInfo_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                HeaderInfo.PageIndex = e.NewPageIndex;
                knowledgebaseId = Convert.ToInt32(hdnkbId.Value);
                templateId = Convert.ToInt32(hdnTemplateId.Value);
                GetHeaderInfo(knowledgebaseId, templateId);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void SubHeaderInfo_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {

                        int index = Convert.ToInt32(e.CommandArgument);

                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void SubHeaderInfo_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                SubHeaderInfo.PageIndex = e.NewPageIndex;
                knowledgebaseId = Convert.ToInt32(hdnkbId.Value);
                templateId = Convert.ToInt32(hdnTemplateId.Value);
                GetSubHeaderInfo(knowledgebaseId, templateId);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void QuestionInfo_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (Page.IsPostBack)
                    {

                        int index = Convert.ToInt32(e.CommandArgument);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void QuestionInfo_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                QuestionInfo.PageIndex = e.NewPageIndex;
                knowledgebaseId = Convert.ToInt32(hdnkbId.Value);
                templateId = Convert.ToInt32(hdnTemplateId.Value);
                GetQuestionInfo(knowledgebaseId, templateId);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            RadioButton rbExisting = new RadioButton();
            RadioButton rbAddKB = new RadioButton();
            rbExisting = (RadioButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("rdoAddExistingKB"));
            rbAddKB = (RadioButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("rdoAddNewKB"));
            if (rbExisting.Checked)
            {
                txtDisplayName.Text = ((TextBox)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("txtExistingKBName"))).Text;
                if (txtDisplayName.Text != "")
                {
                    KnowledgeBaseBO kbn = new KnowledgeBaseBO();
                    KnowledgeBase kb = new KnowledgeBase();
                    kb = kbn.GetKBElementByKBName(txtDisplayName.Text);
                    hdnkbId.Value = kb.KnowledgeBaseId.ToString();
                    txtInstruction.InnerText = kb.InstructionText;
                    if (kb.KnowledgeBaseTypeId == 1)
                    {
                        txtTabLine1.Text = kb.TabName;
                    }
                    else if (kb.KnowledgeBaseTypeId == 2)
                    {
                        if (kb.MustPass != null)
                        {
                            if ((bool)kb.MustPass)
                            {
                                rdoMustPassYes.Checked = true;
                                rdoMustPassNo.Checked = false;
                            }
                            else
                            {
                                rdoMustPassYes.Checked = false;
                                rdoMustPassNo.Checked = true;
                            }
                        }
                    }
                    //else if (kb.KnowledgeBaseTypeId == 3)
                    //{
                    //    if (kb.IsCritical != null)
                    //    {
                    //        if ((bool)kb.IsCritical)
                    //        {
                    //            rdoCriticalYes.Checked = true;
                    //            rdoCriticalNo.Checked = false;
                    //        }
                    //        else
                    //        {
                    //            rdoCriticalYes.Checked = false;
                    //            rdoCriticalNo.Checked = true;
                    //        }
                    //    }
                    //}

                    txtDisplayName.Attributes.Add("readonly", "readonly");
                    txtInstruction.Attributes.Add("readonly", "readonly");
                    txtTabLine1.Attributes.Add("readonly", "readonly");
                    //rdoCriticalNo.Attributes.Add("disabled", "disabled");
                    //rdoCriticalYes.Attributes.Add("disabled", "disabled");
                    rdoMustPassNo.Attributes.Add("disabled", "disabled");
                    rdoMustPassYes.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    EditTemplateMessage.Error("Existing knowledge base is not selected.");
                }
                ImageButton imgSearch = ((ImageButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("KBSearch")));
                imgSearch.OnClientClick = "javascript:DisplayHeaderPopUp(); return false;";
            }
            else if (rbAddKB.Checked)
            {
                txtDisplayName.Text = ((TextBox)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("txtAddNewKB"))).Text;
                txtTabLine1.Text = "";
                txtInstruction.InnerText = "";
                txtDisplayName.Attributes.Remove("readonly");
                txtInstruction.Attributes.Remove("readonly");
                txtTabLine1.Attributes.Remove("readonly");
                rdoCriticalNo.Attributes.Remove("disabled");
                rdoCriticalYes.Attributes.Remove("disabled");
                rdoMustPassNo.Attributes.Remove("disabled");
                rdoMustPassYes.Attributes.Remove("disabled");
                if (rdoCriticalYes.Checked)
                {
                    CriticalToolTip.Attributes.Remove("disabled");
                    txtToolTip.Attributes.Remove("disabled");
                    txtToolTip.Attributes.Remove("readonly");
                }
                ImageButton imgSearch = ((ImageButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("KBSearch")));
                imgSearch.OnClientClick = "javascript:return false";
            }
            else
                EditTemplateMessage.Error("Select that the new Knowledge Base is from Existing or Add New");

            had = Convert.ToInt32(Request.QueryString["KnowledgebaseType"]);
            userType = Session["UserType"].ToString();
            practiceId = Convert.ToInt32(Session["PracticeId"]);
            medicalGroupId = Convert.ToInt32(Session["MedicalGroupId"]);
            GetHeaderList(had, userType, practiceId, medicalGroupId);
        }

        #endregion

        #region FUNCTIONS
        private void GetHeaderByKnowledgebaseId(int knowledgebaseId, int tempId)
        {
            try
            {
                _knowledgeBase = new KnowledgeBaseBO();
                IQueryable _headerList;
                _headerList = _knowledgeBase.GetHeaderByKnowledgebaseId(knowledgebaseId, tempId);
                kbForEdit = new KnowledgeBase();
                kbForEdit = _knowledgeBase.EditKbElement(knowledgebaseId);
                kbtempForEdit = _knowledgeBase.EditkbTemp(knowledgebaseId, tempId);
                gvkbInfo.DataSource = _headerList;
                gvkbInfo.DataBind();
                txtDisplayName.Text = kbForEdit.Name;
                txtTabLine1.Text = kbForEdit.TabName;
                txtInstruction.InnerText = kbForEdit.InstructionText;
                if (txtInstruction.InnerText != "")
                {
                    numLength = TOTAL_LENGHT_OF_DESCRIPTION - (kbForEdit.InstructionText).Length;
                    descriptionLength.Text = numLength + " characters left";
                }
                if (kbForEdit.MustPass != null)
                {
                    if ((bool)kbForEdit.MustPass)
                    {
                        rdoMustPassYes.Checked = true;
                        rdoMustPassNo.Checked = false;
                    }
                    else
                    {
                        rdoMustPassYes.Checked = false;
                        rdoMustPassNo.Checked = true;
                    }
                }
                if (kbtempForEdit.IsCritical != null)
                {
                    if ((bool)kbtempForEdit.IsCritical == true)
                    {
                        rdoCriticalNo.Checked = false;
                        rdoCriticalYes.Checked = true;
                        CriticalToolTip.Attributes.Remove("disabled");
                        txtToolTip.Attributes.Remove("disabled");
                    }
                    else
                    {
                        rdoCriticalYes.Checked = false;
                        rdoCriticalNo.Checked = true;
                    }
                }
                if (kbtempForEdit.CriticalTooltip != "")
                {
                    txtToolTip.Text = kbtempForEdit.CriticalTooltip;
                }
                else
                {
                    txtToolTip.Text = "";
                }
                if (kbtempForEdit.AnswerTypeId != 0)
                {
                    if (kbtempForEdit.AnswerTypeId == 1)
                    {
                        AnsYesNo.Checked = false;
                        AnsYesNoNA.Checked = true;
                        //AnsNone.Checked = false;
                    }
                    else if (kbtempForEdit.AnswerTypeId == 2)
                    {
                        AnsYesNoNA.Checked = false;
                        AnsYesNo.Checked = true;
                        //AnsNone.Checked = false;
                    }
                    else if (kbtempForEdit.AnswerTypeId == 3)
                    {
                        AnsYesNo.Checked = false;
                        AnsYesNoNA.Checked = false;
                        //AnsNone.Checked = true;
                    }
                }
                if (kbtempForEdit.IsDataBox != null)
                {
                    if ((bool)kbtempForEdit.IsDataBox == true)
                    {
                        databoxHeader.Attributes.Remove("disabled");
                        txtDataBoxHeader.Attributes.Remove("disabled");
                        txtDataBoxHeader.Text = kbtempForEdit.DataBoxHeader;
                        chkDataBox.Checked = true;
                    }
                }
                if (kbtempForEdit.IsInfoDocEnable != null)
                {
                    if ((bool)kbtempForEdit.IsInfoDocEnable == true)
                    {
                        rbInfoDocYes.Checked = true;
                        rbInfoDocNo.Checked = false;
                        pageReference.Attributes.Remove("disabled");
                        txtPageReference.Attributes.Remove("disabled");
                        txtPageReference.Text = kbtempForEdit.ReferencePages.ToString();
                    }
                    else
                    {
                        rbInfoDocNo.Checked = true;
                        rbInfoDocYes.Checked = false;
                        txtPageReference.Text = "";
                    }
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        private void GetHeaderInfo(int knowledgebaseId, int templateId)
        {
            try
            {
                _knowledgeBase = new KnowledgeBaseBO();

                _List = _knowledgeBase.GetHeaderInfo(knowledgebaseId, templateId);


                HeaderInfo.DataSource = _List;
                HeaderInfo.DataBind();


            }
            catch (Exception exception)
            { throw exception; }
        }

        private void GetSubHeaderInfo(int knowledgebaseId, int templateId)
        {
            try
            {
                _knowledgeBase = new KnowledgeBaseBO();

                _List = _knowledgeBase.GetHeaderInfo(knowledgebaseId, templateId);
                SubHeaderInfo.DataSource = _List;
                SubHeaderInfo.DataBind();
            }
            catch (Exception exception)
            { throw exception; }
        }

        private void GetQuestionInfo(int knowledgebaseId, int templateId)
        {
            try
            {
                _knowledgeBase = new KnowledgeBaseBO();

                _List = _knowledgeBase.GetHeaderInfo(knowledgebaseId, templateId);

                QuestionInfo.DataSource = _List;
                QuestionInfo.DataBind();

            }
            catch (Exception exception)
            { throw exception; }
        }

        private void EnterKnowledgebaseElement(int TempId, int kbTempType)
        {
            try
            {
                userType = Session["UserType"].ToString();
                practiceId = Convert.ToInt32(Session["PracticeId"]);
                medicalGroupId = Convert.ToInt32(Session["MedicalGroupId"]);
                KnowledgeBaseBO kbform = new KnowledgeBaseBO();
                DataTable dt = new DataTable();
                dt.Columns.Add("KnowledgebaseId");
                dt.Columns.Add("ParentId");
                dt.Columns.Add("KnowledgebaseType");
                dt.Columns.Add("AccessBy");
                dt.Columns.Add("CreatedBy");

                DataRow dr = dt.NewRow();
                dr[FIRSTCOLINDEX] = "NA";
                if (hdnParentId.Value == "")
                {
                    dr[SECONDCOLINDEX] = "NA";
                }
                else
                {
                    dr[SECONDCOLINDEX] = hdnParentId.Value;
                }
                if (kbTempType == 1)
                {
                    dr[THIRDCOLINDEX] = "Header";
                    searchPopup.InnerHtml = "Search Header";
                    text1.InnerHtml = "Important! Before adding an existing header to this Header, please consider adding the new Header to your template instead.";
                    text2.InnerHtml = "(Note:Existing Headers for this header can be added directly from your Edit Templates page and are displayed here for your information only)";
                    GetHeaderList(1, userType, practiceId, medicalGroupId);

                }
                else if (kbTempType == 2)
                {
                    dr[THIRDCOLINDEX] = "Sub Header";
                    searchPopup.InnerHtml = "Search Sub-Headers";
                    text1.InnerHtml = "Important! Before adding an existing sub-header to this Header, please consider adding the new Header to your template instead.";
                    text2.InnerHtml = "(Note: Existing sub-headers for this header can be added directly from your Edit Templates page and are displayed here for your information only)";
                    GetHeaderList(2, userType, practiceId, medicalGroupId);
                }
                else if (kbTempType == 3)
                {
                    dr[THIRDCOLINDEX] = "Question";
                    searchPopup.InnerHtml = "Search Questions";
                    text1.InnerHtml = "Important!Before adding an existing Question to this Sub-Header, please consider adding the new Sub-Header to your template instead.";
                    text2.InnerHtml = "(Note:Existing Questions for this Question can be added directly from your Edit Templates page and are displayed here for your information only)";
                    GetHeaderList(3, userType, practiceId, medicalGroupId);
                }
                dr[FOURTHCOLINDEX] = kbform.GetAccessBy(TempId);
                dr[FIFTHCOLINDEX] = kbform.GetCreatedBy(TempId);
                dt.Rows.Add(dr);

                if (kbTempType == (int)enKBType.Header)
                {
                    gvAddHeader.DataSource = dt;
                    gvAddHeader.DataBind();
                }
                else
                {
                    gvKbEnter.DataSource = dt;
                    gvKbEnter.DataBind();
                    ((TextBox)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("txtExistingKBName"))).Attributes.Add("readonly", "readonly");
                    RadioButton rbAddKB = (RadioButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("rdoAddNewKB"));
                    if (rbAddKB.Checked)
                    {
                        ImageButton imgKBSearch = (ImageButton)(gvKbEnter.Rows[FIRSTROWINDEXVAL].FindControl("KBSearch"));
                        imgKBSearch.Attributes.Add("disabled", "disabled");
                    }
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        protected void GetHeaderList(int kbType, string userType, int practiceId, int medicalGroupId)
        {
            if (kbType != 1)
            {
                Table KBTable = new Table();
                KnowledgeBaseBO kblist = new KnowledgeBaseBO();
                List<KnowledgeBase> kbHeaders = new List<KnowledgeBase>();
                if (kbType == 2)
                {
                    kbHeaders = kblist.GetHeaders(userType, medicalGroupId);
                }
                else if (kbType == 3)
                {
                    kbHeaders = kblist.GetSubHeaders(userType, medicalGroupId);
                }
                for (int kbHeaderCount = 0; kbHeaderCount < kbHeaders.Count; kbHeaderCount++)
                {
                    TableRow headers = new TableRow();

                    TableCell headerCell = new TableCell();

                    Label HeaderName = new Label();
                    HeaderName.Text = Convert.ToString(kbHeaders[kbHeaderCount].Name);
                    if (kbHeaders[kbHeaderCount].KnowledgeBaseId == Convert.ToInt32(hdnParentId.Value))
                    {
                        HeaderName.ForeColor = System.Drawing.Color.Gray;
                    }
                    headerCell.Controls.Add(HeaderName);
                    headers.Controls.Add(headerCell);
                    KBTable.Controls.Add(headers);
                    TableRow subHeaders = new TableRow();
                    TableCell subHeaderCell = new TableCell();
                    Table indentedTable = new Table();
                    indentedTable.CssClass = "indentedList";

                    List<KnowledgeBase> kbElementList = kblist.GetSubHeaderList(Convert.ToInt32(kbHeaders[kbHeaderCount].KnowledgeBaseId), userType, medicalGroupId);

                    for (int KBNameCount = 0; KBNameCount < kbElementList.Count; KBNameCount++)
                    {
                        TableRow KBName = new TableRow();
                        TableCell chkbxKB = new TableCell();

                        RadioButton rdo = new RadioButton();
                        rdo.ClientIDMode = ClientIDMode.Static;
                        rdo.Text = kbElementList[KBNameCount].Name;
                        rdo.GroupName = "ExistingKB";
                        if (kbHeaders[kbHeaderCount].KnowledgeBaseId == Convert.ToInt32(hdnParentId.Value))
                        {
                            rdo.Attributes.Add("disabled", "disabled");
                        }
                        else if (_knowledgeBase.IsKBExist(templateId, kbElementList[KBNameCount].KnowledgeBaseId, NOPARENT, NOGRANDPARENT))
                        {
                            rdo.Attributes.Add("disabled", "disabled");
                        }
                        chkbxKB.Controls.Add(rdo);
                        KBName.Controls.Add(chkbxKB);
                        indentedTable.Controls.Add(KBName);
                    }
                    subHeaderCell.Controls.Add(indentedTable);
                    subHeaders.Controls.Add(subHeaderCell);
                    KBTable.Controls.Add(subHeaders);
                }

                pnlHeaderList.Controls.Add(KBTable);
            }
        }

        protected void CheckForEditingCriteria(int kbId, int templateId)
        {
            try
            {
                _knowledgeBase = new KnowledgeBaseBO();
                if (_knowledgeBase.CheckForEditingCriteria(kbId, templateId))
                {
                    if (had == (int)enKBType.Question)
                    {
                        EditTemplateMessage.Warning("This item is share in other template, you can only Edit Critical designation, Answers and Add-ons.");
                        formKBDocs.Attributes.Add("class", "is-disabled");
                        formKBDocs.Style.Add("color", "Gray");
                        Critical.Style.Add("color", "Black");
                        CriticalToolTip.Style.Add("color", "Black");
                        right.Style.Add("color", "Black");
                        txtDisplayName.Attributes.Add("disabled", "disabled");
                        txtInstruction.Attributes.Add("readonly", "readonly");
                        txtTabLine1.Attributes.Add("disabled", "disabled");
                        txtTabLine2.Attributes.Add("disabled", "disabled");
                        rdoMustPassNo.Attributes.Add("disabled", "disabled");
                        rdoMustPassYes.Attributes.Add("disabled", "disabled");
                        txtInstruction.Attributes.Add("onKeyDown", "javascript:return false");
                        txtInstruction.Attributes.Add("onKeyPress", "javascript:return false");
                    }
                   else if (had == (int)enKBType.SubHeader)
                    {
                        EditTemplateMessage.Warning("This item is share in other template, you can only Edit Info Document to Enable/Disable.");
                        formKBDocs.Attributes.Add("class", "is-disabled");
                        formKBDocs.Style.Add("color", "Gray");
                        infoDocument.Style.Add("color", "Black");
                        txtDisplayName.Attributes.Add("disabled", "disabled");
                        txtInstruction.Attributes.Add("readonly", "readonly");
                        rdoMustPassNo.Attributes.Add("disabled", "disabled");
                        rdoMustPassYes.Attributes.Add("disabled", "disabled");
                        pageReference.Style.Add("color", "Black");
                        txtPageReference.Attributes.Remove("disabled");
                        txtInstruction.Attributes.Add("onKeyDown", "javascript:return false");
                        txtInstruction.Attributes.Add("onKeyPress", "javascript:return false");
                    }
                    else
                    {
                        EditTemplateMessage.Warning("This item is share in other template and you can not Edit it.");
                        formKBDocs.Attributes.Add("class", "is-disabled");
                        formKBDocs.Style.Add("color", "Gray");
                        CurentTemplateForm.Attributes.Add("class", "is-disabled");
                        CurentTemplateForm.Style.Add("color", "Gray");
                        btnSaveEditTemplate.Attributes.Add("disabled", "disabled");
                        btnSaveEditTemplate.OnClientClick = "javascript:return false";
                        txtDisplayName.Attributes.Add("disabled", "disabled");
                        txtInstruction.Attributes.Add("readonly", "readonly");
                        txtTabLine1.Attributes.Add("disabled", "disabled");
                        txtTabLine2.Attributes.Add("disabled", "disabled");
                        rdoMustPassNo.Attributes.Add("disabled", "disabled");
                        rdoMustPassYes.Attributes.Add("disabled", "disabled");
                        txtInstruction.Attributes.Add("onKeyDown", "javascript:return false");
                        txtInstruction.Attributes.Add("onKeyPress", "javascript:return false");
                    }
                }
                else
                {
                    EditTemplateMessage.Clear("");
                    formKBDocs.Attributes.Remove("class");
                    formKBDocs.Style.Remove("color");
                    CurentTemplateForm.Attributes.Remove("class");
                    CurentTemplateForm.Style.Remove("color");
                    btnSaveEditTemplate.Attributes.Remove("disabled");
                    txtDisplayName.Attributes.Remove("disabled");
                    txtInstruction.Attributes.Remove("readonly");
                    txtTabLine1.Attributes.Remove("disabled");
                    txtTabLine2.Attributes.Remove("disabled");
                    rdoCriticalNo.Attributes.Remove("disabled");
                    rdoCriticalYes.Attributes.Remove("disabled");
                    rdoMustPassNo.Attributes.Remove("disabled");
                    rdoMustPassYes.Attributes.Remove("disabled");
                    AnsYesNoNA.Attributes.Remove("disabled");
                    AnsYesNo.Attributes.Remove("disabled");
                    //AnsNone.Attributes.Remove("disabled");
                    txtInstruction.Attributes.Remove("onKeyDown");
                    txtInstruction.Attributes.Remove("onKeyPress");
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion
    }
}