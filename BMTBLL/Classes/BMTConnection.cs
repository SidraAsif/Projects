using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BMTBLL
{
    public class BMTConnection
    {
        #region PROPERTIES
        protected BMTDataContext BMTDataContext { get; set; }
        protected string _connectionString { get; set; }

        #endregion

        #region CONSTRUCTOR
        protected BMTConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BMTConnectionString"].ConnectionString;
            BMTDataContext = new BMTDataContext(_connectionString);
            BMTDataContext.CommandTimeout = 600;  // set timeout to 10 minutes
        }

        #endregion

    }
}
