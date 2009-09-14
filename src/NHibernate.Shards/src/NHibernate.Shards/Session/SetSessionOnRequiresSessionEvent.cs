namespace NHibernate.Shards.Session
{
	public class SetSessionOnRequiresSessionEvent : IOpenSessionEvent
	{
	    private readonly IRequiresSession requiresSession;

        public SetSessionOnRequiresSessionEvent(IRequiresSession requiresSession)
        {
            this.requiresSession = requiresSession;    
        }
		#region IOpenSessionEvent Members

		public void OnOpenSession(ISession session)
		{
		    requiresSession.SetSession(session);
		}

		#endregion
	}
}