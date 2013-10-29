#region Modification History

//  ******************************************************************************
//  Module        : Project
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    04-06-2012      Add Null handling in GetSiteIDByProjectID.
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class ProjectBO : BMTConnection
    {
        #region CONSTANT
        private const string DEFAULT_NAME = "Super Happy Pediatrics";
        private const string DEFAULT_DESCRIPTION = "Dafault Project";
        int? NULL_STRING = null;
        private const string Project_Folder = "ProjectFolder";
        private char[] DELIMITATORS = new char[] { ',' };

        #endregion

        #region PROPERTIES
        private Project _project { get; set; }

        private ProjectAssignment _projectAssignment { get; set; }

        private int _projectId;
        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        private string _name;
        public string Name
        {

            get { return _name; }
            set { _name = value; }
        }

        private string _description;
        public string Description
        {

            get { return _description; }
            set { _description = value; }
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

        private IQueryable _projectList;
        public IQueryable ProjectList
        {

            get { return _projectList; }
            set { _projectList = value; }
        }

        private int _practiceSiteId;
        public int PracticeSiteId
        {
            get { return _practiceSiteId; }
            set { _practiceSiteId = value; }
        }

        private int _practiceId;
        public int PracticeId
        {
            get { return _practiceId; }
            set { _practiceId = value; }
        }

        private int _accessLevelId;
        public int AccessLevelId
        {
            get { return _accessLevelId; }
            set { _accessLevelId = value; }
        }

        private int _lastUpdatedBy;
        public int LastUpdatedBy
        {
            get { return _lastUpdatedBy; }
            set { _lastUpdatedBy = value; }
        }

        private DateTime _lastUpdatedDate;
        public DateTime LastUpdatedDate
        {

            get { return _lastUpdatedDate; }
            set { _lastUpdatedDate = value; }
        }

        private string[] _tempName;
        public string[] TempName
        {
            get { return _tempName; }
            set { _tempName = value; }
        }

        private int _MedicalGroupId;
        public int MedicalGroupId
        {
            get { return _MedicalGroupId; }
            set { _MedicalGroupId = value; }
        }

        private List<int> _MedicalGroupIds;
        public List<int> MedicalGroupIds
        {
            get { return _MedicalGroupIds; }
            set { _MedicalGroupIds = value; }
        }

        private List<int> _EnterpriseIds;
        public List<int> EnterpriseIds
        {
            get { return _EnterpriseIds; }
            set { _EnterpriseIds = value; }
        }

        private int _EnterpriseId;
        public int EnterpriseId
        {
            get { return _EnterpriseId; }
            set { _EnterpriseId = value; }
        }

        private List<int> _PracticeIds;
        public List<int> PracticeIds
        {
            get { return _PracticeIds; }
            set { _PracticeIds = value; }
        }

        private bool _AddProjectFolder;
        public bool AddProjectFolder
        {
            get { return _AddProjectFolder; }
            set { _AddProjectFolder = value; }
        }

        private string[] _formName;
        public string[] FormName
        {

            get { return _formName; }
            set { _formName = value; }
        }

        private string[] _folderList;
        public string[] FolderList
        {

            get { return _folderList; }
            set { _folderList = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public ProjectBO()
        {

            _name = DEFAULT_NAME;
            _description = DEFAULT_DESCRIPTION;
            _createdDate = System.DateTime.Now;
        }


        #endregion

        #region Functions

        public void GetProjectByUserId(int userId)
        {
            try
            {
                _projectList = (from userRecord in BMTDataContext.PracticeUsers
                                join ProjectRecord in BMTDataContext.Projects
                                    on userRecord.PracticeSiteId equals ProjectRecord.ProjectId
                                where userRecord.UserId == userId
                                select new { ID = ProjectRecord.ProjectId, Name = ProjectRecord.Name }).AsQueryable();

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public string GetProjectNameByProjectID(int ProjectId)
        {
            try
            {
                string projectName = (from project in BMTDataContext.Projects
                                      where project.ProjectId == ProjectId
                                      select project.Name).FirstOrDefault();
                return projectName;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public string GetSiteNameByProjectID(int practiceSiteId)
        {
            try
            {
                string sitename = (from a in BMTDataContext.PracticeSites
                                   where a.PracticeSiteId == practiceSiteId
                                   select a.Name).FirstOrDefault();
                return sitename;


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public int GetSiteIDByProjectID(int ProjectId)
        {
            try
            {
                //var siteId = (from projectRecord in BMTDataContext.Projects
                //              where projectRecord.ProjectId == ProjectId
                //              select projectRecord.PracticeSiteId).FirstOrDefault();

                //if (siteId != null)
                //    return Convert.ToInt32(siteId);
                //else
                return 0;

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public string GetPracticeNameByPracticeID(int practiceId)
        {
            try
            {
                string pName = (from name in BMTDataContext.Practices
                                where name.PracticeId == practiceId
                                select name.Name).FirstOrDefault();

                return pName;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetNodeNameBySectionId(int sectionId)
        {
            try
            {
                string Name = (from name in BMTDataContext.ProjectSections
                               where name.ProjectSectionId == sectionId
                               select name.Name).FirstOrDefault();

                return Name;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetPracticeProjectId()
        {
            try
            {
                this.ProjectId = (from projectRecord in BMTDataContext.Projects
                                  //where projectRecord.PracticeId == this.PracticeId
                                  select projectRecord.ProjectId).FirstOrDefault();
                if (this.ProjectId != 0)
                    return this.ProjectId;
                else
                    return CreateProject();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetProjects(int EnterpriseId, int MedicalGroupId, int PracticeId,string userType)
        {
            IQueryable getProjects = null;
            try
            {
                if (userType == enUserRole.SuperUser.ToString())
                {
                    getProjects = (from proj in BMTDataContext.Projects
                                   join projasg in BMTDataContext.ProjectAssignments
                                   on proj.ProjectId equals projasg.ProjectId
                                   join user in BMTDataContext.Users
                                   on proj.CreatedBy equals user.UserId
                                   where projasg.EnterpriseId == EnterpriseId ||
                                   projasg.MedicalGroupId == MedicalGroupId ||
                                   projasg.PracticeId == PracticeId
                                   select new
                                   {
                                       ProjectId = proj.ProjectId,
                                       Name = proj.Name,
                                       Description = proj.Description,
                                       TemplateAccess = proj.AccessLevelId,
                                       CreatedDate = proj.CreatedOn.ToShortDateString(),
                                       CreatedBy = user.Username,
                                   }) as IQueryable;
                }
                else if (userType == enUserRole.SuperAdmin.ToString())
                {
                    getProjects = (from proj in BMTDataContext.Projects
                                   join projasg in BMTDataContext.ProjectAssignments
                                   on proj.ProjectId equals projasg.ProjectId
                                   join user in BMTDataContext.Users
                                   on proj.CreatedBy equals user.UserId
                                   where projasg.EnterpriseId == EnterpriseId ||
                                   projasg.MedicalGroupId == MedicalGroupId ||
                                   projasg.PracticeId == PracticeId
                                   select new
                                   {
                                       ProjectId = proj.ProjectId,
                                       Name = proj.Name,
                                       Description = proj.Description,
                                       TemplateAccess = proj.AccessLevelId,
                                       CreatedDate = proj.CreatedOn.ToShortDateString(),
                                       CreatedBy = user.Username,
                                   }).Concat 
                                   (from proj in BMTDataContext.Projects
                                   join user in BMTDataContext.Users
                                   on proj.CreatedBy equals user.UserId
                                   where proj.AccessLevelId==(int)enAccessLevelId.Public
                                   select new
                                   {
                                       ProjectId = proj.ProjectId,
                                       Name = proj.Name,
                                       Description = proj.Description,
                                       TemplateAccess = proj.AccessLevelId,
                                       CreatedDate = proj.CreatedOn.ToShortDateString(),
                                       CreatedBy = user.Username,
                                   }) as IQueryable;

                }
                return getProjects;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Template> GetTemplates()
        {

            List<Template> Id = (from temp in BMTDataContext.Templates
                                 where TempName.Contains(temp.Name)
                                 select temp).ToList();

            return Id;

        }

        public List<Form> GetForms()
        {

            List<Form> Id = (from forms in BMTDataContext.Forms
                             where FormName.Contains(forms.Name)
                             select forms).ToList();

            return Id;

        }

        public Project GetProjectByProjectId(int projectId)
        {
            Project getProject = null;
            try
            {
                getProject = (from proj in BMTDataContext.Projects
                              where proj.ProjectId == projectId
                              select proj).FirstOrDefault();

                return getProject;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IQueryable GetAllowAccess()
        {
            IQueryable allowAccess = null;
            try
            {
                allowAccess = (from access in BMTDataContext.AccessLevels
                               select access).AsQueryable();

                return allowAccess;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int CreateProject()
        {
            try
            {
                Project _project = new Project();

                if (this.PracticeId != 0)
                    _project.ProjectId = this.PracticeId;

                if (this.PracticeSiteId != 0)
                    _project.ProjectId = this.PracticeSiteId;

                _project.Name = this.Name;
                _project.Description = this.Description;
                _project.CreatedOn = this.CreatedDate;
                _project.CreatedBy = this.CreatedBy;

                if (this.PracticeId != 0 || this.PracticeSiteId != 0)
                {
                    BMTDataContext.Projects.InsertOnSubmit(_project);
                    BMTDataContext.SubmitChanges();
                    this.ProjectId = _project.ProjectId;
                }

                return this.ProjectId;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveProject()
        {
            try
            {
                _project = new Project();
                _project.Name = this.Name;
                _project.Description = this.Description;
                _project.AccessLevelId = this.AccessLevelId;
                _project.CreatedBy = this.CreatedBy;
                _project.CreatedOn = this.CreatedDate;
                _project.LastUpdatedBy = this.LastUpdatedBy;
                _project.LastUpdatedOn = this.LastUpdatedDate;

                BMTDataContext.Projects.InsertOnSubmit(_project);
                BMTDataContext.SubmitChanges();





                if (this.AccessLevelId == (int)enAccessLevelId.Enterprise)
                {
                    foreach (int enterprsie in EnterpriseIds)
                    {
                        _projectAssignment = new ProjectAssignment();
                        _projectAssignment.ProjectId = _project.ProjectId;
                        _projectAssignment.EnterpriseId = enterprsie;

                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }

                }

                else if (this.AccessLevelId == (int)enAccessLevelId.Practice)
                {
                    foreach (int practice in PracticeIds)
                    {
                        _projectAssignment = new ProjectAssignment();
                        _projectAssignment.ProjectId = _project.ProjectId;
                        _projectAssignment.PracticeId = practice;

                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }
                }

                else if (this.AccessLevelId == (int)enAccessLevelId.MedicalGroup)
                {
                    foreach (int medicalGroup in MedicalGroupIds)
                    {
                        _projectAssignment = new ProjectAssignment();
                        _projectAssignment.ProjectId = _project.ProjectId;
                        _projectAssignment.MedicalGroupId = medicalGroup;

                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }
                }

                ProjectSection pSection = new ProjectSection();

                pSection.ParentProjectSectionId = null;
                pSection.Name = _project.Name;
                pSection.SectionTypeId = (int)enSectionType.PlaceHolder;
                pSection.ProjectId = _project.ProjectId;
                pSection.TemplateId = null;
                pSection.AccessLevelId = _project.AccessLevelId;

                BMTDataContext.ProjectSections.InsertOnSubmit(pSection);
                BMTDataContext.SubmitChanges();

                if (AddProjectFolder)
                {
                    ProjectSection pSectionChild = new ProjectSection();

                    pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
                    pSectionChild.Name = "Utility Folder";
                    pSectionChild.ProjectId = _project.ProjectId;
                    pSectionChild.SectionTypeId = (int)enSectionType.PlaceHolder;
                    pSectionChild.AccessLevelId = _project.AccessLevelId;

                    BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                    BMTDataContext.SubmitChanges();

                    ProjectSection pSectionChildNode = new ProjectSection();

                    pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                    pSectionChildNode.Name = "Uploaded Documents";
                    pSectionChildNode.SectionTypeId = (int)enSectionType.ProjectFolder;
                    pSectionChildNode.AccessLevelId = _project.AccessLevelId;
                    pSectionChildNode.ProjectId = _project.ProjectId;
                    BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                    BMTDataContext.SubmitChanges();

                }

                if (FolderList.Count() != 0)
                {
                    ProjectSection newPSection = new ProjectSection();

                    foreach (string folder in FolderList)
                    {
                        string[] newFolder = folder.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
                        if (newFolder.Count() != 0)
                        {

                            if (newFolder[1].ToString() == Project_Folder)
                            {
                                ProjectSection Folder = new ProjectSection();

                                Folder.ParentProjectSectionId = pSection.ProjectSectionId;
                                Folder.Name = newFolder[0].ToString();
                                Folder.ProjectId = _project.ProjectId;
                                Folder.SectionTypeId = (int)enSectionType.Folder;
                                Folder.AccessLevelId = _project.AccessLevelId;

                                BMTDataContext.ProjectSections.InsertOnSubmit(Folder);
                                BMTDataContext.SubmitChanges();

                                newPSection = Folder;
                            }

                            else if (newFolder[1] == newPSection.Name)
                            {
                                ProjectSection Folder = new ProjectSection();

                                Folder.ParentProjectSectionId = newPSection.ProjectSectionId;
                                Folder.Name = newFolder[0].ToString();
                                Folder.ProjectId = _project.ProjectId;
                                Folder.SectionTypeId = (int)enSectionType.Folder;
                                Folder.AccessLevelId = _project.AccessLevelId;

                                BMTDataContext.ProjectSections.InsertOnSubmit(Folder);
                                BMTDataContext.SubmitChanges();
                            }
                        }
                    }
                }

                if (GetTemplates().Count != 0 || GetForms().Count != 0)
                {
                    ProjectSection pSectionChild = new ProjectSection();

                    pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
                    pSectionChild.Name = "Tools";
                    pSectionChild.ProjectId = _project.ProjectId;
                    pSectionChild.SectionTypeId = (int)enSectionType.PlaceHolder;
                    pSectionChild.AccessLevelId = _project.AccessLevelId;
                    pSectionChild.TemplateId = null;
                    List<Template> temps = GetTemplates();
                    if (temps.Count != 0)
                    {
                        pSectionChild.ToolLevelId = (from temp in BMTDataContext.Templates
                                                     where temp.Name == TempName[0].ToString()
                                                     select temp.ToolLevelId).FirstOrDefault();

                        BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                        BMTDataContext.SubmitChanges();

                        foreach (Template tmp in temps)
                        {
                            ProjectSection pSectionChildNode = new ProjectSection();

                            pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                            pSectionChildNode.Name = tmp.Name;
                            pSectionChildNode.TemplateId = tmp.TemplateId;
                            pSectionChildNode.SectionTypeId = (int)enSectionType.Template;
                            pSectionChildNode.AccessLevelId = _project.AccessLevelId;
                            pSectionChildNode.ProjectId = _project.ProjectId;
                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                            BMTDataContext.SubmitChanges();
                        }
                    }
                    if (GetForms().Count != 0)
                    {
                        if (GetTemplates().Count == 0)
                        {
                            pSectionChild.ToolLevelId = (from form in BMTDataContext.Forms
                                                         where form.Name == FormName[0].ToString()
                                                         select form.ToolLevelId).FirstOrDefault();

                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                            BMTDataContext.SubmitChanges();

                        }
                        List<Form> forms = GetForms();
                        foreach (Form form in forms)
                        {
                            ProjectSection pSectionChildNode = new ProjectSection();

                            pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                            pSectionChildNode.Name = form.Name;
                            pSectionChildNode.FormId = form.FormId;
                            pSectionChildNode.SectionTypeId = (int)enSectionType.Form;
                            pSectionChildNode.AccessLevelId = _project.AccessLevelId;
                            pSectionChildNode.ProjectId = _project.ProjectId;
                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                            BMTDataContext.SubmitChanges();
                        }
                    }

                }

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool UpdateProject(int projectId)
        {
            try
            {
                var project = (from proj in BMTDataContext.Projects
                               where proj.ProjectId == projectId
                               select proj).First();

                project.Name = this.Name;
                project.Description = this.Description;
                project.AccessLevelId = this.AccessLevelId;
                project.LastUpdatedBy = this.LastUpdatedBy;
                project.LastUpdatedOn = this.LastUpdatedDate;

                BMTDataContext.SubmitChanges();

                List<ProjectAssignment> projectAssignment = (from projAssignment in BMTDataContext.ProjectAssignments
                                                             where projAssignment.ProjectId == projectId
                                                             select projAssignment).ToList();

                BMTDataContext.ProjectAssignments.DeleteAllOnSubmit(projectAssignment);
                BMTDataContext.SubmitChanges();

                if (this.AccessLevelId == (int)enAccessLevelId.Enterprise)
                {
                    foreach (int enterprise in EnterpriseIds)
                    {
                        _projectAssignment = new ProjectAssignment();

                        _projectAssignment.ProjectId = project.ProjectId;
                        _projectAssignment.EnterpriseId = enterprise;
                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }
                }

                else if (this.AccessLevelId == (int)enAccessLevelId.Practice)
                {
                    foreach (int practice in PracticeIds)
                    {
                        _projectAssignment = new ProjectAssignment();

                        _projectAssignment.ProjectId = project.ProjectId;
                        _projectAssignment.PracticeId = practice;

                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }
                }

                else if (this.AccessLevelId == (int)enAccessLevelId.MedicalGroup)
                {
                    foreach (int medecalgroups in MedicalGroupIds)
                    {
                        _projectAssignment = new ProjectAssignment();

                        _projectAssignment.ProjectId = project.ProjectId;
                        _projectAssignment.MedicalGroupId = medecalgroups;
                        BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
                        BMTDataContext.SubmitChanges();
                    }
                }

                List<ProjectSection> projSection = (from projSec in BMTDataContext.ProjectSections
                                                    where projSec.ProjectId == projectId
                                                    select projSec).ToList();
                if (projSection.Count != 0)
                {
                    foreach (ProjectSection pSec in projSection)
                    {
                        List<ProjectDocument> pDoc = (from pDocRec in BMTDataContext.ProjectDocuments
                                                      where pDocRec.ProjectSectionId == pSec.ProjectSectionId
                                                      select pDocRec).ToList();

                        BMTDataContext.ProjectDocuments.DeleteAllOnSubmit(pDoc);
                        BMTDataContext.SubmitChanges();
                    }

                    BMTDataContext.ProjectSections.DeleteAllOnSubmit(projSection);
                    BMTDataContext.SubmitChanges();
                }

                ProjectSection pSection = new ProjectSection();

                pSection.ParentProjectSectionId = null;
                pSection.Name = project.Name;
                pSection.SectionTypeId = (int)enSectionType.PlaceHolder;
                pSection.ProjectId = project.ProjectId;
                pSection.TemplateId = null;
                pSection.AccessLevelId = project.AccessLevelId;

                BMTDataContext.ProjectSections.InsertOnSubmit(pSection);
                BMTDataContext.SubmitChanges();

                if (AddProjectFolder)
                {
                    ProjectSection pSectionChild = new ProjectSection();

                    pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
                    pSectionChild.Name = "Utility Folder";
                    pSectionChild.ProjectId = project.ProjectId;
                    pSectionChild.SectionTypeId = (int)enSectionType.PlaceHolder;
                    pSectionChild.AccessLevelId = project.AccessLevelId;

                    BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                    BMTDataContext.SubmitChanges();

                    ProjectSection pSectionChildNode = new ProjectSection();

                    pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                    pSectionChildNode.Name = "Uploaded Documents";
                    pSectionChildNode.SectionTypeId = (int)enSectionType.ProjectFolder;
                    pSectionChildNode.AccessLevelId = project.AccessLevelId;
                    pSectionChildNode.ProjectId = project.ProjectId;
                    BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                    BMTDataContext.SubmitChanges();

                }

                if (FolderList.Count() != 0)
                {
                    ProjectSection newPSection = new ProjectSection();

                    foreach (string folder in FolderList)
                    {
                        string[] newFolder = folder.Split(DELIMITATORS, StringSplitOptions.RemoveEmptyEntries);
                        if (newFolder.Count() != 0)
                        {

                            if (newFolder[1].ToString() == Project_Folder)
                            {
                                ProjectSection Folder = new ProjectSection();

                                Folder.ParentProjectSectionId = pSection.ProjectSectionId;
                                Folder.Name = newFolder[0].ToString();
                                Folder.ProjectId = project.ProjectId;
                                Folder.SectionTypeId = (int)enSectionType.Folder;
                                Folder.AccessLevelId = project.AccessLevelId;

                                BMTDataContext.ProjectSections.InsertOnSubmit(Folder);
                                BMTDataContext.SubmitChanges();

                                newPSection = Folder;
                            }

                            else if (newFolder[1] == newPSection.Name)
                            {
                                ProjectSection Folder = new ProjectSection();

                                Folder.ParentProjectSectionId = newPSection.ProjectSectionId;
                                Folder.Name = newFolder[0].ToString();
                                Folder.ProjectId = project.ProjectId;
                                Folder.SectionTypeId = (int)enSectionType.Folder;
                                Folder.AccessLevelId = project.AccessLevelId;

                                BMTDataContext.ProjectSections.InsertOnSubmit(Folder);
                                BMTDataContext.SubmitChanges();
                            }
                        }
                    }
                }

                if (GetTemplates().Count != 0 || GetForms().Count != 0)
                {
                    ProjectSection pSectionChild = new ProjectSection();

                    pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
                    pSectionChild.Name = "Tools";
                    pSectionChild.ProjectId = project.ProjectId;
                    pSectionChild.SectionTypeId = (int)enSectionType.PlaceHolder;
                    pSectionChild.AccessLevelId = project.AccessLevelId;
                    pSectionChild.TemplateId = null;
                    List<Template> temps = GetTemplates();

                    if (temps.Count != 0)
                    {
                        pSectionChild.ToolLevelId = (from temp in BMTDataContext.Templates
                                                     where temp.Name == TempName[0].ToString()
                                                     select temp.ToolLevelId).FirstOrDefault();

                        BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                        BMTDataContext.SubmitChanges();

                        foreach (Template tmp in temps)
                        {
                            ProjectSection pSectionChildNode = new ProjectSection();

                            pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                            pSectionChildNode.Name = tmp.Name;
                            pSectionChildNode.TemplateId = tmp.TemplateId;
                            pSectionChildNode.SectionTypeId = (int)enSectionType.Template;
                            pSectionChildNode.AccessLevelId = project.AccessLevelId;
                            pSectionChildNode.ProjectId = project.ProjectId;
                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                            BMTDataContext.SubmitChanges();
                        }
                    }

                    if (GetForms().Count != 0)
                    {
                        if (GetTemplates().Count == 0)
                        {
                            pSectionChild.ToolLevelId = (from form in BMTDataContext.Forms
                                                         where form.Name == FormName[0].ToString()
                                                         select form.ToolLevelId).FirstOrDefault();

                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
                            BMTDataContext.SubmitChanges();

                        }
                        List<Form> forms = GetForms();
                        foreach (Form form in forms)
                        {
                            ProjectSection pSectionChildNode = new ProjectSection();

                            pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
                            pSectionChildNode.Name = form.Name;
                            pSectionChildNode.FormId = form.FormId;
                            pSectionChildNode.SectionTypeId = (int)enSectionType.Form;
                            pSectionChildNode.AccessLevelId = project.AccessLevelId;
                            pSectionChildNode.ProjectId = project.ProjectId;
                            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
                            BMTDataContext.SubmitChanges();
                        }
                    }

                }

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //public bool UpdateProject(int projectId)
        //{
        //    try
        //    {
        //        var project = (from proj in BMTDataContext.Projects
        //                       where proj.ProjectId == projectId
        //                       select proj).First();

        //        project.Name = this.Name;
        //        project.Description = this.Description;
        //        project.AccessLevelId = this.AccessLevelId;
        //        project.LastUpdatedBy = this.LastUpdatedBy;
        //        project.LastUpdatedOn = this.LastUpdatedDate;

        //        BMTDataContext.SubmitChanges();

        //        List<ProjectAssignment> projectAssignment = (from projAssignment in BMTDataContext.ProjectAssignments
        //                                                     where projAssignment.ProjectId == projectId
        //                                                     select projAssignment).ToList();

        //        BMTDataContext.ProjectAssignments.DeleteAllOnSubmit(projectAssignment);
        //        BMTDataContext.SubmitChanges();

        //        if (this.AccessLevelId == (int)enAccessLevelId.Enterprise)
        //        {
        //            foreach (int enterprise in EnterpriseIds)
        //            {
        //                _projectAssignment = new ProjectAssignment();

        //                _projectAssignment.ProjectId = project.ProjectId;
        //                _projectAssignment.EnterpriseId = EnterpriseId;
        //                BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
        //                BMTDataContext.SubmitChanges();
        //            }
        //        }

        //        else if (this.AccessLevelId == (int)enAccessLevelId.Practice)
        //        {
        //            foreach (int practice in PracticeIds)
        //            {
        //                _projectAssignment = new ProjectAssignment();

        //                _projectAssignment.ProjectId = project.ProjectId;
        //                _projectAssignment.PracticeId = practice;

        //                BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
        //                BMTDataContext.SubmitChanges();
        //            }
        //        }

        //        else if (this.AccessLevelId == (int)enAccessLevelId.MedicalGroup)
        //        {
        //            foreach (int medecalgroups in MedicalGroupIds)
        //            {
        //                _projectAssignment = new ProjectAssignment();

        //                _projectAssignment.ProjectId = project.ProjectId;
        //                _projectAssignment.MedicalGroupId = medecalgroups;
        //                BMTDataContext.ProjectAssignments.InsertOnSubmit(_projectAssignment);
        //                BMTDataContext.SubmitChanges();
        //            }
        //        }

        //        ProjectSection pSection = (from projSec in BMTDataContext.ProjectSections
        //                                   where projSec.ProjectId == projectId
        //                                   select projSec).FirstOrDefault();

        //        pSection.Name = Name;
        //        pSection.AccessLevelId = AccessLevelId;

        //        BMTDataContext.SubmitChanges();

        //        if (AddProjectFolder)
        //        {
        //            ProjectSection pSectionChild = new ProjectSection();

        //            pSectionChild.ParentProjectSectionId = pSection.ProjectSectionId;
        //            pSectionChild.Name = "Utility Folder";
        //            pSectionChild.ProjectId = project.ProjectId;
        //            pSectionChild.SectionTypeId = (int)enSectionType.PlaceHolder;
        //            pSectionChild.AccessLevelId = project.AccessLevelId;

        //            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChild);
        //            BMTDataContext.SubmitChanges();

        //            ProjectSection pSectionChildNode = new ProjectSection();

        //            pSectionChildNode.ParentProjectSectionId = pSectionChild.ProjectSectionId;
        //            pSectionChildNode.Name = "Uploaded Documents";
        //            pSectionChildNode.SectionTypeId = (int)enSectionType.ProjectFolder;
        //            pSectionChildNode.AccessLevelId = project.AccessLevelId;
        //            pSectionChildNode.ProjectId = project.ProjectId;
        //            BMTDataContext.ProjectSections.InsertOnSubmit(pSectionChildNode);
        //            BMTDataContext.SubmitChanges();

        //        }
        //        else
        //        {
        //            ProjectSection pSec = (from projSec in BMTDataContext.ProjectSections
        //                                   where projSec.SectionTypeId == (int)enSectionType.ProjectFolder
        //                                   && projSec.ProjectId == project.ProjectId
        //                                   select projSec).FirstOrDefault();
        //            if (pSec != null)
        //            {
        //                int parentProjFolderId = (int)pSec.ParentProjectSectionId;

        //                BMTDataContext.ProjectSections.DeleteOnSubmit(pSec);

        //                ProjectSection pSecParent = (from projParentSec in BMTDataContext.ProjectSections
        //                                             where projParentSec.ProjectSectionId == parentProjFolderId
        //                                             select projParentSec).FirstOrDefault();
        //                if (pSecParent != null)
        //                {
        //                    BMTDataContext.ProjectSections.DeleteOnSubmit(pSecParent);
        //                }
        //            }

        //        }
        //        if (GetTemplates() != null)
        //        {

        //            List<ProjectSection> pSectionChild = (from projsecRec in BMTDataContext.ProjectSections
        //                                                  where projsecRec.SectionTypeId == (int)enSectionType.Template &&
        //                                                  projsecRec.ProjectId == projectId
        //                                                  select projsecRec).ToList();

        //            //pSectionChild.Name = TempName[0];
        //            // pSectionChild.TemplateId = GetTemplateId();
        //            int toolId = 0;
        //            foreach (ProjectSection pSec in pSectionChild)
        //            {
        //                toolId = (int)pSec.ParentProjectSectionId;
        //                List<ProjectDocument> pDoc = (from pDocRec in BMTDataContext.ProjectDocuments
        //                                              where pDocRec.ProjectSectionId == pSec.ProjectSectionId
        //                                              select pDocRec).ToList();

        //                BMTDataContext.ProjectDocuments.DeleteAllOnSubmit(pDoc);
        //                BMTDataContext.SubmitChanges();
        //            }
        //            if (toolId != 0)
        //            {
        //                BMTDataContext.ProjectSections.DeleteAllOnSubmit(pSectionChild);
        //                List<Template> temps = GetTemplates();
        //                foreach (Template tmp in temps)
        //                {
        //                    ProjectSection pSecChild = new ProjectSection();
        //                    pSecChild.ParentProjectSectionId = toolId;
        //                    pSecChild.Name = tmp.Name;
        //                    pSecChild.TemplateId = tmp.TemplateId;
        //                    pSecChild.SectionTypeId = (int)enSectionType.Template;
        //                    pSecChild.AccessLevelId = project.AccessLevelId;
        //                    pSecChild.ProjectId = project.ProjectId;
        //                    BMTDataContext.ProjectSections.InsertOnSubmit(pSecChild);
        //                    BMTDataContext.SubmitChanges();
        //                }

        //            }
        //        }
        //        if (GetForms() != null)
        //        {

        //            List<ProjectSection> pSectionChild = (from projsecRec in BMTDataContext.ProjectSections
        //                                                  where projsecRec.SectionTypeId == (int)enSectionType.Form &&
        //                                                  projsecRec.ProjectId == projectId
        //                                                  select projsecRec).ToList();

        //            //pSectionChild.Name = TempName[0];
        //            // pSectionChild.TemplateId = GetTemplateId();
        //            int toolId = 0;
        //            foreach (ProjectSection pSec in pSectionChild)
        //            {
        //                toolId = (int)pSec.ParentProjectSectionId;
        //                List<ProjectDocument> pDoc = (from pDocRec in BMTDataContext.ProjectDocuments
        //                                              where pDocRec.ProjectSectionId == pSec.ProjectSectionId
        //                                              select pDocRec).ToList();

        //                BMTDataContext.ProjectDocuments.DeleteAllOnSubmit(pDoc);
        //                BMTDataContext.SubmitChanges();
        //            }
        //            if (toolId != 0)
        //            {
        //                BMTDataContext.ProjectSections.DeleteAllOnSubmit(pSectionChild);
        //                List<Form> forms = GetForms();
        //                foreach (Form form in forms)
        //                {
        //                    ProjectSection pSecChild = new ProjectSection();
        //                    pSecChild.ParentProjectSectionId = toolId;
        //                    pSecChild.Name = form.Name;
        //                    pSecChild.FormId = form.FormId;
        //                    pSecChild.SectionTypeId = (int)enSectionType.Form;
        //                    pSecChild.AccessLevelId = project.AccessLevelId;
        //                    pSecChild.ProjectId = project.ProjectId;
        //                    BMTDataContext.ProjectSections.InsertOnSubmit(pSecChild);
        //                    BMTDataContext.SubmitChanges();
        //                }

        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        public string GetStandardFolderName(int tempId)
        {
            try
            {
                string standardFolderName = (from temp in BMTDataContext.Templates
                                             join stdFolder in BMTDataContext.StandardFolders
                                             on temp.StandardFolderId equals stdFolder.StandardFolderId
                                             where temp.TemplateId == tempId
                                             select stdFolder.Name).FirstOrDefault();

                return standardFolderName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetStandardFolderLocation(int tempId)
        {
            try
            {
                string standardFolderName = (from temp in BMTDataContext.Templates
                                             join stdFolder in BMTDataContext.StandardFolders
                                             on temp.StandardFolderId equals stdFolder.StandardFolderId
                                             where temp.TemplateId == tempId
                                             select stdFolder.Name).FirstOrDefault();

                return standardFolderName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTemplateIdByParentSectionId(int sectionId)
        {
            try
            {
                int tempId = (int)(from projSec in BMTDataContext.ProjectSections
                                   where projSec.ParentProjectSectionId == sectionId &&
                                   projSec.TemplateId != null
                                   select projSec.TemplateId).FirstOrDefault();

                return tempId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSiteNameBySiteID(int siteId)
        {
            try
            {
                string siteName = (from pracSite in BMTDataContext.PracticeSites
                                   where pracSite.PracticeSiteId == siteId
                                   select pracSite.Name).FirstOrDefault();

                return siteName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetFormIdBySectionId(int sectionId)
        {
            try
            {
                int fkFormId = (int)(from projSec in BMTDataContext.ProjectSections
                                     where projSec.ProjectSectionId == sectionId
                                     select projSec.FormId).FirstOrDefault();
                return fkFormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTemplateIdBySectionId(int sectionId)
        {
            try
            {
                int fkTempId = (int)(from projSec in BMTDataContext.ProjectSections
                                     where projSec.ProjectSectionId == sectionId
                                     select projSec.TemplateId).FirstOrDefault();
                return fkTempId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTemplateIdByProjectUsageId(int projectUsageId)
        {
            try
            {
                int projectId = (from projUsage in BMTDataContext.ProjectUsages
                                 where projUsage.ProjectUsageId == projectUsageId
                                 select projUsage.ProjectId).FirstOrDefault();

                int fkTempId = (int)(from projSec in BMTDataContext.ProjectSections
                                     where projSec.ProjectId == projectId
                                     && projSec.TemplateId != null
                                     select projSec.TemplateId).FirstOrDefault();
                return fkTempId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsProjectNameAvailable()
        {
            try
            {
                var projRec = (from proj in BMTDataContext.Projects
                               where proj.Name == this.Name
                               select proj).ToList();
                if (projRec.Count > 0)
                    return false;

                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsProjectNameAvailableForEdit()
        {
            try
            {
                var projRec = (from proj in BMTDataContext.Projects
                               where proj.Name == this.Name &&
                               proj.ProjectId != this.ProjectId
                               select proj).ToList();
                if (projRec.Count > 0)
                    return false;

                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool IsUsedProject(int projectId)
        {
            try
            {
                var pracProj = (from pracTempRec in BMTDataContext.ProjectUsages
                                where pracTempRec.ProjectId == projectId
                                select pracTempRec).ToList();
                if (pracProj.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
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

        public List<string> GetTemplateName()
        {
            try
            {
                List<string> Cat = (from temp in BMTDataContext.Templates
                                    where (temp.AccessLevelId == (int)enAccessLevelId.Public ||
                                    (temp.AccessLevelId == (int)enAccessLevelId.Enterprise &&
                                    temp.EnterpriseId == this.EnterpriseId) ||
                                    (temp.AccessLevelId == (int)enAccessLevelId.MedicalGroup &&
                                    temp.MedicalGroupId == this.MedicalGroupId) ||
                                    (temp.AccessLevelId == (int)enAccessLevelId.Practice &&
                                    temp.PracticeId == this.PracticeId)) &&
                                    temp.IsActive == true
                                    select temp.Name).Distinct().ToList<string>();
                return Cat;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetTemplateProjectSectionByProjectId(int projectId)
        {
            try
            {
                string concatName = "";
                List<string> templateName = (from projSec in BMTDataContext.ProjectSections
                                             join templates in BMTDataContext.Templates
                                             on projSec.TemplateId equals templates.TemplateId
                                             where projSec.ProjectId == projectId
                                             select templates.Name).ToList();
                foreach (string str in templateName)
                {
                    if (concatName == "")
                        concatName = str;
                    else
                        concatName = concatName + "," + str;
                }
                return concatName;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool GetProjectFolder(int projectId)
        {
            try
            {
                ProjectSection projFolder = (from projSec in BMTDataContext.ProjectSections
                                             where projSec.ProjectId == projectId
                                             && projSec.SectionTypeId == (int)enSectionType.ProjectFolder
                                             select projSec).FirstOrDefault();

                if (projFolder != null)
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public List<string> GetFormList()
        {
            List<string> formList = null;
            try
            {
                formList = (from form in BMTDataContext.Forms
                            where form.Active == true
                            select form.Name).ToList();

                return formList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetFormProjectSectionByProjectId(int projectId)
        {
            try
            {
                string concatName = "";
                List<string> formName = (from projSec in BMTDataContext.ProjectSections
                                         join form in BMTDataContext.Forms
                                         on projSec.FormId equals form.FormId
                                         where projSec.ProjectId == projectId
                                         select form.Name).ToList();

                foreach (string str in formName)
                {
                    if (concatName == "")
                        concatName = str;
                    else
                        concatName = concatName + "," + str;
                }
                return concatName;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<MedicalGroup> GetAllMedicalGroups(int enterpriseId)
        {
            try
            {
                List<MedicalGroup> lstMGR = (from medRec in BMTDataContext.MedicalGroups
                                             where medRec.EnterpriseId == enterpriseId
                                             select medRec).ToList();

                return lstMGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicalGroup> GetMedicalGroupsOfProject(int projectId)
        {
            try
            {
                List<MedicalGroup> lstMGR = (from projAssign in BMTDataContext.ProjectAssignments
                                             join medRec in BMTDataContext.MedicalGroups
                                             on projAssign.MedicalGroupId equals medRec.MedicalGroupId
                                             where projAssign.ProjectId == projectId
                                             select medRec).ToList();

                return lstMGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Enterprise> GetEnterpriseOfUser(string userType, int enterpriseId)
        {
            try
            {
                List<Enterprise> lstMGR = new List<Enterprise>();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    lstMGR = (from enterprise in BMTDataContext.Enterprises
                              select enterprise).ToList();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    lstMGR = (from enterprise in BMTDataContext.Enterprises
                              where enterprise.EnterpriseId == enterpriseId
                              select enterprise).ToList();
                }

                return lstMGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Enterprise> GetEnterpriseOfProject(int projectId)
        {
            try
            {
                List<Enterprise> lstMGR = new List<Enterprise>();

                lstMGR = (from enterprise in BMTDataContext.Enterprises
                          join projAssign in BMTDataContext.ProjectAssignments
                          on enterprise.EnterpriseId equals projAssign.EnterpriseId
                          where projAssign.ProjectId == projectId
                          select enterprise).ToList();


                return lstMGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Practice> GetPracticeOfUser(string userType, int enterpriseId)
        {
            try
            {
                List<Practice> lstPrac = new List<Practice>();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    lstPrac = (from pracRec in BMTDataContext.Practices
                               select pracRec).ToList();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    lstPrac = (from pracRec in BMTDataContext.Practices
                               join med in BMTDataContext.MedicalGroups
                               on pracRec.MedicalGroupId equals med.MedicalGroupId
                               where med.EnterpriseId == enterpriseId
                               select pracRec).ToList();
                }

                return lstPrac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Practice> GetPracticeOfProject(int projectId)
        {
            try
            {
                List<Practice> lstPrac = new List<Practice>();

                lstPrac = (from practice in BMTDataContext.Practices
                           join projAssign in BMTDataContext.ProjectAssignments
                           on practice.PracticeId equals projAssign.PracticeId
                           where projAssign.ProjectId == projectId
                           select practice).ToList();


                return lstPrac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicalGroup> GetAllMedicalGroupsOfProject(int enterpriseId, int projectId)
        {
            try
            {
                List<MedicalGroup> lstMGR2 = new List<MedicalGroup>();
                List<MedicalGroup> lstMGR = (from medRec in BMTDataContext.MedicalGroups
                                             where medRec.EnterpriseId == enterpriseId
                                             select medRec).ToList();

                List<int?> medNullableListIds = (from pAssign in BMTDataContext.ProjectAssignments
                                                 where pAssign.ProjectId == projectId
                                                 select pAssign.MedicalGroupId).ToList();

                if (medNullableListIds[0] != null)
                {
                    List<int> medIds = medNullableListIds.ConvertAll(i => (int)i);

                    lstMGR2 = (from lst2MGR in lstMGR
                               where !medIds.Contains(lst2MGR.MedicalGroupId)
                               select lst2MGR).ToList();
                }
                else
                {

                    lstMGR2 = (from lst2MGR in lstMGR
                               select lst2MGR).ToList();
                }
                return lstMGR2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Enterprise> GetEnterpriseListOfProject(string userType, int enterpriseId, int projectId)
        {
            try
            {
                List<Enterprise> lstMGR = new List<Enterprise>();

                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    List<Enterprise> lstENT2 = (from enterprise in BMTDataContext.Enterprises
                                                select enterprise).ToList();

                    List<int?> nullableEntList = (from projAssign in BMTDataContext.ProjectAssignments
                                                  where projAssign.ProjectId == projectId
                                                  select projAssign.EnterpriseId).ToList();

                    List<int> EntIds = nullableEntList.ConvertAll(i => (int)i);

                    lstMGR = (from lst in lstENT2
                              where !EntIds.Contains(lst.EnterpriseId)
                              select lst).ToList();

                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    List<Enterprise> lstENT2 = (from enterprise in BMTDataContext.Enterprises
                                                where enterprise.EnterpriseId == enterpriseId
                                                select enterprise).ToList();

                    List<int?> nullableEntList = (from projAssign in BMTDataContext.ProjectAssignments
                                                  where projAssign.ProjectId == projectId
                                                  select projAssign.EnterpriseId).ToList();
                    if (nullableEntList[0] != null)
                    {
                        List<int> EntIds = nullableEntList.ConvertAll(i => (int)i);

                        lstMGR = (from lst in lstENT2
                                  where !EntIds.Contains(lst.EnterpriseId)
                                  select lst).ToList();
                    }
                    else
                    {
                        lstMGR = (from lst in lstENT2
                                  select lst).ToList();
                    }
                }

                return lstMGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Practice> GetPracticeUserListOfProject(string userType, int enterpriseId, int projectId)
        {
            try
            {
                List<Practice> lstPrac = new List<Practice>();
                if (userType == enUserRole.SuperAdmin.ToString())
                {
                    List<Practice> lstPrac2 = (from pracRec in BMTDataContext.Practices
                                               select pracRec).ToList();

                    List<int?> nullablePracList = (from projAssign in BMTDataContext.ProjectAssignments
                                                   where projAssign.ProjectId == projectId
                                                   select projAssign.PracticeId).ToList();

                    List<int> PracIds = nullablePracList.ConvertAll(i => (int)i);

                    lstPrac = (from lst in lstPrac2
                               where !PracIds.Contains(lst.PracticeId)
                               select lst).ToList();
                }
                else if (userType == enUserRole.SuperUser.ToString())
                {
                    List<Practice> lstPrac2 = (from pracRec in BMTDataContext.Practices
                                               join med in BMTDataContext.MedicalGroups
                                               on pracRec.MedicalGroupId equals med.MedicalGroupId
                                               where med.EnterpriseId == enterpriseId
                                               select pracRec).ToList();

                    List<int?> nullablePracList = (from projAssign in BMTDataContext.ProjectAssignments
                                                   where projAssign.ProjectId == projectId
                                                   select projAssign.PracticeId).ToList();
                    if (nullablePracList[0] != null)
                    {
                        List<int> PracIds = nullablePracList.ConvertAll(i => (int)i);

                        lstPrac = (from lst in lstPrac2
                                   where !PracIds.Contains(lst.PracticeId)
                                   select lst).ToList();
                    }
                    else
                    {
                        lstPrac = (from lst in lstPrac2
                                   select lst).ToList();
                    }
                }
                return lstPrac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetProjectTemplates(string projectId, string fileName)
        {
            try
            {
                if (projectId != "")
                {
                    int Id = (from temp in BMTDataContext.Templates
                              where temp.Name == fileName
                              select temp.TemplateId).FirstOrDefault();

                    ProjectSection pSec = (from pSecRec in BMTDataContext.ProjectSections
                                           where pSecRec.ProjectId == Convert.ToInt32(projectId) &&
                                           pSecRec.TemplateId == Id
                                           select pSecRec).FirstOrDefault();

                    if (pSec != null)
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

        public bool GetProjectForms(string projectId, string fileName)
        {
            try
            {
                if (projectId != "")
                {
                    int Id = (from form in BMTDataContext.Forms
                              where form.Name == fileName
                              select form.FormId).FirstOrDefault();

                    ProjectSection pSec = (from pSecRec in BMTDataContext.ProjectSections
                                           where pSecRec.ProjectId == Convert.ToInt32(projectId) &&
                                           pSecRec.FormId == Id
                                           select pSecRec).FirstOrDefault();

                    if (pSec != null)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public string GetOtherFolders(int projectId)
        {
            try
            {
                var folders = (from pSection in BMTDataContext.ProjectSections
                               where pSection.ProjectId == projectId
                               && pSection.SectionTypeId == (int)enSectionType.Folder
                               select pSection).ToList();


                string folderList = string.Empty;

                if (folders.Count > 0)
                {
                    int ParentSectionId = (int)folders.First().ParentProjectSectionId;

                    var mainFolders = (from pSection in BMTDataContext.ProjectSections
                                       where pSection.ParentProjectSectionId == ParentSectionId
                                       && pSection.SectionTypeId == (int)enSectionType.Folder
                                       select pSection).ToList();

                    foreach (var mainFolder in mainFolders)
                    {
                        int MainFolderId = mainFolder.ProjectSectionId;
                        string MainFolderName = mainFolder.Name;

                        string SubFolders = string.Empty;

                        foreach (var subFolder in folders)
                        {
                            string SubFolderName = subFolder.Name;
                            if (subFolder.ParentProjectSectionId == MainFolderId)
                            {
                                SubFolders = SubFolders + SubFolderName + "," + MainFolderName + "/";
                            }
                        }

                        folderList = folderList + MainFolderName + "," + "ProjectFolder/" + SubFolders;
                    }
                }

                return folderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
