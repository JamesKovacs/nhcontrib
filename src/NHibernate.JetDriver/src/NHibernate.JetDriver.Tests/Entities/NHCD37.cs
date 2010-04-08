using System;
using Iesi.Collections.Generic;

namespace NHibernate.JetDriver.Tests.TransformFromClauseBehaviour
{
    [Serializable]
    public class TestDTO
    {
        private object _Id;

        private String _CatalogCategoryName;
        private String _ProductTypeName;
        private int _CatalogCategoryId;
        private int _ProductTypeId;

        public TestDTO(
            Int32 id,
            Int32 catalogCategoryId,
            Int32 productTypeId,
            String catalogCategoryName,
            String productTypeName
            )
        {
            Id = id;
            _CatalogCategoryId = catalogCategoryId;
            _ProductTypeId = productTypeId;
            _CatalogCategoryName = catalogCategoryName;
            _ProductTypeName = productTypeName;
        }

        public virtual object Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public virtual String CatalogCategoryName
        {
            get { return _CatalogCategoryName; }
            set { _CatalogCategoryName = value; }
        }

        public virtual String ProductTypeName
        {
            get { return _ProductTypeName; }
            set { _ProductTypeName = value; }
        }

        public int CatalogCategoryId
        {
            get { return _CatalogCategoryId; }
            set { _CatalogCategoryId = value; }
        }

        public int ProductTypeId
        {
            get { return _ProductTypeId; }
            set { _ProductTypeId = value; }
        }
    }

    [Serializable]
    public class CatalogEntriesEntity
    {
        private object _Id;
        private CatalogCategoriesEntity _CatalogCategory;
        private ProductTypesEntity _ProductType;

        public virtual object Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public virtual CatalogCategoriesEntity CatalogCategory
        {
            get { return _CatalogCategory; }
            set { _CatalogCategory = value; }
        }

        public virtual ProductTypesEntity ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; }
        }
    }

    [Serializable]
    public class CatalogCategoriesEntity
    {
        private object _Id;
        private readonly ISet<CatalogCategoriesEntity> _ChildrenCategories = new HashedSet<CatalogCategoriesEntity>();
        private readonly ISet<CatalogEntriesEntity> _CatalogEntries = new HashedSet<CatalogEntriesEntity>();
        private CatalogCategoriesEntity _CatalogCategoryParent;
        private String _Name;
        private ProductCatalogsEntity _ProductCatalog;

        public virtual object Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        
        public virtual ISet<CatalogCategoriesEntity> ChildrenCategories
        {
            get { return _ChildrenCategories; }
        }

        public virtual ISet<CatalogEntriesEntity> CatalogEntries
        {
            get { return _CatalogEntries; }
        }

        public virtual CatalogCategoriesEntity CatalogCategoryParent
        {
            get { return _CatalogCategoryParent; }
            set { _CatalogCategoryParent = value; }
        }

        public virtual String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public virtual ProductCatalogsEntity ProductCatalog
        {
            get { return _ProductCatalog; }
            set { _ProductCatalog = value; }
        }
    }

    [Serializable]
    public class ProductTypesEntity
    {
        private object _Id;
        private String _Name;
        private readonly ISet<CatalogEntriesEntity> _CatalogEntries = new HashedSet<CatalogEntriesEntity>();

        public virtual object Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public virtual String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public virtual ISet<CatalogEntriesEntity> CatalogEntries
        {
            get { return _CatalogEntries; }
        }
    }

    [Serializable]
    public class ProductCatalogsEntity
    {
        private object _Id;
        private String _Name;
        private ProductCatalogsEntity _ProductCatalogParent;

        public virtual object Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public virtual String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public virtual ProductCatalogsEntity ProductCatalogParent
        {
            get { return _ProductCatalogParent; }
            set { _ProductCatalogParent = value; }
        }
    }
}
