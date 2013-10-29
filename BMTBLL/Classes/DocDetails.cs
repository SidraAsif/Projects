using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class DocDetails
    {
        #region PROPERTIES
        public int DocumentId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Sequence { get; set; }

        #endregion
    }
}
