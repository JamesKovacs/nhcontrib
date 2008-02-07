namespace NHibernate.Burrow {
    /// <summary>
    /// Persistant Object that can be directly Saved or Deleted
    /// </summary>
    public interface IPersistantObjSaveDelete : IWithId {
        /// <summary>
        /// Tells if this object is transient (not saved to database yet)
        /// </summary>
        bool IsTransient { get; }

        /// <summary>
        /// Persists the object into database
        /// </summary>
        void Save();

        /// <summary>
        /// Remove the object from the database 
        /// </summary>
        void Delete();
    }
}