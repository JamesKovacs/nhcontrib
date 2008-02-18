using System;
using System.Reflection;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Util.Exceptions;

namespace NHibernate.Burrow.Util.DomainSession {
    public class DSConfig {
        /// <summary>
        /// Indicates if the current Domain Assembly is using a DomainSession
        /// </summary>
        public static bool IsUsingDomainLayer {
            get {
                return
                    !string.IsNullOrEmpty(
                         CurrentDomainSessionFactoryName());
            }
        }

        /// <summary>
        /// find the DomainLayerFactory by using the name set in the key "MHGeneralLib.NHDomain.DomainLayerFactoryName" in the config file
        /// if the key is not set or set to be DEFAULT then return a null;
        /// </summary>
        public static IDomainSessionFactory GetDomainSessionFactory() {
            IDomainSessionFactory factory;
            Assembly currentAssembly = AssemblyDataContainer.CurrentDomainAssembly;

            string fName = CurrentDomainSessionFactoryName();
            if (string.IsNullOrEmpty(fName))
                throw new Exception(
                    "To use auto managed domainSession the \"domainSessionFactory\" must be set in the " +
                    " domainAssembly section in NHibernate.Burrow section in the config file.");

            System.Type fType = currentAssembly.GetType(fName);
            if (fType == null)
                throw new TypeNotFoundException("The DomainSessionFactory with the name " + fName +
                                                " was not found in the assembly. Please check domianSessionFactoryName in your DomainTemplate section in the config file. The DomainSession cannot be created.");
            ConstructorInfo ci = fType.GetConstructor(System.Type.EmptyTypes);
            if (ci == null)
                throw new NoSuitableContructorException(
                    "The DomainSessionFactory must have a public constructor with no paremeters. The DomainSession cannot be created.");
            factory = (IDomainSessionFactory) ci.Invoke(new object[0]);
            return factory;
        }

        private static string CurrentDomainSessionFactoryName() {
            throw new NotImplementedException();
          //  return (string) Config.DomainAssemblySettings()["domainSessionFactory"];
        }
    }
}