using System.Collections.Generic;

namespace BasicSample.Core.DataInterfaces
{
    public interface IDao<T, IdT>
    {
        //Comment by Kailuo: changed several signiture here to conform to the Burrow.Util.GenericDAO
        T GetById(IdT id, bool shouldLock);
        T Get (object id );
        IList<T> FindAll();
        IList<T> FindByExample(T exampleInstance, params string[] propertiesToExclude);
        IdT Save(T entity);
        void SaveOrUpdate(T entity);
        void Delete(T entity); 
    }
}
