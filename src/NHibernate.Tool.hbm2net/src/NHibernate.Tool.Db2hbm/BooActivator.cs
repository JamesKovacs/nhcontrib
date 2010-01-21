using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Parser;
using System.Reflection;
using System.IO;

namespace NHibernate.Tool.Db2hbm
{
    static class BooActivator
    {
        public static T CreateInstance<T>(string codeFragment) where T: class
        {
            //ensure parser is copy to the output directory ...
            Boo.Lang.Parser.BooToken token = new BooToken();
            CompilerParameters parms = new CompilerParameters();
            parms.Debug = false;
            parms.Ducky = true;
            parms.GenerateInMemory = true;
            parms.Input.Add(new StringInput("<script>",ProperIdent(codeFragment)));
            parms.Pipeline = new CompileToMemory();
            var compiler = new BooCompiler(parms);
            var results = compiler.Run();
            if (results.Errors.Count > 0)
            {
                throw new Exception("Can't decorate type:" + typeof(T).Name + " Error:"+GetErrorDescr(results.Errors));
            }
            var decorated =  results.GeneratedAssembly.GetTypes().FirstOrDefault(t => typeof(INamingStrategy).IsAssignableFrom(t));
            if (null == decorated)
            {
                throw new Exception("Invalid decorator class for type:" + typeof(T).Name);
            }
            
            return Activator.CreateInstance(decorated) as T;
            
        }

        private static string ProperIdent(string codeFragment)
        {
            StringReader reader = new StringReader(codeFragment.Trim());
            StringBuilder sb = new StringBuilder();
            string line;
            int currLeadingIn = 0;
            int currLeadingOut = 0;
            while ( (line = reader.ReadLine()) != null)
            {
                string unleading = line.TrimStart();
                int c = line.Length-unleading.Length;
                if (c > currLeadingIn)
                    currLeadingOut++;
                if (c < currLeadingIn && currLeadingOut>0)
                    currLeadingOut--;
                currLeadingIn = c;
                sb.AppendLine(new string('\t', currLeadingOut) + unleading);
            }
            return sb.ToString().Trim();
        }

        private static string GetErrorDescr(CompilerErrorCollection compilerErrorCollection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (CompilerError ce in compilerErrorCollection)
            {
                sb.Append(ce.Message);
            }
            return sb.ToString();
        }
    }
}
