using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class NCQASubmissionBO : BMTConnection
    {
        #region FUNCTIONS
        public void InsertNCQASubmission(int _projectId, string _licenseNumber, string _userName, string _password)
        {
            try
            {
                NCQASubmissionRequest ncqaSubmission = new NCQASubmissionRequest();

                //ncqaSubmission.ProjectId = _projectId;
                ncqaSubmission.ISSLicenseNumber = _licenseNumber;
                ncqaSubmission.UserName = _userName;
                ncqaSubmission.Password = _password;
                ncqaSubmission.StatusId = 1;
                ncqaSubmission.RequestedOn = DateTime.Now;
                ncqaSubmission.StatusId = 1;

                BMTDataContext.NCQASubmissionRequests.InsertOnSubmit(ncqaSubmission);
                BMTDataContext.SubmitChanges();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<SubmissionDetails> GetAllSubmissionRequest(int enterpriseId)
        {
            try
            {
                var lstNCQASubmission = (from ncqaSubmissionRecord in BMTDataContext.NCQASubmissionRequests
                                         join ncqaStatusRecord in BMTDataContext.SubmissionStatus
                                            on ncqaSubmissionRecord.StatusId equals ncqaStatusRecord.SubmissionStatusId
                                         join projectRecord in BMTDataContext.ProjectUsages
                                            on ncqaSubmissionRecord.ProjectUsageId equals projectRecord.ProjectUsageId
                                         join siteRecord in BMTDataContext.PracticeSites
                                            on ncqaSubmissionRecord.PracticeSiteId equals siteRecord.PracticeSiteId
                                         join practiceRecord in BMTDataContext.Practices
                                            on siteRecord.PracticeId equals practiceRecord.PracticeId
                                         join medicalGroupRecord in BMTDataContext.MedicalGroups
                                            on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                         join practiceUser in BMTDataContext.PracticeUsers
                                         on ncqaSubmissionRecord.updatedby equals practiceUser.UserId into tempTable
                                         from existingRecord in tempTable.DefaultIfEmpty()
                                         where medicalGroupRecord.EnterpriseId == enterpriseId
                                         orderby siteRecord.Name
                                         select new SubmissionDetails
                                         {
                                             SiteName = siteRecord.Name,
                                             ProjectUsageId = ncqaSubmissionRecord.ProjectUsageId,
                                             PracticeSiteId = ncqaSubmissionRecord.PracticeSiteId,
                                             ISSLicenseNumber = ncqaSubmissionRecord.ISSLicenseNumber,
                                             UserName = ncqaSubmissionRecord.UserName,
                                             Password = ncqaSubmissionRecord.Password,
                                             Status = ncqaStatusRecord.Name,
                                             RequestedOn = ncqaSubmissionRecord.RequestedOn,
                                             LastUpdated = ncqaSubmissionRecord.LastUpdated,
                                             updatedby = existingRecord.FirstName + " " + existingRecord.LastName,
                                             Completedon = ncqaSubmissionRecord.Completedon,
                                             SubmissionType = (int)enSubmissionType.Old,
                                             TemplateId = 1
                                         }).ToList<SubmissionDetails>();


                var lstPracticeSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                                 join template in BMTDataContext.Templates
                                                 on pracSiteSubmission.TemplateId equals template.TemplateId
                                                 join submissionStatus in BMTDataContext.SubmissionStatus
                                                    on pracSiteSubmission.StatusId equals submissionStatus.SubmissionStatusId
                                                 join siteRecord in BMTDataContext.PracticeSites
                                                     on pracSiteSubmission.PracticeSiteId equals siteRecord.PracticeSiteId
                                                 //join project in BMTDataContext.ProjectUsages
                                                 //on pracSiteSubmission.PracticeSiteId equals project.PracticeSiteId
                                                 join practiceRecord in BMTDataContext.Practices
                                                     on siteRecord.PracticeId equals practiceRecord.PracticeId
                                                 join medicalGroupRecord in BMTDataContext.MedicalGroups
                                                 on practiceRecord.MedicalGroupId equals medicalGroupRecord.MedicalGroupId
                                                 join practiceUser in BMTDataContext.PracticeUsers
                                                 on pracSiteSubmission.updatedby equals practiceUser.UserId into tempTable
                                                 from existingRecord in tempTable.DefaultIfEmpty()
                                                 where medicalGroupRecord.EnterpriseId == enterpriseId
                                                 && template.SubmitActionId == (int)enSubmitAction.NCQA
                                                 && pracSiteSubmission.RequestedOn != null
                                                 && pracSiteSubmission.APICredentials != null
                                                 orderby siteRecord.Name
                                                 select new SubmissionDetails
                                                 {
                                                     SiteName = siteRecord.Name,
                                                     ProjectUsageId = -1,
                                                     PracticeSiteId = pracSiteSubmission.PracticeSiteId,
                                                     ISSLicenseNumber = GetCredentialsAttributeByName(pracSiteSubmission.APICredentials, "ISSLicenseNumber"),
                                                     UserName = GetCredentialsAttributeByName(pracSiteSubmission.APICredentials, "UserName"),
                                                     Password = GetCredentialsAttributeByName(pracSiteSubmission.APICredentials, "Password"),
                                                     Status = submissionStatus.Name,
                                                     RequestedOn = (DateTime)pracSiteSubmission.RequestedOn,
                                                     LastUpdated = pracSiteSubmission.LastUpdated,
                                                     updatedby = existingRecord.FirstName + " " + existingRecord.LastName,
                                                     Completedon = pracSiteSubmission.Completedon,
                                                     SubmissionType = (int)enSubmissionType.New,
                                                     TemplateId = pracSiteSubmission.TemplateId
                                                 }).ToList<SubmissionDetails>();

                var all = lstNCQASubmission.Union(lstPracticeSiteSubmission).ToList<SubmissionDetails>();
                return all;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetCredentialsAttributeByName(XElement apiCredentials, string attrName)
        {
            try
            {
                return apiCredentials.Attribute(attrName).Value;
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public IQueryable GetAllSubmissionStatus()
        {
            try
            {
                return (from statusRecord in BMTDataContext.SubmissionStatus
                        select statusRecord) as IQueryable;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateNCQAStatus(int _projectUsageId, int _practiceSiteId, int _ncqaStatusId, int _userId, DateTime _requestedOn)
        {
            try
            {
                var ncqaSubmission = (from ncqaSubmissionRecord in BMTDataContext.NCQASubmissionRequests
                                      where
                                      ncqaSubmissionRecord.ProjectUsageId == _projectUsageId
                                      && ncqaSubmissionRecord.PracticeSiteId == _practiceSiteId
                                      && ncqaSubmissionRecord.RequestedOn.Year == _requestedOn.Year
                                      && ncqaSubmissionRecord.RequestedOn.Month == _requestedOn.Month
                                      && ncqaSubmissionRecord.RequestedOn.Day == _requestedOn.Day
                                      && ncqaSubmissionRecord.RequestedOn.Hour == _requestedOn.Hour
                                      && ncqaSubmissionRecord.RequestedOn.Minute == _requestedOn.Minute
                                      && ncqaSubmissionRecord.RequestedOn.Second == _requestedOn.Second
                                      select ncqaSubmissionRecord).FirstOrDefault();


                if (ncqaSubmission != null)
                {
                    ncqaSubmission.StatusId = _ncqaStatusId;
                    ncqaSubmission.updatedby = _userId;
                    ncqaSubmission.LastUpdated = DateTime.Now;

                    if (_ncqaStatusId == 2) // Fulfilled
                        ncqaSubmission.Completedon = DateTime.Now;
                    else
                        ncqaSubmission.Completedon = null;

                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public string GetPasswordByProjectId(int _projectUsageId, int _practiceSiteId, DateTime _requestedOn)
        {
            try
            {
                string password = string.Empty;
                NCQASubmissionRequest ncqaSubmission = (from ncqaSubmissionRecord in BMTDataContext.NCQASubmissionRequests
                                                        where
                                                        ncqaSubmissionRecord.ProjectUsageId == _projectUsageId
                                                        && ncqaSubmissionRecord.PracticeSiteId == _practiceSiteId
                                                        && ncqaSubmissionRecord.RequestedOn.Year == _requestedOn.Year
                                                        && ncqaSubmissionRecord.RequestedOn.Month == _requestedOn.Month
                                                        && ncqaSubmissionRecord.RequestedOn.Day == _requestedOn.Day
                                                        && ncqaSubmissionRecord.RequestedOn.Hour == _requestedOn.Hour
                                                        && ncqaSubmissionRecord.RequestedOn.Minute == _requestedOn.Minute
                                                        && ncqaSubmissionRecord.RequestedOn.Second == _requestedOn.Second
                                                        select ncqaSubmissionRecord).FirstOrDefault();

                if (ncqaSubmission != null)
                {
                    Security security = new Security();
                    password = security.Decrypt(ncqaSubmission.Password);
                }

                return password;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool NCQASubmissionRequestExists(int _projectId)
        {
            try
            {
                bool status = false;
                NCQASubmissionRequest ncqaSubmission = (from ncqaSubmissionRecord in BMTDataContext.NCQASubmissionRequests
                                                        where 
                                                        //ncqaSubmissionRecord.ProjectId == _projectId && 
                                                        ncqaSubmissionRecord.StatusId != 2 // Not fulfilled
                                                        select ncqaSubmissionRecord).FirstOrDefault();

                if (ncqaSubmission != null)
                {
                    status = true;
                }

                return status;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        #endregion
    }
}
