namespace NHibernate.JetDriver.Tests.Entities
{
    public class ReportDTO
    {
        public ReportDTO(int catalogId, int categoryId, string categoryName, int productTypeId, string productTypeName)
        {
            CatalogId = catalogId;
            CategoryId = categoryId;
            ProductTypeId = productTypeId;
            CategoryName = categoryName;
            ProductTypeName = productTypeName;
        }

        public virtual int CatalogId
        {
            get;
            private set;
        }

        public virtual string CategoryName
        {
            get;
            private set;
        }

        public virtual string ProductTypeName
        {
            get;
            private set;
        }

        public int CategoryId
        {
            get;
            private set;
        }

        public int ProductTypeId
        {
            get;
            private set;
        }
    }
}