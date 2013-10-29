using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Xml.Linq;
using System.Xml;
using System.IO;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT.WebServices
{
    /// <summary>
    /// Summary description for NCQAService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class NCQAService : System.Web.Services.WebService
    {
        #region CONSTANTS
        private const string DEFAULT_SITE_ROOT_DIRECTORY = "NCQA Documentation";
        private const string DEFAULT_SITE_DOCS_DIRECTORY = "My Documents";
        private const string DEFAULT_NODE_NCQA = "NCQA Submission";
        private const string DEFAULT_MORE_NODE = "MORe Submission";
        private const string DEFAULT_TOOL_MYDocuments = "My Documents";
        private const string DEFAULT_UNASSOCIATED_DOCUMENT_SEQUENCE = "100";

        private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        private const string DEFAULT_REFERENCE_PAGE = "All";

        private const int DEFAULT_LEVEL1_STARTPOINT = 35;
        private const int DEFAULT_LEVEL1_ENDPOINT = 59;
        private const int DEFAULT_LEVEL2_STARTPOINT = 60;
        private const int DEFAULT_LEVEL2_ENDPOINT = 84;
        private const int DEFAULT_LEVEL3_STARTPOINT = 85;
        private const int DEFAULT_LEVEL3_ENDPOINT = 100;

        // PCMH scoring level
        private const string DEFAULT_LEVEL1_TEXT = " (Level 1)";
        private const string DEFAULT_LEVEL2_TEXT = " (Level 2)";
        private const string DEFAULT_LEVEL3_TEXT = " (Level 3)";

        #endregion

        #region VARIABLES
        private XDocument storeDocument;
        private string elementSequence;
        private string standardName;
        private string elementName;
        private string CorpElementName;
        private string pcmhSequence;
        private string factorSequence;
        private string file;
        private string location;
        private string title;
        private XDocument questionnaire;
        private string filePath;
        CorporateElementSubmissionBO corporateElementSubmissionBO;
        MOReBO MORe = new MOReBO();
        #endregion

        #region FUNCTIONS

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAStandards()
        {

            List<NCQADetails> _ncqaStandarList = new List<NCQADetails>();
            try
            {
                string recievedQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
                XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                                                  select standardsRecord;
                foreach (XElement standard in standards)
                {
                    if (standard.Descendants("DocFile").Count() > 0)
                        _ncqaStandarList.Add(new NCQADetails(standard.Attribute("sequence").Value.ToString(), "PCMH" + " " + standard.Attribute("sequence").Value.ToString()));
                }

                if (questionnaire.Descendants("UnAssociatedDoc").Descendants("DocFile").Count() > 0)
                    _ncqaStandarList.Add(new NCQADetails(DEFAULT_UNASSOCIATED_DOCUMENT_SEQUENCE, "UNASSOCIATED DOCUMENTS"));
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaStandarList;

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetHeaders()
        {
            try
            {
                MOReBO MORe = new MOReBO();
                string templateId = HttpContext.Current.Session["TemplateId"].ToString();
                string projectUsageId = HttpContext.Current.Session["ProjectUsageId"].ToString();
                string siteId = HttpContext.Current.Session["SiteId"].ToString();

                return MORe.GetHeadersForDDLlist(Convert.ToInt32(templateId), Convert.ToInt32(projectUsageId),Convert.ToInt32(siteId));

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAElements(string sequence)
        {

            List<NCQADetails> _ncqaElementList = new List<NCQADetails>();
            try
            {
                string recievedQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
                XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                                                  where (string)standardsRecord.Attribute("sequence").Value.ToString() == sequence
                                                  select standardsRecord;

                foreach (XElement element in standards.Elements("Element"))
                {
                    if (element.Descendants("DocFile").Count() > 0)
                        _ncqaElementList.Add(new NCQADetails(element.Attribute("sequence").Value.ToString(), "Element " + DEFAULT_LETTERS[Convert.ToInt32(element.Attribute("sequence").Value) - 1].ToString()));
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaElementList;

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetSubHeaders(string sequence)
        {
            try
            {
                MOReBO MORe = new MOReBO();
                string templateId = HttpContext.Current.Session["TemplateId"].ToString();
                string projectUsageId = HttpContext.Current.Session["ProjectUsageId"].ToString();
                string siteId = HttpContext.Current.Session["SiteId"].ToString();

                return MORe.GetSubHeadersForDDLlist(Convert.ToInt32(templateId), Convert.ToInt32(sequence), Convert.ToInt32(projectUsageId),Convert.ToInt32(siteId));
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAFactors(string standardSequence, string elementSequence, string currentFactor, string currentStandard,
            string currentElement)
        {

            List<NCQADetails> _ncqaFactorList = new List<NCQADetails>();
            try
            {
                string recievedQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
                XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                                                  where (string)standardsRecord.Attribute("sequence").Value.ToString() == standardSequence
                                                  select standardsRecord;

                IEnumerable<XElement> elements = from elementRecord in standards.Descendants("Element")
                                                 where (string)elementRecord.Attribute("sequence").Value.ToString() == elementSequence
                                                 select elementRecord;

                foreach (XElement factor in elements.Elements("Factor"))
                {
                    if (factor.Attribute("sequence").Value != currentFactor || standardSequence != currentStandard || elementSequence != currentElement)
                    {
                        if (factor.Descendants("DocFile").Count() > 0)
                            _ncqaFactorList.Add(new NCQADetails(factor.Attribute("sequence").Value.ToString(), "Factor"));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaFactorList;

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetQuestions(string standardSequence, string elementSequence, string currentFactor, string currentStandard,
            string currentElement)
        {
            try
            {
                string templateId = HttpContext.Current.Session["TemplateId"].ToString();
                string projectUsageId = HttpContext.Current.Session["ProjectUsageId"].ToString();
                string siteId = HttpContext.Current.Session["SiteId"].ToString();
                MOReBO MORe = new MOReBO();

                return MORe.GetQuestionsForDDLlist(standardSequence, elementSequence, currentFactor, currentStandard, currentElement, Convert.ToInt32(templateId),
                    Convert.ToInt32(projectUsageId),Convert.ToInt32(siteId));
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetDocs(string standardSequence, string elementSequence, string factorSequence)
        {

            List<NCQADetails> _ncqaDocList = new List<NCQADetails>();
            try
            {
                string recievedQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
                XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);


                if (standardSequence != DEFAULT_UNASSOCIATED_DOCUMENT_SEQUENCE)
                {
                    IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                                                      where (string)standardsRecord.Attribute("sequence").Value.ToString() == standardSequence
                                                      select standardsRecord;




                    IEnumerable<XElement> elements = from elementRecord in standards.Descendants("Element")
                                                     where (string)elementRecord.Attribute("sequence").Value.ToString() == elementSequence
                                                     select elementRecord;

                    IEnumerable<XElement> factors = from elementRecord in elements.Descendants("Factor")
                                                    where (string)elementRecord.Attribute("sequence").Value.ToString() == factorSequence
                                                    select elementRecord;

                    foreach (XElement doc in factors.Descendants("DocFile"))
                    {
                        string fileName = doc.Attribute("name").Value.ToString();

                        fileName = fileName.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "}").Replace("dotsign", ".");
                        _ncqaDocList.Add(new NCQADetails(factorSequence, fileName, doc.Attribute("location").Value.ToString()));
                    }
                }
                else
                {
                    foreach (XElement doc in questionnaire.Descendants("UnAssociatedDoc").Descendants("DocFile"))
                    {
                        string fileName = doc.Attribute("name").Value.ToString();

                        fileName = fileName.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "}").Replace("dotsign", ".");
                        _ncqaDocList.Add(new NCQADetails(factorSequence, fileName, doc.Attribute("location").Value.ToString()));
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);

            }

            return _ncqaDocList;

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetDocsForMORe(string standardSequence, string elementSequence, string factorSequence)
        {

            try
            {

                string templateId = HttpContext.Current.Session["TemplateId"].ToString();
                string projectUsageId = HttpContext.Current.Session["ProjectUsageId"].ToString();
                string siteId = HttpContext.Current.Session["SiteId"].ToString();
                MOReBO MORe = new MOReBO();

                return MORe.GetDocumentList(standardSequence, elementSequence, factorSequence, Convert.ToInt32(templateId), Convert.ToInt32(projectUsageId),Convert.ToInt32(siteId));


            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public bool DocLinkGenerator(string docName, string elementId, string factorId, string pcmhId,
            string referencePage, string relevancyLevel, string docType, string practiceName, string siteName,
            string node, string projectId, string siteId, string practiceId, string location, string fileTitle, string selectedStandard)
        {
            bool response = false;

            try
            {
                //Get Original Document Type

                DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                docType = documentTypeMappingBO.GetOriginalDocumentType(docType);

                fileTitle = docName.Trim() != string.Empty ? docName : fileTitle;
                referencePage = referencePage.Trim() != string.Empty ? referencePage : DEFAULT_REFERENCE_PAGE;

                // Get and store questionnaire 
                GetQuestionnaire();

                // update Questionnaire
                bool _isQuestionnaireUpdated = CreateFileLink(location, pcmhId, elementId, factorId, docType, fileTitle, referencePage,
                    relevancyLevel, projectId, selectedStandard, practiceName, siteName, node, siteId, practiceId);

                // update doc info in database and session
                if (_isQuestionnaireUpdated)
                    response = UpdateDB(Convert.ToInt32(projectId));

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return response;

        }

        [WebMethod(EnableSession = true)]
        public bool DocLinkGeneratorForMORe(string docName, string elementId, string factorId, string pcmhId,
            string referencePage, string relevancyLevel, string docType, string practiceName, string siteName,
            string node, string projectUsageId, string siteId, string practiceId, string location, string fileTitle, string selectedStandard)
        {
            bool response = false;

            try
            {

                fileTitle = docName.Trim() != string.Empty ? docName : fileTitle;
                referencePage = referencePage.Trim() != string.Empty ? referencePage : DEFAULT_REFERENCE_PAGE;

                bool _isUpdated = false;
                // update Database
                _isUpdated = CreateFileLinkForMORe(location, pcmhId, elementId, factorId, docType, fileTitle, referencePage,
                    relevancyLevel, projectUsageId, selectedStandard, practiceName, siteName, node, siteId, practiceId);

                return _isUpdated;

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return response;

        }

        [WebMethod(EnableSession = true)]
        private void GetQuestionnaire()
        {
            // fetch current questionnaire from session
            string storedQuestionnaire = Session["NCQAQuestionnaire"].ToString();

            // parse questionnaire into XDocument
            storeDocument = XDocument.Parse(storedQuestionnaire);
        }

        [WebMethod(EnableSession = true)]
        private bool CreateFileLink(string savingDestinationPath, string pcmhId, string elementId, string factorId, string docType,
                        string fileTitle, string referencePage, string relevancyLevel, string projectId, string selectedStandard,
                        string practiceName, string siteName, string node, string siteId, string practiceId)
        {


            // get root of the document
            XElement storeElement = storeDocument.Root;

            // replace all double slashes in given path
            savingDestinationPath = savingDestinationPath.Replace("\\", "/");

            string elementType = storeElement.Name.ToString();

            if (selectedStandard != DEFAULT_UNASSOCIATED_DOCUMENT_SEQUENCE)
            {
                if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
                {
                    IEnumerable<XElement> standards = from element in storeElement.Descendants("Standard")
                                                      where (string)element.Attribute("sequence") == pcmhId
                                                      select element;

                    IEnumerable<XElement> elements = from element in standards.Descendants("Element")
                                                     where (string)element.Attribute("sequence") == elementId
                                                     select element;

                    IEnumerable<XElement> factors = from factor in elements.Descendants("Factor")
                                                    where (string)factor.Attribute("sequence") == factorId
                                                    select factor;

                    IEnumerable<XElement> factorsDoc = from doc in factors.Descendants("DocFile")
                                                       where (string)doc.Attribute("location") == savingDestinationPath
                                                       select doc;

                    if (factorsDoc.Count() == 0) // if selected file is not already linked with current factor
                    {
                        foreach (XElement factor in factors)
                        {
                            if (factor.Attribute("sequence").Value == factorId)
                            {
                                IEnumerable<XElement> documentType = from docTypeRecord in factor.Descendants(docType)
                                                                     select docTypeRecord;
                                if (documentType.Count() == 0)
                                {
                                    XElement docFile = new XElement(docType, new XElement("DocFile",
                                                                                new XAttribute("name", fileTitle),
                                                                                new XAttribute("referencePages", referencePage),
                                                                                new XAttribute("relevancyLevel", relevancyLevel),
                                                                                new XAttribute("location", savingDestinationPath)),
                                                                     new XAttribute("required", ""));
                                    factor.Add(docFile);
                                }
                                else
                                {
                                    foreach (XElement docElement in factor.Elements(docType))
                                    {

                                        // Add Answer Element
                                        XElement docFile = new XElement("DocFile",
                                                                new XAttribute("name", fileTitle),
                                                                new XAttribute("referencePages", referencePage),
                                                                new XAttribute("relevancyLevel", relevancyLevel),
                                                                new XAttribute("location", savingDestinationPath)); ;
                                        docElement.Add(docFile);
                                        break;
                                    }
                                }
                            }

                        }

                        return true;
                    }
                    else
                        return false;
                }
            }
            else
            {
                string unAssoicateFileLocation = string.Empty;
                string destinationPath = string.Empty;

                IEnumerable<XElement> docs = from doc in storeElement.Descendants("UnAssociatedDoc").Descendants("DocFile")
                                             where (string)doc.Attribute("location") == savingDestinationPath
                                             select doc;
                if (docs.Count() > 0)
                {
                    unAssoicateFileLocation = (from doc in storeElement.Descendants("UnAssociatedDoc").Descendants("DocFile")
                                               where (string)doc.Attribute("location") == savingDestinationPath
                                               select doc.Attribute("location").Value).FirstOrDefault();

                    unAssoicateFileLocation = Util.ExtractDocPath(unAssoicateFileLocation);

                    destinationPath = CreateDirectory(elementId, factorId, pcmhId, referencePage, relevancyLevel, docType, practiceName, siteName,
                        node, projectId, siteId, practiceId);

                    File.Move(unAssoicateFileLocation, destinationPath + "/" + unAssoicateFileLocation.Substring(unAssoicateFileLocation.LastIndexOf('/')));

                    destinationPath = destinationPath.Substring(destinationPath.IndexOf(Util.GetDocRootPath()));
                    destinationPath = Util.GetHostPath() + destinationPath + unAssoicateFileLocation.Substring(unAssoicateFileLocation.LastIndexOf('/'));

                    bool response = CreateFileLink(destinationPath, pcmhId, elementId, factorId, docType, fileTitle, referencePage, relevancyLevel, projectId,
                        pcmhId, practiceName, siteName, node, siteId, practiceId);

                    if (response)
                    {
                        docs.Remove();
                        return true;
                    }
                    else
                        return false;
                }

            }

            return true;
        }

        [WebMethod(EnableSession = true)]
        private bool CreateFileLinkForMORe(string savingDestinationPath, string pcmhId, string elementId, string factorId, string docType,
                        string fileTitle, string referencePage, string relevancyLevel, string projectUsageId, string selectedStandard,
                        string practiceName, string siteName, string node, string siteId, string practiceId)
        {


            // replace all double slashes in given path
            savingDestinationPath = savingDestinationPath.Replace("\\", "/");
            string templateId = HttpContext.Current.Session["TemplateId"].ToString();

            if (docType != enDocType.LogsOrTools.ToString() && docType != enDocType.OtherDocs.ToString() && docType != enDocType.Policies.ToString()
                && docType != enDocType.Reports.ToString() && docType != enDocType.Screenshots.ToString() && docType != enDocType.UnAssociatedDoc.ToString())
            {
                docType = docType.Replace("/", "Or");
                DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                docType = documentTypeMappingBO.GetOriginalDocumentType(docType);
            }

            MOReBO MORe = new MOReBO();

            if (selectedStandard != DEFAULT_UNASSOCIATED_DOCUMENT_SEQUENCE)
            {
                MORe.SaveLinkedDocument(savingDestinationPath, pcmhId, elementId, factorId, docType,
                             fileTitle, referencePage, relevancyLevel, projectUsageId, selectedStandard,
                             practiceName, siteName, node, siteId, practiceId, Convert.ToInt32(templateId));
            }

            else
            {
                string unAssoicateFileLocation = string.Empty;
                string destinationPath = string.Empty;

                TemplateDocument docs = MORe.GetDocumentByPath(savingDestinationPath);
                if (docs != null)
                {
                    unAssoicateFileLocation = savingDestinationPath;

                    unAssoicateFileLocation = Util.ExtractDocPath(unAssoicateFileLocation);

                    destinationPath = CreateDirectoryForMORe(elementId, factorId, pcmhId, referencePage, relevancyLevel, docType, practiceName, siteName,
                        node, projectUsageId, siteId, practiceId, Convert.ToInt32(templateId));

                    File.Move(unAssoicateFileLocation, destinationPath + "/" + unAssoicateFileLocation.Substring(unAssoicateFileLocation.LastIndexOf('/')));

                    destinationPath = destinationPath.Substring(destinationPath.IndexOf(Util.GetDocRootPath()));
                    destinationPath = Util.GetHostPath() + destinationPath + unAssoicateFileLocation.Substring(unAssoicateFileLocation.LastIndexOf('/'));

                    MORe.DeleteDocument(docs.DocumentId);

                    bool response = CreateFileLinkForMORe(destinationPath, pcmhId, elementId, factorId, docType, fileTitle, referencePage, relevancyLevel, projectUsageId,
                        pcmhId, practiceName, siteName, node, siteId, practiceId);

                    if (response)
                    {
                        return true;
                    }
                    else
                        return false;
                }

            }

            return true;
        }

        [WebMethod(EnableSession = true)]
        private bool UpdateDB(int projectId)
        {
            // update database if questionnaire updated successfully
            QuestionBO question;
            string finalizedDoc = Convert.ToString(storeDocument.Root);
            question = new QuestionBO();
            int userId = Convert.ToInt32(Session["UserApplicationId"]);
            int questionnaireId = Convert.ToInt32(HttpContext.Current.Session["NCQAQuestionnaireId"]);

            if (Convert.ToInt32(projectId) > 0 && userId > 0)
                question.SaveFilledQuestionnaire(questionnaireId, Convert.ToInt32(projectId), storeDocument.Root, userId);

            Session["NCQAQuestionnaire"] = storeDocument.Root.ToString();

            return true;

        }

        [WebMethod(EnableSession = true)]
        private string CreateDirectory(string elementId, string factorId, string pcmhId,
            string referencePage, string relevancyLevel, string docType, string practiceName, string siteName,
            string node, string projectId, string siteId, string practiceId)
        {
            string destinationPath = string.Empty;

            // read Root Direct path with name from web.config                
            string DocumentRootDirectory = destinationPath = Server.MapPath("~/") + Util.GetDocRootPath();

            // create Root Directory if not exists
            if (!Directory.Exists(DocumentRootDirectory))
                Directory.CreateDirectory(DocumentRootDirectory);

            // directory for project
            string DocumentProjectDirectory = destinationPath = DocumentRootDirectory + "\\" + practiceId;

            if (!Directory.Exists(DocumentProjectDirectory) && practiceName.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentProjectDirectory);

            destinationPath = DocumentProjectDirectory;

            // root Directory for site
            string DocumentRootSiteDirectory = destinationPath = DocumentProjectDirectory + "\\" + DEFAULT_SITE_ROOT_DIRECTORY;

            if (!Directory.Exists(DocumentRootSiteDirectory))
                Directory.CreateDirectory(DocumentRootSiteDirectory);

            destinationPath = DocumentRootSiteDirectory;

            // directory for site
            string DocumentSiteDirectory = destinationPath = DocumentRootSiteDirectory + "\\" + siteId;

            if (!Directory.Exists(DocumentSiteDirectory))
                Directory.CreateDirectory(DocumentSiteDirectory);

            destinationPath = DocumentSiteDirectory;

            // directory for node
            string DocumentToolDirectory = destinationPath = DocumentSiteDirectory + "\\" + node;

            if (!Directory.Exists(DocumentToolDirectory) && node.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentToolDirectory);

            // directory for TAB
            string Tab = Convert.ToInt32(pcmhId) > 0 ? "PCMH " + pcmhId : string.Empty;
            string DocumentTabDirectory = destinationPath = DocumentToolDirectory + "\\" + Tab;

            if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentTabDirectory);

            // directory for Element
            string Element = "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementId) - 1];
            string DocumentElementDirectory = destinationPath = DocumentTabDirectory + "\\" + Element;

            if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(elementId) > 0)
                Directory.CreateDirectory(DocumentElementDirectory);

            // directory for Factor
            string Factor = "Factor  " + factorId;
            string DocumentFactorDirectory = destinationPath = DocumentElementDirectory + "\\" + Factor;

            if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(factorId) > 0)
                Directory.CreateDirectory(DocumentFactorDirectory);

            // directory for selected Doc Type
            docType = docType.Trim() != string.Empty ? docType : enDocType.Policies.ToString();
            string DocumentTypeDirectory = destinationPath = DocumentFactorDirectory + "\\" + docType;

            if (!Directory.Exists(DocumentTypeDirectory) && docType.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentTypeDirectory);

            destinationPath = DocumentTypeDirectory;

            return destinationPath;


        }

        [WebMethod(EnableSession = true)]
        private string CreateDirectoryForMORe(string elementId, string factorId, string pcmhId,
            string referencePage, string relevancyLevel, string docType, string practiceName, string siteName,
            string node, string projectUsageId, string siteId, string practiceId, int TemplateId)
        {

            MOReBO more = new MOReBO();
            string templateName = more.GetTemplateName(TemplateId);

            string destinationPath = string.Empty;

            // read Root Direct path with name from web.config                
            string DocumentRootDirectory = destinationPath = Server.MapPath("~/") + Util.GetDocRootPath();

            // create Root Directory if not exists
            if (!Directory.Exists(DocumentRootDirectory))
                Directory.CreateDirectory(DocumentRootDirectory);

            // directory for project
            string DocumentProjectDirectory = destinationPath = DocumentRootDirectory + "\\" + practiceId;

            if (!Directory.Exists(DocumentProjectDirectory) && practiceName.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentProjectDirectory);

            destinationPath = DocumentProjectDirectory;

            // root Directory for site
            string DocumentRootSiteDirectory = destinationPath = DocumentProjectDirectory + "\\" + templateName;

            if (!Directory.Exists(DocumentRootSiteDirectory))
                Directory.CreateDirectory(DocumentRootSiteDirectory);

            destinationPath = DocumentRootSiteDirectory;

            // directory for site
            string DocumentSiteDirectory = destinationPath = DocumentRootSiteDirectory + "\\" + siteId;

            if (!Directory.Exists(DocumentSiteDirectory))
                Directory.CreateDirectory(DocumentSiteDirectory);

            destinationPath = DocumentSiteDirectory;

            // directory for node
            string DocumentToolDirectory = destinationPath = DocumentSiteDirectory + "\\" + DEFAULT_MORE_NODE;

            if (!Directory.Exists(DocumentToolDirectory))
                Directory.CreateDirectory(DocumentToolDirectory);

            // directory for TAB
            string Tab = Convert.ToInt32(pcmhId) > 0 ? "Header " + pcmhId : string.Empty;
            string DocumentTabDirectory = destinationPath = DocumentToolDirectory + "\\" + Tab;

            if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentTabDirectory);

            // directory for Element
            string Element = "SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(elementId) - 1];
            string DocumentElementDirectory = destinationPath = DocumentTabDirectory + "\\" + Element;

            if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(elementId) > 0)
                Directory.CreateDirectory(DocumentElementDirectory);

            // directory for Factor
            string Factor = "Question  " + factorId;
            string DocumentFactorDirectory = destinationPath = DocumentElementDirectory + "\\" + Factor;

            if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(factorId) > 0)
                Directory.CreateDirectory(DocumentFactorDirectory);

            // directory for selected Doc Type
            docType = docType.Trim() != string.Empty ? docType : enDocType.Policies.ToString();
            string DocumentTypeDirectory = destinationPath = DocumentFactorDirectory + "\\" + docType;

            if (!Directory.Exists(DocumentTypeDirectory) && docType.Trim() != string.Empty)
                Directory.CreateDirectory(DocumentTypeDirectory);

            destinationPath = DocumentTypeDirectory;

            return destinationPath;


        }

        [WebMethod(EnableSession = true)]
        public bool EditDoc(string pcmhId, string elementId, string factorId, string file, string docName, string referencePage, string relevancyLevel,
            string docType, int projectId)
        {

            try
            {
                Session["pcmhId"] = pcmhId;
                Session["elementId"] = elementId;

                //Get Original Document Type
                DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                docType = documentTypeMappingBO.GetOriginalDocumentType(docType);


                // get Questionnaire
                GetQuestionnaire();

                // Update or move Document
                IEnumerable<XElement> docs = from docRecord in storeDocument.Descendants("Factor").Descendants("DocFile")
                                             where (string)docRecord.Parent.Parent.Attribute("sequence") == factorId
                                                && (string)docRecord.Parent.Parent.Parent.Attribute("sequence") == elementId
                                                && (string)docRecord.Parent.Parent.Parent.Parent.Attribute("sequence") == pcmhId
                                                && docRecord.Attribute("location").Value.Contains(file)
                                             select docRecord;

                foreach (XElement document in docs)
                {
                    document.Attribute("name").Value = docName;
                    document.Attribute("referencePages").Value = referencePage;
                    document.Attribute("relevancyLevel").Value = relevancyLevel;

                    if (document.Parent.Name.ToString() != docType)
                    {
                        IEnumerable<XElement> factors = from docRecord in storeDocument.Descendants("Factor")
                                                        where (string)docRecord.Attribute("sequence") == factorId
                                                           && (string)docRecord.Parent.Attribute("sequence") == elementId
                                                           && (string)docRecord.Parent.Parent.Attribute("sequence") == pcmhId
                                                        select docRecord;

                        IEnumerable<XElement> documentType = from factorDocType in factors.Descendants(docType)
                                                             select factorDocType;
                        if (documentType.Count() != 0)
                        {
                            foreach (XElement newDocument in documentType)
                            {

                                XElement docFile = new XElement("DocFile",
                                                           new XAttribute("name", docName),
                                                           new XAttribute("referencePages", referencePage),
                                                           new XAttribute("relevancyLevel", relevancyLevel),
                                                           new XAttribute("location", document.Attribute("location").Value)); ;
                                newDocument.Add(docFile);
                                break;
                            }
                        }
                        else
                        {

                            foreach (XElement newDocument in factors)
                            {

                                XElement docFile = new XElement(docType, new XElement("DocFile",
                                                                            new XAttribute("name", docName),
                                                                            new XAttribute("referencePages", referencePage),
                                                                            new XAttribute("relevancyLevel", relevancyLevel),
                                                                            new XAttribute("location", document.Attribute("location").Value)),
                                                                 new XAttribute("required", ""));
                                newDocument.Add(docFile);
                                break;
                            }
                        }

                        document.Remove();
                    }
                    break;
                }

                // TODO : To rename the file name in PCMH -> standards -> elements -> Factors
                docs = from docRecord in storeDocument.Descendants("Factor").Descendants("DocFile")
                       where docRecord.Attribute("location").Value.Contains(file)
                       select docRecord;

                foreach (XElement document in docs)
                {
                    document.Attribute("name").Value = docName;
                }

                //update database
                UpdateDB(Convert.ToInt32(projectId));

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        public bool EditDocument(string pcmhId, string elementId, string factorId, string file, string docName, string referencePage, string relevancyLevel,
            string docType, int projectUsageId, int siteId, int templateId)
        {

            try
            {
                Session["pcmhId"] = pcmhId;
                Session["elementId"] = elementId;

                MOReBO MORe = new MOReBO();

                MORe.SaveTemplateDocumentByQuestionId(templateId, file, docName, referencePage, relevancyLevel,
                    docType, projectUsageId,siteId,factorId);

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        private Dictionary<string, string> CreateFileCopy(string pathAndQuery, string destinationPath, string savingDestinationPath, string docName)
        {
            Dictionary<string, string> fileValueDictionary = new Dictionary<string, string>();

            string fileTitle = string.Empty;
            string fileName = string.Empty;

            if (docName.Trim() == string.Empty)
            {
                // extract file name from file relative path
                fileName = pathAndQuery.Substring(pathAndQuery.LastIndexOf("/") + 1);

                // remove old timespan from filename
                fileName = fileTitle = fileName.Substring(14);

                // add new time span with file name
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;
            }
            else
            {
                string extension = Path.GetExtension(pathAndQuery);
                fileTitle = docName;
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + docName + extension;
            }

            // update destination path
            destinationPath += "\\" + fileName;

            // update saving path to store in database
            savingDestinationPath += "\\" + fileName;

            // Create new copy of existing file for current factor
            File.Copy(pathAndQuery, destinationPath);

            // add updated value into dictionary to make use it to store into db
            fileValueDictionary.Add("File Title", fileTitle);
            fileValueDictionary.Add("Saving Path", savingDestinationPath);

            return fileValueDictionary;
        }


        [WebMethod(EnableSession = true)]
        public void SaveNCQAStatus(string _element, string _status, string _projectId, string _level)
        {
            try
            {
                if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                {
                    string RecievedQuestionnaire = Session["NCQAQuestionnaire"].ToString();
                    XDocument questionnaire = XDocument.Parse(RecievedQuestionnaire);

                    if (Convert.ToBoolean(_status))
                    {
                        if (!questionnaire.Root.Elements("Status").Any())
                        {
                            XElement statusElement = new XElement("Status");
                            questionnaire.Root.Add(statusElement);
                        }

                        if (!questionnaire.Root.Elements("Status").Elements(_element).Any())
                        {
                            XElement Reviewed = new XElement(_element);
                            questionnaire.Root.Element("Status").Add(Reviewed);
                        }

                        questionnaire.Root.Elements("Status").Elements(_element).Attributes().Remove();

                        XAttribute status = new XAttribute("Status", _status.ToString());
                        questionnaire.Root.Element("Status").Element(_element).Add(status);

                        if (_element == "Submitted" || _element == "Recognized")
                        {
                            XAttribute date = new XAttribute("Date", DateTime.Now.ToString("MM-dd-yyyy"));
                            questionnaire.Root.Element("Status").Element(_element).Add(date);
                        }

                        if (_element == "Recognized")
                        {
                            XAttribute level = new XAttribute("Level", _level);
                            questionnaire.Root.Element("Status").Element(_element).Add(level);
                        }

                    }
                    else
                    {
                        if (questionnaire.Root.Elements("Status").Any())
                        {
                            if (questionnaire.Root.Elements("Status").Elements(_element).Any())
                            {
                                questionnaire.Root.Elements("Status").Elements(_element).Remove();
                            }
                        }
                    }

                    QuestionBO _questionBO = new QuestionBO();
                    _questionBO.SaveFilledQuestionnaire(Convert.ToInt32(Session["NCQAQuestionnaireId"]), Convert.ToInt32(_projectId), questionnaire.Root, Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));
                    Session[enSessionKey.NCQAQuestionnaire.ToString()] = questionnaire.Root;
                }

            }

            catch (Exception exception)
            {
                throw exception;
            }
        }


        [WebMethod(EnableSession = true)]
        public string ShowPassword(string _projectUsageId, string _practiceSiteId, DateTime _requestedOn, int _submissionType)
        {
            try
            {
                string password = string.Empty;

                if (_submissionType == (int)enSubmissionType.Old)
                {
                    NCQASubmissionBO ncqaSubmission = new NCQASubmissionBO();
                    password = ncqaSubmission.GetPasswordByProjectId(Convert.ToInt32(_projectUsageId), Convert.ToInt32(_practiceSiteId), _requestedOn);
                }
                else if (_submissionType == (int)enSubmissionType.New)
                {
                    NCQASubmissionMethod ncqaSubmissionMethod = new NCQASubmissionMethod();
                    password = ncqaSubmissionMethod.GetPasswordByProjectId(Convert.ToInt32(_practiceSiteId), _requestedOn);
                }

                return password;
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeletePreviousCorporateSubmissionList(int practiceId)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                corporateElementSubmissionBO.DeletePreviousCorporateSubmissionList(practiceId);
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool Save(int practiceId, string corporateElementName)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                return corporateElementSubmissionBO.Save(practiceId, corporateElementName);

            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public string[] CheckForEnableDisable(string CorpElementList, int practiceId)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                return corporateElementSubmissionBO.CheckForEnableDisable(CorpElementList, practiceId);

            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeleteSelectedElementFromXML(int practiceId)
        {
            try
            {
                corporateElementSubmissionBO = new CorporateElementSubmissionBO();

                List<int> PracSitesId = corporateElementSubmissionBO.GetPracticeSiteIdsByPracticeId(practiceId);

                PracticeBO _practice = new PracticeBO();

                foreach (int SiteId in PracSitesId)
                {
                    string recQuestionaire = corporateElementSubmissionBO.GetQuestionaireByPracticeAndSiteId(practiceId, SiteId);

                    int projectId = corporateElementSubmissionBO.GetProjectIdByPracticeSiteId(SiteId);
                    if (_practice.CheckCorporateType(SiteId))
                    {
                        if (recQuestionaire != "")
                        {
                            questionnaire = XDocument.Parse(recQuestionaire);
                            foreach (XElement standard in questionnaire.Root.Elements("Standard"))
                            {
                                pcmhSequence = standard.Attribute("sequence").Value.ToString();
                                foreach (XElement element in standard.Elements("Element"))
                                {
                                    elementSequence = element.Attribute("sequence").Value.ToString();
                                    standardName = standard.Attribute("title").Value.Substring(0, 6);
                                    elementName = element.Attribute("title").Value.Substring(7, 2);
                                    CorpElementName = standardName + elementName;

                                    if (corporateElementSubmissionBO.UnselectedCorpElement.Contains(CorpElementName))
                                    {
                                        string complete = element.Attribute("complete").Value;
                                        if (complete == "Yes")
                                        {
                                            element.Attribute("complete").Value = "No";
                                        }
                                        foreach (XElement factor in element.Elements("Factor"))
                                        {
                                            factorSequence = factor.Attribute("sequence").Value.ToString();

                                            string factAns = factor.Attribute("answer").Value;
                                            if (factAns == "Yes" || factAns == "NA")
                                            {
                                                factor.Attribute("answer").Value = "No";
                                            }
                                            foreach (XElement policies in factor.Elements("Policies"))
                                            {
                                                policies.Attribute("required").Value = "";

                                                foreach (XElement DocFiles in policies.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                policies.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement reports in factor.Elements("Reports"))
                                            {
                                                reports.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in reports.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                reports.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement screenshots in factor.Elements("Screenshots"))
                                            {
                                                screenshots.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in screenshots.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                screenshots.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement logs in factor.Elements("LogsOrTools"))
                                            {
                                                logs.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in logs.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                logs.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement others in factor.Elements("OtherDocs"))
                                            {
                                                others.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in others.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                others.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                            {
                                                privNotes.Remove();
                                            }
                                        }
                                        foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                        {
                                            revNotes.Value = "";
                                        }
                                        foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                        {
                                            evalNotes.Remove();
                                        }
                                        foreach (XElement defaultScore in element.Elements("Calculation"))
                                        {
                                            defaultScore.Attribute("defaultScore").Value = "0%";
                                        }
                                    }
                                }
                            }

                            string Ques = Convert.ToString(questionnaire);
                            corporateElementSubmissionBO.UpdateQuestionaire(Ques, SiteId);
                        }
                    }
                    else
                    {
                        if (recQuestionaire != "")
                        {
                            XDocument questionnaire = XDocument.Parse(recQuestionaire);

                            foreach (XElement standard in questionnaire.Root.Elements("Standard"))
                            {
                                pcmhSequence = standard.Attribute("sequence").Value.ToString();

                                foreach (XElement element in standard.Elements("Element"))
                                {
                                    elementSequence = element.Attribute("sequence").Value.ToString();
                                    standardName = standard.Attribute("title").Value.Substring(0, 6);
                                    elementName = element.Attribute("title").Value.Substring(7, 2);
                                    CorpElementName = standardName + elementName;
                                    if (corporateElementSubmissionBO.SelectedCorpElement.Contains(CorpElementName))
                                    {
                                        string complete = element.Attribute("complete").Value;
                                        if (complete == "Yes")
                                        {
                                            element.Attribute("complete").Value = "No";
                                        }
                                        foreach (XElement factor in element.Elements("Factor"))
                                        {
                                            factorSequence = factor.Attribute("sequence").Value.ToString();

                                            string factAns = factor.Attribute("answer").Value;
                                            if (factAns == "Yes" || factAns == "NA")
                                            {
                                                factor.Attribute("answer").Value = "No";
                                            }
                                            foreach (XElement policies in factor.Elements("Policies"))
                                            {
                                                policies.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in policies.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                policies.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement reports in factor.Elements("Reports"))
                                            {
                                                reports.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in reports.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                reports.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement screenshots in factor.Elements("Screenshots"))
                                            {
                                                screenshots.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in screenshots.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                screenshots.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement logs in factor.Elements("LogsOrTools"))
                                            {
                                                logs.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in logs.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                logs.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement others in factor.Elements("OtherDocs"))
                                            {
                                                others.Attribute("required").Value = "";
                                                foreach (XElement DocFiles in others.Elements("DocFile"))
                                                {
                                                    location = DocFiles.Attribute("location").Value;
                                                    location = location.Substring(location.LastIndexOf('/') + 1);
                                                    location = HttpUtility.JavaScriptStringEncode(location);
                                                    title = DocFiles.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                                                    file = location + "|" + title;
                                                    filePath = DocFiles.Attribute("location").Value;
                                                    FileHandler(pcmhSequence, elementSequence, factorSequence, file, filePath, projectId, practiceId, SiteId);
                                                }
                                                others.Descendants("DocFile").Remove();
                                            }
                                            foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                            {
                                                privNotes.Remove();
                                            }
                                        }
                                        foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                        {
                                            revNotes.Remove();
                                        }
                                        foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                        {
                                            evalNotes.Remove();
                                        }
                                        foreach (XElement privNotes in element.Elements("PrivateNote"))
                                        {
                                            privNotes.Remove();
                                        }
                                        foreach (XElement defaultScore in element.Elements("Calculation"))
                                        {
                                            defaultScore.Attribute("defaultScore").Value = "0%";
                                        }
                                    }
                                }

                            }

                            string Ques = Convert.ToString(questionnaire);
                            corporateElementSubmissionBO.UpdateQuestionaire(Ques, SiteId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckSiteForCorporate(int practiceSiteId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                return practice.CheckForCorporateSite(practiceSiteId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckForRemoveCorporateSite(int practiceSiteId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                return practice.CheckForRemoveCorporateSite(practiceSiteId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void ChangeCorporateSite(int practiceSiteId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                practice.ChangeCorporateSite(practiceSiteId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public void CopyToNonCorporateXML(int practiceSiteId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                practice.CopyToNonCorporateXML(practiceSiteId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckSiteForChangeCorporate(int practiceSiteId)
        {
            try
            {
                PracticeBO _practice = new PracticeBO();
                return _practice.CheckSiteForChangeCorporate(practiceSiteId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        [WebMethod(EnableSession = true)]
        public void FileHandler(string pcmhSequence, string elementSequence, string factorSequence,
                           string file, string filePath, int projectId, int practiceId, int siteId)
        {

            try
            {
                Session["pcmhId"] = pcmhSequence;
                Session["elementId"] = elementSequence;
                string[] fileName = file.Split('|');
                string filepath = string.Empty;

                // TODO: Create Local Disk path to move the selected file into unassociated docs folder
                filepath = Util.GetPathAndQueryByURL(filePath);
                filepath = Util.ExtractDocPath(filePath);

                // TODO:move file to unassociated Folder
                MoveFile(file, filepath, practiceId, siteId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void MoveFile(string file, string filePath, int practiceId, int siteId)
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

                // check if unassociated doc element exists
                foreach (XElement questionElement in questionnaire.Elements())
                {
                    IEnumerable<XElement> unAssociatedDocs = from unAssociatedDoc in questionnaire.Descendants("UnAssociatedDoc")
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
            catch (Exception)
            {
            }
        }

        [WebMethod(EnableSession = true)]
        public bool SaveMOReCorporateElement(int practiceId, string corporateElementTempId)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                return corporateElementSubmissionBO.SaveMOReCorpElement(practiceId, corporateElementTempId);

            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public string[] CheckForEnableDisableCorpMORe(string corpElementListIds, int practiceId)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                return corporateElementSubmissionBO.CheckForEnableDisableCorpMORe(corpElementListIds, practiceId);

            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeletePreviousKBCorporateElement(int practiceId)
        {
            try
            {
                CorporateElementSubmissionBO corporateElementSubmissionBO = new CorporateElementSubmissionBO();
                corporateElementSubmissionBO.DeletePreviousKBCorporateElement(practiceId);
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeleteSelectedElementFromKBTemp(int practiceId, int tempId)
        {
            try
            {
                corporateElementSubmissionBO = new CorporateElementSubmissionBO();

                List<int> PracSitesId = corporateElementSubmissionBO.GetPracticeSiteIdsByPracticeId(practiceId);

                PracticeBO _practice = new PracticeBO();

                foreach (int SiteId in PracSitesId)
                {
                    int projectId = corporateElementSubmissionBO.GetProjectIdByPracticeSiteId(SiteId);
                    if (_practice.CheckCorporateTypeMORe(SiteId))
                    {
                        List<FilledAnswer> filledAns = corporateElementSubmissionBO.GetElementFilledAnswerOfCorpSiteByProjectId(projectId);
                        if (filledAns != null)
                        {
                            foreach (FilledAnswer fillAns in filledAns)
                            {
                                corporateElementSubmissionBO.UpdateElementFilledAnswer(fillAns.FilledAnswersId);
                                corporateElementSubmissionBO.UpdateQuestionFilledAnswer(fillAns.KnowledgeBaseTemplateId, tempId, projectId);
                                List<TemplateDocument> QuestionDocument = corporateElementSubmissionBO.GetAllQuestionOfElement(fillAns.KnowledgeBaseTemplateId, projectId, tempId);
                                foreach (TemplateDocument tempDoc in QuestionDocument)
                                {
                                    string file = tempDoc.Path;
                                    MOReFileHandler(file, projectId, practiceId, SiteId, tempId);
                                }
                            }
                        }
                    }
                    else
                    {
                        List<FilledAnswer> filledAns = corporateElementSubmissionBO.GetElementFilledAnswerOfNonCorpSiteByProjectId(projectId);
                        if (filledAns != null)
                        {
                            foreach (FilledAnswer fillAns in filledAns)
                            {
                                corporateElementSubmissionBO.UpdateElementFilledAnswer(fillAns.FilledAnswersId);
                                corporateElementSubmissionBO.UpdateQuestionFilledAnswer(fillAns.KnowledgeBaseTemplateId, tempId, projectId);
                                List<TemplateDocument> QuestionDocument = corporateElementSubmissionBO.GetAllQuestionOfElement(fillAns.KnowledgeBaseTemplateId, projectId, tempId);
                                foreach (TemplateDocument tempDoc in QuestionDocument)
                                {
                                    string file = tempDoc.Path;
                                    MOReFileHandler(file, projectId, practiceId, SiteId, tempId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        protected void MOReFileHandler(string file, int projectId, int practiceId, int siteId, int templateId)
        {
            try
            {
                file = file.Replace("hashsign", "#");
                string[] fileName = file.Split('|');
                string filePath = string.Empty;

                // TODO: Create Local Disk path to move the selected file into unassociated docs folder
                filePath = Util.GetPathAndQueryByURL(fileName[0]);
                filePath = Util.ExtractDocPath(filePath);

                // TODO:moving file to unassociated Folder
                MOReMoveFile(file, filePath, practiceId, siteId, templateId, projectId);
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        [WebMethod(EnableSession = true)]
        private void MOReMoveFile(string file, string filePath, int practiceId, int siteId, int templateId, int projectId)
        {
            string UnAssociatedDocPath = string.Empty;
            string FileName = string.Empty;

            try
            {
                UnAssociatedDocPath = Util.GetUnAssociatedDocPath(practiceId, siteId);
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


                    MORe.DeleteFile(FileName, UnAssociatedDocPath, templateId, projectId,siteId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckForRemoveCorporateSiteInTemplate(int practiceSiteId,int templateId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                return practice.CheckForRemoveCorporateSiteInTemplate(practiceSiteId,templateId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckSiteForChangeCorporateMORe(int practiceSiteId,int templateId)
        {
            try
            {
                PracticeBO _practice = new PracticeBO();
                return _practice.CheckSiteForChangeCorporateMORe(practiceSiteId, templateId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckSiteForCorporateMORe(int practiceSiteId,int templateId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                return practice.CheckForCorporateSiteMORe(practiceSiteId, templateId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool UpdatePracticeTemplate(int templateId, bool isCorporate, int practiceSiteId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                return practice.UpdatePracticeTemplate(templateId, isCorporate, practiceSiteId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void ChangeCorporateSiteMORe(int practiceSiteId, int tempId)
        {
            try
            {
                corporateElementSubmissionBO = new CorporateElementSubmissionBO();

                int practiceId = corporateElementSubmissionBO.GetPracticeId(practiceSiteId);

                PracticeBO _practice = new PracticeBO();


                int projectId = corporateElementSubmissionBO.GetProjectIdByPracticeSiteId(practiceSiteId);

                List<FilledAnswer> filledAns = corporateElementSubmissionBO.GetCorporateElementFilledAnswerOfCorpSiteByProjectId(projectId);
                        if (filledAns != null)
                        {
                            foreach (FilledAnswer fillAns in filledAns)
                            {
                                corporateElementSubmissionBO.UpdateElementFilledAnswer(fillAns.FilledAnswersId);
                                corporateElementSubmissionBO.UpdateQuestionFilledAnswer(fillAns.KnowledgeBaseTemplateId, tempId, projectId);
                                List<TemplateDocument> QuestionDocument = corporateElementSubmissionBO.GetAllQuestionOfElement(fillAns.KnowledgeBaseTemplateId, projectId, tempId);
                                foreach (TemplateDocument tempDoc in QuestionDocument)
                                {
                                    string file = tempDoc.Path;
                                    MOReFileHandler(file, projectId, practiceId, practiceSiteId, tempId);
                                }
                            }
                        }
                    
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void SaveMOReStatus(string _element, string _status, string _projectUsageId, string siteId, string _level, string _templateId)
        {
            try
            {
                bool? reviewed = null;
                bool? submitted = null;
                bool? recognized = null;

                DateTime? submittedOn = null;
                DateTime? recognizedOn = null;
                string recognizedLevel = string.Empty;

                if (_element == "Reviewed")
                {
                    reviewed = Convert.ToBoolean(_status);
                }
                else if (_element == "Submitted")
                {
                    submitted = Convert.ToBoolean(_status);
                    submittedOn = DateTime.Now;
                }
                else if (_element == "Recognized")
                {
                    recognized = Convert.ToBoolean(_status);
                    recognizedOn = DateTime.Now;
                    recognizedLevel = _level;
                }

                new NCQASubmissionMethod().UpdatePracticeSiteSubmission(Convert.ToInt32(_projectUsageId),Convert.ToInt32(siteId),Convert.ToInt32(_templateId), reviewed, submitted, recognized,
                    submittedOn, recognizedOn, recognizedLevel);
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public void TemplateCopyToNonCorporateSite(int practiceSiteId,int templateId)
        {
            try
            {
                PracticeBO practice = new PracticeBO();
                practice.TemplateCopyToNonCorporateSite(practiceSiteId,templateId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

    }
}
