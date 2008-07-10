using System;
using System.Collections.Generic;

namespace NHibernate.Linq.Tests.Entities
{
    public class Timesheet
    {
        public virtual int Id { get; set; }
        public virtual DateTime SubmittedDate { get; set; }
        public virtual bool Submitted { get; set; }
        public virtual IList<TimesheetEntry> Entries { get; set; }
    }

    public class TimesheetEntry
    {
        public virtual int Id { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual int NumberOfHours { get; set; }
    }
}
