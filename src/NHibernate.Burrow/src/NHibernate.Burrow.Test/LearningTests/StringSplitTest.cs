using NUnit.Framework;

namespace NHibernate.Burrow.Test.LearningTests {
    [TestFixture]
    public class StringSplitTest {
        [Test]
        public void StringsplitTest() {
            string test = "midn";
            Assert.AreEqual(test, test.Split(new char[] {',', ' ', ';'})[0]);
        }
    }
}