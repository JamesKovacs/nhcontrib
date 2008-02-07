using System;

namespace NHibernate.Burrow.Util.EntityBases.BizTransactionEntity {
    /// <summary>
    /// an interface for entity that can be persisted temporarily for Biz transaction purpose.
    /// These entities needs to be deleted when the biz transaction times out without commit.
    /// </summary>
    /// <remarks>
    /// Some transaction Entity can be temporarily saved in the Database to support business transaction which span over seperate requests.
    /// This is a very simple solution to support business transaction without true transaction management support. 
    /// </remarks>
    public interface IBizTransactionEntity {
        /// <summary>
        /// Gets if the related Biz transaction is successfully commit. 
        /// </summary>
        bool FinishedTransaction { get; }

        /// <summary>
        /// gets the time when last activity to this entity occurred
        /// </summary>
        DateTime LastActivityInTransaction { get; set; }

        ///<summary>
        /// Delete the entity
        ///</summary>
        void Delete();
    }
}