using System;
using System.Collections.Generic;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow.Impl {
    internal class ConversationPool
    {
        private static  readonly  ConversationPool instance = new ConversationPool(new Facade().BurrowEnvironment.Configuration);
        private readonly IDictionary<Guid, ConversationPoolItem> pool = new Dictionary<Guid, ConversationPoolItem>();

        private DateTime nextCleanup = DateTime.Now;
        private IConversationExpirationChecker expirationChecker;

        public IConversationExpirationChecker ExpirationChecker
        {
            get { return expirationChecker; }
            set { expirationChecker = value; }
        }

        internal ConversationPool(IBurrowConfig cfg)
        {
            ExpirationChecker = ConversationExpirationCheckerFactory.Create(cfg);
        }

        public int ConversationsInPool
        {
            get { return pool.Count; }
        }

        
    

        public static ConversationPool Instance
        {
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
        public ConversationImpl this[Guid key]
        {
            get
            {
                CheckGuid(key);
                ConversationPoolItem item;
                if (pool.TryGetValue(key, out item))
                {
                    return item.Conversation;
                }
                else
                {
                    throw new ConversationUnavailableException("Conversation (" + key
                                                               +
                                                               ") does not exsits in the pool, it may already expired. "
                                                               +
                                                               ". Do not try to recover a conversation after an exception occurred.");
                }
            }
        }

        private void CheckGuid(Guid key)
        {
            if (key == Guid.Empty)
            {
                throw new ArgumentException("Empty Guid cannot be used to retrieve a Conversation");
            }
        }

        public void Add(Guid key, ConversationImpl value)
        {
            CheckGuid(key);
            lock (this)
            {
                if (pool.ContainsKey(key))
                {
                    throw new ArgumentException(
                        "this key has been used and thus cannot be used to add conversation to the pool");
                }

                CleanUpTimeoutConversation();
                pool[key] = new ConversationPoolItem(value);
            }
        }

        public void Remove(Guid key)
        {
            lock (this)
            {
                pool.Remove(key);
            }
        }

        private void CleanUpTimeoutConversation()
        {
            if (nextCleanup < DateTime.Now)
            {
                return;
            }
            List<Guid> timeOutedIds = new List<Guid>(pool.Keys.Count);

            foreach (KeyValuePair<Guid, ConversationPoolItem> pair in pool)
            {
                if (ConversationIsTimeout(pair))
                {
                    timeOutedIds.Add(pair.Key);
                }
            }

            foreach (Guid id in timeOutedIds)
            {
                pool.Remove(id);
            }
            nextCleanup +=  ExpirationChecker.CleanUpTimeSpan;
        }
         
        private bool ConversationIsTimeout(KeyValuePair<Guid, ConversationPoolItem> pair)
        {
            return ExpirationChecker.IsConversationExpired(pair.Value.Conversation);
        }

        public bool ContainsKey(Guid id)
        {
            return pool.ContainsKey(id);
        }

        public void Clear() {
            for (IEnumerator<ConversationPoolItem> en = pool.Values.GetEnumerator();
                 en.MoveNext();
                 en = pool.Values.GetEnumerator())
                 en.Current.Conversation.RollbackAndClose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remark>
    /// not sure if this is needed any more as I moved both timeout and last visited information into <see cref="conversation"/>
    /// but it still maybe needed in the future, so I keep it here. 
    /// </remark>
    internal class ConversationPoolItem
    {
        private readonly ConversationImpl conversation;

        public ConversationPoolItem(ConversationImpl conversation)
        {
            this.conversation = conversation;
        }

 
        public ConversationImpl Conversation
        {
            get { return conversation; }
        }
    }
}