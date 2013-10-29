using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.IO;

using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class ProjectTemplateBO : BMTConnection
    {
        #region PROPERTIES
        private Template _template { get; set; }

        private int _TemplateId;
        public int TemplateId
        {
            get { return _TemplateId; }
            set { _TemplateId = value; }
        }
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private String _ShortName;
        public String ShortName
        {
            get { return _ShortName; }
            set { _ShortName = value; }
        }
        private String _Description;
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private int _TemplateTypeId;
        public int TemplateTypeId
        {
            get { return _TemplateTypeId; }
            set { _TemplateTypeId = value; }
        }
        private int _TemplateCategoryId;
        public int TemplateCategoryId
        {
            get { return _TemplateCategoryId; }
            set { _TemplateCategoryId = value; }
        }
        private int _TemplateAccessId;
        public int TemplateAccessId
        {
            get { return _TemplateAccessId; }
            set { _TemplateAccessId = value; }
        }
        private int _CreatedBy;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        private int _LastUpdatedBy;
        public int LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }
        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }
        private int _SubmittedActionId;
        public int SubmittedActionId
        {
            get { return _SubmittedActionId; }
            set { _SubmittedActionId = value; }
        }
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        private int _PracticeId;
        public int PracticeId
        {
            get { return _PracticeId; }
            set { _PracticeId = value; }
        }
        private int _MedicalGroupId;
        public int MedicalGroupId
        {
            get { return _MedicalGroupId; }
            set { _MedicalGroupId = value; }
        }
        private int _EnterpriseId;
        public int EnterpriseId
        {
            get { return _EnterpriseId; }
            set { _EnterpriseId = value; }
        }
        private string _DocPath;
        public string DocPath
        {
            get { return _DocPath; }
            set { _DocPath = value; }
        }
        private bool _HasStandardFolder;
        public bool HasStandardFolder
        {
            get { return _HasStandardFolder; }
            set { _HasStandardFolder = value; }
        }
        private bool _HasDocumentStore;
        public bool HasDocumentStore
        {
            get { return _HasDocumentStore; }
            set { _HasDocumentStore = value; }
        }
        private String _DocumentStoreName;
        public String DocumentStoreName
        {
            get { return _DocumentStoreName; }
            set { _DocumentStoreName = value; }
        }
        private int _StandardFolderId;
        public int StandardFolderId
        {
            get { return _StandardFolderId; }
            set { _StandardFolderId = value; }
        }
        private int _ToolLevelId;
        public int ToolLevelId
        {
            get { return _ToolLevelId; }
            set { _ToolLevelId = value; }
        }
        #endregion

        #region CONSTANT
        int? NULL_STRING = null;
        int ADD_SUB_TO_MAX = 1;

        private const string TEMPLATE_UTILITY_FOLDER = "Utility Folder";
        private const string TEMPLATE_UPLOADED_DOCUMENT_FOLDER = "Uploaded Documents";
        private const string TEMPLATE_UPLOADED_DOCUMENT_CONTENT_TYPE = "UploadedDocuments";

        #endregion

        #region FUNCTIONS
        public IQueryable GetTemplateType()
        {
            IQueryable templateType = null;
            try
            {
                templateType = (from tempType in BMTDataContext.TemplateTypes
                                select tempType).AsQueryable();

                return templateType;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetStandardFolder()
        {
            IQueryable standardFolder = null;
            try
            {
                standardFolder = (from standFold in BMTDataContext.StandardFolders
                                  select standFold).AsQueryable();
                return standardFolder;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable GetTemplateCategory()
        {
            IQueryable templateCategory = null;
            try
            {
                templateCategory = (from tempCat in BMTDataContext.TemplateCategories
                                    select tempCat).AsQueryable();

                return templateCategory;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetAllowAccess()
        {
            IQueryable allowAccess = null;
            try
            {
                allowAccess = (from access in BMTDataContext.AccessLevels
                               select access).AsQueryable();

                return allowAccess;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetSubmittedToList()
        {
            IQueryable submitTo = null;
            try
            {
                submitTo = (from subTo in BMTDataContext.TemplateSubmitActions
                            select subTo).AsQueryable();

                return submitTo;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveTemplate()
        {
            try
            {
                _template = new Template();
                _template.Name = this.Name;
                _template.ShortName = this.ShortName;
                _template.Description = this.Description;
                _template.TemplateTypeId = this.TemplateTypeId;
                _template.TemplateCategoryId = this.TemplateCategoryId;
                _template.AccessLevelId = this.TemplateAccessId;
                _template.CreatedBy = this.CreatedBy;
                _template.CreatedDate = this.CreatedDate;
                _template.LastUpdatedBy = this.LastUpdatedBy;
                _template.LastUpdatedDate = this.LastUpdatedDate;
                _template.SubmitActionId = this.SubmittedActionId;
                _template.IsActive = this.IsActive;
                _template.HasStandardFolder = this.HasStandardFolder;
                _template.HasDocumentStore = this._HasDocumentStore;
                _template.DocumentStoreName = this.DocumentStoreName;
                _template.StandardFolderId = this.StandardFolderId;
                _template.ToolLevelId = this.ToolLevelId;

                if (_template.StandardFolderId == 0)
                {
                    _template.StandardFolderId = null;
                }

                if (this.TemplateAccessId != 1)//if not public template then save practiceId otherwise practiceId= Null
                {
                    _template.PracticeId = this.PracticeId;
                    _template.EnterpriseId = this.EnterpriseId;
                }
                else
                {
                    _template.PracticeId = NULL_STRING;
                    _template.EnterpriseId = NULL_STRING;
                }
                //this.DocPath = this.DocPath.Replace("&", "&amp;");
                _template.DocPath = (this.DocPath == null ? null : this.DocPath);

                BMTDataContext.Templates.InsertOnSubmit(_template);
                BMTDataContext.SubmitChanges();

                //ProjectSection pSection = new ProjectSection();

                //pSection.ParentProjectSectionId = 0;
                //pSection.Name = TEMPLATE_UTILITY_FOLDER + " - " + _template.TemplateId;


                //BMTDataContext.ProjectSections.InsertOnSubmit(pSection);
                //BMTDataContext.SubmitChanges();

                //ProjectSection pSectionChild = new ProjectSection();

                //pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
                //pSectionChild.Name = TEMPLATE_UPLOADED_DOCUMENT_FOLDER;
                //pSectionChild.ContentType = TEMPLATE_UPLOADED_DOCUMENT_CONTENT_TYPE;

                //BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                //BMTDataContext.SubmitChanges();

                return true;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public int GetTemplateId(string Name)
        {

            int Id = (from temp in BMTDataContext.Templates
                      where temp.Name == Name
                      select temp.TemplateId).FirstOrDefault();

            return Id;

        }

        public bool UpdateTemplate(int templateId)
        {
            try
            {
                var template = (from temp in BMTDataContext.Templates
                                where temp.TemplateId == templateId
                                select temp).First();

                template.Name = this.Name;
                template.ShortName = this.ShortName;
                template.Description = this.Description;
                template.TemplateTypeId = this.TemplateTypeId;
                template.TemplateCategoryId = this.TemplateCategoryId;
                template.AccessLevelId = this.TemplateAccessId;
                template.LastUpdatedBy = this.LastUpdatedBy;
                template.LastUpdatedDate = this.LastUpdatedDate;
                template.SubmitActionId = this.SubmittedActionId;
                template.HasStandardFolder = this.HasStandardFolder;
                template.HasDocumentStore = this._HasDocumentStore;
                template.DocumentStoreName = this.DocumentStoreName;
                template.StandardFolderId = this.StandardFolderId;
                template.ToolLevelId = this.ToolLevelId;
                if (template.HasStandardFolder == false)
                {
                    template.StandardFolderId = 0;
                }

                if (template.StandardFolderId == 0)
                {
                    template.StandardFolderId = null;
                }

                if (template.HasDocumentStore == false)
                {
                    template.DocumentStoreName = string.Empty;
                }

                if (this.IsActive)
                {
                    template.IsActive = this.IsActive;
                }
                else
                {
                    List<ProjectSection> pracTemp = (from projSecRec in BMTDataContext.ProjectSections
                                                     where projSecRec.TemplateId == template.TemplateId
                                                     select projSecRec).ToList();

                    if (pracTemp.Count != 0)
                    {
                        return false;
                    }
                    else
                    {
                        template.IsActive = this.IsActive;
                    }
                }
                if (this.TemplateAccessId != 1)//if not public template then save practiceId otherwise practiceId= Null
                {
                    template.PracticeId = this.PracticeId;
                    template.EnterpriseId = this.EnterpriseId;
                }
                else
                {
                    template.PracticeId = NULL_STRING;
                    template.EnterpriseId = NULL_STRING;
                }
                template.DocPath = (this.DocPath == null ? null : this.DocPath);

                BMTDataContext.SubmitChanges();
                return true;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetTemplate(int practiceId, string UserType, int enterpriseId)
        {
            IQueryable getTemplate = null;
            try
            {
                if (UserType == enUserRole.User.ToString() || UserType == enUserRole.Consultant.ToString())
                {
                    getTemplate = (from temp in BMTDataContext.Templates
                                   join tempcat in BMTDataContext.TemplateCategories
                                   on temp.TemplateCategoryId equals tempcat.TemplateCategoryId
                                   join tempacc in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempacc.AccessLevelId
                                   where temp.PracticeId == practiceId
                                   select new
                                   {
                                       TemplateId = temp.TemplateId,
                                       Name = temp.Name,
                                       TemplateCategory = tempcat.TemplateCategory1,
                                       Description = temp.Description,
                                       TemplateAccess = tempacc.AccessLevelName,
                                       CreatedDate = temp.CreatedDate.ToShortDateString(),
                                       IsActive = (temp.IsActive == true) ? "Active" : "Inactive"
                                   }) as IQueryable;
                }
                else if (UserType == enUserRole.SuperUser.ToString())
                {
                    getTemplate = (from temp in BMTDataContext.Templates
                                   join tempcat in BMTDataContext.TemplateCategories
                                   on temp.TemplateCategoryId equals tempcat.TemplateCategoryId
                                   join tempacc in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempacc.AccessLevelId
                                   where (temp.PracticeId == practiceId ||
                                   temp.AccessLevelId == 2) &&
                                   temp.EnterpriseId == enterpriseId
                                   select new
                                   {
                                       TemplateId = temp.TemplateId,
                                       Name = temp.Name,
                                       TemplateCategory = tempcat.TemplateCategory1,
                                       Description = temp.Description,
                                       TemplateAccess = tempacc.AccessLevelName,
                                       CreatedDate = temp.CreatedDate.ToShortDateString(),
                                       IsActive = (temp.IsActive == true) ? "Active" : "Inactive"
                                   }) as IQueryable;
                }
                else if (UserType == enUserRole.SuperAdmin.ToString())
                {

                    getTemplate = (from temp in BMTDataContext.Templates
                                   join tempcat in BMTDataContext.TemplateCategories
                                   on temp.TemplateCategoryId equals tempcat.TemplateCategoryId
                                   join tempacc in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempacc.AccessLevelId
                                   where temp.PracticeId == practiceId ||
                                   temp.AccessLevelId == 1 ||//for public 
                                   (temp.AccessLevelId == 2 &&// for Enterprise
                                   temp.EnterpriseId == enterpriseId)
                                   select new
                                   {
                                       TemplateId = temp.TemplateId,
                                       Name = temp.Name,
                                       TemplateCategory = tempcat.TemplateCategory1,
                                       Description = temp.Description,
                                       TemplateAccess = tempacc.AccessLevelName,
                                       CreatedDate = temp.CreatedDate.ToShortDateString(),
                                       IsActive = (temp.IsActive == true) ? "Active" : "Inactive"
                                   }) as IQueryable;
                }
                return getTemplate;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Template GetTemplateByTemplateId(int templateId)
        {
            Template getTemplate = null;
            try
            {
                getTemplate = (from temp in BMTDataContext.Templates
                               where temp.TemplateId == templateId
                               select temp).FirstOrDefault();

                return getTemplate;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetCategory()
        {
            try
            {
                List<string> Cat = (from temp in BMTDataContext.Templates
                                    join temcat in BMTDataContext.TemplateCategories
                                    on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                                    select temcat.TemplateCategory1).Distinct().ToList<string>();
                return Cat;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetTemplateByTemplateCategory(string tempcat, int practiceId, string userType, int enterpriseId)
        {
            try
            {
                List<string> TempName = new List<string>();
                if (userType == enUserRole.User.ToString())
                {
                    TempName = (from temp in BMTDataContext.Templates
                                join temcat in BMTDataContext.TemplateCategories
                                on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                                join tempAccess in BMTDataContext.AccessLevels
                                on temp.AccessLevelId equals tempAccess.AccessLevelId
                                where temcat.TemplateCategory1 == tempcat &&
                               (((temp.PracticeId == null ||
                                temp.PracticeId == practiceId) &&
                                temp.EnterpriseId == enterpriseId) ||
                                tempAccess.AccessLevelName == "Public")
                                select temp.Name).Distinct().ToList<string>();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    TempName = (from temp in BMTDataContext.Templates
                                join temcat in BMTDataContext.TemplateCategories
                                on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                                join tempAccess in BMTDataContext.AccessLevels
                                on temp.AccessLevelId equals tempAccess.AccessLevelId
                                where temcat.TemplateCategory1 == tempcat &&
                                (temp.EnterpriseId == enterpriseId ||
                                temp.EnterpriseId == null)
                                select temp.Name).ToList<string>();
                }
                else if (userType == enUserRole.SuperAdmin.ToString())
                {
                    TempName = (from temp in BMTDataContext.Templates
                                join temcat in BMTDataContext.TemplateCategories
                                on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                                where temcat.TemplateCategory1 == tempcat
                                select temp.Name).ToList<string>();
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    TempName = ((from temp in BMTDataContext.Templates
                                 join temcat in BMTDataContext.TemplateCategories
                                 on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                                 join pracCons in BMTDataContext.PracticeConsultants
                                 on temp.PracticeId equals pracCons.PracticeId
                                 where temcat.TemplateCategory1 == tempcat &&
                                 (
                                 pracCons.ConsultantId == (from consId in BMTDataContext.PracticeConsultants
                                                           where consId.PracticeId == practiceId
                                                           select consId.ConsultantId).First() &&
                                 temp.EnterpriseId == enterpriseId)
                                 select temp.Name).Concat
                        (from temp in BMTDataContext.Templates
                         join temcat in BMTDataContext.TemplateCategories
                         on temp.TemplateCategoryId equals temcat.TemplateCategoryId
                         join tempAccess in BMTDataContext.AccessLevels
                         on temp.AccessLevelId equals tempAccess.AccessLevelId
                         where temcat.TemplateCategory1 == tempcat &&
                         tempAccess.AccessLevelName == "Public"
                         select temp.Name)).Distinct().ToList<string>();

                }
                return TempName;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsTemplateNameAvailable()
        {
            try
            {
                var tempRec = (from temp in BMTDataContext.Templates
                               where temp.Name == this.Name
                               select temp).ToList();
                if (tempRec.Count > 0)
                    return false;

                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool AddTemplateWithCopyExistingTemplate(string[] names)
        {
            try
            {
                _template = new Template();
                _template.Name = this.Name;
                _template.ShortName = this.ShortName;
                _template.Description = this.Description;
                _template.TemplateTypeId = this.TemplateTypeId;
                _template.TemplateCategoryId = this.TemplateCategoryId;
                _template.AccessLevelId = this.TemplateAccessId;
                _template.CreatedBy = this.CreatedBy;
                _template.CreatedDate = this.CreatedDate;
                _template.LastUpdatedBy = this.LastUpdatedBy;
                _template.LastUpdatedDate = this.LastUpdatedDate;
                _template.SubmitActionId = this.SubmittedActionId;
                _template.DocPath = (this.DocPath == null ? null : this.DocPath);
                _template.IsActive = this.IsActive;
                _template.HasStandardFolder = this.HasStandardFolder;
                _template.HasDocumentStore = this._HasDocumentStore;
                _template.DocumentStoreName = this.DocumentStoreName;
                _template.StandardFolderId = this.StandardFolderId;
                _template.ToolLevelId = this.ToolLevelId;
                if (_template.StandardFolderId == 0)
                {
                    _template.StandardFolderId = null;
                }


                if (this.TemplateAccessId != 1)//if not public template then save practiceId otherwise practiceId= Null
                {
                    _template.PracticeId = this.PracticeId;
                    _template.EnterpriseId = this.EnterpriseId;
                }
                else
                {
                    _template.PracticeId = NULL_STRING;
                    _template.EnterpriseId = NULL_STRING;
                }

                BMTDataContext.Templates.InsertOnSubmit(_template);
                BMTDataContext.SubmitChanges();
                int tempId = _template.TemplateId;

                List<int> temRec = (from temp in BMTDataContext.Templates
                                    where names.Contains(temp.Name)
                                    select temp.TemplateId).ToList();

                var kbts = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                            where temRec.Contains(kbtRec.TemplateId)
                            group kbtRec by kbtRec.KnowledgeBaseId into kbttemp
                            select new
                            {
                                KnowledgeBaseId = kbttemp.Key,
                                KnowledgeBaseTemplateId = kbttemp.First().KnowledgeBaseTemplateId,
                                TemplateId = kbttemp.First().TemplateId,
                                ParentKnowledgeBaseId = kbttemp.First().ParentKnowledgeBaseId,
                                AnswerTypeId = kbttemp.First().AnswerTypeId,
                                Sequence = kbttemp.First().Sequence,
                                MaxPoints = kbttemp.First().MaxPoints,
                                IsInfoDocsEnable = kbttemp.First().IsInfoDocEnable,
                                ReferencePages = kbttemp.First().ReferencePages
                            }).ToList();

                foreach (var k in kbts)
                {
                    KnowledgeBaseTemplate kbt = new KnowledgeBaseTemplate
                    {
                        KnowledgeBaseId = k.KnowledgeBaseId,
                        TemplateId = tempId,
                        ParentKnowledgeBaseId = k.ParentKnowledgeBaseId,
                        AnswerTypeId = k.AnswerTypeId,
                        Sequence = k.Sequence,
                        MaxPoints = k.MaxPoints,
                        IsInfoDocEnable = k.IsInfoDocsEnable,
                        ReferencePages = k.ReferencePages
                    };
                    BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(kbt);
                    BMTDataContext.SubmitChanges();

                    if (names.Count() == 1)
                    {
                        List<ScoringRule> scRules = (from scRulesRec in BMTDataContext.ScoringRules
                                                     where scRulesRec.KnowledgeBaseTemplateId == k.KnowledgeBaseTemplateId
                                                     select scRulesRec).ToList();

                        foreach (ScoringRule sc in scRules)
                        {
                            ScoringRule copySc = new ScoringRule();
                            copySc.KnowledgeBaseTemplateId = kbt.KnowledgeBaseTemplateId;
                            copySc.Score = sc.Score;
                            copySc.MinYesFactor = sc.MinYesFactor;
                            copySc.MaxYesFactor = sc.MaxYesFactor;
                            copySc.MustPassFactorSequence = sc.MustPassFactorSequence;
                            copySc.AbsentFactorSequence = sc.AbsentFactorSequence;
                            copySc.MustPresentFactorSequence = sc.MustPresentFactorSequence;

                            BMTDataContext.ScoringRules.InsertOnSubmit(copySc);
                            BMTDataContext.SubmitChanges();
                        }
                    }

                    List<AnswerTypeWeightage> ansTypeWeightage = (from scRulesRec in BMTDataContext.AnswerTypeWeightages
                                                                  where scRulesRec.KnowledgeBaseTemplateId == k.KnowledgeBaseTemplateId
                                                                  select scRulesRec).ToList();

                    foreach (AnswerTypeWeightage atw in ansTypeWeightage)
                    {
                        AnswerTypeWeightage copyATW = new AnswerTypeWeightage();
                        copyATW.KnowledgeBaseTemplateId = kbt.KnowledgeBaseTemplateId;
                        copyATW.AnswerTypeEnumId = atw.AnswerTypeEnumId;
                        copyATW.Weightage = atw.Weightage;

                        BMTDataContext.AnswerTypeWeightages.InsertOnSubmit(copyATW);
                        BMTDataContext.SubmitChanges();
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsTemplateNameAvailableForEdit()
        {
            try
            {
                var tempRec = (from temp in BMTDataContext.Templates
                               where temp.Name == this.Name &&
                               temp.TemplateId != this.TemplateId
                               select temp).ToList();
                if (tempRec.Count > 0)
                    return false;

                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool UpdateWithMergeTemplate(int templateId, string[] names)
        {
            System.Data.Common.DbTransaction transaction;
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();
            BMTDataContext.Connection.Open();
            transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            try
            {
                var template = (from temp in BMTDataContext.Templates
                                where temp.TemplateId == templateId
                                select temp).First();

                template.Name = this.Name;
                template.ShortName = this.ShortName;
                template.Description = this.Description;
                template.TemplateTypeId = this.TemplateTypeId;
                template.TemplateCategoryId = this.TemplateCategoryId;
                template.AccessLevelId = this.TemplateAccessId;
                template.LastUpdatedBy = this.LastUpdatedBy;
                template.LastUpdatedDate = this.LastUpdatedDate;
                template.SubmitActionId = this.SubmittedActionId;
                template.ToolLevelId = this.ToolLevelId;
                template.DocPath = (this.DocPath == null ? null : this.DocPath);
                if (this.IsActive)
                {
                    template.IsActive = this.IsActive;
                }
                else
                {
                    //List<PracticeTemplate> pracTemp = (from PracTempRec in BMTDataContext.PracticeTemplates
                    //                                   where PracTempRec.TemplateId == template.TemplateId
                    //                                   select PracTempRec).ToList();

                    //if (pracTemp.Count != 0)
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    //    template.IsActive = this.IsActive;
                    //}
                }
                if (this.TemplateAccessId != 1)//if not public template then save practiceId otherwise practiceId= Null
                {
                    template.PracticeId = this.PracticeId;
                    template.EnterpriseId = this.EnterpriseId;
                }
                else
                {
                    template.PracticeId = NULL_STRING;
                    template.EnterpriseId = NULL_STRING;
                }

                BMTDataContext.SubmitChanges();

                List<int> temIdRec = (from tempRec in BMTDataContext.Templates
                                      where names.Contains(tempRec.Name)
                                      select tempRec.TemplateId).ToList();

                var kbt = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                           where temIdRec.Contains(kbtRec.TemplateId) ||
                           kbtRec.TemplateId == templateId
                           group kbtRec by kbtRec.KnowledgeBaseId into kbttemp
                           select new
                           {
                               KnowledgeBaseId = kbttemp.Key,
                               TemplateId = kbttemp.First().TemplateId,
                               AnswerTypeId = kbttemp.First().AnswerTypeId,
                               ParentKnowledgeBaseId = kbttemp.First().ParentKnowledgeBaseId,
                               Sequence = kbttemp.First().Sequence,
                               MaxPoints = kbttemp.First().MaxPoints,
                               IsInfoDocsEnable = kbttemp.First().IsInfoDocEnable,
                               ReferencePages = kbttemp.First().ReferencePages
                           }).ToList();

                List<KnowledgeBaseTemplate> delTemFromKbt = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                                                             where kbtRec.TemplateId == templateId
                                                             select kbtRec).ToList();

                List<int> kbTempIds = (from kbtRec in BMTDataContext.KnowledgeBaseTemplates
                                       where kbtRec.TemplateId == templateId
                                       select kbtRec.KnowledgeBaseTemplateId).ToList();

                List<AnswerTypeWeightage> atw = (from ansTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                                 where kbTempIds.Contains(ansTypeWeightage.KnowledgeBaseTemplateId)
                                                 select ansTypeWeightage).ToList();

                BMTDataContext.AnswerTypeWeightages.DeleteAllOnSubmit(atw);

                BMTDataContext.KnowledgeBaseTemplates.DeleteAllOnSubmit(delTemFromKbt);

                foreach (var k in kbt)
                {
                    KnowledgeBaseTemplate insTemKbt = new KnowledgeBaseTemplate
                    {
                        KnowledgeBaseId = k.KnowledgeBaseId,
                        TemplateId = templateId,
                        ParentKnowledgeBaseId = k.ParentKnowledgeBaseId,
                        AnswerTypeId = k.AnswerTypeId,
                        Sequence = k.Sequence,
                        MaxPoints = k.MaxPoints,
                        IsInfoDocEnable = k.IsInfoDocsEnable,
                        ReferencePages = k.ReferencePages
                    };
                    BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(insTemKbt);
                    BMTDataContext.SubmitChanges();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
        }

        public string GetTemplateNameByTemplateId(int templateId)
        {
            try
            {
                string TempName = (from temp in BMTDataContext.Templates
                                   where temp.TemplateId == templateId
                                   select temp.Name).FirstOrDefault();

                return TempName;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetProject(int practiceId, string userType, int enterpriseId)
        {
            IQueryable getproject = null;
            try
            {
                int medicalGroupId = (from practiceRecord in BMTDataContext.Practices
                                      where practiceRecord.PracticeId == practiceId
                                      select practiceRecord.MedicalGroupId).FirstOrDefault();

                List<int> alreadyProjectIds = (from projectUsageRecord in BMTDataContext.ProjectUsages
                                               where projectUsageRecord.PracticeId == practiceId
                                               select projectUsageRecord.ProjectId).ToList();
                //List<int> alreadyTempIds = null;
                //List<int> alreadyTempIds = (from pracTemp in BMTDataContext.PracticeTemplates
                //                            where pracTemp.PracticeId == practiceId
                //                            select pracTemp.TemplateId).ToList();

                if (userType == enUserRole.Consultant.ToString())
                {
                    getproject = ((from practiceConsult in BMTDataContext.PracticeConsultants
                                   join projectUseRec in BMTDataContext.ProjectUsages
                                   on practiceConsult.PracticeId equals projectUseRec.PracticeId
                                   join projectRecord in BMTDataContext.Projects
                                   on projectUseRec.ProjectId equals projectRecord.ProjectId
                                   where (
                                   practiceConsult.ConsultantId == (from consId in BMTDataContext.PracticeConsultants
                                                                    where consId.PracticeId == practiceId
                                                                    select consId.ConsultantId).First())
                                   select new
                                   {
                                       ProjectId = projectRecord.ProjectId,
                                       Name = projectRecord.Name,
                                       Description = projectRecord.Description,
                                       LastUpdatedDate = projectRecord.LastUpdatedOn,
                                       Status = "Active"
                                   }).Concat
                                (from projectRecord in BMTDataContext.Projects
                                 where
                                 projectRecord.AccessLevelId == (int)enAccessLevelId.Public
                                 && !alreadyProjectIds.Contains(projectRecord.ProjectId)
                                 select new
                                 {
                                     ProjectId = projectRecord.ProjectId,
                                     Name = projectRecord.Name,
                                     Description = projectRecord.Description,
                                     LastUpdatedDate = projectRecord.LastUpdatedOn,
                                     Status = "Active"
                                 })).Distinct() as IQueryable;
                }
                else if (userType == enUserRole.SuperAdmin.ToString())
                {

                    getproject = (from projectRecord in BMTDataContext.Projects
                                  join projectAsgRecord in BMTDataContext.ProjectAssignments
                                  on projectRecord.ProjectId equals projectAsgRecord.ProjectId
                                  where (((projectAsgRecord.PracticeId == practiceId && projectRecord.AccessLevelId == (int)enAccessLevelId.Practice)
                                             || (projectRecord.AccessLevelId == (int)enAccessLevelId.MedicalGroup && projectAsgRecord.MedicalGroupId == medicalGroupId)
                                             || (projectRecord.AccessLevelId == (int)enAccessLevelId.Enterprise && projectAsgRecord.EnterpriseId == enterpriseId))
                                             && !alreadyProjectIds.Contains(projectRecord.ProjectId))
                                  select new
                                  {
                                      ProjectId = projectRecord.ProjectId,
                                      Name = projectRecord.Name,
                                      Description = projectRecord.Description,
                                      LastUpdatedDate = projectRecord.LastUpdatedOn,
                                      Status = "Active"
                                  }).
                                  Concat(from projectRecord in BMTDataContext.Projects
                                         where (projectRecord.AccessLevelId == 1
                                         && !alreadyProjectIds.Contains(projectRecord.ProjectId))
                                         select new
                                         {
                                             ProjectId = projectRecord.ProjectId,
                                             Name = projectRecord.Name,
                                             Description = projectRecord.Description,
                                             LastUpdatedDate = projectRecord.LastUpdatedOn,
                                             Status = "Active"
                                         }) as IQueryable;

                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    getproject = (from projectRecord in BMTDataContext.Projects
                                  where (projectRecord.AccessLevelId == (int)enAccessLevelId.Public
                                  && !alreadyProjectIds.Contains(projectRecord.ProjectId))
                                  select new
                                  {
                                      ProjectId = projectRecord.ProjectId,
                                      Name = projectRecord.Name,
                                      Description = projectRecord.Description,
                                      LastUpdatedDate = projectRecord.LastUpdatedOn,
                                      Status = "Active"

                                  }).Concat((from projectRecord in BMTDataContext.Projects
                                             join projectAsgnRecord in BMTDataContext.ProjectAssignments
                                             on projectRecord.ProjectId equals projectAsgnRecord.ProjectId
                                             where (((projectAsgnRecord.PracticeId == practiceId && projectRecord.AccessLevelId == (int)enAccessLevelId.Practice)
                                             || (projectRecord.AccessLevelId == (int)enAccessLevelId.MedicalGroup && projectAsgnRecord.MedicalGroupId == medicalGroupId)
                                             || (projectRecord.AccessLevelId == (int)enAccessLevelId.Enterprise && projectAsgnRecord.EnterpriseId == enterpriseId))
                                             && !alreadyProjectIds.Contains(projectRecord.ProjectId))

                                             select new
                                             {
                                                 ProjectId = projectRecord.ProjectId,
                                                 Name = projectRecord.Name,
                                                 Description = projectRecord.Description,
                                                 LastUpdatedDate = projectRecord.LastUpdatedOn,
                                                 Status = "Active"
                                             })).Distinct() as IQueryable;
                }
                else if (userType == enUserRole.User.ToString())
                {
                    getproject = (from projectRecord in BMTDataContext.Projects
                                  join projectAsgnRecord in BMTDataContext.ProjectAssignments
                                  on projectRecord.ProjectId equals projectAsgnRecord.ProjectId
                                  where (((projectAsgnRecord.PracticeId == practiceId && projectRecord.AccessLevelId == (int)enAccessLevelId.Practice)
                                  || (projectAsgnRecord.EnterpriseId == enterpriseId && projectRecord.AccessLevelId == (int)enAccessLevelId.Enterprise))
                                  && !alreadyProjectIds.Contains(projectRecord.ProjectId))
                                  select new
                                  {
                                      ProjectId = projectRecord.ProjectId,
                                      Name = projectRecord.Name,
                                      Description = projectRecord.Description,
                                      LastUpdatedDate = projectRecord.LastUpdatedOn,
                                      Status = "Active"
                                  }).
                                  Concat(from projectRecord in BMTDataContext.Projects
                                         where (projectRecord.AccessLevelId == (int)enAccessLevelId.Public
                                         && !alreadyProjectIds.Contains(projectRecord.ProjectId))
                                         select new
                                         {
                                             ProjectId = projectRecord.ProjectId,
                                             Name = projectRecord.Name,
                                             Description = projectRecord.Description,
                                             LastUpdatedDate = projectRecord.LastUpdatedOn,
                                             Status = "Active"
                                         }) as IQueryable;
                }
                return getproject;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SavePracticeTemplate(ArrayList templateIds, int practiceId)
        {
            try
            {
                //List<int> tempIds = new List<int>();

                //List<PracticeTemplate> pracTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                                   where pracTempRec.PracticeId == practiceId
                //                                   select pracTempRec).ToList();

                //for (int currentTempId = 0; currentTempId < templateIds.Count; currentTempId++)
                //{
                //    PracticeTemplate existingTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                                     where pracTempRec.PracticeId == practiceId &&
                //                                     pracTempRec.TemplateId == (int)templateIds[currentTempId]
                //                                     select pracTempRec).FirstOrDefault();
                //    if (existingTemp == null)
                //    {
                //        PracticeTemplate newPracTemp = new PracticeTemplate();

                //        newPracTemp.PracticeId = practiceId;
                //        newPracTemp.TemplateId = (int)templateIds[currentTempId];
                //        newPracTemp.Sequence = currentTempId + ADD_SUB_TO_MAX;
                //        newPracTemp.Visible = true;

                //        BMTDataContext.PracticeTemplates.InsertOnSubmit(newPracTemp);
                //        BMTDataContext.SubmitChanges();
                //    }
                //    else
                //    {
                //        existingTemp.Sequence = currentTempId + ADD_SUB_TO_MAX;
                //        existingTemp.Visible = true;
                //        BMTDataContext.SubmitChanges();
                //    }
                //    tempIds.Add((int)templateIds[currentTempId]);
                //}

                //List<PracticeTemplate> NonVisiblepracTemp = (from pracTempExisting in BMTDataContext.PracticeTemplates
                //                                             where pracTempExisting.PracticeId == practiceId &&
                //                                             !tempIds.Contains(pracTempExisting.TemplateId)
                //                                             select pracTempExisting).ToList();

                //foreach (PracticeTemplate InvisiblepracTemp in NonVisiblepracTemp)
                //{
                //    InvisiblepracTemp.Visible = false;
                //    InvisiblepracTemp.Sequence = -ADD_SUB_TO_MAX;
                //    BMTDataContext.SubmitChanges();
                //}
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsTemplateExists(int tempId, int practiceId)
        {
            try
            {
                //PracticeTemplate pracTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                             where pracTempRec.PracticeId == practiceId &&
                //                             pracTempRec.TemplateId == tempId
                //                             select pracTempRec).FirstOrDefault();

                //if (pracTemp != null)
                //    return true;

                //else
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable GetSelectedProject(int practiceId)
        {
            IQueryable getproject = null;
            try
            {
                BMTDataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, BMTDataContext.Projects);

                getproject = (from practiceRecrod in BMTDataContext.Practices
                              join projectUseRecrod in BMTDataContext.ProjectUsages
                              on practiceRecrod.PracticeId equals projectUseRecrod.PracticeId
                              join projectRecord in BMTDataContext.Projects
                              on projectUseRecrod.ProjectId equals projectRecord.ProjectId
                              where practiceRecrod.PracticeId == practiceId
                              orderby (projectUseRecrod.SequenceNo > 0 ? projectUseRecrod.SequenceNo : 1000)
                              select new
                              {
                                  Name = projectRecord.Name,
                                  LastUpdatedDate = projectRecord.LastUpdatedOn,
                                  ProjectId = projectRecord.ProjectId,
                              }) as IQueryable;

                //getproject = (from PracTemp in BMTDataContext.PracticeTemplates
                //              join temp in BMTDataContext.Templates
                //              on PracTemp.TemplateId equals temp.TemplateId
                //              where PracTemp.PracticeId == practiceId
                //              orderby (PracTemp.Sequence > 0 ? PracTemp.Sequence : 1000)
                //              select new
                //              {
                //                  Name = temp.Name,
                //                  LastUpdatedDate = temp.LastUpdatedDate.ToShortDateString(),
                //                  TemplateId = PracTemp.TemplateId,
                //              }) as IQueryable;

                return getproject;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool SavePracticeTemplate(List<int> projectIds, int practiceId)
        {
            try
            {

                List<int> projIds = new List<int>();
                int sequence = 0;

                //for (int currentTempId = 0; currentTempId < templateIds.Count; currentTempId++)
                foreach (int projectId in projectIds)
                {

                    ProjectUsage existingProjects = (from projectUsageRecord in BMTDataContext.ProjectUsages
                                                     where projectUsageRecord.PracticeId == practiceId &&
                                                     projectUsageRecord.ProjectId == projectId
                                                     select projectUsageRecord).FirstOrDefault();
                    if (existingProjects == null)
                    {
                        ProjectUsage newProject = new ProjectUsage();

                        newProject.PracticeId = practiceId;
                        newProject.ProjectId = projectId;
                        newProject.SequenceNo = sequence + ADD_SUB_TO_MAX;
                        newProject.IsVisible = true;

                        BMTDataContext.ProjectUsages.InsertOnSubmit(newProject);
                        BMTDataContext.SubmitChanges();
                    }
                    else
                    {
                        existingProjects.SequenceNo = sequence + ADD_SUB_TO_MAX;
                        existingProjects.IsVisible = true;
                        BMTDataContext.SubmitChanges();
                    }
                    projIds.Add(projectId);
                    sequence = sequence + 1;
                }

                var NonVisibleProjects = (from projectUsageRecord in BMTDataContext.ProjectUsages
                                          where projectUsageRecord.PracticeId == practiceId &&
                                          !projIds.Contains(projectUsageRecord.ProjectId)
                                          select projectUsageRecord).ToList();

                foreach (ProjectUsage InvisibleProjects in NonVisibleProjects)
                {
                    InvisibleProjects.IsVisible = false;
                    InvisibleProjects.SequenceNo = -ADD_SUB_TO_MAX;
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsPracticeHasCorporateTemplate(int practiceId)
        {
            try
            {
                List<int> TemplateIds = (from pracTemp in BMTDataContext.Templates
                                         where pracTemp.PracticeId == practiceId
                                         select pracTemp.TemplateId).ToList();

                var Temp = (from temp in BMTDataContext.Templates
                            where TemplateIds.Contains(temp.TemplateId)
                            && temp.IsCorporate == true
                            select temp).ToList();

                if (Temp.Count != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsUsedTemplate(int templateId)
        {
            try
            {
                var pracTemp = (from pracTempRec in BMTDataContext.ProjectSections
                                where pracTempRec.TemplateId == templateId
                                select pracTempRec).ToList();
                if (pracTemp.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //public PracticeTemplate GetTemplateCorporateSubmission(int practiceId, int templateId)
        //{
        //    try
        //    {
        //        PracticeTemplate pracTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
        //                                     where pracTempRec.TemplateId == templateId &&
        //                                     pracTempRec.PracticeId == practiceId
        //                                     select pracTempRec).FirstOrDefault();

        //        return pracTemp;
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //}

        public bool IsAvailableForCorporate(int practiceId, int templateId)
        {
            try
            {
                List<int> pracSiteIds = (from pracSiteRec in BMTDataContext.PracticeSites
                                         where pracSiteRec.PracticeId == practiceId
                                         select pracSiteRec.PracticeSiteId).ToList();

                if (pracSiteIds.Count > 2)
                {
                    Template temp = (from tempRec in BMTDataContext.Templates
                                     where tempRec.TemplateId == templateId
                                     select tempRec).FirstOrDefault();

                    if (temp.IsCorporate != null)
                    {
                        if ((bool)temp.IsCorporate)
                            return true;
                        else
                            return false;
                    }
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

        public bool AlreadyCorporate(int practiceId, int tempalteId)
        {
            try
            {
                //BMTDataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, BMTDataContext.PracticeTemplates);

                //PracticeTemplate pracTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                             where pracTempRec.PracticeId == practiceId &&
                //                             pracTempRec.TemplateId == tempalteId
                //                             select pracTempRec
                //                       ).FirstOrDefault();
                //if (pracTemp.IsCorporate != null)
                //{
                //    if ((bool)pracTemp.IsCorporate)
                //        return true;
                //    else
                //        return false;
                //}
                //else
                return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsVisible(int practiceId, int porjectId)
        {
            try
            {
                ProjectUsage pracProject = (from projectUseRecrod in BMTDataContext.ProjectUsages
                                            where projectUseRecrod.PracticeId == practiceId &&
                                             projectUseRecrod.ProjectId == porjectId
                                            select projectUseRecrod).FirstOrDefault();
                if (pracProject != null)
                {
                    if (pracProject.IsVisible != null)
                    {
                        if ((bool)pracProject.IsVisible)
                            return true;
                        else
                            return false;
                    }
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

        public string GetFilePath(int tempId)
        {
            try
            {
                string filePath = (from temp in BMTDataContext.Templates
                                   where temp.TemplateId == tempId
                                   select temp.DocPath).FirstOrDefault();
                return filePath;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool AddPracticeTemplate(List<int> projectIds, int practiceId)
        {
            try
            {
                foreach (int projectId in projectIds)
                {
                    ProjectUsage existingProj = (from projectUseRecord in BMTDataContext.ProjectUsages
                                                 where projectUseRecord.ProjectId == projectId
                                                 && projectUseRecord.PracticeId == practiceId
                                                 select projectUseRecord).FirstOrDefault();

                    if (existingProj == null)
                    {
                        ProjectUsage newProject = new ProjectUsage();

                        newProject.PracticeId = practiceId;
                        newProject.ProjectId = projectId;
                        newProject.IsVisible = true;

                        var maxProjectSequence = (from projectUseRecord in BMTDataContext.ProjectUsages
                                                  where projectUseRecord.PracticeId == practiceId
                                                  select projectUseRecord).ToList();
                        if (maxProjectSequence.Count != 0)
                        {
                            newProject.SequenceNo = maxProjectSequence.Count + ADD_SUB_TO_MAX;
                        }
                        else
                        {
                            newProject.SequenceNo = ADD_SUB_TO_MAX;
                        }

                        BMTDataContext.ProjectUsages.InsertOnSubmit(newProject);
                        BMTDataContext.SubmitChanges();
                    }
                }
                //for (int tempId = 0; tempId < templateIds.Count; tempId++)
                //{
                //    PracticeTemplate existingProj = (from PracTempRec in BMTDataContext.PracticeTemplates
                //                                     where PracTempRec.TemplateId == (int)templateIds[tempId] &&
                //                                     PracTempRec.PracticeId == practiceId
                //                                     select PracTempRec).FirstOrDefault();

                //    if (existingProj == null)
                //    {
                //        PracticeTemplate newPracTemp = new PracticeTemplate();

                //        newPracTemp.PracticeId = practiceId;
                //        newPracTemp.TemplateId = (int)templateIds[tempId];
                //        newPracTemp.Visible = true;

                //        var maxPracTempSequence = (from existingPracTempRec in BMTDataContext.PracticeTemplates
                //                                   where existingPracTempRec.PracticeId == practiceId
                //                                   select existingPracTempRec).ToList();

                //        if (maxPracTempSequence.Count != 0)
                //            newPracTemp.Sequence = ((from maxSeqience in maxPracTempSequence
                //                                     select maxSeqience.Sequence).Max()) + ADD_SUB_TO_MAX;
                //        else
                //            newPracTemp.Sequence = ADD_SUB_TO_MAX;
                //        BMTDataContext.PracticeTemplates.InsertOnSubmit(newPracTemp);
                //        BMTDataContext.SubmitChanges();
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable GetToolLevel()
        {
            IQueryable toolLevel = null;
            try
            {
                toolLevel = (from toolLevelRec in BMTDataContext.ToolLevels
                             select toolLevelRec).AsQueryable();

                return toolLevel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }

}
