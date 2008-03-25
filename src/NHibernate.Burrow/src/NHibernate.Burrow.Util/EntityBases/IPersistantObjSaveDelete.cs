namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// Persistence Object that can be directly Saved or Deleted
    /// </summary>
    public interface IPersistentObjSaveDelete : IWithId
    {
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