using System;

namespace NHibernate.Burrow {
    /// <summary>
    /// OBSOLETE
    /// A base class that has an integer Id as its Identity
    /// </summary>
    /// <remarks>
    /// DO NOT USE THIS CLASS AS THE BASE CLASS FOR PERSISTANT CLASS
    /// The purpose of inheriting this class is to inherit its Equals() and GetHashCode()
    /// which were properly overriden for the has-Id characteristic.
    /// The Equals() and GetHashCode() methods have the following behavior: 
    /// the two objects are Equal and have the same HashCode when and only when
    ///  1) they are of the same type 
    /// Note: the HashCode constraint does not always hold.  
    /// 
    /// This class also offers a default meaningful ToString() method
    /// </remarks>
    [Obsolete]
    public abstract class ObjectWithIdentityIdBase : IWithId {
        private Int32 id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected ObjectWithIdentityIdBase(int id) {
            Id = id;
        }

        private ObjectWithIdentityIdBase() {}

        #region IWithId Members

        /// <summary>
        /// The identity integer of the class
        /// </summary>
        public Int32 Id {
            get { return id; }
            private set { id = value; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (!(obj.GetType().Equals(GetType()))) return false;
            if (obj == this) return true;
            ObjectWithIdentityIdBase po = (ObjectWithIdentityIdBase) obj;
            return ((ObjectWithIdentityIdBase) obj).Id.Equals(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int result = 22;
            result = 37*result + GetType().GetHashCode();
            result = 37*result + Id;
            return result;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return GetType().ToString() + " #" + Id.ToString();
        }
    }
}