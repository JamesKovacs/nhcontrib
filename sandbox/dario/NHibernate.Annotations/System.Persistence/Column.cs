namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Column : Attribute, INameable
    {
        private string columnDefinition = string.Empty;
        private bool insertable = true;
        private int length = 255;
        private string name = string.Empty;
        private bool nullable = true;
        private int precision; // decimal precision
        private int scale; // decimal scale
        private string table;
        private bool unique;
        private bool updatable = true;

        public Column()
        {
        }

        public Column(string name)
        {
            this.name = name;
        }

        public bool Unique
        {
            get { return unique; }
            set { unique = value; }
        }

        public bool Nullable
        {
            get { return nullable; }
            set { nullable = value; }
        }

        public bool Insertable
        {
            get { return insertable; }
            set { insertable = value; }
        }

        public bool Updatable
        {
            get { return updatable; }
            set { updatable = value; }
        }

        public string ColumnDefinition
        {
            get { return columnDefinition; }
            set { columnDefinition = value; }
        }

        public string Table
        {
            get { return table; }
            set { table = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public int Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        #region INameable Members

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion
    }
}