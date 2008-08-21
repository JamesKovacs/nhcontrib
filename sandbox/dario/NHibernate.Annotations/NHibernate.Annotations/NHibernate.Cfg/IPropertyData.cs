using System.Reflection;

namespace NHibernate.Cfg
{
    public interface IPropertyData
    {
        /// <summary>
        /// default member access (whether field or property)
        /// </summary>
        string DefaultAccess { get; }

        /// <summary>
        /// return property name
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Returns the returned class itself or the element type if an array
        /// </summary>
        System.Type ClassOrElement { get; }

        /// <summary>
        /// Return the class itself
        /// </summary>
        System.Type PropertyClass { get; }

        /// <summary>
        /// Returns the returned class name itself or the element type if an array
        /// </summary>
        string ClassOrElementName { get; }

        /// <summary>
        /// Returns the returned class name itself
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// 
        /// </summary>
        PropertyInfo Property { get; }
    }
}