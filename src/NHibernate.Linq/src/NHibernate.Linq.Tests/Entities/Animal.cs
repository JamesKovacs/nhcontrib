using System;

namespace NHibernate.Linq.Tests.Entities
{
	public class Animal
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual double BodyWeight { get; set; }
		public virtual Animal Mother { get; set; }
		public virtual Animal Father { get; set; }
		public virtual string SerialNumber { get; set; }
	}

	public abstract class Reptile : Animal
	{
		public virtual double BodyTemperature { get; set; }
	}

	public class Lizard : Reptile { }

	public class Mammal : Animal
	{
		public virtual bool Pregnant { get; set; }
		public virtual DateTime? BirthDate { get; set; }
	}
}
