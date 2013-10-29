using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMT
{
    public partial class LogoUploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["userName"] != null)
            {
                hiddenUserName.Value = Request.QueryString["userName"].ToString();
            }
        }
    }
}