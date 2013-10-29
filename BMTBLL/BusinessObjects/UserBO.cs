#region Modification History

//  ******************************************************************************
//  Module        : User Administration
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/03/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                         Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig        12-12-2011      NUll Handling in GetMaxId Method
//  Mirza Fahad Ali Baig        01/06/2012      Primary Care Providers at Site 
//  Mirza Fahad Ali Baig        02-27-2012      Remove Un-nessary code
//  Mirza Fahad Ali Baig        02-27-2012      Optimize the current code
//  Mirza Fahad Ali Baig        03-06-2012      Set name of variables in GetCredtailDetailByUserId function
//  Mirza Fahad Ali Baig        03-06-2012      Fix GetUserCredentialByUserId function logic
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class UserBO : AddressBO
    {
        #region CONSTANTS
        public const bool DEFAULT_ISACTIVE = true;
        private const string DEFAULT_FIRST_NAME = "Consultant";
        public const int DEFAULT_ROLEID = (int)enUserRole.User;

        #endregion

        #region VARIABLES
        private User _userAccount;
        private PracticeUser _practiceUser;
        private Email email;

        private IQueryable _userList;
        public IQueryable UserList
        {
            get { return _userList; }
            set { _userList = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _projectName;
        public string projectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        private string _companyName;
        public string companyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }
        private string _url;
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }
        private string _body;
        public string body
        {
            get { return _body; }
            set { _body = value; }
        }
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
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

        private DateTime _lastActivityDate;
        public DateTime LastActivityDate
        {
            get { return _lastActivityDate; }
            set { _lastActivityDate = value; }
        }

        private int _roleId;
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private int _practiceId;
        public int PracticeId
        {
            get { return _practiceId; }
            set { _practiceId = value; }
        }

        private string _fistName;
        public string FistName
        {
            get { return _fistName; }
            set { _fistName = value; }
        }


        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private int _practiceSiteId;
        public int PracticeSiteId
        {
            get { return _practiceSiteId; }
            set { _practiceSiteId = value; }
        }

        private int _providerTypeId;
        public int ProviderTypeId
        {
            get { return _providerTypeId; }
            set { _providerTypeId = value; }
        }

        private int? _credentialId;
        public int? CredentialId
        {
            get { return _credentialId; }
            set { _credentialId = value; }
        }

        private int? _specialityId;
        public int? SpecialityId
        {
            get { return _specialityId; }
            set { _specialityId = value; }
        }

        private DateTime _BPRPExpireDate;
        public DateTime BPRPExpireDate
        {
            get { return _BPRPExpireDate; }
            set { _BPRPExpireDate = value; }
        }

        private DateTime _DRPExpireDate;
        public DateTime DRPExpireDate
        {
            get { return _DRPExpireDate; }
            set { _DRPExpireDate = value; }
        }

        private DateTime _HSRPExpireDate;
        public DateTime HSRPExpireDate
        {
            get { return _HSRPExpireDate; }
            set { _HSRPExpireDate = value; }
        }

        private int _PracticeRoleId;
        public int PracticeRoleId
        {
            get { return _PracticeRoleId; }
            set { _PracticeRoleId = value; }
        }

        private IQueryable _primaryCareProviderList;
        public IQueryable primaryCareProviderList
        {
            get { return _primaryCareProviderList; }
            set { _primaryCareProviderList = value; }
        }


        #endregion

        #region CONSTRUCTOR
        public UserBO()
        {
            _isActive = DEFAULT_ISACTIVE;
            _roleId = DEFAULT_ROLEID;
            _createdDate = System.DateTime.Now;
        }

        #endregion

        #region FUNCTIONS
        public bool SaveUser()
        {
            try
            {
                _userAccount = new User();
                _practiceUser = new PracticeUser();

                if (this.UserId < 1)
                    return AddNewUser();
                else
                    return UpdateUser();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ConsultantUpdate(string password, int id)
        {
            var v = from o in BMTDataContext.Users
                    where o.UserId == id
                    select o;

            foreach (User c in v)
            {
                c.Password = password;
            }


            BMTDataContext.SubmitChanges();

        }

        public void UpdatePasswordOnly(string password, int id)
        {
            if ((password != "" || password != null) && id != 0)
            {
                ConsultantUpdate(password, id);
            }

        }

        private bool AddNewUser()
        {
            try
            {
                //  Create User Account
                _userAccount.Username = this.UserName;
                _userAccount.Password = this.Password;
                _userAccount.Email = this.Email;
                _userAccount.IsActive = this.IsActive;
                _userAccount.CreatedDate = this.CreatedDate;
                _userAccount.LastActivitydate = this.LastActivityDate;
                _userAccount.RoleId = this.RoleId;


                BMTDataContext.Users.InsertOnSubmit(_userAccount);
                BMTDataContext.SubmitChanges();

                // Getting UserId
                _userId = _userAccount.UserId;

                // Insert User personal Detail
                _practiceUser.UserId = this.UserId;
                _practiceUser.PracticeId = this.PracticeId;
                _practiceUser.FirstName = this.FistName;
                _practiceUser.LastName = this.LastName;
                _practiceUser.ProviderTypeId = this.ProviderTypeId;
                if (this.CredentialId != 0)
                {
                    _practiceUser.CredentialId = this.CredentialId;
                }
                if (this.SpecialityId != 0)
                {
                    _practiceUser.SpecialityId = this.SpecialityId;
                }
                _practiceUser.PracticeSiteId = this.PracticeSiteId;

                if (this.BPRPExpireDate != Convert.ToDateTime("01/01/0001"))
                {
                    _practiceUser.BPRPExpiryDate = this.BPRPExpireDate;
                }
                if (DRPExpireDate != Convert.ToDateTime("01/01/0001"))
                {
                    _practiceUser.DRPExpiryDate = this.DRPExpireDate;
                }
                if (HSRPExpireDate != Convert.ToDateTime("01/01/0001"))
                {
                    _practiceUser.HSRPExpiryDate = this.HSRPExpireDate;
                }

                _practiceUser.CreatedDate = this.CreatedDate;
                _practiceUser.CreatedBy = this.CreatedBy;
                if (this.PracticeRoleId != 0)
                {
                    _practiceUser.PracticeRoleId = this.PracticeRoleId;
                }

                BMTDataContext.PracticeUsers.InsertOnSubmit(_practiceUser);
                BMTDataContext.SubmitChanges();

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool UpdateUser()
        {

            try
            {
                // update user Practice Detail 
                var userPracticeRow = (from UserPracticeRecord in BMTDataContext.PracticeUsers
                                       where UserPracticeRecord.UserId == this.UserId
                                       select new { UserPracticeRecord }).SingleOrDefault();

                userPracticeRow.UserPracticeRecord.FirstName = this.FistName;
                userPracticeRow.UserPracticeRecord.LastName = this.LastName;
                userPracticeRow.UserPracticeRecord.ProviderTypeId = this.ProviderTypeId;

                if (this.CredentialId != 0)
                {
                    userPracticeRow.UserPracticeRecord.CredentialId = this.CredentialId;
                }
                else { userPracticeRow.UserPracticeRecord.CredentialId = null; }
                if (this.SpecialityId != 0)
                {
                    userPracticeRow.UserPracticeRecord.SpecialityId = this.SpecialityId;
                }
                else { userPracticeRow.UserPracticeRecord.SpecialityId = null; }

                userPracticeRow.UserPracticeRecord.PracticeSiteId = this.PracticeSiteId;
                if (this.BPRPExpireDate == Convert.ToDateTime("01/01/0001"))
                {
                    userPracticeRow.UserPracticeRecord.BPRPExpiryDate = null;
                }
                else
                {
                    userPracticeRow.UserPracticeRecord.BPRPExpiryDate = this.BPRPExpireDate;
                }

                if (this.DRPExpireDate == Convert.ToDateTime("01/01/0001"))
                {

                    userPracticeRow.UserPracticeRecord.DRPExpiryDate = null;
                }
                else { userPracticeRow.UserPracticeRecord.DRPExpiryDate = this.DRPExpireDate; }


                if (this.HSRPExpireDate == Convert.ToDateTime("01/01/0001"))
                {

                    userPracticeRow.UserPracticeRecord.HSRPExpiryDate = null;
                }
                else { userPracticeRow.UserPracticeRecord.HSRPExpiryDate = this.HSRPExpireDate; }
                if (this.PracticeRoleId != 0)
                {
                    userPracticeRow.UserPracticeRecord.PracticeRoleId = this.PracticeRoleId;
                }
                BMTDataContext.SubmitChanges();

                //  Update User Account
                var userAccountRow = (from UserAccountRecord in BMTDataContext.Users
                                      where UserAccountRecord.UserId == this.UserId
                                      select new { UserAccountRecord }).SingleOrDefault();
                userAccountRow.UserAccountRecord.Email = this.Email;
                userAccountRow.UserAccountRecord.Password = this.Password;
                userAccountRow.UserAccountRecord.IsActive = this.IsActive;
                userAccountRow.UserAccountRecord.LastActivitydate = this.LastActivityDate;
                BMTDataContext.SubmitChanges();

                return true;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public int GetMaxIdforConsultant()
        {
            int GetMaxId = (from Maximum in BMTDataContext.Users select Maximum.UserId).Max();
            return (GetMaxId + 1);
        }

        public int ConsultantRoleId()
        {
            var roleid = (from role in BMTDataContext.Roles
                          where role.Name == "Consultant" || role.Name == "consultant"
                          select new
                          {
                              role.RoleId
                          }).SingleOrDefault();


            return (roleid.RoleId);



        }

        public int GetMaxId()
        {
            try
            {
                var RecordExist = (from Record in BMTDataContext.Users select Record.UserId).Take(1);
                int GetMaxId;
                if (RecordExist.Any())
                {
                    GetMaxId = (from Maximum in BMTDataContext.Users select Maximum.UserId).Max();
                }
                else { GetMaxId = 1; }
                return GetMaxId + 1;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public void GetUserByPracticeId(int practiceId)
        {
            try
            {

                _userList = (from UserRecord in BMTDataContext.Users
                             join PracticeUserRecord in BMTDataContext.PracticeUsers
                                on UserRecord.UserId equals PracticeUserRecord.UserId
                             join PracticeSiteRecord in BMTDataContext.PracticeSites
                                 on PracticeUserRecord.PracticeSiteId equals PracticeSiteRecord.PracticeSiteId
                             where (PracticeUserRecord.PracticeId == practiceId)
                             orderby UserRecord.CreatedDate
                             select new
                             {
                                 UserRecord.Username,
                                 PracticeUserRecord.UserId,
                                 PracticeUserRecord.FirstName,
                                 PracticeUserRecord.LastName,
                                 PracticeSiteRecord.Name,
                                 IsActive = UserRecord.IsActive.ToString() == "True" ? "Yes" : "No"
                             }).AsQueryable();

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public void GetUserByUserId(int userId)
        {

            var UserDetail = (from UserRecord in BMTDataContext.Users
                              from PracticeUserRecord in BMTDataContext.PracticeUsers
                              where (UserRecord.UserId == PracticeUserRecord.UserId)
                                && (UserRecord.UserId == userId)
                              select new
                              {
                                  UserRecord,
                                  UserPracticeRecord = PracticeUserRecord
                              }).SingleOrDefault();

            if (UserDetail != null)
            {
                _userName = UserDetail.UserRecord.Username;
                _password = UserDetail.UserRecord.Password;
                _email = UserDetail.UserRecord.Email;
                _isActive = UserDetail.UserRecord.IsActive;

                _fistName = UserDetail.UserPracticeRecord.FirstName;
                _lastName = UserDetail.UserPracticeRecord.LastName;
                _practiceSiteId = (int)UserDetail.UserPracticeRecord.PracticeSiteId;
                _providerTypeId = UserDetail.UserPracticeRecord.ProviderTypeId.HasValue ? (int)UserDetail.UserPracticeRecord.ProviderTypeId : 0;
                _credentialId = UserDetail.UserPracticeRecord.CredentialId.HasValue ? (int)UserDetail.UserPracticeRecord.CredentialId : 0;
                _specialityId = UserDetail.UserPracticeRecord.SpecialityId.HasValue ? (int)UserDetail.UserPracticeRecord.SpecialityId : 0;
                _BPRPExpireDate = UserDetail.UserPracticeRecord.BPRPExpiryDate.HasValue ? (DateTime)UserDetail.UserPracticeRecord.BPRPExpiryDate : default(DateTime);
                _DRPExpireDate = UserDetail.UserPracticeRecord.DRPExpiryDate.HasValue ? (DateTime)UserDetail.UserPracticeRecord.DRPExpiryDate : default(DateTime);
                _HSRPExpireDate = UserDetail.UserPracticeRecord.HSRPExpiryDate.HasValue ? (DateTime)UserDetail.UserPracticeRecord.HSRPExpiryDate : default(DateTime);
                _PracticeRoleId = UserDetail.UserPracticeRecord.PracticeRoleId.HasValue ? (int)UserDetail.UserPracticeRecord.PracticeRoleId : 0;
            }

        }

        public bool IsEmailValid(int enterpriseId)
        {
            try
            {
                int recordId = 0;

                // To add new user
                if (this.UserId == 0)
                {
                    recordId = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                join practiceRecord in BMTDataContext.Practices
                                   on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                   join medicalGroupRecord in BMTDataContext.MedicalGroups
                                   on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                join userRecord in BMTDataContext.Users
                                   on practiceUserRecord.UserId equals userRecord.UserId
                                where userRecord.Email == this.Email.ToLower()
                                && medicalGroupRecord.EnterpriseId == enterpriseId
                                && userRecord.Email.ToLower() == this.Email.ToLower()
                                select userRecord.UserId).FirstOrDefault();
                }
                // to update existing user
                else
                {
                    recordId = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                join practiceRecord in BMTDataContext.Practices
                                   on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                join medicalGroupRecord in BMTDataContext.MedicalGroups
                                on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                join userRecord in BMTDataContext.Users
                                   on practiceUserRecord.UserId equals userRecord.UserId
                                where userRecord.Email == this.Email.ToLower()
                                && medicalGroupRecord.EnterpriseId == enterpriseId
                                && userRecord.UserId != this.UserId
                                select userRecord.UserId).FirstOrDefault();
                }

                if (recordId <= 0)
                    return true;
                else
                    return false;

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public void changePassword(int userId, string password)
        {
            try
            {
                _userAccount = new User();

                var UserRecord = (from userRecord in BMTDataContext.Users
                                  where userRecord.UserId == userId
                                  select userRecord).SingleOrDefault();
                if (UserRecord != null)
                {
                    UserRecord.Password = password;
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        public List<string> GetAccountDetailByUserId(int userId)
        {

            try
            {
                List<string> _userDetails = new List<string>();
                var UserAccountRecord = (from practiceUser in BMTDataContext.PracticeUsers
                                         join practiceRecord in BMTDataContext.Practices
                                             on practiceUser.PracticeId equals practiceRecord.PracticeId
                                         join userRecord in BMTDataContext.Users
                                             on practiceUser.UserId equals userRecord.UserId
                                         where userRecord.UserId == userId
                                         select new { userRecord, practiceUser }).SingleOrDefault();
                if (UserAccountRecord != null)
                {
                    if (UserAccountRecord.practiceUser != null)
                    {
                        _userDetails.Add(UserAccountRecord.practiceUser.FirstName);
                    }
                    else
                    {
                        _userDetails.Add(DEFAULT_FIRST_NAME);
                    }
                    _userDetails.Add(UserAccountRecord.userRecord.Username);
                    _userDetails.Add(UserAccountRecord.userRecord.Password);
                    _userDetails.Add(UserAccountRecord.userRecord.UserId.ToString());

                    return _userDetails;
                }
                else { return null; }

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public IQueryable GetPrimaryCareProviderList(SiteBO _siteDetail)
        {
            try
            {

                var primaryCareProviderList = (from userRecord in BMTDataContext.Users
                                               join userPracticeRecord in BMTDataContext.PracticeUsers
                                                   on userRecord.UserId equals userPracticeRecord.UserId
                                               join siteRecord in BMTDataContext.PracticeSites
                                                   on userPracticeRecord.PracticeSiteId equals siteRecord.PracticeSiteId
                                               join specialityRecord in BMTDataContext.Specialities
                                                   on siteRecord.SpecialityId equals specialityRecord.SpecialityId
                                               join credentialRecord in BMTDataContext.Credentials
                                                   on userPracticeRecord.CredentialId equals credentialRecord.CredentialId
                                                       into credentialRecord_d
                                               where siteRecord.PracticeSiteId == _siteDetail.SiteId
                                               from credentialRecord in credentialRecord_d.DefaultIfEmpty()
                                               select new
                                               {
                                                   LastName = userPracticeRecord.LastName.ToString(),
                                                   FirstName = userPracticeRecord.FirstName.ToString(),
                                                   Credentials = credentialRecord.Name.ToString(),
                                                   Speciality = specialityRecord.Name.ToString(),
                                                   BPRP = userPracticeRecord.BPRPExpiryDate.HasValue ? "Exp. " + Convert.ToString(userPracticeRecord.BPRPExpiryDate.Value.Month) + "/" + Convert.ToString(userPracticeRecord.BPRPExpiryDate.Value.Day) + "/" + Convert.ToString(userPracticeRecord.BPRPExpiryDate.Value.Year) : "None",
                                                   DRP = userPracticeRecord.DRPExpiryDate.HasValue ? "Exp. " + Convert.ToString(userPracticeRecord.DRPExpiryDate.Value.Month) + "/" + Convert.ToString(userPracticeRecord.DRPExpiryDate.Value.Day) + "/" + Convert.ToString(userPracticeRecord.DRPExpiryDate.Value.Year) : "None",
                                                   HSRP = userPracticeRecord.HSRPExpiryDate.HasValue ? "Exp. " + Convert.ToString(userPracticeRecord.HSRPExpiryDate.Value.Month) + "/" + Convert.ToString(userPracticeRecord.HSRPExpiryDate.Value.Day) + "/" + Convert.ToString(userPracticeRecord.HSRPExpiryDate.Value.Year) : "None"

                                               });
                _primaryCareProviderList = primaryCareProviderList.AsQueryable();

                return _primaryCareProviderList;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public string GetUserCredentialByUserId(int userId)
        {
            try
            {

                var usercredential = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                      from credentialRecord in BMTDataContext.Credentials
                                      where (practiceUserRecord.CredentialId == credentialRecord.CredentialId
                                         && practiceUserRecord.UserId == userId)
                                      select new
                                      {
                                          credentialRecord.Name

                                      }).SingleOrDefault();

                if (usercredential != null)
                    return usercredential.Name;
                else
                    return string.Empty;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return null;
        }

        public List<string> GetUserName(int practiceId)
        {
            try
            {

                var userrow = (from UserRecord in BMTDataContext.PracticeUsers
                               where (UserRecord.PracticeId == practiceId)

                               select new
                               {

                                   UserRecord.FirstName,
                                   UserRecord.LastName,


                               }).SingleOrDefault();

                List<string> _UserList = new List<string>();
                if (userrow != null)
                {
                    _UserList.Add(userrow.FirstName);
                    _UserList.Add(userrow.LastName);
                }

                return _UserList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetUserDetailsByUserId(int userId)
        {
            try
            {
                string userInfo = string.Empty;
                var userDetails = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                    join siteRecord in BMTDataContext.PracticeSites
                                    on practiceUserRecord.PracticeSiteId equals siteRecord.PracticeSiteId
                                    join addressRecord in BMTDataContext.Addresses
                                     on siteRecord.AddressId equals addressRecord.AddressId
                                   where practiceUserRecord.UserId == userId
                                    select new
                                    {
                                        address = practiceUserRecord.FirstName + " " + practiceUserRecord.LastName + ", " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " "
                                        + (addressRecord.City.ToString().Trim() != string.Empty ? addressRecord.City.ToString() + ", " : string.Empty)
                                        + addressRecord.State.ToString()
                                    }).AsQueryable();                

                foreach (var item in userDetails)
                {
                    userInfo = item.address.ToString();
                }

                return userInfo;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsDisclaimerPassed(int userId)
        {
            try
            {
                var isDisclaimerPassed = (from userRecord in BMTDataContext.Users
                                          where (userRecord.UserId == userId)
                                          select userRecord.IsDisclaimerPass == null ? false : userRecord.IsDisclaimerPass).SingleOrDefault();

                return Convert.ToBoolean(isDisclaimerPassed);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsSRADisclaimerPassed(int userId)
        {
            try
            {
                var isSRADisclaimerPassed = (from userRecord in BMTDataContext.Users
                                          where (userRecord.UserId == userId)
                                          select userRecord.IsSRADisclaimerPass == null ? false : userRecord.IsSRADisclaimerPass).SingleOrDefault();

                return Convert.ToBoolean(isSRADisclaimerPassed);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateDisclaimerPassed(int userId)
        {
            try
            {
                var UserRecord = (from userRecord in BMTDataContext.Users
                                  where (userRecord.UserId == userId)
                                  select userRecord).SingleOrDefault();
                if (UserRecord != null)
                {
                    UserRecord.IsDisclaimerPass = true;
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateSRADisclaimerPassed(int userId)
        {
            try
            {
                var UserRecord = (from userRecord in BMTDataContext.Users
                                  where (userRecord.UserId == userId)
                                  select userRecord).SingleOrDefault();
                if (UserRecord != null)
                {
                    UserRecord.IsSRADisclaimerPass = true;
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}
