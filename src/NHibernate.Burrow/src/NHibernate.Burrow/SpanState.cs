using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {
 

    public class SpanState {
        public static IList<SpanState> CurrentStates()
        {
            return DomainContext.Current.SpanStates();
        }

        private readonly SpanStrategy strategy;
        private readonly string name;
        private readonly string value;

        public SpanState(string name, string value, SpanStrategy strategy) {
            this.name = name;
            this.strategy = strategy;
            this.value = value;
        }

        public SpanStrategy Strategy {
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