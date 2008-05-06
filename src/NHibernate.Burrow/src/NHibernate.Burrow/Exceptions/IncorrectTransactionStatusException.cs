using System;

namespace NHibernate.Burrow.Exceptions
{
    public class IncorrectTransactionStatusException : BurrowException
    {
        public IncorrectTransactionStatusException() : base() {}
        public IncorrectTransactionStatusException(string msg) : base(msg) {}
    }
}