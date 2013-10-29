#region Modification History

//  ******************************************************************************
//  Module        : UserAccount
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 01/23/2012
//  Description   : Account screens
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
    public class ExpressAssessmentDetail
    {
        #region Properties
        public string Question { get; set; }
        public string Answer { get; set; }        
        public string Critical { get; set; }
        public string WarningMessage { get; set; }

        #endregion

        #region Constructor
        public ExpressAssessmentDetail()
        { }

        public ExpressAssessmentDetail(string question, string answer,  string critical)
        {
            this.Question = question;
            this.Answer = answer;            
            this.Critical = critical;
        }

        public ExpressAssessmentDetail(string warningMessage )
        {            
            this.WarningMessage = warningMessage;
        }
        #endregion
    }
}
