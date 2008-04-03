using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow
{
    public abstract class OverspanStrategy
    {
        public static readonly OverspanStrategy Cookie = new CookieStrategy();
        public static readonly OverspanStrategy DoNotSpan = new DoNotSpanStrategy();
        public static readonly OverspanStrategy Post = new PostStrategy();
        private string name;

        private OverspanStrategy(string name)
        {
            this.name = name;
        }

        public virtual bool SupportLongConversation
        {
            get { return true; }
        }

        public override string ToString()
        {
            return name;
        }

        public virtual void CleanCookies(OverspanState os, HttpContext c)
        {
            if (c.Request.Cookies.Get(os.Name) != null)
            {
                HttpCookie cookie = new HttpCookie(os.Name, string.Empty);
                cookie.Expires = DateTime.Now.AddDays(-1);
                c.Response.Cookies.Add(cookie);
            }
        }

        public abstract void AddOverspanState(Control c, OverspanState os);

        #region Nested type: CookieStrategy

        private class CookieStrategy : OverspanStrategy
        {
            public CookieStrategy() : base("Cookie") {}

            public override void AddOverspanState(Control c, OverspanState os)
            {
                c.Page.Response.Cookies.Add(new HttpCookie(os.Name, os.Value));
            }

            public override void CleanCookies(OverspanState os, HttpContext c) {}
        }

        #endregion

        #region Nested type: DoNotSpanStrategy

        private class DoNotSpanStrategy : OverspanStrategy
        {
            public DoNotSpanStrategy() : base("Do Not Span") {}

            public override bool SupportLongConversation
            {
                get { return false; }
            }

            public override void AddOverspanState(Control c, OverspanState os) {}
        }

        #endregion

        #region Nested type: GetOnlyStrategy

        /// <summary>
        /// This Strategy isn't implemented yet. The challenge is that the Urls can only be generated after all the over span states are ready
        /// </summary>
        private class GetOnlyStrategy : OverspanStrategy
        {
            public GetOnlyStrategy() : base("Get Only") {}

            public override void AddOverspanState(Control c, OverspanState os) {}

            /// <summary>
            /// 
            /// </summary>
            /// <param name="originalUrl"></param>
            /// <returns></returns>
            public string WrapUrlWithOverSpanInfo(string originalUrl)
            {
                StringBuilder sb = new StringBuilder(originalUrl);

                bool firstPara = originalUrl.IndexOf("?") < 0;
                foreach (OverspanState state in DomainContext.Current.OverspanStates())
                {
                    if (state.Strategy != DoNotSpan && !string.IsNullOrEmpty(state.Name)
                        && !string.IsNullOrEmpty(state.Value))
                    {
                        sb.Append((firstPara ? "?" : "&"));
                        sb.Append(HttpContext.Current.Server.UrlEncode(state.Name));
                        sb.Append("=");
                        sb.Append(HttpContext.Current.Server.UrlEncode(state.Value));
                        firstPara = false;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion

        #region Nested type: PostStrategy

        private class PostStrategy : OverspanStrategy
        {
            public PostStrategy() : base("Post") {}

            public override void AddOverspanState(Control c, OverspanState os)
            {
                Literal l = new Literal();
                l.Text = string.Format("<input type='hidden' name='{0}' value='{1}' />", os.Name, os.Value);
                c.Controls.Add(l);
            }
        }

        #endregion
    }
}