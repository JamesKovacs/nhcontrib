namespace System.Persistence
{
    public class InheritanceAttribute : Attribute
    {
        public InheritanceType? Strategy { get; set; }
    }
}