#region Modification History

//  ******************************************************************************
//  Module        : UserAccount
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 01/20/2012
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
    public class NCQASummaryDetail
    {

        #region Properties
        public string StandardSequence { get; set; }
        public string ElementSequence { get; set; }
        public string Title { get; set; }
        public decimal MaxPoints { get; set; }
        public decimal EarnedPoints { get; set; }
        public string MustPass { get; set; }
        public int DocsRequired { get; set; }
        public int DocsUploaded { get; set; }
        public string Notes { get; set; }
        public string Complete { get; set; }

        #endregion

        #region Constructor
        public NCQASummaryDetail()
        { }

        public NCQASummaryDetail(string standardSequence, string elementSequence, string title,
            decimal maxPoints, decimal earnedPoints, string mustPass, int docsRequired, int docsUploaded,
            string notes, string complete)
        {
            this.StandardSequence = standardSequence;
            this.ElementSequence = elementSequence;
            this.Title = title;
            this.MaxPoints = maxPoints;
            this.EarnedPoints = earnedPoints;
            this.MustPass = mustPass;
            this.DocsRequired = docsRequired;
            this.DocsUploaded = docsUploaded;
            this.Notes = notes;
            this.Complete = complete;
        }

        #endregion
    }
}
