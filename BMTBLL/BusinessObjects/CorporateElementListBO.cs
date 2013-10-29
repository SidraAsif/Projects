using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
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
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class CorporateElementListBO : BMTConnection
    {
        #region CONSTANTS
        private char[] DELIMITATOR_STANDARD = new char[] { ':' };
        private int FIRST_INDEX = 0;
        private int ELEMENT_STARTING_INDEX = 7;
        #endregion

        #region VARIABLES
        private string[] standardName;
        private string elementName;
        #endregion

        #region FUNCTIONS
        public List<CorporateElementList> GetElementList()
        {

            try
            {

                List<CorporateElementList> corporateList = (from corporateElementListRecords in BMTDataContext.CorporateElementLists
                                                            select corporateElementListRecords).ToList();

                return corporateList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool AlreadyCorporateElement(int practiceId, int siteId, string corporateElementName)
        {
            try
            {
                var IsCorporateElement = (from CorpSubmissionRec in BMTDataContext.CorporateElementSubmissions
                                          join corpList in BMTDataContext.CorporateElementLists
                                          on CorpSubmissionRec.CorporateElementId equals corpList.CorporateElementId
                                          join PracSite in BMTDataContext.PracticeSites
                                          on CorpSubmissionRec.PracticeId equals PracSite.PracticeId
                                          where CorpSubmissionRec.PracticeId == practiceId &&
                                          PracSite.PracticeSiteId == siteId &&
                                          corpList.ElementName == corporateElementName
                                          select corpList).ToList();
                if (IsCorporateElement.Count != 0)
                    return true;

                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<KBCorporateElement> GetKBTempCorpElementListByTempID(int templateId)
        {
            try
            {
                List<KBCorporateElement> corpElement = new List<KBCorporateElement>();

                var parentIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                 where kbTempRec.TemplateId == templateId &&
                                 kbTempRec.IsCorporateElement == true
                                 select kbTempRec.ParentKnowledgeBaseId).Distinct().ToList();

                foreach (var parent in parentIds)
                {
                    string ParentName = (from kbase in BMTDataContext.KnowledgeBases
                                         where kbase.KnowledgeBaseId == parent
                                         select kbase.Name).FirstOrDefault();

                    standardName = ParentName.Split(DELIMITATOR_STANDARD, StringSplitOptions.RemoveEmptyEntries);

                    List<int> CorpElementIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                where kbTempRec.TemplateId == templateId &&
                                                kbTempRec.IsCorporateElement == true &&
                                                kbTempRec.ParentKnowledgeBaseId == parent
                                                select kbTempRec.KnowledgeBaseId).ToList<int>();

                    foreach (int CorpElementId in CorpElementIds)
                    {
                        KBCorporateElement CorpKBName = (from kbase in BMTDataContext.KnowledgeBases
                                                         join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                         on kbase.KnowledgeBaseId equals kbaseTemp.KnowledgeBaseId
                                                         where kbase.KnowledgeBaseId == CorpElementId
                                                         select new KBCorporateElement
                                                         {
                                                             Name = kbase.Name,
                                                             Id = kbaseTemp.KnowledgeBaseTemplateId
                                                         }).FirstOrDefault();

                        KBCorporateElement kbCorpElement = new KBCorporateElement();
                        kbCorpElement.Name = standardName[FIRST_INDEX] + ' ' + CorpKBName.Name.Substring(ELEMENT_STARTING_INDEX);
                        kbCorpElement.Id = CorpKBName.Id;
                        corpElement.Add(kbCorpElement);
                    }
                }
                return corpElement;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool AlreadyKBCorporateElement(int practiceId, int siteId,int kbtempId)
        {
            try
            {
                var IsCorporateElement = (from corpKBElement in BMTDataContext.CorporateKnowledgeBaseElements
                                          join kbTemp in BMTDataContext.KnowledgeBaseTemplates
                                          on corpKBElement.KnowledgeBaseTemplateId equals kbTemp.KnowledgeBaseTemplateId
                                          join PracSite in BMTDataContext.PracticeSites
                                          on corpKBElement.PracticeId equals PracSite.PracticeId
                                          where corpKBElement.PracticeId == practiceId &&
                                          PracSite.PracticeSiteId == siteId &&
                                          corpKBElement.KnowledgeBaseTemplateId == kbtempId
                                          select corpKBElement).ToList();

                if (IsCorporateElement.Count != 0)
                    return true;

                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}
