using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BMTBLL.Enumeration;
namespace BMTBLL
{
    public class InsertContentBO : BMTConnection
    {
        private PageContent _PageContent { get; set; }
        private int PageContentId;
        private IQueryable<Page> _PageList;

        public IQueryable<Page> PageList
        {
            get { return _PageList; }
            set { _PageList = value; }
        }

        private IQueryable<Page> _SelectedPageList;
        public IQueryable<Page> SelectedPageList
        {
            get { return _SelectedPageList; }
            set { _SelectedPageList = value; }
        }
        private int _PageId { get; set; }
        public int PageId
        {
            get { return _PageId; }
            set { _PageId = value; }
        }

        private int _Name { get; set; }
        public int Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Content;

        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        protected DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        protected int _CreatedBy;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        protected DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }
        protected int _LastUpdatedBy;
        public int LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }
        protected DateTime _PostDate;
        public DateTime PostDate
        {
            get { return _PostDate; }
            set { _PostDate = value; }
        }

        public void GetPage(enUserRole userRoleType, int enterpriseId)
        {
            try
            {
                _PageList = (from PageRecord in BMTDataContext.Pages
                             where PageRecord.RoleId == (int)userRoleType
                             && PageRecord.Configurable == true
                             && PageRecord.EnterpriseId == enterpriseId
                             select PageRecord);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public int GetPageContentId(int PageId, DateTime PostDate)
        {
            try
            {
                DateTime _Postdate = PostDate.Date;
                int pageContentId = 0;
                var pageContent = (from ContentRecord in BMTDataContext.PageContents
                                   where ContentRecord.PageId == PageId && ContentRecord.PostDate == _Postdate
                                   select ContentRecord);

                if (pageContent.Count() != 0)
                {
                    foreach (var pagedetail in pageContent)
                    {
                        pageContentId = pagedetail.PageContentId;
                    }
                }
                if (pageContentId > 0)
                {
                    return pageContentId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }
        public int Save(int PageId, string Content, DateTime CreatedDate, int CreatedBy, DateTime LastUpdatedDate, int LastUpdatedBy, DateTime PostDate)
        {
            try
            {
                _PageContent = new PageContent();
                _PageId = PageId;
                _Content = Content;
                _CreatedDate = CreatedDate;
                _CreatedBy = CreatedBy;
                _LastUpdatedDate = LastUpdatedDate;
                _LastUpdatedBy = LastUpdatedBy;
                _PostDate = PostDate;

                PageContentId = GetPageContentId(PageId, PostDate);
                if (PageContentId == 0)
                {
                    PageContentId = Insert();
                    return PageContentId;
                }
                else
                {
                    PageContentId = Update();
                    return PageContentId;
                }

            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;
            }
            return _PageContent.PageContentId;
        }
        public int Insert()
        {
            try
            {
                using (var scope = new System.Transactions.TransactionScope())
                {
                    _PageContent = new PageContent();
                    _PageContent.PageId = this.PageId;
                    _PageContent.Content = this.Content;
                    _PageContent.CreatedDate = this.CreatedDate;
                    _PageContent.CreatedBy = this.CreatedBy;
                    _PageContent.LastUpdatedDate = this.LastUpdatedDate;
                    _PageContent.LastUpdatedBy = this.LastUpdatedBy;
                    _PageContent.PostDate = this.PostDate;
                    BMTDataContext.PageContents.InsertOnSubmit(_PageContent);
                    BMTDataContext.SubmitChanges();
                    PageContentId = _PageContent.PageContentId;
                    scope.Complete();

                }


            }
            catch (Exception Ex)
            {
                string error = Ex.StackTrace;

            }
            return PageContentId;
        }
        public int Update()
        {
            try
            {
                var contentRecord = (from ContentRecord in BMTDataContext.PageContents
                                     where ContentRecord.PageContentId == PageContentId
                                     select ContentRecord).SingleOrDefault();
                if (contentRecord != null)
                {
                    contentRecord.Content = this.Content;
                    BMTDataContext.SubmitChanges();
                }
            }
            catch (Exception exception)
            { throw exception; }
            return PageContentId;
        }
        public List<string> GetPageContent(int pagecontentid)
        {
            try
            {

                var contentRecord = (from ContentRecord in BMTDataContext.PageContents
                                     where ContentRecord.PageContentId == pagecontentid
                                     select new { ContentRecord.Content, ContentRecord.PostDate, ContentRecord.PageId }).SingleOrDefault();


                List<string> PageContentList = new List<string>();
                if (contentRecord != null)
                {

                    PageContentList.Add(contentRecord.Content);
                    PageContentList.Add(contentRecord.PostDate.ToString());
                    PageContentList.Add(contentRecord.PageId.ToString());

                }
                return PageContentList;
            }
            catch (Exception exception)
            { throw exception; }
        }

        public void GetSelectedPage(int PageId)
        {
            try
            {
                _SelectedPageList = (from ContentRecord in BMTDataContext.Pages
                                     where ContentRecord.PageId == PageId
                                     select ContentRecord);

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }
        
    }
}
