using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property|AttributeTargets.Method)]
    public class AuditOverrideAttribute:Attribute
    {
	    /**
	     * @return <strong>Required</strong> Name of the field (or property) whose mapping
	     * is being overridden.
	     */
	    public String Name;

	    /**
	     * @return Indicates if the field (or property) is audited; defaults to {@code true}.
	     */
	    public bool IsAudited = true;

	    /**
	     * @return New {@link AuditJoinTable} used for this field (or property). Its value
	     * is ignored if {@link #isAudited()} equals to {@code false}.
	     */
        //ORIG: 	AuditJoinTable auditJoinTable() default @AuditJoinTable;
        //TODO Simon - check if compatible ("default" with "= new")
	    public AuditJoinTableAttribute AuditJoinTable = new AuditJoinTableAttribute();
    }
}
