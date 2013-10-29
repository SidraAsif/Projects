using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class NCQANotesDetails
    {
        #region PROPERTIES
        public string PCMHId { get; set; }
        public string ElementId { get; set; }
        public string PCMHTitle { get; set; }
        public string ElementTitle { get; set; }

        public int PracticeId { get; set; }
        public int SiteId { get; set; }
        public int ProjectId { get; set; }

        public string Type { get; set; }
        public string Notes { get; set; }
        #endregion

        #region CONSTRUCTOR
        public NCQANotesDetails()
        { }

        public NCQANotesDetails(string pcmhId, string elementId, string type, string notes,string pcmhTitle, string elementTitle)
        {
            this.PCMHId = pcmhId;
            this.ElementId = elementId;
            this.Type = type;
            this.Notes = notes;
            this.PCMHTitle = pcmhTitle;
            this.ElementTitle = elementTitle;

        }
        #endregion
    }
}
