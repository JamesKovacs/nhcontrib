using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.JetDriver.Tests.Entities
{
    public class DecimalEntity
    {
        public virtual int Id { get; set; }
        public virtual decimal SimpleDecimal { get; set; }
        public virtual double SimpleDouble { get; set; }
        public virtual decimal? NullableDecimal { get; set; }
        public virtual double? NullableDouble { get; set; }
    }
}