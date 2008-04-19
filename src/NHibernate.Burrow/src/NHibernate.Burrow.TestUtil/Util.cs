using System;
using System.Reflection;
using log4net.Config;

namespace NHibernate.Burrow.TestUtil
{
    public static class Util
    {
        public static void PrintObject(object o)
        {
            Console.WriteLine("==========" + o + "===========");
            foreach (PropertyInfo pi in o.GetType().GetProperties())
            {
                if (pi.CanRead)
                {
                    Console.WriteLine("Property - " + pi.Name + ": " + pi.GetValue(o, null));
                }
            }
            Console.WriteLine("========== EOF " + o + "===========");
        }

        public static void BeginLog()
        {
            XmlConfigurator.Configure();
        }
    }
}