using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.hbm2net
{
    public class Hbm2NetParameters:CommandLineParser
    {
        public Hbm2NetParameters(string[] args)
            :base(args)
        {

        }
        [Option("config", LongHelp = "Configuration file ( defaults to T4 generator with default parameters )\nConfig file example:\n\n<codegen>\n\t<generate renderer=\"NHibernate.Tool.hbm2net.T4Render, NHibernate.Tool.hbm2net\" package=\"\">\n\t\t<param name=\"template\">res://NHibernate.Tool.hbm2net.templates.hbm2net.tt</param>\n\t\t<param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param>\n\t</generate>\n</codegen>\n\n* Multiple generation steps is achieved by multiple <codegen> nodes.", Optional = true, Default = "", ShortHelp = "configuration file")]
        public string ConfigFile { get; set; }
        [Option("output", LongHelp = "Directory for generated files", Optional = true,Default="", ShortHelp = "output dir")]
        public string OutputDir { get; set; }
        [Option("ct", LongHelp = "Does not generate file if target is newer than the hbm (defaults to false) ", Optional = true, Default = "", ShortHelp = "check target time")]
        public string CheckTime { get; set; }
    }
}
