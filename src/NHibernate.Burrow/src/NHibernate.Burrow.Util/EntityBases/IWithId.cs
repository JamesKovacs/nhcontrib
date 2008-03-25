using System;

namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// Class with an Integer Id that can identify persistant instances.
    /// </summary>
    public interface IWithId
    {
        /// <summary>
        /// the integer Id that can identify persistant instances.
        /// </summary>
        Int32 Id { get; }
    }
}