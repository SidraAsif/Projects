#region Modification History

//  ******************************************************************************
//  Module        : Practice Size
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/03/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who             Date            Description
//  *******************************************************************************
//  
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class PracticeSizeBO : BMTConnection
    {
        #region PROPERTIES
        public int ID { get; set; }
        public string Name { get; set; }

        private PracticeSize PSize { get; set; }

        private IQueryable _practiceSizeList;
        public IQueryable PracticeSizeList
        {
            get { return _practiceSizeList; }
            set { _practiceSizeList = value; }
        }

        private int _pracriceSizeId { get; set; }
        public int PracriceSizeId
        {
            get { return _pracriceSizeId; }
            set { _pracriceSizeId = value; }
        }

        private int _practiceSize { get; set; }
        public int PracticeSize
        {
            get { return _practiceSize; }
            set { _practiceSize = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public PracticeSizeBO()
        {

            PSize = new PracticeSize();
        }

        public PracticeSizeBO(int id, string name)
        {

            this.ID = id;
            this.Name = name;
        }

        public PracticeSizeBO(int PracticeSizeId)
        {

            _pracriceSizeId = PracticeSizeId;
        }

        #endregion

        #region FUNCTIONS
        public void GetPracticeSizeById()
        {

            try
            {
                _practiceSizeList = (from PracticeSizeRecord in BMTDataContext.PracticeSizes
                                     select new
                                     {
                                         ID = PracticeSizeRecord.PracticeSizeId,
                                         Name = PracticeSizeRecord.PracticeSize1
                                     }).AsQueryable();


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }
        

        public List<PracticeSizeBO> GetPracticeSizeByGroupId()
        {

            try
            {
                List<PracticeSizeBO> cUserDetail = null;
                cUserDetail = (from PracticeSizeRecord in BMTDataContext.PracticeSizes
                               select new PracticeSizeBO
                               {
                                   ID = PracticeSizeRecord.PracticeSizeId,
                                   Name = PracticeSizeRecord.PracticeSize1
                               }
                    ).ToList();

                return cUserDetail;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}
