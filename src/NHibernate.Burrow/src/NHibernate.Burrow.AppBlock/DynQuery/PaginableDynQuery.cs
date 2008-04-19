using System;
using NHibernate.Burrow.AppBlock.Pagination;

namespace NHibernate.Burrow.AppBlock.DynQuery {
    public abstract class PaginableDynQuery<T> : AbstractPaginableRowsCounterQuery<T> {
        private readonly DetachedDynQuery query;

        public PaginableDynQuery(DetachedDynQuery query) {
            if (query == null)
                throw new ArgumentNullException("query");

            this.query = query;
        }

        protected override IDetachedQuery DetachedQuery {
            get { return query; }
        }

        protected override IDetachedQuery GetRowCountQuery() {
            return query.TransformToRowCount();
        }
    }
}