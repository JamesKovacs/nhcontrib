using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow.Util
{
    public class WebUtil
    { 
        
        /// <summary>
        /// Wraps a url with Conversation Information so that conversation can span with Request query
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Please deter calling this method as late as possible. And only call it when you are spanning the conversation. 
        /// If you wrap a url with conversation info and finish or cancel that conversation later in the same request handle, hitting the url will cause a conversationUnavailable error.
        /// </remarks>
        /// <exception cref="IncorrectConversationSpanStatusException" >
        /// thrown if called when the current conversation isn't spanning. 
        /// </exception>
        public string WrapUrlWithConversationInfo(string originalUrl)
        {
            if (new Facade().CurrentConversation.IsSpanning)
                return WorkSpace.Current.WrapUrlWithSpanInfo(originalUrl);
            else
                throw new Exceptions.IncorrectConversationSpanStatusException(
                    "CurrentConversation Must Be In Span Before you can wrap a url");
        }

        public static void AddConversationStates(Control c)
        {
            WorkSpace.Current.UpdateToControl(c);
        }
    }
}
