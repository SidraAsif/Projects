using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using System.Collections;
using System.Web.Caching;

namespace BMT.WEB
{
    public class SessionHandling
    {
        public void ClearSession()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            string CookieName = "Credential";

            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                HttpCookie newCookie = new HttpCookie(CookieName);
                newCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(newCookie);
            }
        }

    }
}
