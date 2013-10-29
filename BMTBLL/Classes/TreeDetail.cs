using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL.Classes
{
    public class TreeDetail
    {

        public int SectionId { get; set; }
        public Nullable<int> ParentSectionId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int PracticeId { get; set; }
        public bool IsTopNode { get; set; }
       
    }
}
