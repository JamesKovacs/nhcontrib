namespace NHibernate.Burrow.AppBlock.Test.CriterionTest
{
    public class Simple
    {
#pragma warning disable 649
        private int id;
#pragma warning restore 649

        private string name;

        public virtual int Id
        {
            get { return id; }
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}