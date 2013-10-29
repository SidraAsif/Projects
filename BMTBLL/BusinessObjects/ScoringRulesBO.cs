using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using BMTBLL.Enumeration;

namespace BMTBLL.BusinessObjects
{
    #region VARIABLES
    
    #endregion

    public class ScoringRulesBO : BMTConnection
    {
        public List<KnowledgeBaseTemplate> GetKnowledgeBaseList(int templateId)
        {
            try
            {
                var knowledgeBaseTemp = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                         join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                        on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                                         join template in BMTDataContext.Templates
                                         on knowledgeBaseTemplate.TemplateId equals template.TemplateId
                                         where knowledgeBase.KnowledgeBaseTypeId == 2
                                         && template.TemplateId == templateId
                                         orderby knowledgeBaseTemplate.ParentKnowledgeBaseId
                                         select knowledgeBaseTemplate).ToList();

                return knowledgeBaseTemp;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public bool SaveScoringRules(int knowledgeBaseTemplateId, string score, string minYesFactors, string maxYesFactors, string mustPresentFactor, string abcentFactor)
        {
            try
            {
                ScoringRule scoringRule = new ScoringRule();
                scoringRule.KnowledgeBaseTemplateId = knowledgeBaseTemplateId;
                scoringRule.Score = score;
                if (minYesFactors != null)
                    scoringRule.MinYesFactor = Convert.ToInt32(minYesFactors);
                if (maxYesFactors != null)
                    scoringRule.MaxYesFactor = Convert.ToInt32(maxYesFactors);

                scoringRule.MustPresentFactorSequence = mustPresentFactor;
                scoringRule.AbsentFactorSequence = abcentFactor;
                scoringRule.MustPassFactorSequence = null;

                if (minYesFactors != null || maxYesFactors != null || mustPresentFactor != null || abcentFactor != null)
                {
                    BMTDataContext.ScoringRules.InsertOnSubmit(scoringRule);
                    BMTDataContext.SubmitChanges();
                }

                else if (score.Substring(0,2) == "0%")
                {
                    scoringRule.MinYesFactor = 0;
                    scoringRule.MaxYesFactor = 0;
                    BMTDataContext.ScoringRules.InsertOnSubmit(scoringRule);
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveSimpleSum(int answerTypeEnumId,int knowledgeBaseTemplateId, int weightage)
        {
            try
            {
                AnswerTypeWeightage answerTypeWeightage = new AnswerTypeWeightage();

                answerTypeWeightage.AnswerTypeEnumId = answerTypeEnumId;
                answerTypeWeightage.KnowledgeBaseTemplateId = knowledgeBaseTemplateId;
                answerTypeWeightage.Weightage = weightage;

                BMTDataContext.AnswerTypeWeightages.InsertOnSubmit(answerTypeWeightage);
                BMTDataContext.SubmitChanges();                

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveMaxPoints(int knowledgeBaseTemplateId, int knowledgeBaseId, int maxPoints)
        {
            try
            {
                var knowledgeBaseTemplateRecord = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                   where knowledgeBaseTemplate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                                                   && knowledgeBaseTemplate.KnowledgeBaseId == knowledgeBaseId
                                                   select knowledgeBaseTemplate).SingleOrDefault();

                if (knowledgeBaseTemplateRecord != null)
                {
                    knowledgeBaseTemplateRecord.MaxPoints = maxPoints;
                    BMTDataContext.SubmitChanges();                    
                }
                return true;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public List<AnswerTypeEnum> GetAnswerTypeEnum(int answerTypeId)
        {
            try
            {
                var answerTypeEnumId = (from answerTypeEnum in BMTDataContext.AnswerTypeEnums
                                                         join answerType in BMTDataContext.AnswerTypes
                                                         on answerTypeEnum.AnswerTypeId equals answerType.AnswerTypeId
                                                         where answerType.AnswerTypeId == answerTypeId
                                                         select answerTypeEnum).ToList();
                return answerTypeEnumId;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public bool DeleteExistingScore(int knowledgeBaseTemplateId)
        {
            try
            {
                var knowledgeBaseScore = (from scoringRuleTepmlate in BMTDataContext.ScoringRules
                                          where scoringRuleTepmlate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                                          select scoringRuleTepmlate).ToList();


                foreach (var kBScore in knowledgeBaseScore)
                {
                    var score = (from scoringRule in BMTDataContext.ScoringRules
                                 where scoringRule.KnowledgeBaseTemplateId == kBScore.KnowledgeBaseTemplateId
                                 select scoringRule).FirstOrDefault();

                    BMTDataContext.ScoringRules.DeleteOnSubmit(score);
                    BMTDataContext.SubmitChanges();
                }
                
                return true;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public bool DeleteExistingWeightage(int knowledgeBaseTemplateId)
        {
            try
            {
                var weightageList = (from answerTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                     where answerTypeWeightage.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                                 select answerTypeWeightage).ToList();

                foreach (var answerWeightage in weightageList)
                {
                    var weightage = (from answerTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                     where answerTypeWeightage.KnowledgeBaseTemplateId == answerWeightage.KnowledgeBaseTemplateId
                                     select answerTypeWeightage).FirstOrDefault();

                    BMTDataContext.AnswerTypeWeightages.DeleteOnSubmit(weightage);

                    BMTDataContext.SubmitChanges();
                }
                
                return true;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public List<ScoringRule> GetScoringRules(int templateId, int knowledgeBaseId)
        {
            try
            {
                var scores = (from scoringRule in BMTDataContext.ScoringRules
                              join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                              on scoringRule.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                              join knowledgeBase in BMTDataContext.KnowledgeBases
                              on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                              where knowledgeBaseTemplate.TemplateId == templateId &&
                                    knowledgeBaseTemplate.KnowledgeBaseId == knowledgeBaseId
                              select scoringRule).ToList();

                return scores;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public List<AnswerTypeWeightage> GetSumList(int templateId, int knowledgeBaseId)
        {
            try
            {
                var scores = (from simpleSum in BMTDataContext.AnswerTypeWeightages
                              join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                             on simpleSum.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                              join knowledgeBase in BMTDataContext.KnowledgeBases
                              on simpleSum.KnowledgeBaseTemplateId equals knowledgeBase.KnowledgeBaseId
                              where knowledgeBaseTemplate.TemplateId == templateId  && 
                                simpleSum.KnowledgeBaseTemplateId == knowledgeBaseId
                              select simpleSum).ToList();

                return scores;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public List<KnowledgeBaseTemplate> GetMaxPoints(int knowledgeBaseId, int TemplateId)
        {
            try
            {
                var knowledgeBaseTemplateMaxPoints = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                      join knowledgeBase in BMTDataContext.KnowledgeBases
                                                            on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                                      where knowledgeBaseTemplate.KnowledgeBaseId == knowledgeBaseId && knowledgeBaseTemplate.TemplateId == TemplateId
                                                      select knowledgeBaseTemplate).ToList();
                return knowledgeBaseTemplateMaxPoints;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public List<KnowledgeBaseTemplate> GetKnowledgeTemplateList(int templateId, int knowledgeBaseTemplateId)
        {
            try
            {
                var knowledgeBaseTempalteList = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                 join sr in BMTDataContext.ScoringRules
                                                 on knowledgeBaseTemplate.KnowledgeBaseTemplateId equals sr.KnowledgeBaseTemplateId
                                                 join template in BMTDataContext.Templates
                                                 on knowledgeBaseTemplate.TemplateId equals template.TemplateId
                                                 where knowledgeBaseTemplate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                                                 && knowledgeBaseTemplate.TemplateId == templateId
                                                 select knowledgeBaseTemplate).ToList();


                return knowledgeBaseTempalteList;
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }

        public List<string> GetStandardElements(int knowledgeBaseTemplateId)
        {
            try
            {
                //string standard = (from knowledgeBase in BMTDataContext.KnowledgeBases
                //                join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                //                on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.ParentKnowledgeBaseId
                //                join kb in BMTDataContext.KnowledgeBases
                //                on knowledgeBaseTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                //                where knowledgeBaseTemplate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                //                select knowledgeBase.Name);

                var element = (from knowledgeBase in BMTDataContext.KnowledgeBases
                               join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                               on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.ParentKnowledgeBaseId
                               join kb in BMTDataContext.KnowledgeBases
                               on knowledgeBaseTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                               where knowledgeBaseTemplate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                               select kb.Name).ToList();

                 return element;

                
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }
    }
}
