using System;
using System.Collections.Generic;
using NHibernate.Burrow;
using NHibernate.Burrow.Util;
using NHibernate.Burrow.Util.EntityBases;
using NUnit.Framework;

namespace NHibernate.Burrow.TestUtil {
    [TestFixture]
    public class DataProviderBase {
        private readonly LinkedList<object> createdData = new LinkedList<object>();

        public readonly Random Random = new Random(19);

        public static string RandomName() {
            return RandomString(5);
        }

        protected static string RandomEmail() {
            return RandomName() + "@" + RandomName() + ".com";
        }

        public static string RandomString(int length) {
            return RandomStringGenerator.GenerateLetterStrings(length);
        }

        public static int RandomInt() {
            return Math.Abs(RandomStringGenerator.GenerateLetterStrings(10).GetHashCode()/100);
        }

        protected void AddCreatedData(object o) {
            createdData.AddFirst(o);
        }

        /// <summary>
        /// Delete the test data in a FIFO fashion
        /// </summary>
        public void ClearData() {
            LinkedListNode<object> node = createdData.First;
            while (node != null) {
                object o = node.Value;
                DeleteEntity(o);
                node = node.Next;
            }
            createdData.Clear();
        }

        public static void DeleteEntity(object o) {
            if (o == null) return;
            if (o is IPersistentObjWithDAO)
                DeletePersistenceObject((IPersistentObjWithDAO) o);
            else if (o is IPersistentObjSaveDelete)
                DeletePersistenceObject((IPersistentObjSaveDelete) o);
            else
                DeletePersistenceObject(o);
        }

        protected static void DeletePersistenceObject(IPersistentObjWithDAO o) {
            o = (IPersistentObjWithDAO) ReloadIWithId(o);
            if (o != null)
                o.DAO.Delete();
        }

        private static object ReloadIWithId(IWithId o) {
            return GetSession(o.GetType()).Get(o.GetType(), o.Id);
        }

        private static ISession GetSession(System.Type t) {
            return SessionManager.GetInstance(t).GetSession();
        }

        protected static void DeletePersistenceObject(IPersistentObjSaveDelete o) {
            o = (IPersistentObjSaveDelete) ReloadIWithId(o);
            if (o != null)
                o.Delete();
        }

        protected static void DeletePersistenceObject(object o) {
            o = GetSession(o.GetType()).Get(o.GetType(), GetEntityId(o));
            if (o != null)
                GetSession(o.GetType()).Delete(o);
        }

        private static object GetEntityId(object entity) {
            return EntityLoader.Instance.GetId(entity);
        }
    }
}