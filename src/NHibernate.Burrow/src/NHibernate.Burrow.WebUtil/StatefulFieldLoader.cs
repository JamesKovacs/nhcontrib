using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil {

    public class StatefulFieldLoader : StatefulFieldProcessor
    {
        public StatefulFieldLoader(Control c) : base(c) { }

        protected override void DoProcess()
        {
            foreach (KeyValuePair<FieldInfo, StatefulField> p in GetStatefulFields())
            {
                StatefulField vsf = p.Value;
                object toSet = ViewState[p.Key.Name];
                if (vsf.Interceptor != null)
                    toSet = vsf.Interceptor.OnLoad(toSet);
                p.Key.SetValue(Control, toSet);
            }
        }

        protected override StatefulFieldProcessor CreateSubProcessor(Control c)
        {
            return new StatefulFieldLoader(c);
        }
    }

    public class StatefulFieldSaver : StatefulFieldProcessor
    {
        public StatefulFieldSaver(Control c) : base(c) { }

        protected override void DoProcess()
        {
            foreach (KeyValuePair<FieldInfo, StatefulField> p in GetStatefulFields())
            {
                StatefulField vsf = p.Value;
                object toSave = p.Key.GetValue(Control);
                object objectInViewState = ViewState[p.Key.Name];
                if (vsf.Interceptor != null)
                    toSave = vsf.Interceptor.OnSave(toSave, objectInViewState);
                ViewState[p.Key.Name] = toSave;
            }
        }

        protected override StatefulFieldProcessor CreateSubProcessor(Control c)
        {
            return new StatefulFieldSaver(c);
        }
    }
}