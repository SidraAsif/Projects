#region Modification History

//  ******************************************************************************
//  Module        : File Upload
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : File upload handler for project, toolbox and library
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig   (May-01-2012)     Remove extra spaces, uncommented code and some bugs
//  *******************************************************************************

#endregion
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
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for FileHandler
/// </summary>
public class GenericFileHandler : IHttpHandler, IRequiresSessionState
{

    //#region CONSTANTS
    //private const string DEFAULT_TOOL_NCQA = "NCQA Submission";
    //private const string DEFAULT_TOOL_MYDocuments = "My Documents";
    //private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    //,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

    //#endregion

    #region VARIABLES
    TreeBO tree = new TreeBO();
    FileBO file = new FileBO();
    ProjectBO project = new ProjectBO();
    #endregion

    #region METHODS

    public void ProcessRequest(HttpContext context)
    {
        /*Local Variables*/
        #region Local variables
        string strFileName;
        string destinationPath;
        string saveDirectory;
        string TableName;
        string Name;
        string Description;
        int SectionId;
        int PracticeId=0;
        #endregion

        /*To Receive the information*/
        /*
                
* 
* context.Request.QueryString["NameOfString"]
* context.Request.Form["NameofKey"]  hint: data{}
* context.Request.Files[0] /*To get the file
* 
* 
* ###################################*/
        context.Response.ContentType = "text/plain";

        /*To get Information*/
        if (context.Request.Form["TableName"] != null
          && context.Request.Form["Name"] != null
          && context.Request.Form["Description"] != null
          && context.Request.Form["SectionId"] != null)
        {

            TableName = context.Request.Form["TableName"];
            Name = context.Request.Form["Name"];
            Description = context.Request.Form["Description"];
            SectionId = Convert.ToInt32(context.Request.Form["SectionId"].ToString());
            if (context.Request.Form["PracticeId"] != "")
            {
                PracticeId = Convert.ToInt32(context.Request.Form["PracticeId"].ToString());
            }

        }
        else
            return;


        #region CREATE_DIRECTORY
        /*Read Root Direct path with name from web.config*/
        string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
        string documentRootDirectory = destinationPath = context.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
        string documentDirectory = "";

        /*Extract URL path*/
        Uri uri = HttpContext.Current.Request.Url;
        String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
        if (virtualDirectory.Length > 1)
        {
            virtualDirectory += "/";
        }

        saveDirectory = host + virtualDirectory + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
        /*Create Root Directory if not exists*/
        if (!Directory.Exists(documentRootDirectory))
        { Directory.CreateDirectory(documentRootDirectory); }

        if (TableName == enDbTables.ToolDocument.ToString())
        {
            if (PracticeId != 0)
            {

                /*Directory for Tool*/
                documentDirectory = destinationPath = documentRootDirectory + "//Tools//" + PracticeId;
                saveDirectory += "/Tools/" + PracticeId;
                if (!Directory.Exists(documentDirectory))
                { Directory.CreateDirectory(documentDirectory); }
                destinationPath = documentDirectory;

            }
            else
            {
                documentDirectory = destinationPath = documentRootDirectory + "//Tools//";
                saveDirectory += "/Tools/";
                if (!Directory.Exists(documentDirectory))
                { Directory.CreateDirectory(documentDirectory); }
            }
        }
        else if (TableName == enDbTables.LibraryDocument.ToString())
        {
            if (PracticeId != 0)
            {

                /*Directory for Library*/
                documentDirectory = destinationPath = documentRootDirectory + "//Library//" + PracticeId;
                saveDirectory += "/Library/" + PracticeId;
                if (!Directory.Exists(documentDirectory))
                { Directory.CreateDirectory(documentDirectory); }
                destinationPath = documentDirectory;

            }
            else
            {
                documentDirectory = destinationPath = documentRootDirectory + "//Library//";
                saveDirectory += "/Library/";
                if (!Directory.Exists(documentDirectory))
                { Directory.CreateDirectory(documentDirectory); }
            }

        }

        else if (TableName == enDbTables.ProjectDocument.ToString())
        {
            if (PracticeId != 0)
            {
                string practiceName = project.GetPracticeNameByPracticeID(PracticeId);
                /*Directory for Tool*/
                documentDirectory = destinationPath = documentRootDirectory + "/" + PracticeId + "/My Documents";
                saveDirectory += "/" + PracticeId + "/My Documents";
                if (!Directory.Exists(documentDirectory))
                { Directory.CreateDirectory(documentDirectory); }
                destinationPath = documentDirectory;

            }
        }

        #endregion

        #region MIMETYPE

        string strResponse = string.Empty;
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
                    strResponse = "errorFileExt";
                    context.Response.Write(strResponse);
                    return;
            }

        #endregion

            #region UPLOAD_FILE

            // File Name Title
            strFileName = context.Request.Files[0].FileName;
            if (strFileName.Contains(":"))
                strFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "Default" + strExtension;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + CleanFileName(strFileName);

            HttpPostedFile file = context.Request.Files[0];

            if (documentDirectory.Trim() != string.Empty)
                destinationPath = documentDirectory + "/" + fileName;

            saveDirectory += "/" + fileName;

            file.SaveAs(destinationPath);

            UpdateTableDocs(context,TableName,Name,saveDirectory,SectionId,Description,PracticeId);

            strResponse = "success";

            #endregion

        }
        catch (Exception _exception)
        {
            context.Response.Write(_exception);
            return;
        }

        context.Response.Write(strResponse);
        return;

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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void UpdateTableDocs(HttpContext context, string TableName, string Name, string saveDirectory, int SectionId, string Description,int PracticeId)
    {

        try
        {
            if (HttpContext.Current.Session["TableName"].ToString() == enDbTables.ToolDocument.ToString())
                file.SaveUploadData(TableName, Name, saveDirectory, System.DateTime.Now, SectionId, Convert.ToInt32(HttpContext.Current.Session["UserApplicationId"]), Description, PracticeId, "~/Themes/Images/GenericFileUploaderImage.png");
            else if (HttpContext.Current.Session["TableName"].ToString() == enDbTables.LibraryDocument.ToString())
                file.SaveUploadData(TableName, Name, saveDirectory, System.DateTime.Now, SectionId, Convert.ToInt32(HttpContext.Current.Session["UserApplicationId"]), Description, PracticeId, "~/Themes/Images/GenericFileUploaderImage.png");
            else if (HttpContext.Current.Session["TableName"].ToString() == enDbTables.ProjectDocument.ToString())
                file.SaveUploadData(TableName, Name, saveDirectory, System.DateTime.Now, SectionId, Convert.ToInt32(HttpContext.Current.Session["UserApplicationId"]), Description, PracticeId, "~/Themes/Images/GenericFileUploaderImage.png");
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    #endregion
}
