using System.Collections;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.UserTypes
{
    [TestFixture]
    public class UpperStringFixture : TestCase
    {
        protected override IList Mappings
        {
            get { return new string[] {"UserTypes.UpperStringMappings.hbm.xml"}; }
        }

        protected override void OnTearDown()
        {
            using (ISession s = OpenSession())
            {
                s.Delete("from Foo");
                s.Flush();
            }
        }

        [Test]
        public void SaveNullPropertyAndGetItBack()
        {
            Foo f = new Foo(2, "Pat Metheny", null);

            using (ISession s = OpenSession())
            {
                s.Save(f);
                s.Flush();
            }

            using (ISession s = OpenSession())
            {
                Foo upperFoo = s.Get<Foo>(2);

                Assert.AreEqual(2, upperFoo.Id);
                Assert.AreEqual("Pat Metheny", upperFoo.Name);
                Assert.IsNull(upperFoo.Description);
            }
        }

        [Test]
        public void SaveObjectWithStringChangedToUpper()
        {
            Foo f = new Foo(1, "Astor Piazolla", "tango");

            using (ISession s = OpenSession())
            {
                s.Save(f);
                s.Flush();
            }

            using (ISession s = OpenSession())
            {
                Foo upperFoo = s.Get<Foo>(1);

                Assert.AreEqual(1, upperFoo.Id);
                Assert.AreEqual("Astor Piazolla", upperFoo.Name);
                Assert.AreEqual("TANGO", upperFoo.Description);
            }
        }

        //Not suported: The attribute owner was removed for IUserType interfaz
        //and in this case was useful.
        //[Test]
        //public void SaveObject2()
        //{
        //    using (ISession s = OpenSession())
        //    {
        //        Foo f = new Foo(3, "Astor Piazolla", "tango");
        //        s.Save(f);
        //        s.Flush();

        //        Assert.AreEqual(3, f.Id);
        //        Assert.AreEqual("Astor Piazolla", f.Name);
        //        Assert.AreEqual("TANGO", f.Description);
        //    }
        //}
    }
}