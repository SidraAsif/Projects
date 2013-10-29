#region Modification History

//  ******************************************************************************
//  Module        : Project Document Business Object
//  Created By    : NA
//  When Created  : NA
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date                                Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01/03/2012 (MM/dd/YYYY)          Remove extra spaces
//  Mirza Fahad Ali Baig    01/03/2012 (MM/dd/YYYY)          DeleteDocById
//  Mirza Fahad Ali Baig    01/03/2012 (MM/dd/YYYY)          apply pascal casing tech. on variables
//  Mirza Fahad Ali Baig    01/03/2012 (MM/dd/YYYY)          remove static keywords from properties
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class ProjectDocumentBO : BMTConnection
    {
        #region VARIABLE
        public IQueryable _projectDocList { get; set; }

        private int _projectDocumentId { get; set; }
        public int ProjectDocumentId
        {

            get { return _projectDocumentId; }
            set { _projectDocumentId = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        #endregion

        #region CONSTANT
        string[] SEQ_ARRAY = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50" };
        private int NUMBER_INDEX = 1;
        #endregion

        #region CONSTRUCTOR
        public ProjectDocumentBO()
        {


        }
        #endregion

        #region FUNCTIONS
        public List<DocDetails> GetDocuments(int toolId, int projectUsageId)
        {

            List<DocDetails> document;

            try
            {
                if (projectUsageId != 0)
                {
                    document = (from docRecord in BMTDataContext.ProjectDocuments
                                where docRecord.ProjectSectionId == toolId
                                && docRecord.ProjectUsageId == projectUsageId
                                select new DocDetails
                                {
                                    Name = docRecord.Name,
                                    DocumentId = docRecord.ProjectDocumentId,
                                    Description = docRecord.Description,
                                    Image = docRecord.Image,
                                    Link = docRecord.Link
                                }).ToList();

                    return document;
                }

                else
                {
                    if (projectUsageId == 0)
                    {
                        document = (from docRecord in BMTDataContext.ProjectDocuments
                                    where docRecord.ProjectSectionId == toolId
                                    select new DocDetails
                                    {
                                        Name = docRecord.Name,
                                        DocumentId = docRecord.ProjectDocumentId,
                                        Description = docRecord.Description,
                                        Image = docRecord.Image,
                                        Link = docRecord.Link,
                                    }).ToList();

                        foreach (DocDetails Docs in document)
                        {
                            string[] SequenceArray = Docs.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (SequenceArray.Length>1)
                            {
                                string Sequence = SequenceArray[NUMBER_INDEX].ToString();
                                if (SEQ_ARRAY.Contains(Sequence))
                                {
                                    Docs.Sequence = Convert.ToInt32(Docs.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[NUMBER_INDEX].ToString());
                                }
                            }

                            document = document.OrderBy(x => x.Sequence).ToList();
                        }
                        return document;
                    }
                }

                return null;

            }


            catch (Exception ex)
            {
                throw ex;

            }

        }
        public List<DocDetails> GetStandardDocument(int template)
        {

            List<DocDetails> document;

            try
            {
                int? stdFolderId = (from temp in BMTDataContext.Templates
                                   where temp.TemplateId == template
                                   select temp.StandardFolderId).FirstOrDefault();
                if (stdFolderId != null)
                {
                    document = (from stdoc in BMTDataContext.StandardDocuments
                                where stdoc.StandardFolderId == (int)stdFolderId
                                select new DocDetails
                                {
                                    Name = stdoc.Name,
                                    DocumentId = stdoc.StandardDocumentId,
                                    Description = stdoc.Description,
                                    Image = stdoc.Image,
                                    Link = stdoc.Link
                                }).ToList();

                    return document;
                }
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public bool DeleteDocById()
        {
            try
            {
                var docRow = (from docRecord in BMTDataContext.ProjectDocuments
                              where docRecord.ProjectDocumentId == ProjectDocumentId
                              select docRecord).SingleOrDefault();

                BMTDataContext.ProjectDocuments.DeleteOnSubmit(docRow);
                BMTDataContext.SubmitChanges();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

    }
}
