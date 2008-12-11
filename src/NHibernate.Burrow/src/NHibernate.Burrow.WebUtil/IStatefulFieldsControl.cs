using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.WebUtil
{
    public interface IStatefulFieldsControl
    {
        bool IgnoreStatefulFields
        { get;}
    }
}
