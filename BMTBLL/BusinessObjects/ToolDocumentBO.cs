#region Modification History

//  ******************************************************************************
//  Module        : Tool Document Business Object
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
    public class ToolDocumentBO : BMTConnection
    {
        #region VARIABLE
        private double _width;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private IQueryable _toolDocList;
        public IQueryable ToolDocList
        {
            get { return _toolDocList; }
            set { _toolDocList = value; }
        }

        private int _toolDocumentId;
        public int ToolDocumentId
        {
            get { return _toolDocumentId; }
            set { _toolDocumentId = value; }
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
        public ToolDocumentBO()
        {


        }

        #endregion

        #region FUNCTIONS
        public List<DocDetails> GetDocuments(int toolId, int practiceId)
        {   

            List<DocDetails> documents;

            try
            {  
                if (practiceId != 0)
                {

                    documents = (from docRecord in BMTDataContext.ToolDocuments
                                 where docRecord.ToolSectionId == toolId && docRecord.PraticeId == practiceId
                                 orderby docRecord.ToolDocumentId
                                 select new DocDetails
                                 {
                                     Name = docRecord.Name,
                                     DocumentId = docRecord.ToolDocumentId,
                                     Description = docRecord.Description,
                                     Image = docRecord.Image,
                                     Link = docRecord.Link
                                 }).ToList();

                    return documents;
                }
                else
                {
                    if (practiceId == 0)
                    {
                        documents = (from docRecord in BMTDataContext.ToolDocuments
                                     where docRecord.ToolSectionId == toolId
                                     orderby docRecord.ToolDocumentId
                                     select new DocDetails
                                     {
                                         Name = docRecord.Name,
                                         DocumentId = docRecord.ToolDocumentId,
                                         Description = docRecord.Description,
                                         Image = docRecord.Image,
                                         Link = docRecord.Link
                                     }).ToList();

                        return documents;
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
                var docRow = (from docRecord in BMTDataContext.ToolDocuments
                              where docRecord.ToolDocumentId == ToolDocumentId
                              select docRecord).SingleOrDefault();

                BMTDataContext.ToolDocuments.DeleteOnSubmit(docRow);
                BMTDataContext.SubmitChanges();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetToolSectionAttrById(int toolId)
        {
            try
            {
                var toolSectionAttributes = (from ToolSectionRecord in BMTDataContext.ToolSections
                                             where ToolSectionRecord.ToolSectionId == toolId
                                             select new
                                             {
                                                 ToolSectionRecord.ContentType,
                                                 ToolSectionRecord.Name
                                             }).FirstOrDefault();

                List<string> toolSectionAttr = new List<string>();
                toolSectionAttr.Add(toolSectionAttributes.ContentType.ToString());
                toolSectionAttr.Add(toolSectionAttributes.Name.ToString());

                return toolSectionAttr;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

    }
}
