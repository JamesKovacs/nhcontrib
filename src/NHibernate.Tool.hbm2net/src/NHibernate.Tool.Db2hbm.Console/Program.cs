using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using log4net;
using System.IO;
using System.Xml;

namespace NHibernate.Tool.Db2hbm.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger("db2hbm");
            Db2hbmParameters parms = new Db2hbmParameters(args);
            try
            {
                parms.Parse();
                MappingGenerator gen = new MappingGenerator();
                if (File.Exists(parms.ConfigFile))
                {
                    using (var reader = XmlReader.Create(new StreamReader(parms.ConfigFile)))
                    {
                        gen.Configure(reader);
                    }
                    if (!Directory.Exists(parms.OutputDir))
                    {
                        Directory.CreateDirectory(parms.OutputDir);
                    }
                    using (var sp = new StreamProvider(parms.OutputDir))
                    {
                        gen.Generate(sp);
                    }
                }
                else
                {
                    throw new FileNotFoundException(parms.ConfigFile);
                }
            }
            catch (NotEnougthParametersException)
            {
                System.Console.Error.WriteLine("Use: db2hbm" + parms.GetShortHelp());
                System.Console.Error.WriteLine(parms.GetHelp());
            }
            catch (Exception e)
            {
                logger.Error("Fatal error", e);
            }
        }
    }
}
