#region Modification History

//  ******************************************************************************
//  Module        : User Administration
//  Created By    : N/A
//  When Created  : N/A
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                         Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig        02-01-2012     Modify Getpractice function 
//  Mirza Fahad Ali Baig        02-01-2012     change getpractice name (apply pascal casing in getPractice)
//  Mirza Fahad Ali Baig        02-01-2012     change getUser name (apply pascal casing in getUser)
//  Mirza Fahad Ali Baig        02-01-2012     Add regions
//  Mirza Fahad Ali Baig        02-01-2012     Add Error Handling
//  Mirza Fahad Ali Baig        02-01-2012     remove extra spaces between functions
//  Mirza Fahad Ali Baig        02-01-2012     Change GetPracticeRecord function name with GetPracticeListByUserId
//  Mirza Fahad Ali Baig        02-01-2012     Change DataHandler function name with AssignPractices
//  Mirza Fahad Ali Baig        03-09-2012     Fix Variable naming and function logic in GetConsultantUserbyId function
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using BMTBLL.Enumeration;
namespace BMTBLL
{
    public class ConsultingUserBO : BMTConnection
    {
        #region FUNCTIONS
        
        public IQueryable GetPracticeList(int enterpriseId)
        {
            try
            {
                IQueryable List = (from userRecord in BMTDataContext.Users
                                   join roleRecord in BMTDataContext.Roles
                                        on userRecord.RoleId equals roleRecord.RoleId
                                   join practiceUserRecord in BMTDataContext.PracticeUsers
                                       on userRecord.UserId equals practiceUserRecord.UserId
                                   join practiceRecord in BMTDataContext.Practices
                                        on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                                   join medicalGroupRecord in BMTDataContext.MedicalGroups
                                        on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                   where roleRecord.RoleId == (int)enUserRole.User
                                        && medicalGroupRecord.EnterpriseId == enterpriseId
                                   group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                                   orderby practiceGroup.Key.Name ascending
                                   select new
                                   {
                                       ID = practiceGroup.Key.PracticeId,
                                       Name = practiceGroup.Key.Name
                                   }) as IQueryable;

                return List;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public IQueryable GetPracticeListByUserId(int UserId, int enterpriseId)
        {
            try
            {

                var List = (from userRecord in BMTDataContext.Users
                            join roleRecord in BMTDataContext.Roles
                                 on userRecord.RoleId equals roleRecord.RoleId
                            join practiceUserRecord in BMTDataContext.PracticeUsers
                                on userRecord.UserId equals practiceUserRecord.UserId
                            join practiceRecord in BMTDataContext.Practices
                                 on practiceUserRecord.PracticeId equals practiceRecord.PracticeId
                            join medicalGroupRecord in BMTDataContext.MedicalGroups
                                 on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                            where roleRecord.RoleId == (int)enUserRole.User
                                 && medicalGroupRecord.EnterpriseId == enterpriseId
                            group practiceRecord by new { practiceRecord.PracticeId, practiceRecord.Name } into practiceGroup
                            orderby practiceGroup.Key.Name ascending
                            select new
                            {
                                ID = practiceGroup.Key.PracticeId,
                                Name = practiceGroup.Key.Name
                            });

                var list2 = (from lst in List
                             where !(from pConst in BMTDataContext.PracticeConsultants
                                     where pConst.ConsultantId == UserId
                                     select pConst.PracticeId).Contains(lst.ID)
                             select lst);


                return list2 as IQueryable;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public IQueryable GetAllPracticeList()
        {
            try
            {
                IQueryable List = (from practice in BMTDataContext.Practices
                                   select new
                                   {
                                       ID = practice.PracticeId,
                                       Name = practice.Name
                                   }) as IQueryable;

                return List;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool AssignPractices(int userId, List<int> practiceId)
        {
            if (userId != 0)
            {
                System.Data.Common.DbTransaction transaction;
                if (BMTDataContext.Connection.State == ConnectionState.Open)
                    BMTDataContext.Connection.Close();
                BMTDataContext.Connection.Open();
                transaction = BMTDataContext.Connection.BeginTransaction();
                BMTDataContext.Transaction = transaction;
                try
                {
                    // Deleting previous records 
                    var detail = (from list in BMTDataContext.ConsultingUserAccesses
                                  where list.UserId == userId
                                  select list);

                    foreach (var del in detail)
                        BMTDataContext.ConsultingUserAccesses.DeleteOnSubmit(del);

                    // Inserting new records 
                    foreach (int list in practiceId)
                    {
                        ConsultingUserAccess consultingUser = new ConsultingUserAccess();
                        consultingUser.UserId = userId;
                        consultingUser.PracticeId = list;
                        BMTDataContext.ConsultingUserAccesses.InsertOnSubmit(consultingUser);
                        BMTDataContext.SubmitChanges();
                    }

                    BMTDataContext.SubmitChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }

            }

            return false;
        }
                           
        public bool isPracticeWithConsultant(int practiceId)
        {
            try
            {
                var record = (from practice in BMTDataContext.ConsultingUserAccesses
                              where practice.PracticeId == practiceId
                              select practice.PracticeId).ToList();

                if (record.Count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ConsultingUserDetail> GetConsultants(int enterpriseId)
        {

            try
            {
                List<ConsultingUserDetail> cUserDetail = null;
                cUserDetail = (from pUser in BMTDataContext.PracticeUsers
                               join user in BMTDataContext.Users on pUser.UserId equals user.UserId
                               join cUser in BMTDataContext.ConsultantUsers on pUser.UserId equals cUser.UserId
                               join cType in BMTDataContext.ConsultantTypes on cUser.ConsultantTypeId equals cType.ConsultantTypeId
                               join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                               join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                               orderby pUser.FirstName, pUser.LastName
                               where medicalGroup.EnterpriseId == enterpriseId

                               select new ConsultingUserDetail
                               {
                                   UserID = pUser.UserId,
                                   LastName = pUser.LastName,
                                   FirstName = pUser.FirstName,
                                   UserName = user.Username,
                                   Organization = cUser.Organization,
                                   ConsultantTypeName = cType.Name,
                                   IsActive = (user.IsActive == true) ? "Active" : "Inactive"
                               }
                    ).ToList();

                return cUserDetail;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public ConsultantAdministrationDetail GetConsultantDetailByUserId(int UserId)
        {

            try
            {
                ConsultantAdministrationDetail cUserDetail = null;
                cUserDetail = (from cUser in BMTDataContext.ConsultantUsers
                               join address in BMTDataContext.Addresses on cUser.AddressId equals address.AddressId into tempAddress
                               from tempTableAddress in tempAddress.DefaultIfEmpty()
                               join pUser in BMTDataContext.PracticeUsers on cUser.UserId equals pUser.UserId into tempPUser
                               from tempTablePUser in tempPUser.DefaultIfEmpty()
                               join user in BMTDataContext.Users on cUser.UserId equals user.UserId into tempUser
                               from tempTableUser in tempUser.DefaultIfEmpty()

                               where tempTablePUser.UserId == UserId

                               select new ConsultantAdministrationDetail
                               {
                                   UserId = tempTablePUser.UserId,
                                   LastName = tempTablePUser.LastName,
                                   FirstName = tempTablePUser.FirstName,
                                   UserName = tempTableUser.Username,
                                   Password = tempTableUser.Password,
                                   IsActive = tempTableUser.IsActive,
                                   Email = tempTableUser.Email,
                                   Telephone = tempTableAddress.Telephone,
                                   ConsultantTypeId = Convert.ToInt32(cUser.ConsultantTypeId),
                                   ServiceArea = cUser.ServiceArea,
                                   Featured = Convert.ToBoolean(cUser.Featured),
                                   Website = cUser.Website,
                                   logoPath = cUser.LogoPath,
                                   Organization = cUser.Organization,
                                   PrimaryAddress = tempTableAddress.PrimaryAddress,
                                   SecondaryAddress = tempTableAddress.SecondaryAddress,
                                   City = tempTableAddress.City,
                                   State = tempTableAddress.State,
                                   ZipCode = tempTableAddress.ZipCode
                               }
                    ).FirstOrDefault();

                return cUserDetail;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public List<ConsultantType> GetConsultantType()
        {

            try
            {

                List<ConsultantType> cType =
                    (from type in BMTDataContext.ConsultantTypes
                     select type).ToList();



                return cType;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Practice> GetPracticeConsultant(int UserId)
        {

            try
            {
                //List<Practice> pCons = (from con in BMTDataContext.PracticeConsultants
                //                          join p in BMTDataContext.Practices on con.PracticeId equals p.PracticeId
                //                          where con.ConsultantId == UserId
                //                        select p).ToList();

                List<Practice> pCons = (from con in BMTDataContext.ConsultingUserAccesses
                                        join p in BMTDataContext.Practices on con.PracticeId equals p.PracticeId
                                        where con.UserId == UserId
                                        select p).ToList();

                return pCons;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Practice> GetAllPracticeListByUserId(int UserId)
        {

            try
            {

                List<Practice> pCons = (from p in BMTDataContext.Practices

                                        where !(from con in BMTDataContext.PracticeConsultants
                                                where con.ConsultantId == UserId
                                                select con.PracticeId).Contains(p.PracticeId)
                                        select p).ToList();


                return pCons;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsEmailAvailable(string EmailAddress, int enterpriseId)
        {

            try
            {
                List<User> users = (from user in BMTDataContext.Users
                                    join pUser in BMTDataContext.PracticeUsers on user.UserId equals pUser.UserId
                                    join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                                    join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                                    where user.Email == EmailAddress
                                    && medicalGroup.EnterpriseId == enterpriseId
                                    && user.RoleId == (int)enUserRole.Consultant
                                    select user).ToList();
                if (users.Count != 0)
                    return false;

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public bool IsUserNameAvailable(string userName, int enterpriseId)
        {

            try
            {
                List<User> users = (from user in BMTDataContext.Users
                                    join pUser in BMTDataContext.PracticeUsers on user.UserId equals pUser.UserId
                                    join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                                    join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                                    where user.Username == userName
                                    && medicalGroup.EnterpriseId == enterpriseId
                                    select user).ToList();
                if (users.Count != 0)
                    return false;

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public bool IsUserNameAvailableForEdit(int UserId, string userName, int enterpriseId)
        {

            try
            {
                var users = (from user in BMTDataContext.Users
                             join pUser in BMTDataContext.PracticeUsers on user.UserId equals pUser.UserId
                             join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                             join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                             where user.Username == userName
                             && medicalGroup.EnterpriseId == enterpriseId
                             && user.UserId != UserId
                             select user).ToList();

                if (users.Count != 0)
                    return false;

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public bool IsEmailAvailableForEdit(int UserId, string emailAddress, int enterpriseId)
        {

            try
            {
                var users = (from user in BMTDataContext.Users
                             join pUser in BMTDataContext.PracticeUsers on user.UserId equals pUser.UserId
                             join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                             join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                             where user.Email == emailAddress
                             && medicalGroup.EnterpriseId == enterpriseId
                             && user.UserId != UserId
                             && user.RoleId == (int)enUserRole.Consultant
                             select user).ToList();

                if (users.Count != 0)
                    return false;

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateRecord(int UserId, string LastName, string FirstName, string UserName, string Email, string Phone, int ConsultantTypeId, string ServiceArea, bool Featured, bool IsActive,
            string Website, string LogoPath, string Organization, string PrimaryAddress, string SecondaryAddress, string City, string State, string Zip, List<int> PracticeId)
        {
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();

            BMTDataContext.Connection.Open();
            System.Data.Common.DbTransaction transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            try
            {
                ConsultantUser cUser = (from user in BMTDataContext.ConsultantUsers
                                        where user.UserId == UserId
                                        select user).FirstOrDefault();

                cUser.ConsultantTypeId = ConsultantTypeId;
                cUser.Featured = Featured;
                cUser.Website = Website;

                if (LogoPath != string.Empty)
                    cUser.LogoPath = LogoPath;

                cUser.Organization = Organization;
                cUser.ServiceArea = ServiceArea;
                BMTDataContext.SubmitChanges();


                User users = (from usrs in BMTDataContext.Users where usrs.UserId == UserId select usrs).FirstOrDefault();

                users.Username = UserName;
                users.IsActive = IsActive;
                users.Email = Email;

                BMTDataContext.SubmitChanges();

                PracticeUser pUser = (from pu in BMTDataContext.PracticeUsers where pu.UserId == UserId select pu).FirstOrDefault();

                pUser.FirstName = FirstName;
                pUser.LastName = LastName;

                BMTDataContext.SubmitChanges();

                if (cUser.AddressId != 0)
                {
                    Address adres = (from ad in BMTDataContext.Addresses where ad.AddressId == cUser.AddressId select ad).FirstOrDefault();

                    adres.PrimaryAddress = PrimaryAddress;
                    adres.SecondaryAddress = SecondaryAddress;
                    adres.City = City;
                    adres.State = State;
                    adres.Telephone = Phone;
                    adres.ZipCode = Zip;

                    BMTDataContext.SubmitChanges();
                }
                else
                {
                    Address Adress = new Address();
                    Adress.PrimaryAddress = PrimaryAddress;
                    Adress.SecondaryAddress = SecondaryAddress;
                    Adress.City = City;
                    Adress.State = State;
                    Adress.Telephone = Phone;
                    Adress.ZipCode = Zip;
                    Adress.Email = Email;

                    BMTDataContext.Addresses.InsertOnSubmit(Adress);
                    BMTDataContext.SubmitChanges();

                    Address adr = (from ad in BMTDataContext.Addresses
                                   where ad.PrimaryAddress == Adress.PrimaryAddress
                                       && ad.Email == Adress.Email
                                   select ad).FirstOrDefault();

                    cUser.AddressId = adr.AddressId;
                    BMTDataContext.SubmitChanges();
                }



                List<PracticeConsultant> oldList = (from list in BMTDataContext.PracticeConsultants
                                                    where list.ConsultantId == UserId
                                                    select list).ToList();

                BMTDataContext.PracticeConsultants.DeleteAllOnSubmit(oldList);
                BMTDataContext.SubmitChanges();

                var detail = (from list in BMTDataContext.ConsultingUserAccesses
                              where list.UserId == UserId
                              select list);

                BMTDataContext.ConsultingUserAccesses.DeleteAllOnSubmit(detail);
                BMTDataContext.SubmitChanges();


                foreach (int _practiceId in PracticeId)
                {
                    PracticeConsultant pConsultant = new PracticeConsultant();
                    pConsultant.PracticeId = _practiceId;
                    pConsultant.ConsultantId = UserId;
                    pConsultant.RelationshipId = 1;
                    BMTDataContext.PracticeConsultants.InsertOnSubmit(pConsultant);

                    ConsultingUserAccess cUserAccess = new ConsultingUserAccess();
                    cUserAccess.UserId = UserId;
                    cUserAccess.PracticeId = _practiceId;
                    BMTDataContext.ConsultingUserAccesses.InsertOnSubmit(cUserAccess);

                    BMTDataContext.SubmitChanges();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }


        public bool InsertRecord(int enterpriseId, string LastName, string FirstName, string UserName, string Password, string Email, string Phone, int ConsultantTypeId,
           string ServiceArea, bool Featured, bool IsActive, string Website, string LogoPath, string Organization, string PrimaryAddress, string SecondaryAddress, string City,
            string State, string Zip, List<int> PracticeId, int currentUserId)
        {


            System.Data.Common.DbTransaction transaction;
            if (BMTDataContext.Connection.State == ConnectionState.Open)
                BMTDataContext.Connection.Close();

            BMTDataContext.Connection.Open();
            transaction = BMTDataContext.Connection.BeginTransaction();
            BMTDataContext.Transaction = transaction;
            try
            {
                int pracId = GetSuperUserPracticeIdByEnterpriseId(enterpriseId);
                int pracSiteId = (from practiceSite in BMTDataContext.PracticeSites
                                  where practiceSite.PracticeId == pracId
                                  select practiceSite.PracticeSiteId).FirstOrDefault();
               

                User user = new User();
                user.Username = UserName;
                user.Password = Password;
                user.Email = Email;
                user.IsActive = IsActive;
                user.CreatedDate = System.DateTime.Now;
                user.RoleId = 3;

                BMTDataContext.Users.InsertOnSubmit(user);
                BMTDataContext.SubmitChanges();
                

                PracticeUser pUser = new PracticeUser();
                pUser.FirstName = FirstName;
                pUser.LastName = LastName;
                pUser.CreatedDate = System.DateTime.Now;
                pUser.UserId = user.UserId;
                pUser.CreatedBy = currentUserId;
                pUser.PracticeId = pracId;
                pUser.PracticeSiteId = pracSiteId;

                BMTDataContext.PracticeUsers.InsertOnSubmit(pUser);
                BMTDataContext.SubmitChanges();
                

                Address adress = new Address();
                adress.PrimaryAddress = PrimaryAddress;
                adress.SecondaryAddress = SecondaryAddress;
                adress.City = City;
                adress.State = State;
                adress.ZipCode = Zip;
                adress.Telephone = Phone;
                adress.Email = Email;

                BMTDataContext.Addresses.InsertOnSubmit(adress);
                BMTDataContext.SubmitChanges();   
             

                ConsultantUser cUser = new ConsultantUser();
                cUser.UserId = user.UserId;
                cUser.ConsultantTypeId = ConsultantTypeId;
                cUser.Featured = Featured;
                cUser.Website = Website;

                if (LogoPath != string.Empty)
                    cUser.LogoPath = LogoPath;

                cUser.Organization = Organization;
                cUser.AddressId = adress.AddressId;
                cUser.ServiceArea = ServiceArea;
                BMTDataContext.ConsultantUsers.InsertOnSubmit(cUser);
                BMTDataContext.SubmitChanges();


                foreach (int _practiceId in PracticeId)
                {
                    PracticeConsultant pConsultant = new PracticeConsultant();
                    pConsultant.PracticeId = _practiceId;
                    pConsultant.ConsultantId = user.UserId;
                    pConsultant.RelationshipId = 1;
                    BMTDataContext.PracticeConsultants.InsertOnSubmit(pConsultant);


                    ConsultingUserAccess cUserAccess = new ConsultingUserAccess();
                    cUserAccess.UserId = user.UserId;
                    cUserAccess.PracticeId = _practiceId;
                    BMTDataContext.ConsultingUserAccesses.InsertOnSubmit(cUserAccess);

                    BMTDataContext.SubmitChanges();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }


        public List<ConsultantAdministrationDetail> GetFacilitators(int _practiceId)
        {

            try
            {

                List<ConsultantAdministrationDetail> facilitatorsList = (from consultingUserRecords in BMTDataContext.ConsultingUserAccesses
                                                                         join userRecords in BMTDataContext.Users
                                                                            on consultingUserRecords.UserId equals userRecords.UserId
                                                                         join practiceUserRecord in BMTDataContext.PracticeUsers
                                                                            on userRecords.UserId equals practiceUserRecord.UserId
                                                                         where consultingUserRecords.PracticeId == _practiceId
                                                                         && userRecords.IsActive == true
                                                                         select new ConsultantAdministrationDetail
                                                                           {
                                                                               FirstName = practiceUserRecord.FirstName,
                                                                               LastName = practiceUserRecord.LastName,
                                                                               Email = userRecords.Email
                                                                           }).ToList();
                return facilitatorsList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetSuperUserPracticeIdByEnterpriseId(int enterpriseId)
        {
            try
            {
                int practiceID = (from pUsers in BMTDataContext.PracticeUsers
                                  join practice in BMTDataContext.Practices on pUsers.PracticeId equals practice.PracticeId
                                  join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                                  join usr in BMTDataContext.Users on pUsers.UserId equals usr.UserId
                                  join role in BMTDataContext.Roles on usr.RoleId equals role.RoleId
                                  where medicalGroup.EnterpriseId == enterpriseId 
                                  && role.Name == enUserRole.SuperUser.ToString()
                                  select practice.PracticeId).FirstOrDefault();

                return practiceID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
