using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using BMTBLL;
using BMTBLL.Enumeration;
using System.Web.SessionState;

/// <summary>
/// Summary description for FileHandler
/// </summary>
public class FileHandler : IHttpHandler, IRequiresSessionState
{

    #region CONSTANTS
    private const string DEFAULT_SITE_ROOT_DIRECTORY = "NCQA Documentation";
    private const string DEFAULT_SITE_DOCS_DIRECTORY = "My Documents";
    private const string DEFAULT_NODE_NCQA = "NCQA Submission";
    private const string DEFAULT_TOOL_MYDocuments = "My Documents";
    private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

    #endregion

    #region VARIABLES
    QuestionBO question;
    int QuestionnaireId;
    int UserId;
    #endregion

    #region METHODS
    public void ProcessRequest(HttpContext context)
    {
        /*Local Variable*/

        #region Local Varaibles
        string StrFileName;
        string DocName;
        string PracticeId;
        string PracticeName;
        string SiteName;
        string Node;
        int SiteId;
        string ElementId;
        string FactorId;
        string PCMHId;
        string ReferencePage;
        string RelevancyLevel;
        string DocsType;
        int ProjectId;
        string ExistingFile;
        string DocLinkedTo;
        string SavingDestinationPath;
        string DestinationPath;
        string FileTitle;
        string CurrentFileName;

        #endregion

        context.Response.ContentType = "text/plain";

        // Replace File
        if (context.Request.Form["elementId"] != null
          && context.Request.Form["factorId"] != null
          && context.Request.Form["PCMHId"] != null
          && context.Request.Form["ReferencePage"] != null
          && context.Request.Form["docType"] != null
          && context.Request.Form["PracticeName"] != null
          && context.Request.Form["SiteName"] != null
          && context.Request.Form["ProjectId"] != null
          && context.Request.Form["PracticeId"] != null
          && context.Request.Form["DocName"] != null
          && context.Request.Form["RelevancyLevel"] != null
          && context.Request.Form["ExistingFile"] != null
          && context.Request.Form["DocLinkedTo"] != null)
        {
            DocName = context.Request.Form["DocName"];
            PracticeId = context.Request.Form["PracticeId"];
            PracticeName = context.Request.Form["PracticeName"];
            ElementId = context.Request.Form["elementId"];
            FactorId = context.Request.Form["factorId"];
            PCMHId = context.Request.Form["PCMHId"];
            ReferencePage = context.Request.Form["ReferencePage"];
            RelevancyLevel = context.Request.Form["RelevancyLevel"];
            DocsType = context.Request.Form["docType"];
            
            SiteName = context.Request.Form["SiteName"];
            Node = context.Request.Form["Node"];
            ProjectId = Convert.ToInt32(context.Request.Form["ProjectId"]);
            SiteId = Convert.ToInt32(context.Request.Form["SiteId"]);
            ExistingFile = context.Request.Form["ExistingFile"];
            DocLinkedTo = context.Request.Form["DocLinkedTo"];

            DocsType = DocsType.Replace("/", "Or");
            DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
            DocsType = documentTypeMappingBO.GetOriginalDocumentType(DocsType);

            #region CREATE_DIRECTORY
            /*Read Root Direct path with name from web.config*/
            string VirtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
            string DocumentRootDirectory = DestinationPath = context.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();

            /*Extract URL path*/
            Uri uri = HttpContext.Current.Request.Url;
            String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
            if (VirtualDirectory.Length > 1)
            {
                VirtualDirectory += "/";
            }

            SavingDestinationPath = host + VirtualDirectory + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
            /*Create Root Directory if not exists*/
            if (!Directory.Exists(DocumentRootDirectory))
            { Directory.CreateDirectory(DocumentRootDirectory); }

            /*Directory for project*/
            string DocumentProjectDirectory = DestinationPath = DocumentRootDirectory + "\\" + PracticeId;
            SavingDestinationPath += "\\" + PracticeId;
            if (!Directory.Exists(DocumentProjectDirectory) && PracticeName.Trim() != string.Empty)
            { Directory.CreateDirectory(DocumentProjectDirectory); }
            DestinationPath = DocumentProjectDirectory;

            /*Root Driectory for Site*/
            string DocumentRootSiteDirectory = DestinationPath = DocumentProjectDirectory + "\\" + DEFAULT_SITE_ROOT_DIRECTORY;
            SavingDestinationPath += "\\" + DEFAULT_SITE_ROOT_DIRECTORY;
            if (!Directory.Exists(DocumentRootSiteDirectory))
            { Directory.CreateDirectory(DocumentRootSiteDirectory); }
            DestinationPath = DocumentRootSiteDirectory;

            /*Directory for Site*/
            string DocumentSiteDirectory = DestinationPath = DocumentRootSiteDirectory + "\\" + SiteId;
            SavingDestinationPath += "\\" + SiteId;
            if (!Directory.Exists(DocumentSiteDirectory))
            { Directory.CreateDirectory(DocumentSiteDirectory); }
            DestinationPath = DocumentSiteDirectory;

            if (Convert.ToInt32(ElementId) > 0)
            {
                /*Directory for NODE*/
                string DocumentToolDirectory = DestinationPath = DocumentSiteDirectory + "\\" + Node;
                SavingDestinationPath += "\\" + Node;
                if (!Directory.Exists(DocumentToolDirectory) && Node.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentToolDirectory); }

                /*DIRECTORY FOR TAB*/
                string Tab = Convert.ToInt32(PCMHId) > 0 ? "PCMH " + PCMHId : string.Empty;
                string DocumentTabDirectory = DestinationPath = DocumentToolDirectory + "\\" + Tab;
                SavingDestinationPath += "\\" + Tab;
                if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTabDirectory); }

                /*DIRECTORY FOR Element*/

                string Element = "Element " + DEFAULT_LETTERS[Convert.ToInt32(ElementId) - 1];
                string DocumentElementDirectory = DestinationPath = DocumentTabDirectory + "\\" + Element;
                SavingDestinationPath += "\\" + Element;
                if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(ElementId) > 0)
                { Directory.CreateDirectory(DocumentElementDirectory); }

                /*DIRECTORY FOR Factor*/
                string Factor = "Factor  " + FactorId;
                string DocumentFactorDirectory = DestinationPath = DocumentElementDirectory + "\\" + Factor;
                SavingDestinationPath += "\\" + Factor;
                if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(FactorId) > 0)
                { Directory.CreateDirectory(DocumentFactorDirectory); }


                /*DIRECTORY FOR selected Doc Type*/
                DocsType = DocsType.Trim() != string.Empty ? DocsType : enDocType.Policies.ToString();
                string DocumentTypeDirectory = DestinationPath = DocumentFactorDirectory + "\\" + DocsType;
                SavingDestinationPath += "\\" + DocsType;
                if (!Directory.Exists(DocumentTypeDirectory) && DocsType.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTypeDirectory); }
                DestinationPath = DocumentTypeDirectory;

            }
            else
            {
                /*Directory for Site Docs*/
                string SitedocumentDirectory = DestinationPath = DocumentSiteDirectory + "\\" + DEFAULT_SITE_DOCS_DIRECTORY;
                SavingDestinationPath += "\\" + DEFAULT_SITE_DOCS_DIRECTORY;
                if (!Directory.Exists(SitedocumentDirectory))
                { Directory.CreateDirectory(SitedocumentDirectory); }
            }
            #endregion


            #region MIMETYPE

            string stringResponse = string.Empty;
            string stringMIMEType = string.Empty;

            try
            {
                /*Allowed file types*/
                string strExtension = Path.GetExtension(context.Request.Files[0].FileName).ToLower();
                switch (strExtension)
                {
                    case ".gif":
                        stringMIMEType = "image/gif";
                        break;

                    case ".jpg":
                        stringMIMEType = "image/jpeg";
                        break;

                    case ".png":
                        stringMIMEType = "image/png";
                        break;

                    case ".txt":
                        stringMIMEType = "Text";
                        break;

                    case ".rtf":
                        stringMIMEType = "Rich-Text-Format";
                        break;

                    case ".vsd":
                        stringMIMEType = "application/msvisio";
                        break;

                    case ".doc":
                        stringMIMEType = "application/msword";
                        break;

                    case ".docx":
                        stringMIMEType = "application/vnd.openxmlformats-officedocument.wordprocessingml.documentofficedocument.wordprocessingml.document";
                        break;

                    case ".xls":
                        stringMIMEType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        stringMIMEType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case ".ppt":
                        stringMIMEType = "application/vnd.ms-powerpoint";
                        break;

                    case ".pptx":
                        stringMIMEType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;

                    case ".pdf":
                        stringMIMEType = "Adobe/pdf";
                        break;

                    case ".ppsx":
                        stringMIMEType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                        break;

                    case ".XLSM":
                        stringMIMEType = "Excel Macro-Enabled Workbook";
                        break;

                    case ".xlsm":
                        stringMIMEType = "Excel Macro-Enabled Workbook";
                        break;

                    default:
                        stringMIMEType = string.Empty;
                        stringResponse = "errorFileExt";
                        context.Response.Write(stringResponse);
                        return;
                }

            #endregion

                #region UPLOAD_FILE

                /*Site File Name Title*/
               /* StrFileName = context.Request.Files[0].FileName;*/
                StrFileName = Path.GetFileName(context.Request.Files[0].FileName);
                string FileName = string.Empty;
                ReferencePage = ReferencePage.Trim() != string.Empty ? ReferencePage : "All";


                FileTitle = StrFileName;
                FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + CleanFileName(StrFileName.Split('\\').Last());


                CurrentFileName = FileName;
                HttpPostedFile file = context.Request.Files[0];
                DestinationPath = DestinationPath + "\\" + FileName;
                SavingDestinationPath += "\\" + FileName;
                file.SaveAs(DestinationPath);
                ReplaceNCQADocs(context,ProjectId,SavingDestinationPath,PCMHId,ElementId,FactorId,FileTitle,DocLinkedTo,ExistingFile,DestinationPath);
                stringResponse = "success";
                context.Response.Write(stringResponse);
                return;

                #endregion

            }
            catch (Exception _exception)
            {
                context.Response.Write(_exception);
                return;
            }


        }

        /*To get NCQA Information*/
        else if (context.Request.Form["elementId"] != null
          && context.Request.Form["factorId"] != null
          && context.Request.Form["PCMHId"] != null
          && context.Request.Form["ReferencePage"] != null
          && context.Request.Form["docType"] != null
          && context.Request.Form["PracticeName"] != null
          && context.Request.Form["SiteName"] != null
          && context.Request.Form["ProjectId"] != null
          && context.Request.Form["PracticeId"] != null
          && context.Request.Form["DocName"] != null
          && context.Request.Form["RelevancyLevel"] != null)
        {
            DocName = context.Request.Form["DocName"];
            PracticeId = context.Request.Form["PracticeId"];
            PracticeName = context.Request.Form["PracticeName"];
            ElementId = context.Request.Form["elementId"];
            FactorId = context.Request.Form["factorId"];
            PCMHId = context.Request.Form["PCMHId"];
            ReferencePage = context.Request.Form["ReferencePage"];
            RelevancyLevel = context.Request.Form["RelevancyLevel"];
            DocsType = context.Request.Form["docType"];
            SiteName = context.Request.Form["SiteName"];
            Node = context.Request.Form["Node"];
            ProjectId = Convert.ToInt32(context.Request.Form["ProjectId"]);
            SiteId = Convert.ToInt32(context.Request.Form["SiteId"]);

            //Get Original Document Type

            DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
            DocsType = documentTypeMappingBO.GetOriginalDocumentType(DocsType);

            #region CREATE_DIRECTORY
            /*Read Root Direct path with name from web.config*/
            string VirtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
            string DocumentRootDirectory = DestinationPath = context.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();

            /*Extract URL path*/
            Uri uri = HttpContext.Current.Request.Url;
            String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
            if (VirtualDirectory.Length > 1)
            {
                VirtualDirectory += "/";
            }

            SavingDestinationPath = host + VirtualDirectory + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
            /*Create Root Directory if not exists*/
            if (!Directory.Exists(DocumentRootDirectory))
            { Directory.CreateDirectory(DocumentRootDirectory); }

            /*Directory for project*/
            string DocumentProjectDirectory = DestinationPath = DocumentRootDirectory + "\\" + PracticeId;
            SavingDestinationPath += "\\" + PracticeId;
            if (!Directory.Exists(DocumentProjectDirectory) && PracticeName.Trim() != string.Empty)
            { Directory.CreateDirectory(DocumentProjectDirectory); }
            DestinationPath = DocumentProjectDirectory;

            /*Root Driectory for Site*/
            string DocumentRootSiteDirectory = DestinationPath = DocumentProjectDirectory + "\\" + DEFAULT_SITE_ROOT_DIRECTORY;
            SavingDestinationPath += "\\" + DEFAULT_SITE_ROOT_DIRECTORY;
            if (!Directory.Exists(DocumentRootSiteDirectory))
            { Directory.CreateDirectory(DocumentRootSiteDirectory); }
            DestinationPath = DocumentRootSiteDirectory;

            /*Directory for Site*/
            string DocumentSiteDirectory = DestinationPath = DocumentRootSiteDirectory + "\\" + SiteId;
            SavingDestinationPath += "\\" + SiteId;
            if (!Directory.Exists(DocumentSiteDirectory))
            { Directory.CreateDirectory(DocumentSiteDirectory); }
            DestinationPath = DocumentSiteDirectory;

            if (Convert.ToInt32(ElementId) > 0)
            {
                /*Directory for NODE*/
                string DocumentToolDirectory = DestinationPath = DocumentSiteDirectory + "\\" + Node;
                SavingDestinationPath += "\\" + Node;
                if (!Directory.Exists(DocumentToolDirectory) && Node.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentToolDirectory); }

                /*DIRECTORY FOR TAB*/
                string Tab = Convert.ToInt32(PCMHId) > 0 ? "PCMH " + PCMHId : string.Empty;
                string DocumentTabDirectory = DestinationPath = DocumentToolDirectory + "\\" + Tab;
                SavingDestinationPath += "\\" + Tab;
                if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTabDirectory); }

                /*DIRECTORY FOR Element*/

                string Element = "Element " + DEFAULT_LETTERS[Convert.ToInt32(ElementId) - 1];
                string DocumentElementDirectory = DestinationPath = DocumentTabDirectory + "\\" + Element;
                SavingDestinationPath += "\\" + Element;
                if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(ElementId) > 0)
                { Directory.CreateDirectory(DocumentElementDirectory); }

                /*DIRECTORY FOR Factor*/
                string Factor = "Factor  " + FactorId;
                string DocumentFactorDirectory = DestinationPath = DocumentElementDirectory + "\\" + Factor;
                SavingDestinationPath += "\\" + Factor;
                if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(FactorId) > 0)
                { Directory.CreateDirectory(DocumentFactorDirectory); }


                /*DIRECTORY FOR selected Doc Type*/
                DocsType = DocsType.Trim() != string.Empty ? DocsType : enDocType.Policies.ToString();
                string DocumentTypeDirectory = DestinationPath = DocumentFactorDirectory + "\\" + DocsType;
                SavingDestinationPath += "\\" + DocsType;
                if (!Directory.Exists(DocumentTypeDirectory) && DocsType.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTypeDirectory); }
                DestinationPath = DocumentTypeDirectory;

            }
            else
            {
                /*Directory for Site Docs*/
                string SitedocumentDirectory = DestinationPath = DocumentSiteDirectory + "\\" + DEFAULT_SITE_DOCS_DIRECTORY;
                SavingDestinationPath += "\\" + DEFAULT_SITE_DOCS_DIRECTORY;
                if (!Directory.Exists(SitedocumentDirectory))
                { Directory.CreateDirectory(SitedocumentDirectory); }
            }
            #endregion

            #region MIMETYPE

            string strResponse = "error";
            string strMIMEType = string.Empty;

            try
            {
                /*Allowed file types*/
                string strExtension = Path.GetExtension(context.Request.Files[0].FileName).ToLower();
                switch (strExtension)
                {
                    case ".gif":
                        strMIMEType = "image/gif";
                        break;

                    case ".jpg":
                        strMIMEType = "image/jpeg";
                        break;

                    case ".png":
                        strMIMEType = "image/png";
                        break;

                    case ".txt":
                        strMIMEType = "Text";
                        break;

                    case ".rtf":
                        strMIMEType = "Rich-Text-Format";
                        break;

                    case ".vsd":
                        strMIMEType = "application/msvisio";
                        break;

                    case ".doc":
                        strMIMEType = "application/msword";
                        break;

                    case ".docx":
                        strMIMEType = "application/vnd.openxmlformats-officedocument.wordprocessingml.documentofficedocument.wordprocessingml.document";
                        break;

                    case ".xls":
                        strMIMEType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        strMIMEType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case ".ppt":
                        strMIMEType = "application/vnd.ms-powerpoint";
                        break;

                    case ".pptx":
                        strMIMEType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;

                    case ".pdf":
                        strMIMEType = "Adobe/pdf";
                        break;

                    case ".ppsx":
                        strMIMEType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                        break;

                    case ".XLSM":
                        strMIMEType = "Excel Macro-Enabled Workbook";
                        break;

                    case ".xlsm":
                        strMIMEType = "Excel Macro-Enabled Workbook";
                        break;

                    default:
                        strMIMEType = string.Empty;
                        return;
                }

            #endregion

                #region UPLOAD_FILE

                /*Site File Name Title*/
               /* StrFileName = context.Request.Files[0].FileName;*/
                StrFileName = Path.GetFileName(context.Request.Files[0].FileName);
                string FileName = string.Empty;
                ReferencePage = ReferencePage.Trim() != string.Empty ? ReferencePage : "All";


                if (StrFileName.Contains(":"))
                {
                    FileTitle = "MY uploaded Doc" + strExtension;
                    StrFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "MY uploaded Doc" + strExtension;
                }
                if (DocName.Trim() != string.Empty)
                {
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + CleanFileName(DocName.Trim()) + strExtension;
                    FileTitle = DocName.Trim();
                }
                else
                {
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + CleanFileName(StrFileName);
                    FileTitle = StrFileName;
                }


                HttpPostedFile file = context.Request.Files[0];
                DestinationPath = DestinationPath + "\\" + FileName;
                SavingDestinationPath += "\\" + FileName;
                file.SaveAs(DestinationPath);
                UpdateNCQADocs(context,ProjectId,SavingDestinationPath,FileTitle,PCMHId,ElementId,FactorId,DocsType,ReferencePage,RelevancyLevel);
                strResponse = "success";
                #endregion

            }
            catch
            {

            }
            context.Response.Write(strResponse);

            return;
        }
        else { return; }

        if (ProjectId < 1 || PracticeName.Trim() == string.Empty || SiteName.Trim() == string.Empty)
        { return; }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void UpdateNCQADocs(HttpContext context, int ProjectId, string SavingDestinationPath, string FileTitle, string PCMHId, string ElementId, string FactorId, string DocsType, string ReferencePage, string RelevancyLevel)
    {

        try
        {

            string StoredQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
            XDocument StoreDocument = XDocument.Parse(StoredQuestionnaire);
            XElement StoreElement = StoreDocument.Root;
            SavingDestinationPath = SavingDestinationPath.Replace("\\", "/");
            FileTitle = FileTitle.Replace("'", "Apostrophe").Replace("^", "circumflex").Replace("+", "plussign").Replace("#", "hashsign").Replace("[", "squarebraketopen").Replace("]", "squarebraketclose").Replace("{", "curlybraketopen").Replace("}", "curlybraketclose").Replace(".","dotsign");

            string elementType = StoreElement.Name.ToString();
            if (Convert.ToInt32(ElementId) > 0)
            {
                if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
                {
                    IEnumerable<XElement> Standard = from element in StoreElement.Descendants("Standard")
                                                     where (string)element.Attribute("sequence") == PCMHId
                                                     select element;

                    IEnumerable<XElement> Elements = from element in Standard.Descendants("Element")
                                                     where (string)element.Attribute("sequence") == ElementId
                                                     select element;
                    IEnumerable<XElement> Factors = from factor in Elements.Descendants("Factor")
                                                    select factor;


                    foreach (XElement factor in Factors)
                    {
                        if (factor.Attribute("sequence").Value == FactorId)
                        {
                            IEnumerable<XElement> documentType = from docType in factor.Descendants(DocsType)
                                                                 select docType;
                            if (documentType.Count() == 0)
                            {
                                XElement docFile = new XElement(DocsType, new XElement("DocFile",
                                                                            new XAttribute("name", FileTitle),
                                                                            new XAttribute("referencePages", ReferencePage),
                                                                            new XAttribute("relevancyLevel", RelevancyLevel),
                                                                            new XAttribute("location", SavingDestinationPath)),
                                                                 new XAttribute("required", ""));
                                factor.Add(docFile);
                            }
                            else
                            {
                                foreach (XElement docElement in factor.Elements(DocsType))
                                {

                                    /*Add Answer Element*/
                                    XElement docFile = new XElement("DocFile",
                                                            new XAttribute("name", FileTitle),
                                                            new XAttribute("referencePages", ReferencePage),
                                                            new XAttribute("relevancyLevel", RelevancyLevel),
                                                            new XAttribute("location", SavingDestinationPath)); ;
                                    docElement.Add(docFile);
                                    break;
                                }
                            }
                        }

                    }
                    /*Save the Questionnaire at the end*/
                }
            }
            else
            {
                if (elementType == enQuestionnaireType.DetailedQuestionnaire.ToString())
                {
                    IEnumerable<XElement> siteDocElement = from element in StoreElement.Descendants("SiteDocuments")
                                                           select element;
                    if (siteDocElement.Count() == 0)
                    {

                        XElement siteDoc = new XElement("SiteDocuments", new XElement("Docuemnt",
                                                                             new XAttribute("name", FileTitle),
                                                                             new XAttribute("referencePages", ReferencePage),
                                                                             new XAttribute("relevancyLevel", RelevancyLevel),
                                                                             new XAttribute("location", SavingDestinationPath)));
                        StoreElement.Add(siteDoc);
                    }
                    else
                    {
                        foreach (XElement siteElement in siteDocElement)
                        {
                            XElement siteDoc = new XElement("Docuemnt",
                                                               new XAttribute("name", FileTitle),
                                                               new XAttribute("referencePages", ReferencePage),
                                                               new XAttribute("relevancyLevel", RelevancyLevel),
                                                               new XAttribute("location", SavingDestinationPath));
                            siteElement.Add(siteDoc);

                        }
                    }

                }
            }

            string FinalizedDoc = Convert.ToString(StoreDocument.Root);
            question = new QuestionBO();
            UserId = Convert.ToInt32(HttpContext.Current.Session["UserApplicationId"]);
            QuestionnaireId = Convert.ToInt32(HttpContext.Current.Session["NCQAQuestionnaireId"]);

            if (ProjectId > 0 && UserId > 0)
            {
                question.SaveFilledQuestionnaire(QuestionnaireId, ProjectId, StoreDocument.Root, UserId);
            }
            HttpContext.Current.Session["NCQAQuestionnaire"] = StoreDocument.Root.ToString();
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }


    protected void ReplaceNCQADocs(HttpContext context,int ProjectId, string SavingDestinationPath, string PCMHId, string ElementId, string FactorId,string FileTitle, string DocLinkedTo, string ExistingFile, string DestinationPath)
    {
        if (HttpContext.Current.Session["NCQAQuestionnaire"].ToString() != null)
        {

            string RecievedQuestionnaire = HttpContext.Current.Session["NCQAQuestionnaire"].ToString();
            XDocument Questionnaire = XDocument.Parse(RecievedQuestionnaire);
            SavingDestinationPath = SavingDestinationPath.Replace("\\", "/");

            string filePath = string.Empty;

            //Find UnAssociated Documents
            IEnumerable<XElement> unAssociatedDocs = from unAssociatedDoc in Questionnaire.Descendants("UnAssociatedDoc")
                                                     select unAssociatedDoc;
            // Standards = PCMH
            // Find standard by sequence No
            IEnumerable<XElement> standards = from standard in Questionnaire.Descendants("Standard")
                                              where (string)standard.Attribute("sequence").Value == PCMHId
                                              select standard;

            // Find element by sequence No
            IEnumerable<XElement> elements = from element in standards.Descendants("Element")
                                             where (string)element.Attribute("sequence").Value == ElementId
                                             select element;
            // Find factor by sequence No
            IEnumerable<XElement> factors = from factor in elements.Descendants("Factor")
                                            where (string)factor.Attribute("sequence").Value == FactorId
                                            select factor;

            // Find file by file name from NCQA Submission Only
            IEnumerable<XElement> docs = from doc in factors.Descendants("DocFile")
                                         where (string)doc.Attribute("location").Value.Substring(doc.Attribute("location").Value.LastIndexOf('/') + 1) == ExistingFile
                                         select doc;


            // if file deleted from Document Viewer popUp
            if (DocLinkedTo != string.Empty)
            {
                string[] docReferences = DocLinkedTo.Split(',');
                int index = 0;
                string pcmhSequence = string.Empty;
                string elementSequence = string.Empty;
                string factorSequence = string.Empty;

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
                           where (string)doc.Attribute("location").Value.Substring(doc.Attribute("location").Value.LastIndexOf('/') + 1) == ExistingFile
                            && (string)doc.Parent.Parent.Attribute("sequence") == factorSequence
                            && (string)doc.Parent.Parent.Parent.Attribute("sequence") == elementSequence
                            && (string)doc.Parent.Parent.Parent.Parent.Attribute("sequence") == pcmhSequence
                           select doc;

                    foreach (XElement doc in docs)
                    {
                        string existingDirectory = DestinationPath.Substring(0, DestinationPath.LastIndexOf('\\') + 1) + ExistingFile;
                        if (File.Exists(existingDirectory))
                            File.Delete(existingDirectory);
                        
                        doc.Attribute("location").Value = SavingDestinationPath;
                        doc.Attribute("name").Value = FileTitle.Replace("'", "Apostrophe").Replace("^", "circumflex").Replace("plussign", "+").Replace("#", "hashsign").Replace("[", "squarebraketopen").Replace("]", "squarebraketclose").Replace("{", "curlybraketopen").Replace("}", "curlybraketclose").Replace(".","dotsign"); 
                        //doc.Attribute("name").Value = CurrentFileName.Substring(14, CurrentFileName.Length - 14);

                    }
                }

                question = new QuestionBO();
                UserId = Convert.ToInt32(HttpContext.Current.Session["UserApplicationId"]);
                QuestionnaireId = Convert.ToInt32(HttpContext.Current.Session["NCQAQuestionnaireId"]);

                if (ProjectId > 0 && UserId > 0)
                {
                    question.SaveFilledQuestionnaire(QuestionnaireId, ProjectId, Questionnaire.Root, UserId);
                }
                HttpContext.Current.Session["NCQAQuestionnaire"] = Questionnaire.Root.ToString();
            }
        }
    }


    public string CleanFileName(string fileName)
    {
        string fileExtension = "";
        if ((fileName.LastIndexOf(".")) != -1)
        {
            fileExtension = fileName.Substring(fileName.LastIndexOf("."));
            fileName = fileName.Replace(fileName.Substring(fileName.LastIndexOf(".")), "");
        }
        string[] charArray = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|", "'", "&", "#", "%", "+", "^", "[", "]", "{", "}","."};

        try 
        {
            foreach (string character in charArray)
            {
                fileName = fileName.Replace(character, string.Empty);
            }
            if (fileExtension != "")
            { fileName = fileName + fileExtension;}
            return fileName;
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    #endregion
}
