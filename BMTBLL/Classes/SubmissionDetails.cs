using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SubmissionDetails
    {
        #region properties

        public string SiteName { get; set; }
        public int ProjectId { get; set; }
        public int ProjectUsageId { get; set; }
        public int PracticeSiteId { get; set; }
        public string ISSLicenseNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? Completedon { get; set; }
        public int SubmissionType { get; set; }
        public string updatedby { get; set; }
        public int TemplateId { get; set; }
        #endregion
    }
}
