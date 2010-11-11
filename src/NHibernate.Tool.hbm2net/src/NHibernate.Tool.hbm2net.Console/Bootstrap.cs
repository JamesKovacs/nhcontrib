using System;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace NHibernate.Tool.hbm2net
{
	/// <summary>
	/// Summary description for Bootstrap.
	/// </summary>
	public class Bootstrap
	{
		[STAThread]
		public static void Main(String[] args)
		{
            XmlConfigurator.Configure();
            try
            {
                SetEnv();
                Console.WriteLine(string.Concat("Version=",Assembly.GetExecutingAssembly().GetName().Version.ToString()));
                CodeGenerator.Generate(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("****** Fatal error, generation stopped:" + e.Message);
                if( e.InnerException != null )
                    Console.Error.WriteLine(":" + e.InnerException.Message);
            }
		}

        private static void SetEnv()
        {
            Environment.SetEnvironmentVariable("HBM2NETPATH", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),EnvironmentVariableTarget.Machine);
        }
	}
}