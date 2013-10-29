using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class SubmissionMethod : BMTConnection
    {
        public virtual PracticeSiteSubmission GetPracticeSiteSubmissionByProjectIdAndTemplateId(int projectUsageId, int siteId, int templateId)
        {
            try
            {
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              join pracSite in BMTDataContext.PracticeSites
                                              on pracSiteSubmission.PracticeSiteId equals pracSite.PracticeSiteId
                                              join projectUsage in BMTDataContext.ProjectUsages
                                              on pracSite.PracticeId equals projectUsage.PracticeId
                                              where projectUsage.ProjectUsageId == projectUsageId
                                              && pracSite.PracticeSiteId == siteId
                                              && pracSiteSubmission.TemplateId == templateId
                                              select pracSiteSubmission).FirstOrDefault();

                return practiceSiteSubmission;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool IsSubmissionRequestExists(int projectUsageId, int siteId, int templateId)
        {
            try
            {
                bool status = false;
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              join pracSite in BMTDataContext.PracticeSites
                                              on pracSiteSubmission.PracticeSiteId equals pracSite.PracticeSiteId
                                              join projectUsage in BMTDataContext.ProjectUsages
                                              on pracSite.PracticeId equals projectUsage.PracticeId
                                              where projectUsage.ProjectUsageId == projectUsageId
                                              && pracSite.PracticeSiteId==siteId
                                              && pracSiteSubmission.TemplateId == templateId
                                              && (pracSiteSubmission.StatusId == null || pracSiteSubmission.StatusId != (int)enSubmissionStatus.Fulfilled)
                                              && pracSiteSubmission.RequestedOn != null
                                              select pracSiteSubmission).FirstOrDefault();

                if (practiceSiteSubmission != null)
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

        public virtual void UpdatePracticeSiteSubmission(int projectUsageId, int siteId, int templateId, bool? reviewed, bool? submitted, bool? recognized, DateTime? submittedOn, DateTime? recognizedOn,
            string recognizedLevel)
        {
            try
            {
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              join pracSite in BMTDataContext.PracticeSites
                                              on pracSiteSubmission.PracticeSiteId equals pracSite.PracticeSiteId
                                              join projectUsage in BMTDataContext.ProjectUsages
                                              on pracSite.PracticeId equals projectUsage.PracticeId
                                              where projectUsage.ProjectUsageId == projectUsageId
                                              && pracSite.PracticeSiteId==siteId
                                              && pracSiteSubmission.TemplateId == templateId
                                              && (pracSiteSubmission.StatusId == null || pracSiteSubmission.StatusId != (int)enSubmissionStatus.Fulfilled)
                                              select pracSiteSubmission).FirstOrDefault();

                if (practiceSiteSubmission != null)
                {
                    if (reviewed != null)
                    {
                        practiceSiteSubmission.Reviewed = reviewed;
                    }
                    else if (submitted != null)
                    {
                        if (Convert.ToBoolean(submitted.ToString().ToLower()))
                        {
                            practiceSiteSubmission.Submitted = submitted;
                            practiceSiteSubmission.SubmittedOn = submittedOn;
                        }
                        else
                        {
                            practiceSiteSubmission.Submitted = null;
                            practiceSiteSubmission.SubmittedOn = null;
                        }
                    }
                    else if (recognized != null)
                    {
                        if (Convert.ToBoolean(recognized.ToString().ToLower()))
                        {
                            practiceSiteSubmission.Recognized = recognized;
                            practiceSiteSubmission.RecognizedOn = recognizedOn;
                            practiceSiteSubmission.RecognizedLevel = recognizedLevel;
                        }
                        else
                        {
                            practiceSiteSubmission.Recognized = null;
                            practiceSiteSubmission.RecognizedOn = null;
                            practiceSiteSubmission.RecognizedLevel = null;
                        }
                    }

                    BMTDataContext.SubmitChanges();
                }
                else
                {
                    InsertPracticeSiteSubmission(templateId, projectUsageId, siteId, reviewed, submitted, recognized, submittedOn, recognizedOn, recognizedLevel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PracticeSiteSubmission InsertPracticeSiteSubmission(int templateId, int projectUsageId, int siteId, bool? reviewed, bool? submitted, bool? recognized, DateTime? submittedOn, DateTime? recognizedOn,
            string recognizedLevel)
        {
            try
            {
                int practiceSiteId = siteId;
                PracticeSiteSubmission newPracticeSiteSubmission = new PracticeSiteSubmission();

                newPracticeSiteSubmission.PracticeSiteId = practiceSiteId;
                newPracticeSiteSubmission.TemplateId = templateId;

                if (reviewed != null)
                {
                    newPracticeSiteSubmission.Reviewed = reviewed;
                }
                else if (submitted != null)
                {
                    newPracticeSiteSubmission.Submitted = submitted;
                    newPracticeSiteSubmission.SubmittedOn = submittedOn;
                }
                else if (recognized != null)
                {
                    newPracticeSiteSubmission.Recognized = recognized;
                    newPracticeSiteSubmission.RecognizedOn = recognizedOn;
                    newPracticeSiteSubmission.RecognizedLevel = recognizedLevel;
                }

                BMTDataContext.PracticeSiteSubmissions.InsertOnSubmit(newPracticeSiteSubmission);
                BMTDataContext.SubmitChanges();

                return newPracticeSiteSubmission;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual void UpdateSubmissionStatus(int practiceSiteId, int statusId, int userId, DateTime requestedOn)
        {
            try
            {
                var practiceSiteSubmission = (from pracSiteSubmission in BMTDataContext.PracticeSiteSubmissions
                                              where pracSiteSubmission.PracticeSiteId == practiceSiteId
                                              && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Year == requestedOn.Year
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Month == requestedOn.Month
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Day == requestedOn.Day
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Hour == requestedOn.Hour
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Minute == requestedOn.Minute
                                                        && Convert.ToDateTime(pracSiteSubmission.RequestedOn).Second == requestedOn.Second
                                              select pracSiteSubmission).FirstOrDefault();


                if (practiceSiteSubmission != null)
                {
                    practiceSiteSubmission.StatusId = statusId;
                    practiceSiteSubmission.updatedby = userId;
                    practiceSiteSubmission.LastUpdated = DateTime.Now;

                    if (statusId == 2) // Fulfilled
                        practiceSiteSubmission.Completedon = DateTime.Now;
                    else
                        practiceSiteSubmission.Completedon = null;

                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}
