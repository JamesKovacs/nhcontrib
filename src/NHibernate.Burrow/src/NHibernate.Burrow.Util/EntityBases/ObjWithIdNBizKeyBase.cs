using System;

namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// A base class that has an integer Identity that can be used to identify the persistant object, and a business key <see cref="BusinessKey"/> for calculating equality
    /// </summary>
    /// <remarks>
    /// The purpose of inheriting this class is to inherit its Equals() and GetHashCode()
    /// which were properly overriden for the has-Id characteristic.
    /// The Equals() and GetHashCode() methods have the following behavior: 
    /// the two objects are Equal and have the same HashCode when and only when
    ///  1) they are of the same type and 2) the have the same business key 
    /// This class also offers a default meaningful ToString() method
    /// </remarks>
    public abstract class ObjWithIdNBizKeyBase : IWithIdNBizKey, IEquatable<ObjWithIdNBizKeyBase>
    {
        /// <summary>
        /// Recommend to use this as the seperator of the composite business key
        /// </summary>
        protected const string BIZKEYSEP = "<!--BIZKEYSEP-->";

        private Int32 id;

        #region IEquatable<ObjWithIdNBizKeyBase> Members

        public bool Equals(ObjWithIdNBizKeyBase objWithIdNBizKeyBase)
        {
            if (objWithIdNBizKeyBase == null)
            {
                return false;
            }
            return Equals(BusinessKey, objWithIdNBizKeyBase.BusinessKey);
        }

        #endregion

        #region IWithIdNBizKey Members

        /// <summary>
        /// A BusinessKey (business key) is a property, or some combinatioin of properties, that is unique for each instance with the same database identity.
        /// Essenstially it's the natural key you'd use if you weren't using a surrogate key. 
        /// </summary>
        public abstract IComparable BusinessKey { get; }

        /// <summary>
        /// The database identity integer of the class or Surrogate key
        /// </summary>
        /// <remarks> 
        /// This method is set as virtual for ORM framework to dynamically create proxy
        /// </remarks>
        public virtual Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>
        /// User BusinessKey to Compare 
        /// This method is set as virtual for ORM framework to dynamically create proxy
        /// </remarks>
        public virtual int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            if (!(obj is ObjWithIdNBizKeyBase))
            {
                throw new ArgumentException("Object must be of type ObjWithIdNBizKeyBase");
            }
            return BusinessKey.CompareTo(((ObjWithIdNBizKeyBase) obj).BusinessKey);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as ObjWithIdNBizKeyBase);
        }

        public override int GetHashCode()
        {
            return BusinessKey.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                GetType().ToString() + " #" + Id.ToString() + "( BusinessKey:"
                + BusinessKey.ToString().Replace(BIZKEYSEP, " ") + ")";
        }
    }
}