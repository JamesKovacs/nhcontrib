namespace Example.Web
{
	using System.Web.UI;
	using Domain.Models;
	using log4net;
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Tool.hbm2ddl;

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

			Configuration configuration = new Configuration();
			configuration.Configure();

			try
			{
				_log.Debug("Creating database...");
				RecreateDatabase(configuration);
				_log.Debug("...database created");
			}
			catch (HibernateException exc)
			{
				_log.Fatal("Did you forget to modify 'connection.connection_string' in web.config to point to a valid database?", exc);
				return;
			}

			using (ISessionFactory factory = configuration.BuildSessionFactory())
			{
				int homePhoneNumberId;
				int personId;

				using (ISession session = factory.OpenSession())
				{
					Person person;
					PhoneNumber homePhoneNumber;

					_log.Debug("Writing data to database...");
					using (ITransaction trans = session.BeginTransaction())
					{
						person = new Person();
						person.Name = "Lazy";
						session.Save(person);

						homePhoneNumber = new PhoneNumber();
						homePhoneNumber.Number = "867-5309";
						homePhoneNumber.PhoneType = "Cell";
						homePhoneNumber.Person = person;
						session.Save(homePhoneNumber);

						trans.Commit();
					}
					_log.Debug("...data written");

					personId = person.Id;
					homePhoneNumberId = homePhoneNumber.Id;
				}

				try
				{
					using (ISession session = factory.OpenSession())
					{
						Person lazyPerson = session.Load<Person>(personId);

						_log.Debug("Lazy loading an entity...");
						string lazyName = lazyPerson.Name;
						_log.Debug("...entity loaded");
					}

					using (ISession session = factory.OpenSession())
					{
						PhoneNumber home = session.Get<PhoneNumber>(homePhoneNumberId);

						_log.Debug("Lazy loading a related entity...");
						Person lazyPerson = home.Person;
						_log.Debug("...related entity loaded");
					}

					_log.Debug("Comment out 'proxyfactory.factory_class' in web.config and try again.");
				}
				catch (HibernateException exc)
				{
					_log.Error("Cannot generate lazy loading proxies in a Medium Trust environment.  Try using NHibernate.ProxyGenerators :)", exc);
				}
			}
		}

		private static void RecreateDatabase(Configuration configuration)
		{
			SchemaExport export = new SchemaExport(configuration);

			export.Drop(false, true);
			export.Create(false, true);
		}
	}
}
