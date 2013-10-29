using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using BMTBLL.Enumeration;
using System.Configuration;
using System.Data;
using BMTBLL.Classes;
using System.Web;
namespace BMTBLL
{
    public class MOReBO : BMTConnection
    {

        #region CONSTRUCTOR
        public MOReBO()
        {

        }


        #endregion


        #region Functions

        public List<KnowledgeBase> GetKnowledgeBaseHeadersByTemplateId(int TemplateId)
        {
            try
            {
                List<KnowledgeBase> kbList = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                              join kb in BMTDataContext.KnowledgeBases
                                               on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
                                              where kbt.TemplateId == TemplateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Header)
                                              && kb.IsActive == true
                                              select kb).ToList<KnowledgeBase>();
                return kbList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<KnowledgeBase> ShowHeadersByTemplateId(int TemplateId)
        //{
        //    try
        //    {
        //        List<KnowledgeBase> kbList = new List<KnowledgeBase>();
        //        List<KnowledgeBase> AllHeaders = (from kbt in BMTDataContext.KnowledgeBaseTemplates
        //                                      join kb in BMTDataContext.KnowledgeBases
        //                                       on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
        //                                      where kbt.TemplateId == TemplateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Header)
        //                                      && kb.IsActive == true
        //                                      select kb).ToList<KnowledgeBase>();

        //        foreach (KnowledgeBase header in AllHeaders)
        //        {

        //            List<int> subHeaderId = (from kbt in BMTDataContext.KnowledgeBaseTemplates
        //                                     join kb in BMTDataContext.KnowledgeBases
        //                                      on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
        //                                     where kbt.TemplateId == TemplateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.SubHeader)
        //                                     && kbt.ParentKnowledgeBaseId == header.KnowledgeBaseId
        //                                     select kb.KnowledgeBaseId).ToList<int>();

        //            var checkQuestion = (from kbt in BMTDataContext.KnowledgeBaseTemplates
        //                                 join kb in BMTDataContext.KnowledgeBases
        //                                  on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
        //                                 where kbt.TemplateId == TemplateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Question)
        //                                 && subHeaderId.Contains(Convert.ToInt32(kbt.ParentKnowledgeBaseId))
        //                                 select kb.KnowledgeBaseId).ToList<int>();

        //            if (checkQuestion.Count != 0)
        //                kbList.Add(header);

        //        }

        //        return kbList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<KnowledgeBase> GetKnowledgeBaseSubHeadersByTemplateId(int TemplateId, int HeaderId)
        {
            try
            {
                BMTDataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, BMTDataContext.KnowledgeBases);
                List<KnowledgeBase> kbList = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                              join kb in BMTDataContext.KnowledgeBases
                                               on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
                                              where kbt.TemplateId == TemplateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.SubHeader)
                                              && kbt.ParentKnowledgeBaseId == HeaderId
                                              select kb).ToList<KnowledgeBase>();
                return kbList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KnowledgeBase GetKnowledgeBaseHeadersById(int HeaderId, int templateId)
        {
            try
            {
                KnowledgeBase kbase = (from kb in BMTDataContext.KnowledgeBases
                                       join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                       where kb.KnowledgeBaseId == HeaderId && kbt.TemplateId == templateId
                                       select kb).FirstOrDefault();
                return kbase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<KnowledgeBase> GetKnowledgeBaseQuestionsBySubHeader(int subHeaderId, int templateId)
        {
            try
            {
                List<KnowledgeBase> kbase = (from kb in BMTDataContext.KnowledgeBases
                                             join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                             where kbt.ParentKnowledgeBaseId == subHeaderId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Question)
                                             && kbt.TemplateId == templateId
                                             select kb).ToList<KnowledgeBase>();
                return kbase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FilledAnswer GetFilledAnswersByQuestions(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                FilledAnswer answer = (from kb in BMTDataContext.KnowledgeBases
                                       join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                       join fa in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fa.KnowledgeBaseTemplateId
                                       where kb.KnowledgeBaseId == QuestionId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Question)
                                       && kbt.TemplateId == templateId
                                       //&& fa.ProjectId == ProjectId
                                       select fa).FirstOrDefault();
                return answer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetAnswerWeightageByEnumId(int AnswerTypeEnumId, int knowledgeBaseTemplateId, int projectUsageId, int siteId)
        {
            try
            {
                int result = BMTDataContext.usp_Get_Answer_Type_Weightage(AnswerTypeEnumId, projectUsageId,siteId, knowledgeBaseTemplateId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable GetDropDownWeightageByQuestion(int QuestionId, int templateId)
        {
            try
            {
                int parentId = Convert.ToInt32((from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                where kbt.TemplateId == templateId && kbt.KnowledgeBaseId == QuestionId
                                                select kbt.ParentKnowledgeBaseId).FirstOrDefault());

                var value = (from question in BMTDataContext.KnowledgeBases
                             join kbt in BMTDataContext.KnowledgeBaseTemplates on question.KnowledgeBaseId equals kbt.KnowledgeBaseId
                             join atew in BMTDataContext.AnswerTypeWeightages on kbt.KnowledgeBaseTemplateId equals atew.KnowledgeBaseTemplateId
                             join ate in BMTDataContext.AnswerTypeEnums on atew.AnswerTypeEnumId equals ate.AnswerTypeEnumId
                             where question.KnowledgeBaseId == parentId && kbt.TemplateId == templateId
                             select new
                             {
                                 Value = atew.AnswerTypeEnumId,
                                 Text = ate.DiscreteValue
                             }).AsQueryable();
                return value.AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetDropDownRowsCountByQuestion(int QuestionId, int templateId)
        {
            try
            {
                List<int> count = (from kb in BMTDataContext.KnowledgeBases
                                   join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                   join at in BMTDataContext.AnswerTypes on kbt.AnswerTypeId equals at.AnswerTypeId
                                   join ate in BMTDataContext.AnswerTypeEnums on at.AnswerTypeId equals ate.AnswerTypeId
                                   where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                   select ate.AnswerTypeEnumId).ToList<int>();

                return count.Count();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetDropDownWeightageByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                int parentId = Convert.ToInt32((from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                where kbt.TemplateId == templateId && kbt.KnowledgeBaseId == QuestionId
                                                select kbt.ParentKnowledgeBaseId).FirstOrDefault());

                List<int> value = (from question in BMTDataContext.KnowledgeBases
                                   join kbt in BMTDataContext.KnowledgeBaseTemplates on question.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                   join atew in BMTDataContext.AnswerTypeWeightages on kbt.KnowledgeBaseTemplateId equals atew.KnowledgeBaseTemplateId
                                   join ate in BMTDataContext.AnswerTypeEnums on atew.AnswerTypeEnumId equals ate.AnswerTypeEnumId
                                   where question.KnowledgeBaseId == parentId && kbt.TemplateId == templateId
                                   select atew.Weightage).ToList<int>();

                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetQuestionCommentByKnowledgeBaseTemplateId(int KBTemplateId, int templateId, int projectId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                where kbt.KnowledgeBaseTemplateId == KBTemplateId && kbt.TemplateId == templateId
                                select kbt.IsDataBox.ToString()).FirstOrDefault();
                return (value != null && value.ToUpper() == "TRUE" ? "Yes" : "No");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetQuestionAnswerByQuestionId(int QuestionId, int templateId, int projectId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join fAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fAns.KnowledgeBaseTemplateId
                                join ate in BMTDataContext.AnswerTypeEnums on fAns.AnswerTypeEnumId equals ate.AnswerTypeEnumId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                //&& fAns.ProjectId == projectId
                                select ate.DiscreteValue).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetEvaluationNotesBySubHeaderId(int subHeaderId, int templateId, int projectUsageId, int siteId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == subHeaderId && kbt.TemplateId == templateId
                                && filledAns.ProjectUsageId == projectUsageId
                                && filledAns.PracticeSiteId == siteId
                                select filledAns.EvaluationNotes).FirstOrDefault();
                return (value != null ? value : "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetReviewNotesBySubHeaderId(int subHeaderId, int templateId, int projectUsageId,int siteId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == subHeaderId && kbt.TemplateId == templateId
                                && filledAns.ProjectUsageId == projectUsageId
                                && filledAns.PracticeSiteId==siteId
                                select filledAns.ReviewNotes).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FilledAnswer GetNotesBySubHeaderId(int subHeaderId, int templateId, int projectId)
        {
            try
            {
                FilledAnswer value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                      join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                      where kbt.KnowledgeBaseId == subHeaderId && kbt.TemplateId == templateId
                                      //&& filledAns.ProjectId == projectId
                                      select filledAns).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPoliciesDocumentCountByQuestionId(int QuestionId, int templateId, int projectId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                //&& filledAns.ProjectId == projectId
                                select filledAns.PoliciesDocumentCount.ToString()).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetReportsDocumentCountByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select filledAns.ReportsDocumentCount.ToString()).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLogsOrToolsDocumentCountByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select filledAns.LogsOrToolsDocumentCount.ToString()).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetScreenShotsDocumentCountByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select filledAns.ScreenShotsDocumentCount.ToString()).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetOtherDocumentCountByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select filledAns.OtherDocumentsCount.ToString()).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrivateNotesByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select filledAns.PrivateNotes).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetPoliciesDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                int value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             //join kbTD in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbTD.KnowledgeBaseTemplateId
                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                             join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                             where kbt.KnowledgeBaseId == QuestionId && tDocument.DocumentType == enDocType.Policies.ToString()
                             && kbt.TemplateId == templateId
                             //&& fA.ProjectId == ProjectId
                             select tDocument.DocumentId).Count();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetReportsDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                int value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             //join kbTD in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbTD.KnowledgeBaseTemplateId
                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                             join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                             where kbt.KnowledgeBaseId == QuestionId && tDocument.DocumentType == enDocType.Reports.ToString()
                             && kbt.TemplateId == templateId
                             //&& fA.ProjectId == ProjectId
                             select tDocument.DocumentId).Count();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetScreenshotsDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                int value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             //join kbTD in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbTD.KnowledgeBaseTemplateId
                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                             join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                             where kbt.KnowledgeBaseId == QuestionId && tDocument.DocumentType == enDocType.Screenshots.ToString()
                             && kbt.TemplateId == templateId
                             //&& fA.ProjectId == ProjectId
                             select tDocument.DocumentId).Count();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetLogsDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                int value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             //join kbTD in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbTD.KnowledgeBaseTemplateId
                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                             join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                             where kbt.KnowledgeBaseId == QuestionId && tDocument.DocumentType == enDocType.LogsOrTools.ToString()
                             && kbt.TemplateId == templateId
                             //&& fA.ProjectId == ProjectId
                             select tDocument.DocumentId).Count();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetOtherDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectId)
        {
            try
            {
                int value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             //join kbTD in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbTD.KnowledgeBaseTemplateId
                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                             join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                             where kbt.KnowledgeBaseId == QuestionId && tDocument.DocumentType == enDocType.OtherDocs.ToString()
                             && kbt.TemplateId == templateId
                             //&& fA.ProjectId == ProjectId
                             select tDocument.DocumentId).Count();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetQuestionNoteByQuestionId(int QuestionId, int templateId)
        {
            try
            {
                string value = (from kb in BMTDataContext.KnowledgeBases
                                join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                select kbt.DataBoxHeader).FirstOrDefault();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxPointsBySubHeaderId(int SubHeaderId, int templateId)
        {
            try
            {
                int points = (from kb in BMTDataContext.KnowledgeBases
                              join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                              where kb.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                              select Convert.ToInt32(kbt.MaxPoints)).FirstOrDefault();
                return points;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<ScoringRule> GetScoringRulesBySubHeaderId(int SubHeaderId, int templateId)
        {
            try
            {
                List<ScoringRule> rule = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                          join score in BMTDataContext.ScoringRules on kbt.KnowledgeBaseTemplateId equals score.KnowledgeBaseTemplateId
                                          where kbt.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                                          select score).ToList<ScoringRule>();
                return rule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDocumentPathBySubHeaderId(int SubHeaderId, int templateId, int ProjectId)
        {
            try
            {

                string rule = (from kb in BMTDataContext.KnowledgeBases
                               join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                               join ftd in BMTDataContext.FilledTemplateDocuments on kbt.KnowledgeBaseTemplateId equals ftd.KnowledgeBaseTemplateId
                               join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                               where kbt.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                               select td.Path).FirstOrDefault();
                return rule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTemplateStDoc(int templateId)
        {
            try
            {

                string path = (from temp in BMTDataContext.Templates
                               where temp.TemplateId == templateId
                               select temp.DocPath).FirstOrDefault();
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TemplateDocument> GetDocumentBySubHeaderId(int SubHeaderId, int templateId, int ProjectUsageId, int siteId)
        {
            try
            {
                List<TemplateDocument> finalList = new List<TemplateDocument>();
                List<KnowledgeBase> kb = GetKnowledgeBaseQuestionsBySubHeader(SubHeaderId, templateId);

                foreach (KnowledgeBase kBase in kb)
                {

                    List<TemplateDocument> document = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                       //join Tdoc in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals Tdoc.KnowledgeBaseTemplateId
                                                       join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                       join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                       join doc in BMTDataContext.TemplateDocuments on ftd.DocumentId equals doc.DocumentId
                                                       where
                                                       fA.ProjectUsageId == ProjectUsageId &&
                                                       fA.PracticeSiteId == siteId
                                                       && kbt.TemplateId == templateId && kbt.KnowledgeBaseId == kBase.KnowledgeBaseId
                                                       select doc).ToList<TemplateDocument>();

                    foreach (TemplateDocument doc in document)
                    {

                        finalList.Add(doc);
                    }
                }

                return finalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SaveEvaluationNotes(int TemplateId, int subHeaderId, string EvaluationNotes, int UserId, int projectUsageId, int siteId)
        {
            try
            {
                FilledAnswer FilledAnswers = (from kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                              join fAnswers in BMTDataContext.FilledAnswers on kbTemplate.KnowledgeBaseTemplateId equals fAnswers.KnowledgeBaseTemplateId
                                              where kbTemplate.KnowledgeBaseId == subHeaderId && kbTemplate.TemplateId == TemplateId
                                             && fAnswers.ProjectUsageId == projectUsageId
                                             && fAnswers.PracticeSiteId == siteId
                                              select fAnswers).FirstOrDefault();

                FilledAnswers.EvaluationNotes = EvaluationNotes;
                FilledAnswers.UpdatedBy = UserId;
                FilledAnswers.LastUpdated = System.DateTime.Now;


                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFactorNotes(int TemplateId, int subHeaderId, string FactorNotes, int projectUsageId, int siteId)
        {
            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fa in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fa.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == subHeaderId && kbt.TemplateId == TemplateId
                                        && fa.ProjectUsageId == projectUsageId
                                        && fa.PracticeSiteId == siteId
                                        select fa).FirstOrDefault();

                fAnswer.DataBoxComments = FactorNotes;


                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrivateNotes(int TemplateId, int HeaderId, int SubHeaderSequence, int QuestionSequence, int projectUsageId, int siteId)
        {
            try
            {
                List<KnowledgeBase> knBase = (from kbase in BMTDataContext.KnowledgeBases
                                              join kbTemplate in BMTDataContext.KnowledgeBaseTemplates on kbase.KnowledgeBaseId equals kbTemplate.KnowledgeBaseId
                                              where kbTemplate.ParentKnowledgeBaseId == HeaderId && kbTemplate.TemplateId == TemplateId
                                              select kbase).ToList<KnowledgeBase>();

                List<KnowledgeBase> QuestionList = (from kbase in BMTDataContext.KnowledgeBases
                                                    join kbTemplate in BMTDataContext.KnowledgeBaseTemplates on kbase.KnowledgeBaseId equals kbTemplate.KnowledgeBaseId
                                                    where kbTemplate.ParentKnowledgeBaseId == knBase[SubHeaderSequence].KnowledgeBaseId && kbTemplate.TemplateId == TemplateId
                                                    select kbase).ToList<KnowledgeBase>();

                string fAnswer = (from fa in BMTDataContext.FilledAnswers
                                  join kbt in BMTDataContext.KnowledgeBaseTemplates
                                      on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                  where kbt.KnowledgeBaseId == QuestionList[QuestionSequence].KnowledgeBaseId
                                  && fa.ProjectUsageId == projectUsageId
                                  && fa.PracticeSiteId == siteId
                                  select fa.PrivateNotes).FirstOrDefault();

                return fAnswer;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrivateNotes(int TemplateId, int questionId, int projectUsageId, int siteId)
        {
            try
            {

                string fAnswer = (from fa in BMTDataContext.FilledAnswers
                                  join kbt in BMTDataContext.KnowledgeBaseTemplates
                                      on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                  where kbt.KnowledgeBaseId == questionId && kbt.TemplateId == TemplateId
                                  && fa.ProjectUsageId == projectUsageId
                                  && fa.PracticeSiteId == siteId
                                  select fa.PrivateNotes).FirstOrDefault();

                return fAnswer;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SavePrivateNotes(int TemplateId, int QuestionId, string Note, int projectUsageId,int siteId)
        {
            try
            {

                FilledAnswer fAnswer = (from fa in BMTDataContext.FilledAnswers
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                            on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                        where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == TemplateId
                                        && fa.ProjectUsageId == projectUsageId
                                        && fa.PracticeSiteId==siteId
                                        select fa).FirstOrDefault();

                fAnswer.PrivateNotes = Note;

                BMTDataContext.SubmitChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveCompleteBySubHeaderId(int SubHeaderId, int templateId, bool Complete, int projectUsageId, int siteId)
        {
            try
            {

                FilledAnswer fAnswers = (from kb in BMTDataContext.KnowledgeBases
                                         join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                         join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                         where kb.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                                         && fA.ProjectUsageId == projectUsageId
                                         && fA.PracticeSiteId == siteId
                                         select fA).FirstOrDefault();

                if (fAnswers != null)
                {
                    fAnswers.Complete = Complete;
                    fAnswers.AnswerTypeEnumId = 1;
                }


                BMTDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetIsCompleteBySubHeaderId(int SubHeaderId, int templateId, int projectId)
        {
            try
            {

                bool complete = Convert.ToBoolean((from kb in BMTDataContext.KnowledgeBases
                                                   join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                   join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                   where kb.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                                                   //&& fA.ProjectId == projectId
                                                   select fA.Complete).FirstOrDefault());

                return complete;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveReviewerNotesBySubHeaderId(int SubHeaderId, int templateId, string ReviewerNotes, int projectId)
        {
            try
            {

                FilledAnswer fA = (from kb in BMTDataContext.KnowledgeBases
                                   join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                   join fAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fAns.KnowledgeBaseTemplateId
                                   where kb.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                                   //&& fAns.ProjectId == projectId
                                   select fAns).FirstOrDefault();

                fA.ReviewNotes = ReviewerNotes;

                BMTDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveAnswers(int QuestionId, int templateId, string Text, int projectUsageId, int siteId)
        {
            try
            {

                int parentId = Convert.ToInt32((from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                where kbt.TemplateId == templateId && kbt.KnowledgeBaseId == QuestionId
                                                select kbt.ParentKnowledgeBaseId).FirstOrDefault());

                int answerType = (from kbase in BMTDataContext.KnowledgeBases
                                  join kbTemplate in BMTDataContext.KnowledgeBaseTemplates on kbase.KnowledgeBaseId equals kbTemplate.KnowledgeBaseId
                                  join atw in BMTDataContext.AnswerTypeWeightages on kbTemplate.KnowledgeBaseTemplateId equals atw.KnowledgeBaseTemplateId
                                  join ate in BMTDataContext.AnswerTypeEnums on atw.AnswerTypeEnumId equals ate.AnswerTypeEnumId
                                  where kbase.KnowledgeBaseId == parentId && ate.DiscreteValue == Text && kbTemplate.TemplateId == templateId
                                  select ate.AnswerTypeEnumId).FirstOrDefault();

                FilledAnswer fAnswer = (from fa in BMTDataContext.FilledAnswers
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                            on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                        where kbt.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        && fa.ProjectUsageId == projectUsageId
                                        && fa.PracticeSiteId == siteId
                                        select fa).FirstOrDefault();

                if (fAnswer != null)
                {
                    fAnswer.AnswerTypeEnumId = answerType;
                }


                BMTDataContext.SubmitChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveRequiredPolicies(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                if (fAnswer != null)
                {
                    fAnswer.PoliciesDocumentCount = Convert.ToInt32(Value);
                }
                else
                {
                    int kbtid = (from kb in BMTDataContext.KnowledgeBases
                                 join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                 where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                 select kbt.KnowledgeBaseTemplateId).FirstOrDefault();

                    FilledAnswer fa = new FilledAnswer();

                    //fa.ProjectId = projectId;
                    fa.UpdatedBy = Convert.ToInt32(HttpContext.Current.Session[enSessionKey.UserApplicationId.ToString()]);
                    fa.LastUpdated = System.DateTime.Now;
                    fa.KnowledgeBaseTemplateId = kbtid;

                    BMTDataContext.FilledAnswers.InsertOnSubmit(fa);
                }
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveReportDocuments(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                fAnswer.ReportsDocumentCount = Convert.ToInt32(Value);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveScreenShotDocuments(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                fAnswer.ScreenShotsDocumentCount = Convert.ToInt32(Value);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveLogsOrToolsDocuments(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                fAnswer.LogsOrToolsDocumentCount = Convert.ToInt32(Value);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveOtherDocuments(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                fAnswer.OtherDocumentsCount = Convert.ToInt32(Value);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveScore(int QuestionId, int templateId, string Value, int projectId)
        {

            try
            {

                FilledAnswer fAnswer = (from kBase in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates on kBase.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kBase.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        //&& fA.ProjectId == projectId
                                        select fA).FirstOrDefault();

                fAnswer.DefaultScore = Value;

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveScoreAndNotes(int SubHeaderId, int templateId, string score, string notes, int projectUsageId, int siteId)
        {

            try
            {

                FilledAnswer fAnswer = (from kBase in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates on kBase.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kBase.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == templateId
                                        && fA.ProjectUsageId == projectUsageId
                                        && fA.PracticeSiteId == siteId
                                        select fA).FirstOrDefault();

                fAnswer.DefaultScore = score;
                fAnswer.ReviewNotes = notes;

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetQuestionNameAndSequence(int documentId, int templateId, int subHeaderId, int ProjectUsageId,int siteId)
        {
            try
            {

                string questionTitle = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                        where ftd.DocumentId == documentId && kbt.TemplateId == templateId
                                        && fA.ProjectUsageId == ProjectUsageId
                                        && fA.PracticeSiteId==siteId
                                        select kb.Name).FirstOrDefault();


                List<string> Questions = (from kb in BMTDataContext.KnowledgeBases
                                          join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                          where kbt.ParentKnowledgeBaseId == subHeaderId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Question)
                                          && kbt.TemplateId == templateId
                                          select kb.Name).ToList<string>();

                int questionSequence = Questions.IndexOf(questionTitle);


                return questionSequence.ToString() + "~" + questionTitle;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetDocumentLinks(string documentPath, int templateId, string headerSequence, string subHeaderSequence, string questionSequence, int ProjectUsageId,int siteId)
        {

            try
            {
                string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

                string returnValue = string.Empty;


                List<int> docList = (from kb in BMTDataContext.KnowledgeBases
                                     join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                     join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                     join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                     join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                                     where td.Path == documentPath && kbt.TemplateId == templateId
                                     && fA.ProjectUsageId == ProjectUsageId
                                     && fA.PracticeSiteId==siteId
                                     select kb.KnowledgeBaseId).ToList<int>();


                if (docList != null)
                {

                    if (docList.Count() > 1)
                    {
                        List<int> allHeaders = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                join kb in BMTDataContext.KnowledgeBases
                                                 on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
                                                where kbt.TemplateId == templateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Header)
                                                && kb.IsActive == true
                                                select kb.KnowledgeBaseId).ToList<int>();

                        foreach (int kbase in docList)
                        {
                            int subHeader = Convert.ToInt32((from kb in BMTDataContext.KnowledgeBases
                                                             //join kbs in BMTDataContext.KnowledgeBases on kb.KnowledgeBaseId equals kbs.ParentKnowledgeBaseId
                                                             join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                             //where kbt.TemplateId == templateId && kbs.KnowledgeBaseId == kbase
                                                             where kbt.TemplateId == templateId && kbt.KnowledgeBaseId == kbase
                                                             select kbt.ParentKnowledgeBaseId).FirstOrDefault());

                            int header = Convert.ToInt32((from kb in BMTDataContext.KnowledgeBases
                                                          //join kbs in BMTDataContext.KnowledgeBases on kb.KnowledgeBaseId equals kbs.ParentKnowledgeBaseId
                                                          join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                          //where kbt.TemplateId == templateId && kbs.KnowledgeBaseId == subHeader
                                                          where kbt.TemplateId == templateId && kbt.KnowledgeBaseId == subHeader
                                                          select kbt.ParentKnowledgeBaseId).FirstOrDefault());


                            List<int> allSubHeaders = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                       join kb in BMTDataContext.KnowledgeBases
                                                        on kbt.KnowledgeBaseId equals kb.KnowledgeBaseId
                                                       where kbt.TemplateId == templateId && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.SubHeader)
                                                       && kbt.ParentKnowledgeBaseId == header
                                                       select kb.KnowledgeBaseId).ToList<int>();

                            List<int> allQuestions = (from kb in BMTDataContext.KnowledgeBases
                                                      join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                      where kbt.ParentKnowledgeBaseId == subHeader && kb.KnowledgeBaseTypeId == Convert.ToInt32(enKnowledgeBaseType.Question)
                                                      && kbt.TemplateId == templateId
                                                      select kb.KnowledgeBaseId).ToList<int>();

                            int HeaderSequence = allHeaders.IndexOf(header) + 1;
                            if (HeaderSequence != 0)
                            {
                                int SubHeaderSequence = allSubHeaders.IndexOf(subHeader) + 1;
                                if (SubHeaderSequence != 0)
                                {
                                    int QuestionSequence = allQuestions.IndexOf(kbase) + 1;
                                    if (QuestionSequence != 0)
                                    {
                                        returnValue += HeaderSequence + DEFAULT_LETTERS[Convert.ToInt32(SubHeaderSequence) - 1] + QuestionSequence + ",";
                                    }
                                }
                            }
                        }

                        return returnValue.Substring(0, returnValue.Length - 1);

                    }
                    else

                        return headerSequence + DEFAULT_LETTERS[Convert.ToInt32(subHeaderSequence) - 1] + questionSequence;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<KnowledgeBase> GetQuestionCriticalComments(int headerSequence, int subHeaderSequence, int templateId)
        {

            try
            {

                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);
                List<KnowledgeBase> subheader = GetKnowledgeBaseSubHeadersByTemplateId(templateId, Headers[headerSequence - 1].KnowledgeBaseId);
                List<KnowledgeBase> question = GetKnowledgeBaseQuestionsBySubHeader(subheader[subHeaderSequence - 1].KnowledgeBaseId, templateId);

                return question;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int GetQuestionIdBySequence(int headerSequence, int subHeaderSequence, int questionSequence, int templateId)
        {

            try
            {
                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);
                List<KnowledgeBase> subheader = GetKnowledgeBaseSubHeadersByTemplateId(templateId, Headers[headerSequence - 1].KnowledgeBaseId);
                List<KnowledgeBase> question = GetKnowledgeBaseQuestionsBySubHeader(subheader[subHeaderSequence - 1].KnowledgeBaseId, templateId);

                return question[questionSequence - 1].KnowledgeBaseId;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public void SaveTemplateDocumentByQuestionId(int templateId, string Path, string docName, string referencePage, string relevancyLevel,
            string docType, int ProjectUsageId,int siteId, string factorId)
        {
            try
            {
                //TemplateDocument Document = (from kb in BMTDataContext.KnowledgeBases
                //                             join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                //                             //join kbtdoc in BMTDataContext.KnowledgeBaseTemplateDocuments on kbt.KnowledgeBaseTemplateId equals kbtdoc.KnowledgeBaseTemplateId
                //                             join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                //                             join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                //                             join tdoc in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tdoc.DocumentId
                //                             where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId && tdoc.Path == Path && fA.ProjectId == ProjectId
                //                             select tdoc).FirstOrDefault();


                List<TemplateDocument> Document = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                   join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                   join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                   join tdoc in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tdoc.DocumentId
                                                   where kbt.TemplateId == templateId && tdoc.Path == Path
                                                   && fA.ProjectUsageId == ProjectUsageId 
                                                   && fA.PracticeSiteId==siteId
                                                   && kbt.Sequence == Convert.ToInt32(factorId)
                                                   select tdoc).ToList<TemplateDocument>();


                docType = docType.Replace("/", "Or");
                DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                docType = documentTypeMappingBO.GetOriginalDocumentType(docType);

                foreach (TemplateDocument Doc in Document)
                {
                    Doc.Name = docName;
                    if (referencePage != string.Empty)
                        Doc.ReferencePages = referencePage;
                    if (relevancyLevel != string.Empty)
                        Doc.RelevencyLevel = relevancyLevel;

                    Doc.DocumentType = docType;

                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SaveTemplateDocument(int templateId, string Path, string docName, string referencePage, string relevancyLevel, string docType,
            int QuestionSequenceId, int SubHeaderSequenceId, int HeaderSequenceId, int ProjectUsageId,int siteId)
        {

            if (docType != "Site Document")
            {

                int QuestionId = GetQuestionIdBySequence(HeaderSequenceId, SubHeaderSequenceId, QuestionSequenceId, templateId);

                TemplateDocument tDoc = new TemplateDocument();

                tDoc.Name = docName;
                tDoc.Path = Path;
                tDoc.ReferencePages = referencePage;
                tDoc.RelevencyLevel = relevancyLevel;
                tDoc.DocumentType = docType;

                BMTDataContext.TemplateDocuments.InsertOnSubmit(tDoc);
                BMTDataContext.SubmitChanges();

                int documentId = (from tempDoc in BMTDataContext.TemplateDocuments
                                  where tempDoc.Name == docName && tempDoc.Path == Path && tempDoc.ReferencePages == referencePage
                                      && tempDoc.RelevencyLevel == relevancyLevel && tempDoc.DocumentType == docType
                                  select tempDoc.DocumentId).FirstOrDefault();


                int fAId = (from kb in BMTDataContext.KnowledgeBases
                            join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                            join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                            where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                            && fA.ProjectUsageId == ProjectUsageId
                            && fA.PracticeSiteId==siteId
                            select fA.FilledAnswersId).FirstOrDefault();



                FilledTemplateDocument kbTemp = new FilledTemplateDocument();

                kbTemp.FilledAnswerId = fAId;
                kbTemp.DocumentId = documentId;

                BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(kbTemp);
                BMTDataContext.SubmitChanges();
            }
            else
            {

                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);

                TemplateDocument tDoc = new TemplateDocument();

                tDoc.Name = docName;
                tDoc.Path = Path;
                tDoc.ReferencePages = referencePage;
                tDoc.RelevencyLevel = relevancyLevel;
                tDoc.DocumentType = docType;

                BMTDataContext.TemplateDocuments.InsertOnSubmit(tDoc);
                BMTDataContext.SubmitChanges();

                int documentId = (from tempDoc in BMTDataContext.TemplateDocuments
                                  where tempDoc.Name == docName && tempDoc.Path == Path && tempDoc.ReferencePages == referencePage
                                      && tempDoc.RelevencyLevel == relevancyLevel && tempDoc.DocumentType == docType
                                  select tempDoc.DocumentId).FirstOrDefault();


                int fAId = (from kb in BMTDataContext.KnowledgeBases
                            join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                            join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                            where kb.KnowledgeBaseId == Headers[0].KnowledgeBaseId && kbt.TemplateId == templateId
                            //&& fA.ProjectId == ProjectId
                            select fA.FilledAnswersId).FirstOrDefault();

                if (fAId != 0)
                {
                    FilledTemplateDocument kbTemp = new FilledTemplateDocument();

                    kbTemp.FilledAnswerId = fAId;
                    kbTemp.DocumentId = documentId;

                    BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(kbTemp);
                    BMTDataContext.SubmitChanges();
                }
                else
                {
                    int kbtId = (from kb in BMTDataContext.KnowledgeBases
                                 join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                 where kb.KnowledgeBaseId == Headers[0].KnowledgeBaseId && kbt.TemplateId == templateId
                                 select kbt.KnowledgeBaseTemplateId).FirstOrDefault();


                    DateTime Date = System.DateTime.Now;
                    FilledAnswer fAns = new FilledAnswer();
                    fAns.KnowledgeBaseTemplateId = kbtId;
                    fAns.LastUpdated = Date;
                    fAns.UpdatedBy = Convert.ToInt32(HttpContext.Current.Session[enSessionKey.UserApplicationId.ToString()]);
                    //fAns.ProjectId = ProjectId;

                    BMTDataContext.FilledAnswers.InsertOnSubmit(fAns);
                    BMTDataContext.SubmitChanges();

                    int fAnsId = (from fA in BMTDataContext.FilledAnswers
                                  where fA.LastUpdated == Date
                                  select fA.FilledAnswersId).FirstOrDefault();

                    FilledTemplateDocument ftDoc = new FilledTemplateDocument();

                    ftDoc.FilledAnswerId = fAnsId;
                    ftDoc.DocumentId = documentId;

                    BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(ftDoc);
                    BMTDataContext.SubmitChanges();

                }
            }
        }


        public void UpdateTemplateDocument(int templateId, string Path, int ProjectUsageId,int siteId, string ExistingPath, string CurrentName)
        {


            List<TemplateDocument> tDoc = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                           join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                           join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                           join doc in BMTDataContext.TemplateDocuments on ftd.DocumentId equals doc.DocumentId
                                           where kbt.TemplateId == templateId
                                           && fA.ProjectUsageId == ProjectUsageId
                                           && fA.PracticeSiteId == siteId
                                           && doc.Path == ExistingPath
                                           select doc).ToList<TemplateDocument>();

            foreach (TemplateDocument doc in tDoc)
            {
                doc.Name = CurrentName;
                doc.Path = Path;
                BMTDataContext.SubmitChanges();
            }
        }

        public string GetDocumentPath(int templateId, string DocType, int ProjectUsageId,int siteId, string ExistingPath)
        {


            string Path = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                           join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                           join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                           join doc in BMTDataContext.TemplateDocuments on ftd.DocumentId equals doc.DocumentId
                           where kbt.TemplateId == templateId && doc.DocumentType == DocType
                           && fA.ProjectUsageId == ProjectUsageId
                           && fA.PracticeSiteId == siteId
                           && doc.Path == ExistingPath
                           select doc.Path).FirstOrDefault();
            return Path;

        }


        public List<NCQADetails> GetHeadersForDDLlist(int templateId, int ProjectUsageId,int siteId)
        {
            try
            {
                List<NCQADetails> _list = new List<NCQADetails>();

                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);

                int index = 1;

                foreach (KnowledgeBase header in Headers)
                {
                    List<KnowledgeBase> SubHeaders = GetKnowledgeBaseSubHeadersByTemplateId(templateId, header.KnowledgeBaseId);

                    foreach (KnowledgeBase subHeader in SubHeaders)
                    {


                        KnowledgeBaseTemplate kbtd = (from kb in BMTDataContext.KnowledgeBases
                                                      join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                      join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                      join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                      where
                                                      fA.ProjectUsageId == ProjectUsageId && 
                                                      fA.PracticeSiteId==siteId &&
                                                      kbt.ParentKnowledgeBaseId == subHeader.KnowledgeBaseId
                                                      select kbt).FirstOrDefault();
                        if (kbtd != null)
                        {
                            KnowledgeBaseTemplate parent = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                                                            where kbtRec.KnowledgeBaseId == kbtd.ParentKnowledgeBaseId
                                                            select kbtRec).FirstOrDefault();

                            int? grandparent = parent.ParentKnowledgeBaseId;
                            KnowledgeBase kb = (from kbRec in BMTDataContext.KnowledgeBases
                                                where kbRec.KnowledgeBaseId == grandparent
                                                select kbRec).FirstOrDefault();

                            _list.Add(new NCQADetails(index.ToString(), kb.Name.Split(':')[0]));
                            break;
                        }
                    }

                    index++;

                    List<int> count = (from kb in BMTDataContext.KnowledgeBases
                                       join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                       join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                       join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                       join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                                       where td.DocumentType == enDocType.UnAssociatedDoc.ToString() & kbt.TemplateId == templateId &&
                                       kb.KnowledgeBaseId == header.KnowledgeBaseId
                                       select td.DocumentId).ToList<int>();

                    if (count.Count > 0)
                    {
                        _list.Add(new NCQADetails("100", "UNASSOCIATED DOCUMENTS"));

                    }
                }

                return _list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<NCQADetails> GetSubHeadersForDDLlist(int templateId, int Sequence, int ProjectUsageId,int siteId)
        {
            try
            {
                List<NCQADetails> _list = new List<NCQADetails>();
                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);
                List<KnowledgeBase> SubHeaders = GetKnowledgeBaseSubHeadersByTemplateId(templateId, Headers[Sequence - 1].KnowledgeBaseId);

                int index = 1;

                foreach (KnowledgeBase _subHeader in SubHeaders)
                {
                    KnowledgeBaseTemplate kbtd = (from kb in BMTDataContext.KnowledgeBases
                                                  join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                  join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                  join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                  where
                                                  fA.ProjectUsageId == ProjectUsageId && 
                                                  fA.PracticeSiteId==siteId &&
                                                  kbt.ParentKnowledgeBaseId == _subHeader.KnowledgeBaseId
                                                  select kbt).FirstOrDefault();

                    if (kbtd != null)
                    {
                        int? parent = kbtd.ParentKnowledgeBaseId;

                        KnowledgeBase kb = (from kbRec in BMTDataContext.KnowledgeBases
                                            where kbRec.KnowledgeBaseId == parent
                                            select kbRec).FirstOrDefault();

                        _list.Add(new NCQADetails(index.ToString(), kb.Name.Split(':')[0]));

                    }

                    index++;
                }

                return _list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<NCQADetails> GetQuestionsForDDLlist(string standardSequence, string elementSequence, string currentFactor, string currentStandard,
            string currentElement, int templateId, int ProjectUsageId,int siteId)
        {

            try
            {
                List<NCQADetails> _list = new List<NCQADetails>();
                List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);
                List<KnowledgeBase> SubHeaders = GetKnowledgeBaseSubHeadersByTemplateId(templateId, Headers[Convert.ToInt32(standardSequence) - 1].KnowledgeBaseId);
                List<KnowledgeBase> Questions = GetKnowledgeBaseQuestionsBySubHeader(SubHeaders[Convert.ToInt32(elementSequence) - 1].KnowledgeBaseId, templateId);

                int index = 1;
                foreach (KnowledgeBase _question in Questions)
                {
                    if (index.ToString() != currentFactor || standardSequence != currentStandard || elementSequence != currentElement)
                    {
                        FilledTemplateDocument kbtd = (from kb in BMTDataContext.KnowledgeBases
                                                       join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                       join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                       join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                       where
                                                       fA.ProjectUsageId == ProjectUsageId &&
                                                       fA.PracticeSiteId==siteId &&
                                                       kbt.KnowledgeBaseId == _question.KnowledgeBaseId
                                                       select ftd).FirstOrDefault();

                        if (kbtd != null)
                        {
                            _list.Add(new NCQADetails(index.ToString(), "Factor"));

                        }

                    }

                    index++;
                }

                return _list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<NCQADetails> GetDocumentList(string standardSequence, string elementSequence, string factorSequence, int templateId, int ProjectUsageId,int siteId)
        {
            try
            {
                List<NCQADetails> _list = new List<NCQADetails>();
                string fileName = string.Empty;

                if (standardSequence != "100")
                {

                    int QuestionId = GetQuestionIdBySequence(Convert.ToInt32(standardSequence), Convert.ToInt32(elementSequence), Convert.ToInt32(factorSequence), templateId);

                    List<KnowledgeBase> Question = (from kb in BMTDataContext.KnowledgeBases
                                                    where kb.KnowledgeBaseId == QuestionId
                                                        && kb.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Question
                                                    select kb).ToList<KnowledgeBase>();
                    foreach (KnowledgeBase _question in Question)
                    {

                        List<TemplateDocument> tempDoc = (from kb in BMTDataContext.KnowledgeBases
                                                          join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                          join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                          join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                          join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                                                          where
                                                          fA.ProjectUsageId == ProjectUsageId && 
                                                          fA.PracticeSiteId==siteId &&
                                                          kb.KnowledgeBaseId == _question.KnowledgeBaseId
                                                          select td).ToList<TemplateDocument>();

                        if (tempDoc != null)
                        {

                            foreach (TemplateDocument td in tempDoc)
                            {

                                if (td.Name == "" || td.Name == null)
                                {
                                    int startIndex = td.Path.LastIndexOf('/') + 1;
                                    int length = td.Path.LastIndexOf('.') - startIndex;
                                    fileName = td.Path.Substring(startIndex, length);
                                }
                                else
                                    fileName = td.Name.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "}").Replace("dotsign", ".");
                                _list.Add(new NCQADetails(factorSequence, fileName, td.Path));
                            }

                        }
                    }
                }

                else
                {

                    List<KnowledgeBase> Headers = GetKnowledgeBaseHeadersByTemplateId(templateId);

                    List<TemplateDocument> tempDoc = (from kb in BMTDataContext.KnowledgeBases
                                                      join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                      join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                      join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                      join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                                                      where
                                                      fA.ProjectUsageId == ProjectUsageId &&
                                                      fA.PracticeSiteId==siteId &&
                                                      kb.KnowledgeBaseId == Headers[0].KnowledgeBaseId
                                                      && td.DocumentType == enDocType.UnAssociatedDoc.ToString()
                                                      select td).ToList<TemplateDocument>();

                    if (tempDoc != null)
                    {

                        foreach (TemplateDocument td in tempDoc)
                        {
                            fileName = td.Name.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "}").Replace("dotsign", ".");
                            _list.Add(new NCQADetails(factorSequence, fileName, td.Path));
                        }

                    }

                }

                return _list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveLinkedDocument(string savingDestinationPath, string pcmhId, string elementId, string factorId, string docType,
                        string fileTitle, string referencePage, string relevancyLevel, string projectUsageId, string selectedStandard,
                        string practiceName, string siteName, string node, string siteId, string practiceId, int templateId)
        {

            try
            {

                int QuestionId = GetQuestionIdBySequence(Convert.ToInt32(pcmhId), Convert.ToInt32(elementId), Convert.ToInt32(factorId), templateId);

                TemplateDocument tDoc = new TemplateDocument();

                tDoc.Name = fileTitle;
                tDoc.Path = savingDestinationPath;
                tDoc.RelevencyLevel = relevancyLevel;
                tDoc.ReferencePages = referencePage;


                //docType = docType.Replace("/", "Or");
                //DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                //docType = documentTypeMappingBO.GetOriginalDocumentType(docType);

                tDoc.DocumentType = docType;

                BMTDataContext.TemplateDocuments.InsertOnSubmit(tDoc);
                BMTDataContext.SubmitChanges();

                int docId = (from tdoc in BMTDataContext.TemplateDocuments
                             where tdoc.Path == savingDestinationPath
                                 && tdoc.Name == fileTitle && tdoc.RelevencyLevel == relevancyLevel && tdoc.ReferencePages == referencePage
                                 && tdoc.DocumentType == docType
                             orderby tdoc.DocumentId descending
                             select tdoc.DocumentId).FirstOrDefault();

                int fAId = (from kb in BMTDataContext.KnowledgeBases
                            join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                            join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                            where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                            && fA.ProjectUsageId == Convert.ToInt32(projectUsageId)
                            && fA.PracticeSiteId==Convert.ToInt32(siteId)
                            select fA.FilledAnswersId).FirstOrDefault();



                FilledTemplateDocument kbtDoc = new FilledTemplateDocument();
                kbtDoc.DocumentId = docId;
                kbtDoc.FilledAnswerId = fAId;

                BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(kbtDoc);
                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetFileReferences(string path)
        {
            try
            {
                List<int> docs = (from tDoc in BMTDataContext.TemplateDocuments where tDoc.Path == path select tDoc.DocumentId).ToList<int>();

                return docs.Count() - 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void DeleteFile(string oldPath, string newPath, int templateId, int ProjectUsageId,int siteId)
        {

            try
            {
                List<TemplateDocument> Documents = (from tDoc in BMTDataContext.TemplateDocuments where tDoc.Path == oldPath select tDoc).ToList<TemplateDocument>();

                foreach (TemplateDocument Doc in Documents)
                {


                    List<FilledTemplateDocument> KBTDoc = (from ftd in BMTDataContext.FilledTemplateDocuments
                                                           join fA in BMTDataContext.FilledAnswers on ftd.FilledAnswerId equals fA.FilledAnswersId
                                                           join kbt in BMTDataContext.KnowledgeBaseTemplates on fA.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                                           where kbt.TemplateId == templateId && kbt.TemplateId == templateId
                                                           && fA.ProjectUsageId == ProjectUsageId &&
                                                           fA.PracticeSiteId==siteId
                                                           && ftd.DocumentId == Doc.DocumentId
                                                           select ftd).ToList<FilledTemplateDocument>();


                    foreach (FilledTemplateDocument KBD in KBTDoc)
                    {

                        BMTDataContext.FilledTemplateDocuments.DeleteOnSubmit(KBD);
                        BMTDataContext.SubmitChanges();
                    }


                    Doc.Path = newPath;
                    Doc.DocumentType = enDocType.UnAssociatedDoc.ToString();
                    BMTDataContext.SubmitChanges();

                    List<KnowledgeBase> Header = GetKnowledgeBaseHeadersByTemplateId(templateId);
                    int HeaderId = Header[0].KnowledgeBaseId;

                    int fAId = (from kb in BMTDataContext.KnowledgeBases
                                join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                where kb.KnowledgeBaseId == HeaderId && kbt.TemplateId == templateId
                                && fA.ProjectUsageId == ProjectUsageId &&
                                fA.PracticeSiteId==siteId
                                select fA.FilledAnswersId).FirstOrDefault();

                    if (fAId != 0)
                    {
                        FilledTemplateDocument ftDoc = new FilledTemplateDocument();

                        ftDoc.FilledAnswerId = fAId;
                        ftDoc.DocumentId = Doc.DocumentId;

                        BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(ftDoc);
                        BMTDataContext.SubmitChanges();
                    }
                    else
                    {
                        int kbtId = (from kb in BMTDataContext.KnowledgeBases
                                     join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                     where kb.KnowledgeBaseId == HeaderId && kbt.TemplateId == templateId
                                     select kbt.KnowledgeBaseTemplateId).FirstOrDefault();


                        FilledAnswer fAns = new FilledAnswer();
                        fAns.KnowledgeBaseTemplateId = kbtId;
                        fAns.LastUpdated = System.DateTime.Now;
                        fAns.UpdatedBy = Convert.ToInt32(HttpContext.Current.Session[enSessionKey.UserApplicationId.ToString()]);
                        fAns.ProjectUsageId = ProjectUsageId;
                        fAns.PracticeSiteId =siteId;

                        BMTDataContext.FilledAnswers.InsertOnSubmit(fAns);
                        BMTDataContext.SubmitChanges();

                        int fAnsId = (from kb in BMTDataContext.KnowledgeBases
                                      join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                      join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                      where kb.KnowledgeBaseId == HeaderId && kbt.TemplateId == templateId
                                      && fA.ProjectUsageId == ProjectUsageId &&
                                      fA.PracticeSiteId==siteId
                                      select fA.FilledAnswersId).FirstOrDefault();

                        FilledTemplateDocument ftDoc = new FilledTemplateDocument();

                        ftDoc.FilledAnswerId = fAnsId;
                        ftDoc.DocumentId = Doc.DocumentId;

                        BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(ftDoc);
                        BMTDataContext.SubmitChanges();

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public FilledAnswer GetFilledAnswersByKnowledgeBase(int knowledgeBaseId, int templateId, int ProjectUsageId,int siteId)
        {

            try
            {
                FilledAnswer fA = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                   join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                   where kbt.KnowledgeBaseId == knowledgeBaseId && kbt.TemplateId == templateId
                                   && filledAns.ProjectUsageId == ProjectUsageId
                                   && filledAns.PracticeSiteId==siteId
                                   select filledAns).FirstOrDefault();

                return fA;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<FilledAnswer> GetFilledAnswersBySubHeader(int subHeaderId, int templateId, int ProjectId)
        {

            try
            {
                List<FilledAnswer> fA = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                         join filledAns in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals filledAns.KnowledgeBaseTemplateId
                                         where kbt.ParentKnowledgeBaseId == subHeaderId && kbt.TemplateId == templateId
                                         //&& filledAns.ProjectId == ProjectId
                                         select filledAns).ToList<FilledAnswer>();

                return fA;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetKnowledgeBaseParentId(int KnowledgeBaseId, int TemplateId)
        {
            try
            {
                return Convert.ToInt32((from kbt in BMTDataContext.KnowledgeBaseTemplates
                                        where kbt.KnowledgeBaseId == KnowledgeBaseId && kbt.TemplateId == TemplateId
                                        select kbt.ParentKnowledgeBaseId).FirstOrDefault());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<int> CheckFilledAnswers(int TemplateId, int ProjectUsageId, int siteId, int HeaderId)
        {
            List<int> ListOfSubHeaders = new List<int>();

            List<int> subHeaders = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                    where kbt.ParentKnowledgeBaseId == HeaderId && kbt.TemplateId == TemplateId
                                    select kbt.KnowledgeBaseId).ToList<int>();

            foreach (int subheader in subHeaders)
            {
                int fAns = (from fa in BMTDataContext.FilledAnswers
                            join kbt in BMTDataContext.KnowledgeBaseTemplates on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                            where kbt.TemplateId == TemplateId
                            && fa.ProjectUsageId == ProjectUsageId
                            && fa.PracticeSiteId == siteId
                            && kbt.KnowledgeBaseId == subheader
                            select fa.FilledAnswersId).FirstOrDefault();

                if (fAns == 0)
                    ListOfSubHeaders.Add(subheader);

            }

            return ListOfSubHeaders;

        }

        public List<int> CheckFilledAnswersQuestions(int TemplateId, int ProjectUsageId, int siteId, int HeaderId)
        {

            try
            {
                List<int> ListOfQuestions = new List<int>();

                List<int> subHeaders = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                        where kbt.ParentKnowledgeBaseId == HeaderId && kbt.TemplateId == TemplateId
                                        select kbt.KnowledgeBaseId).ToList<int>();


                foreach (int subheader in subHeaders)
                {

                    List<int> Questions = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                           where kbt.ParentKnowledgeBaseId == subheader && kbt.TemplateId == TemplateId
                                           select kbt.KnowledgeBaseId).ToList<int>();
                    foreach (int question in Questions)
                    {
                        int fAns = (from fa in BMTDataContext.FilledAnswers
                                    join kbt in BMTDataContext.KnowledgeBaseTemplates on fa.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                    where kbt.TemplateId == TemplateId
                                    && fa.ProjectUsageId == ProjectUsageId
                                    && fa.PracticeSiteId == siteId
                                    && kbt.KnowledgeBaseId == question
                                    select fa.FilledAnswersId).FirstOrDefault();

                        if (fAns == 0)
                            ListOfQuestions.Add(question);
                    }

                }


                return ListOfQuestions;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void SaveFilledAnswerForBlankTemplates(int TemplateId, int ProjectUsageId, int SiteId, int ParentKBId, bool InsertParent)
        {

            int result = BMTDataContext.usp_Save_FilledAnswers(TemplateId, ProjectUsageId,SiteId,ParentKBId, InsertParent);

        }

        public TemplateDocument GetDocumentByPath(string Path)
        {

            TemplateDocument Doc = (from tdoc in BMTDataContext.TemplateDocuments where tdoc.Path == Path select tdoc).FirstOrDefault();

            return (Doc != null ? Doc : null);
        }

        public void DeleteDocument(int DocumentId)
        {

            try
            {
                FilledTemplateDocument fAns = (from fA in BMTDataContext.FilledTemplateDocuments where fA.DocumentId == DocumentId select fA).FirstOrDefault();

                BMTDataContext.FilledTemplateDocuments.DeleteOnSubmit(fAns);
                BMTDataContext.SubmitChanges();

                TemplateDocument Doc = (from tdoc in BMTDataContext.TemplateDocuments where tdoc.DocumentId == DocumentId select tdoc).FirstOrDefault();

                BMTDataContext.TemplateDocuments.DeleteOnSubmit(Doc);
                BMTDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetDefaultScore(int SubHeaderId, int TemplateId, int ProjectUsageId, int siteId)
        {

            try
            {
                string score = (from kb in BMTDataContext.KnowledgeBases
                                join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                where kb.KnowledgeBaseId == SubHeaderId && kbt.TemplateId == TemplateId
                                && fA.ProjectUsageId == ProjectUsageId
                                && fA.PracticeSiteId == siteId
                                select fA.DefaultScore).FirstOrDefault();

                return (score != null && score != "" ? score : "0%");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int GetDocumentId(int questionId, int templateId, int projectUsageId,int siteId, string path)
        {

            try
            {
                int docId = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             join fa in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fa.KnowledgeBaseTemplateId
                             join ftd in BMTDataContext.FilledTemplateDocuments on fa.FilledAnswersId equals ftd.FilledAnswerId
                             join td in BMTDataContext.TemplateDocuments on ftd.DocumentId equals td.DocumentId
                             where kbt.KnowledgeBaseId == questionId && kbt.TemplateId == templateId
                             && fa.ProjectUsageId == projectUsageId && fa.PracticeSiteId==siteId
                             && td.Path == path
                             select td.DocumentId).FirstOrDefault();

                return docId;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetTemplateName(int templateId)
        {

            try
            {
                string name = (from template in BMTDataContext.Templates where template.TemplateId == templateId select template.Name).SingleOrDefault();

                return name;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetTemplateId(string templateName)
        {

            try
            {
                int id = (from template in BMTDataContext.Templates where template.Name == templateName select template.TemplateId).SingleOrDefault();

                return id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Template GetTemplateInfo(int templateId)
        {

            try
            {
                Template name = (from template in BMTDataContext.Templates where template.TemplateId == templateId select template).SingleOrDefault();

                return name;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public KnowledgeBaseTemplate GetKnowledgeBaseTemplate(int QuestionId, int templateId)
        {

            try
            {
                int parentId = Convert.ToInt32((from kb in BMTDataContext.KnowledgeBases
                                                join kbt in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                                where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                                select kbt.ParentKnowledgeBaseId).FirstOrDefault());

                KnowledgeBaseTemplate kbtemp = (from kb in BMTDataContext.KnowledgeBases
                                                join kbtemplate in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbtemplate.KnowledgeBaseId
                                                where kbtemplate.ParentKnowledgeBaseId == parentId && kbtemplate.TemplateId == templateId
                                                select kbtemplate).FirstOrDefault();

                return kbtemp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public KnowledgeBaseTemplate GetKnowledgeBaseTemplateByQuestionId(int QuestionId, int templateId)
        {

            try
            {

                KnowledgeBaseTemplate kbtemp = (from kb in BMTDataContext.KnowledgeBases
                                                join kbtemplate in BMTDataContext.KnowledgeBaseTemplates on kb.KnowledgeBaseId equals kbtemplate.KnowledgeBaseId
                                                where kbtemplate.KnowledgeBaseId == QuestionId && kbtemplate.TemplateId == templateId
                                                select kbtemplate).FirstOrDefault();

                return kbtemp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TemplateDocument> GetDocumentUplodedCountByQuestionId(int QuestionId, int templateId, int ProjectUsageId, int siteId)
        {
            try
            {
                List<TemplateDocument> value = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                                join ftd in BMTDataContext.FilledTemplateDocuments on fA.FilledAnswersId equals ftd.FilledAnswerId
                                                join tDocument in BMTDataContext.TemplateDocuments on ftd.DocumentId equals tDocument.DocumentId
                                                where kbt.KnowledgeBaseId == QuestionId
                                                && kbt.TemplateId == templateId
                                                && fA.ProjectUsageId == ProjectUsageId
                                                && fA.PracticeSiteId == siteId
                                                select tDocument).ToList<TemplateDocument>();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveDocumentCounts(int QuestionId, int templateId, int projectUsageId, int siteId, string policyText, string reportText, string screenText, string logsText, string otherText)
        {

            try
            {

                FilledAnswer fAnswer = (from kb in BMTDataContext.KnowledgeBases
                                        join kbt in BMTDataContext.KnowledgeBaseTemplates
                                        on kb.KnowledgeBaseId equals kbt.KnowledgeBaseId
                                        join fA in BMTDataContext.FilledAnswers on kbt.KnowledgeBaseTemplateId equals fA.KnowledgeBaseTemplateId
                                        where kb.KnowledgeBaseId == QuestionId && kbt.TemplateId == templateId
                                        && fA.ProjectUsageId == projectUsageId
                                        && fA.PracticeSiteId == siteId
                                        select fA).FirstOrDefault();

                fAnswer.PoliciesDocumentCount = Convert.ToInt32(policyText);
                fAnswer.ReportsDocumentCount = Convert.ToInt32(reportText);
                fAnswer.ScreenShotsDocumentCount = Convert.ToInt32(screenText);
                fAnswer.LogsOrToolsDocumentCount = Convert.ToInt32(logsText);
                fAnswer.OtherDocumentsCount = Convert.ToInt32(otherText);

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool CheckNCQASubmission(int templateId)
        {

            try
            {
                int result = (from temp in BMTDataContext.Templates
                              where temp.TemplateId == templateId
                              select temp.SubmitActionId).FirstOrDefault();


                return (result == (int)enSubmitAction.NCQA ? true : false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public KnowledgeBaseTemplate IsInfoDocEnable(int subHeaderId, int templateId)
        {

            try
            {
                KnowledgeBaseTemplate result = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                                                where kbt.KnowledgeBaseId == subHeaderId && kbt.TemplateId == templateId
                                                select kbt).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TemplateDocument> GetDocLinkCount(string fileName, int templateId, int projectUsageId,int siteId)
        {
            try
            {
                List<TemplateDocument> tempDoc = (from docs in BMTDataContext.TemplateDocuments
                                                  join ftdocs in BMTDataContext.FilledTemplateDocuments
                                                  on docs.DocumentId equals ftdocs.DocumentId
                                                  join fans in BMTDataContext.FilledAnswers
                                                  on ftdocs.FilledAnswerId equals fans.FilledAnswersId
                                                  join kbt in BMTDataContext.KnowledgeBaseTemplates
                                                  on fans.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                                  where docs.Path == fileName &&
                                                  kbt.TemplateId == templateId
                                                  && fans.ProjectUsageId == projectUsageId
                                                  && fans.PracticeSiteId==siteId
                                                  select docs).ToList();

                return tempDoc;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string GetSequence(int documentId, int templateId)
        {
            try
            {
                string sequence = "";
                KnowledgeBaseTemplate kbtemp = (from docs in BMTDataContext.TemplateDocuments
                                                join ftdocs in BMTDataContext.FilledTemplateDocuments
                                                on docs.DocumentId equals ftdocs.DocumentId
                                                join fans in BMTDataContext.FilledAnswers
                                                on ftdocs.FilledAnswerId equals fans.FilledAnswersId
                                                join kbt in BMTDataContext.KnowledgeBaseTemplates
                                                on fans.KnowledgeBaseTemplateId equals kbt.KnowledgeBaseTemplateId
                                                where docs.DocumentId == documentId &&
                                                kbt.TemplateId == templateId
                                                select kbt).FirstOrDefault();

                sequence = kbtemp.Sequence.ToString();

                KnowledgeBaseTemplate kbtSubHeader = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                                                      where kbtRec.KnowledgeBaseId == kbtemp.ParentKnowledgeBaseId &&
                                                      kbtRec.TemplateId == templateId
                                                      select kbtRec).FirstOrDefault();

                sequence += "," + kbtSubHeader.Sequence.ToString();

                KnowledgeBaseTemplate kbtHeader = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                                                   where kbtRec.KnowledgeBaseId == kbtSubHeader.ParentKnowledgeBaseId &&
                                                   kbtRec.TemplateId == templateId
                                                   select kbtRec).FirstOrDefault();

                sequence += "," + kbtHeader.Sequence.ToString();

                return sequence;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<KnowledgeBase> GetTemplateHeaders(int templateId)
        {
            try
            {
                List<KnowledgeBase> headers = (from kbts in BMTDataContext.KnowledgeBaseTemplates
                                               join kb in BMTDataContext.KnowledgeBases
                                               on kbts.KnowledgeBaseId equals kb.KnowledgeBaseId
                                               where kb.KnowledgeBaseTypeId == (int)enKBType.Header
                                               && kbts.TemplateId == templateId
                                               select kb).ToList();
                return headers;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<KnowledgeBase> GetTemplateSubHeaders(int parentId,int templateId)
        {
            try
            {
                List<KnowledgeBase> headers = (from kbts in BMTDataContext.KnowledgeBaseTemplates
                                               join kb in BMTDataContext.KnowledgeBases
                                               on kbts.KnowledgeBaseId equals kb.KnowledgeBaseId
                                               where kbts.ParentKnowledgeBaseId==parentId
                                               && kbts.TemplateId == templateId
                                               select kb).ToList();
                return headers;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TemplateDocument> GetUnassociatedDocs(int projectUsageId, int siteId,int templateId)
        {
            try
            {
                int kbtId = (from kbt in BMTDataContext.KnowledgeBaseTemplates
                             where kbt.TemplateId == templateId
                             select kbt.KnowledgeBaseTemplateId).FirstOrDefault();

                int fillAnsId = (from fAns in BMTDataContext.FilledAnswers
                                 where fAns.ProjectUsageId == projectUsageId &&
                                 fAns.PracticeSiteId == siteId &&
                                 fAns.KnowledgeBaseTemplateId==kbtId
                                 select fAns.FilledAnswersId).FirstOrDefault();

                List<TemplateDocument> tDocs = (from tempDocs in BMTDataContext.TemplateDocuments
                                                join fTDocs in BMTDataContext.FilledTemplateDocuments
                                                on tempDocs.DocumentId equals fTDocs.DocumentId
                                                where tempDocs.DocumentType == enDocType.UnAssociatedDoc.ToString()
                                                && fTDocs.FilledAnswerId==fillAnsId
                                                select tempDocs).ToList();

                return tDocs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUnAssociatedFiles(int projectUsageId,int siteId,int templateId,string filePath)
        {
            try
            {
                TemplateDocument tempDocs = (from tDocs in BMTDataContext.TemplateDocuments
                                where tDocs.Path == filePath &&
                                tDocs.DocumentType == "UnAssociatedDoc"
                                select tDocs).FirstOrDefault();

                FilledTemplateDocument ftd = (from ftdRec in BMTDataContext.FilledTemplateDocuments
                                              join fillAns in BMTDataContext.FilledAnswers
                                              on ftdRec.FilledAnswerId equals fillAns.FilledAnswersId
                                              where ftdRec.DocumentId == tempDocs.DocumentId &&
                                              fillAns.ProjectUsageId == projectUsageId &&
                                              fillAns.PracticeSiteId == siteId
                                              select ftdRec).FirstOrDefault();

                BMTDataContext.FilledTemplateDocuments.DeleteOnSubmit(ftd);
                BMTDataContext.TemplateDocuments.DeleteOnSubmit(tempDocs);
                BMTDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTemplateOption(int projectUsageId, int templateId, int enterpriseId)
        {
            try
            {
                var IsTempOption = (from projSec in BMTDataContext.ProjectSections
                                    join tempOption in BMTDataContext.TemplateOptions
                                    on projSec.fkTemplateOptionId equals tempOption.TemplateOptionId
                                    join tempAssign in BMTDataContext.TemplateOptionAssignments
                                    on tempOption.TemplateOptionId equals tempAssign.TemplateOptionId
                                    join projUsage in BMTDataContext.ProjectUsages
                                    on projSec.ProjectId equals projUsage.ProjectId
                                    where tempOption.fkTemplateId == templateId &&
                                    projUsage.ProjectUsageId==projectUsageId &&
                                    tempAssign.EnterpriseId == enterpriseId
                                    select tempOption).FirstOrDefault();

                if (IsTempOption != null)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FilledAnswersExist(int projectUsageId, int siteId)
        {
            try
            {
                List<FilledAnswer> fAns = (from filledAnsRec in BMTDataContext.FilledAnswers
                                           where filledAnsRec.ProjectUsageId == projectUsageId &&
                                           filledAnsRec.PracticeSiteId == siteId
                                           select filledAnsRec).ToList();

                if (fAns.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyTempalteOption(int projectUsageId, int siteId)
        {
            try
            {
                int tempOptionId = (int)(from projectSec in BMTDataContext.ProjectSections
                                    join projectUsage in BMTDataContext.ProjectUsages
                                    on projectSec.ProjectId equals projectUsage.ProjectId
                                    where projectUsage.ProjectUsageId == projectUsageId
                                         select projectSec.fkTemplateOptionId).FirstOrDefault();

                List<FilledAnswersOption> fAnsOps = (from fillAnsOp in BMTDataContext.FilledAnswersOptions
                                                    where fillAnsOp.OptionId == tempOptionId
                                                    select fillAnsOp).ToList();

                foreach (FilledAnswersOption fAnsOp in fAnsOps)
                {
                    FilledAnswer fAns = new FilledAnswer();
                    fAns.PracticeSiteId = siteId;
                    fAns.ProjectUsageId = projectUsageId;
                    fAns.LastUpdated = DateTime.Now;
                    fAns.UpdatedBy = 1;
                    fAns.KnowledgeBaseTemplateId = fAnsOp.KnowledgebaseTemplateId;
                    fAns.AnswerTypeEnumId = fAnsOp.AnswerTypeEnumId;
                    fAns.PoliciesDocumentCount = fAnsOp.PoliciesDocumentCount;
                    fAns.ReportsDocumentCount = fAnsOp.ReportsDocumentCount;
                    fAns.LogsOrToolsDocumentCount = fAnsOp.LogsOrToolsDocumentCount;
                    fAns.OtherDocumentsCount = fAnsOp.OtherDocumentsCount;
                    fAns.PrivateNotes = fAnsOp.PrivateNotes;
                    fAns.ReviewNotes = fAnsOp.ReviewNotes;
                    fAns.ScreenShotsDocumentCount = fAnsOp.ScreenShotsDocumentCount;
                    fAns.Complete = fAnsOp.Complete;
                    fAns.DataBoxComments = fAnsOp.DataBoxComments;
                    fAns.DefaultScore = fAnsOp.DefaultScore;
                    fAns.EvaluationNotes = fAnsOp.EvaluationNotes;
                    BMTDataContext.FilledAnswers.InsertOnSubmit(fAns);
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
