using System;
using System.Runtime.Serialization;

namespace NHibernate.Annotations
{
    public class AnnotationException : MappingException
    {
        public AnnotationException(string message) : base(message)
        {
        }

        public AnnotationException(Exception innerException) : base(innerException)
        {
        }

        public AnnotationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AnnotationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}