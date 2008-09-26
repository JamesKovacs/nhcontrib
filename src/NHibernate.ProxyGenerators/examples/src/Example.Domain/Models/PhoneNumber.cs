namespace Example.Domain.Models
{
	public class PhoneNumber
	{
		private int _id;
		private Person _person;
		private string _number;
		private string _phoneType;

		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public virtual Person Person
		{
			get { return _person; }
			set { _person = value; }
		}

		public virtual string Number
		{
			get { return _number; }
			set { _number = value; }
		}

		public virtual string PhoneType
		{
			get { return _phoneType; }
			set { _phoneType = value; }
		}
	}
}
