#region Modification History

//  ******************************************************************************
//  Module        : Library Business Object
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
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class LibraryBO : BMTConnection
    {
        #region VARIABLES
        private IQueryable _libraryDocList;
        public IQueryable LibraryDocList
        {
            get { return _libraryDocList; }
            set { _libraryDocList = value; }
        }

        private int _libraryDocumentId;
        public int LibraryDocumentId
        {
            get { return _libraryDocumentId; }
            set { _libraryDocumentId = value; }
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public LibraryBO()
        {

        }

        # endregion

        #region FUNCTIONS
        public List<DocDetails> GetDocuments(int libraryId, int practiceId)
        {

            List<DocDetails> libaray;

            try
            {

                if (practiceId != 0)
                {

                    libaray = (from docRecord in BMTDataContext.LibraryDocuments
                               where docRecord.LibrarySectionId == libraryId && docRecord.PracticeId == practiceId
                               select new DocDetails
                               {
                                   Name = docRecord.Name,
                                   DocumentId = docRecord.LibraryDocumentId,
                                   Description = docRecord.description,
                                   Image = docRecord.Image,
                                   Link = docRecord.Link
                               }).ToList();

                    return libaray;
                }

                else
                {
                    if (practiceId == 0)
                    {
                        libaray = (from docRecord in BMTDataContext.LibraryDocuments
                                   where docRecord.LibrarySectionId == libraryId
                                   select new DocDetails
                                   {
                                       Name = docRecord.Name,
                                       DocumentId = docRecord.LibraryDocumentId,
                                       Description = docRecord.description,
                                       Image = docRecord.Image,
                                       Link = docRecord.Link
                                   }).ToList();

                        return libaray;
                    }
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
                var docRow = (from docRecord in BMTDataContext.LibraryDocuments
                              where docRecord.LibraryDocumentId == LibraryDocumentId
                              select docRecord).SingleOrDefault();

                BMTDataContext.LibraryDocuments.DeleteOnSubmit(docRow);
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
