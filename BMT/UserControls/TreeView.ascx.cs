#region Modification History

//  ******************************************************************************
//  Module        : Tree Control
//  Created By    : 
//  When Created  : 
//  Description   : To display the tree in Project, Library and Toolbox screen
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
// Mirza Fahad Ali Baig     05-01-2012      Remove Errors and add null handling for sevel variables etc...
// Mirza Fahad Ali Baig     05-01-2012      Fix value comparion to store the user type
// Mirza Fahad Ali Baig     05-07-2012      Fix Incorrect Path
// Mirza Fahad Ali Baig     05-07-2012      Fix Tree Collapse Issue
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using BMTBLL;
using BMTBLL.Enumeration;
using BMTBLL.Classes;
using System.Web.UI.MobileControls;
using System.Text;

using BMT.WEB;

namespace BMT.UserControls
{
    public partial class ParentChildTreeView : System.Web.UI.UserControl
    {

        #region CONSTANTS
        private const string PROJECT_TOP_LEVEL_NAME = "PCMH Recognition";
        private const string DEFAULT_JUMP_START_MATERIALS = "JumpStartMaterials";
        private const string TEMPLATE_UTILITY_FOLDER = "Utility Folder";
        private const string TEMPLATE_UPLOADED_DOCUMENT_FOLDER = "Uploaded Documents";
        #endregion

        #region VARIABLES
        private TreeBO tree;

        private TreeNode projtreeRoot;
        private TreeNode child;
        private TreeNode siteChild;
        private TreeNode treeRoot;

        private string parentValuePath;
        private List<TreeDetail> commonSection;
        private List<TreeDetail> fixedSiteContents;
        private List<TreeDetail> restrictedFolders;
        private string topLevelNode;
        private string topContentType;

        private bool populateSiteTree;
        private bool isProjectTree;
        private bool treeFirstNode;
        private int projSectionParentId;
        private int noOfParentNode;
        private int noOfSite;
        private int topNodeSectionId;
        private int? topNodeParentSectionId;

        private string parentNodePath;
        private string parentNodename;
        private string siteName;
        private string roleType;
        private List<string> _practiceSiteList;
        private IQueryable<string> _IPractices;


        #endregion

        #region PROPERTIES
        public enTreeType TableName { get; set; }

        public int UserID { get; set; }
        public int PracticeId { get; set; }

        public bool IsToolTreeExpanded { get; set; }
        public bool ProjectSelection { get; set; }
        public bool LibrarySelection { get; set; }
        public bool ToolSelection { get; set; }
        public bool ReportSelection { get; set; }

        public bool IsSRA { get; set; }
        public string SiteName { get; set; }
        public bool IsFromTree { get; set; }

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region SESSION_HANDLING
                if (Session[enSessionKey.UserType.ToString()] != null)
                    roleType = Session[enSessionKey.UserType.ToString()].ToString();
                else
                    return;

                if (roleType == enUserRole.SuperUser.ToString() || roleType == enUserRole.SuperAdmin.ToString())
                    DisplayContextMenu();

                #endregion

                if (!Page.IsPostBack && PracticeId > 0)
                {
                    string recievedNodeType = Request.QueryString["NodeType"] != null ? Request.QueryString["NodeType"] : string.Empty;

                    tree = new TreeBO();
                    siteChild = new TreeNode();

                    _practiceSiteList = tree.GetSiteName(PracticeId);
                    fixedSiteContents = tree.GetSiteFixedContents();

                    CommonLoad();

                    if (recievedNodeType != null && recievedNodeType.Trim() != string.Empty)
                        DynamicNodeExpansion(Convert.ToInt32(recievedNodeType));


                }
                else if (Page.IsPostBack && PracticeId > 0)
                {
                    string treeActivePath = hdnTreeNodePath.Value;
                    if (treeView != null)
                    {
                        if (treeActivePath.Trim() != string.Empty)
                        {
                            treeView.FindNode(treeActivePath).Expanded = true;
                            treeView.FindNode(treeActivePath).Parent.Expand();
                        }
                    }
                }
                else
                    return;

            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }
        }

        #endregion

        #region FUNCTIONS

        public void CommonLoad()
        {
            try
            {
                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                int practiceId = Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]);
                int medicalGroupId = tree.GetMedicalGroupIdByPracticeId(practiceId);
                // store TableName
                hdnScreen.Value = TableName.ToString();
                hdnEnterpriseName.Value = Session["EnterpriseName"] == null ? ConfigurationManager.AppSettings["EnterpriseName"].ToString() : Session["EnterpriseName"].ToString();
                hdnEnterpriseId.Value = enterpriseId.ToString();
                hdnRestrictedFolderList.Value = string.Empty;


                if (TableName == enTreeType.ProjectSection)
                {
                    tree.PracticeId = this.PracticeId;
                    commonSection = tree.GetTreeNodesByProjectAssignment(TableName, enterpriseId, medicalGroupId, practiceId);
                    //restrictedFolders = null;
                    treeFirstNode = true;
                    isProjectTree = ProjectSelection = true;

                }
                if (TableName == enTreeType.LibrarySection)
                {
                    commonSection = tree.GetTreeNodesByEnterpriseId(TableName, enterpriseId);
                    restrictedFolders = tree.GetRestrictedFolders(TableName, enterpriseId);
                    LibrarySelection = true;
                    if (restrictedFolders.Count > 0)
                    {
                        ShowRestrictedFolder(treeView);
                        GetRestrictedFoldersNameCSV();
                    }
                }
                if (TableName == enTreeType.ToolSection)
                {
                    commonSection = tree.GetTreeNodesByEnterpriseId(TableName, enterpriseId);
                    restrictedFolders = tree.GetRestrictedFolders(TableName, enterpriseId);
                    IsToolTreeExpanded = false;
                    ToolSelection = true;
                    {
                        ShowRestrictedFolder(treeView);
                        GetRestrictedFoldersNameCSV();
                    }
                }
                if (TableName == enTreeType.ReportSection)
                {
                    commonSection = tree.GetTreeNodesByEnterpriseId(TableName, enterpriseId);
                    restrictedFolders = tree.GetRestrictedFolders(TableName, enterpriseId);
                    ReportSelection = true;
                    {
                        ShowRestrictedFolder(treeView);
                        GetRestrictedFoldersNameCSV();
                    }
                }
                PopulateTree(treeView, commonSection);

                //if (restrictedFolders.Count > 0)
                //{
                //    ShowRestrictedFolder(treeView);
                //    GetRestrictedFoldersNameCSV();
                //}

                if (isProjectTree)
                {
                    string path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : "";
                    if (path != string.Empty)
                    {
                        int noOfNodes = path.Split('/').Length;
                        TreeNode currentTreeNode = new TreeNode();
                        currentTreeNode = treeView.FindNode(path);

                        while (noOfNodes > 0)
                        {
                            if (currentTreeNode != null)
                            {
                                currentTreeNode.Expanded = true;
                                currentTreeNode = currentTreeNode.Parent;
                            }
                            noOfNodes--;
                        }
                    }
                }

                if ((roleType == enUserRole.SuperUser.ToString() || roleType == enUserRole.SuperAdmin.ToString()) && TableName != enTreeType.ProjectSection && TableName != enTreeType.ReportSection)
                    divRootFolder.Visible = true;
                else
                    divRootFolder.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }
        }

        //This Method remains given node expanded after adding new node

        private void DynamicNodeExpansion(int id)
        {
            try
            {
                string result = tree.GetTopMostNode(id, "LibrarySection");
                string[] resultSplit = result.Split(',');

                foreach (TreeNode treenode in treeView.Nodes)
                {
                    for (int index = 0; index < resultSplit.Length; index++)
                    {
                        if (resultSplit[index] == treenode.Value)
                            treenode.Expand();
                    }
                    NodeExpansionRecursively(id, treenode);
                }
            }

            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }
        }


        //This method used to help the DynamicNodeExpansion method to go in the sub part of the hierarchy

        private void NodeExpansionRecursively(int id, TreeNode value)
        {
            try
            {
                string result = tree.GetTopMostNode(id, "LibrarySection");
                string[] resultSplit = result.Split(',');

                foreach (TreeNode node in value.ChildNodes)
                {
                    for (int index = 0; index < resultSplit.Length; index++)
                    {
                        if (resultSplit[index] == node.Value)
                        {
                            node.Expand();
                        }
                    }
                    NodeExpansionRecursively(id, node);
                }
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }
        }


        //*****************************
        //This is a Generic Method for Tree Population whether it is Project, Tool, and Library        
        //*****************************

        private void PopulateTree(TreeView objTreeView, List<TreeDetail> commonTreeSection)
        {
            try
            {
                if (commonTreeSection != null && PracticeId != 0)
                {
                    foreach (TreeDetail dataRow in commonTreeSection)
                    {
                        if (Convert.ToInt32(dataRow.ParentSectionId) == projSectionParentId)
                        {
                            treeRoot = new TreeNode();
                            treeRoot.Text = dataRow.Name.ToString();
                            treeRoot.Value = dataRow.Name.ToString();
                            treeRoot.Selected = true;

                            if (ProjectSelection)
                            {
                                treeRoot.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                                objTreeView.Nodes.Add(treeRoot);
                                if (Request.QueryString["Path"] == null)
                                {
                                    if (treeFirstNode)
                                    {
                                        treeRoot.Expanded = true;
                                        treeFirstNode = false;
                                    }
                                }
                                if (ConfigurationManager.AppSettings["EnterpriseName"] == treeRoot.Text)
                                {
                                    hdnRestrictedFolderList.Value += dataRow.Name + ",";
                                    AppendChildNodeName(dataRow.SectionId);
                                }
                                parentNodePath = treeRoot.Text;
                            }
                            else
                            {
                                if (ReportSelection)
                                {

                                    treeRoot.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                                    treeRoot.Expanded = true;
                                    objTreeView.Nodes.Add(treeRoot);
                                }
                                if (LibrarySelection || ToolSelection)
                                {
                                    treeRoot.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                                    if (!IsToolTreeExpanded)
                                        treeRoot.Expanded = false;
                                    objTreeView.Nodes.Add(treeRoot);
                                }

                                if (IsToolTreeExpanded)
                                    objTreeView.ExpandAll();

                            }

                            if (dataRow.IsTopNode == false)
                            {
                                foreach (TreeNode childNode in GetChildNodes(Convert.ToInt64(dataRow.SectionId), commonTreeSection))
                                {
                                    if (childNode.Text == "Tools")
                                    {

                                        if (IsSiteLevelTool(Convert.ToInt32(dataRow.SectionId)))
                                        {
                                            TreeNode toolchildNode = new TreeNode();
                                            bool hasDocumentStore = false;
                                            bool hasStandardFolder = false;
                                            int projSectionIdOfToolHasStandardFolder = 0;
                                            int projSectionIdOfToolHasDocs = 0;

                                            foreach (string practiceSite in _practiceSiteList)
                                            {
                                                toolchildNode = new TreeNode();
                                                toolchildNode.Text = practiceSite;
                                                toolchildNode.NavigateUrl = "javascript:return false";
                                                parentNodePath = treeRoot.Text + "/" + practiceSite;
                                                childNode.ChildNodes.Add(toolchildNode);
                                                if (SiteName == practiceSite)
                                                    toolchildNode.ExpandAll();

                                                TreeNode toolSubChildNode = new TreeNode();
                                                toolSubChildNode.Value = childNode.Value;
                                                toolSubChildNode.Text = childNode.Text;
                                                toolSubChildNode.NavigateUrl = "javascript:return false." + childNode.Value;
                                                toolchildNode.ChildNodes.Add(toolSubChildNode);

                                                foreach (TreeNode cNode in GetChildNodes(Convert.ToInt64(childNode.Value), commonTreeSection))
                                                {
                                                    toolSubChildNode.ChildNodes.Add(cNode);

                                                    if (ToolHasDocumemtStore(Convert.ToInt32(cNode.Value)))
                                                    {
                                                        hasDocumentStore = true;
                                                        projSectionIdOfToolHasDocs = Convert.ToInt32(cNode.Value);
                                                    }
                                                    if (ToolHasStandardFolder(Convert.ToInt32(cNode.Value)))
                                                    {
                                                        hasStandardFolder = true;
                                                        projSectionIdOfToolHasStandardFolder = Convert.ToInt32(cNode.Value);
                                                    }
                                                }
                                                if (hasDocumentStore)
                                                {
                                                    TreeNode toolAnotherSubChildNode = new TreeNode();
                                                    toolAnotherSubChildNode.Value = childNode.Value;
                                                    toolAnotherSubChildNode.Text = GetNameOfDocumentStore(projSectionIdOfToolHasDocs);
                                                    string sectionType = "DocumentStore";
                                                    int PID = tree.GetProjectID(dataRow.SectionId);
                                                    int projUsageID = tree.GetProjectUsageID(PracticeId, PID);
                                                    int SID = tree.GetSiteID(PracticeId, practiceSite);
                                                    toolAnotherSubChildNode.NavigateUrl = "javascript:onnodeclick('" + sectionType + "','" + dataRow.SectionId.ToString() + "','" +
                                                        projUsageID.ToString() + "','" + SID.ToString() + "','" +
                                                        System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                                                    toolchildNode.ChildNodes.Add(toolAnotherSubChildNode);
                                                }


                                                treeRoot.ChildNodes.Add(toolchildNode);
                                            }
                                            if (hasStandardFolder)
                                            {
                                                TreeNode toolAnotherSubChildNode = new TreeNode();
                                                toolAnotherSubChildNode.Value = childNode.Value;
                                                toolAnotherSubChildNode.Text = GetNameOfStandardFolder(projSectionIdOfToolHasStandardFolder);
                                                string sectionType = "StandardFolder";
                                                int PID = tree.GetProjectID(dataRow.SectionId);
                                                int SID = 0;
                                                int projUsageID = tree.GetProjectUsageID(PracticeId, PID);
                                                toolAnotherSubChildNode.NavigateUrl = "javascript:onnodeclick('" + sectionType + "','" + childNode.Value.ToString() + "','" +
                                                    projUsageID.ToString() + "','" + SID.ToString() + "','" +
                                                    System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                                                if (toolAnotherSubChildNode.Text != "")
                                                    treeRoot.ChildNodes.AddAt(0, toolAnotherSubChildNode);
                                            }
                                        }
                                        else
                                        {
                                            treeRoot.ChildNodes.Add(childNode);

                                            bool hasDocumentStore = false;
                                            bool hasStandardFolder = false;
                                            int projSectionIdOfToolHasStandardFolder = 0;
                                            int projSectionIdOfToolHasDocs = 0;
                                            foreach (TreeNode cNode in GetChildNodes(Convert.ToInt64(childNode.Value), commonTreeSection))
                                            {

                                                if (ToolHasDocumemtStore(Convert.ToInt32(cNode.Value)))
                                                {
                                                    hasDocumentStore = true;
                                                    projSectionIdOfToolHasDocs = Convert.ToInt32(cNode.Value);
                                                }
                                                if (ToolHasStandardFolder(Convert.ToInt32(cNode.Value)))
                                                {
                                                    hasStandardFolder = true;
                                                    projSectionIdOfToolHasStandardFolder = Convert.ToInt32(cNode.Value);
                                                }
                                            }
                                            if (hasDocumentStore)
                                            {
                                                TreeNode toolAnotherSubChildNode = new TreeNode();
                                                toolAnotherSubChildNode.Value = childNode.Value;
                                                toolAnotherSubChildNode.Text = GetNameOfDocumentStore(projSectionIdOfToolHasDocs);
                                                string sectionType = "DocumentStore";
                                                int PID = tree.GetProjectID(dataRow.SectionId);
                                                int projUsageID = tree.GetProjectUsageID(PracticeId, PID);
                                                int SID = tree.GetSiteIDByPracticeId(PracticeId);
                                                toolAnotherSubChildNode.NavigateUrl = "javascript:onnodeclick('" + sectionType + "','" + dataRow.SectionId.ToString() + "','" +
                                                    projUsageID.ToString() + "','" + SID.ToString() + "','" +
                                                    System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                                               childNode.ChildNodes.Add(toolAnotherSubChildNode);
                                            }
                                            if (hasStandardFolder)
                                            {
                                                TreeNode toolAnotherSubChildNode = new TreeNode();
                                                toolAnotherSubChildNode.Value = childNode.Value;
                                                toolAnotherSubChildNode.Text = GetNameOfStandardFolder(projSectionIdOfToolHasStandardFolder);
                                                string sectionType = "StandardFolder";
                                                int PID = tree.GetProjectID(dataRow.SectionId);
                                                int SID = 0;
                                                int projUsageID = tree.GetProjectUsageID(PracticeId, PID);
                                                toolAnotherSubChildNode.NavigateUrl = "javascript:onnodeclick('" + sectionType + "','" + childNode.Value.ToString() + "','" +
                                                    projUsageID.ToString() + "','" + SID.ToString() + "','" +
                                                    System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                                                if (toolAnotherSubChildNode.Text != "")
                                                    treeRoot.ChildNodes.AddAt(0, toolAnotherSubChildNode);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        treeRoot.ChildNodes.Add(childNode);
                                    }
                                }
                            }

                            // Reading Query String
                            if (ReportSelection)
                            {
                                if (Session["ReturnContentType"] != null)
                                {
                                    int FindChild = 0;
                                    List<TreeDetail> Node = tree.GetNodes(Session["ReturnContentType"].ToString(), TableName);

                                    foreach (TreeNode nodes in objTreeView.Nodes)
                                    {
                                        if (Session["ReturnContentType"].ToString() == "General Status Report")
                                        {
                                            nodes.Selected = true;
                                            break;
                                        }

                                        else
                                        {
                                            nodes.Selected = true;
                                            GetChild(nodes, FindChild);
                                            nodes.ExpandAll();
                                        }
                                    }
                                }
                            }

                            if (LibrarySelection || ToolSelection)
                            {
                                if (Session["ReturnContentType"] != null)
                                {
                                    int FindChild = 0, IsChild = 0;
                                    List<TreeDetail> Node = tree.GetNodes(Session["ReturnContentType"].ToString(), TableName);

                                    foreach (TreeDetail treeNode in Node)
                                    {
                                        // To find the childs of the node
                                        FindChild = treeNode.SectionId;
                                        // To find if it is a chid or not
                                        IsChild = Convert.ToInt32(treeNode.ParentSectionId.ToString());
                                        break;
                                    }

                                    foreach (TreeNode nodes in objTreeView.Nodes)
                                    {
                                        if (IsChild == 0 || nodes.ChildNodes.Count == 0)
                                        {
                                            nodes.Selected = true;
                                            break;
                                        }

                                        if (nodes.ChildNodes.Count > 0)
                                        {
                                            GetChild(nodes, FindChild);
                                            nodes.ExpandAll();
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);

            }
        }

        private void ShowRestrictedFolder(TreeView topTreeView)
        {
            try
            {
                foreach (TreeDetail dataRow in restrictedFolders)
                {
                    if ((dataRow.IsTopNode && TableName == enTreeType.ProjectSection) || (dataRow.ParentSectionId == null))
                    {
                        TreeNode topNode = new TreeNode();
                        topNode.Text = dataRow.Name;
                        topNode.Value = dataRow.SectionId.ToString();
                        if (TableName == enTreeType.ProjectSection)
                        {
                            if (IsOneLevelFolder(dataRow.SectionId, enTreeType.ProjectSection))
                            {
                                topNode.NavigateUrl = "javascript:onclicknode('" + dataRow.ContentType + "','" +
                                        dataRow.SectionId.ToString() + "');";
                            }
                            else
                            {
                                topNode.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                            }
                        }
                        else if (TableName == enTreeType.ToolSection)
                        {
                            if (IsOneLevelFolder(dataRow.SectionId, enTreeType.ToolSection))
                            {
                                topNode.NavigateUrl = "javascript:onclicknode('" + dataRow.ContentType + "','" +
                                        dataRow.SectionId.ToString() + "');";
                            }
                            else
                            {
                                topNode.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                            }
                        }
                        else
                        {
                            topNode.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                        }
                        topTreeView.Nodes.Add(topNode);

                        foreach (TreeNode childNode in GetRestrictedFolderChildNodes(dataRow.SectionId, restrictedFolders, topNode.ValuePath))
                        {
                            topNode.ChildNodes.Add(childNode);
                        }

                        if (TableName == enTreeType.ToolSection)
                        {
                            if (IsToolTreeExpanded)
                                topTreeView.ExpandAll();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }
        }

        //*****************************
        //This is a Generic Method for Tree of Project, Tool, and Library
        //to get the child from different Databases
        //*****************************

        private TreeNodeCollection GetChildNodes(long parentId, List<TreeDetail> commonTreeSection)
        {

            TreeNodeCollection childtreenodes = new TreeNodeCollection();

            List<TreeDetail> childSection = null;
            if (TableName == enTreeType.ToolSection)
            {
                childSection = (from childRecords in commonTreeSection
                                where childRecords.ParentSectionId == parentId
                                orderby childRecords.Name
                                select childRecords).ToList();
            }
            else
            {
                childSection = (from childRecords in commonTreeSection
                                where childRecords.ParentSectionId == parentId
                                select childRecords).ToList();
            }
            try
            {

                if (childSection.Count > 0)
                {
                    if (ProjectSelection)
                        parentValuePath = parentNodePath + "/" + parentId.ToString();

                    foreach (TreeDetail dataRow in childSection)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = dataRow.Name.ToString();
                        childNode.Value = dataRow.SectionId.ToString();

                        if (projSectionParentId == 0)
                        {
                            if (LibrarySelection)
                            {
                                string contenttype = dataRow.ContentType.ToString();
                                parentValuePath = parentId.ToString() + "/" + childNode.Value.ToString();
                                childNode.NavigateUrl = "javascript:onclicknode('" + contenttype + "','" +
                                    dataRow.SectionId.ToString() + "');";
                            }

                            if (ToolSelection)
                            {
                                string contenttype = dataRow.ContentType.ToString();
                                int allchildsCount = HasChildNodes(Convert.ToInt64(dataRow.SectionId), commonTreeSection);
                                if (allchildsCount > 0)
                                {
                                    childNode.NavigateUrl = "javascript:return false," + dataRow.SectionId + ";";
                                }
                                else
                                    childNode.NavigateUrl = "javascript:onclicknode('" + contenttype + "','" +
                                    dataRow.SectionId.ToString() + "');";
                                if (IsToolTreeExpanded)
                                    childNode.ExpandAll();
                            }
                            if (ReportSelection)
                            {
                                string contentType = dataRow.ContentType.ToString();
                                if (contentType == "Custom Reports")
                                    childNode.NavigateUrl = "javascript:return false;";
                                else
                                    childNode.NavigateUrl = "javascript:onclicknode('" + contentType + "','" +
                                        dataRow.SectionId.ToString() + "');";
                            }
                        }

                        if (ProjectSelection)
                        {
                            if (childNode.ChildNodes.Count == 0)
                            {
                                string sectionType = tree.GetSectionTypeBySectionId(dataRow.SectionId);
                                int PID = tree.GetProjectID(dataRow.SectionId);
                                int projUsageID = tree.GetProjectUsageID(PracticeId, PID);
                                string path = System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath);
                                string[] paths = path.Split('/');
                                if (paths.Length > 2)
                                {
                                    siteName = paths[1];
                                }
                                int SID = tree.GetSiteID(PracticeId, siteName);
                                childNode.NavigateUrl = "javascript:onnodeclick('" + sectionType + "','" + dataRow.SectionId.ToString() + "','" +
                                    projUsageID.ToString() + "','" + SID.ToString() + "','" +
                                    System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                            }
                            else
                                parentValuePath = childNode.ValuePath;
                        }

                        foreach (TreeNode cnode in GetChildNodes(Convert.ToInt64(dataRow.SectionId), commonTreeSection))
                        {
                            childNode.ChildNodes.Add(cnode);
                            childNode.NavigateUrl = "javascript:return false," + dataRow.SectionId;
                        }

                        childtreenodes.Add(childNode);
                    }
                }

                return childtreenodes;
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }

            return null;

        }


        private TreeNodeCollection GetRestrictedFolderChildNodes(long parentId, List<TreeDetail> commonTreeSection, string parentNodeValuePath)
        {
            TreeNodeCollection childtreenodes = new TreeNodeCollection();
            List<TreeDetail> childSection = tree.GetChildNodesByParentId(TableName, parentId);
            try
            {

                if (childSection.Count > 0)
                {
                    foreach (TreeDetail dataRow in childSection)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = dataRow.Name.ToString();
                        childNode.Value = dataRow.SectionId.ToString();

                        string contenttype = dataRow.ContentType.ToString();

                        /* if (TableName == enTreeType.ProjectSection)
                         {
                             parentValuePath = parentNodeValuePath + "/" + dataRow.SectionId.ToString();
                             childNode.NavigateUrl = "javascript:onnodeclick('" + contenttype + "','" +
                             dataRow.SectionId.ToString() + "','" + System.Web.HttpUtility.JavaScriptStringEncode(parentValuePath) + "');";
                         }
                         else
                         {*/
                        childNode.NavigateUrl = "javascript:onclicknode('" + contenttype + "','" +
                            dataRow.SectionId.ToString() + "');";
                        /* }*/

                        foreach (TreeNode cnode in GetRestrictedFolderChildNodes(Convert.ToInt64(dataRow.SectionId), commonTreeSection, parentValuePath))
                        {
                            childNode.ChildNodes.Add(cnode);
                        }

                        childtreenodes.Add(childNode);
                    }
                }

                return childtreenodes;
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
            }

            return null;
        }

        private void GetChild(TreeNode nodes, int index)
        {
            foreach (TreeNode Childnode in nodes.ChildNodes)
            {
                if (Childnode.Value == index.ToString())
                {
                    nodes.Expanded = true;
                    Childnode.Selected = true;
                    Childnode.ExpandAll();// = true;                   
                    break;
                }
                if (Childnode.ChildNodes.Count > 0)
                    GetChild(Childnode, index);

            }
        }

        //***********************************
        //This method used to generate the context menu dynamically
        //***********************************

        private void DisplayContextMenu()
        {
            string ctxtmenu = "<ul id='myMenu' class='contextMenu'>";
            ctxtmenu += "<li class='add'><a href='#add'><span class='addspan'>Add</span></a></li>";
            ctxtmenu += "<li class='rename'><a href='#rename'>Rename</a></li>";
            ctxtmenu += "<hr/>";
            ctxtmenu += "<li class='delete'><a id=delete href='#delete'>Delete</a></li></ul>";

            contextMenuContainer.InnerHtml = ctxtmenu;
        }

        private void GetRestrictedFoldersNameCSV()
        {
            foreach (TreeDetail dataRow in restrictedFolders)
            {
                hdnRestrictedFolderList.Value += dataRow.Name + ",";
                AppendChildNodeName(dataRow.SectionId);
            }
        }

        public void AppendChildNodeName(int parentId)
        {
            try
            {
                List<TreeDetail> childSection = tree.GetChildNodesByParentId(TableName, parentId);
                foreach (TreeDetail section in childSection)
                {
                    hdnRestrictedFolderList.Value += section.Name + ",";
                    AppendChildNodeName(section.SectionId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int HasChildNodes(long parentId, List<TreeDetail> commonTreeSection)
        {

            TreeNodeCollection childtreenodes = new TreeNodeCollection();

            List<TreeDetail> childSection = null;
            if (TableName == enTreeType.ToolSection)
            {
                childSection = (from childRecords in commonTreeSection
                                where childRecords.ParentSectionId == parentId
                                orderby childRecords.Name
                                select childRecords).ToList();
            }
            return childSection.Count();
        }

        private bool IsOneLevelFolder(int projectSectionId, enTreeType treeType)
        {
            try
            {
                TreeBO _tree = new TreeBO();
                return _tree.IsOneLevelFolder(projectSectionId, treeType);
            }
            catch
            {
                return false;
            }
        }

        private bool IsSiteLevelTool(int parentProjectSectionId)
        {
            try
            {
                return tree.IsSiteLevelTool(parentProjectSectionId);
            }
            catch
            {
                return false;
            }
        }

        private bool ToolHasDocumemtStore(int projectSectionId)
        {
            try
            {
                return tree.ToolHasDocumemtStore(projectSectionId);
            }
            catch
            {
                return false;
            }
        }

        private string GetNameOfDocumentStore(int projSectionIdOfToolHasDocs)
        {
            try
            {
                return tree.GetNameOfDocumentStore(projSectionIdOfToolHasDocs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ToolHasStandardFolder(int projectSectionId)
        {
            try
            {
                return tree.ToolHasStandardFolder(projectSectionId);
            }
            catch
            {
                return false;
            }
        }

        private string GetNameOfStandardFolder(int projSectionIdOfToolHasDocs)
        {
            try
            {
                return tree.GetNameOfStandardFolder(projSectionIdOfToolHasDocs);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}