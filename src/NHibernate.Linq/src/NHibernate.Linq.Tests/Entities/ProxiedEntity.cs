namespace NHibernate.Linq.Tests.Entities
{
	using System;
	using System.Collections.Generic;

	public interface IProxiedEntity
	{
		int Id { get; }

		string Dummy { get; set; }
	}

	public class ProxiedEntity : IProxiedEntity
	{
		public int Id { get; protected set; }

		public string Dummy { get; set; }

		public IProxiedEntityChild Child { get; set; }
	}

	public class AnotherProxiedEntity : IProxiedEntity
	{
		public int Id { get; protected set; }

		public string Dummy { get; set; }
	}

	public interface IProxiedEntityChild
	{
		int Id { get; }

		ICollection<AnotherChild> Children { get; set; }
	}

	public class ProxiedEntityChild : IProxiedEntityChild
	{
		public int Id { get; protected set; }

		public ICollection<AnotherChild> Children
		{
			get; set;
		}
	}

	public class AnotherChild
	{
		public virtual int Id { get; protected set; }

		public virtual string Foo { get; set; }
	}
}
