using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.utility;

namespace NHibernate.Burrow
{
    /// <summary>
    /// static factory for <see cref="IConversationExpirationChecker"/>
    /// </summary>
    public static class ConversationExpirationCheckerFactory
    {
        public static IConversationExpirationChecker Create()
        {
            string checkerName = NHibernateBurrowCfgSection.GetInstance().ConversationExpirationChecker;
            if (string.IsNullOrEmpty(checkerName))
            {
                return new ConversationExpirationCheckerByTimeout();
            }
            return InstanceLoader.Load<IConversationExpirationChecker>(checkerName);
        }
    }
}