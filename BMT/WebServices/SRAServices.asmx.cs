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
    /// Summary description for SRAServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SRAServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public List<AssetDetails> GetAssetDetails()
        {
            List<AssetDetails> assetTypList = new List<AssetDetails>();
            try
            {
                //QuestionBO _questionBO = new QuestionBO();
                //assetTypList = _questionBO.GetAssetTypes();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return assetTypList;
        }


        [WebMethod(EnableSession = true)]
        public string GetUserDetails()
        {
            string userDetails = string.Empty;
            try
            {
                UserBO _userBO = new UserBO();
                userDetails = _userBO.GetUserDetailsByUserId(Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]));
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
            return userDetails;

        }

        [WebMethod(EnableSession = true)]
        public void DeleteAssessmentByProjectId(string projectId)
        {
            try
            {
                //QuestionBO _questionBO = new QuestionBO();
                //_questionBO.DeleteAssessmentByProjectId((int)enQuestionnaireType.SRAQuestionnaire, Convert.ToInt32(projectId));    
            }

            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }


    }
}
