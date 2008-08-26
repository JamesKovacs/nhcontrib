using System.Collections.Generic;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Annotations.Util;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
	public abstract class FkSecondPass : ISecondPass
	{
		protected SimpleValue value;
		protected Ejb3JoinColumn[] columns;
		/**
		 * TODO: adapt this comment if it's needed or deleted it otherwise.
		 * unique counter is needed to differenciate 2 instances of FKSecondPass
		 * as they are compared.
		 * Fairly hacky but IBM VM sometimes returns the same hashCode for 2 different objects
		 * TODO is it doable to rely on the Ejb3JoinColumn names? Not sure at they could be inferred
		 */
		private int uniqueCounter;
		private static AtomicInteger globalCounter = new AtomicInteger();

		public FkSecondPass(ToOne value, Ejb3JoinColumn[] columns)
		{
			this.value = value;
			this.columns = columns;
			uniqueCounter = globalCounter.GetAndIncrement();
		}

		public SimpleValue Value
		{
			get { return value; }
		}

		public int UniqueCounter
		{
			get { return uniqueCounter; }
		}
        
		public override bool Equals(object o)
		{
			if (this == o) return true;
			if (!(o is FkSecondPass)) return false;

			var that = (FkSecondPass) o;

			if (uniqueCounter != that.uniqueCounter) return false;

			return true;
		}

		public override int GetHashCode()
		{
			return uniqueCounter;
		}

		public abstract void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses);
		
		public abstract string ReferencedEntityName { get; }

		public abstract bool IsInPrimaryKey { get; }
	}
}