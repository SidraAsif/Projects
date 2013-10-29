using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMT.UserControls
{
    public partial class LoadingPanel : System.Web.UI.UserControl
    {

        #region Variables

        public string Message { get; set; }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Value = Message;
        }

        #endregion
    }

}