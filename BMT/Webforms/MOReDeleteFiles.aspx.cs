using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Configuration;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT.Webforms
{
    public partial class MOReDeleteFiles : System.Web.UI.Page
    {
        #region CONSTANTS
        private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        #endregion

        #region VARIABLES
        MOReBO MORe = new MOReBO();
        private int pageNo;
        private string docLinkedTo;
        private string operation;
        private int TemplateId;
        private int projectUsageId;
        #endregion

        #region EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check calling page
                if (Request.QueryString["pageNo"] != null)
                    pageNo = Convert.ToInt32(Request.QueryString["pageNo"]);
                else
                    pageNo = (int)enDocsPageName.NCQASubmission;

                // check all available linked
                if (Request.QueryString["docLinkedTo"] != null)
                    docLinkedTo = Request.QueryString["docLinkedTo"].ToString();
                else
                    docLinkedTo = string.Empty;

                if (Request.QueryString["pcmh"] != null && Request.QueryString["element"] != null && Request.QueryString["factor"] != null &&
                          Request.QueryString["file"] != null && Request.QueryString["project"] != null && Request.QueryString["practiceId"] != null &&
                          Request.QueryString["siteId"] != null && Session["TemplateId"] != null && Request.QueryString["operation"] != null)
                {
                    // get and parse the Questionnaire
                    //string RecievedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
                    //Questionnaire = XDocument.Parse(RecievedQuestionnaire);

                    // fetching value from querystring
                    string pcmhSequence = Request.QueryString["pcmh"];
                    string elementSequence = Request.QueryString["element"];
                    string factorSequence = Request.QueryString["factor"];
                    string file = Request.QueryString["file"];
                    projectUsageId = Convert.ToInt32(Request.QueryString["project"]);
                    int practiceId = Convert.ToInt32(Request.QueryString["practiceId"]);
                    int siteId = Convert.ToInt32(Request.QueryString["siteId"]);
                    TemplateId = Convert.ToInt32(Session["TemplateId"]);
                    operation = Request.QueryString["operation"];

                    // Send all required information into file handler process
                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, projectUsageId, practiceId, siteId, TemplateId, operation);
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                EndUserResponse();
            }
        }

        #endregion

        #region FUNCTION
        protected void FileHandler(string pcmhSequence, string elementSequence, string factorSequence,
                                   string file, int projectUsageId, int practiceId, int siteId, int templateId, string operation)
        {

            try
            {
                Session["pcmhId"] = pcmhSequence;
                Session["elementId"] = elementSequence;
                file = file.Replace("hashsign", "#");
                string[] fileName = file.Split('|');
                string filePath = string.Empty;

                // TODO: Create Local Disk path to move the selected file into unassociated docs folder
                filePath = Util.GetPathAndQueryByURL(fileName[0]);
                filePath = Util.ExtractDocPath(filePath);

                // TODO: if deleting file is not linked with any factor then move it to unassociated Folder
                if (operation == "deleteUnAssociated")
                {
                    DeleteUnAssociatedFiles(file, siteId, templateId, projectUsageId);
                }
                else if (operation != "save")
                {
                    MoveFile(file, filePath, practiceId, siteId, templateId, projectUsageId);
                }
                else
                    DeleteFile(fileName[0],siteId);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //private int CountFileReferences(string file)
        //{

        //    int Count = MORe.GetFileReferences(file);

        //    return Count;
        //}



        private void MoveFile(string file, string filePath, int practiceId, int siteId, int templateId, int projectUsageId)
        {
            string UnAssociatedDocPath = string.Empty;
            string FileName = string.Empty;
            string[] fileNam = file.Split('|');

            try
            {
                UnAssociatedDocPath = Util.GetUnAssociatedDocPathForMORe(practiceId, siteId, templateId);
                string[] fileName = file.Split('|');

                if (!Directory.Exists(UnAssociatedDocPath))
                    Directory.CreateDirectory(UnAssociatedDocPath);

                int startIndex = fileName[0].LastIndexOf('/') + 1;
                string Name = fileName[0].Substring(startIndex, fileName[0].Length - startIndex);
                UnAssociatedDocPath += "/" + Name;
                FileName = fileName[0];
                File.Move(filePath, UnAssociatedDocPath);

            }
            
            finally
            {

                try
                {
                    // create server path to open it through browser
                    UnAssociatedDocPath = UnAssociatedDocPath.Substring(UnAssociatedDocPath.IndexOf(Util.GetDocRootPath()));
                    UnAssociatedDocPath = Util.GetHostPath() + UnAssociatedDocPath;

                    List<TemplateDocument> DocLinkCount = MORe.GetDocLinkCount(FileName, templateId, projectUsageId,siteId);
                    foreach (TemplateDocument doc in DocLinkCount)
                    {
                        string AllSequence = MORe.GetSequence(Convert.ToInt32(doc.DocumentId),templateId);

                        string[] Sequence = AllSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string pcmhSequence = Sequence[2];
                        string elementSequence = Sequence[1];
                        string factorSequence = Sequence[0];
                        string docType = doc.DocumentType;
                        string docs="";
                        if (docType == "Policies")
                            docs = "PoliciesOrProcess";
                        else if (docType == "Reports")
                            docs = "ReportsOrLogs";
                        else if (docType == "Screenshots")
                            docs = "ScreenshotsOrExamples";
                        else if (docType == "LogsOrTools")
                            docs = "RRWB";
                        else if (docType == "OtherDocs")
                            docs = "Extra";

                        string javaScript = "window.parent.TrackDocType('" + docs + "'); ";
                        javaScript += "window.parent.$('#hdnPCMHId').val(" + pcmhSequence + "); ";
                        javaScript += "window.parent.$('#hdnElementId').val(" + elementSequence + "); ";
                        javaScript += "window.parent.$('#hdnFactorId').val(" + factorSequence + "); ";
                        javaScript += "window.parent.UpdateDocStatusOnParentWindow(); ";

                        Response.Write("<script language='javascript'> { " + javaScript + " }</script>");
                    }

                    MORe.DeleteFile(FileName, UnAssociatedDocPath, templateId, projectUsageId,siteId);
                    EndUserResponse();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        private void DeleteFile(string path,int siteId)
        {

            try
            {

                string[] docLinkedArray = docLinkedTo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int startIndex = path.LastIndexOf('/') + 1;
                int length = path.Length - (path.Length - path.LastIndexOf('.')) - startIndex;
                string Name = path.Substring(startIndex, length);

                foreach (string link in docLinkedArray)
                {

                    string[] removeLinks = link.Split(DEFAULT_LETTERS, StringSplitOptions.RemoveEmptyEntries);

                    int headerSequence = Convert.ToInt32(removeLinks[0]);

                    int subHeaderSequence = 0;

                    for (int index = 0; index < DEFAULT_LETTERS.Length; index++)
                    {
                        if (docLinkedTo.Contains(DEFAULT_LETTERS[index]))
                        {
                            subHeaderSequence = index + 1;
                            break;

                        }
                    }

                    int questionSequence = Convert.ToInt32(removeLinks[1]);

                    int QuestionId = MORe.GetQuestionIdBySequence(headerSequence, subHeaderSequence, questionSequence, TemplateId);

                    int documentId = MORe.GetDocumentId(QuestionId, TemplateId, projectUsageId, siteId, path);

                    MORe.DeleteDocument(documentId);

                    string javaScript = "window.parent.TrackDocType('" + Name + "'); ";
                    javaScript += "window.parent.$('#hdnPCMHId').val(" + headerSequence + "); ";
                    javaScript += "window.parent.$('#hdnElementId').val(" + subHeaderSequence + "); ";
                    javaScript += "window.parent.$('#hdnFactorId').val(" + questionSequence + "); ";
                    javaScript += "window.parent.UpdateDocStatusOnParentWindow(); ";

                    Response.Write("<script language='javascript'> { " + javaScript + " }</script>");

                }

                EndUserResponse();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void EndUserResponse()
        {
            // Close window after deleting the file
            if (pageNo == (int)enDocsPageName.NCQAUploadedDocs)
                Response.Write("<script language='javascript'> {window.parent.location.href = window.parent.location.href; window.parent.$('#lightbox-popup-delete').css('opacity', '100');window.parent.$('.lightbox, #lightbox-popup-delete').fadeOut(300); }</script>");
            else
                Response.Write("<script language='javascript'> {window.parent.$('.lightbox, .lightbox-popup').fadeOut(300); }</script>");

        }

        private void DeleteUnAssociatedFiles(string file , int siteId, int templateId, int projectId)
        {
            try
            {
                MORe.DeleteUnAssociatedFiles(projectId,siteId,templateId,file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}