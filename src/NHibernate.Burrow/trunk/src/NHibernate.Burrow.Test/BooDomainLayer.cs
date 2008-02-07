using System;
using System.Configuration;
using NHibernate.Burrow;
using NHibernate.Burrow.NHDomain;

namespace NHibernate.Burrow.Test {
    public class BooDomainLayer : DomainSessionBase, IHasUserId
    {
        private object userId;

        public object UserId {
            get { return userId; }
            set { userId = value; }
        }


        /// <summary>
        /// Just return the BooDomainLayer.Current as BooDomainLayer rather than DomainLayerBase
        /// </summary>
        public new static BooDomainLayer Current
        {
            get {
                return DomainSessionBase.Current as BooDomainLayer;
            }
        }

     
        public string test = "test";
      
    }
}