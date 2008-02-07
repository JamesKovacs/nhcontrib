using System;
using System.Collections.Generic;
using NHibernate.Burrow.Test.PersistantTests;
using NHibernate.Burrow.TestUtil;
using NHibernate.Burrow.Util.AuditLog;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.AuditLogTest {
    [TestFixture]
    public class AuditLogTest : TestBase {
        [Test, Ignore("AuditLog function is temporary disabled")]
        public void AuditTest() {
            string updatedName = "new name";
            int mockUserId = 12;
            int oldCount = AuditLogRecordDAO.Instance.CountAll();
            MockPersistantClass mpc = new MockPersistantClass();
            mpc.Save();
            CommitAndClearSession();

            mpc = MockPersistantClassDAO.Instance.FindById(mpc.Id);
            BooDomainSession.Current.UserId = mockUserId;
            mpc.Name = updatedName;

            CommitAndClearSession();

            mpc = MockPersistantClassDAO.Instance.FindById(mpc.Id);
            mpc.Delete();
            CommitAndClearSession();

            //note that the search result will be returned in time decendent.
            IList<AuditLogRecord> result = AuditLogRecordDAO.Instance.Find(typeof (MockPersistantClass), mpc.Id);
            foreach (AuditLogRecord record in result)
                Console.WriteLine(record.Action + " " + record.EntityId + " " + record.PropertyName + " " +
                                  record.NewValue);
            //we can't predict how many more records were created because we don't know how many fields were updated during insert-update
            Assert.IsTrue(result.Count >= 3);

            int newCount = AuditLogRecordDAO.Instance.CountAll();
            Assert.IsTrue(newCount - oldCount >= 3);

            result = AuditLogRecordDAO.Instance.Find(typeof (MockPersistantClass), mpc.Id, Actions.UPDATE);
            Assert.AreEqual(1, result.Count);
            AuditLogRecord algUpdate = result[0];
            Assert.AreEqual(string.Empty, algUpdate.OldValue);
            Assert.AreEqual(updatedName, algUpdate.NewValue);
            Assert.AreEqual("Name", algUpdate.PropertyName);
            Assert.AreEqual(Actions.UPDATE, algUpdate.Action);
            Assert.AreEqual(mockUserId.ToString(), algUpdate.UserId);

            result = AuditLogRecordDAO.Instance.Find(typeof (MockPersistantClass), mpc.Id, Actions.INSERT);
            Assert.AreEqual(1, result.Count);

            result = AuditLogRecordDAO.Instance.Find(typeof (MockPersistantClass), mpc.Id, Actions.DELETE);
            Assert.AreEqual(1, result.Count);
        }
    }
}