namespace NHibernate.Burrow.AppBlock.DynQuery
{
    /// <summary>
    /// Interface for Dynamic query clause.
    /// </summary>
    public interface IDynClause : IQueryPart
    {
        /// <summary>
        /// The clause has some meber or not?
        /// </summary>
        bool HasMembers { get; }
    }
}