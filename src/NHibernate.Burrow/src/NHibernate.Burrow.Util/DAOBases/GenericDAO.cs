using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Burrow;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Util.DAOBases {
    /// <summary>
    /// Advanced Generic DAO whose return type can be different from Nhibernate type
    /// </summary>
    /// <typeparam name="ReturnT">Type of which the DAO return the entity</typeparam>
    public class GenericDAO<ReturnT> {
        /// <summary>
        /// Default Order Expression
        /// </summary>
        public static readonly string DefaultSortExpression = "Id desc";

        private readonly System.Type _NHEntityType;
        private SessionManager _sm;
 

        private SessionManager sm {
            get {
                if (_sm == null)
                    _sm = SessionManager.GetInstance(_NHEntityType);
                return _sm;
            }
        }

        public GenericDAO(System.Type entityType)
        {
            this._NHEntityType = entityType;
        }
        public GenericDAO() : this(typeof(ReturnT)){}

        /// <summary>
        /// Default Order when query entities
        /// </summary>
        /// <remarks>
        /// if not override, it will use id Desc 
        /// </remarks>
        public virtual Order DefaultOrder {
            get { return Order.Desc("id"); }
        }

        /// <summary>
        /// Gets if the DAO use cacheable query/criteria by default
        /// </summary>
        /// <remarks>
        /// default value is false;
        /// override to return the value you want
        /// </remarks>
        protected virtual bool DefaultCacheable {
            get { return false; }
        }

        /// <summary>
        /// Gets the default cache region 
        /// </summary>
        protected virtual string DefaultCacheRegion {
            get { return _NHEntityType.Name; }
        }

        /// <summary>
        /// Gets the Nhibernate Session 
        /// </summary>
        protected ISession Sess {
            get { return sm.GetSession(); }
        }

        /// <summary>
        /// Finds all entities of the type
        /// </summary>
        /// <returns></returns>
        public IList<ReturnT> FindAll() {
            return FindByCriteria(null);
        }

        /// <summary>
        /// Find all entities of the type with paging and sorting
        /// </summary>
        /// <param name="startRow">the index of the first record to return</param>
        /// <param name="pageSize">the number of the records to return</param>
        /// <param name="sortExpression">the expression for sorting
        /// <example> Name DESC </example>
        /// <example> Year ASC </example>
        /// </param>
        /// <returns></returns>
        public IList<ReturnT> FindAll(int startRow, int pageSize, string sortExpression) {
            return FindByCriteria(startRow, pageSize, null, sortExpression);
        }

        /// <summary>
        /// Counts all entities of the type
        /// </summary>
        /// <returns></returns>
        public int CountAll() {
            return CountByCriteria(null);
        }

        /// <summary>
        /// Reattach a detached the entity to the current session
        /// </summary>
        /// <param name="t">the entity to reAttach</param>
        public void ReAttach(ReturnT t) {
            Sess.Lock(t, LockMode.None);
        }

        /// <summary>
        /// Find the entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Will return null if not found
        /// this will always hit the database
        /// </remarks>
        public ReturnT FindById(object id) {
            return (ReturnT) Sess.Get(_NHEntityType, id);
        }

        /// <summary>
        /// Delete the record of an entity from Database and thus the entity becomes transient
        /// </summary>
        /// <param name="t"></param>
        public virtual void Delete(ReturnT t) {
            Sess.Delete(t);
        }

        /// <summary>
        /// Re-read the state of the entity from the database
        /// </summary>
        /// <param name="t"></param>
        public void Refresh(ReturnT t) {
            Sess.Refresh(t);
        }

        /// <summary>
        /// Either Save() or Update() the given instance, depending upon the value of its identifier property.  
        /// </summary>
        /// <param name="t"></param>
        /// <remarks>
        /// By default the instance is always saved. This behaviour may be adjusted by specifying an unsaved-value attribute of the identifier property mapping 
        /// </remarks>
        public void SaveOrUpdate(ReturnT t) {
            Sess.SaveOrUpdate(t);
        }

        /// <summary>
        /// Update the persistent instance with the identifier of the given transient instance.  
        /// </summary>
        /// <param name="t"> A transient instance containing updated state</param>
        /// <remarks>Transient instance<paramref name="t"/> will be attached to the Session</remarks>
        public void Update(ReturnT t) {
            Sess.Update(t);
        }

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.  
        /// </summary>
        /// <param name="t"></param>
        public void Save(ReturnT t) {
            OnSave(t);
            Sess.Save(t);
        }

        /// <summary>
        /// override to add logic prior to save an entity
        /// </summary>
        /// <param name="t"></param>
        protected virtual void OnSave(ReturnT t) {}

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
        /// For convenience, you can skip the "from XXXX" part of the HQL, the following code is also valide.
        /// <code>
        ///  dao.CreateQuery("tm where tm.TeamType = :tt");	
        /// </code>
        /// this method will automatically add "from Team " before the above query.
        /// </example>
        /// <example>
        /// you can even skip the alias "tm" if you use "this" as the alias
        /// <code>
        ///  dao.CreateQuery("where this.TeamType = :tt");	
        ///  dao.CreateQuery("order by this.CreationTime");	
        /// </code>
        /// this method will automatically add "from Team this " before the above query.
        /// </example>
        /// </remarks>
        protected IQuery CreateQuery(string queryString) {
            queryString = queryString.Trim();
            string lowerCaseQuery = queryString.ToLower();
            bool completeQuery = lowerCaseQuery.IndexOf("select ") == 0
                                 || lowerCaseQuery.IndexOf("from ") == 0;
            string pre = completeQuery ? "" : "from " + _NHEntityType.Name + " ";
            bool usingThis = lowerCaseQuery.IndexOf("order by") == 0
                             || lowerCaseQuery.IndexOf("where") == 0;
            if (usingThis)
                pre += "this ";
            IQuery retVal = Sess.CreateQuery(pre + queryString);
            if (DefaultCacheable)
                retVal.SetCacheable(DefaultCacheable).SetCacheRegion(DefaultCacheRegion);
            return retVal;
        }

        /// <summary>
        /// Count by a collection of Criterion
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        protected int CountByCriteria(IEnumerable<ICriterion> criterion) {
            return CriteriaCount(GetCriteria(criterion, null));
        }

        /// <summary>
        /// Counts the result of <paramref name="c"/>
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected int CriteriaCount(ICriteria c) {
            object o = c.SetProjection(Projections.RowCount()).UniqueResult();
            if (o == null)
                return 0;
            else
                return (int) o;
        }

        /// <summary>
        /// Find the entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// This will try find the entity in session then if not found try find it in DB, if still not find, 
        /// it will throw exception if not found.
        /// Basically it uses ISession.Load
        /// </remarks>
        public ReturnT Load(object id) {
            ReturnT retval = (ReturnT) Sess.Load(_NHEntityType, id);
            OnLoad(retval);
            return retval;
        }

        /// <summary>
        /// override to add logic after an entity is loaded
        /// </summary>
        /// <param name="loaded"></param>
        protected virtual void OnLoad(ReturnT loaded) {}

        /// <summary>
        /// Find according the criterion
        /// </summary>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriterion(ICriterion crit) {
            return GetCriteria(new ICriterion[] {crit}).List<ReturnT>();
        }

        /// <summary>
        /// Find according the criterion
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        /// <param name="criterion"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriterion(int startRow, int pageSize, string sortExpression, ICriterion criterion) {
            return
                FindByCriteria(startRow, pageSize, new ICriterion[] {criterion}, ParseOrder(sortExpression));
        }

        /// <summary>
        /// Find by a collection of Criterion
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriteria(IEnumerable<ICriterion> criteria) {
            return GetCriteria(criteria).List<ReturnT>();
        }

        /// <summary>
        /// Find by a collection of Criterion
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriteria(int startRow, int pageSize, IEnumerable<ICriterion> criteria) {
            return FindByCriteria(startRow, pageSize, criteria, string.Empty);
        }

        /// <summary>
        /// Find by a collection of Criterion
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriteria(int startRow, int pageSize, IEnumerable<ICriterion> criteria,
                                                string sortExpression) {
            return FindByCriteria(startRow, pageSize, criteria, ParseOrder(sortExpression));
        }

        /// <summary>
        /// Find by a collection of Criterion
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriteria(int startRow, int pageSize, IEnumerable<ICriterion> criteria,
                                                Order o) {
            ICriteria c = GetCriteria(criteria, o);
            return PagedCriteria(c, pageSize, startRow);
        }

        /// <summary>
        /// Find by ICriteria created by client
        /// </summary>
        /// <param name="c"></param>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        protected IList<ReturnT> FindByCriteria(ICriteria c, int startRow, int pageSize, string sortExpression) {
            Order o = ParseOrder(sortExpression);
            if (o != null) c.AddOrder(o);
            return PagedCriteria(c, pageSize, startRow);
        }

        private ICriteria GetCriteria(IEnumerable<ICriterion> criterion) {
            return GetCriteria(criterion, DefaultOrder);
        }

        private ICriteria GetCriteria(IEnumerable<ICriterion> criterion, Order odr) {
            ICriteria retVal = GetCriteria();
            if (criterion != null)
                foreach (ICriterion crit in criterion)
                    retVal.Add(crit);
            if (odr != null)
                retVal.AddOrder(odr);

            return retVal;
        }

        /// <summary>
        /// Create a Criteria instance of the Type
        /// </summary>
        /// <returns></returns>
        protected ICriteria GetCriteria() {
            ICriteria retVal = Sess.CreateCriteria(_NHEntityType);
            if (DefaultCacheable)
                retVal.SetCacheable(DefaultCacheable).SetCacheRegion(DefaultCacheRegion);
            return retVal;
        }

        /// <summary>
        /// Create a Criteria instance of the Type
        /// </summary>
        /// <returns></returns>
        protected ICriteria GetCriteria(string alias) {
            return Sess.CreateCriteria(_NHEntityType, alias);
        }

        /// <summary>
        /// Parse the NHibernate.Expression.Order from the string sortExpression
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        protected Order ParseOrder(string sortExpression) {
            if (String.IsNullOrEmpty(sortExpression))
                return DefaultOrder;
            string[] s = sortExpression.Split(' ');
            if (s.Length == 1)
                return new Order(s[0], true);
            else
                return new Order(s[0], s[1] == "ASC");
        }

        /// <summary>
        /// Query by page
        /// </summary>
        /// <param name="q"></param>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected static IList<ReturnT> PagedQuery(IQuery q, int startRow, int pageSize) {
            q.SetFirstResult(startRow);
            if (pageSize > 0)
                q.SetMaxResults(pageSize);
            return q.List<ReturnT>();
        }

        /// <summary>
        /// Query by creteria with paging surport. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="pageSize"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        private static IList<ReturnT> PagedCriteria(ICriteria c, int pageSize, int startRow) {
            c.SetFirstResult(startRow);
            if (pageSize > 0)
                c.SetMaxResults(pageSize);
            return c.List<ReturnT>();
        }

        /// <summary>
        /// Make sure the sortExpression is valid.
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        /// <remarks>if it's invalid return the default sort expression
        /// </remarks>
        protected string EnsureSortExpression(string sortExpression) {
            return string.IsNullOrEmpty(sortExpression)
                       ? DefaultSortExpression
                       : sortExpression;
        }
    }
}