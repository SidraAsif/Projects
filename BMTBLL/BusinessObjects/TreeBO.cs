#region Modification History

//  ******************************************************************************
//  Module        : AddressDetail
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01-17-2012    Super User & modified the code
//  Muhammad ADil Butt      21-03-2012    Optimize the Code  
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using BMTBLL.Enumeration;
using BMTBLL.Classes;

namespace BMTBLL
{
    public class TreeBO : BMTConnection
    {
        #region CONSTANS
        private const string DEFAULT_JUMP_START_MATERIALS = "JumpStartMaterials";
        private const string DEFAULT_RESTRICTED_FOLDER = "RestrictedFolder";
        #endregion

        # region VARIABLES
        private List<TreeDetail> treeDataResult;
        private List<TreeDetail> node;
        private List<LibrarySection> libSectionRcord;


        private string libName;
        private bool isSection = true;
        private bool isSectionRepeatd = true;
        private int parentId;
        private string idsList;
        private string sectId;

        #endregion

        #region PROPERTIES
        public int PracticeId { get; set; }
        #endregion

        #region FUNCTIONS
        public IQueryable<string> GetPracticeName(int practiceId)
        {
            try
            {
                IQueryable<string> practice = (from practiceRecord in BMTDataContext.Practices
                                               where practiceRecord.PracticeId == practiceId
                                               select practiceRecord.Name);

                return practice;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetSiteName(int practiceId)
        {
            try
            {
                List<string> Site = (from practiceRecord in BMTDataContext.Practices
                                     join siteRecord in BMTDataContext.PracticeSites
                                      on practiceRecord.PracticeId equals siteRecord.PracticeId
                                     where siteRecord.PracticeId == practiceId
                                     select siteRecord.Name).ToList();
                return Site;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public string GetDefaultSiteName(int practiceId)
        {
            try
            {
                string Site = (from practiceRecord in BMTDataContext.Practices
                               join siteRecord in BMTDataContext.PracticeSites
                                on practiceRecord.PracticeId equals siteRecord.PracticeId
                               where siteRecord.PracticeId == practiceId
                               select siteRecord.Name).First();
                return Site;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public int GetSiteID(int PracticeId, string SiteName)
        {
            try
            {
                if (SiteName != null)
                {
                    int Site = (from practiceRecord in BMTDataContext.Practices
                                join siteRecord in BMTDataContext.PracticeSites
                                    on practiceRecord.PracticeId equals siteRecord.PracticeId
                                where practiceRecord.PracticeId == PracticeId && siteRecord.Name == SiteName
                                select siteRecord.PracticeSiteId).Single();

                    return Site;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }

        public int GetPracticeID(int UID, string PracticeName)
        {
            try
            {
                int Site = (from a in BMTDataContext.PracticeUsers
                            join b in BMTDataContext.Practices on a.PracticeId equals b.PracticeId
                            where a.UserId == UID && b.Name == PracticeName
                            select b.PracticeId).Single();

                return Site;
            }
            catch
            {
                return 0;
            }
        }

        public int GetProjectUsageID(int PracticeID, int projectId)
        {
            try
            {
                int projectUsage = 0;
                projectUsage = (from projUsage in BMTDataContext.ProjectUsages
                                where projUsage.PracticeId == PracticeID
                                && projUsage.ProjectId == projectId
                                select projUsage.ProjectUsageId).FirstOrDefault();

                return projectUsage;

            }
            catch
            {
                return 0;
            }
        }

        public int GetProjectID(int SectionId)
        {
            try
            {
                int projectId = 0;
                projectId = (from projSec in BMTDataContext.ProjectSections
                             where projSec.ProjectSectionId == SectionId
                             select projSec.ProjectId).FirstOrDefault();

                return projectId;

            }
            catch
            {
                return 0;
            }
        }

        public List<TreeDetail> GetSiteFixedContents()
        {
            try
            {
                List<TreeDetail> Sfc = (from projectSections in BMTDataContext.ProjectSections
                                        select new TreeDetail
                                        {
                                            Name = projectSections.Name,
                                            ParentSectionId = projectSections.ParentProjectSectionId,
                                            SectionId = projectSections.ProjectSectionId,
                                            // ContentType = projectSections.ContentType,
                                            //IsTopNode = projectSections.IsTopLevelNode
                                        }).ToList();

                return Sfc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeDetail> GetTreeNodesByEnterpriseId(enTreeType treeType, int enterpriseId)
        {
            try
            {
                switch (treeType)
                {
                    case enTreeType.LibrarySection:
                        treeDataResult = (from enterpriseLibraryRecord in BMTDataContext.EnterpriseLibrarySections
                                          join libraryRecord in BMTDataContext.LibrarySections
                                          on enterpriseLibraryRecord.LibrarySectionId equals libraryRecord.LibrarySectionId
                                          where enterpriseLibraryRecord.EnterpriseId == enterpriseId
                                          &&
                                            (libraryRecord.ContentType == null
                                            || libraryRecord.ContentType != DEFAULT_RESTRICTED_FOLDER)
                                          select new TreeDetail
                                          {
                                              Name = libraryRecord.Name,
                                              ParentSectionId = libraryRecord.ParentLibrarySectionId,
                                              SectionId = libraryRecord.LibrarySectionId,
                                              ContentType = libraryRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                    case enTreeType.ReportSection:
                        treeDataResult = (from reportRecord in BMTDataContext.ReportSections
                                          join enterpriseReportSection in BMTDataContext.EnterpriseReportSections
                                          on reportRecord.ReportSectionId equals enterpriseReportSection.ReportSectionId
                                          where enterpriseReportSection.EnterpriseId == enterpriseId
                                          && (reportRecord.ContentType == null || reportRecord.ContentType != DEFAULT_RESTRICTED_FOLDER)
                                          select new TreeDetail
                                          {
                                              Name = reportRecord.Name,
                                              ParentSectionId = reportRecord.ParentReportSectionId,
                                              SectionId = reportRecord.ReportSectionId,
                                              ContentType = reportRecord.ContentType,
                                              IsTopNode = false

                                          }).ToList();
                        break;


                    case enTreeType.ProjectSection:

                        treeDataResult = (from projectRecord in BMTDataContext.ProjectSections
                                          join projectAssigment in BMTDataContext.ProjectAssignments
                                          on projectRecord.ProjectId equals projectAssigment.ProjectId
                                          where projectAssigment.EnterpriseId == enterpriseId
                                          //&& (projectRecord.ContentType == null || projectRecord.ContentType != DEFAULT_RESTRICTED_FOLDER)
                                          //  && enterpriseProjectRecord.MedicalGroupId == null
                                          select new TreeDetail
                                          {
                                              Name = projectRecord.Name,
                                              SectionId = projectRecord.ProjectSectionId,
                                              ParentSectionId = projectRecord.ParentProjectSectionId,
                                              //ContentType = projectRecord.ContentType,
                                              //IsTopNode = projectRecord.IsTopLevelNode

                                          }).ToList();
                        break;

                    case enTreeType.ToolSection:
                        treeDataResult = (from enterpriseToolRecord in BMTDataContext.EnterpriseToolSections
                                          join toolRecord in BMTDataContext.ToolSections
                                          on enterpriseToolRecord.ToolSectionId equals toolRecord.ToolSectionId
                                          where enterpriseToolRecord.EnterpriseId == enterpriseId
                                          && (toolRecord.ContentType == null || toolRecord.ContentType != DEFAULT_RESTRICTED_FOLDER)
                                          select new TreeDetail
                                          {
                                              Name = toolRecord.Name,
                                              ParentSectionId = toolRecord.ParentToolSectionId,
                                              SectionId = toolRecord.ToolSectionId,
                                              ContentType = toolRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                }
                return treeDataResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeDetail> GetTreeNodesByMedicalGroupId(int medicalGroupId)
        {
            try
            {
                var lstTreeNodes = (from projectRecord in BMTDataContext.ProjectSections
                                    join projAssignment in BMTDataContext.ProjectAssignments
                                    on projectRecord.ProjectId equals projAssignment.ProjectId
                                    where projAssignment.MedicalGroupId == medicalGroupId
                                    select new TreeDetail
                                    {
                                        Name = projectRecord.Name,
                                        SectionId = projectRecord.ProjectSectionId,
                                        ParentSectionId = projectRecord.ParentProjectSectionId,
                                        //ContentType = projectRecord.ContentType,
                                        //IsTopNode = projectRecord.IsTopLevelNode

                                    }).ToList();

                return lstTreeNodes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TreeDetail> GetRestrictedFolders(enTreeType treeType, int enterpriseId)
        {
            try
            {
                switch (treeType)
                {
                    case enTreeType.LibrarySection:
                        treeDataResult = (from enterpriseLibraryRecord in BMTDataContext.EnterpriseLibrarySections
                                          join libraryRecord in BMTDataContext.LibrarySections
                                          on enterpriseLibraryRecord.LibrarySectionId equals libraryRecord.LibrarySectionId
                                          where enterpriseLibraryRecord.EnterpriseId == enterpriseId
                                          && libraryRecord.ContentType == DEFAULT_RESTRICTED_FOLDER
                                          && (libraryRecord.ParentLibrarySectionId == null || libraryRecord.ParentLibrarySectionId == 0)
                                          select new TreeDetail
                                          {
                                              Name = libraryRecord.Name,
                                              ParentSectionId = libraryRecord.ParentLibrarySectionId,
                                              SectionId = libraryRecord.LibrarySectionId,
                                              ContentType = libraryRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                    case enTreeType.ProjectSection:

                        treeDataResult = (from projectRecord in BMTDataContext.ProjectSections
                                          join projectAssign in BMTDataContext.ProjectAssignments
                                          on projectRecord.ProjectId equals projectAssign.ProjectId
                                          where projectAssign.EnterpriseId == enterpriseId
                                              //&& projectRecord.ContentType == DEFAULT_RESTRICTED_FOLDER
                                          && (projectRecord.ParentProjectSectionId == null || projectRecord.ParentProjectSectionId == 0)
                                          select new TreeDetail
                                          {
                                              Name = projectRecord.Name,
                                              SectionId = projectRecord.ProjectSectionId,
                                              ParentSectionId = projectRecord.ParentProjectSectionId,
                                              //ContentType = projectRecord.ContentType,
                                              //IsTopNode = projectRecord.IsTopLevelNode

                                          }).ToList();
                        break;
                    case enTreeType.ReportSection:

                        treeDataResult = (from reportRecord in BMTDataContext.ReportSections
                                          join enterpriseReportSection in BMTDataContext.EnterpriseReportSections
                                          on reportRecord.ReportSectionId equals enterpriseReportSection.ReportSectionId
                                          where enterpriseReportSection.EnterpriseId == enterpriseId
                                          && reportRecord.ContentType == DEFAULT_RESTRICTED_FOLDER
                                          && (reportRecord.ParentReportSectionId == null || reportRecord.ParentReportSectionId == 0)
                                          select new TreeDetail
                                          {
                                              Name = reportRecord.Name,
                                              SectionId = reportRecord.ReportSectionId,
                                              ParentSectionId = reportRecord.ParentReportSectionId,
                                              ContentType = reportRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();
                        break;

                    case enTreeType.ToolSection:
                        treeDataResult = (from enterpriseToolRecord in BMTDataContext.EnterpriseToolSections
                                          join toolRecord in BMTDataContext.ToolSections
                                          on enterpriseToolRecord.ToolSectionId equals toolRecord.ToolSectionId
                                          where enterpriseToolRecord.EnterpriseId == enterpriseId
                                          && toolRecord.ContentType == DEFAULT_RESTRICTED_FOLDER
                                          && (toolRecord.ParentToolSectionId == null || toolRecord.ParentToolSectionId == 0)
                                          select new TreeDetail
                                          {
                                              Name = toolRecord.Name,
                                              ParentSectionId = toolRecord.ParentToolSectionId,
                                              SectionId = toolRecord.ToolSectionId,
                                              ContentType = toolRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                }
                return treeDataResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeDetail> GetNodes(String QueryString, enTreeType treeType)
        {
            try
            {
                switch (treeType)
                {
                    case enTreeType.LibrarySection:
                        node = (from libraryReport in BMTDataContext.LibrarySections
                                where libraryReport.ContentType == QueryString
                                select new TreeDetail
                                {
                                    Name = libraryReport.Name,
                                    ParentSectionId = libraryReport.ParentLibrarySectionId,
                                    SectionId = libraryReport.LibrarySectionId,
                                    ContentType = libraryReport.ContentType
                                }
                                                           ).ToList();

                        break;
                    case enTreeType.ProjectSection:
                        node = (from projectReport in BMTDataContext.ProjectSections
                                //where projectReport.ContentType == QueryString
                                select new TreeDetail
                                {
                                    Name = projectReport.Name,
                                    ParentSectionId = projectReport.ParentProjectSectionId,
                                    SectionId = projectReport.ProjectSectionId,
                                    //ContentType = projectReport.ContentType
                                }
                                                          ).ToList();

                        break;

                    case enTreeType.ToolSection:
                        node = (from toolReport in BMTDataContext.ToolSections
                                where toolReport.ContentType == QueryString
                                select new TreeDetail
                                {
                                    Name = toolReport.Name,
                                    ParentSectionId = toolReport.ParentToolSectionId,
                                    SectionId = toolReport.ToolSectionId,
                                    ContentType = toolReport.ContentType
                                }
                                                          ).ToList();

                        break;
                }
                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<ProjectSection> GetProjectNode(String QueryString, enTreeType treeType)
        {
            try
            {
                IQueryable<ProjectSection> _Node = (from tree in BMTDataContext.ProjectSections
                                                    // where tree.ContentType == QueryString
                                                    select tree);
                return _Node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<LibrarySection> GetLibraryNode(String QueryString, enTreeType treeType)
        {
            try
            {
                IQueryable<LibrarySection> _Node = (from tree in BMTDataContext.LibrarySections
                                                    where tree.ContentType == QueryString
                                                    select tree);

                return _Node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<ToolSection> GetToolNode(String QueryString, enTreeType treeType)
        {
            try
            {
                IQueryable<ToolSection> _Node = (from tree in BMTDataContext.ToolSections
                                                 where tree.ContentType == QueryString
                                                 select tree);
                return _Node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreeDetail> GetChildNodesByParentId(enTreeType treeType, long parentId)
        {
            try
            {
                switch (treeType)
                {
                    case enTreeType.LibrarySection:
                        treeDataResult = (from libraryRecord in BMTDataContext.LibrarySections
                                          where libraryRecord.ParentLibrarySectionId == parentId
                                          select new TreeDetail
                                          {
                                              Name = libraryRecord.Name,
                                              ParentSectionId = libraryRecord.ParentLibrarySectionId,
                                              SectionId = libraryRecord.LibrarySectionId,
                                              ContentType = libraryRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                    case enTreeType.ProjectSection:

                        treeDataResult = (from projectRecord in BMTDataContext.ProjectSections
                                          where projectRecord.ParentProjectSectionId == parentId
                                          select new TreeDetail
                                          {
                                              Name = projectRecord.Name,
                                              SectionId = projectRecord.ProjectSectionId,
                                              ParentSectionId = projectRecord.ParentProjectSectionId,
                                              //ContentType = projectRecord.ContentType,
                                              //IsTopNode = projectRecord.IsTopLevelNode

                                          }).ToList();
                        break;
                    case enTreeType.ReportSection:

                        treeDataResult = (from reportRecord in BMTDataContext.ReportSections
                                          where reportRecord.ParentReportSectionId == parentId
                                          select new TreeDetail
                                          {
                                              Name = reportRecord.Name,
                                              SectionId = reportRecord.ReportSectionId,
                                              ParentSectionId = reportRecord.ParentReportSectionId,
                                              ContentType = reportRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();
                        break;

                    case enTreeType.ToolSection:
                        treeDataResult = (from toolRecord in BMTDataContext.ToolSections
                                          where toolRecord.ParentToolSectionId == parentId
                                          select new TreeDetail
                                          {
                                              Name = toolRecord.Name,
                                              ParentSectionId = toolRecord.ParentToolSectionId,
                                              SectionId = toolRecord.ToolSectionId,
                                              ContentType = toolRecord.ContentType,
                                              IsTopNode = false
                                          }).ToList();

                        break;
                }
                return treeDataResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetPracticeIdByUserId(int UserId)
        {

            try
            {
                int id = Convert.ToInt32((from Pid in BMTDataContext.PracticeUsers
                                          where Pid.UserId == UserId
                                          select Pid.PracticeId).FirstOrDefault());
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetParentName(string tableName, int sectionId)
        {

            try
            {
                if (tableName == enTreeType.ToolSection.ToString())
                {
                    //string parentName = (from a in BMTDataContext.ToolSections
                    //                     join b in BMTDataContext.ToolSections on a.ParentToolSectionId equals b.ToolSectionId
                    //                     where a.ToolSectionId == sectionId
                    //                     select b.Name).FirstOrDefault();
                    //return parentName;

                    string parentName = (from a in BMTDataContext.ToolSections
                                         where a.ToolSectionId == sectionId
                                         select a.Name).FirstOrDefault();

                    return parentName;

                }

                else if (tableName == enTreeType.LibrarySection.ToString())
                {
                    //string parentName = (from a in BMTDataContext.LibrarySections
                    //                     join b in BMTDataContext.LibrarySections on a.ParentLibrarySectionId equals b.LibrarySectionId
                    //                     where a.LibrarySectionId == sectionId
                    //                     select b.Name).FirstOrDefault();

                    string parentName = (from a in BMTDataContext.LibrarySections
                                         where a.LibrarySectionId == sectionId
                                         select a.Name).FirstOrDefault();

                    return parentName;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetParentId(string tableName, int sectionId)
        {

            try
            {
                if (tableName == enTreeType.ToolSection.ToString())
                {
                    int parentId = Convert.ToInt32((from id in BMTDataContext.ToolSections
                                                    where id.ToolSectionId == sectionId && id.ParentToolSectionId != null
                                                    select id.ParentToolSectionId).FirstOrDefault());
                    return parentId;
                }

                if (tableName == enTreeType.LibrarySection.ToString())
                {
                    int parentId = Convert.ToInt32((from id in BMTDataContext.LibrarySections
                                                    where id.LibrarySectionId == sectionId && id.ParentLibrarySectionId != null
                                                    select id.ParentLibrarySectionId).FirstOrDefault());
                    return parentId;
                }
                return -1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetName(string tableName, int sectionId)
        {

            try
            {
                if (tableName == enTreeType.ToolSection.ToString())
                {
                    string Name = (from name in BMTDataContext.ToolSections
                                   where name.ToolSectionId == sectionId
                                   select name.Name).FirstOrDefault();
                    return Name;
                }

                if (tableName == enTreeType.LibrarySection.ToString())
                {
                    string Name = (from name in BMTDataContext.LibrarySections
                                   where name.LibrarySectionId == sectionId
                                   select name.Name).FirstOrDefault();
                    return Name;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletebySectionId(int sectionId, string treeType)
        {
            try
            {
                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    var libRecord = (from libraryDocuments in BMTDataContext.LibraryDocuments
                                     where sectionId == libraryDocuments.LibrarySectionId
                                     select libraryDocuments).ToList();

                    if (libRecord.Count != 0)
                        BMTDataContext.LibraryDocuments.DeleteAllOnSubmit(libRecord);

                    libSectionRcord = (from librarySection in BMTDataContext.LibrarySections
                                       where librarySection.ParentLibrarySectionId == sectionId
                                       select librarySection).ToList();


                    foreach (LibrarySection libSect in libSectionRcord)
                    {
                        var libDocument = (from libraryDocument in BMTDataContext.LibraryDocuments
                                           where libraryDocument.LibrarySectionId == libSect.LibrarySectionId
                                           select libraryDocument).ToList();

                        if (libDocument.Count != 0)
                            BMTDataContext.LibraryDocuments.DeleteAllOnSubmit(libDocument);


                        DeletebySectionId(libSect.LibrarySectionId, treeType);
                    }

                    var librarySect = (from librarySection in BMTDataContext.LibrarySections
                                       where librarySection.LibrarySectionId == sectionId
                                       select librarySection).ToList();

                    BMTDataContext.LibrarySections.DeleteAllOnSubmit(libSectionRcord);
                    BMTDataContext.LibrarySections.DeleteAllOnSubmit(librarySect);
                    BMTDataContext.SubmitChanges();


                    return true;
                }

                if (treeType == enTreeType.ToolSection.ToString())
                {

                    var toolRow = (from toolDocuments in BMTDataContext.ToolDocuments
                                   where sectionId == toolDocuments.ToolSectionId
                                   select toolDocuments).ToList();

                    if (toolRow.Count != 0)
                        BMTDataContext.ToolDocuments.DeleteAllOnSubmit(toolRow);

                    var toolSectionRow = (from toolSection in BMTDataContext.ToolSections
                                          where toolSection.ParentToolSectionId == sectionId
                                          select toolSection).ToList();


                    foreach (ToolSection tool in toolSectionRow)
                    {
                        var toolDoc = (from toolDocument in BMTDataContext.ToolDocuments
                                       where toolDocument.ToolSectionId == tool.ToolSectionId
                                       select toolDocument).ToList();

                        if (toolDoc.Count != 0)
                            BMTDataContext.ToolDocuments.DeleteAllOnSubmit(toolDoc);

                        DeletebySectionId(tool.ToolSectionId, treeType);

                    }

                    var toolSect = (from toolSection in BMTDataContext.ToolSections
                                    where toolSection.ToolSectionId == sectionId
                                    select toolSection).Single();




                    if (toolSect.Name != "My Tools")
                    {
                        BMTDataContext.ToolSections.DeleteAllOnSubmit(toolSectionRow);
                        BMTDataContext.ToolSections.DeleteOnSubmit(toolSect);
                        BMTDataContext.SubmitChanges();
                        return true;
                    }
                    else return false;

                }

                if (treeType == enTreeType.ProjectSection.ToString())
                {
                    var projRecord = (from projectDocuments in BMTDataContext.ProjectDocuments
                                      where sectionId == projectDocuments.ProjectSectionId
                                      select projectDocuments).ToList();

                    if (projRecord.Count != 0)
                        BMTDataContext.ProjectDocuments.DeleteAllOnSubmit(projRecord);

                    var projSectionRcord = (from projectSection in BMTDataContext.ProjectSections
                                            where projectSection.ParentProjectSectionId == sectionId
                                            select projectSection).ToList();


                    foreach (ProjectSection projSect in projSectionRcord)
                    {
                        var projDocument = (from projectDocument in BMTDataContext.ProjectDocuments
                                            where projectDocument.ProjectSectionId == projSect.ProjectSectionId
                                            select projectDocument).ToList();

                        if (projDocument.Count != 0)
                            BMTDataContext.ProjectDocuments.DeleteAllOnSubmit(projDocument);


                        DeletebySectionId(projSect.ProjectSectionId, treeType);
                    }

                    var projectSect = (from projectSection in BMTDataContext.ProjectSections
                                       where projectSection.ProjectSectionId == sectionId
                                       select projectSection).ToList();

                    BMTDataContext.ProjectSections.DeleteAllOnSubmit(projSectionRcord);
                    BMTDataContext.ProjectSections.DeleteAllOnSubmit(projectSect);
                    BMTDataContext.SubmitChanges();


                    return true;
                }
            }

            catch (Exception ex)
            {
                throw ex;

            }

            return true;
        }

        public string DeleteFolderRecursively(int sectionId, string treeType)
        {
            try
            {
                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    sectId += sectionId.ToString() + ",";

                    var libIds = (from librarySection in BMTDataContext.LibrarySections
                                  where librarySection.ParentLibrarySectionId == sectionId
                                  select librarySection).ToList();

                    foreach (var Id in libIds)
                    {
                        sectId += Id.LibrarySectionId.ToString() + ",";
                        DeleteFolderRecursively(Id.LibrarySectionId, treeType);
                    }

                    sectId = sectId.TrimEnd(',');

                    return sectId;
                }

                if (treeType == enTreeType.ToolSection.ToString())
                {
                    sectId += sectionId.ToString() + ",";

                    var toolIds = (from toolSection in BMTDataContext.ToolSections
                                   where toolSection.ParentToolSectionId == sectionId
                                   select toolSection).ToList();

                    foreach (var Id in toolIds)
                    {
                        sectId += Id.ToolSectionId.ToString() + ",";
                        DeleteFolderRecursively(Id.ToolSectionId, treeType);

                    }

                    sectId += sectId.TrimEnd(',');

                    return sectId;
                }

                if (treeType == enTreeType.ProjectSection.ToString())
                {
                    sectId += sectionId.ToString() + ",";

                    var projIds = (from projSection in BMTDataContext.ProjectSections
                                   where projSection.ParentProjectSectionId == sectionId
                                   select projSection).ToList();

                    foreach (var Id in projIds)
                    {
                        sectId += Id.ProjectSectionId.ToString() + ",";
                        DeleteFolderRecursively(Id.ProjectSectionId, treeType);
                    }

                    sectId += sectId.TrimEnd(',');

                    return sectId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public void DeleteEnterpriseEntity(int sectionId, string treeType, int enterpriseId)
        {
            try
            {
                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    var libSecRow = (from x in BMTDataContext.LibrarySections
                                     where x.ParentLibrarySectionId == sectionId
                                     select x).FirstOrDefault();

                    if (libSecRow != null)
                    {
                        DeleteEnterpriseEntity(libSecRow.LibrarySectionId, treeType, enterpriseId);
                    }

                    var enterpriceRow = from x in BMTDataContext.EnterpriseLibrarySections
                                        where x.LibrarySectionId == sectionId
                                        && x.EnterpriseId == enterpriseId
                                        select x;

                    BMTDataContext.EnterpriseLibrarySections.DeleteAllOnSubmit(enterpriceRow);
                    BMTDataContext.SubmitChanges();
                }
                else if (treeType == enTreeType.ProjectSection.ToString())
                {
                    var projSecRow = (from x in BMTDataContext.ProjectSections
                                      where x.ParentProjectSectionId == sectionId
                                      select x).FirstOrDefault();

                    if (projSecRow != null)
                    {
                        DeleteEnterpriseEntity(projSecRow.ProjectSectionId, treeType, enterpriseId);
                    }

                    var enterpriceRow = from x in BMTDataContext.ProjectSections
                                        join projAssig in BMTDataContext.ProjectAssignments
                                        on x.ProjectId equals projAssig.ProjectId
                                        where x.ProjectSectionId == sectionId
                                        && projAssig.EnterpriseId == enterpriseId
                                        select x;

                    BMTDataContext.ProjectSections.DeleteAllOnSubmit(enterpriceRow);
                    BMTDataContext.SubmitChanges();
                }
                else if (treeType == enTreeType.ToolSection.ToString())
                {
                    var toolSecRow = (from x in BMTDataContext.ToolSections
                                      where x.ParentToolSectionId == sectionId
                                      select x).FirstOrDefault();

                    if (toolSecRow != null)
                    {
                        DeleteEnterpriseEntity(toolSecRow.ToolSectionId, treeType, enterpriseId);
                    }

                    var enterpriceRow = from x in BMTDataContext.EnterpriseToolSections
                                        where x.ToolSectionId == sectionId
                                        && x.EnterpriseId == enterpriseId
                                        select x;

                    BMTDataContext.EnterpriseToolSections.DeleteAllOnSubmit(enterpriceRow);
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddTreeNode(int sectionid, string treeType, string folderName, int enterpriseId, int sessionEnterpriseId, int isJumpMaterialFolder)
        {
            try
            {

                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        LibrarySection libSection = new LibrarySection();
                        libSection.ParentLibrarySectionId = sectionid;

                        var v = (from librarySection in BMTDataContext.LibrarySections
                                 select librarySection.LibrarySectionId).Max();

                        libSection.LibrarySectionId = v + 1;
                        libSection.Name = folderName;
                        libSection.ContentType = folderName;

                        BMTDataContext.LibrarySections.InsertOnSubmit(libSection);
                        BMTDataContext.SubmitChanges();

                        EnterpriseLibrarySection enterpriceLibrarySection = new EnterpriseLibrarySection();
                        enterpriceLibrarySection.LibrarySectionId = libSection.LibrarySectionId;
                        enterpriceLibrarySection.EnterpriseId = sessionEnterpriseId;

                        BMTDataContext.EnterpriseLibrarySections.InsertOnSubmit(enterpriceLibrarySection);
                        BMTDataContext.SubmitChanges();

                        scope.Complete();
                        return libSection.LibrarySectionId;

                    }

                }

                if (treeType == enTreeType.ToolSection.ToString())
                {
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        ToolSection toolSection = new ToolSection();
                        toolSection.ParentToolSectionId = sectionid;

                        var v = (from toolSect in BMTDataContext.ToolSections
                                 select toolSect.ToolSectionId).Max();

                        toolSection.ToolSectionId = v + 1;
                        toolSection.Name = folderName;
                        toolSection.ContentType = folderName;

                        BMTDataContext.ToolSections.InsertOnSubmit(toolSection);
                        BMTDataContext.SubmitChanges();

                        if (enterpriseId == 0)
                        {
                            EnterpriseToolSection enterpriceToolSection = new EnterpriseToolSection();
                            enterpriceToolSection.ToolSectionId = toolSection.ToolSectionId;
                            enterpriceToolSection.EnterpriseId = sessionEnterpriseId;

                            BMTDataContext.EnterpriseToolSections.InsertOnSubmit(enterpriceToolSection);
                            BMTDataContext.SubmitChanges();
                        }

                        scope.Complete();

                        return toolSection.ToolSectionId;
                    }

                }

                if (treeType == enTreeType.ProjectSection.ToString())
                {
                    using (var scope = new System.Transactions.TransactionScope())
                    {
                        ProjectSection pSec = (from projSec in BMTDataContext.ProjectSections
                                               where projSec.ProjectSectionId == sectionid
                                               select projSec).FirstOrDefault();

                        pSec.SectionTypeId = (int)enSectionType.PlaceHolder;

                        ProjectSection projectSection = new ProjectSection();
                        projectSection.ParentProjectSectionId = sectionid;

                        //var v = (from projSect in BMTDataContext.ProjectSections
                        //         select projSect.ProjectSectionId).Max();

                        //projectSection.ProjectSectionId = v + 1;
                        projectSection.Name = folderName;
                        projectSection.SectionTypeId = (int)enSectionType.Folder;
                        projectSection.ProjectId = pSec.ProjectId;
                        projectSection.AccessLevelId = pSec.AccessLevelId;
                        //if (enterpriseId > 0)
                        //    projectSection.ContentType = "RestrictedFolder";
                        //else if (isJumpMaterialFolder == 1)
                        //    projectSection.ContentType = "JumpStartMaterials";


                        BMTDataContext.ProjectSections.InsertOnSubmit(projectSection);
                        BMTDataContext.SubmitChanges();

                        //if (enterpriseId == 0)
                        //{
                        //    ProjectSection enterpriseProjectSection = new ProjectSection();
                        //    enterpriseProjectSection.ProjectSectionId = projectSection.ProjectSectionId;
                        //    enterpriseProjectSection.EnterpriseId = sessionEnterpriseId;

                        //    BMTDataContext.ProjectSections.InsertOnSubmit(enterpriseProjectSection);
                        //    BMTDataContext.SubmitChanges();
                        //}
                        scope.Complete();

                        return projectSection.ProjectSectionId;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        public void RenameNode(string treeType, int sectionId, string newFolderName)
        {
            try
            {
                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    var libRow = (from librarySection in BMTDataContext.LibrarySections
                                  where librarySection.LibrarySectionId == sectionId
                                  select librarySection).Single();


                    libRow.Name = newFolderName;
                    BMTDataContext.SubmitChanges();

                }

                if (treeType == enTreeType.ToolSection.ToString())
                {
                    var toolRow = (from toolSection in BMTDataContext.ToolSections
                                   where toolSection.ToolSectionId == sectionId
                                   select toolSection).Single();

                    toolRow.Name = newFolderName;
                    BMTDataContext.SubmitChanges();
                }

                if (treeType == enTreeType.ProjectSection.ToString())
                {
                    var projRow = (from projSection in BMTDataContext.ProjectSections
                                   where projSection.ProjectSectionId == sectionId
                                   select projSection).Single();

                    projRow.Name = newFolderName;
                    BMTDataContext.SubmitChanges();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string GetNodeName(int sectionId, string treeType)
        {
            try
            {
                string sLibName = string.Empty;

                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    var libRow = (from libSection in BMTDataContext.LibrarySections
                                  //where libSection.ParentLibrarySectionId == sectionId
                                  select libSection).ToList();

                    foreach (var rowName in libRow)
                    {
                        sLibName = sLibName + rowName.Name + ',';
                    }
                    return sLibName;
                }
                else if (treeType == enTreeType.ToolSection.ToString())
                {
                    var toolRow = (from toolSection in BMTDataContext.ToolSections
                                   where toolSection.ParentToolSectionId == sectionId
                                   select toolSection).ToList();

                    foreach (var rowName in toolRow)
                    {
                        sLibName = sLibName + rowName.Name + ',';
                    }
                    return sLibName;
                }
                else if (treeType == enTreeType.ProjectSection.ToString())
                {
                    var projectRow = (from projectSection in BMTDataContext.ProjectSections
                                      where projectSection.ParentProjectSectionId == sectionId
                                      select projectSection).ToList();
                    foreach (var rowName in projectRow)
                    {
                        sLibName = sLibName + rowName.Name + ',';
                    }
                    return sLibName;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        public string GetTopMostNode(int sectionId, string treeType)
        {
            try
            {
                if (treeType == enTreeType.LibrarySection.ToString())
                {
                    var libRecord = (from librarySect in BMTDataContext.LibrarySections
                                     where librarySect.LibrarySectionId == sectionId
                                     select librarySect).ToList();

                    if (libRecord.Count != 0)
                    {
                        foreach (var pname in libRecord)
                        {
                            if (pname.ParentLibrarySectionId != null)
                            {
                                idsList += pname.LibrarySectionId + ",";
                                GetTopMostNode((int)pname.ParentLibrarySectionId, treeType);
                            }
                            else
                            {
                                if (pname.ParentLibrarySectionId == null)
                                {
                                    parentId = (int)pname.LibrarySectionId;
                                    idsList += parentId.ToString() + ",";

                                }
                            }

                        }
                    }

                    idsList = idsList.TrimEnd(',');

                    return idsList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "null";
        }

        public bool AddTopLevelFolder(string folderName, enTreeType dbTableName, int enterpriseId)
        {
            try
            {
                if (dbTableName == enTreeType.LibrarySection)
                {
                    var row = (from record in BMTDataContext.LibrarySections
                               where record.Name == folderName
                               && record.ParentLibrarySectionId == null
                               select record);
                    if (row.Count() == 0)
                    {
                        LibrarySection libSection = new LibrarySection();
                        libSection.Name = folderName;
                        libSection.ContentType = folderName;

                        // Insert Top level Node
                        BMTDataContext.LibrarySections.InsertOnSubmit(libSection);
                        BMTDataContext.SubmitChanges();

                        EnterpriseLibrarySection enterpriceLibSec = new EnterpriseLibrarySection();
                        enterpriceLibSec.LibrarySectionId = libSection.LibrarySectionId;
                        enterpriceLibSec.EnterpriseId = enterpriseId;

                        BMTDataContext.EnterpriseLibrarySections.InsertOnSubmit(enterpriceLibSec);
                        BMTDataContext.SubmitChanges();

                    }
                    else
                        return false;
                }

                else if (dbTableName == enTreeType.ToolSection)
                {
                    var row = (from record in BMTDataContext.ToolSections
                               where record.Name == folderName
                                && record.ParentToolSectionId == null
                               select record);
                    if (row.Count() == 0)
                    {
                        ToolSection toolSection = new ToolSection();
                        toolSection.Name = folderName;
                        toolSection.ContentType = folderName;

                        // Insert Top Level Folder
                        BMTDataContext.ToolSections.InsertOnSubmit(toolSection);
                        BMTDataContext.SubmitChanges();

                        EnterpriseToolSection enterpriceToolSec = new EnterpriseToolSection();
                        enterpriceToolSec.ToolSectionId = toolSection.ToolSectionId;
                        enterpriceToolSec.EnterpriseId = enterpriseId;

                        BMTDataContext.EnterpriseToolSections.InsertOnSubmit(enterpriceToolSec);
                        BMTDataContext.SubmitChanges();

                    }
                    else
                        return false;
                }


                return true;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool IsSecurityRiskAssessmentExist(int enterpriseId)
        {
            try
            {
                var projectSections = (from projectRecord in BMTDataContext.ProjectSections
                                       join projAssess in BMTDataContext.ProjectAssignments
                                       on projectRecord.ProjectId equals projAssess.ProjectId
                                       where projAssess.EnterpriseId == enterpriseId
                                       //&& projectRecord.ContentType == "Security Risk Assessment"
                                       select projectRecord).FirstOrDefault();

                if (projectSections == null)
                    return false;
                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<TreeDetail> GetProjectsByPracticeId(int practiceId)
        {
            try
            {
                List<TreeDetail> templateName = null;
                //List<TreeDetail> templateName = (from pracTempRec in BMTDataContext.PracticeTemplates
                //                                 join tempRec in BMTDataContext.Templates
                //                                 on pracTempRec.TemplateId equals tempRec.TemplateId
                //                                 where pracTempRec.PracticeId == practiceId && pracTempRec.Visible == true
                //                                 orderby pracTempRec.Sequence
                //                                 select new TreeDetail
                //                                 {
                //                                     SectionId = (int)enProjectSectionMORe.SectionId,
                //                                     ParentSectionId = (int)enProjectSectionMORe.ParentSectionId,
                //                                     Name = tempRec.Name,
                //                                     ContentType = "MOReRequirements",
                //                                     PracticeId = practiceId,
                //                                     IsTopNode = false,
                //                                 }).ToList();

                return templateName;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int GetTemplateID(string templateName)
        {
            try
            {
                int TempId = (from temp in BMTDataContext.Templates
                              where temp.Name == templateName
                              select temp.TemplateId).FirstOrDefault();

                return TempId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int GetSectionId(int templateId)
        {
            try
            {
                int sectionId = 0;
                Template template = (from temp in BMTDataContext.Templates
                                     where temp.TemplateId == templateId
                                     select temp).FirstOrDefault();

                //if (template.ProjectSectionId != null)
                //    sectionId = Convert.ToInt32(template.ProjectSectionId);

                return sectionId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public ProjectSection GetProjectSection(int sectionId)
        {
            try
            {
                ProjectSection Section = (from section in BMTDataContext.ProjectSections
                                          where section.ProjectSectionId == sectionId
                                          select section).FirstOrDefault();

                return Section;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public ProjectSection GetChildProjectSection(int sectionId)
        {
            try
            {
                ProjectSection Section = (from section in BMTDataContext.ProjectSections
                                          where section.ParentProjectSectionId == sectionId
                                          select section).FirstOrDefault();

                return Section;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public int GetTemplateUtilityFolderId(string text)
        {
            try
            {
                int sectionId = 0;
                ProjectSection section = (from pSec in BMTDataContext.ProjectSections
                                          where pSec.Name == text
                                          select pSec).FirstOrDefault();

                if (section != null)
                {
                    if (section.ProjectSectionId != null)
                        sectionId = Convert.ToInt32(section.ProjectSectionId);
                }
                return sectionId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public bool IsOneLevelFolder(int projectSectionId, enTreeType treeType)
        {
            try
            {
                string flag = null;
                switch (treeType)
                {
                    case enTreeType.ProjectSection:
                        List<ProjectSection> projSection = (from projSecRec in BMTDataContext.ProjectSections
                                                            where projSecRec.ParentProjectSectionId == projectSectionId
                                                            select projSecRec).ToList();
                        if (projSection.Count == 0)
                            flag = "true";
                        else
                            flag = "false";
                        break;

                    case enTreeType.ToolSection:
                        List<ToolSection> ToolSection = (from projSecRec in BMTDataContext.ToolSections
                                                         where projSecRec.ParentToolSectionId == projectSectionId
                                                         select projSecRec).ToList();
                        if (ToolSection.Count == 0)
                            flag = "true";
                        else
                            flag = "false";
                        break;
                }
                if (flag == "true")
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;

            }
        }

        public List<TreeDetail> GetTreeNodesByProjectAssignment(enTreeType treeType, int enterpriseId, int mediacalGroupId, int practiceId)
        {
            try
            {

                List<int> projectIds = (from projRec in BMTDataContext.ProjectUsages
                                        where projRec.PracticeId == practiceId &&
                                        projRec.IsVisible == true
                                        orderby projRec.SequenceNo
                                        select projRec.ProjectId).ToList();

                List<int> sectionIds = ((from pSec1 in BMTDataContext.ProjectSections
                                         where pSec1.AccessLevelId == (int)enAccessLevelId.Public &&
                                         projectIds.Contains(pSec1.ProjectId)
                                         select pSec1.ProjectSectionId).Concat
                                            (from psec2 in BMTDataContext.ProjectSections
                                             join pAss in BMTDataContext.ProjectAssignments
                                             on psec2.ProjectSectionId equals pAss.ProjectSectionId
                                             where projectIds.Contains(psec2.ProjectId) &&
                                             ((psec2.AccessLevelId == (int)enAccessLevelId.MedicalGroup && pAss.MedicalGroupId == mediacalGroupId) ||
                                             (psec2.AccessLevelId == (int)enAccessLevelId.Enterprise && pAss.EnterpriseId == enterpriseId)||
                                             (psec2.AccessLevelId == (int)enAccessLevelId.Practice && pAss.PracticeId == practiceId))
                                             select psec2.ProjectSectionId).Concat
                                             (from psec3 in BMTDataContext.ProjectSections
                                              join pAss1 in BMTDataContext.ProjectAssignments
                                              on psec3.ProjectId equals pAss1.ProjectId
                                              where projectIds.Contains(psec3.ProjectId) &&
                                              ((psec3.AccessLevelId == (int)enAccessLevelId.MedicalGroup && pAss1.MedicalGroupId == mediacalGroupId) ||
                                              (psec3.AccessLevelId == (int)enAccessLevelId.Enterprise && pAss1.EnterpriseId == enterpriseId)||
                                              (psec3.AccessLevelId == (int)enAccessLevelId.Practice && pAss1.PracticeId == practiceId))
                                              select psec3.ProjectSectionId)).ToList();

                treeDataResult = (from projectRecord in BMTDataContext.ProjectSections
                                  join projectUsage in BMTDataContext.ProjectUsages
                                  on projectRecord.ProjectId equals projectUsage.ProjectId
                                  where sectionIds.Contains(projectRecord.ProjectSectionId)
                                  && projectUsage.PracticeId == practiceId
                                  orderby projectUsage.SequenceNo, projectRecord.SequenceNo
                                  select new TreeDetail
                                  {
                                      Name = projectRecord.Name,
                                      SectionId = projectRecord.ProjectSectionId,
                                      ParentSectionId = projectRecord.ParentProjectSectionId

                                  }).ToList();

                //treeDataResult = (from projectRecord in BMTDataContext.ProjectSections
                //                  join projectUsage in BMTDataContext.ProjectUsages
                //                  on projectRecord.ProjectId equals projectUsage.ProjectId
                //                  where projectUsage.PracticeId == practiceId &&
                //                  projectUsage.IsVisible == true
                //                  orderby projectUsage.SequenceNo
                //                  select new TreeDetail
                //                  {
                //                      Name = projectRecord.Name,
                //                      SectionId = projectRecord.ProjectSectionId,
                //                      ParentSectionId = projectRecord.ParentProjectSectionId

                //                  }).ToList();

                return treeDataResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMedicalGroupIdByPracticeId(int practiceId)
        {
            try
            {
                int medicalGroupId = (from prac in BMTDataContext.Practices
                                      where prac.PracticeId == practiceId
                                      select prac.MedicalGroupId).FirstOrDefault();

                return medicalGroupId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetProjectParentNameBySectionId(long projectSectionId)
        {
            try
            {
                string projectSectionName = (from projSecRec in BMTDataContext.ProjectSections
                                             where projSecRec.ProjectSectionId == projectSectionId
                                             select projSecRec.Name).FirstOrDefault();
                return projectSectionName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsSiteLevelTool(int parentProjectSectionId)
        {
            try
            {
                int? toolLevelId = (from projSec in BMTDataContext.ProjectSections
                                    where projSec.ParentProjectSectionId == parentProjectSectionId
                                    && projSec.Name == "Tools"
                                    select projSec.ToolLevelId).FirstOrDefault();

                if (toolLevelId != null)
                {
                    if (toolLevelId == 2)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ToolHasDocumemtStore(int projectSectionId)
        {
            try
            {
                int? fkTempId = (from projSecRec in BMTDataContext.ProjectSections
                                 where projSecRec.ProjectSectionId == projectSectionId
                                 select projSecRec.TemplateId).FirstOrDefault();
                if (fkTempId != null)
                {
                    bool? hasDocStore = (from temp in BMTDataContext.Templates
                                         where temp.TemplateId == fkTempId
                                         select temp.HasDocumentStore).FirstOrDefault();
                    if (hasDocStore != null)
                    {
                        return (bool)hasDocStore;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ToolHasStandardFolder(int projectSectionId)
        {
            try
            {
                int? fkTempId = (from projSecRec in BMTDataContext.ProjectSections
                                 where projSecRec.ProjectSectionId == projectSectionId
                                 select projSecRec.TemplateId).FirstOrDefault();
                if (fkTempId != null)
                {
                    bool? hasStandardFolder = (from temp in BMTDataContext.Templates
                                               where temp.TemplateId == fkTempId
                                               select temp.HasStandardFolder).FirstOrDefault();
                    if (hasStandardFolder != null)
                    {
                        return (bool)hasStandardFolder;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetNameOfDocumentStore(int projSectionIdOfToolHasDocs)
        {
            try
            {
                int? fkTempId = (from projSecRec in BMTDataContext.ProjectSections
                                 where projSecRec.ProjectSectionId == projSectionIdOfToolHasDocs
                                 select projSecRec.TemplateId).FirstOrDefault();
                if (fkTempId != null)
                {
                    string docStoreName = (from temp in BMTDataContext.Templates
                                           where temp.TemplateId == fkTempId
                                           select temp.DocumentStoreName).FirstOrDefault();
                    if (docStoreName != null)
                    {
                        return docStoreName;
                    }
                    else
                        return "";
                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetNameOfStandardFolder(int projSectionIdOfToolHasDocs)
        {
            try
            {
                int? fkTempId = (from projSecRec in BMTDataContext.ProjectSections
                                 where projSecRec.ProjectSectionId == projSectionIdOfToolHasDocs
                                 select projSecRec.TemplateId).FirstOrDefault();
                if (fkTempId != null)
                {
                    string standardFolderName = (from temp in BMTDataContext.Templates
                                                 join stdFolder in BMTDataContext.StandardFolders
                                                 on temp.StandardFolderId equals stdFolder.StandardFolderId
                                                 where temp.TemplateId == fkTempId
                                                 select stdFolder.Name).FirstOrDefault();
                    if (standardFolderName != null)
                    {
                        return standardFolderName;
                    }
                    else
                        return "";
                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetSectionTypeBySectionId(int sectionId)
        {
            try
            {
                string sectionType = (from projSec in BMTDataContext.ProjectSections
                                      join secType in BMTDataContext.SectionTypes
                                      on projSec.SectionTypeId equals secType.SectionTypeId
                                      where projSec.ProjectSectionId == sectionId
                                      select secType.SectionName).FirstOrDefault();

                return sectionType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetSiteIDByPracticeId(int practiceId)
        {
            try
            {
                int siteId = (from pracSite in BMTDataContext.PracticeSites
                              where pracSite.PracticeId==practiceId
                              select pracSite.PracticeSiteId).FirstOrDefault();


                return siteId;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion
    }
}
