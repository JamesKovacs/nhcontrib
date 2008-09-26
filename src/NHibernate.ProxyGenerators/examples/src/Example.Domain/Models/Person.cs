namespace Example.ActiveRecordDomain.Models
{
	using System.Collections.Generic;
	using Castle.ActiveRecord;

	[ActiveRecord]
	public class Person : ActiveRecordBase<Person>
	{
		private int _id;
		private string _name;
		private IList<PhoneNumber> _phoneNumbers = new List<PhoneNumber>();

		[PrimaryKey]
		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[Property]
		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[HasMany]
		public virtual IList<PhoneNumber> PhoneNumbers
		{
			get { return _phoneNumbers; }
			set { _phoneNumbers = value; }
		}
	}
}
