namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class GeneratedValueAttribute : Attribute
    {
        private GenerationType strategy = GenerationType.Auto;

        private string generator = string.Empty;

        public string Generator
        {
            get { return generator; }
            set { generator = value; }
        }

        public GenerationType Strategy
        {
            get { return strategy; }
            set { strategy = value; }
        }
    }
}
