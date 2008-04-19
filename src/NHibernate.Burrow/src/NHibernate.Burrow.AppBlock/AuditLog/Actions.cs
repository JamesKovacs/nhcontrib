namespace NHibernate.Burrow.Util.AuditLog
{
    /// <summary>
    /// Repository for Action Names
    /// </summary>
    public sealed class Actions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// 
        /// </summary>
        public const string INSERT = "INSERT";

        /// <summary>
        /// 
        /// </summary>
        public const string INSERT_UPDATE = "INSERT_UPDATE";

        /// <summary>
        /// 
        /// </summary>
        public const string UPDATE = "UPDATE";

        private Actions() {}
    }
}