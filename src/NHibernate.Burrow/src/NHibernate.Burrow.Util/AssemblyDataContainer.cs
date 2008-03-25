using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace NHibernate.Burrow.Util
{
    /// <summary>
    /// this helper class help to organize static and container data for multiple domain Assemblies
    /// </summary>
    /// <remarks> 
    /// it's now only used for the DomainSession Instance
    /// </remarks>
    public class AssemblyDataContainer
    {
        private const string DIVIDER = "_:_";
        private static readonly object lockObj = new object();

        private static IDictionary<string, object> staticData = new Dictionary<string, object>();

        [ThreadStatic] private static IDictionary<string, object> threadData = new Dictionary<string, object>();

        /// <summary>
        /// The domain assembly the current context is using.
        /// </summary>
        public static Assembly CurrentDomainAssembly
        {
            get
            {
                foreach (StackFrame s in new StackTrace().GetFrames())
                {
                    Assembly a = Assembly.GetAssembly(s.GetMethod().DeclaringType);
                    if (VerifyAssemly(a))
                    {
                        return a;
                    }
                }
                throw new Exception("There are multilple domain assembly in the config Settings,"
                                    + " none of them is callling this");
            }
        }

        private static bool VerifyAssemly(Assembly a)
        {
            foreach (PersistenceUnit unit in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                if (unit.DomainLayerAssemblies.Contains(a))
                {
                    return true;
                }
            }
            return false;
        }

        private static string ComposeKey(string key)
        {
            return ComposeKey(key, CurrentDomainAssembly);
        }

        private static string ComposeKey(string key, Assembly a)
        {
            return a.GetName().Name + DIVIDER + key;
        }

        /// <summary>
        /// Gets data stored in the CallStack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetCallStackData<T>(string key)
        {
            return (T) CallContext.GetData(key);
        }

        /// <summary>
        /// Store data  into the CallStack
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCallStackData(string key, object value)
        {
            CallContext.SetData(key, value);
        }

        /// <summary>
        /// Gets data stored in the HttpSession
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetHttpSessionData<T>(string key) where T : class
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            if (HttpContext.Current.Session == null)
            {
                throw new Exception("HttpSession must be initialized before GetHttpSessionData can be called");
            }
            return (T) HttpContext.Current.Session[ComposeKey(key)];
        }

        /// <summary>
        /// Stores data into the HttpSession
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>fasle if the there is no current HTTP application</returns>
        public static bool SetHttpSessionData(string key, object value)
        {
            if (HttpContext.Current == null)
            {
                return false;
            }
            if (HttpContext.Current.Session == null)
            {
                throw new Exception("HttpSession must be initialized before SetHttpSessionData can be called");
            }
            HttpContext.Current.Session[ComposeKey(key)] = value;
            return true;
        }

        /// <summary>
        /// Gets data stored in a threadstatic data container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="callerType"></param>
        /// <returns></returns>
        public static T GetThreadStaticData<T>(string key, System.Type callerType) where T : class
        {
            if (threadData == null)
            {
                return null;
            }
            string compositeKey = callerType + DIVIDER + ComposeKey(key);

            if (!threadData.Keys.Contains(compositeKey))
            {
                return null;
            }
            return (T) threadData[compositeKey];
        }

        /// <summary>
        /// Store data into a thread static data container
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="callerType"></param>
        public static void SetThreadStaticData(string key, object value, System.Type callerType)
        {
            if (threadData == null)
            {
                return;
            }
            threadData[callerType + DIVIDER + ComposeKey(key)] = value;
        }

        /// <summary>
        /// Gets data stored in a Static Data Container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="callerType"></param>
        /// <returns></returns>
        public static T GetStaticData<T>(string key, System.Type callerType) where T : class
        {
            lock (lockObj)
            {
                if (staticData == null)
                {
                    return null;
                }
                string compositeKey = callerType + DIVIDER + ComposeKey(key);
                if (!staticData.ContainsKey(compositeKey))
                {
                    return null;
                }
                return (T) staticData[compositeKey];
            }
        }

        /// <summary>
        /// Store data into a Static Data Container
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="callerType"></param>
        public static void SetStaticData(string key, object value, System.Type callerType)
        {
            lock (lockObj)
            {
                staticData[callerType + DIVIDER + ComposeKey(key)] = value;
            }
        }
    }
}