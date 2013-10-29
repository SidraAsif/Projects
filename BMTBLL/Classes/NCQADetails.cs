#region Modification History

//  ******************************************************************************
//  Module        : NCQA Details
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 02-20-2012 
//  Description   : To generate the list
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class NCQADetails
    {
        #region PROPERTIES
        public string Sequence { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public string PCMHSequence { get; set; }
        public string ElementSequence { get; set; }
        public string FactorSequence { get; set; }

        public string File { get; set; }

        public int PracticeId { get; set; }
        public int SiteId { get; set; }
        public int ProjectId { get; set; }
        public int ProjectUsageId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string DocLinkedTo { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public string FactorTitle { get; set; }
        public string DocName { get; set; }
        public string ReferencePage { get; set; }
        public string RelevancyLevel { get; set; }
        public string DocType { get; set; }

        #endregion

        #region CONSTRUCTOR
        public NCQADetails()
        { }

        public NCQADetails(string sequence, string name)
        {
            this.Sequence = sequence;
            this.Name = name;
        }

        public NCQADetails(string sequence, string name, string location)
        {
            this.Sequence = sequence;
            this.Name = name;
            this.Location = location;
        }

        public NCQADetails(string pcmhSequence, string elementSequence, string factorSequence, string file, string type, string title, string docLinkedTo,
            int practiceId, int siteId, int projectUsageId, string docName, string referencePage, string relevancyLevel, string docType, string factorTitle)
        {
            this.PCMHSequence = pcmhSequence;
            this.ElementSequence = elementSequence;
            this.FactorSequence = factorSequence;

            this.File = file;
            this.Type = type;
            this.Title = title;
            this.DocLinkedTo = docLinkedTo;

            this.PracticeId = practiceId;
            this.SiteId = siteId;
            this.ProjectUsageId = projectUsageId;

            this.DocName = docName;
            this.ReferencePage = referencePage;
            this.RelevancyLevel = relevancyLevel;
            this.DocType = docType;

            this.FactorTitle = factorTitle;
        }

        public NCQADetails(string pcmhSequence, string elementSequence, string factorSequence, string file, string type, string title,DateTime? lastUploadedDate, string docLinkedTo,
    int practiceId, int siteId, int projectUsageId, string docName, string referencePage, string relevancyLevel, string docType, string factorTitle)
        {
            this.PCMHSequence = pcmhSequence;
            this.ElementSequence = elementSequence;
            this.FactorSequence = factorSequence;

            this.File = file;
            this.Type = type;
            this.Title = title;
            this.LastUpdatedDate = lastUploadedDate;
            this.DocLinkedTo = docLinkedTo;

            this.PracticeId = practiceId;
            this.SiteId = siteId;
            this.ProjectUsageId = projectUsageId;

            this.DocName = docName;
            this.ReferencePage = referencePage;
            this.RelevancyLevel = relevancyLevel;
            this.DocType = docType;

            this.FactorTitle = factorTitle;
        }

        #endregion
    }
}
