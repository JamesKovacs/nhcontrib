using System;

namespace NHibernate.Burrow {
    /// <summary>
    /// OBSELETE
    /// A type that itself has the CRUD methods
    /// </summary>
    public interface IPersistantObjWithCRUD : IWithIdNBizKey, IPersistantObjWithDAO {
        /// <summary>
        /// Delete the PersistantObj
        /// </summary>
        /// <remarks>  will do nothing if IsDeleted is set to true</remarks>
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
        /// otherwise this will update the DB record to the current status of the object
        /// </summary>
        void Save();

        /// <summary>
        /// ReAttach the object to the current session
        /// </summary>
        /// <exception cref="Exception">Exceptions Will be thrown if there is no DB record for the transient obj</exception>
        void ReAttach();

        /// <summary>
        /// update the DB record of a persistant object
        /// </summary>
        /// <exception cref="Exception">Exceptions Will be thrown if there is no DB record for the transient obj</exception>
        void Update();
    }
}