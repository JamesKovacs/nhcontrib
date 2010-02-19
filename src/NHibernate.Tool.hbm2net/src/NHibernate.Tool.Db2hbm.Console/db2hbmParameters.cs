using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public class Db2hbmParameters:CommandLineParser
    {
        public Db2hbmParameters(string[] args)
            :base(args)
        {

        }
        [Option("config", LongHelp = "Configuration file", Optional = false, Default = "", ShortHelp = "configuration file")]
        public string ConfigFile { get; set; }
        [Option("output", LongHelp = "Directory for generated files", Optional = true,Default=".\\", ShortHelp = "output dir")]
        public string OutputDir { get; set; }
       
    }
}
