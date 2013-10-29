using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class ProviderTypeBO : BMTConnection
    {
        #region variable

        private ProviderType ProvderType { get; set; }

        private IQueryable _ProviderTypeList;
        public IQueryable ProviderTypeList
        {
            get { return _ProviderTypeList; }
            set { _ProviderTypeList = value; }
        }

        private int _ProviderTypeId { get; set; }
        public int ProviderTypeId
        {
            get { return _ProviderTypeId; }
            set { _ProviderTypeId = value; }
        }

        private int _Name { get; set; }
        public int Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        #endregion

        #region Constructor

        public ProviderTypeBO()
        {

            ProvderType = new ProviderType();
        }

        #endregion

        #region Functions

        public void GetProviderTypeById()
        {

            try
            {
                _ProviderTypeList = (from ProviderTypeRecord in BMTDataContext.ProviderTypes
                                     orderby ProviderTypeRecord.ProviderTypeId
                                     select new { ID = ProviderTypeRecord.ProviderTypeId, Name = ProviderTypeRecord.Name }).AsQueryable();


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        #endregion
    }
}
