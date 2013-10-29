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
    public partial class MOReCorporateSubmission : System.Web.UI.Page
    {
        #region CONSTANTS
        private char[] DELIMITATOR_STANDARD = new char[] { ':' };
        private int NAME_INDEX = 0;
        private int DESCRIPTION_INDEX = 1;
        #endregion

        #region VARIABLES
        private int currentIndex;
        private int practiceId;
        private int siteId;
        private int templateId;
        private string[] kbCorpElementName;
        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            practiceId = Convert.ToInt32(Request.QueryString["practiceId"]);
            siteId = Convert.ToInt32(Request.QueryString["siteId"]);
            templateId = Convert.ToInt32(Request.QueryString["TemplateId"]);
            hdnPracticeId.Value = practiceId.ToString();
            hdnTemplateId.Value = templateId.ToString();
            GetCorporateElementList(templateId);
            hdnRecievedQuestionnaire.Value= Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();

        }
        #endregion

        #region FUNCTION
        protected void GetCorporateElementList(int templateId)
        {
            CorporateElementListBO _corporateElementListBO = new CorporateElementListBO();
            List<KBCorporateElement> corporateElementList = _corporateElementListBO.GetKBTempCorpElementListByTempID(templateId);
            try
            {
                Table CorpTable = new Table();
                for (currentIndex = 0; currentIndex < corporateElementList.Count; currentIndex++)
                {
                    TableRow CorpElementName = new TableRow();
                    TableCell chkbxKB = new TableCell();

                    CheckBox chk = new CheckBox();
                    chk.ClientIDMode = ClientIDMode.Static;
                    chk.ID = (corporateElementList[currentIndex].Id).ToString();
                    kbCorpElementName = corporateElementList[currentIndex].Name.Split(DELIMITATOR_STANDARD, StringSplitOptions.RemoveEmptyEntries);
                    chk.Text = "<strong>" + kbCorpElementName[NAME_INDEX] + ": </strong>" + kbCorpElementName[DESCRIPTION_INDEX];
                    if (_corporateElementListBO.AlreadyKBCorporateElement(practiceId, siteId,corporateElementList[currentIndex].Id))
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