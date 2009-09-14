namespace NHibernate.Shards.Session
{
	public class EnableFilterOpenSessionEvent : IOpenSessionEvent
	{
	    private string filterName;

        public EnableFilterOpenSessionEvent(string filterName)
        {
            this.filterName = filterName;
        }

		public void OnOpenSession(ISession session)
		{
		    session.EnableFilter(filterName);
		}
	}
}
