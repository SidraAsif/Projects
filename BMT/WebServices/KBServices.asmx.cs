using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    public class KBServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public string SaveKnowledgeBase(int templateId, int kbId, int kbTypeId, string displayName, string tabName, string instruction, bool mustPass, string answerTypeId, int userId, bool isCritical, int parentId, int grandParentId, List<int> list, string isEditOrAdd, string dataBoxHeader, string criticalToolText, bool isInfoDocsEnable, int pageReference)
        {
            try
            {
                KnowledgeBaseBO kbbo = new KnowledgeBaseBO();
                return kbbo.SaveKb(templateId, kbId, kbTypeId, displayName, tabName, instruction, mustPass, answerTypeId, userId, isCritical, parentId, grandParentId, list, isEditOrAdd, dataBoxHeader, criticalToolText, isInfoDocsEnable, pageReference);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return "Error";
            }
        }

        [WebMethod(EnableSession = true)]
        public bool CheckTemplateScoringRules(string subHeaderId, string tempId)
        {
            try
            {
                KnowledgeBaseBO _knowledgeBase = new KnowledgeBaseBO();
                return _knowledgeBase.CheckTemplateScoringRules(subHeaderId, tempId);
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteScoringRules(int subHeaderId,int tempId)
        {
            try
            {
                KnowledgeBaseBO _knowledgeBase = new KnowledgeBaseBO();
                return _knowledgeBase.DeleteScoringRules(subHeaderId, tempId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool DocExist(int templateId, string fileName)
        {
            try
            {
                return Util.DocExist(templateId, fileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool TempExist(string projectId, string fileName)
        {
            try
            {

                ProjectBO _project = new ProjectBO();
                return _project.GetProjectTemplates(projectId, fileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool FormExist(string projectId, string fileName)
        {
            try
            {

                ProjectBO _project = new ProjectBO();
                return _project.GetProjectForms(projectId, fileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}
