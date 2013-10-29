using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.WebServices
{
    /// <summary>
    /// Summary description for TreeNodeManipulation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TreeNodeManipulation : System.Web.Services.WebService
    {
        #region VARIABLE
        private TreeBO _treeBO;

        #endregion

        #region CONSTRUCTOR
        public TreeNodeManipulation()
        {
            _treeBO = new TreeBO();

        }

        #endregion

        #region FUNCTIONS
        [WebMethod(EnableSession=true)]
        public string DeleteFolder(int sectionId, string treeType)
        {
            try
            {
                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                _treeBO.DeleteEnterpriseEntity(sectionId, treeType, enterpriseId);

                string Id = _treeBO.DeleteFolderRecursively(sectionId, treeType);
                bool result = _treeBO.DeletebySectionId(sectionId, treeType);

                if (result)
                {
                    return Id;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
                return null;
            }
        }

        [WebMethod]
        public bool RenameFolder(int sectionId, string treeType, string newFolderName)
        {
            try
            {
                _treeBO.RenameNode(treeType, sectionId, newFolderName);

                return true;
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
                return false;

            }


        }

        [WebMethod(EnableSession = true)]
        public int AddFolder(int sectionId, string treeType, string folderName, int enterpriseId, int isJumpMaterialFolder)
        {
            try
            {
                int sessionEnterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                int result = _treeBO.AddTreeNode(sectionId, treeType, folderName, enterpriseId, sessionEnterpriseId, isJumpMaterialFolder);

                if (result != 0)
                {
                    return result;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Logger.PrintError(ex);
                return 0;
            }


        }

        [WebMethod]
        public string GetName(int sectionId, string treeType)
        {
            try
            {
                string result = _treeBO.GetNodeName(sectionId, treeType);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod(EnableSession = true)]
        public bool AddTopLevelFolder(string folderName, string treeType)
        {
            try
            {
                int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);
                bool response = false;
                if (treeType == enTreeType.LibrarySection.ToString())
                    response = _treeBO.AddTopLevelFolder(folderName, enTreeType.LibrarySection, enterpriseId);
                else if (treeType == enTreeType.ToolSection.ToString())
                    response = _treeBO.AddTopLevelFolder(folderName, enTreeType.ToolSection, enterpriseId);

                return response;
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
                return false;
            }
        }

        #endregion
    }
}
