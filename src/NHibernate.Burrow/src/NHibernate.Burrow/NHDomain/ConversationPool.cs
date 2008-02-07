using System;
using System.Collections.Generic;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.NHDomain.Exceptions;

namespace NHibernate.Burrow.NHDomain {
    internal class ConversationPool {
        private static readonly ConversationPool instance = new ConversationPool();
        private readonly IDictionary<Guid, ConversationPoolItem> pool = new Dictionary<Guid, ConversationPoolItem>();
        private TimeSpan cleanUpTimeSpan;
        private DateTime nextCleanup = DateTime.Now;
        private TimeSpan timeOut;

        private ConversationPool() {
            MHDomainTemplateSection cfg = MHDomainTemplateSection.GetInstance();
            int timeoutMinutes = cfg.ConversationTimeOut;
            if (timeoutMinutes < 1)
                throw new ConfigurationErrorsException("ConversationTimeOut must be greater than 1");

            timeOut = new TimeSpan(0, timeoutMinutes, 0);
            int freq = cfg.ConversationCleanupFrequency;
            if (freq < 1)
                throw new ConfigurationErrorsException("ConversationCleanupFrequency must be greater than 1");
            cleanUpTimeSpan = new TimeSpan(0, timeoutMinutes*freq, 0);
        }

        internal TimeSpan ConversationTimeout {
            get { return timeOut; }
        }

        public static ConversationPool Instance {
            get { return instance; }
        }

        ///<summary>
        ///Gets or sets the element with the specified key.
        ///</summary>
        ///
        ///<returns>
        ///The element with the specified key.
        ///</returns>
        ///
        ///<param name="key">The key of the element to get or set.</param>
        ///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        public Conversation this[Guid key] {
            get {
                CheckGuid(key);
                ConversationPoolItem item;
                if (pool.TryGetValue(key, out item))
                    return item.Conversation;
                else
                    throw new ConversationUnavailableException("Conversation (" + key +
                                                               ") does not exsits in the pool, it may already expired. "
                                                               +
                                                               ". Do not try to recover a conversation after an exception occurred.");
            }
        }

        private void CheckGuid(Guid key) {
            if (key == Guid.Empty)
                throw new ArgumentException("Empty Guid cannot be used to retrieve a Conversation");
        }

        public void Add(Guid key, Conversation value) {
            CheckGuid(key);
            lock (this) {
                if (pool.ContainsKey(key))
                    throw new ArgumentException(
                        "this key has been used and thus cannot be used to add conversation to the pool");

                CleanUpTimeoutConversation();
                pool[key] = new ConversationPoolItem(value);
            }
        }

        public void Remove(Guid key) {
            lock (this) {
                pool.Remove(key);
            }
        }

        private void CleanUpTimeoutConversation() {
            if (nextCleanup < DateTime.Now)
                return;
            List<Guid> timeOutedIds = new List<Guid>(pool.Keys.Count);

            foreach (KeyValuePair<Guid, ConversationPoolItem> pair in pool)
                if (pair.Value.TimeOutAt > DateTime.Now)
                    timeOutedIds.Add(pair.Key);

            foreach (Guid id in timeOutedIds)
                pool.Remove(id);
            nextCleanup += cleanUpTimeSpan;
        }
    }

    internal class ConversationPoolItem {
        private readonly Conversation conversation;
        private DateTime lastUsed = DateTime.Now;
        private TimeSpan timeOut;

        public ConversationPoolItem(Conversation conversation) {
            this.conversation = conversation;
            timeOut = ConversationPool.Instance.ConversationTimeout;
        }

        public TimeSpan TimeOut {
            get { return timeOut; }
        }

        public DateTime TimeOutAt {
            get { return lastUsed + TimeOut; }
        }

        public Conversation Conversation {
            get {
                lastUsed = DateTime.Now;
                return conversation;
            }
        }
    }
}