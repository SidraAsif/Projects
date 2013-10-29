using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class ConsultantAdministrationDetail
    {
         #region PROPERTIES

        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public int ConsultantTypeId { get; set; }
        public string ServiceArea { get; set; }
        public bool Featured { get; set; }
        public string Website { get; set; }
        public string logoPath { get; set; }
        public string Organization { get; set; }
        public string PrimaryAddress { get; set; }
        public string SecondaryAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        #endregion

         #region CONSTRUCTOR
        public ConsultantAdministrationDetail()
        { }

        #endregion
    }
}
