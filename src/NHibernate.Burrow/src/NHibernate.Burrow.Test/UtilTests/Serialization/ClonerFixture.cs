using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Burrow.Util.Serialization;

namespace NHibernate.Burrow.Test.Serialization
{
    [TestFixture]
    public class SerializeFixture
    {
        [Test]
        public void Clone()
        {
            Foo f = new Foo();
            f.Id = 1;
            f.Name = "f";
            f.Price = 2.0m;
            f.nonSerializablefield = "nonserializable";
            f.Children.Add(new FooChild(1));
            f.Children.Add(new FooChild(2));


            Foo clone = (Foo)Cloner.Clone(f);

            Assert.AreEqual(1, clone.Id);
            Assert.AreEqual("f", clone.Name);
            Assert.AreEqual(2.0m, clone.Price);
            Assert.IsNull(clone.nonSerializablefield);
            Assert.AreEqual(2,f.Children.Count);
            Assert.AreEqual(1, f.Children[0].Id);
            Assert.AreEqual(2, f.Children[1].Id);

        }
    }
}
