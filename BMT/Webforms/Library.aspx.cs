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
//  Mirza Fahad Ali Baig   (May-01-2012)        Remove Practice Combo in Super User because not needed in library page
//  Mirza Fahad Ali Baig   (May-01-2012)        SET variable naming convention in GetPath method like abcd etc...
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
using System.Text;

namespace BMT.Webforms
{
    public partial class Library : System.Web.UI.Page
    {
        #region CONSTANTS
        private const int DEFAULT_PRACITCE_ID = 1;

        #endregion

        #region VARIABLES
        private SessionHandling sessionHandling;
        private TreeBO _treeBO = new TreeBO();

        private int userId;
        private string userType;        

        #endregion

        #region FUNCTIONS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserApplicationId"] != null || Session["UserType"] != null)
                {
                    userId = Convert.ToInt32(Session["UserApplicationId"]);
                    userType = Session["UserType"].ToString();                    
                }
                else
                {
                    sessionHandling = new SessionHandling();
                    sessionHandling.ClearSession();
                    Response.Redirect("~/Account/Login.aspx");
                }
                #region PAGE_LOAD

                #endregion

                Session["QueryString"] = Request.QueryString["ContentType"] != null ? Request.QueryString["ContentType"] : string.Empty;
                Session["TableName"] = enDbTables.LibraryDocument.ToString();
                Session["SectionId"] = hdnLibrarySectionID.Value;

                //pass practice Id to Tree control
                TreeControl.PracticeId = DEFAULT_PRACITCE_ID;                
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
                        int LibrarySectionId = Convert.ToInt32(arg[1]);
                        lblContentTypeName.Text = GetPath(LibrarySectionId);

                        if (ContentType == "UploadedDocuments")
                        {
                            btnUploadDocuments.Visible = true;
                            LibraryList.LibrarySectionId = LibrarySectionId;
                            LibraryList.PracticeId = DEFAULT_PRACITCE_ID;
                        }
                        else if (ContentType != "Null")
                        {
                            if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                                btnUploadDocuments.Visible = true;
                            else
                                btnUploadDocuments.Visible = false;

                            LibraryList.LibrarySectionId = LibrarySectionId;
                            LibraryList.PracticeId = 0;
                        }
                        else
                            btnUploadDocuments.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        private string GetPath(int librarySectionId)
        {
            List<string> _listOfFolderName = new List<string>();
            string serverDirPath = string.Empty;
            int librarySectionid = 0;
            string selectedFolderName = _treeBO.GetName(enTreeType.LibrarySection.ToString(), librarySectionId);

            _listOfFolderName.Add(selectedFolderName);

            librarySectionid = _treeBO.GetParentId(enTreeType.LibrarySection.ToString(), librarySectionId);

            while (librarySectionid != 0)
            {
                string parentFolderName = _treeBO.GetParentName(enTreeType.LibrarySection.ToString(), librarySectionid);

                if (parentFolderName != null)
                    _listOfFolderName.Add(parentFolderName);
                else
                {
                    string temporary = _treeBO.GetName(enTreeType.LibrarySection.ToString(), librarySectionId);
                    serverDirPath += temporary + "/" + serverDirPath;
                }

                librarySectionid = _treeBO.GetParentId(enTreeType.LibrarySection.ToString(), librarySectionid);
                librarySectionId = librarySectionid;
            }

            for (int index = _listOfFolderName.Count - 1; index >= 0; index--)
            {
                serverDirPath += _listOfFolderName[index] + "/";
            }

            serverDirPath = serverDirPath.TrimEnd('/');

            return serverDirPath;
        }

        #endregion
    }
}