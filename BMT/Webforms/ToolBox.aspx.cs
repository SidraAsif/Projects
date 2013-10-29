#region Modification History

//  ******************************************************************************
//  Module        : AddressDetails
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01-17-2012          Super User & Organize the complete code
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

namespace BMT.Webforms
{
    public partial class ToolBox : System.Web.UI.Page
    {
        #region VARIABLES
        private PracticeBO Practice;
        private IQueryable _practiceList;
        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserApplicationId"] != null || Session["PracticeId"] != null || Session["UserType"] != null || Session["EnterpriseId"] != null)
                {
                   /* userId = Convert.ToInt32(Session["UserApplicationId"]);*/
                   /* practiceId = Convert.ToInt32(Session["PracticeId"]);*/
                   /* userType = Session["UserType"].ToString(); */                   
                    /*enterpriseId = Convert.ToInt32(Session["EnterpriseId"]);*/
                }
                else
                {
                    SessionHandling sessionHandling = new SessionHandling();
                    sessionHandling.ClearSession();
                    Response.Redirect("~/Account/Login.aspx");
                }

                #region PAGE_LOAD
                if (!Page.IsPostBack)
                {
                    if (Session["UserType"].ToString() == enUserRole.SuperUser.ToString() || Session["UserType"].ToString() == enUserRole.Consultant.ToString() || Session["UserType"].ToString() == enUserRole.SuperAdmin.ToString())
                    {
                        GetPractices();
                    }
                }

                #endregion

                Session["QueryString"] = Request.QueryString["ContentType"] != null ? Request.QueryString["ContentType"] : string.Empty;
                Session["TableName"] = enDbTables.ToolDocument.ToString();
                Session["SectionId"] = hdnSectionID.Value;

                //pass practice Id to Tree control
                TreeControl.PracticeId = Convert.ToInt32(Session["PracticeId"]);              
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
                    /*practiceId = Convert.ToInt32(Session["PracticeId"]);*/

                    if (Session["UserType"].ToString() == enUserRole.SuperAdmin.ToString())
                    {
                        Session["EnterpriseId"] = ddlEnterprise.SelectedValue;
                        /*enterpriseId = Convert.ToInt32(Session["EnterpriseId"]);*/
                    }

                    Response.Redirect("~/Webforms/ToolBox.aspx", false);
                    //Response.Redirect(Request.RawUrl); 
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
                    if (Convert.ToInt32(ddlEnterprise.SelectedValue) > 0)
                    {
                        ddlPractices.Enabled = true;
                        GetPracticeByEnterpriseId(Convert.ToInt32(ddlEnterprise.SelectedValue));

                        Session["PracticeId"] = ddlPractices.SelectedValue;
                        /*practiceId = Convert.ToInt32(Session["PracticeId"]);*/

                        Session["EnterpriseName"] = ddlEnterprise.SelectedItem.Text.ToString();

                        Session["EnterpriseId"] = ddlEnterprise.SelectedValue;
                        /*enterpriseId = Convert.ToInt32(Session["EnterpriseId"]);*/
                    }
                    else
                    {
                        Session["PracticeId"] = 0;
                    }                   

                    Response.Redirect("~/Webforms/ToolBox.aspx", false);
                    //Response.Redirect(Request.RawUrl); 
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void UpdatePanelControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    if (Request.Params.Get("__EVENTARGUMENT") != "")
                    {
                        string temp = Request.Params.Get("__EVENTARGUMENT");
                        string[] arg = temp.Split('/');
                        string ContentType = arg[0];
                        int toolSectionId = Convert.ToInt32(arg[1]);
                        lblContentTypeName.Text = GetTReePath(toolSectionId);


                        if (ContentType == "UploadedTools")
                        {
                            btnUploadDocuments.Visible = true;
                            ToolList.ToolSectionId = toolSectionId;
                            ToolList.PracticeId = Convert.ToInt32(Session["PracticeId"]);
                        }
                        else
                        {
                            btnUploadDocuments.Visible = false;
                            ToolList.ToolSectionId = toolSectionId;
                            ToolList.PracticeId = 0;
                        }

                        // Enable upload button in super user case
                        if (ContentType != "Null" && ContentType != "UploadedTools")
                        {
                            if (Session["UserType"].ToString() == enUserRole.SuperUser.ToString() || Session["UserType"].ToString() == enUserRole.SuperAdmin.ToString())
                            {
                                btnUploadDocuments.Visible = true;
                            }
                            else
                            {
                                btnUploadDocuments.Visible = false;
                            }
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        #endregion

        #region FUNCTIONS
        private void GetPractices()
        {
            try
            {
                if (Session["UserType"].ToString() == enUserRole.SuperUser.ToString() || Session["UserType"].ToString() == enUserRole.Consultant.ToString() || Session["UserType"].ToString() == enUserRole.SuperAdmin.ToString())
                {
                    if (Session["UserType"].ToString() == enUserRole.SuperAdmin.ToString())
                    {
                        lblEnterprise.Visible = ddlEnterprise.Visible = lblPractices.Visible = ddlPractices.Visible = true;

                        Practice = new PracticeBO();
                        IQueryable _enterpriseList;
                        _enterpriseList = Practice.GetEnterprises();
                        ddlEnterprise.DataTextField = "Name";
                        ddlEnterprise.DataValueField = "ID";
                        ddlEnterprise.DataSource = _enterpriseList;
                        ddlEnterprise.DataBind();

                        //Add Default item in comboBox
                        ddlEnterprise.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.Enabled = false;

                        if (Convert.ToInt32(Session["EnterpriseId"]) > 0)
                        {
                            ddlEnterprise.SelectedIndex = ddlEnterprise.Items.IndexOf(ddlEnterprise.Items.FindByValue(Convert.ToInt32(Session["EnterpriseId"]).ToString()));
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

                        Practice = new PracticeBO();
                        _practiceList = Practice.GetPractices(Convert.ToInt32(Session["UserApplicationId"]), Session["UserType"].ToString(), Convert.ToInt32(Session["EnterpriseId"]));

                        ddlPractices.DataTextField = "Name";
                        ddlPractices.DataValueField = "ID";
                        ddlPractices.DataSource = _practiceList;
                        ddlPractices.DataBind();

                        ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue((Session["PracticeId"]).ToString()));
                    }


                }
                else { ddlPractices.Items.Clear(); lblPractices.Visible = ddlPractices.Visible = false; }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        private string GetTReePath(int toolSectionId)
        {
            //List<string> nodeName = new List<string>();
            //int id;
            TreeBO tree = new TreeBO();
            string name = tree.GetName(enTreeType.ToolSection.ToString(), toolSectionId);
            //nodeName.Add(name);
            //string temp = "";
            //id = tree.GetParentId(enTreeType.ToolSection.ToString(), toolSectionId);
            //while (id != 0)
            //{
            //    string abcd = tree.GetParentName(enTreeType.ToolSection.ToString(), id);
            //    if (abcd != null)
            //    {
            //        nodeName.Add(abcd);
            //    }

            //    else
            //    {
            //        string temporary = tree.GetName(enTreeType.ToolSection.ToString(), toolSectionId);
            //        temp += temporary + "/" + temp;
            //    }

            //    id = tree.GetParentId(enTreeType.ToolSection.ToString(), id);
            //    toolSectionId = id;

            //}

            //for (int index = nodeName.Count - 1; index >= 0; index--)
            //{
            //    temp += nodeName[index] + "/";
            //}

            //temp = temp.TrimEnd('/');

            return name;
        }


        private void GetPracticeByEnterpriseId(int enterpriseId)
        {
            Practice = new PracticeBO();
            _practiceList = Practice.GetPracticesByEnterpriseId(enterpriseId);

            ddlPractices.DataTextField = "Name";
            ddlPractices.DataValueField = "ID";
            ddlPractices.DataSource = _practiceList;
            ddlPractices.DataBind();

            ddlPractices.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPractices.SelectedIndex = ddlPractices.Items.IndexOf(ddlPractices.Items.FindByValue(Session["PracticeId"].ToString()));
        }
        #endregion

    }
}
