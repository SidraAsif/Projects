#region Modification History

//  ******************************************************************************
//  Module        : Questionnaire Module
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    04-05-2012      GetEHRQuestionnaireByType
//  Mirza Fahad Ali Baig    04-05-2012      Remove GetEHRQuestionnaireByType Method
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using System.Xml;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class QuestionBO : BMTConnection
    {
        #region DataMember
        private FilledQuestionnaire _filledQuestionnaire;

        #endregion

        #region PROPERTIES
        private int _questionnaireId;
        public int QuestionnaireId
        {
            get { return _questionnaireId; }
            set { _questionnaireId = value; }
        }

        private int _projectId;
        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        private int _projectUsageId;
        public int ProjectUsageId
        {
            get { return _projectUsageId; }
            set { _projectUsageId = value; }
        }

        private int _practiceId;
        public int PracticeId
        {
            get { return _practiceId; }
            set { _practiceId = value; }
        }
        private int _siteId;
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }
        #endregion

        #region CONSTRUCTOR
        public QuestionBO()
        {

        }

        #endregion

        #region CONSTANT
        int TEMPLATE_ID = 1;
        #endregion

        #region FUNCTIONS
        public string GetQuestionnaireByTypeEHR()
        {

            try
            {
                string question = string.Empty;

                // check if Question is already submitted against the selected project
                var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                           where
                                           QuestionnaireRecord.ProjectUsageId == this.ProjectUsageId &&
                                           QuestionnaireRecord.FormId == this.QuestionnaireId
                                           select QuestionnaireRecord).SingleOrDefault();

                if (filledQuestionnaire != null)
                    return Convert.ToString(filledQuestionnaire.Answers);
                else
                    question = GetNewQuestionnaire();

                return question;


            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetQuestionnaireByType()
        {

            try
            {
                string question = string.Empty;

                // check if Question is already submitted against the selected project
                var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                           where
                                           QuestionnaireRecord.ProjectUsageId == this.ProjectUsageId &&
                                           QuestionnaireRecord.PracticeSiteId == this.SiteId &&
                                           QuestionnaireRecord.FormId == this.QuestionnaireId
                                           select QuestionnaireRecord).SingleOrDefault();

                if (filledQuestionnaire != null)
                    return Convert.ToString(filledQuestionnaire.Answers);
                else
                    question = GetNewQuestionnaire();

                return question;


            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetQuestionnaireByType(int medicalGroupId)
        {

            try
            {
                string question = string.Empty;

                // check if Question is already submitted against the selected project
                var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                           where
                                               //QuestionnaireRecord.ProjectId == this.ProjectId && 
                                           QuestionnaireRecord.FormId == this.QuestionnaireId
                                           select QuestionnaireRecord).SingleOrDefault();

                if (filledQuestionnaire != null)
                    return Convert.ToString(filledQuestionnaire.Answers);
                else
                    question = GetNewQuestionnaire(medicalGroupId);

                return question;


            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetEHRQuestionnaire()
        {

            try
            {
                string question = string.Empty;

                // check if Question is already submitted against the selected project
                var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                           where
                                           QuestionnaireRecord.ProjectUsageId == this.ProjectUsageId &&
                                           (QuestionnaireRecord.FormId == (int)enQuestionnaireType.Subscription
                                           || QuestionnaireRecord.FormId == (int)enQuestionnaireType.License)
                                           select QuestionnaireRecord).SingleOrDefault();

                if (filledQuestionnaire != null)
                    return Convert.ToString(filledQuestionnaire.Answers);
                else
                    question = string.Empty;

                return question;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetNewQuestionnaire()
        {
            try
            {
                // Get Fresh Questionnaire
                var questionnaire = (from questionRecord in BMTDataContext.Forms
                                     where questionRecord.FormId == this.QuestionnaireId
                                     select questionRecord).SingleOrDefault();

                string question = string.Empty;
                if (questionnaire != null)
                    question = Convert.ToString(questionnaire.Questionnaire);
                else
                    question = string.Empty;

                return question;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<NCQADetails> GetStandards()
        {
            var ncqaStandards = (from kbtemplate in BMTDataContext.KnowledgeBaseTemplates
                                 join kbase in BMTDataContext.KnowledgeBases
                                 on kbtemplate.KnowledgeBaseId equals kbase.KnowledgeBaseId
                                 where kbtemplate.TemplateId == TEMPLATE_ID   //NCQA
                                 && kbase.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Header
                                 select new NCQADetails
                                 {
                                     Sequence = kbtemplate.Sequence.ToString(),
                                     Name = kbase.TabName
                                 }).ToList();

            return ncqaStandards;
        }

        public List<NCQADetails> GetElements(string Standardsequence)
        {
            var elements = (from kbtRecord in BMTDataContext.KnowledgeBaseTemplates //For Elements
                            join kbaseRecord in BMTDataContext.KnowledgeBases
                            on kbtRecord.KnowledgeBaseId equals kbaseRecord.KnowledgeBaseId
                            where kbtRecord.TemplateId == TEMPLATE_ID
                            && kbtRecord.ParentKnowledgeBaseId == ((from kbtRecord2 in BMTDataContext.KnowledgeBaseTemplates   // For Standard uisng Sequence
                                                                    join kbaseRecord2 in BMTDataContext.KnowledgeBases
                                                                    on kbtRecord2.KnowledgeBaseId equals kbaseRecord2.KnowledgeBaseId
                                                                    where (kbaseRecord2.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Header
                                                                    && kbtRecord2.TemplateId == TEMPLATE_ID
                                                                    && kbtRecord2.Sequence == Convert.ToInt32(Standardsequence))
                                                                    select kbtRecord2.KnowledgeBaseId).FirstOrDefault())

                            select new NCQADetails
                            {
                                PCMHSequence = Standardsequence,
                                ElementSequence = kbtRecord.Sequence.ToString(),
                                Name = kbaseRecord.DisplayName
                            }).ToList();

            return elements;
        }

        public List<NCQADetails> GetFactors(string Standardsequence, string elementSequence)
        {
            var factors = (from kbtRecord1 in BMTDataContext.KnowledgeBaseTemplates
                           where kbtRecord1.ParentKnowledgeBaseId == ((from kbtRecord2 in BMTDataContext.KnowledgeBaseTemplates
                                                                       where (kbtRecord2.Sequence == Convert.ToInt32(elementSequence)   //to select Element using Sequence
                                                                       && kbtRecord2.ParentKnowledgeBaseId == ((from kbtRecord3 in BMTDataContext.KnowledgeBaseTemplates   // to select Standard uisng Sequence
                                                                                                                join kbaseRecord3 in BMTDataContext.KnowledgeBases
                                                                                                                on kbtRecord3.KnowledgeBaseId equals kbaseRecord3.KnowledgeBaseId
                                                                                                                where (kbaseRecord3.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Header
                                                                                                                && kbtRecord3.TemplateId == TEMPLATE_ID
                                                                                                                && kbtRecord3.Sequence == Convert.ToInt32(Standardsequence))
                                                                                                                select kbtRecord3.KnowledgeBaseId).FirstOrDefault()))
                                                                       select kbtRecord2.KnowledgeBaseId).FirstOrDefault())
                           select new NCQADetails
                           {
                               PCMHSequence = Standardsequence,
                               ElementSequence = elementSequence,
                               FactorSequence = kbtRecord1.Sequence.ToString()
                           }).ToList();

            return factors;


        }

        public string GetNewQuestionnaire(int medicalGroupId)
        {
            try
            {
                // Get Fresh Questionnaire
                var questionnaire = (from questionRecord in BMTDataContext.Forms
                                     where questionRecord.Type == "NCQA"
                                     //&& questionRecord.MedicalGroupId == medicalGroupId
                                     select questionRecord).SingleOrDefault();

                string question = string.Empty;
                if (questionnaire != null)
                    question = Convert.ToString(questionnaire.Questionnaire);
                else
                    question = string.Empty;

                return question;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveFilledQuestionnaire(int questionnaireId, int projectUsageId, XElement answer, int userId)
        {
            try
            {
                var questionRecord = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                      where
                                      QuestionnaireRecord.ProjectUsageId == projectUsageId
                                      &&
                                      QuestionnaireRecord.FormId == questionnaireId
                                      select QuestionnaireRecord).SingleOrDefault();

                _filledQuestionnaire = new FilledQuestionnaire();

                if (questionRecord != null)
                {
                    questionRecord.Answers = answer;
                    questionRecord.LastUpdated = System.DateTime.Now;
                    questionRecord.UpdatedBy = userId;

                    BMTDataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    _filledQuestionnaire.FormId = questionnaireId;
                    _filledQuestionnaire.ProjectUsageId = projectUsageId;
                    _filledQuestionnaire.Answers = answer;
                    _filledQuestionnaire.UserId = userId;

                    BMTDataContext.FilledQuestionnaires.InsertOnSubmit(_filledQuestionnaire);
                    BMTDataContext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool SaveFilledQuestionnaire(int questionnaireId, int projectUsageId, int SiteId, XElement answer, int userId)
        {
            try
            {
                var questionRecord = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                      where
                                      QuestionnaireRecord.ProjectUsageId == projectUsageId
                                      && QuestionnaireRecord.PracticeSiteId == SiteId &&
                                      QuestionnaireRecord.FormId == questionnaireId
                                      select QuestionnaireRecord).SingleOrDefault();

                _filledQuestionnaire = new FilledQuestionnaire();

                if (questionRecord != null)
                {
                    questionRecord.Answers = answer;
                    questionRecord.LastUpdated = System.DateTime.Now;
                    questionRecord.UpdatedBy = userId;

                    BMTDataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    _filledQuestionnaire.FormId = questionnaireId;
                    _filledQuestionnaire.ProjectUsageId = projectUsageId;
                    _filledQuestionnaire.PracticeSiteId = SiteId;
                    _filledQuestionnaire.Answers = answer;
                    _filledQuestionnaire.UserId = userId;

                    BMTDataContext.FilledQuestionnaires.InsertOnSubmit(_filledQuestionnaire);
                    BMTDataContext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public List<AssetDetails> GetAssetTypes(int elementId)
        {
            try
            {

                return (from AssetTypesRecords in BMTDataContext.AssetTypes
                        where AssetTypesRecords.AssetCategoryId == elementId
                        group AssetTypesRecords by new
                        {
                            AssetTypesRecords.AssetParentId,
                            AssetTypesRecords.AssetTypeId,
                            AssetTypesRecords.Name,
                            AssetTypesRecords.ValueKey
                        } into groupby
                        select new AssetDetails
                            {
                                ValueKey = (int)groupby.Key.ValueKey,
                                Name = groupby.Key.Name
                            }).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DeleteAssessmentByProjectUsageAndSiteId(int questionaireId, int projectUsageId, int siteId)
        {
            try
            {
                var filledQuestionaire = (from questionnaireRecord in BMTDataContext.FilledQuestionnaires
                                          where
                                          questionnaireRecord.ProjectUsageId == projectUsageId
                                          && questionnaireRecord.PracticeSiteId == siteId &&
                                          questionnaireRecord.FormId == questionaireId
                                          select new { questionnaireRecord }).SingleOrDefault();

                if (filledQuestionaire != null)
                {
                    BMTDataContext.FilledQuestionnaires.DeleteOnSubmit(filledQuestionaire.questionnaireRecord);
                    BMTDataContext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return false;
        }
        #endregion

    }
}
