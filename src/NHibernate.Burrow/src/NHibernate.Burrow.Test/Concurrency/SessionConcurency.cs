using System;
using System.Threading;
using Iesi.Collections.Generic;
using NHibernate.Burrow;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.Concurrency {
    [TestFixture]
    public class SessionConcurency {
        public static ISet<int> sesss = new HashedSet<int>();
        public static int error = 0;
        public static int threadPerformed = 0;
        /// <summary>
        /// it is set explicit as it's quite time consuming
        /// </summary>
        [Test, Explicit]
        public void ExhostingTest() {
            for (int i = 0; i < 10; i++)
                MultiTreadTest();
        }

        /// <summary>
        /// it is set explicit as it's quite time consuming
        /// </summary>
        [Test] 
        public void MultiTreadTest() {
            error = 0;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            for (int j = 0; j < 50; j++) {
                ThreadTestProcessor processor = new ThreadTestProcessor(j);
                Thread t = new Thread(new ThreadStart(processor.ThreadProc));
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();
            }
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            for (int i = 0; i < 2000; i++) {
                if(i % 100 == 0)
                Console.WriteLine("MainThread sleeping waiting for all thread done " + i + "/2000");
                if(threadPerformed == 50)
                    break;
                Thread.Sleep(10); //Wait for all thread to stop.
            }
            Console.WriteLine("Waitting done");
            Assert.AreEqual(50, threadPerformed, "not all thread are performed");
            Assert.AreEqual(0, error, error + " Errors occured");
        }

        [Test]
        public void SingleTest() {
            new ThreadTestProcessor(1).ThreadProc();
        } 
        
        public class ThreadTestProcessor {
        private int num;

        public ThreadTestProcessor(int num) {
            this.num = num;
        }

        public void ThreadProc() {
            try {
                new Facade().InitializeDomain();
                ISession session = new Facade().GetSession();
                Assert.IsNotNull(session);
                int code = session.GetHashCode();
                session.Flush();
                Assert.IsTrue(SessionConcurency.sesss.Add(code));
                Console.WriteLine("Thread # " + num + " succeeded.");
                threadPerformed++;
            }
            catch (Exception e) {
                Console.WriteLine("Thread #" + num + " occurs error:" + e.Message);
                SessionConcurency.error++;
                throw;
            }
            finally {
                new Facade().CloseDomain();
            }
        }
    }
    }

  
}