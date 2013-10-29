using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;
using System.IO;

namespace BMT.WebServices
{
    /// <summary>
    /// Summary description for DocumentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DocumentService : System.Web.Services.WebService
    {
        #region WEB_METHODS
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string DeleteDocument(int documentId, string link, string docSection)
        {
            try
            {

                string pathAndQuery = string.Empty;

                // extract local disk path of document
                if (!link.Contains("http://"))
                {
                    pathAndQuery = Util.GetPathAndQueryByURL(link);
                    pathAndQuery = Util.ExtractDocPath(pathAndQuery);
                }

                bool response = false;
                if (docSection == enDbTables.ProjectDocument.ToString())
                {
                    ProjectDocumentBO _projectDocumentBO = new ProjectDocumentBO();

                    _projectDocumentBO.ProjectDocumentId = documentId;
                    response = _projectDocumentBO.DeleteDocById();
                }
                else if (docSection == enDbTables.LibraryDocument.ToString())
                {
                    LibraryBO _libraryBO = new LibraryBO();

                    _libraryBO.LibraryDocumentId = documentId;
                    response = _libraryBO.DeleteDocById();
                }
                else if (docSection == enDbTables.ToolDocument.ToString())
                {
                    ToolDocumentBO _toolDocumentBO = new ToolDocumentBO();

                    _toolDocumentBO.ToolDocumentId = documentId;
                    response = _toolDocumentBO.DeleteDocById();
                }

                if (response)
                {
                    if (pathAndQuery != string.Empty)
                    {
                        if (File.Exists(pathAndQuery))
                            File.Delete(pathAndQuery);
                    }

                    return "Done";
                }
                else
                    return "Failed";

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return "Failed";
            }
        }        

        [WebMethod(EnableSession = true)]
        public bool IsSessionExpired()
        {

            if (HttpContext.Current.Session[enSessionKey.UserApplicationId.ToString()] != null)
            {
                return false;
            }
            else
            {
                return true;
            }


            //if (HttpContext.Current.Session == null || !HttpContext.Current.Session.IsNewSession)
            //{
            //    return false;
            //}

            //// If it says it is a new session, but an existing cookie exists, then it must have timed out     
            //string sessionCookie = HttpContext.Current.Request.Headers["Cookie"];
            //return sessionCookie != null && sessionCookie.Contains("ASP.NET_SessionId");
        }  

        #endregion

    }
}
