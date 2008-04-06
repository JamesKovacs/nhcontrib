using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {
 
    /// <summary>
    /// Represents a state that can span over multiple requests
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class SpanState {
        public static IList<SpanState> CurrentStates()
        {
            return DomainContext.Current.SpanStates();
        }

        public static void UpdateToHttpContext(bool spanning) {
            HttpContext c = HttpContext.Current;
            if(c == null) return;
            foreach (SpanState state in CurrentStates()) {
                if(spanning)
                    state.Strategy.ImmdiatelyAddSpanStates(c, state);
                else 
                    state.CleanCookies(c);
            }
        }

        private readonly SpanStrategy strategy;
        private readonly string name;
        private readonly string value;

        internal SpanState(string name, string value, SpanStrategy strategy) {
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
              Strategy.CleanStates(this, c);
        }

        public void AddOverspanState(Control c)
        {
            Strategy.AddOverspanStateWhenRendering(c, this);
        }
    }
}