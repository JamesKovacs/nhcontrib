using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil
{
    internal class StatefulFieldLoader : StatefulFieldProcessor
    {
        public StatefulFieldLoader(Control c, Control globalPlaceHolder) : base(c, globalPlaceHolder) {}

        protected override void DoProcess()
        {
            foreach (KeyValuePair<FieldInfo, StatefulField> p in statefulfields)
            {
                StatefulField vsf = p.Value;
                object toSet = States[p.Key.Name];
                if (vsf.Interceptor != null)
                {
                    toSet = vsf.Interceptor.OnLoad(toSet);
                }
                p.Key.SetValue(Control, toSet);
            }
        }

        protected override StatefulFieldProcessor CreateSubProcessor(Control c, Control globalPlaceHolder)
        {
            return new StatefulFieldLoader(c, globalPlaceHolder);
        }
    }

    internal class StatefulFieldSaver : StatefulFieldProcessor
    {
        public StatefulFieldSaver(Control c, Control globalPlaceHolder) : base(c, globalPlaceHolder) {}

        protected override void DoProcess()
        {
            foreach (KeyValuePair<FieldInfo, StatefulField> p in statefulfields)
            {
                StatefulField vsf = p.Value;
                object toSave = p.Key.GetValue(Control);
                object objectInViewState = States[p.Key.Name];
                if (vsf.Interceptor != null)
                {
                    toSave = vsf.Interceptor.OnSave(toSave, objectInViewState);
                }
                States[p.Key.Name] = toSave;
            }
            if (statefulfields.Count > 0)
            {
                SaveStates();
            }
        }

        protected override StatefulFieldProcessor CreateSubProcessor(Control c, Control globalPlaceHolder)
        {
            return new StatefulFieldSaver(c, globalPlaceHolder);
        }
    }
}