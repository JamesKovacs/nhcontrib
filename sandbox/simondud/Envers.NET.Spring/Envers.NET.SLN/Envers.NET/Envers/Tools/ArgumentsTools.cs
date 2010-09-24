using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Tools
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public class ArgumentsTools
    {
        public static void CheckNotNull(Object o, String paramName)
        {
            if (o == null)
            {
                throw new Exception(paramName + " cannot be null.");
            }
        }

        public static void CheckPositive(long i, String paramName)
        {
            if (i <= 0)
            {
                throw new Exception(paramName + " has to be greater than 0.");
            }
        }
    }
}
