using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Tool.hbm2ddl;
using Environment=NHibernate.Cfg.Environment;

namespace NHibernate.JetDriver.Tests
{
    public abstract class JetTestBase
    {
        private readonly JetDriver jetDriver = new JetDriver();
        private readonly SqlType[] dummyParameterTypes = { };
        private readonly Configuration configuration;
        private readonly ISessionFactory factory;
        private readonly ISessionFactoryImplementor factoryImpl;

        protected JetTestBase() : this(false)
        {
        }

        protected JetTestBase(bool autoCreateTables)
        {
            try
            {
                configuration = new Configuration()
                    .SetProperty(Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                    .SetProperty(Environment.Dialect, typeof(JetDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof(JetDriver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName)
                    .SetProperty(Environment.ConnectionString, string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", DataFile));

                AddEntities();

                factory = configuration.BuildSessionFactory();
                factoryImpl = (ISessionFactoryImplementor)factory;

                if(autoCreateTables)
                {
                    CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void AddEntities()
        {
            foreach (var type in EntityTypes)
            {
                configuration.AddClass(type);
            }
        }

        protected Configuration Configuration
        {
            get { return configuration; }
        }

        protected virtual void CreateTables()
        {
            new SchemaExport(configuration).Create(false, true);
        }

        protected virtual void DropTables()
        {
            new SchemaExport(configuration).Drop(false, true);
        }

        protected virtual IList<System.Type> EntityTypes
        {
            get { return new List<System.Type>(); }
        }

        private string DataFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JetTests.db"); }
        }

        protected string GetTransformedSql(string sqlQuery)
        {
            var sql = SqlString.Parse(sqlQuery);
            var command = jetDriver.GenerateCommand(CommandType.Text, sql, dummyParameterTypes);

            return command.CommandText;
        }

        protected ISessionFactory SessionFactory
        {
            get { return factory; }
        }

        protected ISessionFactoryImplementor SessionFactoryImpl
        {
            get { return factoryImpl; }
        }
    }
}