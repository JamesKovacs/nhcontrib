using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// an interface for entities that has Deletion logic in itself
    /// </summary>
    public interface IDeletable
    {
        bool Delete();
    }
}
