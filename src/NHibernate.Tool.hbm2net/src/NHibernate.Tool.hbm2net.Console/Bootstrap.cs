using System;
using log4net.Config;

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
                CodeGenerator.Generate(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("****** Fatal error, generation stopped:" + e.Message);
            }
		}
	}
}