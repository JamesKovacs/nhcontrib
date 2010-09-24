using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NHibernate.Envers.Exceptions
{
/**
* @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
*/
    public class AuditException : HibernateException {
	private static long serialVersionUID = 4306480965630972168L;

	public AuditException(String message) : base(message)
    {
    }

    public AuditException(String message, System.Exception innerException) 
    : base(message, innerException)
    {
    }

    public AuditException(System.Exception innerException)
    :base(innerException)
    {
    }
}
}
