using System;
using System.Runtime.CompilerServices;

namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// A base class that use its inital hashcode as its Identity
    /// </summary>
    /// <remarks>
    /// It can be used as a base class for those classes withe whom you canot define a business key easily 
    /// For this object to work, you should mapped the HashId property in the hbm file
    /// If you are sure that you use a one session per request pattern you can use entity without business key
    /// <example>
    /// <![CDATA[
    /// <property name="HashId" />
    /// ]]>
    /// </example>
    /// 
    /// </remarks>
    public abstract class ObjectWHashIdBase : ObjWithIdNBizKeyBase, IWithIdNBizKey
    {
        private long hashId;

        /// <summary>
        /// 
        /// </summary>
        protected ObjectWHashIdBase()
        {
            long result = (long) (DateTime.Now - new DateTime(2000, 1, 1)).TotalMilliseconds;
            HashId = result * 131237 + RuntimeHelpers.GetHashCode(this) % 4355463;
        }

        private long HashId
        {
            get { return hashId; }
            set { hashId = value; }
        }

        #region IWithIdNBizKey Members

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// return the HashId
        /// </remarks>
        public override IComparable BusinessKey
        {
            get { return HashId; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// overriden to use the HashId
        /// This method is kept as virtual for ORM framework to dynamically create proxy
        /// But it should never be overriden by developer
        /// </remarks>
        public override int GetHashCode()
        {
            return hashId.GetHashCode();
        }
    }
}