namespace NHibernate.Shards.Session
{
	public class DisableFilterOpenSessionEvent : IOpenSessionEvent
	{
	    private readonly string filterName;

        public DisableFilterOpenSessionEvent(string filterName)
        {
            this.filterName = filterName;
        }

		#region IOpenSessionEvent Members

		public void OnOpenSession(ISession session)
		{
		    session.DisableFilter(filterName);
		}

		#endregion
	}
}