using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using BMTBLL.Enumeration;
using System.IO;

using log4net;
using log4net.Config;
using Ncqa.Iss.DocumentService.Interface;

namespace BMTBLL
{
    public class NCQASubmissionMethod : SubmissionMethod
    {
        private List<Document> lstDocuments;
        private static string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public override PracticeSiteSubmission GetPracticeSiteSubmissionByProjectIdAndTemplateId(int projectUsageId,int siteId, int templateId)
        {
            try
            {
                return base.GetPracticeSiteSubmissionByProjectIdAndTemplateId(projectUsageId,siteId, templateId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool IsSubmissionRequestExists(int projectUsageId,int siteId, int templateId)
        {
            try
            {
                return base.IsSubmissionRequestExists(projectUsageId,siteId, templateId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void UpdatePracticeSiteSubmission(int projectUsageId,int siteId, int templateId, bool? reviewed, bool? submitted, bool? recognized, DateTime? submittedOn, DateTime? recognizedOn,
            string recognizedLevel)
        {
            try
            {
                base.UpdatePracticeSiteSubmission(projectUsageId,siteId, templateId, reviewed, submitted, recognized, submittedOn, recognizedOn, recognizedLevel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertAPICredentials(int projectUsageId, int siteId, int templateId, XElement apiCredentials)
        {
            try
            {
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              where
                                              pracSiteSubmission.PracticeSiteId == siteId
                                              && pracSiteSubmission.TemplateId == templateId
                                              && (pracSiteSubmission.StatusId == null)
                                              select pracSiteSubmission).FirstOrDefault();

                if (practiceSiteSubmission == null)
                {
                    var existingSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              where
                                               pracSiteSubmission.PracticeSiteId == siteId
                                              && pracSiteSubmission.TemplateId == templateId
                                              && pracSiteSubmission.StatusId == (int)enSubmissionStatus.Fulfilled
                                              select pracSiteSubmission).FirstOrDefault();

                    practiceSiteSubmission = base.InsertPracticeSiteSubmission(templateId, projectUsageId,siteId, existingSubmission.Reviewed, existingSubmission.Submitted, existingSubmission.Recognized,
                        existingSubmission.SubmittedOn, existingSubmission.RecognizedOn, existingSubmission.RecognizedLevel);
                }

                if (practiceSiteSubmission != null)
                {
                    practiceSiteSubmission.APICredentials = apiCredentials;
                    practiceSiteSubmission.RequestedOn = DateTime.Now;
                    practiceSiteSubmission.StatusId = (int)enSubmissionStatus.Pending;
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPasswordByProjectId(int practiceSiteId, DateTime requestedOn)
        {
            try
            {
                string password = string.Empty;
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              where
                                                  //project.ProjectId == projectId && 
                                              pracSiteSubmission.PracticeSiteId == practiceSiteId
                                              && pracSiteSubmission.TemplateId == 1
                                              && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Year == requestedOn.Year
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Month == requestedOn.Month
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Day == requestedOn.Day
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Hour == requestedOn.Hour
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Minute == requestedOn.Minute
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Second == requestedOn.Second
                                              select pracSiteSubmission).FirstOrDefault();

                if (practiceSiteSubmission != null)
                {
                    Security security = new Security();
                    password = security.Decrypt(practiceSiteSubmission.APICredentials.Attribute("Password").Value);
                }

                return password;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateSubmissionStatus(int projectUsageId, int practiceSiteId, int statusId, int userId, DateTime requestedOn)
        {
            try
            {
                base.UpdateSubmissionStatus(practiceSiteId, statusId, userId, requestedOn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SubmitToNCQA(string license, string user, string password, int projectUsageId, int enterpriseId, string mapPath, Uri uri, string applicationPath,
            log4net.ILog log, int siteId, int practiceId, int templateId)
        {
            try
            {
                ProjectBO projectBO = new ProjectBO();
                string siteName = projectBO.GetSiteNameByProjectID(siteId);
                string apikey = ConfigurationManager.AppSettings["APIKey"].ToString();

                log.DebugFormat("--------------");
                log.DebugFormat("--------------");
                log.DebugFormat("Submission started at {0} for siteName={1}, license={2}, user={3}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now), siteName, license, user);


                var client = new Ncqa.Iss.DocumentService.Interface.DocumentServiceClient("ncqa-public-staging", apikey, user, password);

                // Delete all existing Document and links
                DeleteExistingDocumentsAndLinks(client, license, log);

                lstDocuments = new List<Document>();

                SubmitTemplateDocument(client, projectUsageId, siteId, templateId, practiceId, mapPath, uri, license, applicationPath, log);

                //Link Documents to elements
                LinkDocumentToElements(client, license, uri, log, projectUsageId, siteId, templateId);


                Email email = new Email();
                email.SendNCQASuccessful(siteId, license, enterpriseId, applicationPath);

                log.DebugFormat("Submission ended at {0}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ERROR: {0} ", ex.Message);
            }
        }

        public void SubmitTemplateDocument(DocumentServiceClient client, int projectUsageId, int practiceSiteId, int templateId, int practiceId, string mapPath, Uri uri, string license, string applicationPath,
            log4net.ILog log)
        {
            try
            {
                KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();
                List<KnowledgeBase> lstKnowledgeBaseHeader = knowledgeBaseBO.GetKnowledgeBaseByTypeAndTemplateId(enKBType.Header, templateId);

                foreach (KnowledgeBase header in lstKnowledgeBaseHeader)
                {
                    List<KnowledgeBase> lstKnowledgeBaseSubHeader = knowledgeBaseBO.GetKnowledgeBaseByParentId(header.KnowledgeBaseId, templateId);

                    foreach (KnowledgeBase subHeader in lstKnowledgeBaseSubHeader)
                    {
                        List<TemplateDocument> lstTemplateDocuments = knowledgeBaseBO.GetDocumentsByKnowledgeBaseIdAndProjectId(projectUsageId, practiceSiteId, subHeader.KnowledgeBaseId);
                        AddDocuments(client, lstTemplateDocuments, license, mapPath, log, practiceId, projectUsageId, practiceSiteId);

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteExistingDocumentsAndLinks(DocumentServiceClient client, string license, log4net.ILog log)
        {
            try
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
            catch (Exception ex)
            {
                log.ErrorFormat("ERROR: {0} ", ex.Message);
            }
        }

        public void AddDocuments(DocumentServiceClient client, List<TemplateDocument> lstTemplateDocument, string license, string mapPath, log4net.ILog log, int practiceId, int projectUsageId, int practiceSiteId)
        {
            try
            {
                Document document = null;

                foreach (TemplateDocument templateDocument in lstTemplateDocument)
                {

                    string location = mapPath + templateDocument.Path.Substring(templateDocument.Path.IndexOf("Documentation"));

                    //Check the document locally
                    document = null;
                    document = lstDocuments.Find(delegate(Document doc) { return doc.AttachPath == location.Replace("/", "\\"); });

                    if (document == null)
                    {
                        log.DebugFormat("Submitting Document: license={0}, DocumentName={1}, PracticeId= {2}, ProjectUsageId={3}, PracticeSiteId={4}, Location={5}", license, templateDocument.Name, practiceId, projectUsageId, practiceSiteId, location);
                        FileInfo fileInfo = new FileInfo(@location);
                        document = AddDocument(client, license, templateDocument.Name, fileInfo, log);

                        if (document != null)
                            lstDocuments.Add(document);
                    }

                }

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

        public void LinkDocumentToElements(DocumentServiceClient client, string license, Uri uri, log4net.ILog log, int projectUsageId, int practiceSiteId, int templateId)
        {
            try
            {
                log.DebugFormat("--------------");
                log.DebugFormat("LinkDocumentToElements started at {0}", String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now));

                KnowledgeBaseBO knowledgeBaseBO = new KnowledgeBaseBO();

                foreach (Document document in lstDocuments)
                {
                    Ncqa.Iss.DocumentService.Interface.Element element;
                    Ncqa.Iss.DocumentService.Interface.NewElementLink newlink = new Ncqa.Iss.DocumentService.Interface.NewElementLink();
                    List<Ncqa.Iss.DocumentService.Interface.Element> lstElements = new List<Ncqa.Iss.DocumentService.Interface.Element>();

                    log.DebugFormat("Linking Document: license={0}, projectUsageId={1}, practiceSiteId={2}, DocumentName={3}, DocumentId={4}", license, projectUsageId, practiceSiteId, document.Name, document.Id);

                    string documentRootDirectory = System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                    string location = document.AttachPath.Substring(document.AttachPath.IndexOf(documentRootDirectory));

                    location = EscapeXML(location);
                    location = location.Replace("\\", "/");

                    //Get all Elements having same location
                    log.DebugFormat("Get all elements Having location:{0}", location);

                    List<TemplateDocument> linkedToTemplateDocs = knowledgeBaseBO.GetLinkedToTemplateDocuments(projectUsageId, practiceSiteId, templateId, location, documentRootDirectory);

                    foreach (TemplateDocument templateDoc in linkedToTemplateDocs)
                    {
                        element = new Ncqa.Iss.DocumentService.Interface.Element();
                        element.Category = "PCMH";

                        KnowledgeBaseTemplate subHeaderKBTemplate = knowledgeBaseBO.GetSubHeaderByTemplateDocumentId(templateDoc.DocumentId);
                        KnowledgeBaseTemplate headerKBTemplate = knowledgeBaseBO.GetHeaderByParentKBTemplateId(Convert.ToInt32(subHeaderKBTemplate.KnowledgeBaseTemplateId));

                        element.Standard = Convert.ToInt32(headerKBTemplate.Sequence);
                        element.Id = DEFAULT_LETTERS[Convert.ToInt32(subHeaderKBTemplate.Sequence) - 1];

                        element.ReferencePages = templateDoc.ReferencePages;
                        element.Level = templateDoc.RelevencyLevel == "Primary" ? 1 : templateDoc.RelevencyLevel == "Secondary" ? 2 : templateDoc.RelevencyLevel == "Supporting" ? 3 : 0;

                        Element existingElement = lstElements.Find(delegate(Element elem) { return elem.Standard == element.Standard && elem.Id == element.Id; });

                        if (existingElement == null)
                        {
                            log.DebugFormat("Linking Element:  license={0}, projectId={1}, practiceSiteId={2},Category={3}, ID={4}, Standard={5}, ReferencePages={6}, Level={7}", license, projectUsageId, practiceSiteId, element.Category, element.Id, element.Standard, element.ReferencePages, element.Level);
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
