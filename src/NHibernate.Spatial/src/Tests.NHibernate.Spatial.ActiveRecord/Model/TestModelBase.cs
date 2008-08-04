using System;
using Castle.ActiveRecord;

namespace NHibernate_Spatial_Tests.Model
{
	public abstract class TestModelBase<T> : ActiveRecordBase<T>
	{
		private Int64 _Id;

		[PrimaryKey(Generator = PrimaryKeyType.Native)]
		public virtual Int64 Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		public override int GetHashCode()
		{
			return (int)((_Id % 32) ^ (_Id >> 32));
		}

		public override bool Equals(object obj)
		{
			TestModelBase<T> target = obj as TestModelBase<T>;
			if (target == null)
				return false;
			else
				return target.Id == this.Id;
		}

	}
}
