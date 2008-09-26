namespace Example.ActiveRecordWeb
{
	using System.Web.UI;
	using Castle.ActiveRecord;
	using Castle.ActiveRecord.Framework;
	using Castle.ActiveRecord.Framework.Config;
	using ActiveRecordDomain.Models;
	using log4net;

	public partial class _Default : Page
	{
		private static readonly ILog _log;

		static _Default()
		{
			log4net.Config.XmlConfigurator.Configure();
			_log = LogManager.GetLogger("Default");
		}

		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);

			ActiveRecordStarter.ResetInitializationFlag();
			ActiveRecordStarter.Initialize(typeof(Person).Assembly, ActiveRecordSectionHandler.Instance);
			NHibernate.Cfg.Environment.UseReflectionOptimizer = false;

			try
			{
				_log.Debug("Creating database...");
				RecreateDatabase();
				_log.Debug("...database created");
			}
			catch (ActiveRecordException exc)
			{
				_log.Fatal("Did you forget to modify 'connection.connection_string' in web.config to point to a valid database?", exc);
				return;
			}

			Person person;
			PhoneNumber homePhoneNumber;

			_log.Debug("Writing data to database...");
			using (TransactionScope trans = new TransactionScope())
			{
				person = new Person();
				person.Name = "Lazy";
				person.Save();

				homePhoneNumber = new PhoneNumber();
				homePhoneNumber.Number = "867-5309";
				homePhoneNumber.PhoneType = "Cell";
				homePhoneNumber.Person = person;
				homePhoneNumber.Save();

				trans.VoteCommit();
			}
			_log.Debug("...data written");

			int personId = person.Id;
			person = null;

			int homePhoneNumberId = homePhoneNumber.Id;
			homePhoneNumber = null;

			try
			{
				using (new SessionScope())
				{
					Person lazyPerson = Person.TryFind(personId);

					_log.Debug("Lazy loading an entity...");
					string lazyName = lazyPerson.Name;
					_log.Debug("...entity loaded");
				}

				using (new SessionScope())
				{
					PhoneNumber home = PhoneNumber.Find(homePhoneNumberId);

					_log.Debug("Lazy loading a related entity...");
					Person lazyPerson = home.Person;
					_log.Debug("...related entity loaded");
				}

				_log.Debug("Comment out 'proxyfactory.factory_class' in web.config and try again.");
			}
			catch (ActiveRecordException exc)
			{
				_log.Error("Cannot generate lazy loading proxies in a Medium Trust environment.  Try using NHibernate.ProxyGenerators :)", exc);
			}
		}

		private static void RecreateDatabase()
		{
			ActiveRecordStarter.DropSchema();
			ActiveRecordStarter.CreateSchema();
		}
	}
}
