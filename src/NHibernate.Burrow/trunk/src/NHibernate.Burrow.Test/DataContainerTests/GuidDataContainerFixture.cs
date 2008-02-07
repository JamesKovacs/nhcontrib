using System;
using NHibernate.Burrow.NHDomain;
using NUnit.Framework;
using NHibernate.Burrow.DataContainers;
namespace NHibernate.Burrow.Test.DataContainerTests {
    [TestFixture]
    public class GuidDataContainerFixture  {
       static readonly GuidDataContainer c = new GuidDataContainer();

        private Guid fieldId = Guid.Empty;
        public string ConversationalString {
            get {
                if(fieldId == Guid.Empty)
                    return default(string);
                return c.Get<String>(fieldId);
            }set {
                if (fieldId == Guid.Empty)
                    fieldId = c.CreateSlot(value);
                else 
                   c.Set(fieldId, value);
            }
        }


        [Test]
        public void NullTest() {
         
            Guid gid =    c.CreateSlot(null);
            Assert.IsNull(c.Get(gid));

        }

        [Test]
        public void PropertyTest() {
            string value = "TEst String Value";
            Assert.IsNull( ConversationalString );

            ConversationalString = value;
            Assert.AreEqual(value, ConversationalString);
            value = "TEstadfadfasdf";
            ConversationalString = value;
            Assert.AreEqual(value, ConversationalString);

            ConversationalString = null;
            Assert.IsNull(ConversationalString);
        }


    }
}