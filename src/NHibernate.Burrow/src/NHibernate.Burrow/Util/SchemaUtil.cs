using NHibernate.Burrow.Impl;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Burrow.Util
{
    public class SchemaUtil
    {
        public void CreateSchemas()
        {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                SchemaExport se = new SchemaExport(pu.NHConfiguration);
                se.Drop(true, true);
                se.Create(true, true);
            }
        }

        public void DropSchemas()
        {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                SchemaExport se = new SchemaExport(pu.NHConfiguration);
                se.Drop(true, true);
            }
        }
    }
}