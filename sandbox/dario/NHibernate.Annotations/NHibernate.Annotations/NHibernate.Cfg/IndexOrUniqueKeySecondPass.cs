using System.Collections.Generic;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
    public class IndexOrUniqueKeySecondPass
    {
        public IndexOrUniqueKeySecondPass(string indexName, Ejb3Column ejb3Column, ExtendedMappings mappings, bool b)
        {
            
        }

        public void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses)
        {
        }
    }
}