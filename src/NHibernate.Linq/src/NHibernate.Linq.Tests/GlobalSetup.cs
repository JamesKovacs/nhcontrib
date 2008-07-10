using System;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Linq.Tests.Entities;
using NHibernate.Tool.hbm2ddl;



public class GlobalSetup
{
	private static ISessionFactory factory;

	[SetUp]
	public void SetupNHibernate()
	{
		Configuration cfg = new Configuration().Configure();
		new SchemaExport(cfg).Execute(false, true, false, true);
		factory = cfg.BuildSessionFactory();

		CreateTestData();
	}

	[TearDown]
	public void TearDown()
	{
		
	}

	private static void CreateTestData()
	{
		var roles = new[]
            {
                new Role()
                {
                    Name = "Admin",
                    IsActive = true,
                    Entity = new AnotherEntity()
                    {
                        Output = "this is output..."
                    }
                },
                new Role()
                {
                    Name = "User",
                    IsActive = false
                }
            };

		var users = new[]
        	{
        		new User("ayende", DateTime.Today)
                {
                    Role = roles[0],
                    InvalidLoginAttempts = 4,
                    Enum1 = EnumStoredAsString.Medium,
                    Enum2 = EnumStoredAsInt32.High,
                    Component = new UserComponent()
                    {
                        Property1 = "test1",
                        Property2 = "test2",
                        OtherComponent = new UserComponent2()
                        {
                            OtherProperty1 = "othertest1"
                        }
                    }
                },
        		new User("rahien", new DateTime(1998, 12, 31))
                {
                    Role = roles[1],
                    InvalidLoginAttempts = 5,
                    Enum1 = EnumStoredAsString.Small,
                    Component = new UserComponent()
                    {
                        Property2 = "test2"
                    }
                },
        		new User("nhibernate", new DateTime(2000, 1, 1))
                {
                    InvalidLoginAttempts = 6,
                    LastLoginDate = DateTime.Now.AddDays(-1),
                    Enum1 = EnumStoredAsString.Medium
                }
        	};

		var timesheets = new[]
            {
                new Timesheet
                {
                    SubmittedDate = DateTime.Today,
                    Submitted = true
                },
                new Timesheet
                {
                    SubmittedDate = DateTime.Today.AddDays(-1),
                    Submitted = false, 
                    Entries = new List<TimesheetEntry>
                    {
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Today,
                            NumberOfHours = 6
                        },
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Today.AddDays(1),
                            NumberOfHours = 14
                        }
                    }
                },
                new Timesheet
                {
                    SubmittedDate = DateTime.Now.AddDays(1),
                    Submitted = true,
                    Entries = new List<TimesheetEntry>
                    {
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Now.AddMinutes(20),
                            NumberOfHours = 4
                        },
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Now.AddMinutes(10),
                            NumberOfHours = 8
                        },
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Now.AddMinutes(13),
                            NumberOfHours = 7
                        },
                        new TimesheetEntry
                        {
                            EntryDate = DateTime.Now.AddMinutes(45),
                            NumberOfHours = 38
                        }
                    }
                }
            };

		using (ISession session = CreateSession())
		{
			session.Delete("from Role");
			session.Delete("from User");
			session.Delete("from Timesheet");
			session.Flush();

			foreach (Role role in roles)
				session.Save(role);

			foreach (User user in users)
				session.Save(user);

			foreach (Timesheet timesheet in timesheets)
				session.Save(timesheet);

			session.Flush();
		}
	}

	public static ISession CreateSession()
	{
		return factory.OpenSession();
	}

	public static ISession CreateSession(IDbConnection con)
	{
		return factory.OpenSession(con);
	}
}
