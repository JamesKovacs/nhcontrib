namespace NHibernate.Envers.Tests.Integration.AccessType
{
    [Audited]
    public class Country
    {
        private readonly int _code;
        private readonly string _name;

        protected Country()
        {
        }

        public Country(int code, string name)
        {
            _code = code;
            _name = name;
        }

        public virtual int Code
        {
            get { return _code; }
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Country;
            if (casted == null)
                return false;
                       
            return casted.Code==Code && casted._name.Equals(_name);
        }

        public override int GetHashCode()
        {
            return Code ^ _name.GetHashCode();
        }
    }
}