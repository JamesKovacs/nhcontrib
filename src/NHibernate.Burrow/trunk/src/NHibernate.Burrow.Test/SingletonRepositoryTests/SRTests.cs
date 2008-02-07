using NUnit.Framework;

namespace MindHarbor.DomainTemplate.Test.SingletonRepositoryTests {
    [TestFixture]
    public class SRTests     {
        [Test]
        public void Test() {
           Assert.AreEqual( typeof(TestClass1),SingletonRepository.Instance.Find(typeof (TestClass1)).GetType());
           Assert.IsNotNull(SingletonRepository.Instance.Find<TestClass2>());
           Assert.AreEqual(2, SingletonRepository.Instance.FindAll<ITestInterface>().Count);

           SingletonRepository.Instance.Register(typeof (TestClass3));
           Assert.AreEqual(3, SingletonRepository.Instance.FindAll<ITestInterface>().Count);
           Assert.IsNotNull(SingletonRepository.Instance.Find<TestClass3>());
         
        }
    }
    
    public class TestClass1 : ITestInterface{
        
    }


    public class TestClass2 : ITestInterface
    {

    }

    public class TestClass3 : ITestInterface
    {

    }
    
    
    public interface ITestInterface{}
}