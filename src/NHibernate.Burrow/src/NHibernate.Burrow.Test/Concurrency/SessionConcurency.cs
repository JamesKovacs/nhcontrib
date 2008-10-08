using System;
using System.Threading;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.Concurrency
{
    [TestFixture]
    public class SessionConcurency
    {
        public static ISet<int> sesss = new HashedSet<int>();
        public static int error = 0;
        public static int threadPerformed = 0;
    	private static bool loudly = false;

    	public class ThreadTestProcessor
        {
            private int num;

            public ThreadTestProcessor(int num)
            {
                this.num = num;
            }

            public void ThreadProc()
            {
                try
                {
                    new BurrowFramework().InitWorkSpace();
                    ISession session = new BurrowFramework().GetSession();
                    Assert.IsNotNull(session);
                    int code = session.GetHashCode();
                    session.Flush();
                    Assert.IsTrue(sesss.Add(code));
                   Output("Thread # " + num + " succeeded.");
                    threadPerformed++;
                }
                catch (Exception e)
                {
					Output("Thread #" + num + " occurs error:" + e.Message);
                    error++;
                    throw;
                }
                finally
                {
                    new BurrowFramework().CloseWorkSpace();
                }
            }
        }


        [Test]
        public void MultiTreadTest()
        {
            error = 0;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            for (int j = 0; j < 50; j++)
            {
                ThreadTestProcessor processor = new ThreadTestProcessor(j);
                Thread t = new Thread(new ThreadStart(processor.ThreadProc));
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();
            }
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            for (int i = 0; i < 2000; i++)
            {
                if (i % 100 == 0)
                {
					Output("MainThread sleeping waiting for all thread done " + i + "/2000");
                }
                if (threadPerformed == 50)
                {
                    break;
                }
                Thread.Sleep(10); //Wait for all thread to stop.
            }
			Output("Waitting done");
            Assert.AreEqual(50, threadPerformed, "not all thread are performed");
            Assert.AreEqual(0, error, error + " Errors occured");
        }

        [Test]
        public void SingleTest()
        {
            new ThreadTestProcessor(1).ThreadProc();
        }

		private static void Output(string line) {
			if(loudly)
				Console.WriteLine(line);

		}
    }
}