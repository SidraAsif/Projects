using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using BMTBLL.Enumeration;
using BMTBLL.Classes;

namespace BMTBLL 
{
    public class PCMHReportBO : BMTConnection 
    {

        #region FUNCTION
        
        public string GetQuestionnaireByType()
        {
            try
            {
                string question = string.Empty;

                // check if Question is already submitted against the selected project
                var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                           orderby QuestionnaireRecord.LastUpdated descending
                                           select QuestionnaireRecord).FirstOrDefault();

                if (filledQuestionnaire != null)
                    return Convert.ToString(filledQuestionnaire.Answers);
                else
                    question = "";

                return question;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public IQueryable GetAllPracticeList()
        {
            try
            {
                IQueryable List = (from practice in BMTDataContext.Practices
                                   select new
                                   {
                                       ID = practice.PracticeId,
                                       Name = practice.Name
                                   }) as IQueryable;

                return List;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<PCMHStatusReport> GetPCMHReportData(string factorSequences, string consultantId, string practiceSizeId, string percentComplete, int medicalId)
        {
            try
            {
                if (consultantId.StartsWith("0,") || consultantId.StartsWith("00"))
                    consultantId = "0";
                if (practiceSizeId.StartsWith("0,") || practiceSizeId.StartsWith("00"))
                    practiceSizeId = "0";
                List<PCMHStatusReport> result = BMTDataContext.usp_Get_PCMH_Report_Data(factorSequences, consultantId, practiceSizeId, percentComplete, medicalId).ToList<PCMHStatusReport>();
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;

            }
        }
        public List<OverAllProjectStatusDetails> GetOverAllProjectStatusList(string factorSequences, string consultantId, int medicalId)
        {
            try
            {
                if (consultantId.StartsWith("0,") || consultantId.StartsWith("00"))
                    consultantId = "0";

                List<OverAllProjectStatusDetails> lstOverAllProjectStatusDetails = BMTDataContext.usp_Get_OverAll_Project_Status(factorSequences, consultantId, medicalId).ToList<OverAllProjectStatusDetails>();
                return lstOverAllProjectStatusDetails;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<GroupProjectStatusDetails> GetGroupProjectStatusList(string factorSequences, string consultantId, string practiceSizeId, int medicalId)
        {
            try
            {
                if (consultantId.StartsWith("0,") || consultantId.StartsWith("00"))
                    consultantId = "0";
                if (practiceSizeId.StartsWith("0,") || practiceSizeId.StartsWith("00"))
                    practiceSizeId = "0";

                List<GroupProjectStatusDetails> lstGroupProjectStatusDetails = BMTDataContext.usp_Get_Group_Project_Status(factorSequences, consultantId, practiceSizeId, medicalId).ToList<GroupProjectStatusDetails>();
                return lstGroupProjectStatusDetails;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public List<OverAllBarProjectStatusDetails> GetOverAllBarProjectStatusList(string factorSequences, string consultantId, string practiceSizeId, int medicalId)
        {
            try
            {
                if (consultantId.StartsWith("0,") || consultantId.StartsWith("00"))
                    consultantId = "0";
                if (practiceSizeId.StartsWith("0,") || practiceSizeId.StartsWith("00"))
                    practiceSizeId = "0";

                List<OverAllBarProjectStatusDetails> lstOverAllBarProjectStatusDetails = BMTDataContext.usp_Get_OverAllBar_Project_Status(factorSequences, consultantId, practiceSizeId, medicalId).ToList<OverAllBarProjectStatusDetails>();
                return lstOverAllBarProjectStatusDetails;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}
