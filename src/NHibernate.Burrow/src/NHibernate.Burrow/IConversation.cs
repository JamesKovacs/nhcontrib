using System;

namespace NHibernate.Burrow {
    public interface IConversation {
        /// <summary>
        /// Gets the unique id of this conversation
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Span with Http Session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The conversation will be available to all requests that shares the same HttpSession
        /// </remarks>
        bool SpanWithHttpSession();

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
    }
}