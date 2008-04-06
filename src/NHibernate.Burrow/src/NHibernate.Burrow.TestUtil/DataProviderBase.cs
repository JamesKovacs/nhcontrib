using System;
using System.Collections.Generic;
using NHibernate.Burrow;
using NHibernate.Burrow.Util.DAOBases;
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
            o = GetSession(o.GetType()).Get(o.GetType(), GetEntityId(o));
            if (o != null)
            {
                if (o is IDeletable)
                    ((IDeletable)o).Delete();
                else
                    GetSession(o.GetType()).Delete(o);
            }
        }
 
        private static ISession GetSession(System.Type t) {
            return new Facade().GetSession(t);
        }
 
        protected static void DeletePersistenceObject(object o) {
           
        }

        private static object GetEntityId(object entity) {
            return EntityLoader.Instance.GetId(entity);
        }
    }
}