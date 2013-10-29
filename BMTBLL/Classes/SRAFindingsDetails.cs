using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRAFindingsDetails
    {
        #region properties

        public string RiskType { get; set; }
        public string RiskTitle { get; set; }        
        public string ThreatVulnerability { get; set; }
        public string RiskRating { get; set; }
        public string RecommendedControlMeasure { get; set; }
        public string ExistingControlMeasuresApplied { get; set; }
        public string Owner { get; set; }
        public string RemediationSteps { get; set; }
        public string Date { get; set; }        

        #endregion
        

        #region Constructor

        public SRAFindingsDetails()
        { }


        public SRAFindingsDetails(string _riskType, string _riskTitle, string _threatVulnerability, string _riskRating, string _recommendedControlMeasure, string _existingControlMeasuresApplied,
            string _owner, string _remediationSteps, string _date)
        {
            this.RiskType = _riskType;
            this.RiskTitle = _riskTitle;
            this.ThreatVulnerability = _threatVulnerability;
            this.RiskRating = _riskRating;  
            this.RecommendedControlMeasure = _recommendedControlMeasure;
            this.ExistingControlMeasuresApplied = _existingControlMeasuresApplied;
            this.Owner = _owner;
            this.RemediationSteps = _remediationSteps;
            this.Date = _date;
                     
        }

        #endregion

    }
}
