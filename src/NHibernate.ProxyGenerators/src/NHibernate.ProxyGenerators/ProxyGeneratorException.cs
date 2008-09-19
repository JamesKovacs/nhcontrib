namespace NHibernate.ProxyGenerators
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class ProxyGeneratorException : Exception
	{
		public ProxyGeneratorException()
		{
		}

		public ProxyGeneratorException(string message)
			: base(message)
		{
		}

		public ProxyGeneratorException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
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
