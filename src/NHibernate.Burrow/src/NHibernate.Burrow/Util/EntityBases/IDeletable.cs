namespace NHibernate.Burrow.Util.EntityBases
{
    /// <summary>
    /// an interface for entities that has Deletion logic in itself
    /// </summary>
    public interface IDeletable
    {
        bool Delete();
    }
}