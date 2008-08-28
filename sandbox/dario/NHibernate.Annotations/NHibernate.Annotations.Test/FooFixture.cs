using System.Persistence;
using NHibernate.Annotations.Cfg;
using Xunit;

namespace NHibernate.Annotations.Test
{
    public class FooFixture
    {
        [Fact]
        public void Test01()
        {
            var cfg = new AnnotationConfiguration();
            cfg.AddAnnotatedType<Foo>();

            using (ISessionFactory sf = cfg.BuildSessionFactory())
            {
				
            }

        }
    }

    [Entity]
    public class Foo
    {
        [Id]
        [GeneratedValue(Strategy = GenerationType.Identity)]
        private int id;

        [Column]
        private string name;

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
