using NHibernate.Envers;

namespace Envers.NET.Tests.NH3.Integration.AccessType
{
    public class FieldAccessEntity
    {
        private int id;
        [Audited]
        private string data;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }

        public virtual string Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    public class PropertyAccessEntity
    {
        public virtual int Id { get; set; }
        [Audited]
        public virtual string Data { get; set; }
    }
}