using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class KBCorporateElement
    {
        #region PROPERTIES

        public string Name { get; set; }
        public int Id { get; set; }
        #endregion

        #region CONSTRUCTOR
        public KBCorporateElement()
        { }

        public KBCorporateElement( string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }
        #endregion
    }
}
