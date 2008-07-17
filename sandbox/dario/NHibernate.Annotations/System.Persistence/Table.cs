namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class Table : Attribute, INameable
    {
        private string name;

        //string catalog = string.Empty;
        //string schema =string.Empty;
        //UniqueConstraint[] uniqueConstraints;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}