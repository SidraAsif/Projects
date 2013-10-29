#region Modification History

//  ******************************************************************************
//  Module        : Credential Details
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
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
    public class CredentialBO : BMTConnection
    {
        #region PROPERTIES
        private Credential Credntial { get; set; }

        private IQueryable _credentialList;
        public IQueryable CredentialList
        {
            get { return _credentialList; }
            set { _credentialList = value; }
        }

        private int _credntialId { get; set; }
        public int CredntialId
        {
            get { return _credntialId; }
            set { _credntialId = value; }
        }

        private int _name { get; set; }
        public int Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region Constructor

        public CredentialBO()
        {

            Credntial = new Credential();
        }

        #endregion

        #region Functions

        public void GetCredentialById()
        {

            try
            {
                _credentialList = (from CredentialRecord in BMTDataContext.Credentials
                                   orderby CredentialRecord.Name
                                   select new { ID = CredentialRecord.CredentialId, Name = CredentialRecord.Name }).AsQueryable();


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        #endregion
    }
}
