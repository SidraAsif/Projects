using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
   public class NCQAFullDetail
    {
         #region PROPERTIES
        public string PCMHId { get; set; }
        public string ElementId { get; set; }
        public string PCMHTitle { get; set; }
        public string ElementTitle { get; set; }
        public int PracticeId { get; set; }
        public int SiteId { get; set; }
        public int ProjectId { get; set; }
        public string Notes { get; set; }
        public string FactorNumber { get; set; }
        public string Factor { get; set; }
        public string Answer { get; set; }
        public string Policies { get; set; }
        public string Report { get; set; }
        public string ScreenShot { get; set; }
        public string RRWB { get; set; }
        public string Others { get; set; }
        public string FactorNotes { get ; set ; }

        #endregion

        #region CONSTRUCTOR
        public NCQAFullDetail()
        { }

        public NCQAFullDetail(string pcmhId, string elementId, string notes,string pcmhTitle, string elementTitle,string factor,string policies,string answer,string reports,string screenShot,string rrwb,string others,string factorNotes,string factorNumber)
        {
            this.PCMHId = pcmhId;
            this.ElementId = elementId;
            this.Notes = notes;
            this.PCMHTitle = pcmhTitle;
            this.ElementTitle = elementTitle;
            this.Factor = factor;
            this.Policies = policies;
            this.Answer = answer;
            this.Report = reports;
            this.ScreenShot = screenShot;
            this.RRWB = rrwb;
            this.Others = others;
            this.FactorNotes = factorNotes;
            this.FactorNumber = factorNumber;

        }
        #endregion
    }
 }

