using NHibernate.Burrow.Util.DomainSession;

namespace NHibernate.Burrow.Util.Test.DomainSession
{
    public class BooDomainSession : DomainSessionBase, IHasUserId {
        public string test = "test";
        private object userId;
        /// <summary>
        /// Just return the BooDomainLayer.Current as BooDomainLayer rather than DomainLayerBase
        /// </summary>
        public new static BooDomainSession Current {
            get { return DomainSessionContainer.Instance.Get(typeof(BooDomainSession).Name) as BooDomainSession; }
        }

        #region IHasUserId Members

        public object UserId {
            get { return userId; }
            set { userId = value; }
        }

        #endregion
    }
}