using System;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {
    public enum ConversationalDataMode {
        /// <summary>
        /// Data will only be available in conversation it is created, outside of the conversation, exception will be thrown if it is accessed 
        /// </summary>
        Single,
        
        /// <summary>
        /// Data will only be available in conversation it is created, outside of the conversation, data will automatically reset to null 
        /// </summary>
        SingleTemp,
        
        /// <summary>
        /// One <see cref="ConversationalData{T}"/> can have different values in different converstaions, each value stick with the corresponding conversation
        /// </summary>
        Overspan
    }

    /// <summary>
    /// A Data container for conversational data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ConversationalData<T> {
        private Guid gid = Guid.Empty;
        private ConversationalDataMode mode = ConversationalDataMode.SingleTemp;
        public ConversationalData() {}

        /// <summary>
        /// 
        /// </summary>
        public ConversationalData(ConversationalDataMode mode) {
            Mode = mode;
        }

        public ConversationalData(ConversationalDataMode mode, T value) : this(value) {
            Mode = mode;
        }

        public ConversationalData(T value)
        {
            Value = value;
        }

        public ConversationalDataMode Mode {
            get { return mode; }
            private set { mode = value; }
        }

        /// <summary>
        /// Gets and sets the data value of this container
        /// </summary>
        public T Value {
            get {
                T retVal = default(T);
                if (gid == Guid.Empty)
                    return retVal;
                if (cItems == null || !cItems.TryGet(gid, out retVal)) {
                    if (Mode == ConversationalDataMode.Single)
                        ConversationUnvailableException();
                    if (Mode == ConversationalDataMode.SingleTemp)
                        gid = Guid.Empty;
                }
                return retVal;
            }
            set {
                if(cItems == null)
                {
                    if (Mode == ConversationalDataMode.Single)
                        ConversationUnvailableException();
                    return;
                }
                if (Equals(value, default(T))) {
                    if (gid != Guid.Empty)
                        cItems.Remove(gid);
                    gid = Guid.Empty;
                }
                else if (gid == Guid.Empty)
                    gid = cItems.CreateSlot(value);
                else
                    cItems.Set(gid, value);
            }
        }

        /// <summary>
        /// indicates if this data is out of conversation 
        /// </summary>
        public bool OutOfConversation {
            get { return gid != Guid.Empty && !cItems.ContainsKey(gid); }
        }

        private void ConversationUnvailableException()
        {
            string msg = Facade.CurrentConversation == null
                             ? "ConversationalData can not be accessed outside conversation. "
                               + "Make sure Conversation is intialized before visiting conversational data."
                               +
                               " It might be caused by missing <add name=\"WebUtilHTTPModule\" type=\"NHibernate.Burrow.WebUtil.WebUtilHTTPModule\" /> in the Web.Config file"
                             : "Conversation may have changed, if you don't need to keep data after conversation changed, please use SingleTemp Mode on.";
            

            throw new ConversationUnavailableException(msg);
        }
        private static GuidDataContainer cItems {
            get {
                return ConversationImpl.Current == null ? null : ConversationImpl.Current.Items;
            }
        }
    }
}