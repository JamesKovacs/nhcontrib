using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;
using NHibernate.Collection;
using System.Runtime.Serialization;
using System.Collections;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Loader.Custom;
using NHibernate.Engine.Query.Sql;
using NHibernate.Type;
using NHibernate.Event;
using System.Data;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
/**
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */

public abstract class AbstractDelegateSessionImplementor : ISessionImplementor {
    protected ISessionImplementor delegat;

    public AbstractDelegateSessionImplementor(ISessionImplementor delegat) {
        this.delegat = delegat;
    }

    public abstract Object doImmediateLoad(String entityName);

    public Object immediateLoad(String entityName, Object id) { 
        return doImmediateLoad(entityName);
    }

    // Delegate methods

	public IInterceptor getInterceptor() {
        return delegat.Interceptor;
    }

    public void setAutoClear(bool enabled) {
        //delegat.setAutoClear(enabled);// not implemented yet
        throw new NotImplementedException();
    }

    public bool isTransactionInProgress() {
        return delegat.TransactionInProgress;
    }

    public void initializeCollection(IPersistentCollection collection, bool writing)  {
        delegat.InitializeCollection(collection, writing);
    }

    public Object internalLoad(String entityName, object id, bool eager, bool nullable) {
        return delegat.InternalLoad(entityName, id, eager, nullable);
    }

    public long getTimestamp() {
        return delegat.Timestamp;
    }

    public ISessionFactoryImplementor getFactory() {
        return delegat.Factory;
    }

    public IBatcher getBatcher() {
        return delegat.Batcher;
    }
    // TODO in second implementation phase
    //public IList list(String query, QueryParameters queryParameters) {
    //    return delegat.List<query, queryParameters>;
    //}
    // TODO in second implementation phase
    //public Iterator iterate(String query, QueryParameters queryParameters)  {
    //    return delegat.iterate(query, queryParameters);
    //}

    // TODO in second implementation phase
    //public ScrollableResults scroll(String query, QueryParameters queryParameters) throws HibernateException {
    //    return delegate.scroll(query, queryParameters);
    //}

        // TODO in second implementation phase
    //public ScrollableResults scroll(CriteriaImpl criteria, ScrollMode scrollMode) {
    //    return delegate.scroll(criteria, scrollMode);
    //}

    public IList list(CriteriaImpl criteria) {
        return delegat.List(criteria);
    }

    public IList listFilter(Object collection, String filter, QueryParameters queryParameters)  {
        return delegat.ListFilter(collection, filter, queryParameters);
    }
    // TODO in second implementation phase
    //public IEnumerator iterateFilter(Object collection, String filter, QueryParameters queryParameters){
    //    return delegat.iterateFilter(collection, filter, queryParameters);
    //}

    public IEntityPersister getEntityPersister(String entityName, Object obj)  {
        return delegat.GetEntityPersister(entityName, obj);
    }

    public Object getEntityUsingInterceptor(EntityKey key) {
        return delegat.GetEntityUsingInterceptor(key);
    }

    public void afterTransactionCompletion(bool successful, ITransaction tx) {
        delegat.AfterTransactionCompletion(successful, tx);
    }

    public void beforeTransactionCompletion(ITransaction tx) {
        delegat.BeforeTransactionCompletion(tx);
    }

    public object getContextEntityIdentifier(Object obj) {
        return delegat.GetContextEntityIdentifier(obj);
    }

    public String bestGuessEntityName(Object obj) {
        return delegat.BestGuessEntityName(obj);
    }

    public String guessEntityName(Object entity) {
        return delegat.GuessEntityName(entity);
    }

    public Object instantiate(String entityName, object id) {
        return delegat.Instantiate(entityName, id);
    }
    //// TODO in second implementation phase
    //public IList listCustomQuery(ICustomQuery customQuery, QueryParameters queryParameters) {
    //    //IList list;
    //    //delegat.ListCustomQuery(customQuery, queryParameters, list);
    //    //return list;
    //}

    //public ScrollableResults scrollCustomQuery(CustomQuery customQuery, QueryParameters queryParameters) throws HibernateException {
    //    return delegate.scrollCustomQuery(customQuery, queryParameters);
    //}

    public IList list(NativeSQLQuerySpecification spec, QueryParameters queryParameters) {
        return delegat.List(spec, queryParameters);
    }
    // TODO in second implementation phase
    //public ScrollableResults scroll(NativeSQLQuerySpecification spec, QueryParameters queryParameters) throws HibernateException {
    //    return delegate.scroll(spec, queryParameters);
    //}

    public Object getFilterParameterValue(String filterParameterName) {
        return delegat.GetFilterParameterValue(filterParameterName);
    }

    public IType getFilterParameterType(String filterParameterName) {
        return delegat.GetFilterParameterType(filterParameterName);
    }

    public IDictionary<String, IFilter> getEnabledFilters() {
        return delegat.EnabledFilters;
    }

    public int getDontFlushFromFind() {
        return delegat.DontFlushFromFind;
    }

    public EventListeners getListeners() {
        return delegat.Listeners;
    }

    public IPersistenceContext getPersistenceContext() {
        return delegat.PersistenceContext;
    }

    public int executeUpdate(String query, QueryParameters queryParameters){
        return delegat.ExecuteUpdate(query, queryParameters);
    }

    public int executeNativeUpdate(NativeSQLQuerySpecification specification, QueryParameters queryParameters)  {
        return delegat.ExecuteNativeUpdate(specification, queryParameters);
    }

    public EntityMode getEntityMode() {
        return delegat.EntityMode;
    }

    public CacheMode getCacheMode() {
        return delegat.CacheMode;
    }

    public void setCacheMode(CacheMode cm) {
        delegat.CacheMode = cm;
    }

    public bool isOpen() {
        return delegat.IsOpen;
    }

    public bool isConnected() {
        return delegat.IsConnected;
    }

    public FlushMode getFlushMode() {
        return delegat.FlushMode;
    }

    public void setFlushMode(FlushMode fm) {
        delegat.FlushMode = fm ;
    }

    public IDbConnection connection() {
        return delegat.Connection; 
    }

    public void flush() {
        delegat.Flush();
    }

    public IQuery getNamedQuery(String name) {
        return delegat.GetNamedQuery(name);
    }

    public IQuery getNamedSQLQuery(String name) {
        return delegat.GetNamedSQLQuery(name);
    }

    public bool isEventSource() {
        return delegat.IsEventSource;
    }
    // TODO in second implementation phase  
    //public void afterScrollOperation() {
    //    delegate.afterScrollOperation();
    //}

    public void setFetchProfile(String name) {
        delegat.FetchProfile = name;
    }

    public String getFetchProfile() {
        return delegat.FetchProfile;
    }

    //public JDBCContext getJDBCContext() {
    //    return delegate.getJDBCContext();
    //}

    public bool isClosed() {
        return delegat.IsClosed;
    }

    #region ISessionImplementor Members

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public void InitializeCollection(IPersistentCollection collection, bool writing)
    {
        throw new NotImplementedException();
    }

    public object InternalLoad(string entityName, object id, bool eager, bool isNullable)
    {
        throw new NotImplementedException();
    }

    public object ImmediateLoad(string entityName, object id)
    {
        throw new NotImplementedException();
    }

    public long Timestamp
    {
        get { throw new NotImplementedException(); }
    }

    public ISessionFactoryImplementor Factory
    {
        get { throw new NotImplementedException(); }
    }

    public IBatcher Batcher
    {
        get { throw new NotImplementedException(); }
    }

    public IList List(string query, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public void List(string query, QueryParameters parameters, IList results)
    {
        throw new NotImplementedException();
    }

    public IList<T> List<T>(string query, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public IList<T> List<T>(CriteriaImpl criteria)
    {
        throw new NotImplementedException();
    }

    public void List(CriteriaImpl criteria, IList results)
    {
        throw new NotImplementedException();
    }

    public IList List(CriteriaImpl criteria)
    {
        throw new NotImplementedException();
    }

    public IEnumerable Enumerable(string query, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Enumerable<T>(string query, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public IList ListFilter(object collection, string filter, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public IList<T> ListFilter<T>(object collection, string filter, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public IEnumerable EnumerableFilter(object collection, string filter, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> EnumerableFilter<T>(object collection, string filter, QueryParameters parameters)
    {
        throw new NotImplementedException();
    }

    public IEntityPersister GetEntityPersister(string entityName, object obj)
    {
        throw new NotImplementedException();
    }

    public void AfterTransactionBegin(ITransaction tx)
    {
        throw new NotImplementedException();
    }

    public void BeforeTransactionCompletion(ITransaction tx)
    {
        throw new NotImplementedException();
    }

    public void AfterTransactionCompletion(bool successful, ITransaction tx)
    {
        throw new NotImplementedException();
    }

    public object GetContextEntityIdentifier(object obj)
    {
        throw new NotImplementedException();
    }

    public object Instantiate(string entityName, object id)
    {
        throw new NotImplementedException();
    }

    public IList List(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public void List(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results)
    {
        throw new NotImplementedException();
    }

    public IList<T> List<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public void ListCustomQuery(ICustomQuery customQuery, QueryParameters queryParameters, IList results)
    {
        throw new NotImplementedException();
    }

    public IList<T> ListCustomQuery<T>(ICustomQuery customQuery, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public object GetFilterParameterValue(string filterParameterName)
    {
        throw new NotImplementedException();
    }

    public IType GetFilterParameterType(string filterParameterName)
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, IFilter> EnabledFilters
    {
        get { throw new NotImplementedException(); }
    }

    public IQuery GetNamedSQLQuery(string name)
    {
        throw new NotImplementedException();
    }

    public NHibernate.Hql.IQueryTranslator[] GetQueries(string query, bool scalar)
    {
        throw new NotImplementedException();
    }

    public IInterceptor Interceptor
    {
        get { throw new NotImplementedException(); }
    }

    public EventListeners Listeners
    {
        get { throw new NotImplementedException(); }
    }

    public int DontFlushFromFind
    {
        get { throw new NotImplementedException(); }
    }

    public NHibernate.AdoNet.ConnectionManager ConnectionManager
    {
        get { throw new NotImplementedException(); }
    }

    public bool IsEventSource
    {
        get { throw new NotImplementedException(); }
    }

    public object GetEntityUsingInterceptor(EntityKey key)
    {
        throw new NotImplementedException();
    }

    public IPersistenceContext PersistenceContext
    {
        get { throw new NotImplementedException(); }
    }

    public CacheMode CacheMode
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public bool IsOpen
    {
        get { throw new NotImplementedException(); }
    }

    public bool IsConnected
    {
        get { throw new NotImplementedException(); }
    }

    public FlushMode FlushMode
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public string FetchProfile
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public string BestGuessEntityName(object entity)
    {
        throw new NotImplementedException();
    }

    public string GuessEntityName(object entity)
    {
        throw new NotImplementedException();
    }

    public IDbConnection Connection
    {
        get { throw new NotImplementedException(); }
    }

    public IQuery GetNamedQuery(string queryName)
    {
        throw new NotImplementedException();
    }

    public bool IsClosed
    {
        get { throw new NotImplementedException(); }
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public bool TransactionInProgress
    {
        get { throw new NotImplementedException(); }
    }

    public EntityMode EntityMode
    {
        get { throw new NotImplementedException(); }
    }

    public int ExecuteNativeUpdate(NativeSQLQuerySpecification specification, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public int ExecuteUpdate(string query, QueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }

    public FutureCriteriaBatch FutureCriteriaBatch
    {
        get { throw new NotImplementedException(); }
    }

    public FutureQueryBatch FutureQueryBatch
    {
        get { throw new NotImplementedException(); }
    }

    public Guid SessionId
    {
        get { throw new NotImplementedException(); }
    }

    public NHibernate.Transaction.ITransactionContext TransactionContext
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public void CloseSessionFromDistributedTransaction()
    {
        throw new NotImplementedException();
    }

    #endregion
}

}
