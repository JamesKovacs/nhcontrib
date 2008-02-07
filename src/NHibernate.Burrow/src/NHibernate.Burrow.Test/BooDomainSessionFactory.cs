using NHibernate.Burrow.Util.DomainSession;

namespace NHibernate.Burrow.Test {
    public class BooDomainSessionFactory : DomainSessionFactoryBase {
        public override DomainSessionBase Create() {
            return new BooDomainSession();
        }
    }
}