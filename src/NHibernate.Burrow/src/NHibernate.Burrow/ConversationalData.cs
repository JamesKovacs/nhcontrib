using System;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow
{
    public enum ConversationalDataMode
    {
        /// <summary>
        /// Data will only be available in conversation it is created, outside of the conversation, exception will be thrown if it is accessed 
        /// </summary>
        Normal,

        /// <summary>
        /// Data will only be available in conversation it is created, once visited outside of the conversation, data will automatically reset to null 
        /// </summary>
        OutOfConversationSafe,
    }

    /// <summary>
    /// A Data container for conversational data that needs to have the same life span with a conversation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// use it when you need some data to have the same life span as the current conversation.
    /// this class does not actually hold a reference to the <see cref="Value"/> which is actually stored in <see cref="IConversation.SafeItems"/> 
    /// Thus <see cref="ConversationalData{T}"/> can be cheaply serialized and stored. 
    /// for example, in a Asp.net application, you can put an entity into a ConversationalData(entity) and then save the conversationalData instance into the ViewState or HttpSession 
    /// </remarks>
    [Serializable]
    public class ConversationalData<T>
    {
        private Guid gid = Guid.Empty;
        private ConversationalDataMode mode = ConversationalDataMode.OutOfConversationSafe;
        public ConversationalData() {}

        /// <summary>
        /// 
        /// </summary>
        public ConversationalData(ConversationalDataMode mode)
        {
            Mode = mode;
        }

        public ConversationalData(ConversationalDataMode mode, T value) : this(value)
        {
            Mode = mode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">the real data you want this container to hold</param>
        /// <remarks>
        /// The default <see cref="Mode"/> is <see cref="ConversationalDataMode.OutOfConversationSafe"/>
        /// </remarks>
        public ConversationalData(T value)
        {
            Value = value;
        }

        public ConversationalDataMode Mode
        {
            get { return mode; }
            private set { mode = value; }
        }

        /// <summary>
        /// Gets and sets the data stored in this container
        /// </summary>
        public T Value
        {
            get
            {
                T retVal = default(T);
                if (gid == Guid.Empty)
                {
                    return retVal;
                }
                if (cItems == null || !cItems.TryGet(gid, out retVal))
                {
                    if (Mode == ConversationalDataMode.Normal)
                    {
                        ConversationUnvailableException();
                    }
                    if (Mode == ConversationalDataMode.OutOfConversationSafe)
                    {
                        gid = Guid.Empty;
                    }
                }
                return retVal;
            }
            set
            {
                if (cItems == null)
                {
                    if (Mode == ConversationalDataMode.Normal)
                    {
                        ConversationUnvailableException();
                    }
                    return;
                }
                if (Equals(value, default(T)))
                {
                    if (gid != Guid.Empty)
                    {
                        cItems.Remove(gid);
                    }
                    gid = Guid.Empty;
                }
                else if (gid == Guid.Empty)
                {
                    gid = cItems.CreateSlot(value);
                }
                else
                {
                    cItems.Set(gid, value);
                }
            }
        }

        /// <summary>
        /// indicates if this data is out of conversation 
        /// </summary>
        public bool OutOfConversation
        {
            get { return cItems == null || ( gid != Guid.Empty && !cItems.ContainsKey(gid)); }
        }

        private static GuidDataContainer cItems
        {
            get
            {
                BurrowFramework f = new BurrowFramework();
                return f.CurrentConversation == null ? null : f.CurrentConversation.SafeItems;
            }
        }

        private void ConversationUnvailableException()
        {
            string msg = new BurrowFramework().CurrentConversation == null
                             ? "ConversationalData can not be accessed outside conversation. "
                               + "Make sure Conversation is intialized before visiting conversational data."
                               +
                               " It might be caused by missing <add name=\"WebUtilHTTPModule\" type=\"NHibernate.Burrow.WebUtil.WebUtilHTTPModule\" /> in the Web.Config file"
                             : "Conversation may have changed, if you don't need to keep data after conversation changed, please use OutOfConversationSafe Mode on.";

            throw new ConversationUnavailableException(msg);
        }
    }
}