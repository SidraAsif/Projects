using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BMT.WEB;
using System.Text;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder aboutUs = new StringBuilder();

            int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);

            aboutUs.Append(Util.GetSystemAboutUs(enterpriseId) + "<br />");
            aboutUs.Append(Util.GetSystemVersion(enterpriseId) + "<br />");
            aboutUs.Append(Util.GetSystemCopyright(enterpriseId) + "<br />");            
            aboutUs.Append("View <a href='../StDocs/General/License_Agreement.pdf' target='_blank'>License Agreement");
            aboutUs.Append("</a> and <a href='../StDocs/General/Privacy_Policy.pdf' target='_blank'>Privacy Policy</a>");

            lblAboutUs.Text = aboutUs.ToString();
            
            
        }
    }
}