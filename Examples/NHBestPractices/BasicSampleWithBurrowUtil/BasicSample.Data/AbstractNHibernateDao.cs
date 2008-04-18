using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Burrow;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Criterion;
using BasicSample.Core.DataInterfaces;

namespace BasicSample.Data
{
    public abstract class AbstractNHibernateDao<T, IdT> : GenericDAO<T>, IDao<T, IdT>
    {
        //Comment from Burrow, you can inherit GenericDAO to add or override methods to extend it
        /// <summary>
        /// Loads an instance of type TypeOfListItem from the DB based on its ID.
        /// </summary>
        public T GetById(IdT id, bool shouldLock) {
            T entity;

            if (shouldLock) {
                entity = (T)Session.Load(typeof(T), id, LockMode.Upgrade);
            }
            else {
                entity = (T)Session.Load(typeof(T), id);
            }

            return entity;
        }

        public new IdT Save(T entity) {
            return (IdT) base.Save(entity);
        }

    }
}
