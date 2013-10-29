using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMTBLL;

namespace BMTBLL
{
    public class PCMHStatusReport
    {
        #region PROPERTIES
        public int ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string PracticeName { get; set; }
        public string SiteName { get; set; }
        public int ProvidersAtSite { get; set; }
        public int ProjectId { get; set; }
        public int TotalComplete { get; set; }
        public int TotalElements { get; set; }
        public decimal PercentScoreFinal { get; set; }
        public string Reviewed { get; set; }
        public string Submitted { get; set; }
        public string Recognized { get; set; }
        public string LastActivity { get; set; }
        public int MustPass { get; set; }
        public decimal PointsEarned { get; set; }
        public int DocumentsUploaded { get; set; }
        public decimal PercentInComplete { get; set; }
        public int PracticeSizeId { get; set; }

        #endregion

        #region CONSTRUCTORS
        public PCMHStatusReport()
        { }

        #endregion
    }
}
