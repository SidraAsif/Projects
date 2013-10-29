using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

namespace BMT.Webforms
{
    public partial class CorporateSubmission : System.Web.UI.Page
    {
        #region VARIABLES
        private int currentIndex;
        private int practiceId;
        private int siteId;
        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            practiceId = Convert.ToInt32(Request.QueryString["practiceId"]);
            siteId = Convert.ToInt32(Request.QueryString["siteId"]);
            hdnPracticeId.Value = practiceId.ToString();
            GetCorporateElementList();
            hdnRecievedQuestionnaire.Value= Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();

        }
        #endregion

        #region FUNCTION
        protected void GetCorporateElementList()
        {
            CorporateElementListBO _corporateElementListBO = new CorporateElementListBO();
            List<CorporateElementList> corporateElementList = _corporateElementListBO.GetElementList();
            try
            {
                Table CorpTable = new Table();
                for (currentIndex = 0; currentIndex < corporateElementList.Count; currentIndex++)
                {
                    TableRow CorpElementName = new TableRow();
                    TableCell chkbxKB = new TableCell();

                    CheckBox chk = new CheckBox();
                    chk.ClientIDMode = ClientIDMode.Static;
                    chk.ID = "corpElements" + currentIndex;
                    chk.Text = "<strong>" + corporateElementList[currentIndex].ElementName + ": </strong>" + corporateElementList[currentIndex].ElementDescription;
                    if (_corporateElementListBO.AlreadyCorporateElement(practiceId,siteId, corporateElementList[currentIndex].ElementName))
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                    chkbxKB.Controls.Add(chk);
                    CorpElementName.Controls.Add(chkbxKB);
                    CorpTable.Controls.Add(CorpElementName);
                }

                elementListDiv.Controls.Add(CorpTable);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}