using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using NHibernate.Burrow.Configuration;

namespace NHibernate.Burrow
{
    public class ConversationExpirationCheckerByTimeout : IConversationExpirationChecker
    {
        private TimeSpan cleanUpTimeSpan;
        private TimeSpan timeout;



        public ConversationExpirationCheckerByTimeout()
        {
            NHibernateBurrowCfgSection cfg = NHibernateBurrowCfgSection.GetInstance();
            int timeoutMinutes = cfg.ConversationTimeOut;
            if (timeoutMinutes < 1)
            {
                throw new ConfigurationErrorsException("ConversationTimeOut must be greater than 1");
            }

            timeout = TimeSpan.FromMinutes(timeoutMinutes);
            int freq = cfg.ConversationCleanupFrequency;
            if (freq < 1)
            {
                throw new ConfigurationErrorsException("ConversationCleanupFrequency must be greater than 1");
            }
            cleanUpTimeSpan = new TimeSpan(0, timeoutMinutes * freq, 0);
        }

        public bool IsConversationExpired(Conversation c)
        {
            return ( c.LastVisit + timeout ) < DateTime.Now; 
        }

        public TimeSpan CleanUpTimeSpan
        {
            get { return cleanUpTimeSpan; }
        }
    }
}
