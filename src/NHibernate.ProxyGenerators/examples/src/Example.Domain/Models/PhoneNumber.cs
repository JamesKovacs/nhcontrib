namespace Example.ActiveRecordDomain.Models
{
	using Castle.ActiveRecord;

	[ActiveRecord]
	public class PhoneNumber : ActiveRecordBase<PhoneNumber>
	{
		private int _id;
		private Person _person;
		private string _number;
		private string _phoneType;

		[PrimaryKey]
		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[BelongsTo]
		public virtual Person Person
		{
			get { return _person; }
			set { _person = value; }
		}

		[Property]
		public virtual string Number
		{
			get { return _number; }
			set { _number = value; }
		}

		[Property]
		public virtual string PhoneType
		{
			get { return _phoneType; }
			set { _phoneType = value; }
		}
	}
}
