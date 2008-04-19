using System;
using NHibernate.Burrow.TestUtil.Exceptions;
using NHibernate.Burrow.Util;

namespace NHibernate.Burrow.TestUtil.Attributes
{
    /// <summary>
    /// a persistant field attribute that indicates that the field will be persisted by UserControlBase
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataProvider : Attribute
    {
        public string dataProviderName;

        public DataProvider() {}

        public DataProvider(string dataProviderName)
        {
            this.dataProviderName = dataProviderName;
        }

        public DataProviderBase CreateDataProvider()
        {
            System.Type type = System.Type.GetType(dataProviderName);
            if (type == null)
            {
                throw new TestUtilException("DataProvider Type " + dataProviderName + " not found. ");
            }
            return (DataProviderBase) InstanceLoader.Load(type);
        }
    }
}