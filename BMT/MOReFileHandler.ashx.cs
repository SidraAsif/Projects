using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BMT.WEB;
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
public class MOReFileHandler : IHttpHandler, IRequiresSessionState
{

    #region CONSTANTS
    //private const string DEFAULT_SITE_ROOT_DIRECTORY = "NCQA Documentation";
    private const string DEFAULT_SITE_DOCS_DIRECTORY = "My Documents";
    private const string DEFAULT_NODE_MORE = "MORe Submission";
    private const string DEFAULT_TOOL_MYDocuments = "My Documents";
    private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

    #endregion

    #region VARIABLES
    MOReBO MORe = new MOReBO();
    #endregion

    #region METHODS
    public void ProcessRequest(HttpContext context)
    {
        /*local varaibles*/
        #region local variables
        string FileTitle;
        string StrFileName;
        string DocName;
        string PracticeId;
        string PracticeName;
        string PCMHId;
        string ElementId;
        string FactorId;
        string ReferencePage;
        string RelevancyLevel;
        string DocsType;
        string SiteName;
        string Node;
        int ProjectUsageId;
        int SiteId;
        string ExistingFile;
        string DocLinkedTo;
        string CurrentFileName;
        string NewFileName;
        string DestinationPath;
        string SavingDestinationPath;
        int TemplateId;
        string TemplateName;
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
          && context.Request.Form["ProjectUsageId"] != null
          && context.Request.Form["PracticeId"] != null
          && context.Request.Form["DocName"] != null
          && context.Request.Form["RelevancyLevel"] != null
          && context.Request.Form["ExistingFile"] != null
          && context.Request.Form["DocLinkedTo"] != null
          && context.Request.Form["templateId"] != null)
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
            TemplateId = Convert.ToInt32(context.Request.Form["templateId"]);
            TemplateName = MORe.GetTemplateName(TemplateId);
            NewFileName = context.Request.Form["filename"];
            SiteName = context.Request.Form["SiteName"];
            Node = context.Request.Form["Node"];
            ProjectUsageId = Convert.ToInt32(context.Request.Form["ProjectUsageId"]);
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
            string DocumentRootSiteDirectory = DestinationPath = DocumentProjectDirectory + "\\" + TemplateName;
            SavingDestinationPath += "\\" + TemplateName;
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
                string DocumentToolDirectory = DestinationPath = DocumentSiteDirectory + "\\" + DEFAULT_NODE_MORE;
                SavingDestinationPath += "\\" + DEFAULT_NODE_MORE;
                if (!Directory.Exists(DocumentToolDirectory))
                { Directory.CreateDirectory(DocumentToolDirectory); }

                /*DIRECTORY FOR TAB*/
                string Tab = Convert.ToInt32(PCMHId) > 0 ? "Header " + PCMHId : string.Empty;
                string DocumentTabDirectory = DestinationPath = DocumentToolDirectory + "\\" + Tab;
                SavingDestinationPath += "\\" + Tab;
                if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTabDirectory); }

                /*DIRECTORY FOR Element*/

                string Element = "SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(ElementId) - 1];
                string DocumentElementDirectory = DestinationPath = DocumentTabDirectory + "\\" + Element;
                SavingDestinationPath += "\\" + Element;
                if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(ElementId) > 0)
                { Directory.CreateDirectory(DocumentElementDirectory); }

                /*DIRECTORY FOR Factor*/
                string Factor = "Question  " + FactorId;
                string DocumentFactorDirectory = DestinationPath = DocumentElementDirectory + "\\" + Factor;
                SavingDestinationPath += "\\" + Factor;
                if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(FactorId) > 0)
                { Directory.CreateDirectory(DocumentFactorDirectory); }


                /*DIRECTORY FOR selected Doc Type*/
                DocsType = DocsType.Trim() != string.Empty ? DocsType :enDocType.Policies.ToString();
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
                /* strFileName = Path.GetFileName(context.Request.Files[0].FileName);*/

                /*Site File Name Title*/
                /*StrFileName = context.Request.Files[0].FileName;*/
                StrFileName = Path.GetFileName(context.Request.Files[0].FileName);
                string FileName = string.Empty;
                ReferencePage = ReferencePage.Trim() != string.Empty ? ReferencePage : "All";


                FileTitle = StrFileName;
                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + CleanFileName(StrFileName.Split('\\').Last());


                CurrentFileName = FileName;
                HttpPostedFile file = context.Request.Files[0];
                DestinationPath = DestinationPath + "\\" + FileName;
                SavingDestinationPath += "\\" + FileName;
                file.SaveAs(DestinationPath);
                ReplaceNCQADocs(context,TemplateId,ProjectUsageId,SiteId,SavingDestinationPath,DocsType,ExistingFile,NewFileName);
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
          && context.Request.Form["ProjectUsageId"] != null
          && context.Request.Form["PracticeId"] != null
          && context.Request.Form["DocName"] != null
          && context.Request.Form["RelevancyLevel"] != null
          && context.Request.Form["templateId"] != null)
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
            ProjectUsageId = Convert.ToInt32(context.Request.Form["ProjectUsageId"]);
            SiteId = Convert.ToInt32(context.Request.Form["SiteId"]);
            TemplateId = Convert.ToInt32(context.Request.Form["templateId"]);
            TemplateName = MORe.GetTemplateName(TemplateId);
            NewFileName = context.Request.Form["filename"];
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
            string DocumentRootSiteDirectory = DestinationPath = DocumentProjectDirectory + "\\" + TemplateName;
            SavingDestinationPath += "\\" + TemplateName;
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
                string DocumentToolDirectory = DestinationPath = DocumentSiteDirectory + "\\" + DEFAULT_NODE_MORE;
                SavingDestinationPath += "\\" + DEFAULT_NODE_MORE;
                if (!Directory.Exists(DocumentToolDirectory))
                { Directory.CreateDirectory(DocumentToolDirectory); }

                /*DIRECTORY FOR TAB*/
                string Tab = Convert.ToInt32(PCMHId) > 0 ? "Header " + PCMHId : string.Empty;
                string DocumentTabDirectory = DestinationPath = DocumentToolDirectory + "\\" + Tab;
                SavingDestinationPath += "\\" + Tab;
                if (!Directory.Exists(DocumentTabDirectory) && Tab.Trim() != string.Empty)
                { Directory.CreateDirectory(DocumentTabDirectory); }

                /*DIRECTORY FOR Element*/

                string Element = "SubHeader " + DEFAULT_LETTERS[Convert.ToInt32(ElementId) - 1];
                string DocumentElementDirectory = DestinationPath = DocumentTabDirectory + "\\" + Element;
                SavingDestinationPath += "\\" + Element;
                if (!Directory.Exists(DocumentElementDirectory) && Convert.ToInt32(ElementId) > 0)
                { Directory.CreateDirectory(DocumentElementDirectory); }

                /*DIRECTORY FOR Factor*/
                string Factor = "Question  " + FactorId;
                string DocumentFactorDirectory = DestinationPath = DocumentElementDirectory + "\\" + Factor;
                SavingDestinationPath += "\\" + Factor;
                if (!Directory.Exists(DocumentFactorDirectory) && Convert.ToInt32(FactorId) > 0)
                { Directory.CreateDirectory(DocumentFactorDirectory); }


                /*DIRECTORY FOR selected Doc Type*/
                DocsType = DocsType.Trim() != string.Empty ? DocsType : enDocType.Policies.ToString() ;

                //DocsType = DocsType.Replace("/", "Or");
                //DocumentTypeMappingBO documentTypeMapping = new DocumentTypeMappingBO();
                //DocsType = documentTypeMapping.GetOriginalDocumentType(DocsType);

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
                /* strFileName = Path.GetFileName(context.Request.Files[0].FileName);*/

                /*Site File Name Title*/
               /* StrFileName = context.Request.Files[0].FileName;*/
                StrFileName = Path.GetFileName(context.Request.Files[0].FileName);
                string FileName = string.Empty;
                ReferencePage = ReferencePage.Trim() != string.Empty ? ReferencePage : "All";


                if (StrFileName.Contains(":"))
                {
                    FileTitle = "MY uploaded Doc" + strExtension;
                    StrFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "MY uploaded Doc" + strExtension;
                }
                if (DocName.Trim() != string.Empty)
                {
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + CleanFileName(DocName.Trim()) + strExtension;
                    FileTitle = DocName.Trim();
                }
                else
                {
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + CleanFileName(StrFileName);
                    FileTitle = StrFileName;
                }


                HttpPostedFile file = context.Request.Files[0];
                DestinationPath = DestinationPath + "\\" + FileName;
                SavingDestinationPath += "\\" + FileName;
                file.SaveAs(DestinationPath);
                UpdateNCQADocs(context,TemplateId,ProjectUsageId,SiteId,SavingDestinationPath,FileTitle,ReferencePage, RelevancyLevel,DocsType,PCMHId,ElementId,FactorId);
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

        //if (ProjectId < 1 || PracticeName.Trim() == string.Empty || SiteName.Trim() == string.Empty)
        //{ return; }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void UpdateNCQADocs(HttpContext context,int TemplateId, int ProjectUsageId,int siteId,string SavingDestinationPath, string FileTitle, string ReferencePage, string RelevancyLevel, string DocsType, string PCMHId, string ElementId, string FactorId)
    {

        try
        {

            SavingDestinationPath = SavingDestinationPath.Replace("\\", "/");
            MOReBO MORe = new MOReBO();
            MORe.SaveTemplateDocument(TemplateId, SavingDestinationPath, FileTitle, ReferencePage, RelevancyLevel, DocsType, Convert.ToInt32(FactorId),
                Convert.ToInt32(ElementId), Convert.ToInt32(PCMHId),ProjectUsageId,siteId);

        }
        catch (Exception exception)
        {

            throw exception;
        }
    }


    protected void ReplaceNCQADocs(HttpContext context, int TemplateId,int ProjectUsageId,int siteId,string SavingDestinationPath, string DocsType, string ExistingFile, string NewFileName)
    {
        SavingDestinationPath = SavingDestinationPath.Replace("\\", "/");

        string filePath = string.Empty;

        //int QuestionId = MORe.GetQuestionIdBySequence(Convert.ToInt32(PCMHId), Convert.ToInt32(ElementId), Convert.ToInt32(FactorId), TemplateId);

        string OldPath = MORe.GetDocumentPath(TemplateId, DocsType, ProjectUsageId,siteId, ExistingFile);

        MORe.UpdateTemplateDocument(TemplateId, SavingDestinationPath, ProjectUsageId,siteId, ExistingFile, NewFileName);

        OldPath = Util.GetPathAndQueryByURL(OldPath);
        OldPath = Util.ExtractDocPath(OldPath);

        OldPath = OldPath.Replace("\\", "/");

        OldPath = OldPath.Replace("/", @"\");

        File.Delete(OldPath);

    }


    public string CleanFileName(string fileName)
    {
        string fileExtension = "";
        if ((fileName.LastIndexOf(".")) != -1)
        {
            fileExtension = fileName.Substring(fileName.LastIndexOf("."));
            fileName = fileName.Replace(fileName.Substring(fileName.LastIndexOf(".")), "");
        }
        string[] charArray = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|", "'", "&", "#", "%", "+", "^", "[", "]", "{", "}", "." };

        try
        {
            foreach (string character in charArray)
            {
                fileName = fileName.Replace(character, string.Empty);
            }
            if (fileExtension != "")
            { fileName = fileName + fileExtension; }
            return fileName;
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    #endregion
}
