using System;

namespace NHibernate.Burrow.AppBlock.DomainSession
{
    /// <summary>
    /// A container used to stored all the with-state helper class in the domainLayer
    /// </summary>
    public interface IDomainSession : IDisposable
    {
        /// <summary>
        /// make sure all connections to outside resources are closed
        /// </summary>
        void Close();

        /// <summary>
        /// make sure all the connections to outside resources are ready
        /// </summary>
        void Open();
    }
}