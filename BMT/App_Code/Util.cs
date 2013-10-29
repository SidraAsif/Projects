using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

using System.ComponentModel;
using System.Reflection;
using BMTBLL;
using BMTBLL.Enumeration;
namespace BMT.WEB
{
    public static class Util
    {
        #region CONSTANTS
        private const string DEFAULT_NCQA_DOCUMENT_DIRECTORY_NAME = "NCQA Documentation";
        private const string DEFAULT_UNASSOCIATED_DOC_DIRECTORY_NAME = "UnAssociated";
        private const char DEFAULT_DIRECTORY_SEPERATOR = '/';
        private const int LAST_INDEX = 1;

        #endregion

        #region FUNCTIONS
        public static string GetPdfPath(int practiceId, string strFileName, bool printOnly)
        {
            try
            {
                // if printOnly is true then dont add time stamp in file name, else add it

                string destinationPath;
                string saveDirectory;
                string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
                string documentRootDirectory = destinationPath = HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                string documentDirectory = "";

                // Extract URL path
                String host = GetHostPath();

                saveDirectory = host + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();

                // Create Root Directory if not exists
                if (!Directory.Exists(documentRootDirectory))
                { Directory.CreateDirectory(documentRootDirectory); }

                if (practiceId != 0)
                {

                    // Directory for Tool
                    documentDirectory = destinationPath = documentRootDirectory + "/" + practiceId + "/My Documents";
                    saveDirectory += "/" + practiceId + "/My Documents";
                    if (!Directory.Exists(documentDirectory))
                        Directory.CreateDirectory(documentDirectory);

                    destinationPath = documentDirectory;

                    // string strFileName = "MyReport.pdf";
                    if (!printOnly)
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + strFileName;
                        if (documentDirectory != "")
                            destinationPath = documentDirectory + "/" + fileName + ".pdf";

                        saveDirectory += "/" + fileName + ".pdf";
                        saveDirectory = saveDirectory.Replace("\\", "/");
                        destinationPath = destinationPath.Replace("\\", "/");

                        return destinationPath + "," + saveDirectory;
                    }
                    else if (printOnly)
                    {
                        if (documentDirectory != "")
                            destinationPath = documentDirectory + "/" + strFileName + ".pdf";

                        saveDirectory += "/" + strFileName + ".pdf";
                        saveDirectory = saveDirectory.Replace("\\", "/");
                        destinationPath = destinationPath.Replace("\\", "/");

                        return destinationPath + "," + saveDirectory;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetTempPdfPath(int practiceId, string strFileName)
        {
            try
            {
                string destinationPath = HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                
                if (practiceId != 0)
                {
                    destinationPath += "/" + practiceId + "/My Documents/Temp";
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    if (destinationPath != string.Empty)
                        destinationPath += "/" + strFileName + ".pdf";

                    destinationPath = destinationPath.Replace("\\", "/");

                    return destinationPath + "," ;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetTempDestinationPath(int practiceId)
        {
            try
            {
                string destinationPath = HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                string saveDirectory = GetHostPath() + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();

                if (practiceId != 0)
                {
                    destinationPath += "/" + practiceId + "/My Documents/Temp/";
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);                    

                    destinationPath = destinationPath.Replace("\\", "/");

                    saveDirectory += "/" + practiceId + "/My Documents/";
                    saveDirectory = saveDirectory.Replace("\\", "/");

                    return destinationPath + "," + saveDirectory;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetHostPath()
        {
            try
            {
                // TODO: Get complete domain name with virtual directory (if exists).
                string virtualDirectory = HttpRuntime.AppDomainAppVirtualPath;
                if (virtualDirectory.Length > 1)
                {
                    virtualDirectory += "/";
                }

                Uri uri = HttpContext.Current.Request.Url;
                String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + virtualDirectory;
                return host;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public static string GetDocRootPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
        }

        public static string GetPathAndQueryByURL(string location)
        {
            try
            {
                Uri uri = new Uri(location);
                string pathAndQuery = uri.PathAndQuery;
                return pathAndQuery;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public static string ExtractDocPath(string pathAndQuery)
        {

            try
            {
                // get document root directory name
                string documentRootDirectory = GetDocRootPath();

                // remove virtual directroy name from existing path
                pathAndQuery = pathAndQuery.Substring(pathAndQuery.IndexOf(documentRootDirectory));

                pathAndQuery = HttpContext.Current.Server.MapPath("~/") + pathAndQuery;
                pathAndQuery = pathAndQuery.Replace("//", "/");
                pathAndQuery = pathAndQuery.Replace("%20", " ");

                return pathAndQuery;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static string GetUnAssociatedDocPath(int practiceId, int siteId)
        {
            // root/Document root Directory/practiceid/ncqa docs directory/siteid/anassociated doc directory
            return HttpContext.Current.Server.MapPath("~/") + GetDocRootPath() + DEFAULT_DIRECTORY_SEPERATOR + practiceId
                + DEFAULT_DIRECTORY_SEPERATOR + DEFAULT_NCQA_DOCUMENT_DIRECTORY_NAME + DEFAULT_DIRECTORY_SEPERATOR + siteId
                + DEFAULT_DIRECTORY_SEPERATOR + DEFAULT_UNASSOCIATED_DOC_DIRECTORY_NAME;
        }

        public static string GetUnAssociatedDocPathForMORe(int practiceId, int siteId, int templateId)
        {
            MOReBO _more = new MOReBO();
            string templateName = _more.GetTemplateName(templateId);

            return HttpContext.Current.Server.MapPath("~/") + GetDocRootPath() + DEFAULT_DIRECTORY_SEPERATOR + practiceId
                + DEFAULT_DIRECTORY_SEPERATOR + templateName + DEFAULT_DIRECTORY_SEPERATOR + siteId
                + DEFAULT_DIRECTORY_SEPERATOR + DEFAULT_UNASSOCIATED_DOC_DIRECTORY_NAME;
        }

        public static string GetSystemBody(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.body);
        }
        public static string GetSystemVersion(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.Version);
        }
        public static string GetSystemAboutUs(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.aboutUs);
        }
        public static string GetSystemCopyright(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.copyright);
        }
        public static string GetSystemProjectName(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.projectName);
        }
        public static string GetSystemConsultingUserMail(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.consultingUserMail);
        }
        public static string GetSystemInviteFriendMail(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.inviteFriendMail);
        }
        public static string GetSystemCompany(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.companyName);
        }
        public static string GetSystemMailTo(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.mailTo);
        }
        public static string GetSystemHost(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.host);
        }

        public static string GetSystemDisclaimerText(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.disclaimerText);
        }

        public static string GetSystemDisclaimerRequired(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.disclaimerRequired);
        }

        public static string GetSRADisclaimerText(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.sraDisclaimerText);
        }

        public static string GetSRADisclaimerRequired(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.sraDisclaimerRequired);
        }

        public static string GetSystemRequiredDocumentsEnabled(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.requiredDocumentsEnabled);
        }

        public static string GetSystemMarkAsCompleteEnabled(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.markAsCompleteEnabled);
        }

        public static string IsNCQADashboardVisible(int enterpriseId)
        {
            SystemInfoBO _systemInfoBO = new SystemInfoBO();
            return _systemInfoBO.GetSystemInfoByKey(enterpriseId, enSystemInfo.ncqaDashboardVisible);
        }

        public static string GetMaxAllowedEHR()
        {
            return System.Configuration.ConfigurationManager.AppSettings["MaxAllowedEHR"].ToString();
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumCategory(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CategoryAttribute[] attributes =
                (CategoryAttribute[])fi.GetCustomAttributes(
                typeof(CategoryAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Category;
            else
                return value.ToString();
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static T GetValueFromCategory<T>(string category)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(CategoryAttribute)) as CategoryAttribute;
                if (attribute != null)
                {
                    if (attribute.Category == category)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == category)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }        

        public static int GetEnterpriseId()
        {            
            try
            {
                UserAccountBO _userAccountBO = new UserAccountBO();
                return _userAccountBO.GetEnterpriseId();
            }
            catch (Exception exception)
            {
                throw exception;
            }            
        }

        public static string GetPageTitle(int enterpriseId)
        {
            SystemInfoBO systemInfo = new SystemInfoBO();
            return systemInfo.GetSystemInfoByKey(enterpriseId, enSystemInfo.projectName);
        }

        public static string CalculateRiskRating(string likelihood, string impact)
        {
            try
            {
                string riskRating = string.Empty;

                int likelihoodScore = likelihood == "Not Likely" ? 1 : (likelihood == "Likely" ? 2 : likelihood == "Very Likely" ? 3 : 0);
                int impactScore = impact == "Low" ? 1 : (impact == "Medium" ? 2 : impact == "High" ? 3 : 0);
                int riskRatingScore = likelihoodScore * impactScore;

                if (riskRatingScore >= 4 && riskRatingScore < 7)
                    riskRating = "mediumRisk";
                else if (riskRatingScore < 4 && riskRatingScore >= 1)
                    riskRating = "lowRisk";
                else if (riskRatingScore > 7)
                    riskRating = "highRisk";
                else if (riskRatingScore == 0)
                    riskRating = "null";

                return riskRating;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static string GetPdfPath(int practiceId, string strFileName, bool printOnly, string mapPath, Uri uri, string virtualDirectory)
        {
            try
            {
                // if printOnly is true then dont add time stamp in file name, else add it

                string destinationPath;
                string saveDirectory;                
                string documentRootDirectory = destinationPath = mapPath + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();
                string documentDirectory = "";

                // Extract URL path
                string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + virtualDirectory;               

                saveDirectory = host + System.Configuration.ConfigurationManager.AppSettings["DocumentRootDirectoryName"].ToString();

                // Create Root Directory if not exists
                if (!Directory.Exists(documentRootDirectory))
                { Directory.CreateDirectory(documentRootDirectory); }

                if (practiceId != 0)
                {

                    // Directory for Tool
                    documentDirectory = destinationPath = documentRootDirectory + "/" + practiceId + "/My Documents";
                    saveDirectory += "/" + practiceId + "/My Documents";
                    if (!Directory.Exists(documentDirectory))
                        Directory.CreateDirectory(documentDirectory);

                    destinationPath = documentDirectory;

                    // string strFileName = "MyReport.pdf";
                    if (!printOnly)
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + strFileName;
                        if (documentDirectory != "")
                            destinationPath = documentDirectory + "/" + fileName + ".pdf";

                        saveDirectory += "/" + fileName + ".pdf";
                        saveDirectory = saveDirectory.Replace("\\", "/");
                        destinationPath = destinationPath.Replace("\\", "/");

                        return destinationPath + "," + saveDirectory;
                    }
                    else if (printOnly)
                    {
                        if (documentDirectory != "")
                            destinationPath = documentDirectory + "/" + strFileName + ".pdf";

                        saveDirectory += "/" + strFileName + ".pdf";
                        saveDirectory = saveDirectory.Replace("\\", "/");
                        destinationPath = destinationPath.Replace("\\", "/");

                        return destinationPath + "," + saveDirectory;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetTempDoc()
        {
            try
            {
                List<string> fileName = new List<string>();
                string[] fNameArray;
                int fName;

                string documentRootDirectory = HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["StDocsRootDirectoryName"].ToString();

                if (!Directory.Exists(documentRootDirectory))
                    Directory.CreateDirectory(documentRootDirectory);

                var files = from file in Directory.EnumerateFiles(documentRootDirectory, "*.pdf")
                            select new
                            {
                                File = file,
                            };

                foreach (var file in files)
                {
                    fNameArray = file.File.ToString().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    fName = fNameArray.Count() - LAST_INDEX;
                    fileName.Add(fNameArray[fName]);
                }

                return fileName;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static string GetStDocsFilePath(string fileName)
        {
            try
            {
                string filePath = null;
                if (fileName != "")
                {
                    filePath = System.Configuration.ConfigurationManager.AppSettings["StDocsRootDirectoryName"].ToString() + '/' + fileName;
                    filePath = filePath.Replace("\\", "/");
                }
                return filePath;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static string GetFileNameByPath(string filePath)
        {
            try
            {
                string[] fileNameArray;
                string fileName="";
                int lastIndex;

                if (filePath != null)
                {
                    fileNameArray = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    lastIndex = fileNameArray.Count() - LAST_INDEX;
                    fileName = fileNameArray[lastIndex];
                }
                return fileName;
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }

        public static bool DocExist(int templateId, string fileName)
        {
            try
            {
                ProjectTemplateBO _project = new ProjectTemplateBO();
               string filePath =  _project.GetFilePath(templateId);
               if (filePath != null)
               {
                   string[] fileArray = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                   int lastIndex = fileArray.Count() - LAST_INDEX;
                   string fName = fileArray[lastIndex];
                   if (fName == fileName)
                       return true;
                   else
                       return false;
               }
               else
                   return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static List<string> GetStandardDocs(int templateId)
        {
            try
            {
                List<string> fileName = new List<string>();
                string[] fNameArray;
                int fName;
                ProjectBO project = new ProjectBO();
                string documentRootDirectory = HttpContext.Current.Server.MapPath("~/") + project.GetStandardFolderLocation(templateId);

                var files = from file in Directory.EnumerateFiles(documentRootDirectory, "*.pdf")
                            select new
                            {
                                File = file,
                            };

                foreach (var file in files)
                {
                    fNameArray = file.File.ToString().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    fName = fNameArray.Count() - LAST_INDEX;
                    fileName.Add(fNameArray[fName]);
                }

                return fileName;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion
    }
}