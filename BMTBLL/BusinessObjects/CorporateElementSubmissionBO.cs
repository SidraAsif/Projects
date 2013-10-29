using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using BMTBLL;
using BMTBLL.Enumeration;
using System.IO;

namespace BMTBLL
{
    public class CorporateElementSubmissionBO : BMTConnection
    {
        #region PROPERTIES
        private CorporateElementSubmission _CorporateElementSubmission { get; set; }

        private int _PracticeId;
        public int PracticeId
        {
            get { return _PracticeId; }
            set { _PracticeId = value; }
        }

        private int _CorporateElementId;
        public int CorporateElementId
        {
            get { return _CorporateElementId; }
            set { _CorporateElementId = value; }
        }

        private int _KnowledgebaseTempId;
        public int KnowledgebaseTempId
        {
            get { return _KnowledgebaseTempId; }
            set { _KnowledgebaseTempId = value; }
        }

        private static string[] _UnselectedCorpElement;
        public string[] UnselectedCorpElement
        {
            get { return _UnselectedCorpElement; }
            set { _UnselectedCorpElement = value; }
        }

        private static string[] _SelectedCorpElement;
        public string[] SelectedCorpElement
        {
            get { return _SelectedCorpElement; }
            set { _SelectedCorpElement = value; }
        }

        private static int[] _UnselectedKBCorpElement;
        public int[] UnselectedKBCorpElement
        {
            get { return _UnselectedKBCorpElement; }
            set { _UnselectedKBCorpElement = value; }
        }

        private static int[] _SelectedKBCorpElement;
        public int[] SelectedKBCorpElement
        {
            get { return _SelectedKBCorpElement; }
            set { _SelectedKBCorpElement = value; }
        }
        #endregion

        #region CONSTANT
        private char[] DELIMITATORS = new char[] { ',' };
        private char[] DELIMITATORS_CORP_NAME = new char[] { ':' };
        private char[] DELIMITATORS_CORP_SUBHEADER_NAME = new char[] { ' ', ':' };
        private int FIRST_INDEX = 0;
        private int ELEMENT_STARTING_INDEX = 7;
        #endregion

        #region VARIABLES
        private PracticeBO _practice;
        private CorporateKnowledgeBaseElement _corporateKnowledgeBaseElement;
        private string[] existingElementName;
        private string[] NonExistingCorporateElementName;
        private int[] NonExistingKBCorporateElementName;
        private int[] existingKBElementIds;
        private string standardName;
        private string elementName;
        private string CorpElementName;
        private string Name = "";
        private string pcmhSequence;
        private string elementSequence;
        private string factorSequence;
        private string file;
        private int projectId;
        private string CorpSubHeaderName;
        #endregion

        #region FUNCTIONS
        public bool Save(int practiceId, string corporateElementName)
        {
            try
            {
                _PracticeId = practiceId;

                _CorporateElementId = (from CorporateElementListRecords in BMTDataContext.CorporateElementLists
                                       where CorporateElementListRecords.ElementName == corporateElementName
                                       select CorporateElementListRecords.CorporateElementId).FirstOrDefault();
                if (Insert())
                    return true;
                else
                    return false;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool Insert()
        {
            try
            {
                _CorporateElementSubmission = new CorporateElementSubmission();
                _CorporateElementSubmission.PracticeId = this.PracticeId;
                _CorporateElementSubmission.CorporateElementId = this.CorporateElementId;

                BMTDataContext.CorporateElementSubmissions.InsertOnSubmit(_CorporateElementSubmission);
                BMTDataContext.SubmitChanges();
                return true;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool IsPracticeCorporate(int practiceId)
        {
            try
            {
                var IsPracCorp = (from pracRec in BMTDataContext.Practices
                                  where pracRec.PracticeId == practiceId
                                  select pracRec).FirstOrDefault();

                if (IsPracCorp.IsCorporate != null)
                {
                    if ((bool)IsPracCorp.IsCorporate)
                        return true;
                    else
                        return false;
                }

                else
                    return false;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool IsSiteCorporate(int practiceId, int siteId)
        {
            try
            {
                List<CorporateElementSubmission> _CorporateElementSubmission = (from Cor in BMTDataContext.CorporateElementSubmissions
                                                                                join site in BMTDataContext.Practices
                                                                                on Cor.PracticeId equals site.PracticeId
                                                                                where Cor.PracticeId == practiceId &&
                                                                                site.PracticeSiteId == siteId
                                                                                select Cor).ToList();
                var practice = (from prac in BMTDataContext.Practices
                                where prac.PracticeId == practiceId &&
                                prac.PracticeSiteId == siteId
                                select prac).ToList();

                if (practice.Count != 0)
                    return true;

                else
                    return false;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool IsNotSubmittedCorporateElement(int practiceId, int siteId, String corporateElementName)
        {

            try
            {

                List<string> elementName = (from corpSub in BMTDataContext.CorporateElementSubmissions
                                            join corpElement in BMTDataContext.CorporateElementLists
                                            on corpSub.CorporateElementId equals corpElement.CorporateElementId
                                            where corpSub.PracticeId == practiceId
                                            select corpElement.ElementName).ToList<string>();

                if (elementName.Count != 0)
                {
                    foreach (string element in elementName)
                    {
                        if (element == corporateElementName)
                            return false;
                    }
                    return true;
                }
                else
                    return true;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;

            }
            return true;
        }

        public bool IsCorporateElementofNonCorporateSite(int practiceId, int siteId, String corporateElementName)
        {

            try
            {
                int CorporateElementId = (from CorporateElementListRecords in BMTDataContext.CorporateElementLists
                                          where CorporateElementListRecords.ElementName == corporateElementName
                                          select CorporateElementListRecords.CorporateElementId).FirstOrDefault();

                if (CorporateElementId != 0)
                {
                    List<CorporateElementSubmission> _corporateElementSubmission = (from Cor in BMTDataContext.CorporateElementSubmissions
                                                                                    join site in BMTDataContext.Practices
                                                                                    on Cor.PracticeId equals site.PracticeId
                                                                                    where Cor.PracticeId == practiceId &&
                                                                                    Cor.CorporateElementId == CorporateElementId
                                                                                    select Cor).ToList();


                    if (_corporateElementSubmission.Count != 0)
                        return true;

                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;

            }
            return true;
        }

        public void DeletePreviousCorporateSubmissionList(int practiceId)
        {
            var records = (from Records in BMTDataContext.CorporateElementSubmissions
                           where Records.PracticeId == practiceId
                           select Records).ToList();
            try
            {
                foreach (var record in records)
                {
                    BMTDataContext.CorporateElementSubmissions.DeleteOnSubmit(record);
                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }

        }

        public string[] CheckForEnableDisable(string corporateElementList, int practiceId)
        {
            try
            {
                existingElementName = corporateElementList.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);

                NonExistingCorporateElementName = (from CorpElement in BMTDataContext.CorporateElementLists
                                                   where !existingElementName.Contains(CorpElement.ElementName)
                                                   select CorpElement.ElementName).ToArray();

                string[] DataBaseRecord = (from corpElementList in BMTDataContext.CorporateElementLists
                                           join corpElementSubmission in BMTDataContext.CorporateElementSubmissions
                                           on corpElementList.CorporateElementId equals corpElementSubmission.CorporateElementId
                                           where corpElementSubmission.PracticeId == practiceId
                                           select corpElementList.ElementName).ToArray();

                SelectedCorpElement = (from CorpElementList in BMTDataContext.CorporateElementLists
                                       where existingElementName.Contains(CorpElementList.ElementName) &&
                                       !DataBaseRecord.Contains(CorpElementList.ElementName)
                                       select CorpElementList.ElementName).ToArray();

                UnselectedCorpElement = (from corpElementList in BMTDataContext.CorporateElementLists
                                         join corpElementSubmission in BMTDataContext.CorporateElementSubmissions
                                         on corpElementList.CorporateElementId equals corpElementSubmission.CorporateElementId
                                         where !existingElementName.Contains(corpElementList.ElementName)
                                         select corpElementList.ElementName).ToArray();

                List<int> practiceSitesId = (from pracSite in BMTDataContext.PracticeSites
                                             where pracSite.PracticeId == practiceId
                                             select pracSite.PracticeSiteId).ToList<int>();

                _practice = new PracticeBO();
                foreach (int Site in practiceSitesId)
                {
                    if (_practice.CheckCorporateType(Site))
                    {
                        XDocument questionnaire = XDocument.Parse(GetQuestionaireByPracticeAndSiteId(practiceId, Site));

                        foreach (XElement standard in questionnaire.Root.Elements("Standard"))
                        {
                            foreach (XElement element in standard.Elements("Element"))
                            {
                                standardName = standard.Attribute("title").Value.Substring(0, 6);
                                elementName = element.Attribute("title").Value.Substring(7, 2);
                                CorpElementName = standardName + elementName;
                                for (int currentCorpElementIndex = 0; currentCorpElementIndex < UnselectedCorpElement.Length; currentCorpElementIndex++)
                                {
                                    if (CorpElementName == UnselectedCorpElement[currentCorpElementIndex])
                                    {
                                        string complete = element.Attribute("complete").Value;
                                        if (complete == "Yes")
                                        {
                                            if (!Name.Contains(CorpElementName))
                                            {
                                                if (Name == "")
                                                {
                                                    Name = UnselectedCorpElement[currentCorpElementIndex];
                                                }
                                                else
                                                {
                                                    Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                }
                                                break;
                                            }
                                        }
                                        foreach (XElement factor in element.Elements("Factor"))
                                        {
                                            string factAns = factor.Attribute("answer").Value;
                                            if (factAns == "Yes" || factAns == "NA")
                                            {
                                                if (!Name.Contains(CorpElementName))
                                                {
                                                    if (Name == "")
                                                    {
                                                        Name = UnselectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    else
                                                    {
                                                        Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    break;
                                                }
                                            }
                                            foreach (XElement policies in factor.Elements("Policies"))
                                            {
                                                string policy = policies.Attribute("required").Value;
                                                if (policy != "" || !policies.IsEmpty)
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }

                                            }
                                            foreach (XElement reports in factor.Elements("Reports"))
                                            {
                                                string report = reports.Attribute("required").Value;
                                                if (report != "" || !reports.IsEmpty)
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            foreach (XElement screenshots in factor.Elements("Screenshots"))
                                            {
                                                string screenshot = screenshots.Attribute("required").Value;
                                                if (screenshot != "" || !screenshots.IsEmpty)
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            foreach (XElement logs in factor.Elements("LogsOrTools"))
                                            {
                                                string log = logs.Attribute("required").Value;
                                                if (log != "" || !logs.IsEmpty)
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            foreach (XElement others in factor.Elements("OtherDocs"))
                                            {
                                                string other = others.Attribute("required").Value;
                                                if (other != "" || !others.IsEmpty)
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                            {
                                                string privNote = privNotes.Value;
                                                if (privNote != "")
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }

                                        }
                                        foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                        {
                                            string revNote = revNotes.Value;
                                            if (revNote != "")
                                            {
                                                if (revNotes.Value != "")
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                        {
                                            string evalNote = evalNotes.Value;
                                            if (evalNote != "")
                                                if (!Name.Contains(CorpElementName))
                                                {
                                                    if (Name == "")
                                                    {
                                                        Name = UnselectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    else
                                                    {
                                                        Name += ", " + UnselectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    break;
                                                }
                                        }

                                    }
                                }
                            }
                        }
                        if (Name != "")
                        {
                            string[] MessageToReturn = { "CorpSite", Name };
                            return MessageToReturn;
                        }
                    }
                    else
                    {
                        string recQuestionaire = GetQuestionaireByPracticeAndSiteId(practiceId, Site);
                        if (recQuestionaire != "")
                        {
                            XDocument questionnaire = XDocument.Parse(recQuestionaire);

                            foreach (XElement standard in questionnaire.Root.Elements("Standard"))
                            {
                                foreach (XElement element in standard.Elements("Element"))
                                {
                                    standardName = standard.Attribute("title").Value.Substring(0, 6);
                                    elementName = element.Attribute("title").Value.Substring(7, 2);
                                    CorpElementName = standardName + elementName;
                                    for (int currentCorpElementIndex = 0; currentCorpElementIndex < SelectedCorpElement.Length; currentCorpElementIndex++)
                                    {
                                        if (CorpElementName == SelectedCorpElement[currentCorpElementIndex])
                                        {
                                            string complete = element.Attribute("complete").Value;
                                            if (complete == "Yes")
                                            {
                                                if (!Name.Contains(CorpElementName))
                                                {
                                                    if (Name == "")
                                                    {
                                                        Name = SelectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    else
                                                    {
                                                        Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                    }
                                                    break;
                                                }
                                            }
                                            foreach (XElement factor in element.Elements("Factor"))
                                            {
                                                string factAns = factor.Attribute("answer").Value;
                                                if (factAns == "Yes" || factAns == "NA")
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                                foreach (XElement policies in factor.Elements("Policies"))
                                                {
                                                    string policy = policies.Attribute("required").Value;
                                                    if (policy != "" || !policies.IsEmpty)
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }

                                                }
                                                foreach (XElement reports in factor.Elements("Reports"))
                                                {
                                                    string report = reports.Attribute("required").Value;
                                                    if (report != "" || !reports.IsEmpty)
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                foreach (XElement screenshots in factor.Elements("Screenshots"))
                                                {
                                                    string screenshot = screenshots.Attribute("required").Value;
                                                    if (screenshot != "" || !screenshots.IsEmpty)
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                foreach (XElement logs in factor.Elements("LogsOrTools"))
                                                {
                                                    string log = logs.Attribute("required").Value;
                                                    if (log != "" || !logs.IsEmpty)
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                foreach (XElement others in factor.Elements("OtherDocs"))
                                                {
                                                    string other = others.Attribute("required").Value;
                                                    if (other != "" || !others.IsEmpty)
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                                {
                                                    string privNote = privNotes.Value;
                                                    if (privNote != "")
                                                    {
                                                        if (!Name.Contains(CorpElementName))
                                                        {
                                                            if (Name == "")
                                                            {
                                                                Name = SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            else
                                                            {
                                                                Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                            {
                                                string revNote = revNotes.Value;
                                                if (revNote != "")
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                            {
                                                string evalNote = evalNotes.Value;
                                                if (evalNote != "")
                                                {
                                                    if (!Name.Contains(CorpElementName))
                                                    {
                                                        if (Name == "")
                                                        {
                                                            Name = SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        else
                                                        {
                                                            Name += ", " + SelectedCorpElement[currentCorpElementIndex];
                                                        }
                                                        break;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        if (Name != "")
                        {
                            string[] MessageToReturn = { "NonCorpSite", Name };
                            return MessageToReturn;
                        }

                    }

                }
                string[] EmptyMessage = { "", "" };
                return EmptyMessage;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                string[] EmptyMessage = { "", "" };
                return EmptyMessage;
            }
        }

        public string GetQuestionaireByPracticeAndSiteId(int SiteId, int practiceSiteId)
        {
            string questionaire = "";
            try
            {
                questionaire = Convert.ToString((from fillQues in BMTDataContext.FilledQuestionnaires
                                                 //join project in BMTDataContext.Projects
                                                 //on fillQues.ProjectId equals project.ProjectId
                                                 //join pracSite in BMTDataContext.PracticeSites
                                                 //on project.PracticeSiteId equals pracSite.PracticeSiteId
                                                 //where pracSite.PracticeSiteId == practiceSiteId &&
                                                 //fillQues.QuestionnaireId==2  //for NCQA
                                                 select fillQues.Answers).FirstOrDefault());

                return questionaire;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return questionaire;
            }

        }

        //public void DeleteSelectedElementFromXML(int practiceId)
        //{
        //    try
        //    {
        //        List<int> PracSitesId = (from PracSiteRec in BMTDataContext.PracticeSites
        //                                 where PracSiteRec.PracticeId == practiceId
        //                                 select PracSiteRec.PracticeSiteId).ToList<int>();

        //        //int CorpSiteId = Convert.ToInt32((from pracRec in BMTDataContext.Practices
        //        //                                  where pracRec.PracticeId == practiceId
        //        //                                  select pracRec.PracticeSiteId).FirstOrDefault());
        //        _practice = new PracticeBO();

        //        foreach (int SiteId in PracSitesId)
        //        {
        //            string recQuestionaire = GetQuestionaireByPracticeAndSiteId(practiceId, SiteId);

        //            projectId = (from projRec in BMTDataContext.Projects
        //                         where projRec.PracticeSiteId == SiteId
        //                         select projRec.ProjectId).FirstOrDefault();

        //            if (_practice.CheckCorporateType(SiteId))
        //            {
        //                if (recQuestionaire != "")
        //                {
        //                    XDocument questionnaire = XDocument.Parse(recQuestionaire);
        //                    foreach (XElement standard in questionnaire.Root.Elements("Standard"))
        //                    {
        //                        pcmhSequence = standard.Attribute("sequence").Value.ToString();
        //                        foreach (XElement element in standard.Elements("Element"))
        //                        {
        //                            elementSequence =element.Attribute("sequence").Value.ToString();
        //                            standardName = standard.Attribute("title").Value.Substring(0, 6);
        //                            elementName = element.Attribute("title").Value.Substring(7, 2);
        //                            CorpElementName = standardName + elementName;

        //                            if (UnselectedCorpElement.Contains(CorpElementName))
        //                            {
        //                                string complete = element.Attribute("complete").Value;
        //                                if (complete == "Yes")
        //                                {
        //                                    element.Attribute("complete").Value = "No";
        //                                }
        //                                foreach (XElement factor in element.Elements("Factor"))
        //                                {
        //                                    factorSequence = factor.Attribute("sequence").Value.ToString();

        //                                    string factAns = factor.Attribute("answer").Value;
        //                                    if (factAns == "Yes" || factAns == "NA")
        //                                    {
        //                                        factor.Attribute("answer").Value = "No";
        //                                    }
        //                                    foreach (XElement policies in factor.Elements("Policies"))
        //                                    {
        //                                        policies.Attribute("required").Value = "";
        //                                        policies.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement reports in factor.Elements("Reports"))
        //                                    {
        //                                        reports.Attribute("required").Value = "";
        //                                        reports.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement screenshots in factor.Elements("Screenshots"))
        //                                    {
        //                                        screenshots.Attribute("required").Value = "";
        //                                        screenshots.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement logs in factor.Elements("LogsOrTools"))
        //                                    {
        //                                        logs.Attribute("required").Value = "";
        //                                        logs.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement others in factor.Elements("OtherDocs"))
        //                                    {
        //                                        others.Attribute("required").Value = "";
        //                                        others.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement privNotes in factor.Elements("PrivateNote"))
        //                                    {
        //                                        privNotes.Remove();
        //                                    }
        //                                }
        //                                foreach (XElement revNotes in element.Elements("ReviewerNotes"))
        //                                {
        //                                    revNotes.Value = "";
        //                                }
        //                                foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
        //                                {
        //                                    evalNotes.Remove();
        //                                }
        //                                foreach (XElement defaultScore in element.Elements("Calculation"))
        //                                {
        //                                    defaultScore.Attribute("defaultScore").Value = "0%";
        //                                }
        //                            }
        //                        }
        //                    }

        //                    string Ques = Convert.ToString(questionnaire);
        //                    UpdateQuestionaire(Ques, SiteId);
        //                }
        //            }
        //            else
        //            {
        //                if (recQuestionaire != "")
        //                {
        //                    XDocument questionnaire = XDocument.Parse(recQuestionaire);

        //                    foreach (XElement standard in questionnaire.Root.Elements("Standard"))
        //                    {
        //                        foreach (XElement element in standard.Elements("Element"))
        //                        {
        //                            standardName = standard.Attribute("title").Value.Substring(0, 6);
        //                            elementName = element.Attribute("title").Value.Substring(7, 2);
        //                            CorpElementName = standardName + elementName;
        //                            if (SelectedCorpElement.Contains(CorpElementName))
        //                            {
        //                                string complete = element.Attribute("complete").Value;
        //                                if (complete == "Yes")
        //                                {
        //                                    element.Attribute("complete").Value = "No";
        //                                }
        //                                foreach (XElement factor in element.Elements("Factor"))
        //                                {
        //                                    string factAns = factor.Attribute("answer").Value;
        //                                    if (factAns == "Yes" || factAns == "NA")
        //                                    {
        //                                        factor.Attribute("answer").Value = "No";
        //                                    }
        //                                    foreach (XElement policies in factor.Elements("Policies"))
        //                                    {
        //                                        policies.Attribute("required").Value = "";
        //                                        policies.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement reports in factor.Elements("Reports"))
        //                                    {
        //                                        reports.Attribute("required").Value = "";
        //                                        reports.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement screenshots in factor.Elements("Screenshots"))
        //                                    {
        //                                        screenshots.Attribute("required").Value = "";
        //                                        screenshots.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement logs in factor.Elements("LogsOrTools"))
        //                                    {
        //                                        logs.Attribute("required").Value = "";
        //                                        logs.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement others in factor.Elements("OtherDocs"))
        //                                    {
        //                                        others.Attribute("required").Value = "";
        //                                        others.Descendants("DocFile").Remove();
        //                                    }
        //                                    foreach (XElement privNotes in factor.Elements("PrivateNote"))
        //                                    {
        //                                        privNotes.Remove();
        //                                    }
        //                                }
        //                                foreach (XElement revNotes in element.Elements("ReviewerNotes"))
        //                                {
        //                                    revNotes.Remove();
        //                                }
        //                                foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
        //                                {
        //                                    evalNotes.Remove();
        //                                }
        //                                foreach (XElement privNotes in element.Elements("PrivateNote"))
        //                                {
        //                                    privNotes.Remove();
        //                                }
        //                                foreach (XElement defaultScore in element.Elements("Calculation"))
        //                                {
        //                                    defaultScore.Attribute("defaultScore").Value = "0%";
        //                                }
        //                            }
        //                        }

        //                    }

        //                    string Ques = Convert.ToString(questionnaire);
        //                    UpdateQuestionaire(Ques, SiteId);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string error = Ex.StackTrace;
        //    }

        //}

        public void UpdateQuestionaire(string RecQuestionaire, int practiceSiteId)
        {
            try
            {
                int projectId = Convert.ToInt32((from ProjRec in BMTDataContext.Projects
                                                 //where ProjRec.PracticeSiteId == practiceSiteId
                                                 select ProjRec.ProjectId).FirstOrDefault());

                FilledQuestionnaire filledQuestion = (from fillQues in BMTDataContext.FilledQuestionnaires
                                                      //where fillQues.ProjectId == projectId &&
                                                      //fillQues.QuestionnaireId == 2//for NCQA
                                                      select fillQues).FirstOrDefault();

                filledQuestion.Answers = XElement.Parse(RecQuestionaire);

                BMTDataContext.SubmitChanges();
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }

        }

        public bool IsSiteNotCorpDelete(int SiteId)
        {
            try
            {
                int practiceId = (from PracSite in BMTDataContext.PracticeSites
                                  where PracSite.PracticeSiteId == SiteId
                                  select PracSite.PracticeId).FirstOrDefault();

                if (IsPracticeCorporate(practiceId))
                {
                    var SiteIds = (from pracSiteRec in BMTDataContext.PracticeSites
                                   where pracSiteRec.PracticeId == practiceId
                                   select pracSiteRec.PracticeSiteId).ToList();

                    if (SiteIds.Count <= 3)
                        return false;
                    else
                        return true;
                }
                else
                    return true;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsDeletingCorporateSite(int siteId)
        {
            try
            {
                int practiceId = (from pracSite in BMTDataContext.PracticeSites
                                  where pracSite.PracticeSiteId == siteId
                                  select pracSite.PracticeId).FirstOrDefault();

                if (IsPracticeCorporate(practiceId))
                {
                    int CorpSiteId = Convert.ToInt32((from PracRec in BMTDataContext.Practices
                                                      where PracRec.PracticeId == practiceId
                                                      select PracRec.PracticeSiteId).FirstOrDefault());

                    if (CorpSiteId == siteId)
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

        public List<int> GetPracticeSiteIdsByPracticeId(int practiceId)
        {
            try
            {
                List<int> PracSitesId = (from PracSiteRec in BMTDataContext.PracticeSites
                                         where PracSiteRec.PracticeId == practiceId
                                         select PracSiteRec.PracticeSiteId).ToList<int>();

                return PracSitesId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int GetProjectIdByPracticeSiteId(int SiteId)
        {
            try
            {
                projectId = (from projRec in BMTDataContext.Projects
                             //where projRec.PracticeSiteId == SiteId
                             select projRec.ProjectId).FirstOrDefault();

                return projectId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool SaveMOReCorpElement(int practiceId, string corporateElementTempId)
        {
            try
            {
                _PracticeId = practiceId;
                _KnowledgebaseTempId = Convert.ToInt32(corporateElementTempId);

                if (InsertMOReCorp())
                    return true;
                else
                    return false;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool InsertMOReCorp()
        {
            try
            {
                _corporateKnowledgeBaseElement = new CorporateKnowledgeBaseElement();
                _corporateKnowledgeBaseElement.PracticeId = this.PracticeId;
                _corporateKnowledgeBaseElement.KnowledgeBaseTemplateId = this.KnowledgebaseTempId;

                BMTDataContext.CorporateKnowledgeBaseElements.InsertOnSubmit(_corporateKnowledgeBaseElement);
                BMTDataContext.SubmitChanges();
                return true;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public string[] CheckForEnableDisableCorpMORe(string corpElementListIds, int practiceId)
        {
            try
            {
                existingElementName = corpElementListIds.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);

                existingKBElementIds = new int[existingElementName.Length];

                for (int length = 0; length < existingElementName.Length; length++)
                {
                    existingKBElementIds[length] = Convert.ToInt32(existingElementName[length].ToString());
                }

                NonExistingKBCorporateElementName = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                     where !existingKBElementIds.Contains(kbaseTempRec.KnowledgeBaseTemplateId) &&
                                                     kbaseTempRec.IsCorporateElement == true
                                                     select kbaseTempRec.KnowledgeBaseTemplateId).ToArray();

                int[] DataBaseRecord = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                        join kbaseCorpSubmission in BMTDataContext.CorporateKnowledgeBaseElements
                                        on kbaseTempRec.KnowledgeBaseTemplateId equals kbaseCorpSubmission.KnowledgeBaseTemplateId
                                        where kbaseCorpSubmission.PracticeId == practiceId
                                        select kbaseTempRec.KnowledgeBaseTemplateId).ToArray();

                SelectedKBCorpElement = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                         where existingKBElementIds.Contains(kbaseTempRec.KnowledgeBaseTemplateId) &&
                                        !DataBaseRecord.Contains(kbaseTempRec.KnowledgeBaseTemplateId)
                                         select kbaseTempRec.KnowledgeBaseTemplateId).ToArray();

                UnselectedKBCorpElement = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                           join kbaseCorpSubmission in BMTDataContext.CorporateKnowledgeBaseElements
                                          on kbaseTempRec.KnowledgeBaseTemplateId equals kbaseCorpSubmission.KnowledgeBaseTemplateId
                                           where (!existingKBElementIds.Contains(kbaseTempRec.KnowledgeBaseTemplateId) &&
                                           kbaseTempRec.IsCorporateElement == true)
                                           select kbaseTempRec.KnowledgeBaseTemplateId).ToArray();

                List<int> practiceSitesId = (from pracSite in BMTDataContext.PracticeSites
                                             where pracSite.PracticeId == practiceId
                                             select pracSite.PracticeSiteId).ToList<int>();

                _practice = new PracticeBO();
                foreach (int Site in practiceSitesId)
                {
                    int projectID = (from projectRec in BMTDataContext.Projects
                                     //where projectRec.PracticeSiteId == Site
                                     select projectRec.ProjectId).FirstOrDefault();

                    if (_practice.CheckCorporateTypeMORe(Site))
                    {
                        var filledAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                         where 
                                         //filledAnsRec.ProjectId == projectID &&
                                         UnselectedKBCorpElement.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                         select filledAnsRec).Distinct().ToList();

                        foreach (var ans in filledAns)
                        {
                            int headerId = Convert.ToInt32((from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                            where kbaseTemp.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                            select kbaseTemp.ParentKnowledgeBaseId).FirstOrDefault());

                            int subheaderId = Convert.ToInt32((from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                               where kbaseTemp.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                               select kbaseTemp.KnowledgeBaseId).FirstOrDefault());

                            CorpElementName = (from kbase in BMTDataContext.KnowledgeBases
                                               where kbase.KnowledgeBaseId == headerId
                                               select kbase.Name).FirstOrDefault();

                            CorpSubHeaderName = (from kbase in BMTDataContext.KnowledgeBases
                                                 where kbase.KnowledgeBaseId == subheaderId
                                                 select kbase.Name).FirstOrDefault();

                            CorpElementName = CorpElementName.Split(DELIMITATORS_CORP_NAME, StringSplitOptions.RemoveEmptyEntries)[0];
                            CorpSubHeaderName = CorpSubHeaderName.Split(DELIMITATORS_CORP_SUBHEADER_NAME, StringSplitOptions.RemoveEmptyEntries)[1];

                            if (ans.Complete == true)
                            {
                                if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                {
                                    if (Name == "")
                                    {
                                        Name = CorpElementName + " " + CorpSubHeaderName;
                                    }
                                    else
                                    {
                                        Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                    }
                                }
                            }
                            if (ans.ReviewNotes != "")
                            {
                                if (ans.ReviewNotes != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }
                            if (ans.EvaluationNotes != "")
                            {
                                if (ans.EvaluationNotes != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }

                            KnowledgeBaseTemplate kbTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                            where kbaseTempRec.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                            select kbaseTempRec).FirstOrDefault();

                            List<int> kbTempIds = (from knowledgebaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                   where knowledgebaseTemp.ParentKnowledgeBaseId == kbTemp.KnowledgeBaseId
                                                   select knowledgebaseTemp.KnowledgeBaseTemplateId).ToList();


                            var fillAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                           where 
                                           //filledAnsRec.ProjectId == projectID &&
                                           kbTempIds.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                           select filledAnsRec).Distinct().ToList();

                            foreach (FilledAnswer answer in fillAns)
                            {
                                if (answer.DataBoxComments != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                                if (answer.PoliciesDocumentCount != 0)
                                {
                                    if (answer.PoliciesDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.ReportsDocumentCount != 0)
                                {
                                    if (answer.ReportsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.ScreenShotsDocumentCount != 0)
                                {
                                    if (answer.ScreenShotsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.LogsOrToolsDocumentCount != 0)
                                {
                                    if (answer.LogsOrToolsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.OtherDocumentsCount != 0)
                                {
                                    if (answer.OtherDocumentsCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.AnswerTypeEnumId != null)
                                {
                                    string ansType = (from fAnswer in BMTDataContext.FilledAnswers
                                                      join ansTypeEnum in BMTDataContext.AnswerTypeEnums
                                                      on fAnswer.AnswerTypeEnumId equals ansTypeEnum.AnswerTypeEnumId
                                                      where fAnswer.FilledAnswersId == answer.FilledAnswersId
                                                      select ansTypeEnum.DiscreteValue).FirstOrDefault();

                                    if (ansType != "No")
                                    { if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                    }
                                }
                                List<FilledTemplateDocument> fTempDoc = (from fTempDocs in BMTDataContext.FilledTemplateDocuments
                                                                         where fTempDocs.FilledAnswerId == answer.FilledAnswersId
                                                                         select fTempDocs).ToList();

                                if (fTempDoc.Count != 0)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }
                        }
                        if (Name != "")
                        {
                            string[] MessageToReturn = { "CorpSite", Name };
                            return MessageToReturn;
                        }
                    }
                    else
                    {
                        var filledAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                         where 
                                         //filledAnsRec.ProjectId == projectID &&
                                        SelectedKBCorpElement.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                         select filledAnsRec).Distinct().ToList();

                        foreach (var ans in filledAns)
                        {
                            int headerId = Convert.ToInt32((from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                            where kbaseTemp.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                            select kbaseTemp.ParentKnowledgeBaseId).FirstOrDefault());
                            int subheaderId = Convert.ToInt32((from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                               where kbaseTemp.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                               select kbaseTemp.KnowledgeBaseId).FirstOrDefault());

                            CorpElementName = (from kbase in BMTDataContext.KnowledgeBases
                                               where kbase.KnowledgeBaseId == headerId
                                               select kbase.Name).FirstOrDefault();

                            CorpSubHeaderName = (from kbase in BMTDataContext.KnowledgeBases
                                                 where kbase.KnowledgeBaseId == subheaderId
                                                 select kbase.Name).FirstOrDefault();

                            CorpElementName = CorpElementName.Split(DELIMITATORS_CORP_NAME, StringSplitOptions.RemoveEmptyEntries)[0];
                            CorpSubHeaderName = CorpSubHeaderName.Split(DELIMITATORS_CORP_SUBHEADER_NAME, StringSplitOptions.RemoveEmptyEntries)[1];
                            if (ans.Complete == true)
                            {
                                if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                {
                                    if (Name == "")
                                    {
                                        Name = CorpElementName + " " + CorpSubHeaderName;
                                    }
                                    else
                                    {
                                        Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                    }
                                }
                            }
                            if (ans.ReviewNotes != "")
                            {
                                if (ans.ReviewNotes != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }
                            if (ans.EvaluationNotes != "")
                            {
                                if (ans.EvaluationNotes != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }

                            KnowledgeBaseTemplate kbTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                            where kbaseTempRec.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                                            select kbaseTempRec).FirstOrDefault();

                            List<int> kbTempIds = (from knowledgebaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                   where knowledgebaseTemp.ParentKnowledgeBaseId == kbTemp.KnowledgeBaseId
                                                   select knowledgebaseTemp.KnowledgeBaseTemplateId).ToList();


                            var fillAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                           where 
                                           //filledAnsRec.ProjectId == projectID &&
                                           kbTempIds.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                           select filledAnsRec).Distinct().ToList();

                            foreach (FilledAnswer answer in fillAns)
                            {
                                if (answer.DataBoxComments != null)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                                if (answer.PoliciesDocumentCount != 0)
                                {
                                    if (answer.PoliciesDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.ReportsDocumentCount != 0)
                                {
                                    if (answer.ReportsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.ScreenShotsDocumentCount != 0)
                                {
                                    if (answer.ScreenShotsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.LogsOrToolsDocumentCount != 0)
                                {
                                    if (answer.LogsOrToolsDocumentCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.OtherDocumentsCount != 0)
                                {
                                    if (answer.OtherDocumentsCount != null)
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                if (answer.AnswerTypeEnumId != null)
                                {
                                    string ansType = (from fAnswer in BMTDataContext.FilledAnswers
                                                      join ansTypeEnum in BMTDataContext.AnswerTypeEnums
                                                      on fAnswer.AnswerTypeEnumId equals ansTypeEnum.AnswerTypeEnumId
                                                      where fAnswer.FilledAnswersId == answer.FilledAnswersId
                                                      select ansTypeEnum.DiscreteValue).FirstOrDefault();

                                    if (ansType != "No")
                                    {
                                        if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                        {
                                            if (Name == "")
                                            {
                                                Name = CorpElementName + " " + CorpSubHeaderName;
                                            }
                                            else
                                            {
                                                Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                            }
                                        }
                                    }
                                }
                                List<FilledTemplateDocument> fTempDoc = (from fTempDocs in BMTDataContext.FilledTemplateDocuments
                                                                         where fTempDocs.FilledAnswerId == answer.FilledAnswersId
                                                                         select fTempDocs).ToList();

                                if (fTempDoc.Count != 0)
                                {
                                    if (!Name.Contains(CorpElementName + " " + CorpSubHeaderName))
                                    {
                                        if (Name == "")
                                        {
                                            Name = CorpElementName + " " + CorpSubHeaderName;
                                        }
                                        else
                                        {
                                            Name += ", " + CorpElementName + " " + CorpSubHeaderName;
                                        }
                                    }
                                }
                            }
                        }
                        if (Name != "")
                        {
                            string[] MessageToReturn = { "NonCorpSite", Name };
                            return MessageToReturn;
                        }

                    }

                }
                string[] EmptyMessage = { "", "" };
                return EmptyMessage;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                string[] EmptyMessage = { "", "" };
                return EmptyMessage;
            }
        }

        public void DeletePreviousKBCorporateElement(int practiceId)
        {
            try
            {
                var records = (from Records in BMTDataContext.CorporateKnowledgeBaseElements
                               where Records.PracticeId == practiceId
                               select Records).ToList();
                foreach (var record in records)
                {
                    BMTDataContext.CorporateKnowledgeBaseElements.DeleteOnSubmit(record);
                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }

        }

        public List<FilledAnswer> GetElementFilledAnswerOfCorpSiteByProjectId(int projectId)
        {
            try
            {
                List<FilledAnswer> filledAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                                where 
                                                //filledAnsRec.ProjectId == projectId &&
                                                UnselectedKBCorpElement.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                                select filledAnsRec).Distinct().ToList();

                return filledAns;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void UpdateElementFilledAnswer(int filledAnsId)
        {
            try
            {
                FilledAnswer FillAns = (from fillAnsRec in BMTDataContext.FilledAnswers
                                        where fillAnsRec.FilledAnswersId == filledAnsId
                                        select fillAnsRec).FirstOrDefault();

                FillAns.Complete = null;
                FillAns.DefaultScore = "0%";
                FillAns.ReviewNotes = "";
                FillAns.EvaluationNotes = "";

                BMTDataContext.SubmitChanges();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<TemplateDocument> GetAllQuestionOfElement(int ElementKbTempId, int projectId, int templateId)
        {
            try
            {
                int ElementkbId = (from kbTemp in BMTDataContext.KnowledgeBaseTemplates
                                   where kbTemp.KnowledgeBaseTemplateId == ElementKbTempId
                                   select kbTemp.KnowledgeBaseId).FirstOrDefault();

                List<int> QuestionkbTempIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                               where kbTempRec.ParentKnowledgeBaseId == ElementkbId &&
                                               kbTempRec.TemplateId == templateId
                                               select kbTempRec.KnowledgeBaseTemplateId).ToList();

                List<int> QuestionfilledAndIds = (from filledAns in BMTDataContext.FilledAnswers
                                                  where QuestionkbTempIds.Contains(filledAns.KnowledgeBaseTemplateId)
                                                  //&& filledAns.ProjectId == projectId
                                                  select filledAns.FilledAnswersId).ToList();

                List<int> filledTempDoc = (from fillTempDoc in BMTDataContext.FilledTemplateDocuments
                                           where QuestionfilledAndIds.Contains((int)fillTempDoc.FilledAnswerId)
                                           select fillTempDoc.DocumentId).ToList();

                List<TemplateDocument> tempDoc = (from tempDocs in BMTDataContext.TemplateDocuments
                                                  where filledTempDoc.Contains(tempDocs.DocumentId)
                                                  select tempDocs).ToList();
                return tempDoc;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<FilledAnswer> GetElementFilledAnswerOfNonCorpSiteByProjectId(int projectId)
        {
            try
            {
                List<FilledAnswer> filledAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                                where 
                                                //filledAnsRec.ProjectId == projectId &&
                                                SelectedKBCorpElement.Contains(filledAnsRec.KnowledgeBaseTemplateId)
                                                select filledAnsRec).Distinct().ToList();

                return filledAns;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void UpdateQuestionFilledAnswer(int elementTempId, int templateId, int projectId)
        {
            try
            {
                int ElementkbId = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                   where kbTempRec.KnowledgeBaseTemplateId == elementTempId
                                   select kbTempRec.KnowledgeBaseId).FirstOrDefault();

                List<int> QuestionkbTempIds = (from kbase in BMTDataContext.KnowledgeBaseTemplates
                                               where kbase.ParentKnowledgeBaseId == ElementkbId &&
                                               kbase.TemplateId == templateId
                                               select kbase.KnowledgeBaseTemplateId).ToList();

                List<FilledAnswer> fAns = (from fAnsRec in BMTDataContext.FilledAnswers
                                           where QuestionkbTempIds.Contains(fAnsRec.KnowledgeBaseTemplateId) 
                                           //&& fAnsRec.ProjectId == projectId
                                           select fAnsRec).ToList();

                foreach (FilledAnswer ans in fAns)
                {
                    ans.DataBoxComments = null;
                    ans.PoliciesDocumentCount = 0;
                    ans.ReportsDocumentCount = 0;
                    ans.ScreenShotsDocumentCount = 0;
                    ans.LogsOrToolsDocumentCount = 0;
                    ans.OtherDocumentsCount = 0;
                    ans.PrivateNotes = "";
                    ans.AnswerTypeEnumId = (int)enAnsTypeEnum.TypeYesNoNA_No;

                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsNotSubmittedMOReCorporateElement(int practiceId, int siteId, int templateId, String corporateElementName)
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

                    string[] headerName = ParentName.Split(DELIMITATORS_CORP_NAME, StringSplitOptions.RemoveEmptyEntries);

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

                        string ElementID = CorpKBName.Name.Split(DELIMITATORS_CORP_SUBHEADER_NAME, StringSplitOptions.RemoveEmptyEntries)[1];

                        List<int> submittedElementIds = (from corpKBSubmission in BMTDataContext.CorporateKnowledgeBaseElements
                                                         where corpKBSubmission.PracticeId == practiceId
                                                         select corpKBSubmission.KnowledgeBaseTemplateId).ToList();

                        if (submittedElementIds.Contains(CorpKBName.Id))
                        {
                            KBCorporateElement kbCorpElement = new KBCorporateElement();
                            kbCorpElement.Name = headerName[FIRST_INDEX] + ' ' + ElementID;
                            kbCorpElement.Id = CorpKBName.Id;
                            corpElement.Add(kbCorpElement);
                        }
                    }
                }

                foreach (KBCorporateElement kbcorpElement in corpElement)
                {
                    if (kbcorpElement.Name == corporateElementName)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;

            }
            return true;
        }

        public bool IsCorporateElementofMOReNonCorporateSite(int practiceId, int siteId, int templateId, String corporateElementName)
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

                    string[] headerName = ParentName.Split(DELIMITATORS_CORP_NAME, StringSplitOptions.RemoveEmptyEntries);

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

                        string ElementID = CorpKBName.Name.Split(DELIMITATORS_CORP_SUBHEADER_NAME, StringSplitOptions.RemoveEmptyEntries)[1];

                        List<int> submittedElementIds = (from corpKBSubmission in BMTDataContext.CorporateKnowledgeBaseElements
                                                         where corpKBSubmission.PracticeId == practiceId
                                                         select corpKBSubmission.KnowledgeBaseTemplateId).ToList();

                        if (submittedElementIds.Contains(CorpKBName.Id))
                        {
                            KBCorporateElement kbCorpElement = new KBCorporateElement();
                            kbCorpElement.Name = headerName[FIRST_INDEX] + ' ' + ElementID;
                            kbCorpElement.Id = CorpKBName.Id;
                            corpElement.Add(kbCorpElement);
                        }
                    }
                }

                foreach (KBCorporateElement kbcorpElement in corpElement)
                {
                    if (kbcorpElement.Name == corporateElementName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;

            }
            return true;
        }

        public int GetPracticeId(int siteId)
        {
            try
            {
                int practiceId = (from pracSite in BMTDataContext.PracticeSites
                                  where pracSite.PracticeSiteId == siteId
                                  select pracSite.PracticeId).FirstOrDefault();

                return practiceId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<FilledAnswer> GetCorporateElementFilledAnswerOfCorpSiteByProjectId(int projectId)
        {
            try
            {
                List<FilledAnswer> filledAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                                join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                on filledAnsRec.KnowledgeBaseTemplateId equals kbaseTemp.KnowledgeBaseTemplateId
                                                join kbase in BMTDataContext.KnowledgeBases
                                                on kbaseTemp.KnowledgeBaseId equals kbase.KnowledgeBaseId
                                                where 
                                                //filledAnsRec.ProjectId == projectId &&
                                                kbase.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.SubHeader &&
                                                (kbaseTemp.IsCorporateElement == null || kbaseTemp.IsCorporateElement == false)
                                                select filledAnsRec).Distinct().ToList();

                return filledAns;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsPracticeCorporateMORe(int practiceId,int templateId)
        {
            try
            {
                //var IsPracCorp = (from pracRec in BMTDataContext.PracticeTemplates
                //                  where pracRec.PracticeId == practiceId &&
                //                  pracRec.TemplateId==templateId
                //                  select pracRec).FirstOrDefault();

                //if (IsPracCorp.IsCorporate != null)
                //{
                //    if ((bool)IsPracCorp.IsCorporate)
                //        return true;
                //    else
                //        return false;
                //}

                //else
                    return false;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool IsSiteCorporateMORe(int practiceId, int siteId)
        {
            try
            {
                //List<CorporateKnowledgeBaseElement> _CorporateElementSubmission = (from Cor in BMTDataContext.CorporateKnowledgeBaseElements
                //                                                                   join site in BMTDataContext.Practices
                //                                                                   on Cor.PracticeId equals site.PracticeId
                //                                                                   where Cor.PracticeId == practiceId &&
                //                                                                   site.PracticeSiteId == siteId
                //                                                                   select Cor).ToList();
                //var practice = (from prac in BMTDataContext.PracticeTemplates
                //                where prac.PracticeId == practiceId &&
                //                prac.PracticeSiteId == siteId
                //                select prac).ToList();

                //if (practice.Count != 0)
                //    return true;

                //else
                    return false;

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }

        public bool IsMOReSiteNotCorpDelete(int SiteId)
        {
            try
            {
                int practiceId = (from PracSite in BMTDataContext.PracticeSites
                                  where PracSite.PracticeSiteId == SiteId
                                  select PracSite.PracticeId).FirstOrDefault();

                if (IsPracticeCorpMORe(practiceId))
                {
                    var SiteIds = (from pracSiteRec in BMTDataContext.PracticeSites
                                   where pracSiteRec.PracticeId == practiceId
                                   select pracSiteRec.PracticeSiteId).ToList();

                    if (SiteIds.Count <= 3)
                        return false;
                    else
                        return true;
                }
                else
                    return true;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsDeletingMOReCorporateSite(int siteId)
        {
            try
            {
                int practiceId = (from pracSite in BMTDataContext.PracticeSites
                                  where pracSite.PracticeSiteId == siteId
                                  select pracSite.PracticeId).FirstOrDefault();

                if (IsPracticeCorpMORe(practiceId))
                {
                    int CorpSiteId = Convert.ToInt32((from PracRec in BMTDataContext.Practices
                                                      where PracRec.PracticeId == practiceId
                                                      select PracRec.PracticeSiteId).FirstOrDefault());

                    if (CorpSiteId == siteId)
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

        public bool IsPracticeCorpMORe(int practiceId)
        {
            try
            {
                //var IsPracCorp = (from pracRec in BMTDataContext.PracticeTemplates
                //                  where pracRec.PracticeId == practiceId
                //                  select pracRec).FirstOrDefault();

                //if (IsPracCorp.IsCorporate != null)
                //{
                //    if ((bool)IsPracCorp.IsCorporate)
                //        return true;
                //    else
                //        return false;
                //}

                //else
                    return false;
            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
                return false;
            }
        }
        #endregion
    }
}