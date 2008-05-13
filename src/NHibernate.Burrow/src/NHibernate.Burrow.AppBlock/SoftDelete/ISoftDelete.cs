using System;

namespace NHibernate.Burrow.AppBlock.SoftDelete
{
    public interface ISoftDelete
    {
        DateTime? DeleteDate { get; set; }
        bool Deleted { get; set; }
    }
}
