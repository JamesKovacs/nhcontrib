using System;

namespace NHibernate.Burrow.WebUtil.Exceptions {
	public class DuplicatedGlobalHolderException : BurrowWebUtilException
	{
		public DuplicatedGlobalHolderException() : base() {}
		public DuplicatedGlobalHolderException(string msg) : base(msg) {}
	}
}