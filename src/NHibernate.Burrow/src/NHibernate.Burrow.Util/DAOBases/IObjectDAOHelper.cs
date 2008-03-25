using System;

namespace NHibernate.Burrow.Util.DAOBases
{
    /// <summary>
    /// This is a helper that goes with a persistant object to
    /// 1) offer helper mothods to the persistant object, 
    /// 2) maintain persistance states for the persistant object (such as IsDeleted, IsTransient)
    /// </summary>
    public interface IObjectDAOHelper
    {
        /// <summary>
        /// if the object is already 
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// if the object is Transient
        /// </summary>
        bool IsTransient { get; }

        /// <summary>
        /// fired when the object is about to be deleted
        /// </summary>
        event EventHandler PreDeleted;

        /// <summary>
        /// Delete the PersistentObj
        /// </summary>
        /// <remarks>  will do nothing if the entity is either already deleted or still transient</remarks>
        void Delete();

        /// <summary>
        ///  Refresh the object from database
        /// </summary>
        void Refresh();

        /// <summary>
        /// if the object is transient, this will make the object persistant into DB
        /// otherwise this will update the DB record to the current status of the object
        /// </summary>
        void SaveOrUpdate();

        /// <summary>
        /// if the object is transient, this will make the object persistant into DB
        /// otherwise this will throw an exception
        /// </summary>
        void Save();

        /// <summary>
        /// ReAttach a detached object with the current session
        /// </summary>
        void ReAttach();

        /// <summary>
        /// update the DB record of a persistant object
        /// </summary>
        /// <exception cref="Exception">Exceptions Will be thrown if there is no DB record for the transient obj</exception>
        void Update();
    }
}