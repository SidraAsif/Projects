
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
public class LogoHandler : IHttpHandler, IRequiresSessionState
{


    #region VARIABLES
    FileBO file = new FileBO();
    #endregion

    #region METHODS

    public void ProcessRequest(HttpContext context)
    {
        /*Local variable*/
        #region local variable
        string strFileName;
        string UserName;
        string destinationPath;
        string saveDirectory;
        #endregion

        context.Response.ContentType = "text/plain";

        /*To get Information*/
        if (context.Request.Form["UserName"] != null)
        {

            UserName = context.Request.Form["UserName"].ToString();

        }
        else
            return;


        #region CREATE_DIRECTORY
        /*Read Root Direct path with name from web.config*/
        string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
        string documentRootDirectory = destinationPath = context.Server.MapPath("~/") + "Themes";
        string documentDirectory = "";

        /*Extract URL path*/
        Uri uri = HttpContext.Current.Request.Url;
        String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
        if (virtualDirectory.Length > 1)
        {
            virtualDirectory += "/";
        }

        saveDirectory = host + virtualDirectory + "Themes";
        /*Create Root Directory if not exists*/
        if (!Directory.Exists(documentRootDirectory))
        { Directory.CreateDirectory(documentRootDirectory); }


        documentDirectory = destinationPath = documentRootDirectory + "/" + "Logos/" + UserName;
        saveDirectory += "/" + "Logos/" + UserName;
        if (!Directory.Exists(documentDirectory))
        { Directory.CreateDirectory(documentDirectory); }
        destinationPath = documentDirectory;

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

                default:
                    strMIMEType = string.Empty;
                    strResponse = "errorFileExt";
                    context.Response.Write(strResponse);
                    return;
            }

        #endregion

            #region UPLOAD_FILE

            // File Name Title
            strFileName = context.Request.Files[0].FileName.ToString().Split('\\').Last();

            HttpPostedFile file = context.Request.Files[0];

            if (documentDirectory.Trim() != string.Empty)
                destinationPath = documentDirectory + "/" + strFileName;

            saveDirectory += "/" + strFileName;

            file.SaveAs(destinationPath);

            context.Session["FilePath"] = saveDirectory;

            strResponse = "success";

            #endregion

        }
        catch
        {

        }

        context.Response.Write(strResponse);
        return;

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #endregion
}
