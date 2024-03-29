﻿#region Modification History

//  ******************************************************************************
//  Module        : Print Report/Documents etc...
//  Created By    : Haris
//  When Created  : 
//  Description   : Get pdf save path and saving pdf into db
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    04/29/2012      Add new parameter in SaveData
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BMTBLL.Enumeration;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using BMTBLL;
using System.IO;

namespace BMTBLL
{
    public class PrintBO : BMTConnection
    {

        private const string TEMPLATE_UTILITY_FOLDER = "Utility Folder";
        private const string TEMPLATE_UPLOADED_DOCUMENT_FOLDER = "Uploaded Documents";

        #region FUNCTIONS
        private int GetSectionIdByContentType(string Contenttype)
        {
            try
            {
                int sectionId = Convert.ToInt32((from id in BMTDataContext.ProjectSections
                                                 //where id.ContentType == Contenttype
                                                 select id.ProjectSectionId).FirstOrDefault());

                return sectionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private int GetSectionIdByName(string name)
        {

            try
            {
                int id = (from child in BMTDataContext.ProjectSections
                          join parent in BMTDataContext.ProjectSections on child.ParentProjectSectionId equals parent.ProjectSectionId
                          where parent.Name == name
                          select child.ProjectSectionId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private string GetUserNameByUserID(int userId)
        {
            try
            {
                string username = Convert.ToString((from name in BMTDataContext.Users
                                                    where name.UserId == userId
                                                    select name.Username).FirstOrDefault());

                return username;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private int CheckPrintReport(string name)
        {

            try
            {
                int ID = 0;
                ID = Convert.ToInt32((from record in BMTDataContext.ProjectDocuments
                                      where record.Name == name
                                      select record.ProjectDocumentId).FirstOrDefault());

                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveData(string name, string link, DateTime docUploadedDate, int docUploadedBy, int PraticeId, bool printOnly,
            string contentType)
        {
            if (!printOnly)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);

                link = link + "?" + randomNumber;
                ProjectDocument pDoc = new ProjectDocument();
                pDoc.Name = name;
                pDoc.Link = link;
                pDoc.UploadedDate = docUploadedDate;
                pDoc.ProjectSectionId = GetSectionIdByContentType(contentType);
                pDoc.UploadedBy = docUploadedBy;
                pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                //pDoc.PracticeId = PraticeId;
                pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                BMTDataContext.SubmitChanges();
            }
            else if (printOnly)
            {
                if (CheckPrintReport(name) == 0)
                {
                    ProjectDocument pDoc = new ProjectDocument();
                    pDoc.Name = name;
                    pDoc.Link = link;
                    pDoc.UploadedDate = docUploadedDate;
                    pDoc.ProjectSectionId = GetSectionIdByContentType(contentType);
                    pDoc.UploadedBy = docUploadedBy;
                    pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                    // pDoc.PracticeId = PraticeId;
                    pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                    BMTDataContext.SubmitChanges();

                }
                else
                {
                    ProjectDocument pDoc = new ProjectDocument();
                    ProjectDocument Doc = (from record in BMTDataContext.ProjectDocuments
                                           where record.Name == name
                                           select record).FirstOrDefault();
                    Doc.Name = name;
                    Doc.Link = link;
                    Doc.UploadedDate = docUploadedDate;
                    Doc.ProjectSectionId = GetSectionIdByContentType(contentType);
                    Doc.UploadedBy = docUploadedBy;
                    Doc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                    //Doc.PracticeId = PraticeId;
                    Doc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                    BMTDataContext.SubmitChanges();
                }
            }

        }

        public void SaveData(string name, string link, DateTime docUploadedDate, int docUploadedBy, int PraticeId, bool printOnly,
    string contentType,int projectUsageId,int sectionId)
        {
            int projectId = GetProjectIdBySectionId(sectionId);

            int Id = (from projSec in BMTDataContext.ProjectSections
                      where projSec.ProjectId == projectId &&
                      projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                      select projSec.ProjectSectionId).FirstOrDefault();

            if (!printOnly)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);

                link = link + "?" + randomNumber;
                ProjectDocument pDoc = new ProjectDocument();
                pDoc.Name = name;
                pDoc.Link = link;
                pDoc.UploadedDate = docUploadedDate;
                pDoc.ProjectSectionId = Id;
                pDoc.UploadedBy = docUploadedBy;
                pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                pDoc.ProjectUsageId = projectUsageId;
                pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                BMTDataContext.SubmitChanges();
            }
            else if (printOnly)
            {
                if (CheckPrintReport(name) == 0)
                {
                    ProjectDocument pDoc = new ProjectDocument();
                    pDoc.Name = name;
                    pDoc.Link = link;
                    pDoc.UploadedDate = docUploadedDate;
                    pDoc.ProjectSectionId = Id;
                    pDoc.UploadedBy = docUploadedBy;
                    pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                    pDoc.ProjectUsageId = projectUsageId;
                    pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                    BMTDataContext.SubmitChanges();

                }
                else
                {
                    ProjectDocument pDoc = new ProjectDocument();
                    ProjectDocument Doc = (from record in BMTDataContext.ProjectDocuments
                                           where record.Name == name
                                           select record).FirstOrDefault();
                    Doc.Name = name;
                    Doc.Link = link;
                    Doc.UploadedDate = docUploadedDate;
                    Doc.ProjectSectionId = Id;
                    Doc.UploadedBy = docUploadedBy;
                    Doc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                    Doc.ProjectUsageId = projectUsageId;
                    Doc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                    BMTDataContext.SubmitChanges();
                }
            }

        }

        public void SaveDataForMORe(string name, string link, DateTime docUploadedDate, int docUploadedBy, int PraticeId, bool printOnly,
           string contentType, int templateId,int sectionId,int projectUsageId)
        {
            if (!printOnly)
            {
                int projectId = GetProjectIdBySectionId(sectionId);

                int Id = (from projSec in BMTDataContext.ProjectSections
                          where projSec.ProjectId == projectId &&
                          projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                          select projSec.ProjectSectionId).FirstOrDefault();

                if (Id != 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 100000);

                    link = link + "?" + randomNumber;

                    ProjectDocument pDoc = new ProjectDocument();
                    pDoc.Name = name;
                    pDoc.Link = link;
                    pDoc.UploadedDate = docUploadedDate;
                    pDoc.ProjectSectionId = Id;
                    pDoc.UploadedBy = docUploadedBy;
                    pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                    pDoc.ProjectUsageId = projectUsageId;
                    pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                    BMTDataContext.SubmitChanges();
                }
            }
            else if (printOnly)
            {
                if (CheckPrintReport(name) == 0)
                {
                    int projectId = GetProjectIdBySectionId(sectionId);

                    int Id = (from projSec in BMTDataContext.ProjectSections
                              where projSec.ProjectId == projectId &&
                              projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                              select projSec.ProjectSectionId).FirstOrDefault();

                    if (Id != 0)
                    {
                        ProjectDocument pDoc = new ProjectDocument();
                        pDoc.Name = name;
                        pDoc.Link = link;
                        pDoc.UploadedDate = docUploadedDate;
                        pDoc.ProjectSectionId = Id;
                        pDoc.UploadedBy = docUploadedBy;
                        pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                        pDoc.ProjectUsageId = projectUsageId;
                        pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                        BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                        BMTDataContext.SubmitChanges();
                    }

                }
                else
                {
                    int projectId = GetProjectIdBySectionId(sectionId);

                    int Id = (from projSec in BMTDataContext.ProjectSections
                              where projSec.ProjectId == projectId &&
                              projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                              select projSec.ProjectSectionId).FirstOrDefault();

                    if (Id != 0)
                    {
                        ProjectDocument pDoc = new ProjectDocument();
                        ProjectDocument Doc = (from record in BMTDataContext.ProjectDocuments
                                               where record.Name == name
                                               select record).FirstOrDefault();
                        Doc.Name = name;
                        Doc.Link = link;
                        Doc.UploadedDate = docUploadedDate;
                        Doc.ProjectSectionId = Id;
                        Doc.UploadedBy = docUploadedBy;
                        Doc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy");
                        Doc.ProjectUsageId = projectUsageId;
                        Doc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                        BMTDataContext.SubmitChanges();
                    }
                }
            }

        }

        public void SaveSRAData(string name, string link, int docUploadedBy, int PraticeId, string contentType,int sectionId,int projectUsageId)
        {
            try
            {
                int projectId = GetProjectIdBySectionId(sectionId);

                int Id = (from projSec in BMTDataContext.ProjectSections
                          where projSec.ProjectId == projectId &&
                          projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                          select projSec.ProjectSectionId).FirstOrDefault();

                Random random = new Random();
                int randomNumber = random.Next(0, 100000);

                link = link + "?" + randomNumber;
                ProjectDocument pDoc = new ProjectDocument();
                pDoc.Name = name;
                pDoc.Link = link;
                pDoc.UploadedDate = System.DateTime.Now;
                pDoc.ProjectSectionId = Id;
                pDoc.UploadedBy = docUploadedBy;
                pDoc.Description = "Generated by " + GetUserNameByUserID(docUploadedBy) + " on " + DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");
                pDoc.ProjectUsageId = projectUsageId;
                pDoc.Image = "~/Themes/Images/GenericFileUploaderImage.png";

                BMTDataContext.ProjectDocuments.InsertOnSubmit(pDoc);
                BMTDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetProjectIdBySectionId(int sectionId)
        {
            try
            {
                int projectId = (from projSec in BMTDataContext.ProjectSections
                                 where projSec.ProjectSectionId == sectionId
                                 select projSec.ProjectId).FirstOrDefault();

                return projectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}