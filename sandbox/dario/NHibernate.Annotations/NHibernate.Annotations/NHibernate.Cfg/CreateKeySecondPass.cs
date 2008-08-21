using System;
using System.Collections.Generic;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
	public class CreateKeySecondPass 
	{
		private RootClass rootClass;
		private JoinedSubclass joinedSubClass;
        private Dialect.Dialect dialect;

		public CreateKeySecondPass(RootClass rootClass)
		{
			this.rootClass = rootClass;
		}

		public CreateKeySecondPass(JoinedSubclass joinedSubClass)
		{
			this.joinedSubClass = joinedSubClass;
		}

	    public CreateKeySecondPass(Dialect.Dialect dialect)
	    {
	        this.dialect = dialect;
	    }

        public void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses)
		{
			if (rootClass != null)
			{
			    rootClass.CreatePrimaryKey(dialect);
			}
			else if (joinedSubClass != null)
			{
			    joinedSubClass.CreatePrimaryKey(dialect);
			    joinedSubClass.CreateForeignKey();
			}
			else
			{
				throw new Exception("rootClass and joinedSubClass are null");
			}
		}
	}
}