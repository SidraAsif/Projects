using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMT
{
    public partial class GenericFileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["TableName"] != null && Session["PracticeId"] != null && Session["SectionId"] != null)
                {
                    hiddenTableName.Value = Session["TableName"].ToString();
                    hiddenPracticeId.Value = Session["PracticeId"].ToString();
                    hiddenSectionId.Value = Session["SectionId"].ToString();
                }
               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}