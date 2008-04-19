using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// the strategy with which Burrow span the Conversation
    /// </summary>
    internal abstract class SpanStrategy
    {
        public static readonly SpanStrategy Cookie = new CookieStrategy();
        public static readonly SpanStrategy DoNotSpan = new DoNotSpanStrategy();
        public static readonly SpanStrategy GetOnly = new UrlQueryOnlyStrategy();
        public static readonly SpanStrategy Post = new PostStrategy();
        private string name;

        private SpanStrategy(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// indicate if this strategy does span
        /// </summary>
        public virtual bool ValidForSpan
        {
            get { return true; }
        }

        protected abstract bool UseCookie { get; }

        public override string ToString()
        {
            return name;
        }

        public abstract void AddOverspanStateWhenRendering(Control c, string key, string val);

        public void UpdateSpanStates(HttpContext c, string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, UseCookie ? value : string.Empty);
            if (!UseCookie)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }

            updateCookie(cookie, c.Response.Cookies);
            updateCookie(cookie, c.Request.Cookies);
            //added here for so that even the states will be there even when request is Redirected and no reponse is generated
        }

        private static void updateCookie(HttpCookie cookie, HttpCookieCollection cookies)
        {
            cookies.Remove(cookie.Name);
            cookies.Add(cookie);
        }

        #region Nested type: CookieStrategy

        private class CookieStrategy : SpanStrategy
        {
            public CookieStrategy() : base("Cookie") {}

            protected override bool UseCookie
            {
                get { return true; }
            }

            public override void AddOverspanStateWhenRendering(Control c, string key, string val) {}
        }

        #endregion

        #region Nested type: DoNotSpanStrategy

        private class DoNotSpanStrategy : SpanStrategy
        {
            public DoNotSpanStrategy() : base("Do Not Span") {}

            public override bool ValidForSpan
            {
                get { return false; }
            }

            protected override bool UseCookie
            {
                get { return false; }
            }

            public override void AddOverspanStateWhenRendering(Control c, string key, string val) {}
        }

        #endregion

        #region Nested type: PostStrategy

        private class PostStrategy : SpanStrategy
        {
            public PostStrategy() : base("Post") {}

            protected override bool UseCookie
            {
                get { return false; }
            }

            public override void AddOverspanStateWhenRendering(Control c, string key, string val)
            {
                HtmlInputHidden hi = new HtmlInputHidden();
                hi.Name = key;
                hi.ID = key;
                hi.Value = val;
                c.Controls.Add(hi);
            }
        }

        #endregion

        #region Nested type: UrlQueryOnlyStrategy

        /// <summary>
        /// This Strategy span by using Url Query only 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        private class UrlQueryOnlyStrategy : SpanStrategy
        {
            public UrlQueryOnlyStrategy() : base("Url Query Only") {}

            protected override bool UseCookie
            {
                get { return false; }
            }

            public override void AddOverspanStateWhenRendering(Control c, string key, string val) {}
        }

        #endregion
    }
}