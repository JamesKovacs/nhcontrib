namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IdClassAttribute : Attribute
    {
        public Type Type { get; set;}
    }
}