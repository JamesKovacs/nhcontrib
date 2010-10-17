using Iesi.Collections.Generic;

namespace NHibernate.Envers.Tests.Entities.Collection
{
    public class StringSetEntity
    {
        public StringSetEntity()
        {
            Strings = new HashedSet<string>();
        }

        public virtual int Id { get; set; }
        [Audited]
        public virtual ISet<string> Strings { get; set; }
    }
}