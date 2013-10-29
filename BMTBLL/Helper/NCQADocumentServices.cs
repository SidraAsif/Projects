using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Xml;
using System.Web;
using System.IO;
using System.Security;

using BMTBLL;
using BMTBLL.Enumeration;
using log4net;
using log4net.Config;

using Ncqa.Iss.DocumentService.Interface;

namespace BMTBLL.Helper
{
    public class NCQADocumentServices
    {
        private List<Document> lstDocuments;

        private static string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public void SubmitNCQADocuments(string license, string user, string password, int projectId, int enterpriseId, string mapPath, Uri uri, string applicationPath, log4net.ILog log, int siteId, int practiceId, string recievedQuestionnaire)
        {
            try
            {
                ProjectBO projectBO = new ProjectBO();
                string siteName = projectBO.GetSiteNameByProjectID(projectId);

                log.DebugFormat("--------------");
                log.DebugFormat("--------------");
                log.DebugFormat("Submission started at {0} for siteName={1}, license={2}, user={3}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now), siteName, license, user);

                string apikey = ConfigurationManager.AppSettings["APIKey"].ToString();

                var client = new Ncqa.Iss.DocumentService.Interface.DocumentServiceClient("ncqa-public-staging", apikey, user, password);

                DeleteExistingDocumentsAndLinks(client, license, log);

                lstDocuments = new List<Document>();

                if (recievedQuestionnaire != string.Empty)
                {
                    XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                    foreach (XElement standard in questionnaire.Root.Elements("Standard"))
                    {
                        string pcmhSequenceId = standard.Attribute("sequence").Value;

                        foreach (XElement element in standard.Elements("Element"))
                        {
                            string elementSequenceId = element.Attribute("sequence").Value;
                            List<NCQADetails> listOfNCQADetail = NCQADataHelper.GetDocsByElementId(pcmhSequenceId, elementSequenceId, practiceId, siteId, projectId, recievedQuestionnaire);

                            //Add Documents
                            AddDocuments(client, listOfNCQADetail, license, mapPath, log);
                        }
                    }

                    //Link Documents to elements
                    LinkDocumentToElements(client, questionnaire, license, uri, log, projectId);


                    Email email = new Email();
                    email.SendNCQASuccessful(projectId, license, enterpriseId, applicationPath);

                    log.DebugFormat("Submission ended at {0}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ERROR: {0} ", ex.Message);
            }
        }

        public void AddDocuments(DocumentServiceClient client, List<NCQADetails> listOfNCQADetail, string license, string mapPath, log4net.ILog log)
        {
            try
            {
                Document document = null;

                foreach (NCQADetails ncqaDetails in listOfNCQADetail)
                {
                    if (!(ncqaDetails.Type.Contains("Note") || ncqaDetails.Type.Contains("Comment")))
                    {
                        string location = mapPath + ncqaDetails.Title.Substring(ncqaDetails.Title.IndexOf("Documentation")).Substring(0, ncqaDetails.Title.Substring(ncqaDetails.Title.IndexOf("Documentation")).IndexOf("'"));

                        //Check the document locally
                        document = null;
                        document = lstDocuments.Find(delegate(Document doc) { return doc.AttachPath == location.Replace("/", "\\"); });

                        if (document == null)
                        {
                            log.DebugFormat("Submitting Document: license={0}, DocumentName={1}, PracticeId= {2}, ProjectId={3}, Location={4}", license, ncqaDetails.DocName, ncqaDetails.PracticeId, ncqaDetails.ProjectId, location);
                            FileInfo fileInfo = new FileInfo(@location);
                            document = AddDocument(client, license, ncqaDetails.DocName, fileInfo, log);

                            if (document != null)
                                lstDocuments.Add(document);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LinkDocumentToElements(DocumentServiceClient client, XDocument questionnaire, string license, Uri uri, log4net.ILog log, int projectId)
        {
            try
            {
                log.DebugFormat("--------------");
                log.DebugFormat("LinkDocumentToElements started at {0}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));

                foreach (Document document in lstDocuments)
                {
                    Ncqa.Iss.DocumentService.Interface.Element element;
                    Ncqa.Iss.DocumentService.Interface.NewElementLink newlink = new Ncqa.Iss.DocumentService.Interface.NewElementLink();
                    List<Ncqa.Iss.DocumentService.Interface.Element> lstElements = new List<Ncqa.Iss.DocumentService.Interface.Element>();

                    log.DebugFormat("Linking Document: license={0}, projectId={1}, DocumentName={2}, DocumentId={3}", license, projectId, document.Name, document.Id);

                    string documentRootDirectory = System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                    string location = document.AttachPath.Substring(document.AttachPath.IndexOf(documentRootDirectory));

                    location = EscapeXML(location);
                    location = location.Replace("\\", "/");

                    //Get all Elements having same location
                    log.DebugFormat("Get all elements Having location:{0}", location);

                    IEnumerable<XElement> docsLinkedTo = (from docRecord in questionnaire.Descendants("DocFile")
                                                          where docRecord.Attribute("location").ToString().Substring(docRecord.Attribute("location").ToString().IndexOf(documentRootDirectory), docRecord.Attribute("location").ToString().Length - docRecord.Attribute("location").ToString().IndexOf(documentRootDirectory) - 1) == location
                                                          select docRecord);

                    foreach (XElement docs in docsLinkedTo)
                    {
                        element = new Ncqa.Iss.DocumentService.Interface.Element();
                        element.Category = "PCMH";
                        element.Standard = Convert.ToInt32(docs.Parent.Parent.Parent.Parent.Attribute("sequence").Value);
                        element.Id = DEFAULT_LETTERS[Convert.ToInt32(docs.Parent.Parent.Parent.Attribute("sequence").Value) - 1];
                        element.ReferencePages = docs.Attribute("referencePages").Value;
                        element.Level = docs.Attribute("relevancyLevel").Value == "Primary" ? 1 : docs.Attribute("relevancyLevel").Value == "Secondary" ? 2 : docs.Attribute("relevancyLevel").Value == "Supporting" ? 3 : 0;

                        Element existingElement = lstElements.Find(delegate(Element elem) { return elem.Standard == element.Standard && elem.Id == element.Id; });

                        if (existingElement == null)
                        {
                            log.DebugFormat("Linking Element:  license={0}, projectId={1}, Category={2}, ID={3}, Standard={4}, ReferencePages={5}, Level={6}", license, projectId, element.Category, element.Id, element.Standard, element.ReferencePages, element.Level);
                            lstElements.Add(element);
                        }
                    }

                    newlink.DocumentId = document.Id;
                    newlink.Elements = lstElements;
                    client.LinkDocumentToElements(license, newlink);
                }

                log.DebugFormat("LinkDocumentToElements ended at {0}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Document AddDocument(DocumentServiceClient client, string license, string name, FileInfo contents, log4net.ILog log, NewLink initialLink = null)
        {
            try
            {

                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                else if (name.Trim().Length == 0)
                {
                    throw new ArgumentException("String argument 'name' cannot be empty.", "name");
                }

                if (contents.Exists)
                {
                    name = name.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "}").Replace("dotsign", ".");
                    var newDocument = new NewDocument()
                    {
                        AttachPath = contents.FullName,
                        Extension = contents.Extension,
                        InitialLink = initialLink,
                        Name = name,
                        OriginalFileName = contents.Name,
                    };
                    using (var stream = contents.OpenRead())
                    {
                        log.DebugFormat("Adding Document: DocumentName={0}", name);
                        var createdDocument = client.AddDocument(license, newDocument);

                        try
                        {
                            log.DebugFormat("Set Document Contents: DocumentId={0}", createdDocument.Id.ToString());
                            client.SetDocumentContents(license, createdDocument.Id.ToString(), stream);
                            return createdDocument;
                        }
                        catch (Exception ex)
                        {
                            log.DebugFormat("--------------");
                            log.Error("Error: An error ocurred while uploading " + name + ", " + ex.Message);
                            log.DebugFormat("--------------");
                            client.DeleteDocument(license, createdDocument.Id.ToString());
                            return null;
                        }
                    }
                }
                else
                {
                    log.ErrorFormat("ERROR: The specified file {0} does not exist.", contents.FullName);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddNotesDocument(string location, string license, string user, string password, log4net.ILog log)
        {
            try
            {
                string apikey = ConfigurationManager.AppSettings["APIKey"].ToString();
                var client = new Ncqa.Iss.DocumentService.Interface.DocumentServiceClient("ncqa-public-staging", apikey, user, password);


                //Get all Documents
                log.DebugFormat("Get all documents by license: license={0}", license);
                List<Document> existingDocuments = client.GetDocuments(license).ToList();


                Document document = null;
                //Delete the notes report if already Exists.
                document = existingDocuments.Find(delegate(Document doc) { return doc.AttachPath == location.Replace("/", "\\"); });

                if (document != null)
                {
                    //Delete the document.                            
                    client.DeleteDocument(license, document.Id.ToString());
                    existingDocuments.Remove(document);
                }


                if (document == null)
                {
                    FileInfo fileInfo = new FileInfo(@location);
                    document = AddDocument(client, license, fileInfo.Name, fileInfo, log);

                    if (document != null)
                        lstDocuments.Add(document);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteExistingDocumentsAndLinks(DocumentServiceClient client, string license, log4net.ILog log)
        {
            //Get all Links
            log.DebugFormat("Get all links by license: license={0}", license);
            List<Link> existingLinks = client.GetLinks(license).ToList();

            //Delete All Existing Links
            log.DebugFormat("Links deletion started against license: license={0}", license);

            foreach (Link existinglink in existingLinks)
            {
                log.DebugFormat("LinksId={0}, license={1}", existinglink.Id.ToString(), license);
                client.DeleteLink(license, existinglink.Id.ToString());
            }

            log.DebugFormat("Links deletion ended");


            //Get all Documents
            log.DebugFormat("Get all documents by license: license={0}", license);
            List<Document> existingDocuments = client.GetDocuments(license).ToList();

            //Delete All Existing Documents
            log.DebugFormat("Douments deletion started against license: license={0}", license);

            foreach (Document existingDocument in existingDocuments)
            {
                log.DebugFormat("DocumentId={0}, license={1}", existingDocument.Id.ToString(), license);
                client.DeleteDocument(license, existingDocument.Id.ToString());
            }

            log.DebugFormat("Douments deletion ended");
        }

        public string EscapeXML(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string returnString = input;
            returnString = returnString.Replace("'", "&apos;");
            returnString = returnString.Replace("\"", "&quot;");
            returnString = returnString.Replace(">", "&gt;");
            returnString = returnString.Replace("<", "&lt;");
            returnString = returnString.Replace("&", "&amp;");
            return returnString;
        }

    }
}
