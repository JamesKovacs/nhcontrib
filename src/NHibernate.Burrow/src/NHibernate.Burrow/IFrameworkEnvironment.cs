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
    }
}