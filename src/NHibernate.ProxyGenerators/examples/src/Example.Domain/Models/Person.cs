namespace Example.Domain.Models
{
	using System.Collections.Generic;

	public class Person
	{
		private int _id;
		private string _name;
		private IList<PhoneNumber> _phoneNumbers = new List<PhoneNumber>();

		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public virtual IList<PhoneNumber> PhoneNumbers
		{
			get { return _phoneNumbers; }
			set { _phoneNumbers = value; }
		}
	}
}
