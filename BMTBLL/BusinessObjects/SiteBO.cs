#region Modification History

//  ******************************************************************************
//  Module        : Practice Sites
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/02/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                         Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig        12/12/2011      Create New project against the Site
//  Mirza Fahad Ali Baig        01/06/2012      GetSiteAddressBySiteId 
//  Mirza Fahad Ali Baig        02-27-2012      Remove Un-nessary code
//  Mirza Fahad Ali Baig        02-27-2012      Optimize the current code
//  Mirza Fahad Ali Baig        02-27-2012      Optimize IsSiteNameExists & UpdatePracticeSite Function + Remove extra code  
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class SiteBO : AddressBO
    {
        #region CONSTANTS
        private const string DEFAULT_NAME = "BizMed";
        private const bool DEFAULT_ISMAINSITE = true;

        #endregion

        #region PROPERTIES

        private PracticeSite _site { get; set; }
        private Project _project { get; set; }

        private ProjectBO _projectDetail { get; set; }

        private IQueryable _practiceSiteList;
        public IQueryable PracticeSiteList
        {
            get { return _practiceSiteList; }
            set { _practiceSiteList = value; }
        }

        private int _siteId;
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        private int _practiceId;
        public int PracticeId
        {

            get { return _practiceId; }
            set { _practiceId = value; }
        }

        private string _name;
        public string Name
        {

            get { return _name; }
            set { _name = value; }
        }

        private int _AddressId;
        public int AddressId
        {

            get { return _AddressId; }
            set { _AddressId = value; }
        }

        private int _sitePrimarySpecialityId;
        public int SitePrimarySpecialityId
        {
            get { return _sitePrimarySpecialityId; }
            set { _sitePrimarySpecialityId = value; }
        }

        private int _numberOfProvider;
        public int NumberOfProvider
        {
            get { return _numberOfProvider; }
            set { _numberOfProvider = value; }
        }

        private string _siteGroupNPI;
        public string SiteGroupNPI
        {
            get { return _siteGroupNPI; }
            set { _siteGroupNPI = value; }
        }

        private bool _isMainSite;
        public bool IsMainSite
        {
            get { return _isMainSite; }
            set { _isMainSite = value; }
        }

        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        private int _createdBy;
        public int CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public string _contactName;
        public string ContactName
        {

            get { return _contactName; }
            set { _contactName = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public SiteBO()
        {
            _name = DEFAULT_NAME;
            _isMainSite = DEFAULT_ISMAINSITE;
            _createdDate = System.DateTime.Now;
        }

        #endregion

        #region FUNCTIONS
        public bool SavePracticeSite()
        {
            try
            {
                _site = new PracticeSite();
                _project = new Project();
                _projectDetail = new ProjectBO();

                if (this.SiteId == 0)
                    return AddNewPracticeSite();
                else
                    return UpdatePracticeSite();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsSiteNameExists(string siteName)
        {
            try
            {
                int receivedSiteId = (from practiceSiteRecord in BMTDataContext.PracticeSites
                                      where practiceSiteRecord.Name == siteName && practiceSiteRecord.PracticeId == this.PracticeId
                                      && practiceSiteRecord.PracticeSiteId != this.SiteId
                                      select practiceSiteRecord.PracticeSiteId).FirstOrDefault();
                if (receivedSiteId == 0 || receivedSiteId == null)
                    return false;
                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool AddNewPracticeSite()
        {
            try
            {
                bool _isSiteNameExist = IsSiteNameExists(Name);
                if (!_isSiteNameExist)
                {
                    _site.PracticeId = this.PracticeId;
                    _site.Name = this.Name;
                    _site.AddressId = this.AddressId;
                    _site.SpecialityId = this.SitePrimarySpecialityId;
                    _site.SiteGroupNPI = this.SiteGroupNPI;
                    _site.NumberOfProviders = this.NumberOfProvider;
                    _site.IsMainSite = this.IsMainSite;
                    _site.CreatedDate = this.CreatedDate;
                    _site.CreatedBy = this.CreatedBy;
                    _site.ContactName = this.ContactName;

                    BMTDataContext.PracticeSites.InsertOnSubmit(_site);
                    BMTDataContext.SubmitChanges();

                    // Get site Id to Create a project for the site
                    //_siteId = _site.PracticeSiteId;

                    // Create project against the created site
                    //_project.PracticeId = this.PracticeId;
                    //_project.PracticeSiteId = this.SiteId;
                    //_project.Name = _projectDetail.Name;
                    //_project.Description = _projectDetail.Description;
                    //_project.CreatedOn = _projectDetail.CreatedDate;
                    //_project.CreatedBy = this.CreatedBy;
                    //BMTDataContext.Projects.InsertOnSubmit(_project);
                    //BMTDataContext.SubmitChanges();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool UpdatePracticeSite()
        {
            try
            {
                bool _isSiteNameExist = IsSiteNameExists(Name);
                if (!_isSiteNameExist)
                {

                    var SiteInfo = (from SiteRecord in BMTDataContext.PracticeSites
                                    where (SiteRecord.PracticeSiteId == this.SiteId)
                                    select SiteRecord).First();

                    SiteInfo.Name = this.Name;
                    SiteInfo.SpecialityId = this.SitePrimarySpecialityId;
                    SiteInfo.SiteGroupNPI = this.SiteGroupNPI;
                    SiteInfo.NumberOfProviders = this.NumberOfProvider;
                    SiteInfo.IsMainSite = this.IsMainSite;
                    SiteInfo.ContactName = this.ContactName;

                    BMTDataContext.SubmitChanges();
                    return true;
                }
                else
                    return false;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void GetSitesByPracticeId(int PracticeId)
        {
            try
            {
                _practiceSiteList = (from SiteRecord in BMTDataContext.PracticeSites
                                     join AddressRecord in BMTDataContext.Addresses on SiteRecord.AddressId equals AddressRecord.AddressId
                                     where SiteRecord.PracticeId == PracticeId
                                     orderby SiteRecord.Name
                                     select new
                                     {
                                         SiteRecord.PracticeSiteId,
                                         SiteRecord.Name,
                                         IsMainSite = SiteRecord.IsMainSite.ToString() == "True" ? "Yes" : "No",
                                         AddressRecord.City,
                                         AddressRecord.State
                                     }).AsQueryable();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<PracticeSite> GetSiteListByPracticeId(int practiceId)
        {
            try
            {
                List<PracticeSite> practiceSiteList = new List<PracticeSite>();
                practiceSiteList = (from SiteRecord in BMTDataContext.PracticeSites
                                    where SiteRecord.PracticeId == practiceId
                                    orderby SiteRecord.Name
                                    select SiteRecord).ToList();

                return practiceSiteList;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public void GetSiteBySiteId(int SiteId)
        {
            try
            {

                _siteId = SiteId;
                var UserSite = (from SiteRecord in BMTDataContext.PracticeSites
                                from AddressRecord in BMTDataContext.Addresses
                                where (SiteRecord.PracticeSiteId == this.SiteId)
                                    && (AddressRecord.AddressId == SiteRecord.AddressId)
                                select new { SiteRecord, AddressRecord }).SingleOrDefault();

                _practiceId = UserSite.SiteRecord.PracticeId;
                _name = UserSite.SiteRecord.Name;
                _AddressId = UserSite.SiteRecord.AddressId.HasValue ? (int)UserSite.SiteRecord.AddressId : 0;
                _sitePrimarySpecialityId = UserSite.SiteRecord.SpecialityId.HasValue ? (int)UserSite.SiteRecord.SpecialityId : 0;
                _numberOfProvider = UserSite.SiteRecord.NumberOfProviders.HasValue ? (int)UserSite.SiteRecord.NumberOfProviders : 0;
                _siteGroupNPI = UserSite.SiteRecord.SiteGroupNPI;
                _isMainSite = UserSite.SiteRecord.IsMainSite;
                _createdDate = UserSite.SiteRecord.CreatedDate;
                _contactName = UserSite.SiteRecord.ContactName;

                _primaryAddress = UserSite.AddressRecord.PrimaryAddress;
                _secondaryAddress = UserSite.AddressRecord.SecondaryAddress;
                _city = UserSite.AddressRecord.City;
                _state = UserSite.AddressRecord.State;
                _zipCode = UserSite.AddressRecord.ZipCode;
                _phone = UserSite.AddressRecord.Telephone;
                _mobile = UserSite.AddressRecord.Mobile;
                _fax = UserSite.AddressRecord.Fax;
                _email = UserSite.AddressRecord.Email;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int DeleteSiteBySiteId(int SiteId)
        {
            try
            {

                _site = new PracticeSite();
                int response = 0;
                _siteId = SiteId;

                // Check If site has users
                var ChildRecord = (from SiteRecord in BMTDataContext.PracticeSites
                                   join UserReocord in BMTDataContext.PracticeUsers
                                      on SiteRecord.PracticeSiteId equals UserReocord.PracticeSiteId
                                   where SiteRecord.PracticeSiteId == this.SiteId
                                   select SiteRecord);

                //int siteProjectId = (from projectRecord in BMTDataContext.Projects
                //                     //where projectRecord.PracticeSiteId == this.SiteId
                //                     select projectRecord.ProjectId).FirstOrDefault();

                // Check If site has childs records
                var SubChildRecords = (from SiteRecord in BMTDataContext.PracticeSites
                                       from FilledQuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                       from FilledAnswerRecord in BMTDataContext.FilledAnswers
                                       where FilledQuestionnaireRecord.PracticeSiteId==SiteId
                                                 || FilledAnswerRecord.PracticeSiteId == SiteId
                                       select SiteRecord);

                if (ChildRecord.Any() || SubChildRecords.Any())
                { response = (int)enSiteCheck.HasParent; }
                else
                {

                    var UserSite = (from SiteRecord in BMTDataContext.PracticeSites
                                    from AddressRecord in BMTDataContext.Addresses
                                    where (SiteRecord.PracticeSiteId == this.SiteId)
                                        && (AddressRecord.AddressId == SiteRecord.AddressId)
                                    select new { SiteRecord, AddressRecord }).SingleOrDefault();

                    if (UserSite.AddressRecord != null && UserSite.SiteRecord != null)
                    {
                        /* Get Site Practice Id */
                        int SitePracticeId = UserSite.SiteRecord.PracticeId;

                        /*Check if more site is available*/
                        int TotalSiteaAgainstPractice = (from SiteRecord in BMTDataContext.PracticeSites
                                                         where SiteRecord.PracticeId == SitePracticeId
                                                         select SiteRecord).ToList().Count();

                        if (TotalSiteaAgainstPractice > 1)
                        {

                            BMTDataContext.PracticeSites.DeleteOnSubmit(UserSite.SiteRecord);
                            BMTDataContext.SubmitChanges();

                            BMTDataContext.Addresses.DeleteOnSubmit(UserSite.AddressRecord);
                            BMTDataContext.SubmitChanges();
                            response = (int)enSiteCheck.Pass;
                        }
                        else { response = (int)enSiteCheck.NoMultiple; }
                    }
                    else
                    { response = (int)enSiteCheck.Invalid; }
                }
                return response;

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public string GetSiteAddressBySiteId(int siteId)
        {
            try
            {

                var siteAddresss = (from siteRecord in BMTDataContext.PracticeSites
                                    join addressRecord in BMTDataContext.Addresses
                                     on siteRecord.AddressId equals addressRecord.AddressId
                                    where siteRecord.PracticeSiteId == siteId
                                    select new
                                    {
                                        address = (addressRecord.City.ToString().Trim() != string.Empty ? addressRecord.City.ToString() + ", " : string.Empty)
                                        + addressRecord.State.ToString()
                                    }).AsQueryable();
                string address = string.Empty;
                foreach (var item in siteAddresss)
                {

                    address = item.address.ToString();
                }

                return address;


            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        #endregion
    }
}
