#region Modification History

//  ******************************************************************************
//  Module        : Practice
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01/17/2012          getPractices
//  Mirza Fahad Ali Baig    01/30/2012          Remove Super User practice from list
//  Mirza Fahad Ali Baig    02/22/2012          GetPracticeDetails & Count Practices for dashboard screen
//  Mirza Fahad Ali Baig    02-27-2012      Remove Un-nessary code
//  Mirza Fahad Ali Baig    02-27-2012      Optimize the current code
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

using BMTBLL.Helper;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class PracticeBO : AddressBO
    {
        #region CONSTANT
        private const int DEFAULT_MEDICALGROUPID = 1;
        private const string DEFAULT_NAME = "BizMED";
        private const string DEFAULT_DATE_CHANGE_COLOR = "Gray";

        #endregion

        #region PROPERTIES
        private Practice _practice { get; set; }

        private int _PracticeId;
        public int PracticeId
        {
            get { return _PracticeId; }
            set { _PracticeId = value; }
        }

        private string _Name;
        public string Name
        {

            get { return _Name; }
            set { _Name = value; }
        }

        private int _MedicalGroupId;
        public int MedicalGroupId
        {

            get { return _MedicalGroupId; }
            set { _MedicalGroupId = value; }
        }

        private int _AddressId;
        public int AddressId
        {

            get { return _AddressId; }
            set { _AddressId = value; }
        }

        private int _PracticeSizeId;
        public int PracticeSizeId
        {
            get { return _PracticeSizeId; }
            set { _PracticeSizeId = value; }
        }

        private int _SpecialityId;
        public int SpecialityId
        {
            get { return _SpecialityId; }
            set { _SpecialityId = value; }
        }

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        private int _CreatedBy;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set { _contactName = value; }
        }

        private int _practiceSiteId;
        public int PracticeSiteId
        {
            get { return _practiceSiteId; }
            set { _practiceSiteId = value; }
        }

        private bool _isCorporate;
        public bool IsCorporate
        {
            get { return _isCorporate; }
            set { _isCorporate = value; }
        }
        #endregion

        #region CONSTRUCTOR

        public PracticeBO()
        {
            _MedicalGroupId = DEFAULT_MEDICALGROUPID;
            _Name = DEFAULT_NAME;
            _CreatedDate = System.DateTime.Now;
        }

        public PracticeBO(int medicalGroupId)
        {
            _MedicalGroupId = medicalGroupId;
            _Name = DEFAULT_NAME;
            _CreatedDate = System.DateTime.Now;
        }

        #endregion

        #region VARIABLE
        XElement newElement;
        #endregion

        #region FUNCTIONS
        public int SavePractice()
        {
            try
            {
                _practice = new Practice();

                if (this.PracticeId == 0)
                    return AddNewPractice();
                else
                    return UpdatePractice();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private int AddNewPractice()
        {
            try
            {
                _practice.Name = this.Name;
                _practice.MedicalGroupId = this.MedicalGroupId;
                _practice.AddressId = this.AddressId;
                _practice.PracticeSizeId = this.PracticeSizeId;
                _practice.SpecialityId = this.SpecialityId;
                _practice.CreatedDate = this.CreatedDate;
                _practice.CreatedBy = this.CreatedBy;
                _practice.ContactName = this.ContactName;
                _practice.PracticeSiteId = this.PracticeSiteId;
                _practice.IsCorporate = this.IsCorporate;

                BMTDataContext.Practices.InsertOnSubmit(_practice);
                BMTDataContext.SubmitChanges();

                return _practice.PracticeId;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private int UpdatePractice()
        {

            try
            {
                var PracticeInfo = (from practiceRecord in BMTDataContext.Practices
                                    where practiceRecord.PracticeId == this.PracticeId
                                    select practiceRecord).First();

                if (PracticeInfo.PracticeSiteId != this.PracticeSiteId || this.IsCorporate == false)
                {
                    CorporateElementSubmissionBO corpElementSubmission = new CorporateElementSubmissionBO();
                    corpElementSubmission.DeletePreviousCorporateSubmissionList(this.PracticeId);
                }
                PracticeInfo.Name = this.Name;
                PracticeInfo.MedicalGroupId = this.MedicalGroupId;
                PracticeInfo.PracticeSizeId = this.PracticeSizeId;
                PracticeInfo.SpecialityId = this.SpecialityId;
                PracticeInfo.ContactName = this.ContactName;
                PracticeInfo.PracticeSiteId = this.PracticeSiteId;
                PracticeInfo.IsCorporate = this.IsCorporate;

                BMTDataContext.SubmitChanges();
                return this.PracticeId;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void GetPracticeByPracticeId(int PracticeId)
        {

            _PracticeId = PracticeId;
            try
            {
                var UserPractice = (from PracticeRecord in BMTDataContext.Practices
                                    from AddressRecord in BMTDataContext.Addresses
                                    where (PracticeRecord.PracticeId == this.PracticeId)
                                        && (AddressRecord.AddressId == PracticeRecord.AddressId)
                                    select new { PracticeRecord, AddressRecord }).SingleOrDefault();
                if (UserPractice != null)
                {

                    _PracticeId = UserPractice.PracticeRecord.PracticeId;
                    _Name = UserPractice.PracticeRecord.Name;
                    _MedicalGroupId = UserPractice.PracticeRecord.MedicalGroupId;
                    _AddressId = (int)UserPractice.PracticeRecord.AddressId;
                    _PracticeSizeId = (int)UserPractice.PracticeRecord.PracticeSizeId;
                    _SpecialityId = (int)UserPractice.PracticeRecord.SpecialityId;
                    _CreatedDate = UserPractice.PracticeRecord.CreatedDate;
                    _contactName = UserPractice.PracticeRecord.ContactName;

                    _primaryAddress = UserPractice.AddressRecord.PrimaryAddress;
                    _secondaryAddress = UserPractice.AddressRecord.SecondaryAddress;
                    _city = UserPractice.AddressRecord.City;
                    _state = UserPractice.AddressRecord.State;
                    _zipCode = UserPractice.AddressRecord.ZipCode;
                    _phone = UserPractice.AddressRecord.Telephone;
                    _mobile = UserPractice.AddressRecord.Mobile;
                    _fax = UserPractice.AddressRecord.Fax;
                    _email = UserPractice.AddressRecord.Email;
                    if (UserPractice.PracticeRecord.IsCorporate != null)
                        _isCorporate = (bool)UserPractice.PracticeRecord.IsCorporate;
                    if (_isCorporate == true)
                        _practiceSiteId = (int)UserPractice.PracticeRecord.PracticeSiteId;

                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetPractices(int userId, string userType, int enterpriseId)
        {
            try
            {
                IQueryable practiceList = null;
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    practiceList = (from practiceUser in BMTDataContext.PracticeUsers
                                    join practiceRecord in BMTDataContext.Practices
                                        on practiceUser.PracticeId equals practiceRecord.PracticeId
                                    join userRecord in BMTDataContext.Users
                                        on practiceUser.UserId equals userRecord.UserId
                                    where userRecord.RoleId == 1 // Only User
                                    group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                                    orderby practiceGroup.Key.Name ascending
                                    select new { ID = practiceGroup.Key.PracticeId, Name = practiceGroup.Key.Name.ToString() }).AsQueryable();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    practiceList = (from practiceUser in BMTDataContext.PracticeUsers
                                    join practiceRecord in BMTDataContext.Practices
                                        on practiceUser.PracticeId equals practiceRecord.PracticeId
                                    join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                    join userRecord in BMTDataContext.Users
                                        on practiceUser.UserId equals userRecord.UserId
                                    where medicalGroupRecord.EnterpriseId == enterpriseId
                                    && userRecord.RoleId == 1 // Only User
                                    group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                                    orderby practiceGroup.Key.Name ascending
                                    select new { ID = practiceGroup.Key.PracticeId, Name = practiceGroup.Key.Name.ToString() }).AsQueryable();
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    practiceList = (from userRecord in BMTDataContext.Users
                                    join userRoleRecord in BMTDataContext.Roles
                                        on userRecord.RoleId equals userRoleRecord.RoleId
                                    join practiceUserRecord in BMTDataContext.PracticeUsers
                                        on userRecord.UserId equals practiceUserRecord.UserId
                                    join practiceRecord in BMTDataContext.Practices
                                        on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                    join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                    join ConsultingRecord in BMTDataContext.ConsultingUserAccesses
                                        on practiceRecord.PracticeId equals ConsultingRecord.PracticeId
                                         into ConsultingRecord_d
                                    from ConsultingRecord in ConsultingRecord_d.DefaultIfEmpty()
                                    where ConsultingRecord.UserId == userId
                                        && userRecord.RoleId == 1 // Only User
                                        && medicalGroupRecord.EnterpriseId == enterpriseId
                                    group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                                    orderby practiceGroup.Key.Name ascending
                                    select new { ID = practiceGroup.Key.PracticeId, Name = practiceGroup.Key.Name.ToString() }).AsQueryable();
                }

                return practiceList;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public IQueryable GetPracticesByEnterpriseId(int enterpriseId)
        {
            try
            {
                IQueryable practiceList = null;
                practiceList = (from practiceUser in BMTDataContext.PracticeUsers
                                join practiceRecord in BMTDataContext.Practices
                                    on practiceUser.PracticeId equals practiceRecord.PracticeId
                                join medicalGroupRecord in BMTDataContext.MedicalGroups
                                    on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                join userRecord in BMTDataContext.Users
                                    on practiceUser.UserId equals userRecord.UserId
                                where userRecord.RoleId == 1 // Only User
                                    && medicalGroupRecord.EnterpriseId == enterpriseId
                                group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                                orderby practiceGroup.Key.Name ascending
                                select new { ID = practiceGroup.Key.PracticeId, Name = practiceGroup.Key.Name.ToString() }).AsQueryable();

                return practiceList;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public IQueryable GetEnterprises()
        {
            IQueryable enterpriseList = null;
            try
            {
                enterpriseList = (from enterpriseRecord in BMTDataContext.Enterprises
                                  select new { ID = enterpriseRecord.EnterpriseId, Name = enterpriseRecord.Name.ToString() }).AsQueryable();
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return enterpriseList;
        }

        public List<PracticeDetails> GetPracticeDetails(int userId, string userType, int startRowIndex, int pageSize, string column, string orderBy, int enterpriseId)
        {
            try
            {
                List<PracticeDetails> practiceDetailList = null;
                PracticeDetails p = new PracticeDetails();

                practiceDetailList = BMTDataContext.usp_Get_Practice_Details(enterpriseId, userId, userType).ToList<PracticeDetails>();

                #region SuperUser
                //if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                //{
                //    //practiceDetailList = (from userRecord in BMTDataContext.Users
                //    //                      join practiceUser in BMTDataContext.PracticeUsers
                //    //                          on userRecord.UserId equals practiceUser.UserId

                //    //                      join practiceRecord in BMTDataContext.Practices
                //    //                          on practiceUser.PracticeId equals practiceRecord.PracticeId

                //    //                      join medicalGroupRecord in BMTDataContext.MedicalGroups
                //    //                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId

                //    //                      join practiceAddressRecord in BMTDataContext.Addresses
                //    //                          on practiceRecord.AddressId equals practiceAddressRecord.AddressId

                //    //                      join siteRecord in BMTDataContext.PracticeSites
                //    //                          on practiceRecord.PracticeId equals siteRecord.PracticeId

                //    //                      join filledAnsRecord in BMTDataContext.FilledAnswers
                //    //                        on siteRecord.PracticeSiteId equals filledAnsRecord.PracticeSiteId

                //    //                      //join projectUseRecord in BMT.ProjectUsages
                //    //                      //  on filledAnsRecord.ProjectUsageId equals projectUseRecord.ProjectUsageId

                //    //                      join siteAddressRecord in BMTDataContext.Addresses
                //    //                          on siteRecord.AddressId equals siteAddressRecord.AddressId
                //    //                      where userRecord.RoleId == 1
                //    //                      && medicalGroupRecord.EnterpriseId == enterpriseId

                //    //                      // group on following fields
                //    //                      group siteRecord by new
                //    //                      {
                //    //                          practiceRecord.PracticeId,

                //    //                          PracticeName = practiceRecord.Name,

                //    //                          SiteName = siteRecord.Name,

                //    //                          ProjectUsageId = (int)filledAnsRecord.ProjectUsageId,

                //    //                          PracticeSiteId = (int)filledAnsRecord.PracticeSiteId,

                //    //                          // if no contact name for site, get the contact name from practice
                //    //                          ContactName = siteRecord.ContactName.Trim() != string.Empty ? siteRecord.ContactName.Trim() :
                //    //                                        practiceRecord.ContactName.Trim(),

                //    //                          // if no contact phone for site, get the contact phone from practice
                //    //                          ContactPhone = siteAddressRecord.Telephone.Trim() != string.Empty ? siteAddressRecord.Telephone.Trim() :
                //    //                                        practiceAddressRecord.Telephone.Trim(),
                //    //                          EmailText = siteAddressRecord.Email.Trim() != string.Empty ? siteAddressRecord.Email.Trim() :
                //    //                                        practiceAddressRecord.Email.Trim()

                //    //                      } into groupby

                //    //                      orderby groupby.Key.PracticeName ascending
                //    //                      select new PracticeDetails
                //    //                      {
                //    //                          SecurePracticeId = groupby.Key.PracticeId.ToString(),

                //    //                          PracticeName = groupby.Key.PracticeName.ToString(),

                //    //                          ProjectUsageId = groupby.Key.ProjectUsageId,

                //    //                          PracticeSiteId = groupby.Key.PracticeSiteId,

                //    //                          SiteName = groupby.Key.SiteName.ToString(),

                //    //                          Points =  GetPointsByprojectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId).ToString(),

                //    //                          Documents = GetDocumentsByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId).ToString(),

                //    //                          LastActivity = GetPracticeLastActivityDate(groupby.Key.PracticeSiteId),

                //    //                          ContactName = groupby.Key.ContactName,

                //    //                          ContactPhone = groupby.Key.ContactPhone,

                //    //                          EmailText = groupby.Key.EmailText,

                //    //                          DateForeColor = GetDateColor(groupby.Key.PracticeId)

                //    //                      }).ToList();
                //}

                #endregion

                #region CONSULTANT
                //else if (userType == enUserRole.Consultant.ToString())
                //{
                //    practiceDetailList = (from userRecord in BMTDataContext.Users
                //                          join userRoleRecord in BMTDataContext.Roles
                //                              on userRecord.RoleId equals userRoleRecord.RoleId

                //                          join practiceUserRecord in BMTDataContext.PracticeUsers
                //                              on userRecord.UserId equals practiceUserRecord.UserId

                //                          join practiceRecord in BMTDataContext.Practices
                //                           on practiceUserRecord.PracticeId equals practiceRecord.PracticeId

                //                          join practiceAddressRecord in BMTDataContext.Addresses
                //                            on practiceRecord.AddressId equals practiceAddressRecord.AddressId

                //                          join siteRecord in BMTDataContext.PracticeSites
                //                            on practiceRecord.PracticeId equals siteRecord.PracticeId

                //                          join filledAnsRecord in BMTDataContext.FilledAnswers
                //                            on siteRecord.PracticeSiteId equals filledAnsRecord.PracticeSiteId

                //                          //join projectUseRecord in BMTDataContext.ProjectUsages
                //                          //  on filledAnsRecord.ProjectUsageId equals projectUseRecord.ProjectUsageId

                //                          join siteAddressRecord in BMTDataContext.Addresses
                //                            on siteRecord.AddressId equals siteAddressRecord.AddressId

                //                          join ConsultingRecord in BMTDataContext.ConsultingUserAccesses
                //                              on practiceRecord.PracticeId equals ConsultingRecord.PracticeId
                //                               into ConsultingRecord_d

                //                          from ConsultingRecord in ConsultingRecord_d.DefaultIfEmpty()

                //                          where ConsultingRecord.UserId == userId
                //                              && userRoleRecord.Name == enUserRole.User.ToString()

                //                          // group on following fields
                //                          group siteRecord by new
                //                          {
                //                              practiceRecord.PracticeId,

                //                              PracticeName = practiceRecord.Name,

                //                              SiteName = siteRecord.Name,

                //                              ProjectUsageId = (int)filledAnsRecord.ProjectUsageId,

                //                              PracticeSiteId = (int)filledAnsRecord.PracticeSiteId,

                //                              // if no contact name for site, get the contact name from practice
                //                              ContactName = siteRecord.ContactName.Trim() != string.Empty ? siteRecord.ContactName.Trim() :
                //                                            practiceRecord.ContactName.Trim(),

                //                              // if no contact phone for site, get the contact phone from practice
                //                              ContactPhone = siteAddressRecord.Telephone.Trim() != string.Empty ? siteAddressRecord.Telephone.Trim() :
                //                                            practiceAddressRecord.Telephone.Trim(),
                //                              EmailText = siteAddressRecord.Email.Trim() != string.Empty ? siteAddressRecord.Email.Trim() :
                //                                            practiceAddressRecord.Email.Trim()

                //                          } into groupby

                //                          orderby groupby.Key.PracticeName ascending
                //                          select new PracticeDetails
                //                          {
                //                              SecurePracticeId = groupby.Key.PracticeId.ToString(),

                //                              PracticeName = groupby.Key.PracticeName.ToString(),

                //                              ProjectUsageId = groupby.Key.ProjectUsageId,

                //                              PracticeSiteId = groupby.Key.PracticeSiteId,

                //                              SiteName = groupby.Key.SiteName.ToString(),

                //                              Points = GetPointsByprojectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId).ToString(),

                //                              Documents = GetDocumentsByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId).ToString(),

                //                              LastActivity = GetPracticeLastActivityDate(groupby.Key.PracticeSiteId),

                //                              ContactName = groupby.Key.ContactName,

                //                              ContactPhone = groupby.Key.ContactPhone,

                //                              EmailText = groupby.Key.EmailText,

                //                              DateForeColor = GetDateColor(groupby.Key.PracticeId)

                //                          }).ToList();
                //}
                #endregion

                practiceDetailList = SkipExtraPages(practiceDetailList, startRowIndex, pageSize, column, orderBy);
                return practiceDetailList;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<SRAdashboard> GetSRAData(int userId, string userType, int startRowIndex, int pageSize, string column, string orderBy, int enterpriseId)
        {
            try
            {
                List<SRAdashboard> practiceDetailList = null;
                BMTDataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, BMTDataContext.FilledQuestionnaires);

                #region SuperUser
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    practiceDetailList = (from userRecord in BMTDataContext.Users
                                          join practiceUser in BMTDataContext.PracticeUsers
                                            on userRecord.UserId equals practiceUser.UserId

                                          join practiceRecord in BMTDataContext.Practices
                                            on practiceUser.PracticeId equals practiceRecord.PracticeId

                                          join medicalGroupRecord in BMTDataContext.MedicalGroups
                                          on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId

                                          join practiceAddressRecord in BMTDataContext.Addresses
                                            on practiceRecord.AddressId equals practiceAddressRecord.AddressId

                                          join siteRecord in BMTDataContext.PracticeSites
                                            on practiceRecord.PracticeId equals siteRecord.PracticeId

                                          join filledQuesRecord in BMTDataContext.FilledQuestionnaires
                                            on siteRecord.PracticeSiteId equals filledQuesRecord.PracticeSiteId

                                          //join projectUseRecord in BMTDataContext.ProjectUsages
                                          //  on filledQuesRecord.ProjectUsageId equals projectUseRecord.ProjectUsageId

                                          join siteAddressRecord in BMTDataContext.Addresses
                                            on siteRecord.AddressId equals siteAddressRecord.AddressId

                                          where userRecord.RoleId == 1
                                          && medicalGroupRecord.EnterpriseId == enterpriseId
                                          && filledQuesRecord.FormId == 6   //SRA

                                          // group on following fields
                                          group siteRecord by new
                                          {
                                              practiceRecord.PracticeId,

                                              PracticeName = practiceRecord.Name,

                                              ProjectUsageId = filledQuesRecord.ProjectUsageId,

                                              PracticeSiteId = (int)filledQuesRecord.PracticeSiteId,

                                              SiteName = siteRecord.Name,

                                              // if no contact name for site, get the contact name from practice
                                              ContactName = siteRecord.ContactName.Trim() != string.Empty ? siteRecord.ContactName.Trim() :
                                                            practiceRecord.ContactName.Trim(),

                                              // if no contact phone for site, get the contact phone from practice
                                              ContactPhone = siteAddressRecord.Telephone.Trim() != string.Empty ? siteAddressRecord.Telephone.Trim() :
                                                            practiceAddressRecord.Telephone.Trim(),
                                              EmailText = siteAddressRecord.Email.Trim() != string.Empty ? siteAddressRecord.Email.Trim() :
                                                            practiceAddressRecord.Email.Trim()

                                          } into groupby

                                          orderby groupby.Key.PracticeName ascending
                                          select new SRAdashboard
                                          {
                                              SecurePracticeId = groupby.Key.PracticeId.ToString(),

                                              PracticeName = groupby.Key.PracticeName.ToString(),

                                              SiteName = groupby.Key.SiteName.ToString(),

                                              ProjectUsageId = groupby.Key.ProjectUsageId,

                                              PracticeSiteId = groupby.Key.PracticeSiteId,

                                              FindingsFinalized = GetFindingsFinalizedByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId),

                                              FollowupFinalized = GetFollowupFinalizedByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId),

                                              LastActivity = GetPracticeLastActivityDate(groupby.Key.PracticeSiteId),

                                              ContactName = groupby.Key.ContactName,

                                              ContactPhone = groupby.Key.ContactPhone,

                                              EmailText = groupby.Key.EmailText,

                                              DateForeColor = GetDateColor(groupby.Key.PracticeId)

                                          }).ToList();
                }

                #endregion

                #region CONSULTANT
                else if (userType == enUserRole.Consultant.ToString())
                {
                    practiceDetailList = (from userRecord in BMTDataContext.Users
                                          join userRoleRecord in BMTDataContext.Roles
                                              on userRecord.RoleId equals userRoleRecord.RoleId

                                          join practiceUserRecord in BMTDataContext.PracticeUsers
                                              on userRecord.UserId equals practiceUserRecord.UserId

                                          join practiceRecord in BMTDataContext.Practices
                                           on practiceUserRecord.PracticeId equals practiceRecord.PracticeId

                                          join practiceAddressRecord in BMTDataContext.Addresses
                                            on practiceRecord.AddressId equals practiceAddressRecord.AddressId

                                          join siteRecord in BMTDataContext.PracticeSites
                                            on practiceRecord.PracticeId equals siteRecord.PracticeId

                                          join filledQuesRecord in BMTDataContext.FilledQuestionnaires
                                            on siteRecord.PracticeSiteId equals filledQuesRecord.PracticeSiteId

                                          //join projectUseRecord in BMTDataContext.ProjectUsages
                                          //  on filledQuesRecord.ProjectUsageId equals projectUseRecord.ProjectUsageId

                                          join siteAddressRecord in BMTDataContext.Addresses
                                            on siteRecord.AddressId equals siteAddressRecord.AddressId

                                          join ConsultingRecord in BMTDataContext.ConsultingUserAccesses
                                              on practiceRecord.PracticeId equals ConsultingRecord.PracticeId
                                               into ConsultingRecord_d

                                          from ConsultingRecord in ConsultingRecord_d.DefaultIfEmpty()

                                          where ConsultingRecord.UserId == userId
                                              && userRoleRecord.Name == enUserRole.User.ToString()

                                          // group on following fields
                                          group siteRecord by new
                                          {
                                              practiceRecord.PracticeId,

                                              PracticeName = practiceRecord.Name,

                                              ProjectUsageId = filledQuesRecord.ProjectUsageId,

                                              PracticeSiteId = (int)filledQuesRecord.PracticeSiteId,

                                              SiteName = siteRecord.Name,

                                              // if no contact name for site, get the contact name from practice
                                              ContactName = siteRecord.ContactName.Trim() != string.Empty ? siteRecord.ContactName.Trim() :
                                                            practiceRecord.ContactName.Trim(),

                                              // if no contact phone for site, get the contact phone from practice
                                              ContactPhone = siteAddressRecord.Telephone.Trim() != string.Empty ? siteAddressRecord.Telephone.Trim() :
                                                            practiceAddressRecord.Telephone.Trim(),
                                              EmailText = siteAddressRecord.Email.Trim() != string.Empty ? siteAddressRecord.Email.Trim() :
                                                            practiceAddressRecord.Email.Trim()

                                          } into groupby

                                          orderby groupby.Key.PracticeName ascending
                                          select new SRAdashboard
                                          {
                                              SecurePracticeId = groupby.Key.PracticeId.ToString(),

                                              PracticeName = groupby.Key.PracticeName.ToString(),

                                              SiteName = groupby.Key.SiteName.ToString(),

                                              ProjectUsageId = groupby.Key.ProjectUsageId,

                                              PracticeSiteId = groupby.Key.PracticeSiteId,

                                              FindingsFinalized = GetFindingsFinalizedByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId),

                                              FollowupFinalized = GetFollowupFinalizedByProjectId(groupby.Key.ProjectUsageId, groupby.Key.PracticeSiteId),

                                              LastActivity = GetPracticeLastActivityDate(groupby.Key.PracticeSiteId),

                                              ContactName = groupby.Key.ContactName,

                                              ContactPhone = groupby.Key.ContactPhone,

                                              EmailText = groupby.Key.EmailText,

                                              DateForeColor = GetDateColor(groupby.Key.PracticeId)

                                          }).ToList();
                }
                #endregion

                practiceDetailList = SkipExtraPagesfForSRA(practiceDetailList, startRowIndex, pageSize, column, orderBy);
                return practiceDetailList;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public void SaveHomeDashboard(int userId, int Dashboard)
        {
            try
            {
                User users = (from usr in BMTDataContext.Users
                              where usr.UserId == userId
                              select usr).SingleOrDefault();

                users.HomeDashboard = Dashboard;

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public void SaveDashboard(int Dashboard)
        {
            try
            {
                User users = (from usr in BMTDataContext.Users
                              where usr.Username == "SuperAdmin"
                              select usr).FirstOrDefault();

                users.HomeDashboard = Dashboard;

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public int GetHomeDashboard(int userId)
        {
            try
            {
                int? homeDashboard = (from usr in BMTDataContext.Users
                                      where usr.UserId == userId
                                      select usr.HomeDashboard).FirstOrDefault();

                int homeDashboardId = homeDashboard == null ? 0 : Convert.ToInt32(homeDashboard.ToString());
                return homeDashboardId;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private List<PracticeDetails> SkipExtraPages(List<PracticeDetails> practiceDetailsList, int startRowIndex, int pageSize, string columnName, string orderBy)
        {
            // apply order by condition on list
            // By Practice Name Asc/Desc
            if (columnName == enDashBoardColumns.PracticeName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.PracticeName).ToList();
            else if (columnName == enDashBoardColumns.PracticeName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.PracticeName).ToList();

            // By Site Name Asc/Desc
            else if (columnName == enDashBoardColumns.SiteName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.SiteName).ToList();
            else if (columnName == enDashBoardColumns.SiteName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.SiteName).ToList();

            // By Points Asc/Desc
            else if (columnName == enDashBoardColumns.Points.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => Convert.ToDecimal(columnIndex.Points)).ToList();
            else if (columnName == enDashBoardColumns.Points.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => Convert.ToDecimal(columnIndex.Points)).ToList();

            // By Documents Asc/Desc
            else if (columnName == enDashBoardColumns.Documents.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => Convert.ToDecimal(columnIndex.Documents)).ToList();
            else if (columnName == enDashBoardColumns.Documents.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => Convert.ToDecimal(columnIndex.Documents)).ToList();

            // By Last Activity Asc/Desc
            else if (columnName == enDashBoardColumns.LastActivity.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.LastActivity).ToList();
            else if (columnName == enDashBoardColumns.LastActivity.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.LastActivity).ToList();

            // By Contact Name
            else if (columnName == enDashBoardColumns.ContactName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.ContactName).ToList();
            else if (columnName == enDashBoardColumns.ContactName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.ContactName).ToList();

            // skip extra page after applying order
            return practiceDetailsList.Skip(startRowIndex).Take(pageSize).ToList();
        }


        private List<SRAdashboard> SkipExtraPagesfForSRA(List<SRAdashboard> practiceDetailsList, int startRowIndex, int pageSize, string columnName, string orderBy)
        {
            // apply order by condition on list
            // By Practice Name Asc/Desc
            if (columnName == enDashBoardColumns.PracticeName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.PracticeName).ToList();
            else if (columnName == enDashBoardColumns.PracticeName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.PracticeName).ToList();

            // By Site Name Asc/Desc
            else if (columnName == enDashBoardColumns.SiteName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.SiteName).ToList();
            else if (columnName == enDashBoardColumns.SiteName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.SiteName).ToList();

            // By Points Asc/Desc
            else if (columnName == enDashBoardColumns.FindingsFinalized.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.FindingsFinalized).ToList();
            else if (columnName == enDashBoardColumns.FindingsFinalized.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.FindingsFinalized).ToList();

            // By Documents Asc/Desc
            else if (columnName == enDashBoardColumns.FollowupFinalized.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.FollowupFinalized).ToList();
            else if (columnName == enDashBoardColumns.FollowupFinalized.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.FollowupFinalized).ToList();

            // By Last Activity Asc/Desc
            else if (columnName == enDashBoardColumns.LastActivity.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.LastActivity).ToList();
            else if (columnName == enDashBoardColumns.LastActivity.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.LastActivity).ToList();

            // By Contact Name
            else if (columnName == enDashBoardColumns.ContactName.ToString() && orderBy == enSortingType.Ascending.ToString())
                practiceDetailsList = practiceDetailsList.OrderBy(columnIndex => columnIndex.ContactName).ToList();
            else if (columnName == enDashBoardColumns.ContactName.ToString() && orderBy == enSortingType.Descending.ToString())
                practiceDetailsList = practiceDetailsList.OrderByDescending(columnIndex => columnIndex.ContactName).ToList();

            // skip extra page after applying order
            return practiceDetailsList.Skip(startRowIndex).Take(pageSize).ToList();
        }

        private decimal GetPointsByprojectId(int projectUsageId, int siteId)
        {
            List<NCQASummaryDetail> ncqaquestionnaire = (from filledAnsRecord in BMTDataContext.FilledAnswers
                                                         join kbtRecord in BMTDataContext.KnowledgeBaseTemplates
                                                         on filledAnsRecord.KnowledgeBaseTemplateId equals kbtRecord.KnowledgeBaseTemplateId
                                                         join kbRecord in BMTDataContext.KnowledgeBases
                                                         on kbtRecord.KnowledgeBaseId equals kbRecord.KnowledgeBaseId
                                                         where kbtRecord.TemplateId == 1 //NCQA
                                                         && filledAnsRecord.ProjectUsageId == projectUsageId
                                                         && filledAnsRecord.PracticeSiteId == siteId
                                                         && kbRecord.KnowledgeBaseTypeId == 2 //Sub-Header (Element Nodes)
                                                         select new NCQASummaryDetail
                                                         {
                                                             MaxPoints = (int)kbtRecord.MaxPoints == null ? 0 : (int)kbtRecord.MaxPoints,
                                                             EarnedPoints = filledAnsRecord.DefaultScore == null ? 0 : Convert.ToInt32(filledAnsRecord.DefaultScore.Replace("%", ""))

                                                         }).ToList();

            if (ncqaquestionnaire != null)

                return Math.Round(NCQADataHelper.GetPoints(ncqaquestionnaire), 2);
            else
                return 0M;
        }

        private decimal GetDocumentsByProjectId(int projectUsageId, int siteId)
        {
            List<NCQAFullDetail> ncqaDocs = (from filledAnsRecord in BMTDataContext.FilledAnswers
                                             join kbtRecord in BMTDataContext.KnowledgeBaseTemplates
                                             on filledAnsRecord.KnowledgeBaseTemplateId equals kbtRecord.KnowledgeBaseTemplateId
                                             join kbRecord in BMTDataContext.KnowledgeBases
                                             on kbtRecord.KnowledgeBaseId equals kbRecord.KnowledgeBaseId
                                             where kbtRecord.TemplateId == 1 //NCQA
                                             && filledAnsRecord.ProjectUsageId == projectUsageId
                                             && filledAnsRecord.PracticeSiteId == siteId
                                             && kbRecord.KnowledgeBaseTypeId == 3 //Questions (Factor Nodes)
                                             select new NCQAFullDetail
                                             {
                                                 FactorNumber = filledAnsRecord.FilledAnswersId.ToString(), //To get uploaded Documents against each AnswerId
                                                 Policies = filledAnsRecord.PoliciesDocumentCount == null ? "0" : filledAnsRecord.PoliciesDocumentCount.ToString(),
                                                 Report = filledAnsRecord.ReportsDocumentCount == null ? "0" : filledAnsRecord.ReportsDocumentCount.ToString(),
                                                 ScreenShot = filledAnsRecord.ScreenShotsDocumentCount == null ? "0" : filledAnsRecord.ScreenShotsDocumentCount.ToString(),
                                                 RRWB = filledAnsRecord.LogsOrToolsDocumentCount == null ? "0" : filledAnsRecord.LogsOrToolsDocumentCount.ToString(),
                                                 Others = filledAnsRecord.OtherDocumentsCount == null ? "0" : filledAnsRecord.OtherDocumentsCount.ToString()


                                             }).ToList();

            int ncqaUploadedDocs = GetNcqaUploadedDocuments(ncqaDocs);

            if (ncqaDocs != null && ncqaUploadedDocs != 0)
                return Math.Round(NCQADataHelper.GetDocuments(ncqaDocs, ncqaUploadedDocs), 2);
            else
                return 0M;
        }

        private int GetNcqaUploadedDocuments(List<NCQAFullDetail> document)
        {
            int ncqaUploadedDocs = 0;

            foreach (NCQAFullDetail factor in document)
            {
                ncqaUploadedDocs = ncqaUploadedDocs + (from filledTempDocRecord in BMTDataContext.FilledTemplateDocuments
                                                       //join filledAnswerRecord in BMTDataContext.FilledAnswers
                                                       //on filledTempDocRecord.FilledAnswerId equals filledAnswerRecord.FilledAnswersId
                                                       where filledTempDocRecord.FilledAnswerId == Convert.ToInt32(factor.FactorNumber)
                                                       select filledTempDocRecord.FilledDocumentId).Count();
            }

            return ncqaUploadedDocs;
        }

        private DateTime? GetPracticeLastActivityDate(int siteId)
        {
            DateTime? lastActivityDate = null;
            int userId = (from siteRecord in BMTDataContext.PracticeSites
                          join practiceUserRecord in BMTDataContext.PracticeUsers
                          on siteRecord.PracticeSiteId equals practiceUserRecord.PracticeSiteId
                          where siteRecord.PracticeSiteId == siteId
                          select practiceUserRecord.UserId).FirstOrDefault();

            //int userId = (from ProjectRecord in BMTDataContext.Projects
            //              join PracticeUserRecord in BMTDataContext.PracticeUsers
            //                 on ProjectRecord.PracticeSiteId equals PracticeUserRecord.PracticeSiteId
            //              where ProjectRecord.ProjectId == projectId
            //              select PracticeUserRecord.UserId).FirstOrDefault();

            if (userId != 0)
            {
                var loginTrack = (from loginRecord in BMTDataContext.LoginRecords
                                  where loginRecord.UserId == userId
                                  select loginRecord.LoginTime).Max();


                if (loginTrack != null)
                {
                    lastActivityDate = loginTrack;
                }
                else
                {
                    var user = (from userRecord in BMTDataContext.Users
                                join practiceUserRecord in BMTDataContext.PracticeUsers
                                    on userRecord.UserId equals practiceUserRecord.UserId
                                where userRecord.UserId == userId
                                select userRecord).First();

                    lastActivityDate = user.LastActivitydate == null ? user.CreatedDate : user.LastActivitydate;
                }
            }
            return lastActivityDate;
        }

        private string GetDateColor(int practiceId)
        {
            DateTime? lastActivityDate = (from userRecord in BMTDataContext.Users
                                          join practiceUserRecord in BMTDataContext.PracticeUsers
                                              on userRecord.UserId equals practiceUserRecord.UserId
                                          where practiceUserRecord.PracticeId == practiceId
                                          select userRecord.LastActivitydate).Max();
            if (lastActivityDate == null)
                return DEFAULT_DATE_CHANGE_COLOR;
            else
                return string.Empty;

        }

        public int CountPractices(int userId, string userType, int enterpriseId)
        {
            try
            {
                int totalPractices = 0;
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    totalPractices = (from userRecord in BMTDataContext.Users
                                      join practiceUser in BMTDataContext.PracticeUsers
                                        on userRecord.UserId equals practiceUser.UserId
                                      join practiceRecord in BMTDataContext.Practices
                                          on practiceUser.PracticeId equals practiceRecord.PracticeId
                                      join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                      join siteRecord in BMTDataContext.PracticeSites
                                          on practiceUser.PracticeId equals siteRecord.PracticeId
                                      join filledAnsRecord in BMTDataContext.FilledAnswers
                                          on siteRecord.PracticeSiteId equals filledAnsRecord.PracticeSiteId

                                      where userRecord.RoleId == (int)enUserRole.User
                                          && medicalGroupRecord.EnterpriseId == enterpriseId

                                      // group on following fields
                                      group siteRecord by new { filledAnsRecord.ProjectUsageId, filledAnsRecord.PracticeSiteId } into siteRecord_group

                                      select new { siteRecord_group.Key.PracticeSiteId }).Count();
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    totalPractices = (from userRecord in BMTDataContext.Users
                                      join userRoleRecord in BMTDataContext.Roles
                                          on userRecord.RoleId equals userRoleRecord.RoleId

                                      join practiceUserRecord in BMTDataContext.PracticeUsers
                                          on userRecord.UserId equals practiceUserRecord.UserId

                                      join practiceRecord in BMTDataContext.Practices
                                          on practiceUserRecord.PracticeId equals practiceRecord.PracticeId

                                      join siteRecord in BMTDataContext.PracticeSites
                                          on practiceRecord.PracticeId equals siteRecord.PracticeId

                                      join filledAnsRecord in BMTDataContext.FilledAnswers
                                          on siteRecord.PracticeSiteId equals filledAnsRecord.PracticeSiteId

                                      join ConsultingRecord in BMTDataContext.ConsultingUserAccesses
                                          on practiceRecord.PracticeId equals ConsultingRecord.PracticeId
                                           into ConsultingRecord_d

                                      from ConsultingRecord in ConsultingRecord_d.DefaultIfEmpty()
                                      where ConsultingRecord.UserId == userId
                                          && userRoleRecord.Name == enUserRole.User.ToString()

                                      // group on following fields
                                      group siteRecord by new { filledAnsRecord.ProjectUsageId, filledAnsRecord.PracticeSiteId } into siteRecord_group
                                      select new { siteRecord_group.Key.PracticeSiteId }).Count();
                }
                return totalPractices;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int CountSRAPractices(int userId, string userType, int enterpriseId)
        {
            try
            {
                int totalPractices = 0;
                if (userType == enUserRole.SuperUser.ToString() || userType == enUserRole.SuperAdmin.ToString())
                {
                    totalPractices = (from userRecord in BMTDataContext.Users
                                      join practiceUser in BMTDataContext.PracticeUsers
                                        on userRecord.UserId equals practiceUser.UserId
                                      join practiceRecord in BMTDataContext.Practices
                                          on practiceUser.PracticeId equals practiceRecord.PracticeId
                                      join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                      join siteRecord in BMTDataContext.PracticeSites
                                          on practiceUser.PracticeId equals siteRecord.PracticeId
                                      join filledQuesRecord in BMTDataContext.FilledQuestionnaires
                                            on siteRecord.PracticeSiteId equals filledQuesRecord.PracticeSiteId

                                      where userRecord.RoleId == (int)enUserRole.User
                                          && medicalGroupRecord.EnterpriseId == enterpriseId
                                          && filledQuesRecord.FormId == (int)enQuestionnaireType.SRAQuestionnaire

                                      // group on following fields
                                      group siteRecord by new { filledQuesRecord.ProjectUsageId, filledQuesRecord.PracticeSiteId } into siteRecord_group

                                      select new { siteRecord_group.Key.PracticeSiteId }).Count();
                }
                else if (userType == enUserRole.Consultant.ToString())
                {
                    totalPractices = (from userRecord in BMTDataContext.Users
                                      join userRoleRecord in BMTDataContext.Roles
                                          on userRecord.RoleId equals userRoleRecord.RoleId

                                      join practiceUserRecord in BMTDataContext.PracticeUsers
                                          on userRecord.UserId equals practiceUserRecord.UserId

                                      join practiceRecord in BMTDataContext.Practices
                                          on practiceUserRecord.PracticeId equals practiceRecord.PracticeId

                                      join siteRecord in BMTDataContext.PracticeSites
                                          on practiceRecord.PracticeId equals siteRecord.PracticeId

                                      join filledQuesRecord in BMTDataContext.FilledQuestionnaires
                                            on siteRecord.PracticeSiteId equals filledQuesRecord.PracticeSiteId

                                      join ConsultingRecord in BMTDataContext.ConsultingUserAccesses
                                          on practiceRecord.PracticeId equals ConsultingRecord.PracticeId
                                           into ConsultingRecord_d

                                      from ConsultingRecord in ConsultingRecord_d.DefaultIfEmpty()
                                      where ConsultingRecord.UserId == userId
                                          && userRoleRecord.Name == enUserRole.User.ToString()
                                          && filledQuesRecord.FormId == (int)enQuestionnaireType.SRAQuestionnaire

                                      // group on following fields
                                      group siteRecord by new { filledQuesRecord.ProjectUsageId, filledQuesRecord.PracticeSiteId } into siteRecord_group
                                      select new { siteRecord_group.Key.PracticeSiteId }).Count();
                }
                return totalPractices;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetPracticeNameByPracticeId(int PracticeId)
        {

            _PracticeId = PracticeId;
            try
            {
                var userPracticeName = (from PracticeRecord in BMTDataContext.Practices
                                        where (PracticeRecord.PracticeId == this.PracticeId)
                                        select new { PracticeRecord }).SingleOrDefault();

                if (userPracticeName != null)
                    _Name = userPracticeName.PracticeRecord.Name;

                return _Name;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public int GetMedicalGroupIdByPracticeId(int practiceId)
        {
            try
            {
                int medicalGroupId = (from PracticeRecord in BMTDataContext.Practices
                                      where (PracticeRecord.PracticeId == practiceId)
                                      select PracticeRecord.MedicalGroupId).SingleOrDefault();



                return medicalGroupId;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public string GetFindingsFinalizedByProjectId(int _projectUsageId, int siteId)
        {

            string findingsFinalized = string.Empty;

            var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                       where QuestionnaireRecord.ProjectUsageId == _projectUsageId
                                       && QuestionnaireRecord.PracticeSiteId == siteId
                                       && QuestionnaireRecord.FormId == (int)enQuestionnaireType.SRAQuestionnaire
                                       select QuestionnaireRecord).SingleOrDefault();

            if (filledQuestionnaire != null)
            {
                var sraAnswer = XDocument.Parse(Convert.ToString(filledQuestionnaire.Answers));
                bool isFindingsFinalized = Convert.ToBoolean(sraAnswer.Root.Elements("Findings").Attributes().Count() > 0 ? sraAnswer.Root.Element("Findings").Attribute("Finalize").Value : "false");
                findingsFinalized = isFindingsFinalized ? "Yes" : "No";
            }
            else
            {
                findingsFinalized = "No";
            }

            return findingsFinalized;
        }

        public string GetFollowupFinalizedByProjectId(int _projectUsageId, int siteId)
        {

            string followupFinalized = string.Empty;

            var filledQuestionnaire = (from QuestionnaireRecord in BMTDataContext.FilledQuestionnaires
                                       where QuestionnaireRecord.ProjectUsageId == _projectUsageId
                                       && QuestionnaireRecord.PracticeSiteId == siteId
                                       && QuestionnaireRecord.FormId == (int)enQuestionnaireType.SRAQuestionnaire
                                       select QuestionnaireRecord).SingleOrDefault();

            if (filledQuestionnaire != null)
            {
                var sraAnswer = XDocument.Parse(Convert.ToString(filledQuestionnaire.Answers));
                bool isFollowupFinalized = Convert.ToBoolean(sraAnswer.Root.Elements("Followup").Attributes().Count() > 0 ? sraAnswer.Root.Element("Followup").Attribute("Finalize").Value : "false");
                followupFinalized = isFollowupFinalized ? "Yes" : "No";
            }
            else
            {
                followupFinalized = "No";
            }

            return followupFinalized;
        }

        public void UpdateUserPrefer(string columnName, string sortDirection, string tableType, int userID)
        {
            try
            {
                XElement perference = new XElement("Perference");
                XElement dashboard = new XElement("Dashboard");
                XElement favouriteSort = new XElement("FavouriteSort");
                XElement favColumn = new XElement("Column", columnName);
                XElement direction = new XElement("Direction", sortDirection);

                favouriteSort.SetAttributeValue("Type", tableType);
                favouriteSort.Add(favColumn);
                favouriteSort.Add(direction);
                dashboard.Add(favouriteSort);
                perference.Add(dashboard);


                var user = BMTDataContext.Users.FirstOrDefault(x => x.UserId == userID);
                XDocument xDoc = null;
                if (user.Preferences != null)
                    xDoc = XDocument.Parse((from per in BMTDataContext.Users
                                            where per.UserId == userID
                                            select per.Preferences).FirstOrDefault().ToString());

                if (xDoc != null)
                {
                    XElement xDashboard = xDoc.XPathSelectElement("Perference/Dashboard");
                    if (xDashboard != null)
                    {
                        string xPath = "Perference/Dashboard/FavouriteSort[@Type='" + tableType + "']";
                        XElement xFavSort = xDoc.XPathSelectElement(xPath);
                        if (xFavSort != null)
                        {
                            xFavSort.Element("Column").SetValue(columnName);
                            xFavSort.Element("Direction").SetValue(sortDirection);
                            user.Preferences = xDoc.Root;
                        }
                        else
                        {
                            xDashboard.Add(favouriteSort);
                            user.Preferences = xDoc.Root;
                        }
                    }
                    else
                    {
                        xDoc.Root.Add(dashboard);
                        user.Preferences = xDoc.Root;

                    }
                }
                else
                {
                    user.Preferences = perference;
                }

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public XElement GetUserXml(int userID, string tableType)
        {
            try
            {
                XElement xFavSort = null;
                var user = BMTDataContext.Users.FirstOrDefault(x => x.UserId == userID);
                XDocument xDoc = null;
                if (user.Preferences != null)
                    xDoc = XDocument.Parse(user.Preferences.ToString());
                if (xDoc != null)
                {
                    string xPath = "Perference/Dashboard/FavouriteSort[@Type='" + tableType + "']";
                    xFavSort = xDoc.XPathSelectElement(xPath);
                }
                return xFavSort;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckCorporateType(int siteId)
        {

            try
            {
                List<Practice> corporatePractice = (from practiceRecords in BMTDataContext.Practices
                                                    where practiceRecords.PracticeSiteId == siteId
                                                    && practiceRecords.IsCorporate == true
                                                    select practiceRecords).ToList();
                if (corporatePractice.Count != 0)
                    return true;

                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CheckForCorporateSite(int SiteId)
        {
            try
            {
                string Questionaire = GetXmlBySiteId(SiteId);

                List<string> CorpElementList = (from CorpElement in BMTDataContext.CorporateElementLists
                                                select CorpElement.ElementName).ToList<string>();

                if (Questionaire != "")
                {
                    XDocument filledQuestionaire = XDocument.Parse(Questionaire);

                    foreach (XElement standard in filledQuestionaire.Root.Elements("Standard"))
                    {
                        foreach (XElement element in standard.Elements("Element"))
                        {

                            string standardName = standard.Attribute("title").Value.Substring(0, 6);
                            string elementName = element.Attribute("title").Value.Substring(7, 2);
                            string CorpElementName = standardName + elementName;

                            if (!CorpElementList.Contains(CorpElementName))
                            {
                                string complete = element.Attribute("complete").Value;
                                if (complete == "Yes")
                                {
                                    return false;
                                }
                                foreach (XElement factor in element.Elements("Factor"))
                                {
                                    string factAns = factor.Attribute("answer").Value;
                                    if (factAns == "Yes" || factAns == "NA")
                                    {
                                        return false;
                                    }
                                    foreach (XElement policies in factor.Elements("Policies"))
                                    {
                                        string policy = policies.Attribute("required").Value;
                                        if (policy != "")
                                            return false;
                                    }
                                    foreach (XElement reports in factor.Elements("Reports"))
                                    {
                                        string report = reports.Attribute("required").Value;
                                        if (report != "")
                                            return false;
                                    }
                                    foreach (XElement screenshots in factor.Elements("Screenshots"))
                                    {
                                        string screenshot = screenshots.Attribute("required").Value;
                                        if (screenshot != "")
                                            return false;
                                    }
                                    foreach (XElement logs in factor.Elements("LogsOrTools"))
                                    {
                                        string log = logs.Attribute("required").Value;
                                        if (log != "")
                                            return false;
                                    }
                                    foreach (XElement others in factor.Elements("OtherDocs"))
                                    {
                                        string other = others.Attribute("required").Value;
                                        if (other != "")
                                            return false;
                                    }
                                    foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                    {
                                        string privNote = privNotes.Value;
                                        if (privNote != "")
                                            return false;
                                    }
                                }
                                foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                {
                                    string revNote = revNotes.Value;
                                    if (revNote != "")
                                        return false;
                                }
                                foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                {
                                    string evalNote = evalNotes.Value;
                                    if (evalNote != "")
                                        return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public bool CheckForRemoveCorporateSite(int SiteId)
        {
            try
            {
                int pracId = (from pracRec in BMTDataContext.PracticeSites
                              where pracRec.PracticeSiteId == SiteId
                              select pracRec.PracticeId).FirstOrDefault();

                int pracSiteId = Convert.ToInt32((from prac in BMTDataContext.Practices
                                                  where prac.PracticeId == pracId
                                                  select prac.PracticeSiteId).FirstOrDefault());

                string Questionaire = GetXmlBySiteId(pracSiteId);

                List<string> CorpElementList = (from CorpElement in BMTDataContext.CorporateElementLists
                                                select CorpElement.ElementName).ToList<string>();

                if (Questionaire != "")
                {
                    XDocument filledQuestionaire = XDocument.Parse(Questionaire);

                    foreach (XElement standard in filledQuestionaire.Root.Elements("Standard"))
                    {
                        foreach (XElement element in standard.Elements("Element"))
                        {

                            string standardName = standard.Attribute("title").Value.Substring(0, 6);
                            string elementName = element.Attribute("title").Value.Substring(7, 2);
                            string CorpElementName = standardName + elementName;

                            if (CorpElementList.Contains(CorpElementName))
                            {
                                string complete = element.Attribute("complete").Value;
                                if (complete == "Yes")
                                {
                                    return false;
                                }
                                foreach (XElement factor in element.Elements("Factor"))
                                {
                                    string factAns = factor.Attribute("answer").Value;
                                    if (factAns == "Yes" || factAns == "NA")
                                    {
                                        return false;
                                    }

                                    foreach (XElement policies in factor.Elements("Policies"))
                                    {
                                        string policy = policies.Attribute("required").Value;
                                        if (policy != "")
                                            return false;
                                        else if (!policies.IsEmpty)
                                            return false;
                                    }
                                    foreach (XElement reports in factor.Elements("Reports"))
                                    {
                                        string report = reports.Attribute("required").Value;
                                        if (report != "")
                                            return false;
                                        else if (!reports.IsEmpty)
                                            return false;
                                    }
                                    foreach (XElement screenshots in factor.Elements("Screenshots"))
                                    {
                                        string screenshot = screenshots.Attribute("required").Value;
                                        if (screenshot != "")
                                            return false;
                                        else if (!screenshots.IsEmpty)
                                            return false;
                                    }
                                    foreach (XElement logs in factor.Elements("LogsOrTools"))
                                    {
                                        string log = logs.Attribute("required").Value;
                                        if (log != "")
                                            return false;
                                        else if (!logs.IsEmpty)
                                            return false;
                                    }
                                    foreach (XElement others in factor.Elements("OtherDocs"))
                                    {
                                        string other = others.Attribute("required").Value;
                                        if (other != "")
                                            return false;
                                    }
                                    foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                    {
                                        string privNote = privNotes.Value;
                                        if (privNote != "")
                                            return false;
                                    }
                                }
                                foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                {
                                    string revNote = revNotes.Value;
                                    if (revNote != "")
                                        return false;
                                }
                                foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                {
                                    string evalNote = evalNotes.Value;
                                    if (evalNote != "")
                                        return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public string GetXmlBySiteId(int SiteId)
        {
            try
            {
                string Questionaire = Convert.ToString((from filledQuestionaire in BMTDataContext.FilledQuestionnaires
                                                        //join project in BMTDataContext.Projects
                                                        //on filledQuestionaire.ProjectId equals project.ProjectId
                                                        //where project.PracticeSiteId == SiteId &&
                                                        //filledQuestionaire.QuestionnaireId == 2//for NCQA    
                                                        select filledQuestionaire.Answers).FirstOrDefault());

                return Questionaire;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void ChangeCorporateSite(int practiceSiteId)
        {
            try
            {


                string questionaire = Convert.ToString((from filledQuestion in BMTDataContext.FilledQuestionnaires
                                                        //join project in BMTDataContext.Projects
                                                        //on filledQuestion.ProjectId equals project.ProjectId
                                                        //where project.PracticeSiteId == practiceSiteId &&
                                                        //filledQuestion.QuestionnaireId == 2//for NCQA
                                                        select filledQuestion.Answers).FirstOrDefault());

                List<string> CorpElementList = (from CorpElement in BMTDataContext.CorporateElementLists
                                                select CorpElement.ElementName).ToList<string>();

                if (questionaire != "")
                {
                    XDocument RecQues = XDocument.Parse(questionaire);

                    foreach (XElement standard in RecQues.Root.Elements("Standard"))
                    {
                        foreach (XElement element in standard.Elements("Element"))
                        {
                            string standardName = standard.Attribute("title").Value.Substring(0, 6);
                            string elementName = element.Attribute("title").Value.Substring(7, 2);
                            string CorpElementName = standardName + elementName;
                            if (!CorpElementList.Contains(CorpElementName))
                            {
                                string complete = element.Attribute("complete").Value;
                                if (complete == "Yes")
                                {
                                    element.Attribute("complete").Value = "No";
                                }
                                foreach (XElement factor in element.Elements("Factor"))
                                {
                                    string factAns = factor.Attribute("answer").Value;
                                    if (factAns == "Yes" || factAns == "NA")
                                    {
                                        factor.Attribute("answer").Value = "No";
                                    }
                                    foreach (XElement policies in factor.Elements("Policies"))
                                    {
                                        policies.Attribute("required").Value = "";
                                        policies.Descendants("DocFile").Remove();
                                    }
                                    foreach (XElement reports in factor.Elements("Reports"))
                                    {
                                        reports.Attribute("required").Value = "";
                                        reports.Descendants("DocFile").Remove();
                                    }
                                    foreach (XElement screenshots in factor.Elements("Screenshots"))
                                    {
                                        screenshots.Attribute("required").Value = "";
                                        screenshots.Descendants("DocFile").Remove();
                                    }
                                    foreach (XElement logs in factor.Elements("LogsOrTools"))
                                    {
                                        logs.Attribute("required").Value = "";
                                        logs.Descendants("DocFile").Remove();
                                    }
                                    foreach (XElement others in factor.Elements("OtherDocs"))
                                    {
                                        others.Attribute("required").Value = "";
                                        others.Descendants("DocFile").Remove();
                                    }
                                    foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                    {
                                        privNotes.Remove();
                                    }
                                }
                                foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                {
                                    revNotes.Remove();
                                }
                                foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                {
                                    evalNotes.Remove();
                                }
                                foreach (XElement defaultScore in element.Elements("Calculation"))
                                {
                                    defaultScore.Attribute("defaultScore").Value = "0%";
                                }
                            }
                        }
                    }

                    string UpdatedQues = Convert.ToString(RecQues);
                    CorporateElementSubmissionBO _corpElementSubmission = new CorporateElementSubmissionBO();
                    _corpElementSubmission.UpdateQuestionaire(UpdatedQues, practiceSiteId);

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void CopyToNonCorporateXML(int practiceSiteId)
        {
            try
            {
                XElement[] CorpElementXML = { newElement, newElement, newElement, newElement, newElement, newElement, newElement, newElement, newElement, 
                                                newElement, newElement, newElement, newElement, newElement, newElement, newElement };

                int practiceId = (from pracSite in BMTDataContext.PracticeSites
                                  where pracSite.PracticeSiteId == practiceSiteId
                                  select pracSite.PracticeId).FirstOrDefault();

                int CorpSiteId = Convert.ToInt32((from prac in BMTDataContext.Practices
                                                  where prac.PracticeId == practiceId
                                                  select prac.PracticeSiteId).FirstOrDefault());

                List<string> submittedCorpElementList = (from CorpElementSubmission in BMTDataContext.CorporateElementSubmissions
                                                         join CorpElementList in BMTDataContext.CorporateElementLists
                                                         on CorpElementSubmission.CorporateElementId equals CorpElementList.CorporateElementId
                                                         where CorpElementSubmission.PracticeId == practiceId
                                                         select CorpElementList.ElementName).ToList<string>();

                submittedCorpElementList.Sort();

                string CorpQuestionaire = GetXmlBySiteId(CorpSiteId);
                if (CorpQuestionaire != "")
                {
                    XDocument recCorpQues = XDocument.Parse(CorpQuestionaire);

                    foreach (XElement standard in recCorpQues.Root.Elements("Standard"))
                    {
                        foreach (XElement element in standard.Elements("Element"))
                        {
                            string standardName = standard.Attribute("title").Value.Substring(0, 6);
                            string elementName = element.Attribute("title").Value.Substring(7, 2);
                            string CorpElementName = standardName + elementName;
                            for (int currentSubmittedElement = 0; currentSubmittedElement < submittedCorpElementList.Count; currentSubmittedElement++)
                            {
                                if (submittedCorpElementList[currentSubmittedElement] == CorpElementName)
                                {
                                    CorpElementXML[currentSubmittedElement] = element;
                                }
                            }
                        }
                    }

                }

                List<int> PracSiteId = (from PracSite in BMTDataContext.PracticeSites
                                        where PracSite.PracticeId == practiceId
                                        select PracSite.PracticeSiteId).ToList<int>();

                foreach (int SiteId in PracSiteId)
                {
                    CorporateElementSubmissionBO CorpElementSubmission = new CorporateElementSubmissionBO();
                    if (!CorpElementSubmission.IsSiteCorporate(practiceId, SiteId))
                    {
                        string CorpQues = GetXmlBySiteId(SiteId);
                        if (CorpQues != "")
                        {
                            XDocument Ques = XDocument.Parse(CorpQues);
                            for (int currentCopyingElement = 0; currentCopyingElement < submittedCorpElementList.Count; currentCopyingElement++)
                            {
                                foreach (XElement standards in Ques.Root.Elements("Standard"))
                                {
                                    foreach (XElement elements in standards.Elements("Element"))
                                    {
                                        string stdrdName = standards.Attribute("title").Value.Substring(0, 6);
                                        string eleName = elements.Attribute("title").Value.Substring(7, 2);
                                        string CorpElementName = stdrdName + eleName;
                                        //for (int currentCopyingElement = 0; currentCopyingElement < submittedCorpElementList.Count; currentCopyingElement++)
                                        //{
                                        if (submittedCorpElementList[currentCopyingElement] == CorpElementName)
                                        {
                                            elements.ReplaceWith(CorpElementXML[currentCopyingElement]);
                                        }
                                        //}
                                    }
                                }
                            }
                            string CopyQues = Convert.ToString(Ques);

                            CorporateElementSubmissionBO _corporateElementSubmission = new CorporateElementSubmissionBO();
                            _corporateElementSubmission.UpdateQuestionaire(CopyQues, SiteId);

                        }
                    }
                }
                // EmptyCorporateSite(CorpSiteId);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool CheckSiteForChangeCorporate(int practiceSiteId)
        {
            try
            {
                int practiceId = (from PracSite in BMTDataContext.PracticeSites
                                  where PracSite.PracticeSiteId == practiceSiteId
                                  select PracSite.PracticeId).FirstOrDefault();

                Practice prevCorpId = (from PracRec in BMTDataContext.Practices
                                       where PracRec.PracticeId == practiceId
                                       select PracRec).FirstOrDefault();

                if (prevCorpId.IsCorporate == true)
                {
                    if (prevCorpId.PracticeSiteId == practiceSiteId)
                    {
                        return true;
                    }
                    else
                    {
                        string recQuestionaire = GetXmlBySiteId(Convert.ToInt32(prevCorpId.PracticeSiteId));
                        if (recQuestionaire != "")
                        {
                            XDocument filledQuestionaire = XDocument.Parse(recQuestionaire);

                            foreach (XElement standard in filledQuestionaire.Root.Elements("Standard"))
                            {
                                foreach (XElement element in standard.Elements("Element"))
                                {

                                    string standardName = standard.Attribute("title").Value.Substring(0, 6);
                                    string elementName = element.Attribute("title").Value.Substring(7, 2);
                                    string CorpElementName = standardName + elementName;
                                    string complete = element.Attribute("complete").Value;
                                    if (complete == "Yes")
                                    {
                                        return false;
                                    }
                                    foreach (XElement factor in element.Elements("Factor"))
                                    {
                                        string factAns = factor.Attribute("answer").Value;
                                        if (factAns == "Yes" || factAns == "NA")
                                        {
                                            return false;
                                        }
                                        foreach (XElement policies in factor.Elements("Policies"))
                                        {
                                            string policy = policies.Attribute("required").Value;
                                            if (policy != "")
                                                return false;
                                            else if (!policies.IsEmpty)
                                                return false;
                                        }
                                        foreach (XElement reports in factor.Elements("Reports"))
                                        {
                                            string report = reports.Attribute("required").Value;
                                            if (report != "")
                                                return false;
                                            else if (!reports.IsEmpty)
                                                return false;
                                        }
                                        foreach (XElement screenshots in factor.Elements("Screenshots"))
                                        {
                                            string screenshot = screenshots.Attribute("required").Value;
                                            if (screenshot != "")
                                                return false;
                                            else if (!screenshots.IsEmpty)
                                                return false;
                                        }
                                        foreach (XElement logs in factor.Elements("LogsOrTools"))
                                        {
                                            string log = logs.Attribute("required").Value;
                                            if (log != "")
                                                return false;
                                            else if (!logs.IsEmpty)
                                                return false;
                                        }
                                        foreach (XElement others in factor.Elements("OtherDocs"))
                                        {
                                            string other = others.Attribute("required").Value;
                                            if (other != "")
                                                return false;
                                            else if (!others.IsEmpty)
                                                return false;
                                        }
                                    }
                                    foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                                    {
                                        string revNote = revNotes.Value;
                                        if (revNote != "")
                                            return false;
                                    }
                                    foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                                    {
                                        string evalNote = evalNotes.Value;
                                        if (evalNote != "")
                                            return false;
                                    }

                                }
                            }
                        }
                        else
                            return true;
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void EmptyCorporateSite(int practiceSiteId)
        {
            try
            {
                string recQuestionaire = GetXmlBySiteId(practiceSiteId);

                if (recQuestionaire != null)
                {
                    XDocument RecQues = XDocument.Parse(recQuestionaire);

                    foreach (XElement standard in RecQues.Root.Elements("Standard"))
                    {
                        foreach (XElement element in standard.Elements("Element"))
                        {
                            string standardName = standard.Attribute("title").Value.Substring(0, 6);
                            string elementName = element.Attribute("title").Value.Substring(7, 2);
                            string CorpElementName = standardName + elementName;
                            string complete = element.Attribute("complete").Value;
                            if (complete == "Yes")
                            {
                                element.Attribute("complete").Value = "No";
                            }
                            foreach (XElement factor in element.Elements("Factor"))
                            {
                                string factAns = factor.Attribute("answer").Value;
                                if (factAns == "Yes" || factAns == "NA")
                                {
                                    factor.Attribute("answer").Value = "No";
                                }
                                foreach (XElement policies in factor.Elements("Policies"))
                                {
                                    policies.Attribute("required").Value = "";
                                    policies.Descendants("DocFile").Remove();
                                }
                                foreach (XElement reports in factor.Elements("Reports"))
                                {
                                    reports.Attribute("required").Value = "";
                                    reports.Descendants("DocFile").Remove();
                                }
                                foreach (XElement screenshots in factor.Elements("Screenshots"))
                                {
                                    screenshots.Attribute("required").Value = "";
                                    screenshots.Descendants("DocFile").Remove();
                                }
                                foreach (XElement logs in factor.Elements("LogsOrTools"))
                                {
                                    logs.Attribute("required").Value = "";
                                    logs.Descendants("DocFile").Remove();
                                }
                                foreach (XElement others in factor.Elements("OtherDocs"))
                                {
                                    others.Attribute("required").Value = "";
                                    others.Descendants("DocFile").Remove();
                                }
                                foreach (XElement privNotes in factor.Elements("PrivateNote"))
                                {
                                    privNotes.Remove();
                                }
                            }
                            foreach (XElement revNotes in element.Elements("ReviewerNotes"))
                            {
                                revNotes.Remove();
                            }
                            foreach (XElement evalNotes in element.Elements("EvaluationNotes"))
                            {
                                evalNotes.Remove();
                            }
                            foreach (XElement defaultScore in element.Elements("Calculation"))
                            {
                                defaultScore.Attribute("defaultScore").Value = "0%";
                            }

                        }
                    }

                    string UpdatedQues = Convert.ToString(RecQues);
                    CorporateElementSubmissionBO _corpElementSubmission = new CorporateElementSubmissionBO();
                    _corpElementSubmission.UpdateQuestionaire(UpdatedQues, practiceSiteId);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsTemplateCorporate(int templateId, int practiceSiteId)
        {
            try
            {
                //List<PracticeTemplate> corporatePractice = (from practiceRecords in BMTDataContext.PracticeTemplates
                //                                            where practiceRecords.PracticeSiteId == practiceSiteId
                //                                            && practiceRecords.IsCorporate == true &&
                //                                            practiceRecords.TemplateId == templateId
                //                                            select practiceRecords).ToList();
                //if (corporatePractice.Count != 0)
                //    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckSiteForChangeCorporateMORe(int practiceSiteId, int templateId)
        {
            try
            {
                //int practiceId = (from PracSite in BMTDataContext.PracticeSites
                //                  where PracSite.PracticeSiteId == practiceSiteId
                //                  select PracSite.PracticeId).FirstOrDefault();

                //PracticeTemplate prevCorpId = (from PracRec in BMTDataContext.PracticeTemplates
                //                               where PracRec.PracticeId == practiceId &&
                //                               PracRec.TemplateId == templateId
                //                               select PracRec).FirstOrDefault();

                //if (prevCorpId.IsCorporate == true)
                //{
                //    if (prevCorpId.PracticeSiteId == practiceSiteId)
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        int projectId = (from project in BMTDataContext.Projects
                //                         where project.PracticeSiteId == prevCorpId.PracticeSiteId
                //                         select project.ProjectId).FirstOrDefault();

                //        List<FilledAnswer> fAns = (from filledAnswer in BMTDataContext.FilledAnswers
                //                                   join kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                //                                   on filledAnswer.KnowledgeBaseTemplateId equals kbaseTemp.KnowledgeBaseTemplateId
                //                                   where filledAnswer.ProjectId == projectId &&
                //                                   kbaseTemp.IsCorporateElement == true
                //                                   select filledAnswer).ToList();

                //        foreach (FilledAnswer ans in fAns)
                //        {
                //            if (ans.Complete == true)
                //                return false;
                //            if (ans.DefaultScore != "0%")
                //            {
                //                if (ans.DefaultScore != null)
                //                    return false;
                //            }
                //            if (ans.ReviewNotes != "")
                //            {
                //                if (ans.ReviewNotes != null)
                //                    return false;
                //            }
                //            if (ans.EvaluationNotes != "")
                //            {
                //                if (ans.EvaluationNotes != null)
                //                    return false;
                //            }

                //            int ElementkbId = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                //                               where kbTempRec.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                //                               select kbTempRec.KnowledgeBaseId).FirstOrDefault();

                //            List<int> QuestionkbTempIds = (from kbase in BMTDataContext.KnowledgeBaseTemplates
                //                                           where kbase.ParentKnowledgeBaseId == ElementkbId &&
                //                                           kbase.TemplateId == templateId
                //                                           select kbase.KnowledgeBaseTemplateId).ToList();

                //            List<FilledAnswer> fQuesAns = (from fAnsRec in BMTDataContext.FilledAnswers
                //                                           where QuestionkbTempIds.Contains(fAnsRec.KnowledgeBaseTemplateId) &&
                //                                           fAnsRec.ProjectId == projectId
                //                                           select fAnsRec).ToList();

                //            foreach (FilledAnswer ansRec in fQuesAns)
                //            {
                //                if (ansRec.DataBoxComments != null)
                //                    return false;
                //                if (ansRec.PoliciesDocumentCount != 0)
                //                {
                //                    if (ansRec.PoliciesDocumentCount != null)
                //                        return false;
                //                }
                //                if (ansRec.ScreenShotsDocumentCount != 0)
                //                {
                //                    if (ansRec.ScreenShotsDocumentCount != null)
                //                        return false;
                //                }
                //                if (ansRec.ReportsDocumentCount != 0)
                //                {
                //                    if (ansRec.ReportsDocumentCount != null)
                //                        return false;
                //                }
                //                if (ansRec.LogsOrToolsDocumentCount != 0)
                //                {
                //                    if (ansRec.LogsOrToolsDocumentCount != null)
                //                        return false;
                //                }
                //                if (ansRec.OtherDocumentsCount != 0)
                //                {
                //                    if (ansRec.OtherDocumentsCount != null)
                //                        return false;
                //                }

                //                if (ansRec.AnswerTypeEnumId != null)
                //                {
                //                    string ansType = (from fAnswer in BMTDataContext.FilledAnswers
                //                                      join ansTypeEnum in BMTDataContext.AnswerTypeEnums
                //                                      on fAnswer.AnswerTypeEnumId equals ansTypeEnum.AnswerTypeEnumId
                //                                      where fAnswer.FilledAnswersId == ansRec.FilledAnswersId
                //                                      select ansTypeEnum.DiscreteValue).FirstOrDefault();

                //                    if (ansType != "No")
                //                        return false;
                //                }
                //            }
                //        }
                //        return true;
                //    }
                //}
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool CheckForRemoveCorporateSiteInTemplate(int SiteId, int templateId)
        {
            try
            {
                int pracId = (from pracRec in BMTDataContext.PracticeSites
                              where pracRec.PracticeSiteId == SiteId
                              select pracRec.PracticeId).FirstOrDefault();

                int pracSiteId = Convert.ToInt32((from prac in BMTDataContext.Practices
                                                  where prac.PracticeId == pracId
                                                  select prac.PracticeSiteId).FirstOrDefault());

                int projectId = (from project in BMTDataContext.Projects
                                 //where project.PracticeSiteId == SiteId
                                 select project.ProjectId).FirstOrDefault();

                List<FilledAnswer> fAns = (from filledAnswer in BMTDataContext.FilledAnswers
                                           join kbaseTempRec in BMTDataContext.KnowledgeBaseTemplates
                                           on filledAnswer.KnowledgeBaseTemplateId equals kbaseTempRec.KnowledgeBaseTemplateId
                                           where 
                                           //filledAnswer.ProjectId == projectId &&
                                           kbaseTempRec.IsCorporateElement == true
                                           select filledAnswer).ToList();

                foreach (FilledAnswer ans in fAns)
                {
                    if (ans.Complete == true)
                        return false;
                    if (ans.DefaultScore != "0%")
                    {
                        if (ans.DefaultScore != null)
                            return false;
                    }
                    if (ans.ReviewNotes != "")
                    {
                        if (ans.ReviewNotes != null)
                            return false;
                    }
                    if (ans.EvaluationNotes != "")
                    {
                        if (ans.EvaluationNotes != null)
                            return false;
                    }
                    int ElementkbId = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       where kbTempRec.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                       select kbTempRec.KnowledgeBaseId).FirstOrDefault();

                    List<int> QuestionkbTempIds = (from kbase in BMTDataContext.KnowledgeBaseTemplates
                                                   where kbase.ParentKnowledgeBaseId == ElementkbId &&
                                                   kbase.TemplateId == templateId
                                                   select kbase.KnowledgeBaseTemplateId).ToList();

                    List<FilledAnswer> fQuesAns = (from fAnsRec in BMTDataContext.FilledAnswers
                                                   where QuestionkbTempIds.Contains(fAnsRec.KnowledgeBaseTemplateId) 
                                                   //&& fAnsRec.ProjectId == projectId
                                                   select fAnsRec).ToList();

                    foreach (FilledAnswer ansRec in fQuesAns)
                    {
                        if (ansRec.DataBoxComments != null)
                            return false;
                        if (ansRec.PoliciesDocumentCount != 0)
                        {
                            if (ansRec.PoliciesDocumentCount != null)
                                return false;
                        }
                        if (ansRec.ScreenShotsDocumentCount != 0)
                        {
                            if (ansRec.ScreenShotsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.ReportsDocumentCount != 0)
                        {
                            if (ansRec.ReportsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.LogsOrToolsDocumentCount != 0)
                        {
                            if (ansRec.LogsOrToolsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.OtherDocumentsCount != 0)
                        {
                            if (ansRec.OtherDocumentsCount != null)
                                return false;
                        }

                        if (ansRec.AnswerTypeEnumId != null)
                        {
                            string ansType = (from fAnswer in BMTDataContext.FilledAnswers
                                              join ansTypeEnum in BMTDataContext.AnswerTypeEnums
                                              on fAnswer.AnswerTypeEnumId equals ansTypeEnum.AnswerTypeEnumId
                                              where fAnswer.FilledAnswersId == ansRec.FilledAnswersId
                                              select ansTypeEnum.DiscreteValue).FirstOrDefault();

                            if (ansType != "No")
                                return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool CheckForCorporateSiteMORe(int SiteId, int templateId)
        {
            try
            {
                int projectId = (from project in BMTDataContext.Projects
                                 //where project.PracticeSiteId == SiteId
                                 select project.ProjectId).FirstOrDefault();

                List<FilledAnswer> fAns = (from filledAnswer in BMTDataContext.FilledAnswers
                                           join kbTemp in BMTDataContext.KnowledgeBaseTemplates
                                           on filledAnswer.KnowledgeBaseTemplateId equals kbTemp.KnowledgeBaseTemplateId
                                           join kbase in BMTDataContext.KnowledgeBases
                                           on kbTemp.KnowledgeBaseId equals kbase.KnowledgeBaseId
                                           where 
                                           //filledAnswer.ProjectId == projectId &&
                                           kbase.KnowledgeBaseTypeId == (int)enKnowledgeBaseType.SubHeader &&
                                           (kbTemp.IsCorporateElement == null||kbTemp.IsCorporateElement ==false)
                                           select filledAnswer).ToList();

                foreach (FilledAnswer ans in fAns)
                {
                    if (ans.Complete == true)
                        return false;
                    if (ans.DefaultScore != "0%")
                    {
                        if (ans.DefaultScore != null)
                            return false;
                    }
                    if (ans.ReviewNotes != "")
                    {
                        if (ans.ReviewNotes != null)
                            return false;
                    }
                    if (ans.EvaluationNotes != "")
                    {
                        if (ans.EvaluationNotes != null)
                            return false;
                    }
                    int ElementkbId = (from kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                                       where kbTempRec.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                                       select kbTempRec.KnowledgeBaseId).FirstOrDefault();

                    List<int> QuestionkbTempIds = (from kbase in BMTDataContext.KnowledgeBaseTemplates
                                                   where kbase.ParentKnowledgeBaseId == ElementkbId &&
                                                   kbase.TemplateId == templateId
                                                   select kbase.KnowledgeBaseTemplateId).ToList();

                    List<FilledAnswer> fQuesAns = (from fAnsRec in BMTDataContext.FilledAnswers
                                                   where QuestionkbTempIds.Contains(fAnsRec.KnowledgeBaseTemplateId) 
                                                   //&& fAnsRec.ProjectId == projectId
                                                   select fAnsRec).ToList();

                    foreach (FilledAnswer ansRec in fQuesAns)
                    {
                        if (ansRec.DataBoxComments != null)
                            return false;
                        if (ansRec.PoliciesDocumentCount != 0)
                        {
                            if (ansRec.PoliciesDocumentCount != null)
                                return false;
                        }
                        if (ansRec.ScreenShotsDocumentCount != 0)
                        {
                            if (ansRec.ScreenShotsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.ReportsDocumentCount != 0)
                        {
                            if (ansRec.ReportsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.LogsOrToolsDocumentCount != 0)
                        {
                            if (ansRec.LogsOrToolsDocumentCount != null)
                                return false;
                        }
                        if (ansRec.OtherDocumentsCount != 0)
                        {
                            if (ansRec.OtherDocumentsCount != null)
                                return false;
                        }

                        if(ansRec.AnswerTypeEnumId!=null)
                        {
                        string ansType = (from fAnswer in BMTDataContext.FilledAnswers
                                          join ansTypeEnum in BMTDataContext.AnswerTypeEnums
                                          on fAnswer.AnswerTypeEnumId equals ansTypeEnum.AnswerTypeEnumId
                                          where fAnswer.FilledAnswersId==ansRec.FilledAnswersId
                                          select ansTypeEnum.DiscreteValue).FirstOrDefault();

                        if (ansType != "No")
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void TemplateCopyToNonCorporateSite(int practiceSiteId, int templateId)
        {
            try
            {
                //int practiceId = (from pracSite in BMTDataContext.PracticeSites
                //                  where pracSite.PracticeSiteId == practiceSiteId
                //                  select pracSite.PracticeId).FirstOrDefault();

                //List<int> practiceSiteIds = (from pracSiteIds in BMTDataContext.PracticeSites
                //                             where pracSiteIds.PracticeId == practiceId
                //                             select pracSiteIds.PracticeSiteId).ToList();

                //int CorpSiteId = Convert.ToInt32((from prac in BMTDataContext.PracticeTemplates
                //                                  where prac.PracticeId == practiceId &&
                //                                  prac.TemplateId == templateId
                //                                  select prac.PracticeSiteId).FirstOrDefault());

                //int CorpSiteprojectId = (from projectRec in BMTDataContext.Projects
                //                         where projectRec.PracticeSiteId == CorpSiteId
                //                         select projectRec.ProjectId).FirstOrDefault();

                //List<int> nonCorpSitesProjectIds = (from projectRec in BMTDataContext.Projects
                //                                    where practiceSiteIds.Contains((int)projectRec.PracticeSiteId) &&
                //                                    projectRec.PracticeSiteId != CorpSiteId
                //                                    select projectRec.ProjectId).ToList();

                //List<FilledAnswer> subHeaderLevelFAns = (from fAnsRec in BMTDataContext.FilledAnswers
                //                                         join kbTempRec in BMTDataContext.KnowledgeBaseTemplates
                //                                         on fAnsRec.KnowledgeBaseTemplateId equals kbTempRec.KnowledgeBaseTemplateId
                //                                         where fAnsRec.ProjectId == CorpSiteprojectId &&
                //                                         kbTempRec.IsCorporateElement == true
                //                                         select fAnsRec).ToList();

                //foreach (FilledAnswer ans in subHeaderLevelFAns)
                //{
                //    int subHeaderKbIds = (from kbaseTemp in BMTDataContext.KnowledgeBaseTemplates
                //                          where kbaseTemp.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId
                //                          select kbaseTemp.KnowledgeBaseId).FirstOrDefault();

                //    List<int> TempIds = (from kbaseRec in BMTDataContext.KnowledgeBaseTemplates
                //                         where kbaseRec.ParentKnowledgeBaseId == subHeaderKbIds
                //                         select kbaseRec.KnowledgeBaseTemplateId).ToList();

                //    List<FilledAnswer> nonCorpSites = (from filledAns in BMTDataContext.FilledAnswers
                //                                       where filledAns.KnowledgeBaseTemplateId == ans.KnowledgeBaseTemplateId &&
                //                                       nonCorpSitesProjectIds.Contains((int)filledAns.ProjectId)
                //                                       select filledAns).ToList();

                //    foreach (FilledAnswer copySubHeaderAns in nonCorpSites)
                //    {
                //        copySubHeaderAns.EvaluationNotes = ans.EvaluationNotes;
                //        copySubHeaderAns.ReviewNotes = ans.ReviewNotes;
                //        copySubHeaderAns.Complete = ans.Complete;
                //        copySubHeaderAns.DefaultScore = ans.DefaultScore;

                //        BMTDataContext.SubmitChanges();
                //    }
                //    List<FilledAnswer> questions = (from fAns in BMTDataContext.FilledAnswers
                //                                    where TempIds.Contains(fAns.KnowledgeBaseTemplateId) &&
                //                                    fAns.ProjectId == CorpSiteprojectId
                //                                    select fAns).ToList();

                //    foreach (FilledAnswer filledAns in questions)
                //    {
                //        List<FilledAnswer> nonCorpSitesQuestion = (from filledAnswer in BMTDataContext.FilledAnswers
                //                                                   where filledAnswer.KnowledgeBaseTemplateId == filledAns.KnowledgeBaseTemplateId &&
                //                                                   nonCorpSitesProjectIds.Contains((int)filledAnswer.ProjectId)
                //                                                   select filledAnswer).ToList();

                //        foreach (FilledAnswer copyQuestion in nonCorpSitesQuestion)
                //        {
                //            copyQuestion.ReportsDocumentCount = filledAns.ReportsDocumentCount;
                //            copyQuestion.ScreenShotsDocumentCount = filledAns.ScreenShotsDocumentCount;
                //            copyQuestion.PoliciesDocumentCount = filledAns.PoliciesDocumentCount;
                //            copyQuestion.LogsOrToolsDocumentCount = filledAns.LogsOrToolsDocumentCount;
                //            copyQuestion.OtherDocumentsCount = filledAns.OtherDocumentsCount;
                //            copyQuestion.PrivateNotes = filledAns.PrivateNotes;
                //            copyQuestion.AnswerTypeEnumId = filledAns.AnswerTypeEnumId;
                //            copyQuestion.DataBoxComments = filledAns.DataBoxComments;

                //            BMTDataContext.SubmitChanges();

                //            List<FilledTemplateDocument> fDoc = (from fillDocs in BMTDataContext.FilledTemplateDocuments
                //                                                 where fillDocs.FilledAnswerId == filledAns.FilledAnswersId
                //                                                 select fillDocs).ToList();

                //            foreach (FilledTemplateDocument fTempDoc in fDoc)
                //            {
                //                TemplateDocument tempDoc = (from tDocs in BMTDataContext.TemplateDocuments
                //                                            where tDocs.DocumentId == fTempDoc.DocumentId
                //                                            select tDocs).FirstOrDefault();

                //                TemplateDocument newTDocs = new TemplateDocument();
                //                newTDocs.Name = tempDoc.Name;
                //                newTDocs.ReferencePages = tempDoc.ReferencePages;
                //                newTDocs.Path = tempDoc.Path;
                //                newTDocs.RelevencyLevel = tempDoc.RelevencyLevel;
                //                newTDocs.DocumentType = tempDoc.DocumentType;

                //                BMTDataContext.TemplateDocuments.InsertOnSubmit(newTDocs);
                //                BMTDataContext.SubmitChanges();
                //                int newTdocId = newTDocs.DocumentId;

                //                List<FilledTemplateDocument> filledDoc = (from fTempDocs in BMTDataContext.FilledTemplateDocuments
                //                                                          where fTempDocs.FilledAnswerId == copyQuestion.FilledAnswersId
                //                                                          select fTempDocs).ToList();

                //                foreach (FilledTemplateDocument fTDoc in filledDoc)
                //                {
                //                    TemplateDocument templateDoc = (from tempDocument in BMTDataContext.TemplateDocuments
                //                                                    where tempDocument.DocumentId == fTDoc.DocumentId
                //                                                    select tempDocument).FirstOrDefault();

                //                    BMTDataContext.FilledTemplateDocuments.DeleteOnSubmit(fTDoc);
                //                    BMTDataContext.SubmitChanges();
                //                    if (templateDoc != null)
                //                    {
                //                        BMTDataContext.TemplateDocuments.DeleteOnSubmit(templateDoc);
                //                        BMTDataContext.SubmitChanges();
                //                    }
                //                }

                //                FilledTemplateDocument newfDoc = new FilledTemplateDocument();
                //                newfDoc.FilledAnswerId = copyQuestion.FilledAnswersId;
                //                newfDoc.DocumentId = newTdocId;

                //                BMTDataContext.FilledTemplateDocuments.InsertOnSubmit(newfDoc);
                //                BMTDataContext.SubmitChanges();
                //            }
                //        }
                //    }
                //}
                //DeleteFromPreviousCorpSubmissionMORe(practiceId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool UpdatePracticeTemplate(int templateId, bool isCorporate, int practiceSiteId)
        {
            try
            {
                //int practiceId = (from pracSite in BMTDataContext.PracticeSites
                //                  where pracSite.PracticeSiteId == practiceSiteId
                //                  select pracSite.PracticeId).FirstOrDefault();

                //PracticeTemplate pracTemp = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                             where pracTempRec.TemplateId == templateId &&
                //                             pracTempRec.PracticeId == practiceId
                //                             select pracTempRec).FirstOrDefault();
                //if (pracTemp != null)
                //{
                //    if (isCorporate)
                //    {
                //        if (pracTemp.PracticeSiteId != practiceSiteId)
                //        {
                //            DeleteFromPreviousCorpSubmissionMORe(practiceId);
                //        }
                //        pracTemp.IsCorporate = isCorporate;
                //        pracTemp.PracticeSiteId = practiceSiteId;
                //        BMTDataContext.SubmitChanges();
                //    }
                //    else
                //    {
                //        pracTemp.IsCorporate = isCorporate;
                //        pracTemp.PracticeSiteId = 0;
                //        BMTDataContext.SubmitChanges();
                //    }
                //    return true;
                //}
                //else
                //{
                    return false;
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool DeleteFromPreviousCorpSubmissionMORe(int practiceId)
        {
            System.Data.Common.DbTransaction transaction;
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();
            BMTDataContext.Connection.Open();
            transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            try
            {
                List<CorporateKnowledgeBaseElement> corpSubmissionInMORe = (from corpKBElement in BMTDataContext.CorporateKnowledgeBaseElements
                                                                            where corpKBElement.PracticeId == practiceId
                                                                            select corpKBElement).ToList();

                foreach (CorporateKnowledgeBaseElement corpSub in corpSubmissionInMORe)
                {
                    BMTDataContext.CorporateKnowledgeBaseElements.DeleteOnSubmit(corpSub);
                    BMTDataContext.SubmitChanges();
                }
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool CheckCorporateTypeMORe(int siteId)
        {

            try
            {
                //List<PracticeTemplate> corporatePractice = (from practiceRecords in BMTDataContext.PracticeTemplates
                //                                    where practiceRecords.PracticeSiteId == siteId
                //                                    && practiceRecords.IsCorporate == true
                //                                    select practiceRecords).ToList();
                //if (corporatePractice.Count != 0)
                //    return true;

                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

    }
}
