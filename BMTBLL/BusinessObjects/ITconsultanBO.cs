using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BMTBLL
{
    public class ITconsultanBO : BMTConnection
    {
        public List<ConsultantDetails> GetFeaturedConsultants(int enterpriseId)
        {
            try
            {

                List<ConsultantDetails> detail = null;
                detail = (from consultant in BMTDataContext.ConsultantUsers
                          join address in BMTDataContext.Addresses on consultant.AddressId equals address.AddressId
                          join pUser in BMTDataContext.PracticeUsers on consultant.UserId equals pUser.UserId
                          join user in BMTDataContext.Users on consultant.UserId equals user.UserId
                          join practice in BMTDataContext.Practices on pUser.PracticeId equals practice.PracticeId
                          join medicalGroup in BMTDataContext.MedicalGroups on practice.MedicalGroupId equals medicalGroup.MedicalGroupId
                          where consultant.Featured == true 
                          && user.IsActive == true
                          && medicalGroup.EnterpriseId == enterpriseId 
                          && consultant.ConsultantTypeId == 3
                          select new ConsultantDetails
                          {
                              UserID = consultant.UserId,
                              LogoPath = consultant.LogoPath,
                              CompanyName = consultant.Organization,
                              City = address.City,
                              State = address.State,
                              ServiceArea = consultant.ServiceArea,
                              Phone = address.Telephone,
                              Email = address.Email,
                              Website = consultant.Website
                              //Relationship = (temptable.RelationshipId.ToString() != DBNull.Value.ToString()) ? Convert.ToInt32(temptable.RelationshipId) : 0,
                              //PracticeID = (temptable.PracticeId.ToString() != DBNull.Value.ToString()) ? temptable.PracticeId : 0
                          }).ToList();

                return detail;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        public PracticeConsultant GetPracticeAndRelationship(int PracticeId)
        {

            try
            {
                PracticeConsultant pCon = (from pc in BMTDataContext.PracticeConsultants where pc.PracticeId == PracticeId select pc).FirstOrDefault();
            return pCon;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }


        }


        public List<Relationship> GetRelationship()
        {

            try
            {
                List<Relationship> relation = (from rel in BMTDataContext.Relationships
                                               select rel).ToList();
                return relation;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void UpdatePracticeConsultant(int PracticeConsultantId,int ConsultantId, int RelationshipId,XElement xml)
        {

            try
            {
                PracticeConsultant query = (from cons in BMTDataContext.PracticeConsultants
                                            where cons.PracticeConsultantId == PracticeConsultantId
                                            select cons).FirstOrDefault();


                ConsultingUserAccess uAccess = (from user in BMTDataContext.ConsultingUserAccesses
                                                where user.UserId == query.ConsultantId && user.PracticeId == query.PracticeId
                                                select user).FirstOrDefault();

                if (RelationshipId == 0)
                    query.RelationshipId = null;
                
                else
                    query.RelationshipId = RelationshipId;


                if (ConsultantId == 0)
                    query.ConsultantId = null;
                else
                {
                    query.ConsultantId = ConsultantId;
                    uAccess.UserId = ConsultantId;
                }

                if (xml == null)
                    query.XmlData = null;
                else
                    query.XmlData = xml;

               
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        public void InsertPracticeConsultant(int PracticeId, int ConsultantId, int RelationshipId, XElement xml)
        {

            try
            {
                PracticeConsultant query = new PracticeConsultant();

                if (RelationshipId != 0)
                    query.RelationshipId = RelationshipId;
                else
                    query.RelationshipId = null;

                if (ConsultantId != 0)
                    query.ConsultantId = ConsultantId;
                else
                    query.ConsultantId = null;

                if (xml != null)
                    query.XmlData = xml;
                else
                    query.XmlData = null;

                if (PracticeId != 0)
                    query.PracticeId = PracticeId;

                BMTDataContext.PracticeConsultants.InsertOnSubmit(query);

                ConsultingUserAccess userAccess = new ConsultingUserAccess();
                userAccess.PracticeId = PracticeId;
                userAccess.UserId = ConsultantId;

                BMTDataContext.ConsultingUserAccesses.InsertOnSubmit(userAccess);

                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        

        public int GetUserIdByPracticeId(int PracticeId)
        {

            try
            {

                int pConst = Convert.ToInt32((from cons in BMTDataContext.PracticeConsultants
                                              where cons.PracticeId == PracticeId
                                              select cons.ConsultantId).FirstOrDefault());

                return pConst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PracticeConsultant GetPracticeConsultantByPracticeId(int PracticeId)
        {

            try
            {
                PracticeConsultant pConst = new PracticeConsultant();

                pConst = (from cons in BMTDataContext.PracticeConsultants
                          where cons.PracticeId == PracticeId
                          select cons).FirstOrDefault();

                return pConst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CheckNullData(int PracticeId)
        {

            try
            {

                if (PracticeId != 0)
                {

                    var data = (from cons in BMTDataContext.PracticeConsultants
                                where cons.PracticeId == PracticeId
                                select cons).FirstOrDefault();

                    if (data.ConsultantId == null && data.RelationshipId == null && data.XmlData == null)
                        return true;
                    return false;
                }

                return false;

            }
            catch
            {

                return false;
            }
        }

        public string CheckXMLData(int PracticeId)
        {

            try
            {

                if (PracticeId != 0)
                {

                    var data = (from cons in BMTDataContext.PracticeConsultants
                                where cons.PracticeId == PracticeId
                                select cons).FirstOrDefault();

                    if (data.ConsultantId == null && data.RelationshipId == null && data.XmlData != null)
                        return Convert.ToString(data.XmlData);
                    return null;
                }

                return null;

            }
            catch
            {

                return null;
            }
        }


        public string LoadXMLData(XDocument xml, string name)
        {

            try
            {

                var temp = from data in xml.Elements("Data")
                           where data.Attribute("Name").Value == name
                           select data.Attribute("Value").Value;
                
                return temp.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetPracticeName(int PracticeId)
        {

            try
            {

                var temp = (from site in BMTDataContext.PracticeSites
                           join consultant in BMTDataContext.PracticeConsultants on site.PracticeId equals consultant.PracticeId
                            where consultant.PracticeId == PracticeId
                           select site.Name).FirstOrDefault();

                return temp.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetUserName(int UserId)
        {

            try
            {

                var temp = (from user in BMTDataContext.Users
                            join consultant in BMTDataContext.PracticeConsultants on user.UserId equals consultant.ConsultantId
                            where consultant.ConsultantId == UserId
                            select user.Username).FirstOrDefault();

                if(temp != null)
                return temp.ToString();

                return null;

            }
            catch
            {

                return null;
            }
        }

        public string GetNewUserName(int UserId)
        {

            try
            {

                var temp = (from user in BMTDataContext.Users
                            where user.UserId == UserId
                            select user.Username).FirstOrDefault();

                if (temp != null)
                    return temp.ToString();

                return null;

            }
            catch
            {

                return null;
            }
        }

        public string GetRelationship(int relationshipId)
        {

            try
            {

                var temp = (from relation in BMTDataContext.Relationships
                            join consultant in BMTDataContext.PracticeConsultants on relation.RelationshipId equals consultant.RelationshipId
                            where consultant.RelationshipId == relationshipId
                            select relation.Name).FirstOrDefault();

                if (temp != null)
                return temp.ToString();

                return null;

            }
            catch
            {

                return null;
            }
        }

        public string UserName(int practiceId)
        {

            try
            {

                string temp = (from pUser in BMTDataContext.PracticeUsers
                            where pUser.PracticeId == practiceId
                            select
                            pUser.FirstName + " " + pUser.LastName).FirstOrDefault();

                if (temp != null)
                    return temp;

                return null;

            }
            catch
            {

                return null;
            }
        }

        public string LoginName(int practiceId)
        {

            try
            {

                string temp = (from pUser in BMTDataContext.PracticeUsers
                               join users in BMTDataContext.Users on pUser.UserId equals users.UserId
                               where pUser.PracticeId == practiceId
                               select
                               users.Username).FirstOrDefault();

                if (temp != null)
                    return temp;

                return null;

            }
            catch
            {

                return null;
            }
        }
    }
}
