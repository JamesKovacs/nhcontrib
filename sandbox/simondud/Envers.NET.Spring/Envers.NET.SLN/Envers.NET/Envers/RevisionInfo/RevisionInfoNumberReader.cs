using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Entities;
using System.Reflection;

namespace NHibernate.Envers.RevisionInfo
{
/**
 * Gets a revision number from a persisted revision info entity.
 * @author Adam Warski (adam at warski dot org)
 */
public class RevisionInfoNumberReader {
    private readonly System.Type revisionIdType;
    private readonly PropertyData pd;
    public RevisionInfoNumberReader(System.Type revisionInfoType, PropertyData revisionInfoIdData) {
        this.revisionIdType = revisionInfoType;
        this.pd = revisionInfoIdData; // ReflectionTools.getGetter(revisionInfoClass, revisionInfoIdData);
    }

    public long getRevisionNumber(Object revision) {
        PropertyInfo prop = revisionIdType.GetProperty(pd.BeanName, typeof(DateTime));

        return (long)prop.GetValue(revision.GetType(), null);
    }
}
}
