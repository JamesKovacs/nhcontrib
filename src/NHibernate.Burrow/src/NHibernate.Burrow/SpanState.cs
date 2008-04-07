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

        public static void UpdateToHttpContext() {
            HttpContext c = HttpContext.Current;
            if(c == null) return;
            foreach (SpanState state in CurrentStates()) 
                state.Strategy.UpdateSpanStates(c, state);
            
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

     

        public void AddOverspanStateToResponse(Control c)
        {
            Strategy.AddOverspanStateWhenRendering(c, this);
        }
    }
}