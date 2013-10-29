
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRAdashboard
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

        public int ProjectId { get; set; }

        public int ProjectUsageId { get; set; }

        public int PracticeSiteId{ get; set; }

        public string DateForeColor { get; set; }

        public string SecureEnterpriseId { get; set; }

        public string FindingsFinalized { get; set; }

        public string FollowupFinalized { get; set; }

        #endregion

        #region CONSTRUCTOR
        public SRAdashboard()
        { }

        #endregion
    }
}
