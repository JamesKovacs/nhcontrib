using System;

namespace NHibernate.Burrow.Exceptions
{
    public class UnableToGetPersistenceUnitException : Exception
    {
        public UnableToGetPersistenceUnitException() : base() {}
        public UnableToGetPersistenceUnitException(string msg) : base(msg) {}
    }
}