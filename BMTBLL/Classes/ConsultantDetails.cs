using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class ConsultantDetails
    {
        #region PROPERTIES


        public int UserID { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ServiceArea { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        //public int Relationship { get; set; }
        //public int PracticeID { get; set; }
        #endregion

         #region CONSTRUCTOR
        public ConsultantDetails()
        { }

        #endregion

    }
}
