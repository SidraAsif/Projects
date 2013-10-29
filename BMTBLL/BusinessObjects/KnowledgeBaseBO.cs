using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class KnowledgeBaseBO : BMTConnection
    {
        #region FUNCTIONS

        public IQueryable GetHeader(string userType)
        {
            IQueryable getHeader = null;
            try
            {
                if (userType == enUserRole.SuperUser.ToString())
                {
                    getHeader = (from kb in BMTDataContext.KnowledgeBases
                                 where kb.KnowledgeBaseTypeId == (int)enKBType.Header
                                 select kb).AsQueryable();
                }
                else if (userType == enUserRole.SuperAdmin.ToString())
                {
                    getHeader = (from kb in BMTDataContext.KnowledgeBases
                                 where kb.KnowledgeBaseTypeId == (int)enKBType.Header
                                 select kb).AsQueryable();
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    getHeader = (from kb in BMTDataContext.KnowledgeBases
                                 where kb.KnowledgeBaseTypeId == (int)enKBType.Header
                                 select kb).AsQueryable();
                }
                else if (userType == enUserRole.User.ToString())
                {
                    getHeader = (from kb in BMTDataContext.KnowledgeBases
                                 where kb.KnowledgeBaseTypeId == (int)enKBType.Header
                                 select kb).AsQueryable();
                }
                return getHeader;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetSubHeaderByTempId(int tempId)
        {
            IQueryable getSubHeader = null;
            try
            {
                getSubHeader = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                join kb in BMTDataContext.KnowledgeBases
                                on kbtemp.KnowledgeBaseId equals kb.KnowledgeBaseId
                                where kb.KnowledgeBaseTypeId == (int)enKBType.SubHeader &&
                                kbtemp.TemplateId == tempId
                                select kb).AsQueryable();

                return getSubHeader;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetQuestionByTempId(int tempId)
        {
            IQueryable getQuestion = null;
            try
            {
                getQuestion = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                               join kb in BMTDataContext.KnowledgeBases
                               on kbtemp.KnowledgeBaseId equals kb.KnowledgeBaseId
                               where kb.KnowledgeBaseTypeId == (int)enKBType.Question &&//for Question
                               kbtemp.TemplateId == tempId
                               select kb).AsQueryable();

                return getQuestion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetHeaderByKnowledgebaseId(int kbId, int tempId)
        {
            IQueryable getQuestion = null;
            try
            {
                getQuestion = (from kb in BMTDataContext.KnowledgeBases
                               join kbTemp in BMTDataContext.KnowledgeBaseTemplates
                               on kb.KnowledgeBaseId equals kbTemp.KnowledgeBaseId
                               join kbType in BMTDataContext.KnowledgeBaseTypes
                               on kb.KnowledgeBaseTypeId equals kbType.KnowledgeBaseTypeId
                               join kbAccess in BMTDataContext.AccessLevels
                               on kb.AccessId equals kbAccess.AccessLevelId
                               join kbCreated in BMTDataContext.Users
                               on kb.CreatedBy equals kbCreated.UserId
                               where kb.KnowledgeBaseId == kbId &&
                               kbTemp.TemplateId == tempId
                               select new
                               {
                                   ParentId = kbTemp.ParentKnowledgeBaseId,
                                   KnowledgebaseId = kb.KnowledgeBaseId,
                                   KnowledgebaseType = kbType.KnowledgeBaseType1,
                                   Name = kb.Name,
                                   TabName = kb.TabName,
                                   MustPass = kb.MustPass,
                                   Description = kb.InstructionText,
                                   AccessBy = kbAccess.AccessLevelName,
                                   CreatedBy = kbCreated.Username
                               }).AsQueryable();


                return getQuestion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public KnowledgeBase EditKbElement(int kbId)
        {
            try
            {
                KnowledgeBase getElementForEdit = (from kb in BMTDataContext.KnowledgeBases
                                                   where kb.KnowledgeBaseId == kbId
                                                   select kb).FirstOrDefault();
                return getElementForEdit;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetHeaderInfo(int kbId, int templateId)
        {
            IQueryable getQuestion = null;
            try
            {
                getQuestion = (from kb in BMTDataContext.KnowledgeBases
                               join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                               on kb.KnowledgeBaseId equals kbtemp.KnowledgeBaseId
                               join temp in BMTDataContext.Templates
                               on kbtemp.TemplateId equals temp.TemplateId
                               join kbType in BMTDataContext.KnowledgeBaseTypes
                               on kb.KnowledgeBaseTypeId equals kbType.KnowledgeBaseTypeId
                               join kbAccess in BMTDataContext.AccessLevels
                               on kb.AccessId equals kbAccess.AccessLevelId
                               join kbCreated in BMTDataContext.Users
                               on kb.CreatedBy equals kbCreated.UserId
                               where kb.KnowledgeBaseId == kbId &&
                               kbtemp.TemplateId != templateId
                               select new
                               {
                                   TemplateId = kbtemp.TemplateId,
                                   TemplateName = temp.Name,
                                   DisplayName = kb.Name,
                                   TabName = kb.TabName,
                                   MustPass = (kb.MustPass == true) ? "Yes" : "No",
                                   AccessBy = kbAccess.AccessLevelName,
                                   Instruction = kb.InstructionText == null ? " " : kb.InstructionText,
                                   Answer = (kbtemp.AnswerTypeId == (int)enAnswerType.YesNoNA) ? "Yes/No/NA" : (kbtemp.AnswerTypeId == (int)enAnswerType.YesNo) ? "Yes/No" : "None",
                                   Critical = (kbtemp.IsCritical == true) ? "Yes" : "No",
                                   AddOns = (kbtemp.IsDataBox == true) ? "D" : "None"
                               }).AsQueryable();


                return getQuestion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string SaveKb(int templateId, int kbId, int kbTypeId, string displayName, string tabName, string instruction, bool mustPass, string answerTypeId, int userId, bool isCritical, int parentId, int grandParentId, List<int> list, string isEditOrAdd, string dataBoxHeader, string criticalToolText, bool isInfoDocsEnable, int pageReference)
        {
            System.Data.Common.DbTransaction transaction;
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();
            BMTDataContext.Connection.Open();
            transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            int newkb = 0;
            try
            {
                if (kbTypeId == (int)enKBType.Header && isEditOrAdd == "Add")
                {
                    Template tempRec = (from tem in BMTDataContext.Templates
                                        where tem.TemplateId == templateId
                                        select tem).FirstOrDefault();

                    KnowledgeBase sameNameHeader = (from kbase in BMTDataContext.KnowledgeBases
                                                    where kbase.KnowledgeBaseTypeId == (int)enKBType.Header
                                                    && kbase.Name == displayName
                                                    select kbase).FirstOrDefault();

                    if (sameNameHeader == null)
                    {
                        KnowledgeBase kb = new KnowledgeBase();
                        kb.KnowledgeBaseTypeId = kbTypeId;
                        kb.Name = displayName;
                        kb.TabName = tabName;
                        kb.DisplayName = displayName;
                        kb.InstructionText = instruction;
                        kb.AccessId = tempRec.AccessLevelId;
                        kb.CreatedBy = userId;
                        kb.IsActive = true;

                        BMTDataContext.KnowledgeBases.InsertOnSubmit(kb);
                        BMTDataContext.SubmitChanges();
                        newkb = kb.KnowledgeBaseId;

                        KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();

                        kbTemp.KnowledgeBaseId = newkb;
                        kbTemp.TemplateId = templateId;

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTemp);
                        BMTDataContext.SubmitChanges();
                        SetHeaderSequence(templateId);
                    }
                    else
                    {
                        return "Header Name Already Exists.";
                    }
                }
                else if (isEditOrAdd == "Add" && kbTypeId == (int)enKBType.SubHeader && kbId == 0)
                {
                    Template tempRec = (from tem in BMTDataContext.Templates
                                        where tem.TemplateId == templateId
                                        select tem).FirstOrDefault();

                    KnowledgeBase sameNameSubHeader = (from kbase in BMTDataContext.KnowledgeBases
                                                       where kbase.KnowledgeBaseTypeId == (int)enKBType.SubHeader
                                                       && kbase.Name == displayName
                                                       select kbase).FirstOrDefault();

                    if (sameNameSubHeader == null)
                    {
                        KnowledgeBase kb = new KnowledgeBase();
                        kb.KnowledgeBaseTypeId = kbTypeId;
                        kb.Name = displayName;
                        kb.TabName = tabName;
                        kb.DisplayName = displayName;
                        kb.InstructionText = instruction;
                        kb.AccessId = tempRec.AccessLevelId;
                        kb.MustPass = mustPass;
                        kb.CreatedBy = userId;
                        kb.IsActive = true;

                        BMTDataContext.KnowledgeBases.InsertOnSubmit(kb);
                        BMTDataContext.SubmitChanges();
                        newkb = kb.KnowledgeBaseId;

                        KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();

                        kbTemp.KnowledgeBaseId = newkb;
                        kbTemp.TemplateId = templateId;
                        kbTemp.ParentKnowledgeBaseId = parentId;
                        kbTemp.IsInfoDocEnable = isInfoDocsEnable;
                        kbTemp.ReferencePages = pageReference;

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTemp);
                        BMTDataContext.SubmitChanges();

                        SetSubHeaderSequence(templateId, parentId);

                        KnowledgeBaseTemplate ParentHeader = (from parentTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                              where parentTempRec.KnowledgeBaseId == parentId &&
                                                              parentTempRec.TemplateId == templateId
                                                              select parentTempRec).FirstOrDefault();
                        if (ParentHeader == null)
                        {
                            KnowledgeBaseTemplate AddParent = new KnowledgeBaseTemplate();

                            AddParent.KnowledgeBaseId = parentId;
                            AddParent.TemplateId = templateId;

                            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(AddParent);
                            BMTDataContext.SubmitChanges();

                            SetHeaderSequence(templateId);
                        }
                    }
                    else
                    {
                        return "Sub-Header Name Already Exists.";
                    }
                }
                else if (isEditOrAdd == "Add" && kbTypeId == (int)enKBType.Question && kbId == 0)
                {
                    int parentkbaseTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                           where kbaseTempRec.KnowledgeBaseId == parentId &&
                                           kbaseTempRec.TemplateId == templateId
                                           select kbaseTempRec.KnowledgeBaseTemplateId).FirstOrDefault();

                    var scRules = (from scRulesRec in BMTDataContext.ScoringRules
                                   where scRulesRec.KnowledgeBaseTemplateId == parentkbaseTemp
                                   select scRulesRec).ToList();

                    if (scRules.Count != 0)
                        return "Scoring Rules Exists.";

                    Template tempRec = (from tem in BMTDataContext.Templates
                                        where tem.TemplateId == templateId
                                        select tem).FirstOrDefault();

                    KnowledgeBase sameNameQuestion = (from kbase in BMTDataContext.KnowledgeBases
                                                      where kbase.KnowledgeBaseTypeId == (int)enKBType.Question
                                                      && kbase.Name == displayName
                                                      select kbase).FirstOrDefault();

                    if (sameNameQuestion == null)
                    {
                        KnowledgeBase kb = new KnowledgeBase();
                        kb.KnowledgeBaseTypeId = kbTypeId;
                        kb.Name = displayName;
                        kb.TabName = tabName;
                        kb.DisplayName = displayName;
                        kb.InstructionText = instruction;
                        kb.AccessId = tempRec.AccessLevelId;
                        kb.MustPass = mustPass;
                        kb.CreatedBy = userId;
                        kb.IsActive = true;

                        BMTDataContext.KnowledgeBases.InsertOnSubmit(kb);
                        BMTDataContext.SubmitChanges();
                        newkb = kb.KnowledgeBaseId;

                        KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();

                        kbTemp.KnowledgeBaseId = newkb;
                        kbTemp.TemplateId = templateId;
                        kbTemp.ParentKnowledgeBaseId = parentId;
                        kbTemp.IsCritical = isCritical;
                        if (dataBoxHeader != "")
                        {
                            kbTemp.IsDataBox = true;
                            kbTemp.DataBoxHeader = dataBoxHeader;
                        }
                        kbTemp.CriticalTooltip = criticalToolText;
                        if (answerTypeId == enAnsTypeId.AnsYesNo.ToString())
                        {
                            kbTemp.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNo);
                        }
                        else if (answerTypeId == enAnsTypeId.AnsYesNoNA.ToString())
                        {
                            kbTemp.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNoNA);
                        }

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTemp);
                        BMTDataContext.SubmitChanges();

                        SetQuestionSequence(templateId, parentId);

                        KnowledgeBaseTemplate parentHeader = (from parentTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                              where parentTempRec.KnowledgeBaseId == parentId &&
                                                              parentTempRec.TemplateId == templateId
                                                              select parentTempRec).FirstOrDefault();
                        if (parentHeader == null)
                        {
                            KnowledgeBaseTemplate addParent = new KnowledgeBaseTemplate();

                            addParent.KnowledgeBaseId = parentId;
                            addParent.TemplateId = templateId;
                            addParent.ParentKnowledgeBaseId = grandParentId;
                            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(addParent);
                            BMTDataContext.SubmitChanges();

                            SetSubHeaderSequence(templateId, grandParentId);

                            KnowledgeBaseTemplate grandParentHeader = (from parentTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                       where parentTempRec.KnowledgeBaseId == grandParentId &&
                                                                       parentTempRec.TemplateId == templateId
                                                                       select parentTempRec).FirstOrDefault();
                            if (grandParentHeader == null)
                            {
                                KnowledgeBaseTemplate addGrandParent = new KnowledgeBaseTemplate();

                                addGrandParent.KnowledgeBaseId = grandParentId;
                                addGrandParent.TemplateId = templateId;

                                BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(addGrandParent);
                                BMTDataContext.SubmitChanges();

                                SetHeaderSequence(templateId);
                            }
                        }
                    }
                    else
                    {
                        return "Question Name Already Exists.";
                    }
                }
                else if (isEditOrAdd == "Add" && kbTypeId == (int)enKBType.SubHeader && kbId != 0)
                {
                    KnowledgeBaseTemplate alreadyExist = (from kbAlreadyExist in BMTDataContext.KnowledgeBaseTemplates
                                                          where kbAlreadyExist.KnowledgeBaseId == kbId &&
                                                          kbAlreadyExist.ParentKnowledgeBaseId == parentId &&
                                                          kbAlreadyExist.TemplateId == templateId
                                                          select kbAlreadyExist).FirstOrDefault();
                    if (alreadyExist == null)
                    {
                        KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();

                        kbTemp.KnowledgeBaseId = kbId;
                        kbTemp.TemplateId = templateId;
                        kbTemp.ParentKnowledgeBaseId = parentId;
                        kbTemp.IsInfoDocEnable = isInfoDocsEnable;
                        kbTemp.ReferencePages = pageReference;

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTemp);
                        BMTDataContext.SubmitChanges();

                        SetSubHeaderSequence(templateId, parentId);

                        KnowledgeBaseTemplate parentKbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                              where kbTempRec.KnowledgeBaseId == parentId &&
                                                              kbTempRec.TemplateId == templateId
                                                              select kbTempRec).FirstOrDefault();

                        if (parentKbTemp == null)
                        {
                            KnowledgeBaseTemplate addParent = new KnowledgeBaseTemplate();

                            addParent.KnowledgeBaseId = parentId;
                            addParent.TemplateId = templateId;

                            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(addParent);
                            BMTDataContext.SubmitChanges();

                            SetHeaderSequence(templateId);
                        }
                    }
                    else
                    {
                        return "Sub-Header Already Exists in this Header";
                    }
                }
                else if (isEditOrAdd == "Add" && kbTypeId == (int)enKBType.Question && kbId != 0)
                {
                    KnowledgeBaseTemplate alreadyExist = (from kbAlreadyExist in BMTDataContext.KnowledgeBaseTemplates
                                                          where kbAlreadyExist.KnowledgeBaseId == kbId &&
                                                          kbAlreadyExist.ParentKnowledgeBaseId == parentId &&
                                                          kbAlreadyExist.TemplateId == templateId
                                                          select kbAlreadyExist).FirstOrDefault();
                    if (alreadyExist == null)
                    {
                        KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();

                        kbTemp.KnowledgeBaseId = kbId;
                        kbTemp.TemplateId = templateId;
                        kbTemp.ParentKnowledgeBaseId = parentId;
                        kbTemp.IsCritical = isCritical;
                        if (dataBoxHeader != "")
                        {
                            kbTemp.IsDataBox = true;
                            kbTemp.DataBoxHeader = dataBoxHeader;
                        }
                        kbTemp.CriticalTooltip = criticalToolText;
                        if (answerTypeId == enAnsTypeId.AnsYesNo.ToString())
                        {
                            kbTemp.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNo);
                        }
                        else if (answerTypeId == enAnsTypeId.AnsYesNoNA.ToString())
                        {
                            kbTemp.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNoNA);
                        }

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTemp);
                        BMTDataContext.SubmitChanges();

                        SetQuestionSequence(templateId, parentId);

                        KnowledgeBaseTemplate parentKbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                              where kbTempRec.KnowledgeBaseId == parentId &&
                                                              kbTempRec.TemplateId == templateId
                                                              select kbTempRec).FirstOrDefault();

                        if (parentKbTemp == null)
                        {
                            KnowledgeBaseTemplate addParent = new KnowledgeBaseTemplate();

                            addParent.KnowledgeBaseId = parentId;
                            addParent.TemplateId = templateId;
                            addParent.ParentKnowledgeBaseId = grandParentId;

                            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(addParent);
                            BMTDataContext.SubmitChanges();

                            SetSubHeaderSequence(templateId, grandParentId);

                            KnowledgeBaseTemplate grandParentKbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                       where kbTempRec.KnowledgeBaseId == grandParentId &&
                                                                       kbTempRec.TemplateId == templateId
                                                                       select kbTempRec).FirstOrDefault();

                            if (grandParentKbTemp == null)
                            {
                                KnowledgeBaseTemplate addGrandParent = new KnowledgeBaseTemplate();

                                addGrandParent.KnowledgeBaseId = grandParentId;
                                addGrandParent.TemplateId = templateId;

                                BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(addGrandParent);
                                BMTDataContext.SubmitChanges();

                                SetHeaderSequence(templateId);
                            }
                        }
                    }
                    else
                    {
                        return "Question Already Exists in this Sub-Header";
                    }
                }
                else if (isEditOrAdd == "Edit")
                {
                    if (kbTypeId == (int)enKBType.Question)
                    {
                        Template tempRec = (from tem in BMTDataContext.Templates
                                            where tem.TemplateId == templateId
                                            select tem).FirstOrDefault();

                        List<KnowledgeBaseTemplate> kBaseTemp = (from kBaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                 where kBaseTempRec.KnowledgeBaseId == kbId &&
                                                                 kBaseTempRec.TemplateId != templateId
                                                                 select kBaseTempRec).ToList();

                        if (kBaseTemp.Count != 0)
                        {
                            KnowledgeBaseTemplate kbaseTempRec = (from kbaseRec in BMTDataContext.KnowledgeBaseTemplates
                                                                  where kbaseRec.TemplateId == templateId &&
                                                                  kbaseRec.KnowledgeBaseId == kbId &&
                                                                  kbaseRec.ParentKnowledgeBaseId == parentId
                                                                  select kbaseRec).FirstOrDefault();

                            if (kbaseTempRec != null)
                            {
                                if (dataBoxHeader != "")
                                {
                                    kbaseTempRec.IsDataBox = true;
                                    kbaseTempRec.DataBoxHeader = dataBoxHeader;
                                }
                                else
                                {
                                    kbaseTempRec.IsDataBox = false;
                                    kbaseTempRec.DataBoxHeader = null;
                                }
                                kbaseTempRec.CriticalTooltip = criticalToolText;
                                if (answerTypeId == enAnsTypeId.AnsYesNo.ToString())
                                {
                                    kbaseTempRec.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNo);
                                }
                                else if (answerTypeId == enAnsTypeId.AnsYesNoNA.ToString())
                                {
                                    kbaseTempRec.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNoNA);
                                }
                                kbaseTempRec.IsCritical = isCritical;
                                BMTDataContext.SubmitChanges();
                            }
                        }

                        else
                        {
                            KnowledgeBase kb = (from kBaseRec in BMTDataContext.KnowledgeBases
                                                where kBaseRec.KnowledgeBaseId == kbId
                                                select kBaseRec).FirstOrDefault();

                            KnowledgeBase sameNameQuestion = (from kbase in BMTDataContext.KnowledgeBases
                                                              where kbase.Name == displayName &&
                                                              kbase.KnowledgeBaseId!=kbId
                                                              select kbase).FirstOrDefault();

                            if (sameNameQuestion != null)
                            {
                                if (kbTypeId == (int)enKBType.Question)
                                    return "Question Already Exists in this Header";
                            }

                            kb.KnowledgeBaseTypeId = kbTypeId;
                            kb.Name = displayName;
                            kb.TabName = tabName;
                            kb.DisplayName = displayName;
                            kb.InstructionText = instruction;
                            kb.AccessId = tempRec.AccessLevelId;
                            kb.CreatedBy = userId;
                            kb.IsActive = true;

                            BMTDataContext.SubmitChanges();

                            KnowledgeBaseTemplate kbaseTempRec = (from kbaseRec in BMTDataContext.KnowledgeBaseTemplates
                                                                  where kbaseRec.TemplateId == templateId &&
                                                                  kbaseRec.KnowledgeBaseId == kbId &&
                                                                  kbaseRec.ParentKnowledgeBaseId == parentId
                                                                  select kbaseRec).FirstOrDefault();

                            if (kbaseTempRec != null)
                            {
                                if (dataBoxHeader != "")
                                {
                                    kbaseTempRec.IsDataBox = true;
                                    kbaseTempRec.DataBoxHeader = dataBoxHeader;
                                }
                                else
                                {
                                    kbaseTempRec.IsDataBox = false;
                                    kbaseTempRec.DataBoxHeader = null;
                                }
                                kbaseTempRec.CriticalTooltip = criticalToolText;
                                if (answerTypeId == enAnsTypeId.AnsYesNo.ToString())
                                {
                                    kbaseTempRec.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNo);
                                }
                                else if (answerTypeId == enAnsTypeId.AnsYesNoNA.ToString())
                                {
                                    kbaseTempRec.AnswerTypeId = Convert.ToInt32(enAnsTypeId.AnsYesNoNA);
                                }
                                kbaseTempRec.IsCritical = isCritical;
                                BMTDataContext.SubmitChanges();
                            }
                        }
                    }
                    if (kbTypeId == (int)enKBType.SubHeader)
                    {
                        Template tempRec = (from tem in BMTDataContext.Templates
                                            where tem.TemplateId == templateId
                                            select tem).FirstOrDefault();

                        List<KnowledgeBaseTemplate> kBaseTemp = (from kBaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                 where kBaseTempRec.KnowledgeBaseId == kbId &&
                                                                 kBaseTempRec.TemplateId != templateId
                                                                 select kBaseTempRec).ToList();

                        if (kBaseTemp.Count != 0)
                        {
                            KnowledgeBaseTemplate kbaseTempRec = (from kbaseRec in BMTDataContext.KnowledgeBaseTemplates
                                                                  where kbaseRec.TemplateId == templateId &&
                                                                  kbaseRec.KnowledgeBaseId == kbId &&
                                                                  kbaseRec.ParentKnowledgeBaseId == parentId
                                                                  select kbaseRec).FirstOrDefault();

                            if (kbaseTempRec != null)
                            {
                                kbaseTempRec.IsInfoDocEnable = isInfoDocsEnable;
                                kbaseTempRec.ReferencePages = pageReference;
                                BMTDataContext.SubmitChanges();
                            }
                        }

                        else
                        {
                            KnowledgeBase kb = (from kBaseRec in BMTDataContext.KnowledgeBases
                                                where kBaseRec.KnowledgeBaseId == kbId
                                                select kBaseRec).FirstOrDefault();

                            KnowledgeBase sameNameQuestion = (from kbase in BMTDataContext.KnowledgeBases
                                                              where kbase.Name == displayName &&
                                                              kbase.KnowledgeBaseId != kbId
                                                              select kbase).FirstOrDefault();

                            if (sameNameQuestion != null)
                            {
                                if (kbTypeId == (int)enKBType.Question)
                                    return "Sub-Header Already Exists in this Header";
                            }

                            kb.KnowledgeBaseTypeId = kbTypeId;
                            kb.Name = displayName;
                            kb.TabName = tabName;
                            kb.DisplayName = displayName;
                            kb.InstructionText = instruction;
                            kb.AccessId = tempRec.AccessLevelId;
                            kb.CreatedBy = userId;
                            kb.IsActive = true;

                            BMTDataContext.SubmitChanges();

                            KnowledgeBaseTemplate kbaseTempRec = (from kbaseRec in BMTDataContext.KnowledgeBaseTemplates
                                                                  where kbaseRec.TemplateId == templateId &&
                                                                  kbaseRec.KnowledgeBaseId == kbId &&
                                                                  kbaseRec.ParentKnowledgeBaseId == parentId
                                                                  select kbaseRec).FirstOrDefault();

                            if (kbaseTempRec != null)
                            {
                                kbaseTempRec.IsInfoDocEnable = isInfoDocsEnable;
                                kbaseTempRec.ReferencePages = pageReference;
                                BMTDataContext.SubmitChanges();
                            }
                        }
                    }
                    else if(kbTypeId == (int)enKBType.Header)
                    {
                        Template tempRec = (from tem in BMTDataContext.Templates
                                            where tem.TemplateId == templateId
                                            select tem).FirstOrDefault();

                        List<KnowledgeBaseTemplate> kBaseTemp = (from kBaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                 where kBaseTempRec.KnowledgeBaseId == kbId &&
                                                                 kBaseTempRec.TemplateId != templateId
                                                                 select kBaseTempRec).ToList();

                        if (kBaseTemp.Count != 0)
                        {
                            return "This item is share in other template and you can not Edit it.";
                        }

                        else
                        {
                            KnowledgeBase kb = (from kBaseRec in BMTDataContext.KnowledgeBases
                                                where kBaseRec.KnowledgeBaseId == kbId
                                                select kBaseRec).FirstOrDefault();

                            KnowledgeBase sameNameQuestion = (from kbase in BMTDataContext.KnowledgeBases
                                                              where kbase.Name == displayName &&
                                                              kbase.KnowledgeBaseId!=kbId
                                                              select kbase).FirstOrDefault();

                            if (sameNameQuestion != null)
                            {
                                if (kbTypeId == (int)enKBType.Header)
                                    return "Header Name Already Exists.";
                            }

                            kb.KnowledgeBaseTypeId = kbTypeId;
                            kb.Name = displayName;
                            kb.TabName = tabName;
                            kb.DisplayName = displayName;
                            kb.InstructionText = instruction;
                            kb.AccessId = tempRec.AccessLevelId;
                            kb.CreatedBy = userId;
                            kb.MustPass = mustPass;
                            kb.IsActive = true;

                            BMTDataContext.SubmitChanges();
                        }
                    }
                }
                if (list.Count != 0)
                {
                    foreach (int li in list)
                    {
                        KnowledgeBaseTemplate kbtemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                        where kbTempRec.TemplateId == li &&
                                                        kbTempRec.KnowledgeBaseId == kbId
                                                        select kbTempRec).FirstOrDefault();
                        kbtemp.KnowledgeBaseId = newkb;
                        BMTDataContext.SubmitChanges();
                    }
                }
                transaction.Commit();
                return "saved successfully";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "Error";
            }
        }

        public IQueryable BindSubHeader(int parentid, string userType)
        {
            IQueryable getSubHeader = null;
            try
            {
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    List<int> kbIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       select kbTempRec.KnowledgeBaseId).ToList();

                    List<int> KBs = (from kbase in BMTDataContext.KnowledgeBases
                                     where (!kbIds.Contains(kbase.KnowledgeBaseId)) &&
                                     kbase.KnowledgeBaseTypeId == (int)enKBType.SubHeader
                                     select kbase.KnowledgeBaseId).ToList();

                    getSubHeader = (from kb in BMTDataContext.KnowledgeBases
                                    join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                    on kb.KnowledgeBaseId equals kbtemp.KnowledgeBaseId into kbaseTemp
                                    from kbaseTempTable in kbaseTemp.DefaultIfEmpty()
                                    join tempAccess in BMTDataContext.AccessLevels
                                    on kb.AccessId equals tempAccess.AccessLevelId
                                    where (kb.KnowledgeBaseTypeId == (int)enKBType.SubHeader &&//for sub Header
                                     kbaseTempTable.ParentKnowledgeBaseId == parentid &&
                                    (tempAccess.AccessLevelName == "Public" ||
                                     tempAccess.AccessLevelName == "Practice" ||
                                     tempAccess.AccessLevelName == "Enterprise")) ||
                                     (KBs.Contains(kb.KnowledgeBaseId))
                                    select kb).Distinct().AsQueryable();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    List<int> kbIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       select kbTempRec.KnowledgeBaseId).ToList();

                    List<int> KBs = (from kbase in BMTDataContext.KnowledgeBases
                                     where (!kbIds.Contains(kbase.KnowledgeBaseId)) &&
                                     kbase.KnowledgeBaseTypeId == (int)enKBType.SubHeader
                                     select kbase.KnowledgeBaseId).ToList();

                    getSubHeader = (from kb in BMTDataContext.KnowledgeBases
                                    join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                    on kb.KnowledgeBaseId equals kbtemp.KnowledgeBaseId into kbaseTemp
                                    from kbaseTempTable in kbaseTemp.DefaultIfEmpty()
                                    join tempAccess in BMTDataContext.AccessLevels
                                    on kb.AccessId equals tempAccess.AccessLevelId
                                    where (kb.KnowledgeBaseTypeId == (int)enKBType.SubHeader &&//for sub Header
                                     kbaseTempTable.ParentKnowledgeBaseId == parentid &&
                                    (tempAccess.AccessLevelName == "Public" ||
                                     tempAccess.AccessLevelName == "Practice" ||
                                     tempAccess.AccessLevelName == "Enterprise")) ||
                                     (KBs.Contains(kb.KnowledgeBaseId))
                                    select kb).Distinct().AsQueryable();
                }

                return getSubHeader;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable BindQuestion(int parentid, string userType)
        {
            IQueryable getQuestion = null;
            try
            {
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    List<int> kbIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       select kbTempRec.KnowledgeBaseId).ToList();

                    List<int> KBs = (from kbase in BMTDataContext.KnowledgeBases
                                     where (!kbIds.Contains(kbase.KnowledgeBaseId)) &&
                                     kbase.KnowledgeBaseTypeId == (int)enKBType.Question
                                     select kbase.KnowledgeBaseId).ToList();

                    getQuestion = (from kb in BMTDataContext.KnowledgeBases
                                   join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                   on kb.KnowledgeBaseId equals kbtemp.KnowledgeBaseId into kbaseTemp
                                   from kbaseTempTable in kbaseTemp.DefaultIfEmpty()
                                   join tempAccess in BMTDataContext.AccessLevels
                                   on kb.AccessId equals tempAccess.AccessLevelId
                                   where (kb.KnowledgeBaseTypeId == (int)enKBType.Question &&//for Question
                                    kbaseTempTable.ParentKnowledgeBaseId == parentid &&
                                   (tempAccess.AccessLevelName == "Public" ||
                                    tempAccess.AccessLevelName == "Practice" ||
                                    tempAccess.AccessLevelName == "Enterprise")) ||
                                     (KBs.Contains(kb.KnowledgeBaseId))
                                   select kb).Distinct().AsQueryable();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    List<int> kbIds = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       select kbTempRec.KnowledgeBaseId).ToList();

                    List<int> KBs = (from kbase in BMTDataContext.KnowledgeBases
                                     where (!kbIds.Contains(kbase.KnowledgeBaseId)) &&
                                     kbase.KnowledgeBaseTypeId == (int)enKBType.Question
                                     select kbase.KnowledgeBaseId).ToList();

                    getQuestion = (from kb in BMTDataContext.KnowledgeBases
                                   join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                   on kb.KnowledgeBaseId equals kbtemp.KnowledgeBaseId into kbaseTemp
                                   from kbaseTempTable in kbaseTemp.DefaultIfEmpty()
                                   join tempAccess in BMTDataContext.AccessLevels
                                   on kb.AccessId equals tempAccess.AccessLevelId
                                   where (kb.KnowledgeBaseTypeId == (int)enKBType.Question &&//for Question
                                    kbaseTempTable.ParentKnowledgeBaseId == parentid &&
                                   (tempAccess.AccessLevelName == "Public" ||
                                    tempAccess.AccessLevelName == "Practice" ||
                                    tempAccess.AccessLevelName == "Enterprise")) ||
                                     (KBs.Contains(kb.KnowledgeBaseId))
                                   select kb).Distinct().AsQueryable();
                }
                return getQuestion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public String GetAccessBy(int tempId)
        {
            try
            {
                string accessBy = (from temp in BMTDataContext.Templates
                                   join tempAccess in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempAccess.AccessLevelId
                                   where temp.TemplateId == tempId
                                   select tempAccess.AccessLevelName).FirstOrDefault();
                return accessBy;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public String GetCreatedBy(int tempId)
        {
            try
            {
                string CreatedBy = (from temp in BMTDataContext.Templates
                                    join user in BMTDataContext.Users
                                    on temp.CreatedBy equals user.UserId
                                    where temp.TemplateId == tempId
                                    select user.Username).FirstOrDefault();

                return CreatedBy;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool KBAlreadyExist(string kbElementList, int templateId)
        {
            try
            {
                var ExistingKB = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                  join kbRec in BMTDataContext.KnowledgeBases
                                  on kbTempRec.KnowledgeBaseId equals kbRec.KnowledgeBaseId
                                  where kbTempRec.TemplateId == templateId &&
                                  kbRec.Name == kbElementList
                                  select kbTempRec).ToList();

                if (ExistingKB.Count != 0)
                    return true;

                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public KnowledgeBase GetKBElementByKBName(string KBName)
        {
            try
            {
                KnowledgeBase kbId = (from kbRec in BMTDataContext.KnowledgeBases
                                      where kbRec.Name == KBName
                                      select kbRec).FirstOrDefault();
                return kbId;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void DeleteKBElement(int kbid, int templateId)
        {
            try
            {
                var tempRec = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                               where kbTempRec.KnowledgeBaseId == kbid &&
                               kbTempRec.TemplateId == templateId
                               select kbTempRec).FirstOrDefault();

                var kbSub = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                             join kb in BMTDataContext.KnowledgeBases
                             on kbtemp.KnowledgeBaseId equals kb.KnowledgeBaseId
                             where kbtemp.ParentKnowledgeBaseId == kbid
                             select kbtemp).ToList();

                if (kbSub.Count != 0)
                {
                    foreach (var subElement in kbSub)
                    {
                        var temp = (from temRec in BMTDataContext.KnowledgeBaseTemplates
                                    where temRec.TemplateId == templateId &&
                                    temRec.KnowledgeBaseId == subElement.KnowledgeBaseId
                                    select temRec).FirstOrDefault();

                        var subQues = (from kbTemp in BMTDataContext.KnowledgeBaseTemplates
                                       join kb in BMTDataContext.KnowledgeBases
                                       on kbTemp.KnowledgeBaseId equals kb.KnowledgeBaseId
                                       where kbTemp.ParentKnowledgeBaseId == subElement.KnowledgeBaseId
                                       select kbTemp).ToList();

                        if (subQues.Count != 0)
                        {
                            foreach (var Ques in subQues)
                            {
                                var kbTemp = (from tem in BMTDataContext.KnowledgeBaseTemplates
                                              where tem.TemplateId == templateId &&
                                              tem.KnowledgeBaseId == Ques.KnowledgeBaseId
                                              select tem).FirstOrDefault();

                                if (kbTemp != null)
                                {
                                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbTemp);
                                    BMTDataContext.SubmitChanges();
                                }

                                var kbTempOther = (from tem in BMTDataContext.KnowledgeBaseTemplates
                                                   where tem.KnowledgeBaseId == Ques.KnowledgeBaseId
                                                   select tem).ToList();

                                if (kbTempOther.Count == 0)
                                {
                                    var kb = (from kbRecord in BMTDataContext.KnowledgeBases
                                              where kbRecord.KnowledgeBaseId == Ques.KnowledgeBaseId
                                              select kbRecord).FirstOrDefault();

                                    if (kb != null)
                                    {
                                        BMTDataContext.KnowledgeBases.DeleteOnSubmit(kb);
                                        BMTDataContext.SubmitChanges();
                                    }
                                }
                            }
                        }

                        if (temp != null)
                        {
                            BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(temp);
                            BMTDataContext.SubmitChanges();
                        }

                        var kbSubTempOther = (from tem in BMTDataContext.KnowledgeBaseTemplates
                                              where tem.KnowledgeBaseId == subElement.KnowledgeBaseId
                                              select tem).ToList();

                        if (kbSubTempOther.Count == 0)
                        {
                            var kb = (from kbRecord in BMTDataContext.KnowledgeBases
                                      where kbRecord.KnowledgeBaseId == subElement.KnowledgeBaseId
                                      select kbRecord).FirstOrDefault();

                            if (kb != null)
                            {
                                BMTDataContext.KnowledgeBases.DeleteOnSubmit(kb);
                                BMTDataContext.SubmitChanges();
                            }
                        }
                    }
                }
                if (tempRec != null)
                {
                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(tempRec);
                    BMTDataContext.SubmitChanges();
                }

                var kbElement = (from tem in BMTDataContext.KnowledgeBaseTemplates
                                 where tem.KnowledgeBaseId == kbid
                                 select tem).ToList();

                if (kbElement.Count == 0)
                {
                    KnowledgeBase kb = (from kbRecord in BMTDataContext.KnowledgeBases
                                        where kbRecord.KnowledgeBaseId == kbid
                                        select kbRecord).FirstOrDefault();

                    var childSubHeader = (from kbRec in BMTDataContext.KnowledgeBases
                                          join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                          on kbRec.KnowledgeBaseId equals kbtemp.KnowledgeBaseId
                                          where kbtemp.ParentKnowledgeBaseId == kb.KnowledgeBaseId
                                          select kbRec).ToList();

                    if (childSubHeader != null)
                    {
                        BMTDataContext.KnowledgeBases.DeleteOnSubmit(kb);
                        BMTDataContext.SubmitChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<KnowledgeBase> GetHeaders(string userType, int medicalGroupId)
        {
            try
            {
                List<KnowledgeBase> Headers = new List<KnowledgeBase>();

                Headers = (from kbRec in BMTDataContext.KnowledgeBases
                           where kbRec.KnowledgeBaseTypeId == (int)enKBType.Header//for Headers
                           select kbRec).ToList();

                return Headers;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<KnowledgeBase> GetSubHeaderList(int parentId, string userType, int medicalGroupId)
        {
            try
            {
                List<KnowledgeBase> subHeaders = new List<KnowledgeBase>();
                subHeaders = (from kbRec in BMTDataContext.KnowledgeBases
                              join kbtemp in BMTDataContext.KnowledgeBaseTemplates
                            on kbRec.KnowledgeBaseId equals kbtemp.KnowledgeBaseId
                              where kbtemp.ParentKnowledgeBaseId == parentId
                              select kbRec).Distinct().ToList();

                return subHeaders;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public List<KnowledgeBase> GetSubHeaders(string userType, int medicalGroupId)
        {
            try
            {
                List<KnowledgeBase> Headers = new List<KnowledgeBase>();

                Headers = (from kbRec in BMTDataContext.KnowledgeBases
                           where kbRec.KnowledgeBaseTypeId == (int)enKBType.SubHeader//for SubHeaders
                           select kbRec).ToList();

                return Headers;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool SaveHeaderInKBTemplate(int tempId, int kbId)
        {
            try
            {
                KnowledgeBaseTemplate kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                where kbTempRec.KnowledgeBaseId == kbId &&
                                                kbTempRec.TemplateId == tempId &&
                                                kbTempRec.ParentKnowledgeBaseId == null
                                                select kbTempRec).FirstOrDefault();
                if (kbTemp == null)
                {
                    KnowledgeBaseTemplate kbTempNew = new KnowledgeBaseTemplate();

                    kbTempNew.KnowledgeBaseId = kbId;
                    kbTempNew.TemplateId = tempId;

                    BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempNew);
                    BMTDataContext.SubmitChanges();
                    SetHeaderSequence(tempId);
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool SaveSubHeaderInKBTemplate(int tempId, int kbId, int parentId)
        {
            try
            {
                KnowledgeBaseTemplate kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                where kbTempRec.KnowledgeBaseId == kbId &&
                                                kbTempRec.TemplateId == tempId &&
                                                kbTempRec.ParentKnowledgeBaseId == parentId
                                                select kbTempRec).FirstOrDefault();

                if (kbTemp == null)
                {
                    KnowledgeBaseTemplate kbaseTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                       where kbaseTempRec.TemplateId == tempId &&
                                                       kbaseTempRec.KnowledgeBaseId == kbId
                                                       select kbaseTempRec).FirstOrDefault();
                    if (kbaseTemp != null)
                    {
                        return false;
                    }

                    KnowledgeBaseTemplate kbParent = (from kbTemParent in BMTDataContext.KnowledgeBaseTemplates
                                                      where kbTemParent.TemplateId == tempId &&
                                                      kbTemParent.KnowledgeBaseId == parentId &&
                                                      kbTemParent.ParentKnowledgeBaseId == null
                                                      select kbTemParent).FirstOrDefault();
                    if (kbParent == null)
                    {
                        KnowledgeBaseTemplate kbTempParent = new KnowledgeBaseTemplate();

                        kbTempParent.KnowledgeBaseId = parentId;
                        kbTempParent.TemplateId = tempId;

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempParent);
                        BMTDataContext.SubmitChanges();
                        SetHeaderSequence(tempId);
                    }

                    KnowledgeBaseTemplate kbTempNew = new KnowledgeBaseTemplate();

                    kbTempNew.KnowledgeBaseId = kbId;
                    kbTempNew.TemplateId = tempId;
                    kbTempNew.ParentKnowledgeBaseId = parentId;

                    BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempNew);
                    BMTDataContext.SubmitChanges();
                    SetSubHeaderSequence(tempId, parentId);
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool SaveQuestionInKBTemplate(int tempId, int kbId, int parentId, int grandParentId)
        {
            try
            {
                KnowledgeBaseTemplate kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                where kbTempRec.KnowledgeBaseId == kbId &&
                                                kbTempRec.TemplateId == tempId &&
                                                kbTempRec.ParentKnowledgeBaseId == parentId
                                                select kbTempRec).FirstOrDefault();
                if (kbTemp == null)
                {
                    KnowledgeBaseTemplate kbaseTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                       where kbaseTempRec.KnowledgeBaseId == kbId &&
                                                    kbaseTempRec.TemplateId == tempId
                                                       select kbaseTempRec).FirstOrDefault();
                    if (kbaseTemp != null)
                    {
                        return false;
                    }
                    KnowledgeBaseTemplate kbParent = (from kbTemParent in BMTDataContext.KnowledgeBaseTemplates
                                                      where kbTemParent.TemplateId == tempId &&
                                                      kbTemParent.KnowledgeBaseId == parentId &&
                                                      kbTemParent.ParentKnowledgeBaseId == grandParentId
                                                      select kbTemParent).FirstOrDefault();
                    if (kbParent == null)
                    {
                        KnowledgeBaseTemplate kbGrandParent = (from kbTemParent in BMTDataContext.KnowledgeBaseTemplates
                                                               where kbTemParent.TemplateId == tempId &&
                                                               kbTemParent.KnowledgeBaseId == grandParentId &&
                                                               kbTemParent.ParentKnowledgeBaseId == null
                                                               select kbTemParent).FirstOrDefault();
                        if (kbGrandParent == null)
                        {

                            KnowledgeBaseTemplate kbTempGrandParent = new KnowledgeBaseTemplate();

                            kbTempGrandParent.KnowledgeBaseId = grandParentId;
                            kbTempGrandParent.TemplateId = tempId;

                            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempGrandParent);
                            BMTDataContext.SubmitChanges();

                            SetHeaderSequence(tempId);
                        }

                        KnowledgeBaseTemplate kbTempParent = new KnowledgeBaseTemplate();

                        kbTempParent.KnowledgeBaseId = parentId;
                        kbTempParent.TemplateId = tempId;
                        kbTempParent.ParentKnowledgeBaseId = grandParentId;

                        BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempParent);
                        BMTDataContext.SubmitChanges();

                        SetSubHeaderSequence(tempId, grandParentId);
                    }

                    KnowledgeBaseTemplate kbTempAns = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                       where kbTempRec.KnowledgeBaseId == kbId
                                                       select kbTempRec).FirstOrDefault();

                    KnowledgeBaseTemplate kbTempNew = new KnowledgeBaseTemplate();

                    kbTempNew.KnowledgeBaseId = kbId;
                    kbTempNew.TemplateId = tempId;
                    kbTempNew.ParentKnowledgeBaseId = parentId;
                    if (kbTempAns != null)
                        kbTempNew.AnswerTypeId = kbTempAns.AnswerTypeId;

                    BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbTempNew);
                    BMTDataContext.SubmitChanges();
                    SetQuestionSequence(tempId, parentId);

                    int ParentKbTempId = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                          where kbaseTempRec.TemplateId == tempId &&
                                          kbaseTempRec.KnowledgeBaseId == parentId
                                          select kbaseTempRec.KnowledgeBaseTemplateId).FirstOrDefault();

                    if (ParentKbTempId != 0)
                    {
                        List<ScoringRule> scoreRules = (from scoreRec in BMTDataContext.ScoringRules
                                                        where scoreRec.KnowledgeBaseTemplateId == ParentKbTempId
                                                        select scoreRec).ToList();
                        if (scoreRules.Count != 0)
                        {
                            BMTDataContext.ScoringRules.DeleteAllOnSubmit(scoreRules);
                            BMTDataContext.SubmitChanges();
                        }
                        List<FilledAnswer> filledAnss = (from fillAnsRec in BMTDataContext.FilledAnswers
                                                         where fillAnsRec.KnowledgeBaseTemplateId == ParentKbTempId
                                                         select fillAnsRec).ToList();

                        if (filledAnss.Count != 0)
                        {
                            BMTDataContext.FilledAnswers.DeleteAllOnSubmit(filledAnss);
                            BMTDataContext.SubmitChanges();
                        }

                        List<AnswerTypeWeightage> ansType = (from ansTypeRec in BMTDataContext.AnswerTypeWeightages
                                                             where ansTypeRec.KnowledgeBaseTemplateId == ParentKbTempId
                                                             select ansTypeRec).ToList();

                        if (ansType.Count != 0)
                        {
                            BMTDataContext.AnswerTypeWeightages.DeleteAllOnSubmit(ansType);
                            BMTDataContext.SubmitChanges();
                        }
                    }
                }
                return true;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool DeleteKBFromTemplate(int templateId, int kbId, int parentId)
        {
            System.Data.Common.DbTransaction transaction;
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();
            BMTDataContext.Connection.Open();
            transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            try
            {
                KnowledgeBaseTemplate kbTemp = new KnowledgeBaseTemplate();
                if (parentId != 0)
                {
                    kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                              where kbTempRec.TemplateId == templateId &&
                              kbTempRec.KnowledgeBaseId == kbId &&
                              kbTempRec.ParentKnowledgeBaseId == parentId
                              select kbTempRec).FirstOrDefault();
                }
                else
                {
                    kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                              where kbTempRec.TemplateId == templateId &&
                              kbTempRec.KnowledgeBaseId == kbId
                              select kbTempRec).FirstOrDefault();
                }

                if (kbTemp != null)
                {
                    int ParentKbTempId = (from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                          where kbaseTemp.TemplateId == templateId &&
                                          kbaseTemp.KnowledgeBaseId == parentId
                                          select kbaseTemp.KnowledgeBaseTemplateId).FirstOrDefault();

                    List<ScoringRule> scoreRules = (from scoreRec in BMTDataContext.ScoringRules
                                                    where scoreRec.KnowledgeBaseTemplateId == kbTemp.KnowledgeBaseTemplateId ||
                                                    scoreRec.KnowledgeBaseTemplateId == ParentKbTempId
                                                    select scoreRec).ToList();
                    if (scoreRules.Count != 0)
                    {
                        BMTDataContext.ScoringRules.DeleteAllOnSubmit(scoreRules);
                        BMTDataContext.SubmitChanges();
                    }
                    List<FilledAnswer> filledAnss = (from fillAnsRec in BMTDataContext.FilledAnswers
                                                     where fillAnsRec.KnowledgeBaseTemplateId == kbTemp.KnowledgeBaseTemplateId
                                                     select fillAnsRec).ToList();

                    foreach (FilledAnswer fillAns in filledAnss)
                    {
                        List<FilledTemplateDocument> fillTempDoc = (from filledTempDocs in BMTDataContext.FilledTemplateDocuments
                                                                    where filledTempDocs.FilledAnswerId == fillAns.FilledAnswersId
                                                                    select filledTempDocs).ToList();
                        if (fillTempDoc.Count != 0)
                        {
                            BMTDataContext.FilledTemplateDocuments.DeleteAllOnSubmit(fillTempDoc);
                            BMTDataContext.SubmitChanges();
                        }
                    }
                    if (filledAnss.Count != 0)
                    {
                        BMTDataContext.FilledAnswers.DeleteAllOnSubmit(filledAnss);
                        BMTDataContext.SubmitChanges();
                    }

                    List<AnswerTypeWeightage> ansType = (from ansTypeRec in BMTDataContext.AnswerTypeWeightages
                                                         where ansTypeRec.KnowledgeBaseTemplateId == kbTemp.KnowledgeBaseTemplateId ||
                                                         ansTypeRec.KnowledgeBaseTemplateId == ParentKbTempId
                                                         select ansTypeRec).ToList();

                    if (ansType.Count != 0)
                    {
                        BMTDataContext.AnswerTypeWeightages.DeleteAllOnSubmit(ansType);
                        BMTDataContext.SubmitChanges();
                    }
                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbTemp);
                    BMTDataContext.SubmitChanges();

                    List<KnowledgeBase> childKb = (from kbChildRec in BMTDataContext.KnowledgeBases
                                                   join kbChildTemp in BMTDataContext.KnowledgeBaseTemplates
                                                   on kbChildRec.KnowledgeBaseId equals kbChildTemp.KnowledgeBaseId
                                                   where kbChildTemp.ParentKnowledgeBaseId == kbId
                                                   select kbChildRec).ToList();

                    foreach (KnowledgeBase kbChild in childKb)
                    {
                        KnowledgeBaseTemplate kbChildExist = (from kbChildTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                              where kbChildTempRec.TemplateId == templateId &&
                                                              kbChildTempRec.KnowledgeBaseId == kbChild.KnowledgeBaseId
                                                              select kbChildTempRec).FirstOrDefault();

                        if (kbChildExist != null)
                        {
                            List<ScoringRule> scoreRule = (from scoreRec in BMTDataContext.ScoringRules
                                                           where scoreRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                           select scoreRec).ToList();
                            if (scoreRule.Count != 0)
                            {
                                BMTDataContext.ScoringRules.DeleteAllOnSubmit(scoreRule);
                                BMTDataContext.SubmitChanges();
                            }
                            List<FilledAnswer> filledAns = (from fillAnsRec in BMTDataContext.FilledAnswers
                                                            where fillAnsRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                            select fillAnsRec).ToList();

                            foreach (FilledAnswer fillAns in filledAns)
                            {
                                List<FilledTemplateDocument> fillTempDoc = (from filledTempDocs in BMTDataContext.FilledTemplateDocuments
                                                                            where filledTempDocs.FilledAnswerId == fillAns.FilledAnswersId
                                                                            select filledTempDocs).ToList();
                                if (fillTempDoc.Count != 0)
                                {
                                    BMTDataContext.FilledTemplateDocuments.DeleteAllOnSubmit(fillTempDoc);
                                    BMTDataContext.SubmitChanges();
                                }
                            }
                            if (filledAns.Count != 0)
                            {
                                BMTDataContext.FilledAnswers.DeleteAllOnSubmit(filledAns);
                                BMTDataContext.SubmitChanges();
                            }
                            List<AnswerTypeWeightage> ansTypes = (from ansTypeRec in BMTDataContext.AnswerTypeWeightages
                                                                  where ansTypeRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                                  select ansTypeRec).ToList();

                            if (ansTypes.Count != 0)
                            {
                                BMTDataContext.AnswerTypeWeightages.DeleteAllOnSubmit(ansTypes);
                                BMTDataContext.SubmitChanges();
                            }
                            BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbChildExist);
                            BMTDataContext.SubmitChanges();

                            List<KnowledgeBase> grandChildKb = (from kbGrandChildRec in BMTDataContext.KnowledgeBases
                                                                join kbGrandTemp in BMTDataContext.KnowledgeBaseTemplates
                                                                on kbGrandChildRec.KnowledgeBaseId equals kbGrandTemp.KnowledgeBaseId
                                                                where kbGrandTemp.ParentKnowledgeBaseId == kbChild.KnowledgeBaseId
                                                                select kbGrandChildRec).ToList();

                            foreach (KnowledgeBase kbGrandChild in grandChildKb)
                            {
                                KnowledgeBaseTemplate kbGrangChildExist = (from kbGrandChildTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                                           where kbGrandChildTempRec.TemplateId == templateId &&
                                                                          kbGrandChildTempRec.KnowledgeBaseId == kbGrandChild.KnowledgeBaseId
                                                                           select kbGrandChildTempRec).FirstOrDefault();

                                if (kbGrangChildExist != null)
                                {
                                    List<ScoringRule> scoreRuled = (from scoreRec in BMTDataContext.ScoringRules
                                                                    where scoreRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                                    select scoreRec).ToList();
                                    if (scoreRuled.Count != 0)
                                    {
                                        BMTDataContext.ScoringRules.DeleteAllOnSubmit(scoreRuled);
                                        BMTDataContext.SubmitChanges();
                                    }
                                    List<FilledAnswer> filledAnswer = (from fillAnsRec in BMTDataContext.FilledAnswers
                                                                       where fillAnsRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                                       select fillAnsRec).ToList();

                                    foreach (FilledAnswer fillAns in filledAnswer)
                                    {
                                        List<FilledTemplateDocument> fillTempDoc = (from filledTempDocs in BMTDataContext.FilledTemplateDocuments
                                                                                    where filledTempDocs.FilledAnswerId == fillAns.FilledAnswersId
                                                                                    select filledTempDocs).ToList();
                                        if (fillTempDoc.Count != 0)
                                        {
                                            BMTDataContext.FilledTemplateDocuments.DeleteAllOnSubmit(fillTempDoc);
                                            BMTDataContext.SubmitChanges();
                                        }
                                    }
                                    if (filledAnswer.Count != 0)
                                    {
                                        BMTDataContext.FilledAnswers.DeleteAllOnSubmit(filledAnswer);
                                        BMTDataContext.SubmitChanges();
                                    }
                                    List<AnswerTypeWeightage> ansTyped = (from ansTypeRec in BMTDataContext.AnswerTypeWeightages
                                                                          where ansTypeRec.KnowledgeBaseTemplateId == kbChildExist.KnowledgeBaseTemplateId
                                                                          select ansTypeRec).ToList();

                                    if (ansTyped.Count != 0)
                                    {
                                        BMTDataContext.AnswerTypeWeightages.DeleteAllOnSubmit(ansTyped);
                                        BMTDataContext.SubmitChanges();
                                    }

                                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbGrangChildExist);
                                    BMTDataContext.SubmitChanges();
                                }
                            }
                        }
                    }
                }
                SetHeaderSequence(templateId);
                SetSubHeaderSequence(templateId, parentId);
                SetQuestionSequence(templateId, parentId);
                transaction.Commit();
                return true;
            }
            catch (Exception Ex)
            {
                transaction.Rollback();
                throw Ex;
            }
        }

        public bool IsKBExist(int tempId, int kbId, int parentId, int grandParentId)
        {
            try
            {
                KnowledgeBaseTemplate KbExist = new KnowledgeBaseTemplate();

                if (parentId != 0 && grandParentId == 0)
                {
                    KbExist = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                               where kbtemp.TemplateId == tempId &&
                               kbtemp.KnowledgeBaseId == kbId &&
                               kbtemp.ParentKnowledgeBaseId == parentId
                               select kbtemp).FirstOrDefault();
                }
                else if (parentId != 0 && grandParentId != 0)
                {
                    var parents = (from kbTemp in BMTDataContext.KnowledgeBaseTemplates
                                   where kbTemp.KnowledgeBaseId == parentId &&
                                   kbTemp.TemplateId == tempId
                                   select kbTemp).ToList();

                    if (parents.Count != 0)
                    {
                        foreach (var parent in parents)
                        {
                            if (parent.ParentKnowledgeBaseId == grandParentId)
                            {
                                KbExist = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                           where kbtemp.TemplateId == tempId &&
                                           kbtemp.KnowledgeBaseId == kbId &&
                                           kbtemp.ParentKnowledgeBaseId == parentId
                                           select kbtemp).FirstOrDefault();
                            }
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    KbExist = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                               where kbtemp.TemplateId == tempId &&
                               kbtemp.KnowledgeBaseId == kbId
                               select kbtemp).FirstOrDefault();
                }

                if (KbExist != null)
                    return true;

                else
                    return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetTemplateName(int tempId)
        {
            try
            {
                string tempName = (from tempRec in BMTDataContext.Templates
                                   where tempRec.TemplateId == tempId
                                   select tempRec.Name).FirstOrDefault();

                return tempName;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool CheckForEditingCriteria(int kbId, int templateId)
        {
            try
            {
                var kbTemp = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                              where kbTempRec.TemplateId != templateId &&
                              kbTempRec.KnowledgeBaseId == kbId
                              select kbTempRec).ToList();

                if (kbTemp.Count != 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int EditAnswer(int kbId, int TempId)
        {
            try
            {
                var AnswerType = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                  where kbTempRec.KnowledgeBaseId == kbId &&
                                  kbTempRec.TemplateId == TempId
                                  select kbTempRec.AnswerTypeId).FirstOrDefault();

                int AnswerTypeId = AnswerType == null ? 0 : Convert.ToInt32(AnswerType);

                return AnswerTypeId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int GetUniqueDocumentsCount(int projectUsageId,int siteId, int templateId)
        {
            try
            {
                var lstUniqueDocs = (from templateDocument in BMTDataContext.TemplateDocuments
                                     join filledTemplateDocument in BMTDataContext.FilledTemplateDocuments
                                     on templateDocument.DocumentId equals filledTemplateDocument.DocumentId
                                     join filledAnswer in BMTDataContext.FilledAnswers
                                     on filledTemplateDocument.FilledAnswerId equals filledAnswer.FilledAnswersId
                                     join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                     on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                     where templateDocument.DocumentType != enDocType.UnAssociatedDoc.ToString()
                                     && filledAnswer.ProjectUsageId == projectUsageId
                                     && filledAnswer.PracticeSiteId==siteId
                                     && kbTemplate.TemplateId == templateId
                                     select templateDocument.Path).Distinct();

                return lstUniqueDocs.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTemplateIdByProjectId(int projectId)
        {
            try
            {
                int templateId = (from filledAnswer in BMTDataContext.FilledAnswers
                                  join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                  on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                  //where filledAnswer.ProjectId == projectId
                                  select kbTemplate.TemplateId).FirstOrDefault();

                return templateId;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgeBase> GetKnowledgeBaseByTypeAndTemplateId(enKBType kbType, int templateId)
        {
            try
            {
                var lstKnowledgeBase = (from kbtemp in BMTDataContext.KnowledgeBaseTemplates
                                        join kb in BMTDataContext.KnowledgeBases
                                        on kbtemp.KnowledgeBaseId equals kb.KnowledgeBaseId
                                        where kb.KnowledgeBaseTypeId == (int)kbType
                                        && kbtemp.TemplateId == templateId
                                        select kb).ToList();

                return lstKnowledgeBase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetHeaderMaxPointByHeaderId(int headerId, int TemplateId)
        {
            try
            {
                int? maxpoint = (from kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                 where kbTemplate.ParentKnowledgeBaseId == headerId && kbTemplate.TemplateId == TemplateId
                                 select kbTemplate.MaxPoints).Sum();

                return maxpoint == null ? "0" : maxpoint.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetMaxPointByKnowledgeId(int knowledgeBaseId, int TemplateId)
        {
            try
            {
                int? maxpoint = (from kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                 where kbTemplate.KnowledgeBaseId == knowledgeBaseId && kbTemplate.TemplateId == TemplateId
                                 select kbTemplate.MaxPoints).FirstOrDefault();

                return maxpoint == null ? "0" : maxpoint.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgeBase> GetKnowledgeBaseByParentId(int parentKBId, int templateId)
        {
            try
            {
                var lstKnowledgeBase = (from kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                        join kb in BMTDataContext.KnowledgeBases
                                        on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                        where kbTemplate.ParentKnowledgeBaseId == parentKBId && kbTemplate.TemplateId == templateId
                                        select kb).ToList();

                return lstKnowledgeBase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FilledAnswer GetFilledAnswerByProjectIdAndKnowledgeBaseId(int projectUsageId,int siteId, int knowledgeBaseId, int TemplateId)
        {
            FilledAnswer filledAnswerRecord = (from filledAnswer in BMTDataContext.FilledAnswers
                                               join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                               on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                               join kb in BMTDataContext.KnowledgeBases
                                               on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                               where kb.KnowledgeBaseId == knowledgeBaseId
                                               && filledAnswer.ProjectUsageId == projectUsageId 
                                               && filledAnswer.PracticeSiteId==siteId
                                               && kbTemplate.TemplateId == TemplateId
                                               select filledAnswer).FirstOrDefault();

            return filledAnswerRecord;
        }

        public int GetUploadedDocsCount(int knowledgeBaseId, int projectUsageId,int siteId, int TemplateId)
        {
            try
            {
                int docsCount = (from filledTemplateDocs in BMTDataContext.FilledTemplateDocuments
                                 join templateDocs in BMTDataContext.TemplateDocuments
                                 on filledTemplateDocs.DocumentId equals templateDocs.DocumentId
                                 join filledAnswer in BMTDataContext.FilledAnswers
                                 on filledTemplateDocs.FilledAnswerId equals filledAnswer.FilledAnswersId
                                 join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                 on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                 join kb in BMTDataContext.KnowledgeBases
                                 on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                 where kbTemplate.ParentKnowledgeBaseId == knowledgeBaseId
                                 && filledAnswer.ProjectUsageId == projectUsageId
                                 && filledAnswer.PracticeSiteId==siteId
                                 select templateDocs).Count();
                return docsCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? GetRequiredDocsCount(int projectId, int knowledgeBaseId)
        {
            try
            {
                int? filledAnswerRecord = (from filledAnswer in BMTDataContext.FilledAnswers
                                           join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                           on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                           join kb in BMTDataContext.KnowledgeBases
                                           on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                           where kb.KnowledgeBaseId == knowledgeBaseId
                                           //&& filledAnswer.ProjectId == projectId
                                           select
                                               filledAnswer.PoliciesDocumentCount +
                                               filledAnswer.ReportsDocumentCount +
                                               filledAnswer.ScreenShotsDocumentCount +
                                               filledAnswer.LogsOrToolsDocumentCount +
                                               filledAnswer.OtherDocumentsCount
                                           ).FirstOrDefault();

                return filledAnswerRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? GetSubHeaderRequiredDocsCount(int projectUsageId,int SiteId, int parentId, int TemplateId)
        {
            try
            {
                var fdfilledAnswerRecord = (from filledAnswer in BMTDataContext.FilledAnswers
                                            join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                            on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                            join kb in BMTDataContext.KnowledgeBases
                                            on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                            where kbTemplate.ParentKnowledgeBaseId == parentId
                                            && filledAnswer.ProjectUsageId == projectUsageId 
                                            && filledAnswer.PracticeSiteId==SiteId
                                            && kbTemplate.TemplateId == TemplateId
                                            select filledAnswer);

                int? filledAnswerRecord = (from filledAnswer in BMTDataContext.FilledAnswers
                                           join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                           on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                           join kb in BMTDataContext.KnowledgeBases
                                           on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                           where kbTemplate.ParentKnowledgeBaseId == parentId
                                           && filledAnswer.ProjectUsageId == projectUsageId
                                           && filledAnswer.PracticeSiteId==SiteId
                                           && kbTemplate.TemplateId == TemplateId
                                           select
                                               filledAnswer.PoliciesDocumentCount +
                                               filledAnswer.ReportsDocumentCount +
                                               filledAnswer.ScreenShotsDocumentCount +
                                               filledAnswer.LogsOrToolsDocumentCount +
                                               filledAnswer.OtherDocumentsCount
                                           ).Sum();

                return (filledAnswerRecord != null ? filledAnswerRecord : 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckTemplateScoringRules(string SubHeaderId, string tempId)
        {
            try
            {
                int kbaseTempId = (from kBaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                   where kBaseTempRec.KnowledgeBaseId == Convert.ToInt32(SubHeaderId) &&
                                   kBaseTempRec.TemplateId == Convert.ToInt32(tempId)
                                   select kBaseTempRec.KnowledgeBaseTemplateId).FirstOrDefault();

                var scRules = (from scrulesRec in BMTDataContext.ScoringRules
                               where scrulesRec.KnowledgeBaseTemplateId == kbaseTempId
                               select scrulesRec).ToList();

                if (scRules.Count != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool DeleteScoringRules(int subHeaderId, int templateId)
        {
            try
            {
                int kbaseTempId = (from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                   where kbaseTemp.KnowledgeBaseId == subHeaderId &&
                                   kbaseTemp.TemplateId == templateId
                                   select kbaseTemp.KnowledgeBaseTemplateId).FirstOrDefault();

                List<ScoringRule> scRules = (from scRulesRec in BMTDataContext.ScoringRules
                                             where scRulesRec.KnowledgeBaseTemplateId == kbaseTempId
                                             select scRulesRec).ToList();

                if (scRules.Count != 0)
                {
                    BMTDataContext.ScoringRules.DeleteAllOnSubmit(scRules);
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public string EditIsCritical(int knowledgebaseId, int tempId)
        {
            try
            {
                string IsCritical = (from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                     where kbaseTemp.KnowledgeBaseId == knowledgebaseId &&
                                     kbaseTemp.TemplateId == tempId
                                     select (kbaseTemp.IsCritical == true) ? "Yes" : "No").FirstOrDefault();

                return IsCritical;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public KnowledgeBaseTemplate EditkbTemp(int knowledgebaseId, int tempId)
        {
            try
            {
                KnowledgeBaseTemplate kbaseTemp = (from kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                                   where kbaseTempRec.KnowledgeBaseId == knowledgebaseId &&
                                                   kbaseTempRec.TemplateId == tempId
                                                   select kbaseTempRec).FirstOrDefault();
                return kbaseTemp;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<TemplateDocument> GetDocumentsByKnowledgeBaseIdAndProjectId(int projectUsageId, int practiceSiteId, int knowledgeBaseId)
        {
            try
            {
                var lstTemplateDocs = (from filledTemplateDocs in BMTDataContext.FilledTemplateDocuments
                                       join templateDocs in BMTDataContext.TemplateDocuments
                                       on filledTemplateDocs.DocumentId equals templateDocs.DocumentId
                                       join filledAnswer in BMTDataContext.FilledAnswers
                                       on filledTemplateDocs.FilledAnswerId equals filledAnswer.FilledAnswersId
                                       join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                       on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                       join kb in BMTDataContext.KnowledgeBases
                                       on kbTemplate.KnowledgeBaseId equals kb.KnowledgeBaseId
                                       where kbTemplate.ParentKnowledgeBaseId == knowledgeBaseId
                                       && filledAnswer.ProjectUsageId == projectUsageId
                                       && filledAnswer.PracticeSiteId == practiceSiteId
                                       select templateDocs).ToList<TemplateDocument>();

                return lstTemplateDocs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetHeaderSequence(int templateId)
        {
            try
            {
                List<KnowledgeBaseTemplate> kbTemplate = (from kb in BMTDataContext.KnowledgeBases
                                                          join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                          on kb.KnowledgeBaseId equals kbaseTemp.KnowledgeBaseId
                                                          where kbaseTemp.TemplateId == templateId &&
                                                          kb.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Header
                                                          select kbaseTemp).ToList();

                for (int headerCount = 0; headerCount < kbTemplate.Count; headerCount++)
                {
                    kbTemplate[headerCount].Sequence = headerCount + 1;
                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void SetSubHeaderSequence(int templateId, int parentId)
        {
            try
            {
                List<KnowledgeBaseTemplate> kbTemplate = (from kb in BMTDataContext.KnowledgeBases
                                                          join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                          on kb.KnowledgeBaseId equals kbaseTemp.KnowledgeBaseId
                                                          where kbaseTemp.TemplateId == templateId &&
                                                          kbaseTemp.ParentKnowledgeBaseId == parentId &&
                                                          kb.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.SubHeader
                                                          select kbaseTemp).ToList();

                for (int subHeaderCount = 0; subHeaderCount < kbTemplate.Count; subHeaderCount++)
                {
                    kbTemplate[subHeaderCount].Sequence = subHeaderCount + 1;
                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void SetQuestionSequence(int templateId, int parentId)
        {
            try
            {
                List<KnowledgeBaseTemplate> kbTemplate = (from kb in BMTDataContext.KnowledgeBases
                                                          join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                                                          on kb.KnowledgeBaseId equals kbaseTemp.KnowledgeBaseId
                                                          where kbaseTemp.TemplateId == templateId &&
                                                          kbaseTemp.ParentKnowledgeBaseId == parentId &&
                                                          kb.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.Question
                                                          select kbaseTemp).ToList();

                for (int questionCount = 0; questionCount < kbTemplate.Count; questionCount++)
                {
                    kbTemplate[questionCount].Sequence = questionCount + 1;
                    BMTDataContext.SubmitChanges();
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public KnowledgeBaseTemplate GetSubHeaderByTemplateDocumentId(int documentId)
        {
            try
            {
                KnowledgeBaseTemplate knowledgeBaseTemplate = (from filledTemplateDocs in BMTDataContext.FilledTemplateDocuments
                                                               join templateDocs in BMTDataContext.TemplateDocuments
                                                               on filledTemplateDocs.DocumentId equals templateDocs.DocumentId
                                                               join filledAnswer in BMTDataContext.FilledAnswers
                                                               on filledTemplateDocs.FilledAnswerId equals filledAnswer.FilledAnswersId
                                                               join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                               on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                                               join kb in BMTDataContext.KnowledgeBases
                                                               on kbTemplate.ParentKnowledgeBaseId equals kb.KnowledgeBaseId
                                                               join parentKBTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                               on kb.KnowledgeBaseId equals parentKBTemplate.KnowledgeBaseTemplateId
                                                               where templateDocs.DocumentId == documentId
                                                               select parentKBTemplate).Single();

                return knowledgeBaseTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public KnowledgeBaseTemplate GetHeaderByParentKBTemplateId(int knowledgeBaseTemplateId)
        {
            try
            {
                KnowledgeBaseTemplate knowledgeBaseTemplate = (from kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                               join knowledgeBase in BMTDataContext.KnowledgeBases
                                                               on kbTemplate.ParentKnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                                               join parentKBTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                               on knowledgeBase.KnowledgeBaseId equals parentKBTemplate.KnowledgeBaseId
                                                               where kbTemplate.KnowledgeBaseTemplateId == knowledgeBaseTemplateId
                                                               select parentKBTemplate).Single();
                return knowledgeBaseTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TemplateDocument> GetLinkedToTemplateDocuments(int projectUsageId, int practiceSiteId, int templateId, string path, string documentRootDirectory)
        {
            try
            {
                var lstDocuments = (from templateDocument in BMTDataContext.TemplateDocuments
                                    join filledTemplateDocument in BMTDataContext.FilledTemplateDocuments
                                    on templateDocument.DocumentId equals filledTemplateDocument.DocumentId
                                    join filledAnswer in BMTDataContext.FilledAnswers
                                    on filledTemplateDocument.FilledAnswerId equals filledAnswer.FilledAnswersId
                                    join kbTemplate in BMTDataContext.KnowledgeBaseTemplates
                                    on filledAnswer.KnowledgeBaseTemplateId equals kbTemplate.KnowledgeBaseTemplateId
                                    where templateDocument.DocumentType != enDocType.UnAssociatedDoc.ToString()
                                    && filledAnswer.ProjectUsageId == projectUsageId
                                    && filledAnswer.PracticeSiteId == practiceSiteId
                                    && kbTemplate.TemplateId == templateId
                                    && templateDocument.Path.Substring(templateDocument.Path.IndexOf(documentRootDirectory)) == path
                                    select templateDocument).ToList();

                return lstDocuments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

