using System;

namespace NHibernate.Impl
{
	public interface ISessionIdLoggingContext
	{
		Guid? SessionId { get; set; }
	}
}