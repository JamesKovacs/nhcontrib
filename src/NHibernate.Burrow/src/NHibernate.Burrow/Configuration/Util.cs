using System.Collections;
using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    internal class Util {
        public static IDictionary CopyToDict(KeyValueConfigurationCollection settings) {
            IDictionary dict = new Hashtable(settings.Count);
            foreach (KeyValueConfigurationElement setting in settings) dict.Add(setting.Key, setting.Value);
            return dict;
        }

    }
}