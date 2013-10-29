#region Modification History
// *******************************************************************************
// Module       : DocumentVista.cs 
// Created By   : Muhammad Adil Butt
// Created On   : 9/03/2012 
// Description  : Generic class to present the views of diffrent documents for C# Language 
//
// **************************** Modification History *****************************
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
using System.Collections;
using System.Text.RegularExpressions;

namespace BMT.UserControls
{
    public partial class DocumentVista : System.Web.UI.UserControl
    {

        #region CONSTANTS
        private const string DEFAULT_TABLE_CSS_CLASS = "table1";
        private const string MULTI_TABLE_CSS_CLASS = "multitable1";
        private const string DEFAULT_IMAGE_CSS_CLASS = "feedimage";
        private const string DEFAULT_LINK_CSS_CLASS = "feedlink";
        private const string MULTI_LINK_CSS_CLASS = "multifeedlink";
        private const string DEFAULT_DESCRIPTION_CSS_CLASS = "feeddesc";
        private const string GROUP_DESCRIPTION_CLASS = "groupDescription";
        private const string REFERENCE_DESCRIPTION_CSS_CLASS = "referencedesc";
        
        #endregion

        #region VARIABLES
        private  List<DocDetails> _listOfDocs;
        private string userType;
        #endregion

        #region PROPERTY

        public enDbTables DBTableName { get; set; }

        public int LibrarySectionId
        {

            get;
            set;
        }

        public int PracticeId
        {

            get;
            set;
        }

        public int ProjectId
        {
            get;
            set;

        }

        public int ProjectUsageId
        {
            get;
            set;

        }

        public int ToolSectionId
        {

            get;
            set;
        }

        public string ContentType
        {

            get;
            set;
        }

        public string ToolSectionName
        {

            get;
            set;
        }

        public int TemplateId
        {

            get;
            set;
        }

        #endregion

        #region EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Visible)
                    return;

                if (Session["UserType"] != null)
                {
                    userType = Session["UserType"].ToString();
                    _listOfDocs = new List<DocDetails>();
                    DocumentViewerSelection(DBTableName);
                    if (TemplateId != 0)
                        ShowStandardDocument(TemplateId);
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        #endregion

        # region FUNCTIONS

        private void DocumentViewerSelection(enDbTables enDbTableName)
        {

            switch (enDbTableName)
            {
                case enDbTables.LibraryDocument:
                    LibraryBO objLibraryDocument = new LibraryBO();
                    _listOfDocs = objLibraryDocument.GetDocuments(LibrarySectionId, PracticeId);
                    break;
                case enDbTables.ToolDocument:
                    ToolDocumentBO objToolDocument = new ToolDocumentBO();
                    _listOfDocs = objToolDocument.GetDocuments(ToolSectionId, PracticeId);
                    List<string> toolSectionAttr = objToolDocument.GetToolSectionAttrById(ToolSectionId);
                    ContentType = toolSectionAttr[0];
                    ToolSectionName = toolSectionAttr[1];
                    break;
                case enDbTables.ProjectDocument:
                    ProjectDocumentBO objProjectDocument = new ProjectDocumentBO();
                    _listOfDocs = objProjectDocument.GetDocuments(ToolSectionId, ProjectUsageId);
                    break;
            }

            Show(_listOfDocs);

        }

        private void ShowStandardDocument(int templateId)
        {
            ProjectDocumentBO objProjectDocument = new ProjectDocumentBO();
            _listOfDocs = objProjectDocument.GetStandardDocument(templateId);
            Show(_listOfDocs);
        }

        private void Show(List<DocDetails> _recievedDocList)
        {
            Table _docsTable;
            TableRow docTableRow = new TableRow();
            TableCell docTableCell;
            Panel imagePanel;
            HyperLink _hyperLink;
            Label _label;

            if (ContentType == "TripleColumn")
            {
                int cellCount = 0;
                int rowCount = 0;
                int totalDocCount = 0;
                string groupDescription = string.Empty;
               
                if (_recievedDocList.Count != 0)
                {
                    Table docTable = new Table();

                    if (ToolSectionName == "Algorithms and Pathways")
                    {
                        string referenceText = "These tools are developed and maintained by the Department of Family and Community Medicine at the University of Missouri, and its partners.";
                        referenceText += "<br/>";
                        referenceText += "If you would like to integrate these and other Clinical Decision Support tools into your EHR, please contact <a href=mailto:" + "kochendorferk@health.missouri.edu>Dr. Karl Kochendorfer</a> at the University of Missouri.";
                        _label = new Label();
                        _label.Text = referenceText;
                        _label.CssClass = REFERENCE_DESCRIPTION_CSS_CLASS;

                        docTableRow = new TableRow();
                        docTableCell = new TableCell();
                        docTableCell.ColumnSpan = 3;

                        docTableCell.Controls.Add(_label);
                        docTableRow.Controls.Add(docTableCell);
                        docTable.Controls.Add(docTableRow);  
                    }


                    if (ToolSectionName != "Algorithms and Pathways")
                    {
                        docTableRow = new TableRow();
                        docTableCell = new TableCell();
                        docTableCell.ColumnSpan = 3;
                        docTableCell.HorizontalAlign = HorizontalAlign.Right;
                        docTableCell.Style.Add("padding-right", "20px");
                        docTableCell.Style.Add("font-weight", "bold");

                        _hyperLink = new HyperLink();
                        _hyperLink.ClientIDMode = ClientIDMode.Static;
                        _hyperLink.ID = "MakeASuggestion";
                        _hyperLink.Text = "Suggest a Resource";
                        _hyperLink.NavigateUrl = "javascript:return false;";

                        docTableCell.Controls.Add(_hyperLink);
                        docTableRow.Controls.Add(docTableCell);
                        docTable.Controls.Add(docTableRow);
                    }

                    foreach (DocDetails doc in _recievedDocList)
                    {
                        rowCount = rowCount + 1;

                        _docsTable = new Table();
                        docTableCell = new TableCell();

                        if (doc.Description != groupDescription && doc.Description != null)
                        {
                            docTableCell.ColumnSpan = 2;

                            if (docTableRow.Cells.Count > 0)
                            {
                                cellCount = 0;
                                docTable.Controls.Add(docTableRow);
                                docTableRow = new TableRow();
                            }

                            groupDescription = doc.Description;

                            if (groupDescription.Contains("~/"))
                            {
                                string[] urlAttributes = Regex.Split(groupDescription, "---");

                                Image instituteImage = new Image();                                
                                instituteImage.ImageUrl = urlAttributes[0];

                                Panel instituteImagePanel = new Panel();
                                instituteImagePanel.Controls.Add(instituteImage);

                                _hyperLink = new HyperLink();
                                _hyperLink.NavigateUrl = urlAttributes[1];
                                _hyperLink.Target = urlAttributes[1];
                                _hyperLink.CssClass = MULTI_LINK_CSS_CLASS;

                                _hyperLink.Controls.Add(instituteImagePanel);
                                docTableCell.Controls.Add(_hyperLink);                                
                            }
                            else
                            {
                                _label = new Label();
                                _label.Text = groupDescription;
                                _label.CssClass = GROUP_DESCRIPTION_CLASS;

                                docTableCell.Controls.Add(_label);
                            }

                            docTableRow.Controls.Add(docTableCell);
                            docTable.Controls.Add(docTableRow);

                            docTableRow = new TableRow();
                            docTableCell = new TableCell();
                        }

                        if (doc.Image != null)
                        {
                            TableRow _tableRow = new TableRow();
                            _tableRow.ID = "tr" + rowCount;
                            _tableRow.ClientIDMode = ClientIDMode.Static;
                            _tableRow.CssClass = MULTI_TABLE_CSS_CLASS;

                            // create cell
                            TableCell _tableData = new TableCell();
                            _tableData.Width = Unit.Pixel(25);

                            // set image properties
                            Image docImage = new Image();
                            docImage.ImageUrl = doc.Image;
                            docImage.CssClass = DEFAULT_IMAGE_CSS_CLASS;

                            imagePanel = new Panel();
                            imagePanel.Controls.Add(docImage);

                            _hyperLink = new HyperLink();
                            _hyperLink.NavigateUrl = doc.Link;
                            _hyperLink.Target = doc.Link;
                            _hyperLink.CssClass = MULTI_LINK_CSS_CLASS;

                            _hyperLink.Controls.Add(imagePanel);
                            _tableData.Controls.Add(_hyperLink);
                            _tableRow.Controls.Add(_tableData);

                            // Create new Cell for Doc name
                            _tableData = new TableCell();
                            _tableData.Width = Unit.Pixel(240);

                            // set hyper link control proerties
                            _hyperLink = new HyperLink();
                            _hyperLink.Text = doc.Name;
                            _hyperLink.NavigateUrl = doc.Link;
                            _hyperLink.Target = doc.Link;
                            _hyperLink.CssClass = MULTI_LINK_CSS_CLASS;

                            // add hyper link tag in cell#2
                            _tableData.Controls.Add(_hyperLink);
                            _tableRow.Controls.Add(_tableData);                            

                            //if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                            //{
                            //    SuperUserSelection(doc);
                            //}

                            // add created row into table
                            _docsTable.Controls.Add(_tableRow);
                            docTableCell.Controls.Add(_docsTable);
                            docTableRow.Controls.Add(docTableCell);

                            cellCount++;
                            totalDocCount++;

                            if (cellCount == 3)
                            {
                                cellCount = 0;
                                docTable.Controls.Add(docTableRow);
                                docTableRow = new TableRow();
                            }

                            if (totalDocCount == _recievedDocList.Count())
                            {
                                docTable.Controls.Add(docTableRow);
                            }
                        }
                        else
                        {
                            totalDocCount++;
                        }

                    }

                    docTableRow = new TableRow();
                    docTableCell = new TableCell();
                    docTableCell.ColumnSpan = 3;

                    if (ToolSectionName == "Algorithms and Pathways")
                    {
                        _label = new Label();
                        _label.Text = "*All trademarks or logos are the property of their respective owners.";
                        _label.CssClass = REFERENCE_DESCRIPTION_CSS_CLASS;
                        _label.Style.Add("font-style", "italic");

                        docTableCell.Controls.Add(_label);
                    }

                    _label = new Label();
                    _label.Text = "<br/>" + "NOTE: External links are provided as a resource to our users. We accept no responsibility for the content of linked sites";
                    _label.CssClass = DEFAULT_DESCRIPTION_CSS_CLASS;
                    _label.Style.Add("color", "#595959");

                    docTableCell.Controls.Add(_label);
                    docTableRow.Controls.Add(docTableCell);
                    docTable.Controls.Add(docTableRow);

                    pnlDocuments.Controls.Add(docTable);
                }
            }
            else
            {
                int rowCount = 0;
                if (_recievedDocList.Count != 0)
                {
                    _docsTable = new Table();

                    foreach (DocDetails doc in _recievedDocList)
                    {
                        // TO assign unique id to each row
                        rowCount = rowCount + 1;

                        TableRow _tableRow = new TableRow();
                        _tableRow.ID = "tr" + rowCount;
                        _tableRow.ClientIDMode = ClientIDMode.Static;
                        _tableRow.CssClass = DEFAULT_TABLE_CSS_CLASS;

                        // create cell
                        TableCell _tableData = new TableCell();
                        _tableData.Width = Unit.Pixel(25);

                        // set image properties
                        Image docImage = new Image();
                        docImage.ImageUrl = doc.Image;
                        docImage.CssClass = DEFAULT_IMAGE_CSS_CLASS;

                        // add image into cell#1
                        _tableData.Controls.Add(docImage);
                        _tableRow.Controls.Add(_tableData);

                        // Create new Cell for Doc name
                        _tableData = new TableCell();
                        _tableData.Width = Unit.Pixel(625);

                        // set hyper link control proerties
                        _hyperLink = new HyperLink();
                        _hyperLink.Text = doc.Name;
                        _hyperLink.NavigateUrl = doc.Link;
                        _hyperLink.Target = doc.Link;
                        _hyperLink.CssClass = DEFAULT_LINK_CSS_CLASS;

                        // add hyper link tag in cell#2
                        _tableData.Controls.Add(_hyperLink);
                        _tableRow.Controls.Add(_tableData);

                        // set label properties to display the doc description
                        _label = new Label();
                        _label.Text = "<br/>" + doc.Description;
                        _label.CssClass = DEFAULT_DESCRIPTION_CSS_CLASS;

                        // add description current cell (cell#2)
                        _tableData.Controls.Add(_label);
                        _tableRow.Controls.Add(_tableData);

                        if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                        {
                            SuperUserSelection(doc,_docsTable,_tableRow,_tableData);

                        }

                        // add created row into table
                        _docsTable.Controls.Add(_tableRow);
                    }

                    pnlDocuments.Controls.Add(_docsTable);
                }
            }
        }

        private void SuperUserSelection(DocDetails doc, Table _docsTable,TableRow _tableRow,TableCell _tableData)
        {
            // cell#3 add control to delete the document
            _tableData = new TableCell();
            _tableData.Width = Unit.Pixel(15);

            Literal _literal = new Literal();

            // ajax loading image
            _literal.Text = "<img id=\"imgajax" + doc.DocumentId + "\" ";
            _literal.Text += "src=\"../Themes/Images/ajax-mini-loader.gif\" style=\"display:none;\" /> ";

            // delete image
            _literal.Text += "<img id=\"imgdelete" + doc.DocumentId + "\" ";
            _literal.Text += "src=\"../Themes/Images/delete.png\" class=\"img-delete\" alt=\"Delete\" ";
            _literal.Text += "onClick=\"javascript:ProcessDeleteFile('" + doc.DocumentId + "',";
            _literal.Text += "'" + doc.Link + "','" + DBTableName.ToString() + "','imgajax','imgdelete','" + _tableRow.ID + "');\" ";

            // add delete image control
            _tableData.Controls.Add(_literal);
            _tableRow.Controls.Add(_tableData);

            _docsTable.Controls.Add(_tableRow);
            pnlDocuments.Controls.Add(_docsTable);
        }

        #endregion

    }
}

