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
    public partial class DeleteFiles : System.Web.UI.Page
    {
        #region CONSTANTS
        private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        #endregion

        #region VARIABLES
        XDocument Questionnaire;
        QuestionBO question = new QuestionBO();

        private int pageNo;
        private string docLinkedTo;

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
                          Request.QueryString["siteId"] != null)
                {
                    // get and parse the Questionnaire
                    string RecievedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
                    Questionnaire = XDocument.Parse(RecievedQuestionnaire);

                    // fetching value from querystring
                    string pcmhSequence = Request.QueryString["pcmh"];
                    string elementSequence = Request.QueryString["element"];
                    string factorSequence = Request.QueryString["factor"];
                    string file = Request.QueryString["file"];
                    int projectId = Convert.ToInt32(Request.QueryString["project"]);
                    int practiceId = Convert.ToInt32(Request.QueryString["practiceId"]);
                    int siteId = Convert.ToInt32(Request.QueryString["siteId"]);

                    // Send all required information into file handler process
                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, projectId, practiceId, siteId);
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
                                   string file, int projectId, int practiceId, int siteId)
        {

            try
            {
                Session["pcmhId"] = pcmhSequence;
                Session["elementId"] = elementSequence;
                string[] fileName = file.Split('|');
                string filePath = string.Empty;

                // get file path and remove file from XML
                filePath = GetFilePath(fileName[0], pcmhSequence, elementSequence, factorSequence);

                // count number of referenced links against current file
                int NumerOfRefrences = CountFileReferences(fileName[0]);

                // TODO: Create Local Disk path to move the selected file into unassociated docs folder
                filePath = Util.GetPathAndQueryByURL(filePath);
                filePath = Util.ExtractDocPath(filePath);

                // TODO: if deleting file is not linked with any factor then move it to unassociated Folder
                if (NumerOfRefrences == 0 && elementSequence != "0" && factorSequence != "0")
                    MoveFile(file, filePath, practiceId, siteId);
                else if (elementSequence == "0" && factorSequence == "0")
                    DeleteFile(filePath);

                // save questionnaire
                SaveQuestionnaire(projectId);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private int CountFileReferences(string file)
        {

            // Number of links exist against selected file
            IEnumerable<XElement> docLinks = from docLink in Questionnaire.Descendants("DocFile")
                                             where docLink.Attribute("location").Value.Contains(file)
                                             select docLink;

            return docLinks.Count();
        }

        private string GetFilePath(string file, string pcmhSequence, string elementSequence, string factorSequence)
        {

            string filePath = string.Empty;
            string[] fileName = file.Split('|');

            //Find UnAssociated Documents
            IEnumerable<XElement> unAssociatedDocs = from unAssociatedDoc in Questionnaire.Descendants("UnAssociatedDoc")
                                                     select unAssociatedDoc;
            // Standards = PCMH
            // Find standard by sequence No
            IEnumerable<XElement> standards = from standard in Questionnaire.Descendants("Standard")
                                              where (string)standard.Attribute("sequence").Value == pcmhSequence
                                              select standard;

            // Find element by sequence No
            IEnumerable<XElement> elements = from element in standards.Descendants("Element")
                                             where (string)element.Attribute("sequence").Value == elementSequence
                                             select element;
            // Find factor by sequence No
            IEnumerable<XElement> factors = from factor in elements.Descendants("Factor")
                                            where (string)factor.Attribute("sequence").Value == factorSequence
                                            select factor;

            // Find file by file name from NCQA Submission Only
            IEnumerable<XElement> docs = from doc in factors.Descendants("DocFile")
                                         where (string)doc.Attribute("location").Value.Substring(doc.Attribute("location").Value.LastIndexOf('/') + 1) == fileName[0]
                                         select doc;

            // if file deleting from UnAssociated Documents Folder
            if (elementSequence == "0" || factorSequence == "0")
            {
                docs = from doc in unAssociatedDocs.Descendants("DocFile")
                       where (string)doc.Attribute("location").Value.Substring(doc.Attribute("location").Value.LastIndexOf('/') + 1) == fileName[0]
                       select doc;
            }

            // if file deleted from Document Viewer popUp
            if (docLinkedTo != string.Empty)
            {
                string[] docReferences = docLinkedTo.Split(',');
                int index = 0;

                for (index = 0; index < docReferences.Count(); index++)
                {
                    pcmhSequence = docReferences[index].Substring(0, 1);
                    elementSequence = docReferences[index].Substring(1, 1);
                    factorSequence = docReferences[index].Substring(2);

                    for (int character = 0; character < DEFAULT_LETTERS.Length; character++)
                    {
                        if (DEFAULT_LETTERS[character] == elementSequence)
                        {
                            elementSequence = (character + 1).ToString();
                            break;
                        }
                    }

                    docs = from doc in Questionnaire.Descendants("DocFile")
                           where (string)doc.Attribute("location").Value.Substring(doc.Attribute("location").Value.LastIndexOf('/') + 1) == fileName[0]
                            && (string)doc.Parent.Parent.Attribute("sequence") == factorSequence
                            && (string)doc.Parent.Parent.Parent.Attribute("sequence") == elementSequence
                            && (string)doc.Parent.Parent.Parent.Parent.Attribute("sequence") == pcmhSequence
                           select doc;

                    foreach (XElement doc in docs)
                    {
                        filePath = doc.Attribute("location").Value;
                        string docsType="";
                        if (doc.Parent.Name == "Policies")
                            docsType ="PoliciesOrProcess";
                        else if(doc.Parent.Name == "Reports")
                            docsType = "ReportsOrLogs";
                        else if (doc.Parent.Name == "Screenshots")
                            docsType = "ScreenshotsOrExamples";
                        else if (doc.Parent.Name == "LogsOrTools")
                            docsType = "RRWB";
                        else if (doc.Parent.Name == "OtherDocs")
                            docsType = "Extra";

                        string javaScript = "window.parent.TrackDocType('" + docsType + "'); ";
                        javaScript += "window.parent.$('#hdnPCMHId').val(" + pcmhSequence + "); ";
                        javaScript += "window.parent.$('#hdnElementId').val(" + elementSequence + "); ";
                        javaScript += "window.parent.$('#hdnFactorId').val(" + factorSequence + "); ";
                        javaScript += "window.parent.UpdateDocStatusOnParentWindow(); ";

                        Response.Write("<script language='javascript'> { " + javaScript + " }</script>");
                        break;
                    }

                    // remove document from XML
                    if (docs.Count() != 0)
                        docs.Remove();
                }


            }

            // get file path
            if (filePath == string.Empty)
            {
                foreach (XElement doc in docs)
                {
                    filePath = doc.Attribute("location").Value;
                    break;
                }


                // remove document from XML
                if (docs.Count() != 0)
                    docs.Remove();
            }

            return filePath;
        }

        private void MoveFile(string file, string filePath, int practiceId, int siteId)
        {

            try
            {
                string UnAssociatedDocPath = Util.GetUnAssociatedDocPath(practiceId, siteId);
                string[] fileName = file.Split('|');

                if (!Directory.Exists(UnAssociatedDocPath))
                    Directory.CreateDirectory(UnAssociatedDocPath);

                UnAssociatedDocPath += "/" + fileName[0];
                File.Move(filePath, UnAssociatedDocPath);

                // create server path to open it through browser
                UnAssociatedDocPath = UnAssociatedDocPath.Substring(UnAssociatedDocPath.IndexOf(Util.GetDocRootPath()));
                UnAssociatedDocPath = Util.GetHostPath() + UnAssociatedDocPath;

                // remove timestamp from filename
                //file = file.Substring(14);

                // check if unassociated doc element exists
                foreach (XElement questionElement in Questionnaire.Elements())
                {
                    IEnumerable<XElement> unAssociatedDocs = from unAssociatedDoc in Questionnaire.Descendants("UnAssociatedDoc")
                                                             select unAssociatedDoc;

                    if (unAssociatedDocs.Count() == 0)
                    {
                        XElement docFile = new XElement("UnAssociatedDoc", new XElement("DocFile",
                                                                           new XAttribute("name", fileName[1]),
                                                                           new XAttribute("location", UnAssociatedDocPath)),
                                                                           new XAttribute("sequence", "1"),
                                                                           new XAttribute("title", "UNASSOCIATED DOCUMENTS"));
                        questionElement.Add(docFile);
                    }
                    else
                    {
                        foreach (XElement doc in unAssociatedDocs)
                        {
                            XElement docFile = new XElement("DocFile",
                                                            new XAttribute("name", fileName[1]),
                                                            new XAttribute("location", UnAssociatedDocPath));
                            doc.Add(docFile);
                        }
                    }

                    break;
                }
            }
            catch (FileNotFoundException exception)
            {

            }
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private void SaveQuestionnaire(int projectId)
        {

            question = new QuestionBO();
            int userId = Convert.ToInt32(Session["UserApplicationId"]);
            int questionnaireId = Convert.ToInt32(Session["NCQAQuestionnaireId"]);

            if (projectId > 0 && userId > 0)
            {
                // Update Questionnaire in db
                question.SaveFilledQuestionnaire(questionnaireId, projectId, Questionnaire.Root, userId);

                // update Questionnaire in Session
                Session["NCQAQuestionnaire"] = Questionnaire.Root.ToString();

            }

            EndUserResponse();
        }

        private void EndUserResponse()
        {
            // Close window after deleting the file
            if (pageNo == (int)enDocsPageName.NCQAUploadedDocs)
                Response.Write("<script language='javascript'> {window.parent.location.href = window.parent.location.href; window.parent.$('#lightbox-popup-delete').css('opacity', '100');window.parent.$('.lightbox, #lightbox-popup-delete').fadeOut(300); }</script>");
            else
                Response.Write("<script language='javascript'> {window.parent.$('.lightbox, .lightbox-popup').fadeOut(300); }</script>");

        }
        #endregion
    }
}