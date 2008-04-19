using System;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow
{
    public interface IConversationExpirationChecker
    {
        /// <summary>
        /// Gets the period for the conversation pool cleaning.
        /// </summary>
        /// <remarks>
        /// in another sentence, it articulates the frequency of the <see cref="ConversationPool"/>'s cleaning of expired <see cref="ConversationImpl"/>. 
        /// </remarks>
        TimeSpan CleanUpTimeSpan { get; }

        bool IsConversationExpired(IConversation c);
    }
}