using NHibernate.Burrow.Impl;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Burrow.Util
{
    public class SchemaUtil
    {
        public void CreateSchemas()
        {
			CreateSchemas(true,true);
        }
		
		public void CreateSchemas(bool script, bool export)
        {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                SchemaExport se = new SchemaExport(pu.NHConfiguration); 
                se.Create(script, export);
            }
        }

        public void DropSchemas(bool script, bool export)
        {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                SchemaExport se = new SchemaExport(pu.NHConfiguration);
                se.Drop(script, export);
            }
        }

		public void UpdateSchemas(bool script, bool update) {
			foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
			{
				SchemaUpdate su = new SchemaUpdate(pu.NHConfiguration);
				su.Execute(script, update);
			}
		}
    }
}