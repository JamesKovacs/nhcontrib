using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Test {
    public class BooDomainLayerFactory : NHibernate.Burrow.NHDomain.DomainSessionFactoryBase
    {
        public override NHibernate.Burrow.NHDomain.DomainSessionBase Create()
        {
            return new BooDomainLayer();
        }
    }
}

