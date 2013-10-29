#region Modification History

//  ******************************************************************************
//  Module        : AddressDetails
//  Created By    : Undefined
//  When Created  : Undefined
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date                     Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    01-25-2012                  GetPageList + 
//                                                      Replace getPageId with GetPageId
//                                                      Set name of variables and properties
//                                                      remove extra variables etc...
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Web;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;

using BMTBLL.Enumeration;
namespace BMTBLL
{
    public class PageBo : BMTConnection
    {

        #region Variable
        private int pageId;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }

        }

        #endregion

        #region contructor
        public PageBo()
        {

        }

        #endregion

        #region Function
        public int GetPageId(string pageName, enUserRole userRoleType,int enterpriseId)
        {
            try
            {
                var pageRow = (from pageRecord in BMTDataContext.Pages
                               where pageRecord.Name == pageName
                               && pageRecord.RoleId == (int)userRoleType
                               && pageRecord.EnterpriseId == enterpriseId
                               select new { pageRecord }).SingleOrDefault();

                if (pageRow != null)
                    pageId = pageRow.pageRecord.PageId;
                else
                    pageId = 0;

                return pageId;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<Page> GetListOfPages(enUserRole userRoleType, int enterpriseId)
        {
            try
            {
                List<Page> _pageList;
                _pageList = (from pageRecords in BMTDataContext.Pages
                             where pageRecords.RoleId == (int)userRoleType
                             && pageRecords.EnterpriseId == enterpriseId
                             select pageRecords).ToList();

                return _pageList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}




