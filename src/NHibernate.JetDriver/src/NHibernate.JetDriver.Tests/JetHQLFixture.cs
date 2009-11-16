using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.JetDriver.Tests.Entities;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetHQLFixture : JetTestBase
    {
        public JetHQLFixture() : base(true)
        {
        }

        protected override IList<System.Type> EntityTypes
        {
            get { return new[] { typeof (Foo)}; }
        }

        [Test]
        public void Can_Delete_Objects_With_HQL()
        {
            object savedId;
            using (var s = SessionFactory.OpenSession())
            using (var t = s.BeginTransaction())
            {
                savedId = s.Save(new Foo
                {
                    ModuleId = -1,
                    Module = "bar"
                });

                t.Commit();
            }

            using (var s = SessionFactory.OpenSession())
            using (var t = s.BeginTransaction())
            {
                var m = s.Get<Foo>(savedId);
                m.Module = "bar2";
                t.Commit();
            }

            //delete using a HQL statement
            using (var s = SessionFactory.OpenSession())
            using (var t = s.BeginTransaction())
            {
                var m = s.Get<Foo>(savedId);

                s.Delete("from Foo foo where foo.ModuleId = ?",
                         m.ModuleId, NHibernateUtil.Int32);

                t.Commit();
            }

            using (var s = SessionFactory.OpenSession())
            using (var t = s.BeginTransaction())
            {
                s.CreateQuery("delete from Foo").ExecuteUpdate();
                t.Commit();
            }
        }
    }
}