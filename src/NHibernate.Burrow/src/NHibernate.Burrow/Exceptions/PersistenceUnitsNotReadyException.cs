using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Exceptions
{
    public class PersistenceUnitsNotReadyException : BurrowException
    {
        public PersistenceUnitsNotReadyException() :base("PersistenceUnits is not ready - either the environment is not started yet or there are no persistence unit set in the config file")
        {
        }

        public PersistenceUnitsNotReadyException(string msg) : base(msg)
        {
        }

    }
}
