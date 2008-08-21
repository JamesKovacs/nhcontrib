using System;
using System.Runtime.Serialization;
using NHibernate.Annotations;

namespace NHibernate.Cfg
{
    public class RecoverableException : AnnotationException
    {
        public RecoverableException(string message) : base(message)
        {
        }

        public RecoverableException(Exception innerException) : base(innerException)
        {
        }

        public RecoverableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecoverableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}