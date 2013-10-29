using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMTBLL.Enumeration;
using BMTBLL.Helper;
using BMT.WEB;
using BMTBLL;

namespace BMT.Webforms
{
    public partial class NCQAUploadedDocs : System.Web.UI.Page
    {
        #region DATA_MEMBER

        #endregion

        #region CONSTANTS
        private const string DEFAULT_ASC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/asc.png' />";
        private const string DEFAULT_DESC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/desc.png' />";
        private const string DEFAULT_ASC_DESC_IMAGE_HTML = "&nbsp;<img src='../Themes/Images/asc-desc.png' />";

        // sorting columns only
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT = "Factor";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT = "Type";
        private const string DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT = "Last Uploaded";

        #endregion

        #region EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    // reset column view state
                    ResetSortingState();
                    DisplayDocs();
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void datagridDocViewer_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression.ToString();
                string currentSortingState = string.Empty;
                List<NCQADetails> _listOfNCQADetail = (List<NCQADetails>)Session["listNCQADetails"];
                // Reset column images on page changed
                ResetColumnSortingImage();

                switch (sortExpression)
                {
                    case "FactorSequence":

                        currentSortingState = ViewState["FactorSequence"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridDocViewer.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderBy(SortExpression => SortExpression.FactorSequence).ToList();
                            ViewState["FactorSequence"] = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridDocViewer.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderByDescending(SortExpression => SortExpression.FactorSequence).ToList();
                            ViewState["FactorSequence"] = enSortingType.Descending.ToString();
                        }
                        break;

                    case "Type":

                        currentSortingState = ViewState["Type"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridDocViewer.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderBy(SortExpression => SortExpression.Type).ToList();
                            ViewState["Type"] = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridDocViewer.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderByDescending(SortExpression => SortExpression.Type).ToList();
                            ViewState["Type"] = enSortingType.Descending.ToString();
                        }
                        break;
                    case "LastUpdatedDate":

                        currentSortingState = ViewState["LastUpdatedDate"].ToString();
                        if ((currentSortingState == enSortingType.Normal.ToString()) || (currentSortingState == enSortingType.Descending.ToString()))
                        {
                            datagridDocViewer.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderBy(SortExpression => SortExpression.LastUpdatedDate).ToList();
                            ViewState["LastUpdatedDate"] = enSortingType.Ascending.ToString();
                        }
                        else
                        {
                            datagridDocViewer.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_DESC_IMAGE_HTML;
                            _listOfNCQADetail = _listOfNCQADetail.OrderByDescending(SortExpression => SortExpression.LastUpdatedDate).ToList();
                            ViewState["LastUpdatedDate"] = enSortingType.Descending.ToString();
                        }
                        break;


                    default:
                        return;

                }

                datagridDocViewer.DataSource = _listOfNCQADetail;
                datagridDocViewer.DataBind();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }
        #endregion

        #region FUNCTIONS
        private void DisplayDocs()
        {
            List<NCQADetails> _listOfNCQADetail;
            if (Request.QueryString["pcmhId"] != null && Request.QueryString["elementId"] != null && Request.QueryString["practiceId"] != null
            && Request.QueryString["siteId"] != null && Request.QueryString["projectId"] != null)
            {
                string pcmhSequenceId = Request.QueryString["pcmhId"].ToString();
                string elementSequenceId = Request.QueryString["elementId"];
                int practiceId = Convert.ToInt32(Request.QueryString["practiceId"]);
                int siteId = Convert.ToInt32(Request.QueryString["siteId"]);
                int projectId = Convert.ToInt32(Request.QueryString["projectId"]);

                // get list of docs + comments
                Session["listNCQADetails"]= _listOfNCQADetail = NCQADataHelper.GetDocsByElementId(pcmhSequenceId, elementSequenceId, practiceId, siteId, projectId);

                // get Practice and Site Name
                PracticeBO practiceBO = new PracticeBO();
                string practiceName = practiceBO.GetPracticeNameByPracticeId(practiceId);
                hdnPracticeName.Value = System.Web.HttpUtility.JavaScriptStringEncode(practiceName);

                SiteBO siteBO = new SiteBO();
                siteBO.GetSiteBySiteId(siteId);
                string siteName = siteBO.Name;
                hdnSiteName.Value = System.Web.HttpUtility.JavaScriptStringEncode(siteName);

                // set page size to dispaly all docs in one page
                datagridDocViewer.PageSize = _listOfNCQADetail.Count() + 1;

                // remove pagination area from grid
                datagridDocViewer.PagerStyle.Visible = false;

                // bind data source
                datagridDocViewer.DataSource = _listOfNCQADetail;
                datagridDocViewer.DataBind();

                if (_listOfNCQADetail.Count() == 0)
                    pnlRecordWarning.Visible = true;
                else
                    pnlRecordWarning.Visible = false;
            }

        }

        private void ResetSortingState()
        {
            // Reset column images on page changed
            ResetColumnSortingImage();

            // reset column view state on page changed
            ViewState["FactorSequence"] = enSortingType.Normal.ToString();
            ViewState["Type"] = enSortingType.Normal.ToString();
            ViewState["LastUpdatedDate"] = enSortingType.Normal.ToString();
        }

        private void ResetColumnSortingImage()
        {
            // Reset column images on page changed
            datagridDocViewer.Columns[0].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN0_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridDocViewer.Columns[1].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN1_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
            datagridDocViewer.Columns[4].HeaderText = DEFAULT_DASHBOARD_HEADER_COLUMN4_TEXT + DEFAULT_ASC_DESC_IMAGE_HTML;
        }

        #endregion
    }
}