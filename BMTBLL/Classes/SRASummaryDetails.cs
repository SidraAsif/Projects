using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRASummaryDetails
    {
        #region properties
        
        public string task { get; set;}
        public string items { get; set; }
        public string itemCompleted { get; set; }
        public string percentComplete { get; set; }
        public string reviewNotes { get; set; }
        public string status { get; set; }

        #endregion
        

        #region Constructor

        public SRASummaryDetails()
        { }


        public SRASummaryDetails(string _task, string _items, string _itemsCompleted, string _percentComplete, string _reviewNotes, string _status)
        {
            this.task = _task;
            this.items = _items;
            this.itemCompleted = _itemsCompleted;
            this.percentComplete = _percentComplete;
            this.reviewNotes = _reviewNotes;
            this.status = _status;        
        }

        #endregion
    }
}
