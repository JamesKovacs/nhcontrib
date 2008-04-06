using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Impl;
using NHibernate.Burrow.Util;

namespace NHibernate.Burrow.Impl {
    /// <summary>
    /// static factory for <see cref="IConversationExpirationChecker"/>
    /// </summary>
    public static class ConversationExpirationCheckerFactory
    {
        public static IConversationExpirationChecker Create(IBurrowConfig cfg)
        {
            string checkerName =  cfg.ConversationExpirationChecker;
            if (string.IsNullOrEmpty(checkerName))
            {
                return new ConversationExpirationCheckerByTimeout();
            }
            return InstanceLoader.Load<IConversationExpirationChecker>(checkerName);
        }
    }
}