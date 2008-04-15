using System;
using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {

    /// <summary>
    /// Facade of the Burrow 
    /// </summary>
    /// <remarks>
    /// creating an instance of this class is free.
    /// </remarks>
    public  class Facade {
        public  IConversation CurrentConversation {
            get {
                if(WorkSpace.Current != null)
                    return  WorkSpace.Current.Conversation;
                return null;
            }
        }

        /// <summary>
        /// <see cref="IFrameworkEnvironment"/>
        /// </summary>
        public IFrameworkEnvironment BurrowEnvironment {
            get { return FrameworkEnvironment.Instance; }
        }

        /// <summary>
        /// gets if the domain layer is already <see cref="InitializeDomain(bool,NameValueCollection)"/>.
        /// </summary>
        public  bool Alive {
            get { return WorkSpace.Current != null; }
        }

        /// <summary>
        /// a shotcut to <see cref="InitializeDomain(bool,NameValueCollection)"/> with (false, null) as the parameter
        /// </summary>
        public  void InitializeDomain() {
            InitializeDomain(false, null);
        }

        /// <summary>
        ///  prepare the Burrow environment for the current visit to your Domain Layer
        /// </summary>
        /// <param name="ignoreUnclosedDomain">if there is an existing Domain Context, ignoreUnclosedDomain = true will close it first. 
        ///                                     ignoreUnclosedDomain = false will throw an Exception .</param>
        /// <param name="states">the span states that should be used to initialized it, if you are not spanning the conversation, leave it null</param>
       /// <remarks>
        /// This should be called before any NHibernate related operation and actually, for example, in the begining of handling a http request
        /// if you are using  NHibernate.Burrow.WebUtil's HttpModule, it will call this for you, you don't need to worry about this.
        /// </remarks>
        public  void InitializeDomain(bool ignoreUnclosedDomain, NameValueCollection states) {
            InitializeDomain(ignoreUnclosedDomain,states,string.Empty);
        }  
        
        public  void InitializeDomain(bool ignoreUnclosedDomain, NameValueCollection states, string currentWorkSpaceName) {
            if (ignoreUnclosedDomain && Alive)
                CloseDomain();
            FrameworkEnvironment.Instance.StartNewDomainContext(states, currentWorkSpaceName);
        }

        public void InitializeDomain(Guid conversationId)
        {
            NameValueCollection nve = new NameValueCollection();
            nve.Add(ConversationImpl.ConversationIdKeyName, conversationId.ToString());
            InitializeDomain(false, nve);
        }

     
        /// <summary>
        /// close the Burrow environment for the current visit to your Domain Layer
        /// </summary>
        /// <remarks>
        /// This should be called after the current visit to the domainlayer is finished and the time of next visit is unknow, for example, at the very end of handling the http request
        /// if you are using  NHibernate.Burrow.WebUtil's HttpModule, it will call this for you, you don't need to worry about this.
        /// </remarks>
        public  void CloseDomain() {
            if (WorkSpace.Current != null)
                WorkSpace.Current.Close();
        }

        /// <summary>
        /// Gets the managed NHibernate ISession
        /// </summary>
        /// <returns></returns>
        public  ISession GetSession() {
            return (( ConversationImpl)CurrentConversation).GetSessionManager().GetSession();
        }

        /// <summary>
        /// Gets a managed ISession
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <remarks>
        /// Please do not try to close or commit transaction of this session as its status and transaction are controlled by Burrow
        /// </remarks>
        public  ISession GetSession(System.Type entityType) {
            return  (( ConversationImpl)CurrentConversation).GetSessionManager(entityType).GetSession();
        }

      
    }
}