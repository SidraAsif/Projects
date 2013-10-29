#region Modification History

//  ******************************************************************************
//  Module        : Medical Practice Speciality
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
    public class SpecialityBO : BMTConnection
    {

        #region VARIABLE
        private Speciality Specialty { get; set; }

        private IQueryable _specialityList;
        public IQueryable SpecialityList
        {
            get { return _specialityList; }
            set { _specialityList = value; }
        }

        private int _specialityId { get; set; }
        public int SpecialityId
        {
            get { return _specialityId; }
            set { _specialityId = value; }
        }

        private int _name { get; set; }
        public int Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public SpecialityBO()
        {

            Specialty = new Speciality();
        }

        public SpecialityBO(int SpecialityId)
        {

            _specialityId = SpecialityId;
        }

        #endregion

        #region FUNCTIONS
        public void GetSpecialityById()
        {

            try
            {
                _specialityList = (from SpecialityRecord in BMTDataContext.Specialities
                                   select new
                                   {
                                       ID = SpecialityRecord.SpecialityId,
                                       Name = SpecialityRecord.Name
                                   }).AsQueryable();


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        #endregion

    }
}
