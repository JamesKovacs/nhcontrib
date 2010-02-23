using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    [global::System.Serializable]
    public class ConfigurationValidationException : Exception
    {
        public ConfigurationValidationException() { }
        public ConfigurationValidationException(string message) : base(message) { }
        public ConfigurationValidationException(string message, Exception inner) : base(message, inner) { }
        protected ConfigurationValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
