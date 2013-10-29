using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;


public partial class General : System.Web.UI.UserControl
{
    #region PROPERTIES
    public int ProjectId { get; set; }
    public int PracticeId { get; set; }
    public string PracticeName { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; }

    #endregion

    #region VARIABLES
    private SiteBO siteBO;
    private UserBO userBO;
    private CorporateElementSubmissionBO corporateElementSubmissionBO;

    private int currentIndex;
    private int selectedchkbx;
    private int practiceId;
    private IQueryable _primaryCareProviderList;

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Session["UserApplicationId"] == null || Session["UserType"] == null || Session["PracticeId"] == null)
                Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut", false);

            GetPrimaryCareProviders();
            CheckForCorporateType();
            practiceId = Convert.ToInt32(Session["PracticeId"].ToString());
            hiddenPracticeId.Value = Convert.ToString(practiceId);
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    protected void gvPrimaryCareProvider_OnPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvPrimaryCareProvider.PageIndex = e.NewPageIndex;
            gvPrimaryCareProvider.DataSource = _primaryCareProviderList;
            gvPrimaryCareProvider.DataBind();

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    #endregion

    #region FUNCTIONS

    protected void GetPrimaryCareProviders()
    {
        try
        {
            if (SiteId != 0)
            {
                lblSiteInfo.Text = SiteName;
                hiddenProjectId.Value = Convert.ToString(ProjectId);
                hiddenPracticeId.Value = Convert.ToString(PracticeId);
                hiddenPracticeName.Value = PracticeName;
                hiddenSiteName.Value = SiteName;
                hiddenSiteId.Value = Convert.ToString(SiteId);
                string siteAddress = GetSiteAddressBySiteId(SiteId);
                siteAddress = siteAddress.Trim() != string.Empty ? ", " + siteAddress : "";
                lblsiteDescription.Text = SiteName + siteAddress;
                _primaryCareProviderList = GetPrimaryCareProviderAtSite(SiteId);
                gvPrimaryCareProvider.DataSource = _primaryCareProviderList;
                gvPrimaryCareProvider.DataBind();
            }

        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }

    protected string GetSiteAddressBySiteId(int _siteId)
    {
        try
        {
            siteBO = new SiteBO();
            string _siteAddress = siteBO.GetSiteAddressBySiteId(_siteId);
            return _siteAddress;
        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected IQueryable GetPrimaryCareProviderAtSite(int _siteId)
    {
        try
        {
            siteBO = new SiteBO();
            siteBO.SiteId = _siteId;
            userBO = new UserBO();
            IQueryable _primaryCareProviderList = userBO.GetPrimaryCareProviderList(siteBO);
            return _primaryCareProviderList;

        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void CheckForCorporateType()
    {
        try
        {
            if (SiteId != 0)
            {
                PracticeBO _practice = new PracticeBO();
                bool ifSiteIsCorporate = _practice.CheckCorporateType(SiteId);
                hiddenSiteId.Value = Convert.ToString(SiteId);
                if (ifSiteIsCorporate)
                    visibleOnlyForCorporateSite.Visible = true;
                else
                    visibleOnlyForCorporateSite.Visible = false;
            }
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }
    }

    #endregion
}
