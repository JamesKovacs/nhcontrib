using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Util.DAOBases
{
    /// <summary>
    /// A DAO that includes a set of shotcut methods. 
    /// </summary>
    /// <typeparam name="ReturnT">Type as which GenericDAO returns entities</typeparam>
    /// <remarks>
    /// This DAO can be inherited to add/override functions.
    /// It's totally optional to client to use it or not, as other classes in the  NHibernate.Burrow.Util namespace 
    /// </remarks>
    public class GenericDAO<ReturnT>
    {
        private readonly System.Type _NHEntityType;

        #region Constructors

        /// <summary>
        /// Constructs a GenericDAO whose <typeparamref name="ReturnT"/> is different from the mapped entity type in NHibernate
        /// </summary>
        /// <param name="entityTypeMapped"></param>
        /// <remarks>
        /// for example, you can use an unmapped interface as <typeparamref name="ReturnT"/> and a mapped implementation type as the entityTypeMapped 
        /// </remarks>
        public GenericDAO(System.Type entityTypeMapped)
        {
            _NHEntityType = entityTypeMapped;
        }

        /// <summary>
        /// Constructs a GenericDAO whose <typeparamref name="ReturnT"/> is mapped in NHibernate
        /// </summary>
        public GenericDAO() : this(typeof (ReturnT)) {}

        #endregion

        #region protected members

        /// <summary>
        /// Default Order when query entities
        /// </summary>
        /// <remarks>
        /// if not override, it will use id Desc, 
        /// As "id" is reserved in HQL to represent identifier, this should be always safe 
        /// </remarks>
        protected virtual Order DefaultOrder
        {
            get { return Order.Desc("id"); }
        }

        /// <summary>
        /// Gets if the DAO use cacheable query/criteria by default
        /// </summary>
        /// <remarks>
        /// default value is false;
        /// override to return the value you want
        /// </remarks>
        protected virtual bool DefaultCacheable
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the default cache region 
        /// </summary>
        protected virtual string DefaultCacheRegion
        {
            get { return _NHEntityType.Name; }
        }

        /// <summary>
        /// Gets the Nhibernate Session 
        /// </summary>
        protected ISession Session
        {
            get { return new BurrowFramework().GetSession(_NHEntityType); }
        }

        /// <summary>
        /// Parse the NHibernate.Expression.Order from the string sortExpression
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        protected Order ParseOrder(string sortExpression)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                return DefaultOrder;
            }
            string[] s = sortExpression.Split(' ');
            if (s.Length == 1)
            {
                return new Order(s[0], true);
            }
            else
            {
                return new Order(s[0], s[1] == "ASC");
            }
        }

        /// <summary>
        /// Gets the unique result from results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        /// <remarks>
        /// you can use this together with other helper methods to do unqiue search
        /// <example>
        /// <code>
        /// public class CustomerDAO : GenericDAO{Customer}  {
        ///      public Customer FindbyName(string name){
        ///          return UniqueResult(Find(Expression.Eq( "Name", name)); 
        ///      }
        /// }
        /// </code>
        /// </example>
        ///
        /// </remarks>
        protected T UniqueResult<T>(IList<T> results)
        {
            if (results == null || results.Count == 0)
            {
                return default(T);
            }
            if (results.Count > 1 && new HashedSet<T>(results).Count > 1)
            {
                throw new HibernateException("More than one results return");
            }
            return results[0];
        }

        #region Query Helpers

        /// <summary>
        /// Create a Hibernate query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        /// <remarks>
        /// <example>
        /// In most case, the query must start with "select " or "from " like the following
        /// <code>
        /// dao.CreateQuery("select tm from Team tm WHERE tm.TeamType = :tt");	
        /// </code>
        /// or 
        /// <code>
        /// dao.CreateQuery("from Team tm where tm.TeamType = :tt");	
        /// </code>
        /// For convenience, as the DAO already know the entity name, if you are query the entity, 
        ///  you can skip the "from XXXX" part of the HQL, the following code is also valide.
        /// <code>
        ///  dao.CreateQuery("tm where tm.TeamType = :tt");	
        /// </code>
        /// this method will automatically add "from Team " before the above query.
        /// </example>
        /// <example>
        /// you can even skip the alias "tm" if you use "this" as the alias for the current entity
        /// <code>
        ///  dao.CreateQuery("where this.TeamType = :tt");	
        ///  dao.CreateQuery("order by this.CreationTime");	
        /// </code>
        /// this method will automatically add "from Team this " before the above query.
        /// </example>
        /// </remarks>
        protected IQuery CreateQuery(string queryString)
        {
            queryString = queryString.Trim();
            string lowerCaseQuery = queryString.ToLower();
            bool completeQuery = lowerCaseQuery.IndexOf("select ") == 0 || lowerCaseQuery.IndexOf("from ") == 0;
            string pre = completeQuery ? "" : "from " + _NHEntityType.Name + " ";
            bool usingThis = lowerCaseQuery.IndexOf("order by") == 0 || lowerCaseQuery.IndexOf("where") == 0;
            if (usingThis)
            {
                pre += "this ";
            }
            IQuery retVal = Session.CreateQuery(pre + queryString);
            if (DefaultCacheable)
            {
                retVal.SetCacheable(DefaultCacheable).SetCacheRegion(DefaultCacheRegion);
            }
            return retVal;
        }

        /// <summary>
        /// Query by page
        /// </summary>
        /// <param name="q"></param>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected static IList<ReturnT> PaginatedQuery(IQuery q, int startRow, int pageSize)
        {
            q.SetFirstResult(startRow);
            if (pageSize > 0)
            {
                q.SetMaxResults(pageSize);
            }
            return q.List<ReturnT>();
        }

        #endregion

        #region Criteria Helpers

        /// <summary>
        /// Create a criteria using defaultOrder
        /// </summary>
        /// <param name="criterions"></param>
        /// <returns></returns>
        protected ICriteria CreateCriteria(IEnumerable<ICriterion> criterions)
        {
            return CreateCriteria(DefaultOrder, criterions);
        }

        /// <summary>
        /// Create Criteria using <paramref name="odr"/>
        /// </summary>
        /// <param name="odr">can pass null</param>
        /// <param name="criterion"></param>
        /// <returns></returns>
        protected ICriteria CreateCriteria(Order odr, IEnumerable<ICriterion> criterion)
        {
            ICriteria retVal = CreateCriteria();
            if (criterion != null)
            {
                foreach (ICriterion crit in criterion)
                {
                    retVal.Add(crit);
                }
            }
            if (odr != null)
            {
                retVal.AddOrder(odr);
            }

            return retVal;
        }

        /// <summary>
        /// Create a Criteria without order
        /// </summary>
        /// <returns></returns>
        protected ICriteria CreateCriteria()
        {
            ICriteria retVal = Session.CreateCriteria(_NHEntityType);
            if (DefaultCacheable)
            {
                retVal.SetCacheable(DefaultCacheable).SetCacheRegion(DefaultCacheRegion);
            }
            return retVal;
        }

        /// <summary>
        /// Create a Criteria instance of the Type
        /// </summary>
        /// <returns></returns>
        protected ICriteria CreateCriteria(string alias)
        {
            return Session.CreateCriteria(_NHEntityType, alias);
        }

        #region Find

        /// <summary>
        /// Find according the criterion
        /// </summary>
        /// <returns></returns>
        protected IList<ReturnT> Find(params ICriterion[] crit)
        {
            return CreateCriteria(crit).List<ReturnT>();
        }

        /// <summary>
        /// Query by creteria with paging surport. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="pageSize"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        protected IList<ReturnT> Find(int pageSize, int startRow, ICriteria c)
        {
            c.SetFirstResult(startRow);
            if (pageSize > 0)
            {
                c.SetMaxResults(pageSize);
            }
            return c.List<ReturnT>();
        }

        /// <summary>
        /// Find by ICriteria created by client with Pagination and Sorting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This pagination signature is designed to work with ASP.NET ObjectDataSource, for more advanced pagination, please consider NHiberante.Burrow.Util.Pagination.IPaginator, NHiberante.Burrow.Util
        /// </remarks>
        protected IList<ReturnT> Find(int startRow, int pageSize, string sortExpression, ICriteria c)
        {
            Order o = ParseOrder(sortExpression);
            if (o != null)
            {
                c.Orders.Clear();
                c.AddOrder(o);
            }
            return Find(pageSize, startRow, c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression">will use <see cref="DefaultOrder"/> if empty</param>
        /// <param name="crit"></param>
        /// <returns></returns>
        /// <remarks>
        /// This pagination signature is designed to work with ASP.NET ObjectDataSource, for more advanced pagination, please consider NHiberante.Burrow.Util.Pagination.IPaginator, NHiberante.Burrow.Util
        /// </remarks>
        protected IList<ReturnT> Find(int startRow, int pageSize, string sortExpression, params ICriterion[] crit)
        {
            return Find(startRow, pageSize, sortExpression, CreateCriteria(crit));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression">will use <see cref="DefaultOrder"/> if empty</param>
        /// <param name="crit"></param>
        /// <returns></returns>
        /// <remarks>
        /// This pagination signature is designed to work with ASP.NET ObjectDataSource, for more advanced pagination, please consider NHiberante.Burrow.Util.Pagination.IPaginator, NHiberante.Burrow.Util
        /// </remarks>
        protected IList<ReturnT> Find(int startRow, int pageSize, string sortExpression, ICollection<ICriterion> crit)
        {
            return Find(startRow, pageSize, sortExpression, CreateCriteria(crit));
        }

        #endregion

        #region Count

        /// <summary>
        /// Count by a collection of Criterion
        /// </summary>
        /// <param name="criterions"></param>
        /// <returns></returns>
        protected int Count(ICollection<ICriterion> criterions)
        {
            return Count(CreateCriteria(null, criterions));
        }

        protected int Count(params ICriterion[] criterions)
        {
            return Count(CreateCriteria(null, criterions));
        }

        /// <summary>
        /// Counts the result of <paramref name="c"/>
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected int Count(ICriteria c)
        {
            object o = c.SetProjection(Projections.RowCount()).UniqueResult();
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (int) o;
            }
        }

        #endregion

        #endregion

        #endregion

        #region public members

        #region Basic CRUD

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance. (If the instance, or a proxy for the instance, is already associated with the session, return that instance or proxy.)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a persistent instance or null</returns>
        public virtual ReturnT Get(object id)
        {
            return (ReturnT) Session.Get(_NHEntityType, id);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, assuming that the instance exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The persistent instance or proxy</returns>
        /// <remarks>
        /// You should not use this method to determine if an instance exists (use a query or NHibernate.ISession.Get(System.Type,System.Object) instead). Use this only to retrieve an instance that you assume exists, where non-existence would be an actual error.
        /// </remarks>
        public virtual ReturnT Load(object id)
        {
            ReturnT retval = (ReturnT) Session.Load(_NHEntityType, id);
            return retval;
        }

        /// <summary>
        /// Delete the record of an entity from Database and thus the entity becomes transient
        /// </summary>
        /// <param name="t"></param>
        public virtual void Delete(ReturnT t)
        {
            Session.Delete(t);
        }

        /// <summary>
        /// Re-read the state of the entity from the database
        /// </summary>
        /// <param name="t"></param>
        public virtual void Refresh(ReturnT t)
        {
            Session.Refresh(t);
        }

        /// <summary>
        /// Persist the entity <paramref name="t"/> to DB if it has not been persisted before 
        /// </summary>
        /// <param name="t"></param>
        /// <remarks>
        /// By default the instance is always saved. 
        /// This behaviour may be adjusted by specifying an unsaved-value attribute of the identifier property mapping 
        /// </remarks>
        public virtual void SaveOrUpdate(ReturnT t)
        {
            Session.SaveOrUpdate(t);
        }

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.  
        /// </summary>
        /// <param name="t">the given transient instance</param>
        /// <returns>The generated identifier
        /// </returns>
        /// <remarks>
        /// Save will use the current value of the identifier property if the Assigned generator is used.
        /// </remarks>
        public virtual object Save(ReturnT t)
        {
            return Session.Save(t);
        }

        #endregion

        #region Basic quering

        /// <summary>
        /// Finds all entities of the type
        /// </summary>
        /// <returns></returns>
        public virtual IList<ReturnT> FindAll()
        {
            return Find();
        }

        /// <summary>
        /// Find all entities of the type with paging and sorting
        /// </summary>
        /// <param name="startRow">the index of the first record to return</param>
        /// <param name="pageSize">the number of the records to return</param>
        /// <param name="sortExpression">the expression for sorting
        /// <example> Name DESC </example>
        /// <example> Year ASC </example>
        /// this parameter can be IsEmptyOrNull when sorting is not needed
        /// </param>
        /// <returns></returns>
        public virtual IList<ReturnT> FindAll(int startRow, int pageSize, string sortExpression)
        {
            return Find(startRow, pageSize, sortExpression);
        }

        /// <summary>
        /// Counts all entities of the type
        /// </summary>
        /// <returns></returns>
        public virtual int CountAll()
        {
            return Count(CreateCriteria());
        }

        public IList<ReturnT> FindByExample(ReturnT exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = CreateCriteria();
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<ReturnT>();
        }

        #endregion

        #endregion
    }
}