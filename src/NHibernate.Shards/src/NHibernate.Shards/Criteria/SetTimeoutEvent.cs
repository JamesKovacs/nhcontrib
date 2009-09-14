namespace NHibernate.Shards.Criteria
{
    class SetTimeoutEvent:ICriteriaEvent
    {
        private readonly int timeout;

        public SetTimeoutEvent(int timeout)
        {
            this.timeout = timeout;
        }

        #region Implementation of ICriteriaEvent

        public void OnEvent(ICriteria crit)
        {
            crit.SetTimeout(timeout);
        }

        #endregion
    }
}
