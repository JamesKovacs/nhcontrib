using System;
using System.Collections.Generic;
using System.Threading;
using Iesi.Collections;
using NHibernate.Burrow.Util.EntityBases;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.LearningTests {
    [TestFixture]
    public class HashCodeTest {
        [Test]
        public void GetHashCodeTest() {
            HashCodeClass1 hcc1 = new HashCodeClass1();
            HashCodeClass2 hcc2 = new HashCodeClass2();
            Assert.AreEqual(hcc2.GetHashCode(), hcc1.GetHashCode());
            ISet set = new HashedSet();
            set.Add(hcc1);
            Assert.IsTrue(set.Add(hcc2));
            Assert.AreEqual(2, set.Count);
            IDictionary<object, string> dict = new Dictionary<object, string>();
            dict.Add(hcc1, "hcc1");
            dict.Add(hcc2, "hcc2");
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("hcc1", dict[hcc1]);
            Assert.AreEqual("hcc2", dict[hcc2]);
        }

        [Test, Explicit]
        public void temp() {
            Console.WriteLine(2005%10);
        }

        [Test, Explicit]
        public void temp2() {
            Console.WriteLine(Convert.ToInt32(long.MaxValue));
        }

        [Test, Explicit]
        public void TestCalHashCode() {
            HashedSet set = new HashedSet();
            for (int i = 0; i < 3000000; i++) {
                Thread.Sleep(1);
                if (!set.Add(new HashCodeClass4()))
                    Assert.Fail("duplicated HashCode " + set.Count);
            }
        }

        [Test, Explicit]
        public void TestCalOldHashCode() {
            HashedSet set = new HashedSet();
            int conflict = 0;
            for (int i = 0; i < 30000; i++) {
                HashCodeClass4 o = new HashCodeClass4();
                if (!set.Add(o))
                    conflict++;
            }
            Assert.AreEqual(0, conflict);
        }

        [Test, Explicit]
        public void TestSpeed() {
            long startC = long.MaxValue - new HashCodeClass4().GetHashCode();
            Console.WriteLine(startC);
            Thread.Sleep(5000);
            long lastC = long.MaxValue - new HashCodeClass4().GetHashCode();
            Console.WriteLine(lastC);

            double speed = (startC - lastC)/5000;
            Console.WriteLine(speed);
            long sec = (long) lastC/(long) speed;
            Console.WriteLine(sec/1000/3600/24/365);
            Console.WriteLine(sec);
        }
    }

    public class HashCodeClass1 {
        public override int GetHashCode() {
            return 1;
        }
    }

    public class HashCodeClass2 {
        public override int GetHashCode() {
            return 1;
        }
    }

    public class HashCodeClass4 : ObjectWHashIdBase {}
}