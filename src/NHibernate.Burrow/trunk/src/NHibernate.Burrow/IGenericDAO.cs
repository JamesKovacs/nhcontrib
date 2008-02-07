using System;
using System.Collections.Generic;

namespace NHibernate.Burrow {
    /// <summary>
    /// Obsolete
    /// Generic DAO ( Data Access Object ) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IGenericDAO<T> {
        /// <summary>
        /// Find the object byId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindById(object id);

        /// <summary>
        /// Finds all entities of the type
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll();

        /// <summary>
        /// Finds all entities of the type
        /// </summary>
        /// <returns></returns>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        IList<T> FindAll(int startRow, int pageSize, string sortExpression);

        /// <summary>
        /// Count entities of the Type
        /// </summary>
        /// <returns></returns>
        int CountAll();

        /// <summary>
        /// Make the object transient, and set the IsDeleted to true.
        /// </summary>
        void Delete(T t);

        /// <summary>
        ///  Refresh the object from database
        /// </summary>
        /// <param name="t"></param>
        void Refresh(T t);

        /// <summary>
        /// if the object is transient, this will make the object persistant into DB
        /// otherwise this will update the DB record according to the current status of the object
        /// </summary>
        /// <param name="t"></param>
        void SaveOrUpdate(T t);

        /// <summary>
        /// if the object is transient, this will make the object persistant into DB
        /// otherwise this will throw an exception
        /// </summary>
        /// <param name="t"></param>
        void Save(T t);

        /// <summary>
        /// update the DB record of a persistant object
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="Exception">Exception Will be thrown if there is no DB record for the persistant obj</exception>
        void Update(T t);

        /// <summary>
        /// ReAttach a detached object to the current session
        /// </summary>
        /// <param name="t"></param>
        void ReAttach(T t);
    }
}