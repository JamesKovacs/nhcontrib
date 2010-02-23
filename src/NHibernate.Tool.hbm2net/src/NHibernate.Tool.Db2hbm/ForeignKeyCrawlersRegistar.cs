using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public static class ForeignKeyCrawlersRegistar
    {
        static Dictionary<System.Type, IForeignKeyCrawlerFactory> factories = new Dictionary<System.Type,IForeignKeyCrawlerFactory>() ;
        public static void Register(IForeignKeyCrawlerFactory factory, System.Type dialectType)
        {
            factories[dialectType] = factory;
        }
        public static IForeignKeyCrawler GetForDialect(System.Type dialect)
        {
            IForeignKeyCrawlerFactory factory;
            factories.TryGetValue(dialect, out factory);
            if (null != factory)
                return factory.Create();
            throw new Exception("No ForeignKeyCrawler avaiable for dialect:" + dialect.Name);
        }
    }
}
