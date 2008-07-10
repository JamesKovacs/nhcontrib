using System;

namespace NHibernate.Linq
{
	/// <summary>
	/// Wraps an <see cref="T:NHibernate.ISession"/> object to provide base functionality
	/// for custom, database-specific context classes.
	/// </summary>
	public abstract class NHibernateContext : IDisposable, ICloneable
	{
		/// <summary>
		/// Provides access to database provider specific methods.
		/// </summary>
		public readonly IDbMethods Methods;

		private ISession session;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NHibernate.Linq.NHibernateContext"/> class.
		/// </summary>
		/// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
		public NHibernateContext(ISession session)
		{
			this.session = session;
		}

		/// <summary>
		/// Gets a reference to the <see cref="T:NHibernate.ISession"/> associated with this object.
		/// </summary>
		public virtual ISession Session
		{
			get { return session; }
		}

		#region ICloneable Members

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		public virtual object Clone()
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}

			return Activator.CreateInstance(GetType(), session);
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes the wrapped <see cref="T:NHibernate.ISession"/> object.
		/// </summary>
		public virtual void Dispose()
		{
			if (session != null)
			{
				session.Dispose();
				session = null;
			}
		}

		#endregion
	}
}