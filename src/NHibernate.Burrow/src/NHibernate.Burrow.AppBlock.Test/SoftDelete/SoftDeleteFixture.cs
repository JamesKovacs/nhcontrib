using System;
using System.Collections;
using NHibernate.Event;
using NUnit.Framework;
using NHibernate.Burrow.AppBlock.SoftDelete;

namespace NHibernate.Burrow.AppBlock.Test.SoftDelete
{
    [TestFixture]
    public class SoftDeleteFixture : TestCase
    {
        protected override IList Mappings
        {
            get { return new string[] { "SoftDelete.SoftDeleter.hbm.xml", "SoftDelete.RandomEntity.hbm.xml" }; }
        }

        [Test]
        public void NormalEntityShouldActuallyBeDeleted()
		{
			IDeleteEventListener[] defaultListeners = cfg.EventListeners.DeleteEventListeners;
            cfg.EventListeners.DeleteEventListeners = new IDeleteEventListener[] {new SoftDeleteEventListener()};
            using (ISession session = OpenSession())
            {
                int id;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    RandomEntity ne = new RandomEntity();
                    ne.Name = "Bob";
                    session.Save(ne);
                    id = ne.Id;
                    transaction.Commit();
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    RandomEntity ne = session.Get<RandomEntity>(id);
                    session.Delete(ne);
                    transaction.Commit();
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    RandomEntity ne = session.Get<RandomEntity>(id);
                    Assert.IsNull(ne);
                    transaction.Commit();
                }
            }
			cfg.EventListeners.DeleteEventListeners = defaultListeners;
        }

        [Test]
        public void DeletedSoftDeleteEntitiesAreNotReallyDeleted()
        {
            int id;
            IDeleteEventListener[] defaultListeners = cfg.EventListeners.DeleteEventListeners;

            cfg.EventListeners.DeleteEventListeners = new IDeleteEventListener[] { new SoftDeleteEventListener() };
            using (ISession session = OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SoftDeleter sd = new SoftDeleter();
                    sd.DeleteDate = DateTime.Parse("1/1/07 1:00:00 pm");
                    session.Save(sd);
                    id = sd.Id;
                    transaction.Commit();
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SoftDeleter sd = session.Get<SoftDeleter>(id);
                    session.Delete(sd);
                    transaction.Commit();
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SoftDeleter sd = session.Get<SoftDeleter>(id);
                    Assert.IsTrue(sd.Deleted);
                    Assert.IsNotNull(sd.DeleteDate);
                    Assert.AreNotEqual(DateTime.Parse("1/1/07 1:00:00 pm"), sd.DeleteDate.Value);
                    transaction.Commit();
                }
                session.Clear();
            }

            cfg.EventListeners.DeleteEventListeners = defaultListeners;
            using (ISession session = OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    SoftDeleter sd = session.Get<SoftDeleter>(id);
                    session.Delete(sd);
                   // session.Flush();
                    transaction.Commit();
                }
            }
        }
    }
}
