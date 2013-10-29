using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRARiskDetails
    {
        #region properties

        public int ElementId { get; set; }
        public string ElementName { get; set; }        
        public string ThreatVulnerability { get; set; }
        public string RecommendedControlMeasure { get; set; }
        public string ExistingControl { get; set; }
        public string ExistingControlEffectiveness { get; set; }
        public string ExposurePotential { get; set; }
        public string Likelihood { get; set; }
        public string Impact { get; set; }
        public string RiskRating { get; set; }
        

        #endregion
        

        #region Constructor

        public SRARiskDetails()
        { }


        public SRARiskDetails(int _elementId, string _elementName, string _threatVulnerability, string _recommendedControlMeasure, string _existingControl,
            string _existingControlEffectiveness, string _exposurePotential, string _likelihood, string _impact, string _riskRating)
        {
            this.ElementId = _elementId;
            this.ElementName = _elementName;
            this.ThreatVulnerability = _threatVulnerability;
            this.RecommendedControlMeasure = _recommendedControlMeasure;
            this.ExistingControl = _existingControl;
            this.ExistingControlEffectiveness = _existingControlEffectiveness;
            this.ExposurePotential = _exposurePotential;
            this.Likelihood = _likelihood;
            this.Impact = _impact;
            this.RiskRating = _riskRating;           
        }

        #endregion

    }
}
