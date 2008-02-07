using System;
using System.Threading;
using Iesi.Collections.Generic;
using NHibernate.Burrow.NHDomain;
using NHibernate;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.SessionManagerTest {
    [TestFixture]
    public class GetSessionConcurencyTest  {
   
        public static ISet<int> sesss = new HashedSet<int>();
        public static int error = 0;
        [Test]
        public void SingleTest() {
            new ThreadTestProcessor(1).ThreadProc();
        }
        
        [Test]
        public void MultiTreadTest() {
            error = 0;
            for (int j = 0; j < 50; j++){
                ThreadTestProcessor processor = new ThreadTestProcessor(j);
                Thread t = new Thread(new ThreadStart(processor.ThreadProc));
                t.Start();
            }
            for (int i = 0; i < 20; i++){
                Console.WriteLine("MainThread sleeping waiting for all thread done " + i + "/20" );
                Thread.Sleep(1000); //Wait for all thread to stop.
            }
            
            Assert.AreEqual(0, error, error + " Errors occured");
        }

        [Test, Explicit]
        public void ExhostingTest() {
            for (int i = 0; i < 10; i++)
            {
                MultiTreadTest();
            }
        }
    }

    public class ThreadTestProcessor {
        private int num;

        public ThreadTestProcessor(int num) {
            this.num = num;
        }

        public void ThreadProc()
        {
            try
            {
                DomainContext.Initialize();
                ISession session = SessionManager.Instance.GetSession();
                Assert.IsNotNull(session);
                int code = session.GetHashCode();
                session.Flush();
                Assert.IsTrue(GetSessionConcurencyTest.sesss.Add(code));
                Console.WriteLine("Thread # " + num + " succeeded.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread #" + num + " occurs error:" + e.Message);
                GetSessionConcurencyTest.error++;
                throw;
            }
            finally {
                DomainContext.Current.Close();
            }
        }
    }

}