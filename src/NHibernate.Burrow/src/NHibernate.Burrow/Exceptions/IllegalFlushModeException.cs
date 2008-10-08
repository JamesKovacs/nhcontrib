using System;

namespace NHibernate.Burrow.Exceptions {
	public class IllegalFlushModeException : BurrowException {
		public IllegalFlushModeException() : base() {}
		public IllegalFlushModeException(string msg) : base(msg) {}
	}
}