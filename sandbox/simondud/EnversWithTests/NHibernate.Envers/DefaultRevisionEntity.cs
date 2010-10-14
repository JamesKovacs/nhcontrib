using System;

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

    public override bool Equals(Object o) 
    {
        if (this == o) return true;
        var revisionEntity = o as DefaultRevisionEntity;
        if (revisionEntity == null) return false;

        var that = revisionEntity;

        if (id != that.id) return false;
        return RevisionDate == that.RevisionDate;
    }

    public override int GetHashCode() 
    {
        var result = id;
        result = 31 * result + (int) (((ulong)RevisionDate.Ticks) ^ (((ulong)RevisionDate.Ticks) >> 32));
        return result;
    }

    public override string ToString() 
    {
        return "DefaultRevisionEntity(id = " + id + 
            ", revisionDate = " + RevisionDate.ToShortDateString() + ")";
    }
}
}
