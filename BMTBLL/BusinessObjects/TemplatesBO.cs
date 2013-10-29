using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

using System.Configuration;
using System.Data;
using BMTBLL.Classes;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class TemplatesBO : BMTConnection
    {
        #region Funtions

        private const string TEMPLATE_UTILITY_FOLDER = "Utility Folder";
        private const string TEMPLATE_UPLOADED_DOCUMENT_FOLDER = "Uploaded Documents";
        private const string TEMPLATE_UPLOADED_DOCUMENT_CONTENT_TYPE = "UploadedDocuments";

        public bool ImportFromExcel(string file, int templateId, int userId)
        {
            try
            {
                DataSet ds = new DataSet();

                // -- Start of Constructing OLEDB connection string to Excel file
                Dictionary<string, string> props = new Dictionary<string, string>();

                // For Excel 2007/2010
                if (file.EndsWith(".xlsx"))
                {
                    props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                    props["Extended Properties"] = "Excel 12.0";
                }
                // For Excel 2003 and older
                else if (file.EndsWith(".xls"))
                {
                    props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                    props["Extended Properties"] = "Excel 8.0";
                }
                else
                    return false;

                props["Data Source"] = file;

                StringBuilder sb = new StringBuilder();

                foreach (KeyValuePair<string, string> prop in props)
                {
                    sb.Append(prop.Key);
                    sb.Append('=');
                    sb.Append(prop.Value);
                    sb.Append(';');
                }

                string connectionString = sb.ToString();
                // -- End of Constructing OLEDB connection string to Excel file

                // Connecting to Excel File
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;

                    // Get all Sheets in Excel File
                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    // Loop through all Sheets to get data
                    foreach (DataRow dr in dtSheet.Rows)
                    {
                        string sheetName = dr["TABLE_NAME"].ToString();

                        // Get all rows from the Sheet
                        cmd.CommandText = "SELECT KnowledgeBaseId,KnowledgeBaseTypeId,Name,TabName,DisplayName,InstructionText,AccessId,MustPass,ParentKnowledgeBaseId,Sequence,AnswerTypeId FROM [" + sheetName + "]";

                        DataTable dt = new DataTable();
                        dt.TableName = sheetName.Replace("$", string.Empty);

                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(dt);

                        ds.Tables.Add(dt);

                        int knowledgeBaseTypeId, accessId, mustPass, createdBy, Sequence, AnswerTypeId;
                        string name, tabName, displayName, instructionText, parentKnowledgeBaseId;

                        int parentKnowledgeBaseTypeId = 0;
                        int parentAccessId = 0;
                        string parentName = string.Empty;

                        for (int kbRow = 0; kbRow < dt.Rows.Count; kbRow++)
                        {
                            knowledgeBaseTypeId = Convert.ToInt32(dt.Rows[kbRow].ItemArray[1].ToString());
                            name = dt.Rows[kbRow].ItemArray[2].ToString();
                            tabName = dt.Rows[kbRow].ItemArray[3].ToString();
                            displayName = dt.Rows[kbRow].ItemArray[4].ToString();
                            instructionText = dt.Rows[kbRow].ItemArray[5].ToString();
                            accessId = Convert.ToInt32(dt.Rows[kbRow].ItemArray[6].ToString());
                            if (dt.Rows[kbRow].ItemArray[7].ToString() != string.Empty)
                                mustPass = Convert.ToInt32(dt.Rows[kbRow].ItemArray[7].ToString());
                            else
                                mustPass = 0;

                            createdBy = userId;
                            parentKnowledgeBaseId = dt.Rows[kbRow].ItemArray[8].ToString();
                            Sequence = Convert.ToInt32(dt.Rows[kbRow].ItemArray[9].ToString());

                            if (dt.Rows[kbRow].ItemArray[10].ToString() != string.Empty)
                                AnswerTypeId = Convert.ToInt32(dt.Rows[kbRow].ItemArray[10].ToString());
                            else
                                AnswerTypeId = 0;

                            if (parentKnowledgeBaseId != string.Empty)
                            {
                                for (int Row = 0; Row < dt.Rows.Count; Row++)
                                {
                                    if (Convert.ToInt32(parentKnowledgeBaseId) == Convert.ToInt32(dt.Rows[Row].ItemArray[0].ToString()))
                                    {
                                        parentKnowledgeBaseTypeId = Convert.ToInt32(dt.Rows[Row].ItemArray[1].ToString());
                                        parentAccessId = Convert.ToInt32(dt.Rows[Row].ItemArray[6].ToString());
                                        parentName = dt.Rows[Row].ItemArray[2].ToString();
                                        break;
                                    }
                                }

                                parentKnowledgeBaseId = Convert.ToString((from knowledgeBase in BMTDataContext.KnowledgeBases
                                                                          orderby knowledgeBase.KnowledgeBaseId descending
                                                                          where knowledgeBase.KnowledgeBaseTypeId == parentKnowledgeBaseTypeId
                                                                          && knowledgeBase.AccessId == parentAccessId
                                                                          && knowledgeBase.Name == parentName
                                                                          select knowledgeBase.KnowledgeBaseId).FirstOrDefault());

                            }

                            InsertKnowledgeBase(knowledgeBaseTypeId, name, tabName, displayName, instructionText, accessId, mustPass, createdBy, parentKnowledgeBaseId, Sequence, AnswerTypeId, templateId);
                        }
                    }

                    cmd = null;
                    conn.Close();
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        // Save the Excel Sheet data in SQL Row by Row.
        public bool InsertKnowledgeBase(int knowledgeBaseTypeId, string name, string tabName, string displayName, string instructionText,
                                        int accessId, int mustPass, int createdBy, string parentKnowledgeBaseId, int sequence, int answerTypeId, int templateId)
        {
            KnowledgeBase knowledgeBase = new KnowledgeBase();
            knowledgeBase.KnowledgeBaseTypeId = knowledgeBaseTypeId;
            knowledgeBase.Name = name;
            if (tabName != string.Empty)
                knowledgeBase.TabName = tabName;

            if (displayName != string.Empty)
                knowledgeBase.DisplayName = displayName;

            if (instructionText != string.Empty)
                knowledgeBase.InstructionText = instructionText;

            knowledgeBase.AccessId = accessId;
            if (mustPass == 1)
                knowledgeBase.MustPass = true;

            knowledgeBase.CreatedBy = createdBy;
            knowledgeBase.IsActive = true;


            BMTDataContext.KnowledgeBases.InsertOnSubmit(knowledgeBase);
            BMTDataContext.SubmitChanges();

            KnowledgeBaseTemplate knowledgeBaseTemplate = new KnowledgeBaseTemplate();
            knowledgeBaseTemplate.KnowledgeBaseId = knowledgeBase.KnowledgeBaseId;
            knowledgeBaseTemplate.TemplateId = templateId;
            if (parentKnowledgeBaseId != string.Empty)
                knowledgeBaseTemplate.ParentKnowledgeBaseId = Convert.ToInt32(parentKnowledgeBaseId);
            knowledgeBaseTemplate.Sequence = sequence;
            if (answerTypeId != 0)
                knowledgeBaseTemplate.AnswerTypeId = answerTypeId;
            BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(knowledgeBaseTemplate);
            BMTDataContext.SubmitChanges();

            return true;
        }

        public List<KnowledgeBase> GetAllHeaders(int templateId)
        {
            try
            {
                var List = (from knowledgeBase in BMTDataContext.KnowledgeBases
                            join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                            on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                            where knowledgeBase.KnowledgeBaseTypeId == 1 && knowledgeBaseTemplate.TemplateId == templateId
                            select knowledgeBase).ToList();

                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetHeadersTemplate(int templateId)
        {
            try
            {
                IQueryable List = (from KB in BMTDataContext.KnowledgeBases
                                   join KBT in BMTDataContext.KnowledgeBaseTemplates on KB.KnowledgeBaseId equals KBT.KnowledgeBaseId
                                   join T in BMTDataContext.Templates on KBT.TemplateId equals T.TemplateId
                                   where T.TemplateId == templateId
                                   && KB.KnowledgeBaseTypeId == 1
                                   select new
                                   {
                                       KB.KnowledgeBaseId,
                                       KB.Name
                                   }
                        ) as IQueryable;
                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetsubHeaderTemplate(int templateId)
        {
            try
            {
                IQueryable List = (from KB in BMTDataContext.KnowledgeBases
                                   join KBT in BMTDataContext.KnowledgeBaseTemplates on KB.KnowledgeBaseId equals KBT.KnowledgeBaseId
                                   join T in BMTDataContext.Templates on KBT.TemplateId equals T.TemplateId
                                   where T.TemplateId == templateId
                                  && KB.KnowledgeBaseTypeId == 2
                                   select new
                                   {
                                       KB.KnowledgeBaseId,
                                       KB.Name
                                   }
                        ) as IQueryable;
                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetQuestionTemplate(int templateId)
        {
            try
            {
                IQueryable List = (from KB in BMTDataContext.KnowledgeBases
                                   join KBT in BMTDataContext.KnowledgeBaseTemplates on KB.KnowledgeBaseId equals KBT.KnowledgeBaseId
                                   join T in BMTDataContext.Templates on KBT.TemplateId equals T.TemplateId
                                   where T.TemplateId == templateId
                                   && KB.KnowledgeBaseTypeId == 3
                                   select new
                                   {
                                       KB.KnowledgeBaseId,
                                       KB.Name
                                   }
                        ) as IQueryable;
                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<KnowledgeBase> GetAllSubHeaders(int headerId, int templateId)
        {
            try
            {
                var knowledgeBaseParent = from knowledgeBaseParentId in BMTDataContext.KnowledgeBases
                                          select knowledgeBaseParentId.KnowledgeBaseId;

                var List = (from knowledgeBase in BMTDataContext.KnowledgeBases
                            join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                            on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                            where knowledgeBase.KnowledgeBaseTypeId == 2 && knowledgeBaseTemplate.TemplateId == templateId
                            && knowledgeBaseParent.Contains(knowledgeBase.KnowledgeBaseId)
                            && knowledgeBaseTemplate.ParentKnowledgeBaseId == headerId
                            select knowledgeBase).ToList();
                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<KnowledgeBase> GetAllQuestions(int subHeaderId, int templateId)
        {
            try
            {
                var knowledgeBaseParent = from knowledgeBaseParentId in BMTDataContext.KnowledgeBases
                                          select knowledgeBaseParentId.KnowledgeBaseId;

                var List = (from knowledgeBase in BMTDataContext.KnowledgeBases
                            join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                            on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                            where knowledgeBase.KnowledgeBaseTypeId == 3 && knowledgeBaseTemplate.TemplateId == templateId
                            && knowledgeBaseParent.Contains(knowledgeBase.KnowledgeBaseId)
                            && knowledgeBaseTemplate.ParentKnowledgeBaseId == subHeaderId
                            select knowledgeBase).ToList();
                return List;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetTemplate(string userType, int enterpriseId)
        {
            IQueryable getTemplate = null;
            try
            {
                if (userType == enUserRole.SuperUser.ToString())
                {
                    getTemplate = (from temp in BMTDataContext.Templates
                                   join tempcat in BMTDataContext.TemplateCategories
                                   on temp.TemplateCategoryId equals tempcat.TemplateCategoryId
                                   join tempacc in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempacc.AccessLevelId
                                   where //temp.PracticeId == practiceId ||
                                   temp.EnterpriseId == enterpriseId
                                   select new
                                   {
                                       TemplateId = temp.TemplateId,
                                       Name = temp.Name
                                   }) as IQueryable;
                }
                else if (userType == enUserRole.SuperAdmin.ToString())
                {

                    getTemplate = (from temp in BMTDataContext.Templates
                                   join tempcat in BMTDataContext.TemplateCategories
                                   on temp.TemplateCategoryId equals tempcat.TemplateCategoryId
                                   join tempacc in BMTDataContext.AccessLevels
                                   on temp.AccessLevelId equals tempacc.AccessLevelId
                                   where //temp.PracticeId == practiceId ||
                                   temp.AccessLevelId == (int)enTemplateAccessType.Public ||//for public
                                   (temp.AccessLevelId == (int)enTemplateAccessType.Enterprise &&// for Enterprise
                                   temp.EnterpriseId == enterpriseId)
                                   select new
                                   {
                                       TemplateId = temp.TemplateId,
                                       Name = temp.Name
                                   }) as IQueryable;
                }
                return getTemplate;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveTemplate(string templateName, string shortName, string description, int UserId, int enterpriseId, int templateAccessId)
        {
            try
            {

                Template template = new Template();
                template.Name = templateName.Trim();
                template.ShortName = shortName.Trim();
                template.Description = description;
                template.TemplateTypeId = 1;
                template.TemplateCategoryId = 1;
                template.AccessLevelId = templateAccessId;
                template.CreatedBy = UserId;
                template.CreatedDate = System.DateTime.Now;
                template.LastUpdatedBy = UserId;
                template.LastUpdatedDate = System.DateTime.Now;

                if (templateAccessId != 1)
                    template.EnterpriseId = enterpriseId;

                template.SubmitActionId = 1;
                template.IsActive = true;

                BMTDataContext.Templates.InsertOnSubmit(template);
                BMTDataContext.SubmitChanges();

                //ProjectSection pSection = new ProjectSection();

                //pSection.ParentProjectSectionId = 0;
                //pSection.Name = TEMPLATE_UTILITY_FOLDER + " - " + template.TemplateId;

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
            catch
            {

                return false;
            }
        }

        public bool AddHeader(string headerName, int templateId)
        {
            try
            {
                var record = (from header in BMTDataContext.KnowledgeBases
                              select new { header }).FirstOrDefault();


                KnowledgeBase knowledgeBase = new KnowledgeBase();
                knowledgeBase.KnowledgeBaseTypeId = 1;
                knowledgeBase.Name = headerName.Trim();
                knowledgeBase.TabName = "More";
                knowledgeBase.AccessId = 1;
                knowledgeBase.MustPass = true;
                knowledgeBase.CreatedBy = 1;
                knowledgeBase.IsActive = true;

                BMTDataContext.KnowledgeBases.InsertOnSubmit(knowledgeBase);
                BMTDataContext.SubmitChanges();

                KnowledgeBaseTemplate knowledgeBaseTemplate = new KnowledgeBaseTemplate();
                knowledgeBaseTemplate.KnowledgeBaseId = knowledgeBase.KnowledgeBaseId;
                knowledgeBaseTemplate.TemplateId = templateId;
                BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(knowledgeBaseTemplate);
                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsHeaderExist(string headerName, int templateId, int headerId)
        {
            try
            {
                var header = (from knowledgeBase in BMTDataContext.KnowledgeBases
                              join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                              where knowledgeBase.Name == headerName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                              && knowledgeBaseTemplate.ParentKnowledgeBaseId == null
                              && knowledgeBase.KnowledgeBaseId != headerId
                              select knowledgeBase.Name).FirstOrDefault();

                if (header == null)
                    return true;
                else
                    return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsHeaderAllRedayExist(string headerName, int templateId)
        {
            try
            {
                var header = (from knowledgeBase in BMTDataContext.KnowledgeBases
                              join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                              where knowledgeBase.Name == headerName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                              && knowledgeBaseTemplate.ParentKnowledgeBaseId == null
                              select knowledgeBase.Name).FirstOrDefault();

                KnowledgeBase sameNameHeader = (from kbase in BMTDataContext.KnowledgeBases
                                                where kbase.KnowledgeBaseTypeId == (int)enKBType.Header
                                                && kbase.Name == headerName
                                                select kbase).FirstOrDefault();
                if (sameNameHeader != null)
                    return false;

                if (header == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsSubHeaderExist(string subHeaderName, int templateId, int parentId, int subHeaderId)
        {
            try
            {
                var subHeader = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                 join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                                 where knowledgeBase.Name == subHeaderName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                                && knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId
                                && knowledgeBase.KnowledgeBaseId != subHeaderId
                                 select knowledgeBase.Name).FirstOrDefault();

                if (subHeader == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsSubHeaderAllReadyExist(string subHeaderName, int templateId, int parentId)
        {
            try
            {
                var subHeader = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                 join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                                 where knowledgeBase.Name == subHeaderName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                                 && knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId
                                 select knowledgeBase.Name).FirstOrDefault();

                KnowledgeBase sameNameHeader = (from kbase in BMTDataContext.KnowledgeBases
                                                where kbase.KnowledgeBaseTypeId == (int)enKBType.SubHeader
                                                && kbase.Name == subHeaderName
                                                select kbase).FirstOrDefault();
                if (sameNameHeader != null)
                    return false;

                if (subHeader == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsQuestionExist(string qustionName, int templateId, int parentId, int questionId)
        {
            try
            {
                var question = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                                where knowledgeBase.Name == qustionName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                                 && knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId
                                 && knowledgeBase.KnowledgeBaseId != questionId
                                select knowledgeBase.Name).FirstOrDefault();

                if (question == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsAllReadyQuestionExist(string qustionName, int templateId, int parentId)
        {
            try
            {
                var question = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates on knowledgeBase.KnowledgeBaseId equals knowledgeBaseTemplate.KnowledgeBaseId
                                where knowledgeBase.Name == qustionName.Trim() && knowledgeBaseTemplate.TemplateId == templateId
                                && knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId
                                select knowledgeBase.Name).FirstOrDefault();

                KnowledgeBase sameNameHeader = (from kbase in BMTDataContext.KnowledgeBases
                                                where kbase.KnowledgeBaseTypeId == (int)enKBType.Question
                                                && kbase.Name == qustionName
                                                select kbase).FirstOrDefault();
                if (sameNameHeader != null)
                    return false;

                if (question == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsTemplateNameAvailable(string Name, int enterpriseId)
        {

            try
            {
                var template = (from templates in BMTDataContext.Templates
                                where templates.Name == Name.Trim()
                                && (templates.EnterpriseId == enterpriseId || templates.EnterpriseId == null)
                                select templates.Name).FirstOrDefault();

                if (template == null)
                    return true;
                else return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool UpdateHeaders(int headerId, string headerName)
        {
            try
            {

                var record = (from header in BMTDataContext.KnowledgeBases
                              where header.KnowledgeBaseId == headerId
                              select header).FirstOrDefault();


                if (record != null)
                {
                    record.Name = headerName.Trim();
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DeleteHeader(int headerId, int TemplateId)
        {
            try
            {
                var subHeaderList = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                     where knowledgeBaseTemplate.ParentKnowledgeBaseId == headerId
                                     select knowledgeBaseTemplate).ToList();

                //DELETING ASSOCIATED FOREIGN KEYS RECORDS.
                foreach (var subHeader in subHeaderList)
                {
                    var scoringRuleList = (from scoringRule in BMTDataContext.ScoringRules
                                           join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                               on scoringRule.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                           join knowledgeBase in BMTDataContext.KnowledgeBases
                                               on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                           where knowledgeBaseTemplate.KnowledgeBaseTemplateId == subHeader.KnowledgeBaseTemplateId
                                           select scoringRule).ToList();

                    foreach (var kBScore in scoringRuleList)
                    {
                        var score = (from scoringRule in BMTDataContext.ScoringRules
                                     where scoringRule.KnowledgeBaseTemplateId == kBScore.KnowledgeBaseTemplateId
                                     select scoringRule).FirstOrDefault();

                        BMTDataContext.ScoringRules.DeleteOnSubmit(score);
                        BMTDataContext.SubmitChanges();
                    }

                    var answerTypeWeightageList = (from anserTypeEnumWeightage in BMTDataContext.AnswerTypeWeightages
                                                   join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                       on anserTypeEnumWeightage.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                                   join knowledgeBase in BMTDataContext.KnowledgeBases
                                                       on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                                   where knowledgeBaseTemplate.KnowledgeBaseTemplateId == subHeader.KnowledgeBaseTemplateId
                                                   select anserTypeEnumWeightage).ToList();

                    foreach (var answerWeightage in answerTypeWeightageList)
                    {
                        var weightage = (from answerTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                         where answerTypeWeightage.KnowledgeBaseTemplateId == answerWeightage.KnowledgeBaseTemplateId
                                         select answerTypeWeightage).FirstOrDefault();

                        BMTDataContext.AnswerTypeWeightages.DeleteOnSubmit(weightage);
                        BMTDataContext.SubmitChanges();
                    }


                    //var filledAnswerId = (from filledAnswer in BMTDataContext.FilledAnswers
                    //                      join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                    //                          on filledAnswer.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                    //                      join knowledgeBase in BMTDataContext.KnowledgeBases
                    //                          on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                    //                      where knowledgeBaseTemplate.KnowledgeBaseTemplateId == subHeader.KnowledgeBaseTemplateId
                    //                      select filledAnswer).ToList();

                    //foreach (var filledAsnwer in filledAnswerId)
                    //{
                    //    BMTDataContext.FilledAnswers.DeleteOnSubmit(filledAsnwer);
                    //    BMTDataContext.SubmitChanges();
                    //}


                    // DELETING SUB-HEADER RECORDS 
                    var parentKnowledgeBaseIds = (from knowledgeBaseParent in BMTDataContext.KnowledgeBaseTemplates
                                                  where knowledgeBaseParent.ParentKnowledgeBaseId == subHeader.ParentKnowledgeBaseId
                                                  select knowledgeBaseParent).ToList();

                    foreach (var parentId in parentKnowledgeBaseIds)
                    {
                        var kbtChildRecord = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                              where knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId.KnowledgeBaseId
                                              select knowledgeBaseTemplate).ToList();

                        foreach (var knowledgeBaseSubHeaderId in kbtChildRecord)
                        {
                            var kbtQuestionRecord = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                     where knowledgeBaseTemplate.ParentKnowledgeBaseId == knowledgeBaseSubHeaderId.ParentKnowledgeBaseId
                                                     select knowledgeBaseTemplate).FirstOrDefault();

                            var kbQuestionRecord = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                                    where knowledgeBase.KnowledgeBaseId == knowledgeBaseSubHeaderId.KnowledgeBaseId
                                                    select knowledgeBase).FirstOrDefault();

                            if (kbtQuestionRecord != null)
                            {
                                BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbtQuestionRecord);
                                BMTDataContext.SubmitChanges();
                            }
                            if (kbQuestionRecord != null)
                            {
                                BMTDataContext.KnowledgeBases.DeleteOnSubmit(kbQuestionRecord);
                                BMTDataContext.SubmitChanges();
                            }
                        }
                        BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(parentId);
                        BMTDataContext.SubmitChanges();


                        var kbSubHeader = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                           where knowledgeBase.KnowledgeBaseId == parentId.KnowledgeBaseId
                                           select knowledgeBase).FirstOrDefault();

                        BMTDataContext.KnowledgeBases.DeleteOnSubmit(kbSubHeader);
                        BMTDataContext.SubmitChanges();
                    }
                }

                // DELETING HEADER RECORD FROM KNOWLEDGEBASETEMPLATE AND KNOWLEDGEBASE
                var knRecordHeaderTemplate = (from knowledgeBase in BMTDataContext.KnowledgeBaseTemplates
                                              where knowledgeBase.KnowledgeBaseId == headerId
                                              select knowledgeBase).SingleOrDefault();

                if (knRecordHeaderTemplate != null)
                {
                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(knRecordHeaderTemplate);
                    BMTDataContext.SubmitChanges();
                }

                var knRecordHeader = (from knowledgeBase in BMTDataContext.KnowledgeBases
                                      where knowledgeBase.KnowledgeBaseId == headerId
                                      select knowledgeBase).SingleOrDefault();


                if (knRecordHeader != null)
                {
                    BMTDataContext.KnowledgeBases.DeleteOnSubmit(knRecordHeader);
                    BMTDataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool AddSubHeader(int headerId, string subHeaderName, int templateId)
        {
            try
            {
                var record = (from subHeader in BMTDataContext.KnowledgeBases
                              select new { subHeader }).FirstOrDefault();

                KnowledgeBase knowledgeBase = new KnowledgeBase();
                knowledgeBase.KnowledgeBaseTypeId = 2;
                knowledgeBase.Name = subHeaderName.Trim();
                knowledgeBase.AccessId = 1;
                knowledgeBase.MustPass = true;
                knowledgeBase.CreatedBy = 1;
                knowledgeBase.IsActive = true;

                BMTDataContext.KnowledgeBases.InsertOnSubmit(knowledgeBase);
                BMTDataContext.SubmitChanges();

                KnowledgeBaseTemplate knowledgeBaseTemplate = new KnowledgeBaseTemplate();
                knowledgeBaseTemplate.KnowledgeBaseId = knowledgeBase.KnowledgeBaseId;
                knowledgeBaseTemplate.TemplateId = templateId;
                knowledgeBaseTemplate.ParentKnowledgeBaseId = headerId;
                BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(knowledgeBaseTemplate);
                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool UpdateSubHeaders(int subHeaderId, string subHeaderName)
        {
            try
            {

                var record = (from subHeader in BMTDataContext.KnowledgeBases
                              where subHeader.KnowledgeBaseId == subHeaderId
                              select subHeader).FirstOrDefault();


                if (record != null)
                {
                    record.Name = subHeaderName.Trim();
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DeleteSubHeader(int subHeaderId)
        {
            try
            {
                var scoringRuleList = (from scoringRule in BMTDataContext.ScoringRules
                                       join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                           on scoringRule.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                       join knowledgeBase in BMTDataContext.KnowledgeBases
                                           on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                       where knowledgeBase.KnowledgeBaseId == subHeaderId
                                       select scoringRule).ToList();

                foreach (var kBScore in scoringRuleList)
                {
                    var score = (from scoringRule in BMTDataContext.ScoringRules
                                 where scoringRule.KnowledgeBaseTemplateId == kBScore.KnowledgeBaseTemplateId
                                 select scoringRule).FirstOrDefault();
                    if (score != null)
                    {
                        BMTDataContext.ScoringRules.DeleteOnSubmit(score);
                        BMTDataContext.SubmitChanges();
                    }
                }

                var answerTypeWeightageList = (from anserTypeEnumWeightage in BMTDataContext.AnswerTypeWeightages
                                               join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                   on anserTypeEnumWeightage.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                               join knowledgeBase in BMTDataContext.KnowledgeBases
                                                   on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                               where knowledgeBase.KnowledgeBaseId == subHeaderId
                                               select anserTypeEnumWeightage).ToList();

                foreach (var answerWeightage in answerTypeWeightageList)
                {
                    var weightage = (from answerTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                     where answerTypeWeightage.KnowledgeBaseTemplateId == answerWeightage.KnowledgeBaseTemplateId
                                     select answerTypeWeightage).FirstOrDefault();

                    if (weightage != null)
                    {
                        BMTDataContext.AnswerTypeWeightages.DeleteOnSubmit(weightage);
                        BMTDataContext.SubmitChanges();
                    }
                }

                var parentKnowledgeBaseIds = (from knowledgeBaseParent in BMTDataContext.KnowledgeBaseTemplates
                                              where knowledgeBaseParent.ParentKnowledgeBaseId == subHeaderId
                                              select knowledgeBaseParent).ToList();

                foreach (var parentId in parentKnowledgeBaseIds)
                {
                    var kbtChildRecord = (from knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                          where knowledgeBaseTemplate.ParentKnowledgeBaseId == parentId.ParentKnowledgeBaseId
                                          select knowledgeBaseTemplate).FirstOrDefault();

                    if (kbtChildRecord != null)
                    {
                        BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbtChildRecord);
                        BMTDataContext.SubmitChanges();
                    }
                }

                var kbtRecord = (from KBK in BMTDataContext.KnowledgeBaseTemplates
                                 where KBK.KnowledgeBaseId == subHeaderId
                                 select KBK).SingleOrDefault();

                if (kbtRecord != null)
                {
                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbtRecord);
                    BMTDataContext.SubmitChanges();
                }

                var kbRecord = (from KB in BMTDataContext.KnowledgeBases
                                where KB.KnowledgeBaseId == subHeaderId
                                select KB).SingleOrDefault();

                if (kbRecord != null)
                {
                    BMTDataContext.KnowledgeBases.DeleteOnSubmit(kbRecord);
                    BMTDataContext.SubmitChanges();
                }


                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool AddQuestion(int subHeaderId, string questionName, int templateId)
        {
            try
            {
                var record = (from question in BMTDataContext.KnowledgeBases
                              select new { question }).FirstOrDefault();

                KnowledgeBase knowledgeBase = new KnowledgeBase();

                knowledgeBase.KnowledgeBaseTypeId = 3;
                knowledgeBase.Name = questionName.Trim();
                knowledgeBase.AccessId = 1;
                knowledgeBase.MustPass = true;
                knowledgeBase.CreatedBy = 1;
                knowledgeBase.IsActive = true;
                BMTDataContext.KnowledgeBases.InsertOnSubmit(knowledgeBase);
                BMTDataContext.SubmitChanges();

                KnowledgeBaseTemplate knowledgeBaseTemplate = new KnowledgeBaseTemplate();
                knowledgeBaseTemplate.KnowledgeBaseId = knowledgeBase.KnowledgeBaseId;
                knowledgeBaseTemplate.TemplateId = templateId;
                knowledgeBaseTemplate.ParentKnowledgeBaseId = subHeaderId;
                BMTDataContext.KnowledgeBaseTemplates.InsertOnSubmit(knowledgeBaseTemplate);
                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool UpdateQuestions(int headerId, string questionName)
        {
            try
            {

                var record = (from question in BMTDataContext.KnowledgeBases
                              where question.KnowledgeBaseId == headerId
                              select question).FirstOrDefault();


                if (record != null)
                {
                    record.Name = questionName.Trim();
                    BMTDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DeleteQuestion(int questionId)
        {
            try
            {
                var subHeaderId = (from parentKnowledgeBaseId in BMTDataContext.KnowledgeBaseTemplates
                                   join knowledgeBase in BMTDataContext.KnowledgeBases
                                       on parentKnowledgeBaseId.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                   where knowledgeBase.KnowledgeBaseId == questionId
                                   select parentKnowledgeBaseId.ParentKnowledgeBaseId).FirstOrDefault();

                var scoringRuleList = (from scoringRule in BMTDataContext.ScoringRules
                                       join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                           on scoringRule.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                       join knowledgeBase in BMTDataContext.KnowledgeBases
                                           on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                       where knowledgeBase.KnowledgeBaseId == subHeaderId
                                       select scoringRule).ToList();

                foreach (var kBScore in scoringRuleList)
                {
                    var score = (from scoringRule in BMTDataContext.ScoringRules
                                 where scoringRule.KnowledgeBaseTemplateId == kBScore.KnowledgeBaseTemplateId
                                 select scoringRule).FirstOrDefault();
                    if (score != null)
                    {
                        BMTDataContext.ScoringRules.DeleteOnSubmit(score);
                        BMTDataContext.SubmitChanges();
                    }
                }

                var answerTypeWeightageList = (from anserTypeEnumWeightage in BMTDataContext.AnswerTypeWeightages
                                               join knowledgeBaseTemplate in BMTDataContext.KnowledgeBaseTemplates
                                                   on anserTypeEnumWeightage.KnowledgeBaseTemplateId equals knowledgeBaseTemplate.KnowledgeBaseTemplateId
                                               join knowledgeBase in BMTDataContext.KnowledgeBases
                                                   on knowledgeBaseTemplate.KnowledgeBaseId equals knowledgeBase.KnowledgeBaseId
                                               where knowledgeBase.KnowledgeBaseId == subHeaderId
                                               select anserTypeEnumWeightage).ToList();

                foreach (var answerWeightage in answerTypeWeightageList)
                {
                    var weightage = (from answerTypeWeightage in BMTDataContext.AnswerTypeWeightages
                                     where answerTypeWeightage.KnowledgeBaseTemplateId == answerWeightage.KnowledgeBaseTemplateId
                                     select answerTypeWeightage).FirstOrDefault();

                    if (weightage != null)
                    {
                        BMTDataContext.AnswerTypeWeightages.DeleteOnSubmit(weightage);
                        BMTDataContext.SubmitChanges();
                    }
                }


                // DELETING QUESTIONS FROM KnowledgeBaseTempate And KnowledgeBase
                var kbtRecord = (from KBK in BMTDataContext.KnowledgeBaseTemplates
                                 where KBK.KnowledgeBaseId == questionId
                                 select KBK).SingleOrDefault();
                if (kbtRecord != null)
                {
                    BMTDataContext.KnowledgeBaseTemplates.DeleteOnSubmit(kbtRecord);
                }
                var record = (from questionRecord in BMTDataContext.KnowledgeBases
                              where questionRecord.KnowledgeBaseId == questionId
                              select questionRecord).SingleOrDefault();

                if (record != null)
                {
                    BMTDataContext.KnowledgeBases.DeleteOnSubmit(record);
                }

                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception exception)
            {
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

        #endregion
    }
}

