namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System;
	using System.Runtime.Serialization;
	using NHibernate;

	[Serializable]
	public class AstException : HibernateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AstException()
		{
		}

		public AstException(string message) : base(message)
		{
		}

		public AstException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AstException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}