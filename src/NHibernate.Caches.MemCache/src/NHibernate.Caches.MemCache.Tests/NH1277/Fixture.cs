using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Core;
using Memcached.ClientLibrary;
using NUnit.Framework;

namespace NHibernate.Caches.MemCache.Tests.NH1277
{
	[TestFixture]
	public class Fixture
	{
		[Test]
		public void DontInitializeTwice()
		{
			log4net.Config.XmlConfigurator.Configure();

			using (LogSpy spy = new LogSpy(typeof(SockIOPool)))
			{
				MemCacheProvider prov = new MemCacheProvider();
				MemCacheProvider prov2 = new MemCacheProvider();

				prov.Start(new Dictionary<string, string>());
				prov2.Start(new Dictionary<string, string>());

				prov.Stop();
				prov2.Stop();

				foreach (LoggingEvent loggingEvent in spy.Appender.GetEvents())
				{
					if (loggingEvent.Level >= Level.Warn)
					{
						Debug.WriteLine(loggingEvent.Level + " received: " + loggingEvent.RenderedMessage);
						Assert.IsFalse(loggingEvent.RenderedMessage.Contains("Trying to initialize an already initialized pool"));
					}
				}
			}
		}
	}
}