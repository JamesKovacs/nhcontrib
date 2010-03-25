using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Tools.Query;
using NHibernate.Envers.Entities.Mapper.Id;

/**
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */
namespace NHibernate.Envers.Entities.Mapper.Id
{
    public abstract class AbstractIdMapper:  IIdMapper
    {
        private Parameters getParametersToUse(Parameters parameters, IList<QueryParameterData> paramDatas)
        {
            if (paramDatas.Count > 1)
            {
                return parameters.addSubParameters("and");
            }
            else
            {
                return parameters;
            }
        }

        public void AddIdsEqualToQuery(Parameters parameters, String prefix1, String prefix2) {
        IList<QueryParameterData> paramDatas = MapToQueryParametersFromId(null); 

        Parameters parametersToUse = getParametersToUse(parameters, paramDatas);

        foreach (QueryParameterData paramData in paramDatas) {
            parametersToUse.addWhere(paramData.getProperty(prefix1), false, "=", paramData.getProperty(prefix2), false);
        }
    }

        public void AddIdsEqualToQuery(Parameters parameters, String prefix1, IIdMapper mapper2, String prefix2)
        {
            IList<QueryParameterData> paramDatas1 = MapToQueryParametersFromId(null);
            IList<QueryParameterData> paramDatas2 = mapper2.MapToQueryParametersFromId(null); 

            Parameters parametersToUse = getParametersToUse(parameters, paramDatas1);

            IEnumerator<QueryParameterData> paramDataIter1 = paramDatas1.GetEnumerator();
            IEnumerator<QueryParameterData> paramDataIter2 = paramDatas2.GetEnumerator();
            while (paramDataIter1.MoveNext())
            {
                QueryParameterData paramData1 = paramDataIter1.Current;
                QueryParameterData paramData2 = paramDataIter2.Current;

                parametersToUse.addWhere(paramData1.getProperty(prefix1), false, "=", paramData2.getProperty(prefix2), false);
            }
        }

        public void AddIdEqualsToQuery(Parameters parameters, Object id, String prefix, bool equals) {
        IList<QueryParameterData> paramDatas = MapToQueryParametersFromId(id); 

        Parameters parametersToUse = getParametersToUse(parameters, paramDatas);

        foreach (QueryParameterData paramData in paramDatas) {
            parametersToUse.addWhereWithParam(paramData.getProperty(prefix) , equals ? "=" : "<>", paramData.getValue());
        }
    }

        public void AddNamedIdEqualsToQuery(Parameters parameters, String prefix, bool equals) {
        IList<QueryParameterData> paramDatas = MapToQueryParametersFromId(null); 

        Parameters parametersToUse = getParametersToUse(parameters, paramDatas);

        foreach (QueryParameterData paramData in paramDatas) {
            parametersToUse.addWhereWithNamedParam(paramData.getProperty(prefix), equals ? "=" : "<>", paramData.GetQueryParameterName());
        }
    }

        public abstract IList<QueryParameterData> MapToQueryParametersFromId(Object obj);

        #region IIdMapper Members

        public abstract void MapToMapFromId(IDictionary<string, object> data, object obj);

        public abstract void MapToMapFromEntity(IDictionary<string, object> data, object obj);

        public abstract void MapToEntityFromMap(object obj, IDictionary<string, object> data);

        public abstract object MapToIdFromEntity(object data);

        public abstract object MapToIdFromMap(IDictionary<string, object> data);

        public abstract IIdMapper PrefixMappedProperties(string prefix);

        #endregion
    }
}
