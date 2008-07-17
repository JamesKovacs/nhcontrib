namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IdClass : Attribute
    {
        private Type type;

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}