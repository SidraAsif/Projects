#region Modification History

//  ******************************************************************************
//  Module        : 
//  Created By    : 
//  When Created  : 
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    02-03-2012       Set practice Id ==NULL if Super User uploaded the file in ToolBox for public and add error handling
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

namespace BMTBLL
{
    public class FileBO : BMTConnection
    {
        TreeBO tree = new TreeBO();

        public void SaveUploadData(string tablename, string name, string link, DateTime UploadedDate, int SectionId, int UploadedBy, string Description, int PraticeId, string Image)
        {
            try
            {
                if (tablename == enDbTables.ToolDocument.ToString())
                {
                    ToolDocument toolDocument = new ToolDocument();
                    toolDocument.Name = name;
                    toolDocument.Link = link;
                    toolDocument.UploadedDate = UploadedDate;
                    toolDocument.ToolSectionId = SectionId;
                    toolDocument.UploadedBy = UploadedBy;
                    toolDocument.Description = Description;

                    if (PraticeId > 0)
                        toolDocument.PraticeId = PraticeId;

                    toolDocument.Image = Image;

                    BMTDataContext.ToolDocuments.InsertOnSubmit(toolDocument);
                    BMTDataContext.SubmitChanges();

                }

                if (tablename == enDbTables.LibraryDocument.ToString())
                {
                    LibraryDocument libraryDocument = new LibraryDocument();
                    libraryDocument.Name = name;
                    libraryDocument.Link = link;
                    libraryDocument.UploadedDate = UploadedDate;
                    libraryDocument.LibrarySectionId = SectionId;
                    libraryDocument.UploadedBy = UploadedBy;
                    libraryDocument.description = Description;
                    libraryDocument.Image = Image;

                    BMTDataContext.LibraryDocuments.InsertOnSubmit(libraryDocument);
                    BMTDataContext.SubmitChanges();

                }

                if (tablename == enDbTables.ProjectDocument.ToString())
                {
                    ProjectDocument projectDocument = new ProjectDocument();
                    projectDocument.Name = name;
                    projectDocument.Link = link;
                    projectDocument.UploadedDate = UploadedDate;
                    projectDocument.ProjectSectionId = SectionId;
                    projectDocument.UploadedBy = UploadedBy;
                    projectDocument.Description = Description;
                    //projectDocument.PracticeId = PraticeId;
                    projectDocument.Image = Image;

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(projectDocument);
                    BMTDataContext.SubmitChanges();

                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public void SaveUploadData(string tablename, string name, string link, DateTime UploadedDate, int SectionId, int UploadedBy, string Description, string Image,int ProjectUsageId)
        {
            try
            {
                if (ProjectUsageId != 0)
                {
                    ProjectDocument projectDocument = new ProjectDocument();
                    projectDocument.Name = name;
                    projectDocument.Link = link;
                    projectDocument.UploadedDate = UploadedDate;
                    projectDocument.ProjectSectionId = SectionId;
                    projectDocument.UploadedBy = UploadedBy;
                    projectDocument.Description = Description;
                    projectDocument.ProjectUsageId = ProjectUsageId;
                    projectDocument.Image = Image;

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(projectDocument);
                    BMTDataContext.SubmitChanges();
                }
                else
                {
                    ProjectDocument projectDocument = new ProjectDocument();
                    projectDocument.Name = name;
                    projectDocument.Link = link;
                    projectDocument.UploadedDate = UploadedDate;
                    projectDocument.ProjectSectionId = SectionId;
                    projectDocument.UploadedBy = UploadedBy;
                    projectDocument.Description = Description;
                    projectDocument.Image = Image;

                    BMTDataContext.ProjectDocuments.InsertOnSubmit(projectDocument);
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