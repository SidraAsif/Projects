using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class PracticeRoleBO : BMTConnection
    {
        #region PROPERTIES
        private PracticeRole practiceRole { get; set; }

        private IQueryable _practiceRoleList;
        public IQueryable PracticeRoleList
        {
            get { return _practiceRoleList; }
            set { _practiceRoleList = value; }
        }

        private int _practiceRoleId { get; set; }
        public int PracticeRoleId
        {
            get { return _practiceRoleId; }
            set { _practiceRoleId = value; }
        }

        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region CONSTRUCTOR

        public PracticeRoleBO()
        {

            practiceRole = new PracticeRole();
        }

        #endregion

        #region FUNCTIONS
        public void GetPracticeRoleById()
        {

            try
            {
                _practiceRoleList = (from RoleRecord in BMTDataContext.PracticeRoles
                                     orderby RoleRecord.PracticeRoleId
                                     select new { ID = RoleRecord.PracticeRoleId, Name = RoleRecord.Name }).AsQueryable();


            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        #endregion

    }
}
