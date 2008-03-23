using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow
{
    public interface IConversationExpirationChecker
    {
        bool IsConversationExpired(Conversation c);

        /// <summary>
        /// Gets the period for the conversation pool cleaning.
        /// </summary>
        /// <remarks>
        /// in another sentence, it articulates the frequency of the <see cref="ConversationPool"/>'s cleaning of expired <see cref="Conversation"/>. 
        /// </remarks>
        TimeSpan CleanUpTimeSpan { get; }
    }
}
