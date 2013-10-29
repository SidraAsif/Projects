#region Modification History

//  ******************************************************************************
//  Module        : UserAccount
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/09/2011
//  Description   : Account screens
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    12-09-2012      SignUp and login process with forgot password feature
//  Mirza Fahad Ali Baig    01-23-2012      Fix spaces between practice name
//  *******************************************************************************

#endregion

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Configuration;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class UserAccountBO : BMTConnection
    {

        #region PROPERTIES
        private string AssignUserName { get; set; }
        private int UserId { get; set; }
        private int AddressId { get; set; }
        private int PracticeId { get; set; }
        private int SiteId { get; set; }

        #endregion

        #region VARIABLE
        private User user;
        private Address address;
        private Practice practice;
        private PracticeSite practiceSite;
        private PracticeUser practiceUser;
        private Project project;
        private Email email;
        private Security security;
        private LoginRecord loginRecord;

        #endregion

        #region CONSTRUCTOR
        public UserAccountBO()
        { }

        #endregion

        #region FUNCTIONS
        public int SignUp(UserBO userDetail, PracticeSizeBO practiceSizeDetail, SpecialityBO specialityDetail,
            PracticeBO practiceDetail, SiteBO siteDetail, ProjectBO projectDetail, int enterpriseId,int medicalGroupId)
        {
            try
            {
                user = new User();
                practice = new Practice();
                practiceSite = new PracticeSite();
                practiceUser = new PracticeUser();
                project = new Project();
                email = new Email();
                security = new Security();

                using (var scope = new System.Transactions.TransactionScope())
                {
                    AssignUserName = userDetail.FistName + " " + userDetail.LastName + " " + "Practice";

                    // Create User Account 
                    user.Username = userDetail.UserName;
                    user.Password = userDetail.Password;
                    user.Email = userDetail.Email;
                    user.IsActive = userDetail.IsActive;
                    user.CreatedDate = userDetail.CreatedDate;
                    user.RoleId = userDetail.RoleId;
                    BMTDataContext.Users.InsertOnSubmit(user);
                    BMTDataContext.SubmitChanges();

                    // Get UserId
                    UserId = user.UserId;

                    // Create Default Address Information for Practice
                    AddAddress(userDetail);

                    // Create Default Practice
                    practice.MedicalGroupId = practiceDetail.MedicalGroupId;
                    practice.Name = AssignUserName;
                    practice.AddressId = AddressId;
                    practice.PracticeSizeId = practiceSizeDetail.PracriceSizeId;
                    practice.SpecialityId = specialityDetail.SpecialityId;
                    practice.CreatedDate = practiceDetail.CreatedDate;
                    practice.CreatedBy = UserId;
                    BMTDataContext.Practices.InsertOnSubmit(practice);
                    BMTDataContext.SubmitChanges();

                    // Get PracticeId
                    PracticeId = practice.PracticeId;

                    // Create Default Address Information for Site
                    AddAddress(userDetail);

                    // Create Default Site against the practice
                    practiceSite.PracticeId = PracticeId;
                    practiceSite.Name = AssignUserName;
                    practiceSite.AddressId = AddressId;
                    practiceSite.SpecialityId = specialityDetail.SpecialityId;
                    practiceSite.IsMainSite = siteDetail.IsMainSite;
                    practiceSite.CreatedDate = siteDetail.CreatedDate;
                    practiceSite.CreatedBy = UserId;
                    BMTDataContext.PracticeSites.InsertOnSubmit(practiceSite);
                    BMTDataContext.SubmitChanges();

                    // Getting Site Id

                    SiteId = practiceSite.PracticeSiteId;
                    List<Project> projec = (from projRec in BMTDataContext.Projects
                                            join projAssignment in BMTDataContext.ProjectAssignments
                                            on projRec.ProjectId equals projAssignment.ProjectId
                                            where projRec.AccessLevelId == (int)enAccessLevelId.MedicalGroup &&
                                            projAssignment.MedicalGroupId == medicalGroupId
                                            select projRec).ToList();

                    foreach (Project proj in projec)
                    {
                        ProjectUsage pUsage = new ProjectUsage();
                        pUsage.ProjectId = proj.ProjectId;
                        pUsage.PracticeId = PracticeId;
                        pUsage.IsVisible = true;
                        BMTDataContext.ProjectUsages.InsertOnSubmit(pUsage);
                        BMTDataContext.SubmitChanges();
                    }

                    // Create Default Project for site
                    List<Project> projs = (from projRec in BMTDataContext.Projects
                                           where projRec.AccessLevelId == (int)enAccessLevelId.Public
                                           select projRec).ToList();
                    foreach (Project proj in projs)
                    {
                        ProjectUsage pUsage = new ProjectUsage();
                        pUsage.ProjectId = proj.ProjectId;
                        pUsage.PracticeId = PracticeId;
                        if(proj.ProjectId==(int)enProjectId.PCMHProject)
                            pUsage.IsVisible = true;
                        BMTDataContext.ProjectUsages.InsertOnSubmit(pUsage);
                        BMTDataContext.SubmitChanges();
                    }

                    List<Project> projects = (from projRec in BMTDataContext.Projects
                                               join projAssignment in BMTDataContext.ProjectAssignments
                                               on projRec.ProjectId equals projAssignment.ProjectId
                                              where projRec.AccessLevelId == (int)enAccessLevelId.Enterprise &&
                                               projAssignment.EnterpriseId == enterpriseId
                                               select projRec).ToList();

                    foreach (Project proj in projects)
                    {
                        ProjectUsage pUsage = new ProjectUsage();
                        pUsage.ProjectId = proj.ProjectId;
                        pUsage.PracticeId = PracticeId;
                        pUsage.IsVisible = true;
                        BMTDataContext.ProjectUsages.InsertOnSubmit(pUsage);
                        BMTDataContext.SubmitChanges();
                    }
                    //project.PracticeId = PracticeId;
                    //project.PracticeSiteId = SiteId;
                    //project.Name = projectDetail.Name;
                    //project.Description = projectDetail.Description;
                    //project.CreatedOn = projectDetail.CreatedDate;
                    //project.CreatedBy = UserId;
                    //BMTDataContext.Projects.InsertOnSubmit(project);
                    //BMTDataContext.SubmitChanges();

                    // At the end, Create PracticeUser information
                    practiceUser.UserId = UserId;
                    practiceUser.FirstName = userDetail.FistName;
                    practiceUser.LastName = userDetail.LastName;
                    practiceUser.SpecialityId = specialityDetail.SpecialityId;
                    practiceUser.CreatedDate = userDetail.CreatedDate;
                    practiceUser.CreatedBy = UserId;
                    practiceUser.PracticeId = PracticeId;
                    practiceUser.PracticeSiteId = SiteId;
                    BMTDataContext.PracticeUsers.InsertOnSubmit(practiceUser);

                    BMTDataContext.SubmitChanges();

                    SystemInfoBO sysInfo = new SystemInfoBO();
                    string body = sysInfo.GetSystemInfoByKey(enterpriseId, enSystemInfo.body);
                    string Project = sysInfo.GetSystemInfoByKey(enterpriseId, enSystemInfo.projectName);
                    string Company = sysInfo.GetSystemInfoByKey(enterpriseId, enSystemInfo.companyName);

                    // Send Credential
                    email.SendUserCredentialDetails
                        (
                          userDetail.Email,
                          userDetail.UserName,
                          security.Decrypt(userDetail.Password),
                          Project,
                          Company,
                          body,
                          UserId
                        );

                    // Track login record
                    TrackLogin(UserId);

                    // Complete Transaction
                    scope.Complete();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return UserId;
        }

        private void AddAddress(UserBO userDetail)
        {

            try
            {
                address = new Address();
                address.PrimaryAddress = userDetail.PrimaryAddress;
                address.City = userDetail.City;
                address.State = userDetail.State;
                address.ZipCode = userDetail.ZipCode;
                address.Telephone = userDetail.Phone;
                address.Email = userDetail.Email;

                BMTDataContext.Addresses.InsertOnSubmit(address);
                BMTDataContext.SubmitChanges();

                /* Get Address Id for Practice */
                AddressId = address.AddressId;


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public int Login(UserBO userDetail, int enterpriseId)
        {

            try
            {
                UserId = 0;

                var userAccountRecord = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                         join practiceRecord in BMTDataContext.Practices
                                            on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                         join userRecord in BMTDataContext.Users
                                            on practiceUserRecord.UserId equals userRecord.UserId
                                         join medicalGroupRecord in BMTDataContext.MedicalGroups
                                         on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                         where (userRecord.Username == userDetail.UserName)
                                            && (userRecord.Password == userDetail.Password)
                                            && (userRecord.IsActive == true)
                                            && (medicalGroupRecord.EnterpriseId == enterpriseId)
                                         select new { userRecord.UserId }).SingleOrDefault();

                if (userAccountRecord == null)
                { UserId = 0; }
                else
                {
                    UserId = userAccountRecord.UserId;
                    /*Track login records*/
                    TrackLogin(UserId);
                }
                return UserId;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        private void TrackLogin(int _userId)
        {
            try
            {
                loginRecord = new LoginRecord();
                DateTime loginTime = System.DateTime.Now;

                loginRecord.UserId = _userId;
                loginRecord.LoginTime = loginTime;
                BMTDataContext.LoginRecords.InsertOnSubmit(loginRecord);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public List<string> GetUserLoginInfo(string emailAddress, int enterpriseId)
        {
            try
            {
                List<string> _credentialList = new List<string>();

                var userRecordRow = (from practiceUserRecord in BMTDataContext.PracticeUsers
                                     join practiceRecord in BMTDataContext.Practices
                                        on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                     join userRecord in BMTDataContext.Users
                                        on practiceUserRecord.UserId equals userRecord.UserId
                                     join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                     where userRecord.Email == emailAddress
                                     && medicalGroupRecord.EnterpriseId == enterpriseId
                                     select userRecord).SingleOrDefault();


                if (userRecordRow != null)
                {
                    _credentialList.Add(userRecordRow.Username);
                    _credentialList.Add(userRecordRow.Password);
                    _credentialList.Add(userRecordRow.UserId.ToString());
                }


                return _credentialList;
            }
            catch (Exception exception)
            { throw exception; }

        }

        public List<string> GetUserDetailById(int userId)
        {
            try
            {
                var userRow = (from userRecord in BMTDataContext.Users
                               join practiceUserRecord in BMTDataContext.PracticeUsers
                                on userRecord.UserId equals practiceUserRecord.UserId
                                into practiceUserRecord_d
                               join UserRoleRecord in BMTDataContext.Roles
                                on userRecord.RoleId equals UserRoleRecord.RoleId
                               from practiceUserRecord in practiceUserRecord_d.DefaultIfEmpty()
                               where userRecord.UserId == userId
                               select new
                               {
                                   userRecord.Username,
                                   practiceUserRecord.PracticeId,
                                   RoleType = UserRoleRecord.Name,
                                   userRecord.Email
                               }).SingleOrDefault();

                // Create list to store the required details of user
                List<string> CredentialList = new List<string>();
                CredentialList.Add(userRow.Username);
                CredentialList.Add(userRow.PracticeId.ToString());
                CredentialList.Add(userRow.RoleType);
                CredentialList.Add(userRow.Email);

                return CredentialList;
            }
            catch (Exception exception)
            { throw exception; }
        }

        public bool EmailExist(string emailAddress, int enterpriseId)
        {
            try
            {
                user = new User();
                var userRow = (from practiceUserRecord in BMTDataContext.PracticeUsers
                               join practiceRecord in BMTDataContext.Practices
                                  on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                               join userRecord in BMTDataContext.Users
                                  on practiceUserRecord.UserId equals userRecord.UserId
                               join medicalGroupRecord in BMTDataContext.MedicalGroups
                               on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                               where userRecord.Email == emailAddress
                               && medicalGroupRecord.EnterpriseId == enterpriseId
                               select new { userRecord.Email }).SingleOrDefault();

                if (userRow != null)
                { return false; }
                else
                { return true; }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool UserNameExist(string username)
        {
            try
            {
                user = new User();
                var userRow = (from userRecord in BMTDataContext.Users
                               where userRecord.Username == username
                               select new { userRecord.Username }).SingleOrDefault();

                if (userRow != null)
                    return false;
                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetUserInformation(int userApplicationId)
        {
            try
            {
                var userRow = (from userReocord in BMTDataContext.Users
                               join practiceUserRecord in BMTDataContext.PracticeUsers
                                on userReocord.UserId equals practiceUserRecord.UserId
                               join practiceRecord in BMTDataContext.Practices
                                on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                               where userReocord.UserId == userApplicationId
                               select new
                               {
                                   userReocord,
                                   practiceUserRecord,
                                   practiceRecord
                               }).SingleOrDefault();

                List<string> _credentialList = new List<string>();
                if (userRow != null)
                {
                    _credentialList.Add(userRow.userReocord.UserId.ToString());
                    _credentialList.Add(userRow.userReocord.Email);
                    _credentialList.Add(userRow.practiceUserRecord.FirstName);
                    _credentialList.Add(userRow.practiceUserRecord.LastName);
                    _credentialList.Add(userRow.practiceRecord.Name);
                }
                return _credentialList;
            }
            catch (Exception exception)
            { throw exception; }

        }

        public int GetEnterpriseId()
        {
            try
            {
                int enterpriseId = (from enterpriseRecord in BMTDataContext.Enterprises
                                    where enterpriseRecord.Name == ConfigurationManager.AppSettings["EnterpriseName"].ToString()
                                    select enterpriseRecord.EnterpriseId).SingleOrDefault();

                return enterpriseId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
