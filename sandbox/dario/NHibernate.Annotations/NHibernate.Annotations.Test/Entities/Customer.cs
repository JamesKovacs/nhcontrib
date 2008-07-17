using System.Persistence;

namespace NHibernate.Annotations.Test.Entities
{
    [Entity]
    public class Customer
    {
        [Id] 
        [GeneratedValue(Strategy = GenerationType.Identity)]
        private int id;

        [Column(Name = "FullName")]
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
    }
}