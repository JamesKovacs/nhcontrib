using System;
using System.Collections.Specialized;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow
{
    /// <summary>
    /// Facade of Burrow 
    /// </summary>
    /// <remarks>
    /// Creating an instance of this class is free. The instance is stateless and can be stored anywhere
    /// </remarks>
    public class BurrowFramework
    {
        /// <summary>
        /// Gets the current <see cref="IConversation"/>
        /// </summary>
        public IConversation CurrentConversation
        {
            get
            {
                if (WorkSpace.Current != null)
                {
                    return WorkSpace.Current.Conversation;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="IFrameworkEnvironment"/>
        /// </summary>
        public IFrameworkEnvironment BurrowEnvironment
        {
            get { return FrameworkEnvironment.Instance; }
        }

        /// <summary>
        /// gets if the Burrow workspace is already <see cref="InitWorkSpace()"/> and not <see cref="CloseWorkSpace()"/>  yet.
        /// </summary>
        public bool WorkSpaceIsReady
        {
            get { return WorkSpace.Current != null; }
        }

        /// <summary>
        /// a shotcut to <see cref="InitWorkSpace(bool,NameValueCollection,string)"/> with (false, null, string.Empty) as the parameter
        /// </summary>
        public void InitWorkSpace()
        {
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
        public void InitWorkSpace(bool ignoreUnclosedWorkSpace, NameValueCollection states, string currentWorkSpaceName)
        {
            if (ignoreUnclosedWorkSpace && WorkSpaceIsReady)
            {
                CloseWorkSpace();
            }
            FrameworkEnvironment.Instance.InitWorkSpace(states, currentWorkSpaceName);
        }

        /// <summary>
        /// StartNew the WorkSpace and join the conversation with <paramref name="conversationId"/>
        /// </summary>
        /// <param name="conversationId"></param>
        public void InitWorkSpace(Guid conversationId)
        {
            InitWorkSpace(false, WorkSpace.CreateState(conversationId, string.Empty), string.Empty);
        }

        ///<summary>
        /// Initiate a work space that can have a long life time. 
        /// On the other hand it requires semi-auto transaction management by you, see <see cref="ITransactionManager"/>.
        /// You can use it within a winform environment where you need 
        /// long session and multiple transactions within the session. 
        ///</summary>
        public void InitStickyWorkSpace()
        {
            FrameworkEnvironment.Instance.InitMultipleTransactionEnabledWorkSpace();
        }

        /// <summary>
        /// close the Burrow environment for the current visit to your Domain Layer
        /// </summary>
        /// <remarks>
        /// This should be called after the current visit to the domainlayer is finished 
        /// and the time of next visit is unknow, for example, 
        /// at the very end of handling the http request.
        /// If you are using  NHibernate.Burrow.WebUtil's HttpModule,
        /// it will call this for you, you don't need to worry about this.
        /// </remarks>
        public void CloseWorkSpace()
        {
            if (WorkSpaceIsReady)
            {
                WorkSpace.Current.Close();
            }
        }

        /// <summary>
        /// overloaded version of <see cref="GetSession(Type)"/> in a single-Database environment
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            return ((AbstractConversation) CurrentConversation).GetSessionManager().GetSession();
        }

        /// <summary>
        /// Gets a managed ISession
        /// </summary>
        /// <param name="entityType">
        /// The entity type whose mapping is included in the SessionFactory,
        /// when there are multiple databases, Burrow use this to locate the right one
        /// </param>
        /// <returns>The Burrow managed ISession</returns>
        /// <remarks>
        /// Please do not try to close or commit transaction of this session as its status and transaction are controlled by Burrow.
        /// To get an unmanaged session please use GetSessionFactory()
        /// To setup the interceptor for every managed ISession for a persistent Unit, <see cref="IPersistenceUnitCfg.InterceptorFactory"/>
        /// </remarks>
        public ISession GetSession(System.Type entityType)
        {
            return GetSessionManager(entityType).GetSession();
        }

        private SessionManager GetSessionManager(System.Type entityType) {
            return ((AbstractConversation) CurrentConversation).GetSessionManager(entityType);
        }

        /// <summary>
        /// Gets the ISessionFactory
        /// </summary>
        /// <param name="entityType">the entity type whose mapping is included in the SessionFactory, 
        /// when there are multiple databases, Burrow use this to locate the right one</param>
        /// <returns>the sessionFactory</returns>
        /// <remarks>
        /// For getting a Session please use <see cref="GetSession(Type)"/> as it's managed by Burrow. 
        /// If you use OpenSession() of this SessionFactory, 
        /// the session you get won't be managed by Burrow 
        /// and you will be responsible for managing the status of that session yourself 
        /// </remarks>
        public ISessionFactory GetSessionFactory(System.Type entityType)
        {
            return GetSessionManager(entityType).SessionFactory;
        } 
        
    }
}