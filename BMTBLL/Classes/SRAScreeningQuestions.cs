using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRAScreeningQuestions
    {
        #region properties
        
        public string Topic { get; set;}
        public string Questions { get; set; }
        public string Response { get; set; }
        public string ThreatVulnerability { get; set; }
        public string PeopleOrProcess { get; set; }
        public string Technology { get; set; }
        public int ElementId { get; set; }
        public string ElementName { get; set; }

        #endregion
        

        #region Constructor

        public SRAScreeningQuestions()
        { }


        public SRAScreeningQuestions(string _topic, string _questions, string _response, string _threatVulnerability, string _peopleOrProcess, string _technology,
            int _elementId, string _elementName)
        {
            this.Topic = _topic;
            this.Questions = _questions;
            this.Response = _response;
            this.ThreatVulnerability = _threatVulnerability;
            this.PeopleOrProcess = _peopleOrProcess;
            this.Technology = _technology;
            this.ElementId = _elementId;
            this.ElementName = _elementName;
        }

        #endregion
    }
}
