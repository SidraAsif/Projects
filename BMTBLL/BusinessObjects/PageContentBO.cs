#region Modification History

//  ******************************************************************************
//  Module        : PageContentBO
//  Created By    : Waqad Amin
//  When Created  : N/A
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                      Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    03-06-2012       GetActiveContentDetail to get active content detail for Editor page
//  Mirza Fahad Ali Baig    03-06-2012       Fix Get page content code
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

namespace BMTBLL
{
    public class PageContentBO : BMTConnection
    {
        #region PROPERTIES

        private int _PageId;
        public int PageId
        {
            get { return _PageId; }
            set { _PageId = value; }

        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }

        }

        private DateTime? _createdDate;
        public DateTime? createdDate
        {

            get { return _createdDate; }
            set { _createdDate = value; }
        }

        private DateTime? _postedDate;
        public DateTime? PostedDate
        {

            get { return _postedDate; }
            set { _postedDate = value; }
        }

        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }

        }

        private IQueryable _PagContenteList;
        public IQueryable PagContenteList
        {
            get { return _PagContenteList; }
            set { _PagContenteList = value; }
        }


        #endregion

        #region CONSTRUCTOR
        public PageContentBO()
        { }

        #endregion

        #region function
        public string GetPageContent(int pageID)
        {
            string data = string.Empty;
            try
            {

                string pageContent = (from pageRecord in BMTDataContext.PageContents.AsQueryable()
                                      where pageRecord.PageId == pageID && pageRecord.PostDate <= DateTime.Now
                                      orderby pageRecord.PostDate descending
                                      select pageRecord.Content).FirstOrDefault();


                if (pageContent != null)
                    return pageContent;
                else
                    return string.Empty;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PageContentBO> GetActiveContentDetailByPageId(int pageId)
        {
            try
            {
                return (from pageRecord in BMTDataContext.PageContents
                        where pageRecord.PageId == pageId && pageRecord.PostDate <= DateTime.Now
                        orderby pageRecord.PostDate descending
                        select new PageContentBO
                        {
                            Content = pageRecord.Content,
                            PostedDate = pageRecord.PostDate
                        }).Take(1).ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        #endregion

    }
}
