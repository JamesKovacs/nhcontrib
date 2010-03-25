using System;
using NHibernate.Engine;

namespace NHibernate.Impl
{
	using System.Web;

	public class CurrentSessionIdLoggingContext : IDisposable
	{
		[ThreadStatic] private static Guid? CurrentSessionId;

		private const string CurrentSessionIdKey = "NHibernate.Impl.CurrentSessionIdLoggingContext.CurrentSessionId";

		private readonly Guid? oldSessonId;

		public CurrentSessionIdLoggingContext(Guid id)
		{
			oldSessonId = SessionId;
			SessionId = id;
		}

		/// <summary>
		/// Error handling in this case will only kick in if we cannot set values on the TLS
		/// this is usally the case if we are called from the finalizer, since this is something
		/// that we do only for logging, we ignore the error.
		/// </summary>
		public static Guid? SessionId
		{
			get
			{
				if (HttpContext.Current != null)
					return (Guid?)HttpContext.Current.Items[CurrentSessionIdKey];
				return CurrentSessionId;
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Items[CurrentSessionIdKey] = value;
				else
					CurrentSessionId = value;
			}
		}

		public void Dispose()
		{
			SessionId = oldSessonId;
		}

		private static CurrentSessionIdLoggingContext GetCurrentSessionIdContext(ISessionFactory factory)
		{
			var factoryImpl = factory as ISessionFactoryImplementor;

			if (factoryImpl == null)
			{
				throw new HibernateException("Session factory does not implement ISessionFactoryImplementor.");
			}

			if (factoryImpl.CurrentSessionContext == null)
			{
				throw new HibernateException("No current sessionID context configured.");
			}

			var currentSessionContext = factoryImpl.CurrentSessionContext as CurrentSessionIdLoggingContext;
			if (currentSessionContext == null)
			{
				throw new HibernateException("Current session context does not extend class CurrentSessionIdLoggingContext.");
			}

			return currentSessionContext;
		}
	}
}