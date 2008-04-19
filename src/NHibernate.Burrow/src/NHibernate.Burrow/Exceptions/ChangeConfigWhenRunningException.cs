namespace NHibernate.Burrow.Exceptions
{
    public class ChangeConfigWhenRunningException : BurrowException
    {
        public ChangeConfigWhenRunningException() : base() {}
        public ChangeConfigWhenRunningException(string msg) : base(msg) {}
    }
}