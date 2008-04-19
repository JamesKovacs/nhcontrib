using System;
using NHibernate.Burrow.AppBlock.Transform;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.Transform {
    [TestFixture]
    public class PosToBeanResultTransformer {
        private class ASimplePOCO {
            private int _intProp;

            private string _stringProp;

            public int IntProp {
                get { return _intProp; }
                set { _intProp = value; }
            }

            public string StringProp {
                get { return _stringProp; }
                set { _stringProp = value; }
            }

            public string NoSetterProp {
                get { return _stringProp; }
            }
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorInvalidAliases() {
            PositionalToBeanResultTransformer t =
                new PositionalToBeanResultTransformer(typeof (ASimplePOCO), new string[] {});
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorInvalidType() {
            PositionalToBeanResultTransformer t = new PositionalToBeanResultTransformer(null, new string[] {"a", "b"});
        }

        [Test]
        public void Setters() {
            string[] aliases = new string[] {"_intProp", "_stringProp"};
            string[] propAliases = new string[] {"IntProp", "StringProp"};

            // Test with field
            PositionalToBeanResultTransformer t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), aliases);
            ASimplePOCO asp = (ASimplePOCO) t.TransformTuple(new object[] {1, "test"}, aliases);
            Assert.AreEqual(1, asp.IntProp);
            Assert.AreEqual("test", asp.StringProp);

            // Test with properties
            t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), propAliases);
            asp = (ASimplePOCO) t.TransformTuple(new object[] {1, "test"}, propAliases);
            Assert.AreEqual(1, asp.IntProp);
            Assert.AreEqual("test", asp.StringProp);
        }

        [Test, ExpectedException(typeof (HibernateException))]
        public void TupleDifferentScalars() {
            string[] aliases = new string[] {"_intProp", "_stringProp"};
            PositionalToBeanResultTransformer t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), aliases);
            ASimplePOCO asp = (ASimplePOCO) t.TransformTuple(new object[] {1}, aliases);
        }
    }
}