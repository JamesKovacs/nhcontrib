namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Transient : Attribute
    {
    }
}