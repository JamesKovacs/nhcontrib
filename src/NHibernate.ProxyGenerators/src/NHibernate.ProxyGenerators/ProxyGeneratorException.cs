namespace NHibernate.ProxyGenerators
{
	using System;
	using System.Runtime.Serialization;

	public class ProxyGeneratorException : Exception
	{
		public ProxyGeneratorException()
		{
		}

		public ProxyGeneratorException(string message)
			: base(message)
		{
		}

		protected ProxyGeneratorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public ProxyGeneratorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
