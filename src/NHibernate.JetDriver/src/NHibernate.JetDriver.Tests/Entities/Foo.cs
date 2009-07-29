namespace NHibernate.JetDriver.Tests.Entities
{
    public class Foo
    {
        private int moduleId;
        public virtual int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private string module;
        public virtual string Module
        {
            get { return module; }
            set { module = value; }
        }

        private string shortName;
        public virtual string ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        private string longName;
        public virtual string LongName
        {
            get { return longName; }
            set { longName = value; }
        }

        private string description;
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }

        private long moduleValue;
        public virtual long ModuleValue
        {
            get { return moduleValue; }
            set { moduleValue = value;}
        }
    }
}