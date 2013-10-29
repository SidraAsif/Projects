using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class ConsultingUserDetail
    {
         #region PROPERTIES

        public int UserID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Organization { get; set; }
        public string ConsultantTypeName { get; set; }
        public string IsActive { get; set; }

        #endregion

         #region CONSTRUCTOR
        public ConsultingUserDetail()
        { }
        public ConsultingUserDetail(int userId, string userName)
        {
            this.UserID = userId;
            this.UserName = userName;
        }

        #endregion
    }
}
