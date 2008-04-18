using System;
using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {

    /// <summary>
    /// Facade of Burrow 
    /// </summary>
    /// <remarks>
    /// Creating an instance of this class is free. The instance is stateless and can be stored anywhere
    /// </remarks>
    public class BurrowFramework {

        /// <summary>
        /// Gets the current <see cref="IConversation"/>
        /// </summary>
        public  IConversation CurrentConversation {
            get {
                if(WorkSpace.Current != null)
                    return  WorkSpace.Current.Conversation;
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="IFrameworkEnvironment"/>
        /// </summary>
        public IFrameworkEnvironment BurrowEnvironment {
            get { return FrameworkEnvironment.Instance; }
        }

        /// <summary>
        /// gets if the Burrow workspace is already <see cref="InitWorkSpace()"/> and not <see cref="CloseWorkSpace()"/>  yet.
        /// </summary>
        public  bool WorkSpaceIsReady {
            get { return WorkSpace.Current != null; }
        }

        /// <summary>
        /// a shotcut to <see cref="InitWorkSpace(bool,NameValueCollection,string)"/> with (false, null, string.Empty) as the parameter
        /// </summary>
        public  void InitWorkSpace() {
            InitWorkSpace(false, null, string.Empty);
        }

      
        /// <summary>
        ///  prepare the Burrow environment for the current visit to your Domain Layer
        /// </summary>
        /// <param name="ignoreUnclosedWorkSpace">if there is an existing WorkSpace, ignoreUnclosedWorkSpace = true will close it first. 
        ///                                     ignoreUnclosedWorkSpace = false will throw an Exception .</param>
        /// <param name="states">the span states that should be used to initialized it, if you are not spanning the conversation, leave it null</param>
        /// <remarks>
        /// This should be called before any NHibernate related operation and actually, for example, in the begining of handling a http request
        /// if you are using  NHibernate.Burrow.WebUtil's HttpModule, as you should in an Web Application, it will call this for you, you don't need to worry about this.
        /// </remarks>
        /// <param name="currentWorkSpaceName">the workSpaceName of the current context (usually defined by the handler side)</param>
        public  void InitWorkSpace(bool ignoreUnclosedWorkSpace, NameValueCollection states, string currentWorkSpaceName) {
            if (ignoreUnclosedWorkSpace && WorkSpaceIsReady)
                CloseWorkSpace();
            FrameworkEnvironment.Instance.StartNewWorkSpace(states, currentWorkSpaceName);
        }


        /// <summary>
        /// Initialize the WorkSpace and join the conversation with <paramref name="conversationId"/>
        /// </summary>
        /// <param name="conversationId"></param>
        public void InitWorkSpace(Guid conversationId)
        { 
            InitWorkSpace(false, WorkSpace.CreateState(conversationId, string.Empty), string.Empty);
        }

     
        /// <summary>
        /// close the Burrow environment for the current visit to your Domain Layer
        /// </summary>
        /// <remarks>
        /// This should be called after the current visit to the domainlayer is finished and the time of next visit is unknow, for example, at the very end of handling the http request
        /// if you are using  NHibernate.Burrow.WebUtil's HttpModule, it will call this for you, you don't need to worry about this.
        /// </remarks>
        public void CloseWorkSpace() {
			if (WorkSpaceIsReady)
                WorkSpace.Current.Close();
        }

        /// <summary>
        /// overloaded version of <see cref="GetSession(Type)"/> in a single-SessionFactory environment
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