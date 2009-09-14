using NHibernate.Transform;

namespace NHibernate.Shards.Criteria
{
    class SetResultTransformerEvent:ICriteriaEvent
    {

        private readonly IResultTransformer resultTransformer;

        public SetResultTransformerEvent(IResultTransformer resultTransformer)
        {
            this.resultTransformer = resultTransformer;
        }

        #region Implementation of ICriteriaEvent

        public void OnEvent(ICriteria crit)
        {
            crit.SetResultTransformer(resultTransformer);
        }

        #endregion
    }
}
