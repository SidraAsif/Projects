#region Modification History

//  ******************************************************************************
//  Module        : PracticeDetails
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 02/22/2012
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class PracticeDetails
    {
        #region PROPERTIES
        public string SecurePracticeId { get; set; }

        public string PracticeName { get; set; }

        public string SiteName { get; set; }

        public string Points { get; set; }

        public string Documents { get; set; }

        public DateTime? LastActivity { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string EmailText { get; set; }

        public int ProjectUsageId { get; set; }

        public int PracticeSiteId { get; set; }

        public string DateForeColor { get; set; }

        public string SecureEnterpriseId { get; set; }

        #endregion

        #region CONSTRUCTOR
        public PracticeDetails()
        { }

        #endregion
    }
}
