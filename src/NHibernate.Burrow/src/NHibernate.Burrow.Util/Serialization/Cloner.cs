using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NHibernate.Burrow.Util.Serialization
{
    /// <summary>
    /// Helper to clone objects using Serialization.
    /// </summary>
    public class Cloner
    {
        /// <summary>
        /// Clone a object via Binary Serializing/Deserializing.
        /// The object to clone must have the attribute [Serializable]
        /// <example>
        /// Here it's an example to make a simple object clone:
        /// <code>
        /// Foo foo = new Foo();
        /// Foo clonedFoo = (Foo)Cloner.Clone(foo);  
        /// </code>
        /// Then the class Foo in order to work must implement [Serializable]
        /// <code>
        /// [Serializable]
        /// public class Foo
        /// {
        ///  ...
        /// } 
        /// </code>
        /// </example>
        /// <seealso cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="obj">Object to clone</param>
        /// <returns>Clone object</returns>
        public static object Clone(object obj)
        {
            return Clone(obj, new BinaryFormatter());
        }

        /// <summary>
        /// Clone a object with via Serializing/Deserializing
        /// using a implementation of IFormatter. 
        /// <seealso cref="IFormatter"/> 
        /// </summary>
        /// <param name="obj">Object to clone</param>
        /// <param name="formatter">IFormatter implementation in order to Serialize/Deserialize</param>
        /// <returns>Clone object</returns>
        public static object Clone(object obj, IFormatter formatter)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                return formatter.Deserialize(buffer);
            }
        }
    }
}