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
    }
}