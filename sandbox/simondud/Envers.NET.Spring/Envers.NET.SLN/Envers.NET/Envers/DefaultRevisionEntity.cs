using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace NHibernate.Envers
{
/**
 * @author Adam Warski (adam at warski dot org)
 */
[Serializable]
public class DefaultRevisionEntity {
    //ORIG: @Id - TODO Simon - see if @Id is necessary
    [RevisionNumber]
    public virtual int id {get; set;}

    [RevisionTimestamp]
    public virtual DateTime RevisionDate { get; set; }

    //ORIG: @Transient - TODO Simon - see equivalent
    //[Transient]
    public bool Equals(Object o) {
        if (this == o) return true;
        if (!(o is DefaultRevisionEntity)) return false;

        DefaultRevisionEntity that = (DefaultRevisionEntity) o;

        if (id != that.id) return false;
        if (RevisionDate != that.RevisionDate) return false;

        return true;
    }

    public int getHashCode() {
        int result;
        result = id;
        result = 31 * result + (int) (((ulong)RevisionDate.Ticks) ^ (((ulong)RevisionDate.Ticks) >> 32));
        return result;
    }

    public String ToString() {
        return "DefaultRevisionEntity(id = " + id + 
            ", revisionDate = " + RevisionDate.ToShortDateString() + ")";
    }
}
}
