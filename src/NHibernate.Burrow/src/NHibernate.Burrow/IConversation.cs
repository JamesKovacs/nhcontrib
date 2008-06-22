using System;
using System.Collections.Generic;
using NHibernate.Burrow.DataContainers;

namespace NHibernate.Burrow
{
    /// <summary>
    ///
    /// </summary>
    public interface IConversation
    {
        /// <summary>
        /// Gets the unique id of this conversation
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets if this conversation is Spanning (either with Postbacks or HttpSessions)
        /// </summary>
        bool IsSpanning { get; }

        /// <summary>
        /// Gets the safe data bag the conversation holds
        /// </summary>
        /// <remarks>
        /// You can use this item to store conversation span data. It's safer as it requires you to use a GUID as key.
        /// Basically this is a dictionary that uses guid as keys. 
        /// Another more convenient and better way to  safely store datathat share the same life span as conversation is the <see cref="ConversationalData{T}"/>
        /// </remarks>
        GuidDataContainer SafeItems { get; }

        /// <summary>
        /// Gets the data bag the conversation holds. You can use this item to store conversation span data.
        /// </summary>
        /// <remarks>
        /// You can use it as how you want. As it's using string as key, you might have key conflict problem, for safer conversational bag, you can use SafeItems
        /// </remarks>
        IDictionary<string, object> Items { get; }

        /// <summary>
        /// Gets the string WorkSpace name, 
        /// </summary>
        /// <remarks>
        /// the workSpaceName define the group of pages/handlers within which it spans, 
        /// if not null or empty, the conversation will span within the pages/handlers with the same workSpaceName
        /// </remarks>
        string WorkSpaceName { get; }

        /// <summary>
        /// Gets the last time this conversation is visited
        /// </summary>
        DateTime LastVisit { get; }


        /// <summary>
        /// Gets if the conversation is already given up (it's data change will no long be committed)
        /// </summary>
        bool GivenUp { get; }

        /// <summary>
        /// Gets the <see cref="ITransactionManager"/> when in an Sticky WorkSpace.
        /// </summary>
        /// <remarks>
        /// For this property to be available, you must call trun on manualTransactionManagement in the configuration
        /// </remarks>
        ITransactionManager TransactionManager
        {
            get;
        }

        /// <summary>
        /// Span with Http Session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The conversation will be available to all requests that shares the same HttpSession
        /// </remarks>
        bool SpanWithCookie(string workSpaceName);

        /// <summary>
        /// Span with a chain of post http requests
        /// </summary>
        /// <returns></returns>
        bool SpanWithPostBacks();

        /// <summary>
        /// Give up the data change made in this conversation
        /// </summary>
        /// <remarks>
        /// This won't imediately close the conversation, it tells the conversation not to commit the DB change when it is closed. 
        /// </remarks>
        void GiveUp();

        /// <summary>
        /// tells the conversation to stop spanning itself and commit the data change made in it when it's closed
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// call this method when all operations in a long span conversation are successfully done
        /// </remarks>
        bool FinishSpan();
        

        /// <summary>
        /// fired when it is closed
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts">Controls the TransactionStrategy</param>
        /// <returns></returns>
        bool SpanWithPostBacks(TransactionStrategy ts);

        bool SpanWithCookie(string inWorkSpaceName, TransactionStrategy ts);
    }
}