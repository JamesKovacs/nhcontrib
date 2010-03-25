using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Tools.Query;

namespace NHibernate.Envers.Query
{
/**
 * A class for incrementaly building a HQL query.
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */
public interface IQueryBuilder {
    ////////TODO in second implementation phase
    //////private readonly String entityName;
    //////private readonly String alias;

    ///////**
    ////// * For use by alias generator (in case an alias is not provided by the user).
    ////// */
    //////private int aliasCounter;
    ///////**
    ////// * For use by parameter generator, in {@link Parameters}. This counter must be
    ////// * the same in all parameters and sub-queries of this query.
    ////// */
    //////private int paramCounter;
    ///////**
    ////// * Main "where" parameters for this query.
    ////// */
    //////private readonly Parameters rootParameters;

    ///////**
    ////// * A list of pairs (from entity name, alias name).
    ////// */
    //////private readonly List<KeyValuePair<String, String>> froms;
    ///////**
    ////// * A list of pairs (property name, order ascending?).
    ////// */
    //////private readonly List<KeyValuePair<String, Boolean>> orders;
    ///////**
    ////// * A list of complete projection definitions: either a sole property name, or a function(property name).
    ////// */
    //////private readonly IList<String> projections;

    ///////**
    ////// *
    ////// * @param entityName Main entity which should be selected.
    ////// * @param alias Alias of the entity
    ////// */
    //////public QueryBuilder(String entityName, String alias) {
    //////    this(entityName, alias, new MutableInteger(), new MutableInteger());
    //////}

    //////private QueryBuilder(String entityName, String alias, MutableInteger aliasCounter, MutableInteger paramCounter) {
    //////    this.entityName = entityName;
    //////    this.alias = alias;
    //////    this.aliasCounter = aliasCounter;
    //////    this.paramCounter = paramCounter;

    //////    rootParameters = new Parameters(alias, "and", paramCounter);

    //////    froms = new ArrayList<Pair<String, String>>();
    //////    orders = new ArrayList<Pair<String, Boolean>>();
    //////    projections = new ArrayList<String>();

    //////    addFrom(entityName, alias);
    //////}

    ///////**
    ////// * Add an entity from which to select.
    ////// * @param entityName Name of the entity from which to select.
    ////// * @param alias Alias of the entity. Should be different than all other aliases.
    ////// */
    //////public void addFrom(String entityName, String alias) {
    //////    froms.add(Pair.make(entityName, alias));
    //////}

    //////private String generateAlias() {
    //////    return "_e" + aliasCounter.getAndIncrease();
    //////}

    ///////**
    ////// * @return A sub-query builder for the same entity (with an auto-generated alias). The sub-query can
    ////// * be later used as a value of a parameter.
    ////// */
    //////public IQueryBuilder newSubQueryBuilder() {
    //////    return newSubQueryBuilder(entityName, generateAlias());
    //////}

    ///////**
    ////// * @param entityName Entity name, which will be the main entity for the sub-query.
    ////// * @param alias Alias of the entity, which can later be used in parameters.
    ////// * @return A sub-query builder for the given entity, with the given alias. The sub-query can
    ////// * be later used as a value of a parameter.
    ////// */
    //////public IQueryBuilder newSubQueryBuilder(String entityName, String alias) {
    //////    return new IQueryBuilder(entityName, alias, aliasCounter, paramCounter);
    //////}

    //////public Parameters getRootParameters() {
    //////    return rootParameters;
    //////}

    //////public void addOrder(String propertyName, boolean ascending) {
    //////    orders.add(Pair.make(propertyName, ascending));
    //////}

    //////public void addProjection(String function, String propertyName, boolean distinct) {
    //////    addProjection(function, propertyName, distinct, true);
    //////}

    //////public void addProjection(String function, String propertyName, boolean distinct, boolean addAlias) {
    //////    if (function == null) {
    //////        projections.add((distinct ? "distinct " : "") + (addAlias ? alias+ "." : "") + propertyName);
    //////    } else {
    //////        projections.add(function + "(" + (distinct ? "distinct " : "") + (addAlias ? alias + "." : "") + propertyName + ")");
    //////    }
    //////}

    /**
     * Builds the given query, appending results to the given string buffer, and adding all query parameter values
     * that are used to the map provided.
     * @param sb String builder to which the query will be appended.
     * @param queryParamValues Map to which name and values of parameters used in the query should be added.
     */
     void Build(StringBuilder sb, IDictionary<String, Object> queryParamValues);
    ////////TODO in second implementation phase
    //{
    //    sb.Append("select ");
    //    if (projections.size() > 0)
    //    {
    //        // all projections separated with commas
    //        StringTools.append(sb, projections.iterator(), ", ");
    //    }
    //    else
    //    {
    //        // all aliases separated with commas
    //        StringTools.append(sb, getAliasList().iterator(), ", ");
    //    }
    //    sb.append(" from ");
    //    // all from entities with aliases, separated with commas
    //    StringTools.append(sb, getFromList().iterator(), ", ");
    //    // where part - rootParameters
    //    if (!rootParameters.isEmpty())
    //    {
    //        sb.append(" where ");
    //        rootParameters.build(sb, queryParamValues);
    //    }
    //    // orders
    //    if (orders.size() > 0)
    //    {
    //        sb.append(" order by ");
    //        StringTools.append(sb, getOrderList().iterator(), ", ");
    //    }
    //}
    ////////TODO in second implementation phase
    //////private List<String> getAliasList() {
    //////    List<String> aliasList = new ArrayList<String>();
    //////    for (Pair<String, String> from : froms) {
    //////        aliasList.add(from.getSecond());
    //////    }

    //////    return aliasList;
    //////}

    //////private List<String> getFromList() {
    //////    List<String> fromList = new ArrayList<String>();
    //////    for (Pair<String, String> from : froms) {
    //////        fromList.add(from.getFirst() + " " + from.getSecond());
    //////    }

    //////    return fromList;
    //////}

    //////private List<String> getOrderList() {
    //////    List<String> orderList = new ArrayList<String>();
    //////    for (Pair<String, Boolean> order : orders) {
    //////        orderList.add(alias + "." + order.getFirst() + " " + (order.getSecond() ? "asc" : "desc"));
    //////    }

    //////    return orderList;
    //////}
}

}
