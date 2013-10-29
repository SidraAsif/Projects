using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace BMTBLL
{
    public class GetKnowledgebaseADO
    {
        public DataTable GetAllKnowledgeBaseOfTemplate(int templateId, int projectUsageId,int siteId, int parentId)
        {

            try
            {

                SqlConnection sqlConn = null;
                DataSet ds = new DataSet();
                //List<KBTemplateElement> newlist = new List<KBTemplateElement>();
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["BMTConnectionString"];

                string connString = cs.ConnectionString;

                // Open our connection

                if (sqlConn != null)
                {

                    if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }

                }

                sqlConn = new SqlConnection(connString);

                sqlConn.Open();

                string sp = "usp_Get_All_KnowledgeBase_Template";

                SqlCommand dbCommand = new SqlCommand(sp, sqlConn);

                dbCommand.CommandType = CommandType.StoredProcedure;
                
                dbCommand.Parameters.Add(new SqlParameter("@param_TemplateId", templateId));
                dbCommand.Parameters.Add(new SqlParameter("@param_ProjectUsageId", projectUsageId));
                dbCommand.Parameters.Add(new SqlParameter("@param_SiteId", siteId));
                dbCommand.Parameters.Add(new SqlParameter("@param_ParentId", parentId));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = dbCommand;

                da.Fill(ds, "KBTemplate");
                DataTable dt = new DataTable();
                dt  = ds.Tables["KBTemplate"];

                sqlConn.Close();
                sqlConn.Dispose();

                return dt;
            }

            catch (Exception ex)
            {
                throw ex;

            }
        }

        public DataTable GetAllTemplateDocuments(int templateId, int projectUsageId, int siteId)
        {

            try
            {

                SqlConnection sqlConn = null;
                DataSet ds = new DataSet();
                //List<KBTemplateElement> newlist = new List<KBTemplateElement>();
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["BMTConnectionString"];

                string connString = cs.ConnectionString;

                // Open our connection

                if (sqlConn != null)
                {

                    if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }

                }

                sqlConn = new SqlConnection(connString);

                sqlConn.Open();

                string sp = "usp_Get_All_Template_Documents";

                SqlCommand dbCommand = new SqlCommand(sp, sqlConn);

                dbCommand.CommandType = CommandType.StoredProcedure;

                dbCommand.Parameters.Add(new SqlParameter("@param_TemplateId", templateId));
                dbCommand.Parameters.Add(new SqlParameter("@param_ProjectUsageId", projectUsageId));
                dbCommand.Parameters.Add(new SqlParameter("@param_SiteId", siteId));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = dbCommand;

                da.Fill(ds, "KBTemplate");
                DataTable dt = new DataTable();
                dt = ds.Tables["KBTemplate"];

                sqlConn.Close();
                sqlConn.Dispose();

                return dt;
            }

            catch (Exception ex)
            {
                throw ex;

            }
        }

        //public DataTable GetScoringRules(int templateId, int subHeaderTemplateId)
        //{

        //    try
        //    {

        //        SqlConnection sqlConn = null;
        //        DataSet ds = new DataSet();
        //        //List<KBTemplateElement> newlist = new List<KBTemplateElement>();
        //        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["BMTConnectionString"];

        //        string connString = cs.ConnectionString;

        //        // Open our connection

        //        if (sqlConn != null)
        //        {

        //            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }

        //        }

        //        sqlConn = new SqlConnection(connString);

        //        sqlConn.Open();

        //        string sp = "usp_Get_Scoring_Rules";

        //        SqlCommand dbCommand = new SqlCommand(sp, sqlConn);

        //        dbCommand.CommandType = CommandType.StoredProcedure;

        //        dbCommand.Parameters.Add(new SqlParameter("@param_TemplateId", templateId));
        //        dbCommand.Parameters.Add(new SqlParameter("@param_SubHeaderkbTempId", subHeaderTemplateId));

        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = dbCommand;

        //        da.Fill(ds, "ScoringRules");
        //        DataTable dt = new DataTable();
        //        dt = ds.Tables["ScoringRules"];

        //        sqlConn.Close();
        //        sqlConn.Dispose();

        //        return dt;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}

        //public DataTable GetAnswerWeightageByEnumId(int AnswerTypeEnumId, int knowledgeBaseTemplateId, int projectId)
        //{

        //    try
        //    {

        //        SqlConnection sqlConn = null;
        //        DataSet ds = new DataSet();
        //        List<KBTemplateElement> newlist = new List<KBTemplateElement>();
        //        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["BMTConnectionString"];

        //        string connString = cs.ConnectionString;

        //         Open our connection

        //        if (sqlConn != null)
        //        {

        //            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }

        //        }

        //        sqlConn = new SqlConnection(connString);

        //        sqlConn.Open();

        //        string sp = "usp_Get_Answer_Type_Weightage";

        //        SqlCommand dbCommand = new SqlCommand(sp, sqlConn);

        //        dbCommand.CommandType = CommandType.StoredProcedure;

        //        dbCommand.Parameters.Add(new SqlParameter("@param_AnsTypeId", AnswerTypeEnumId));
        //        dbCommand.Parameters.Add(new SqlParameter("@param_ProjectId", projectId));
        //        dbCommand.Parameters.Add(new SqlParameter("@param_KBTempId", knowledgeBaseTemplateId));

        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = dbCommand;

        //        da.Fill(ds, "Weightage");
        //        DataTable dt = new DataTable();
        //        dt = ds.Tables["Weightage"];

        //        sqlConn.Close();
        //        sqlConn.Dispose();

        //        return dt;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}

    }
}
