using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow
{
    /// <summary>
    /// The Burrow Framework Environment
    /// </summary>
    /// <remarks>
    /// the envrionment that the application resides in.
    /// </remarks>
    public interface IFrameworkEnvironment
    {
        IBurrowConfig Configuration { get; }

        bool IsRunning { get; }

        /// <summary>
        /// Gets the num of Spanning Conversations
        /// </summary>
        int SpanningConversations { get; }

        /// <summary>
        /// ShutDown the whole thing
        /// </summary>
        void ShutDown();

        void Start();

    	/// <summary>
    	/// Get the NHibernate Configuration of a Persistence Unit
    	/// </summary>
    	/// <param name="persistenceUnitName">the name of the <see cref="PersistenceUnit"/></param>
    	/// <returns></returns>
    	/// <remarks>
		/// Please understand that the if you need to rebuild the sessionfactories after you changed the
		/// retrieved NHibernate Configure, please call <see cref="RebuildSessionFactories"/> 
		/// If you restart the Burrow Environment by calling <see cref="ShutDown"/> 
		/// and <see cref="Start"/>, your change will get lost.  
		/// </remarks>
    	NHibernate.Cfg.Configuration GetNHConfig(string persistenceUnitName);

    	/// <summary>
    	/// Force all SessionFactory get rebuild
    	/// </summary>
    	void RebuildSessionFactories();

        /// <summary>
        /// use the configurator to ReConig the environment
        /// </summary>
        /// <param name="configurator"></param>
        /// <remarks>
        /// This will restart the environment and thus must be caused when workspace is closed
        /// </remarks>
        void ReConfig(IConfigurator configurator);
    }
}