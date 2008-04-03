using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {
 

    public class OverspanState {
        public static IList<OverspanState> CurrentStates()
        {
            return DomainContext.Current.OverspanStates();
        }

        private readonly OverspanStrategy strategy;
        private readonly string name;
        private readonly string value;

        public OverspanState(string name, string value, OverspanStrategy strategy) {
            this.name = name;
            this.strategy = strategy;
            this.value = value;
        }

        public OverspanStrategy Strategy {
            get { return strategy; }
        }

        public string Name {
            get { return name; }
        }

        public string Value {
            get { return value; }
        }

        public void CleanCookies(HttpContext c)
        {
              Strategy.CleanCookies(this, c);
        }

        public void AddOverspanState(Control c)
        {
            Strategy.AddOverspanState(c, this);
        }
    }
}