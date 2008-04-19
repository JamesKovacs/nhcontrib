using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal abstract class StatefulFieldProcessor
    {
        private static readonly IDictionary<Type, IDictionary<FieldInfo, StatefulField>> fieldInfoCache =
            new Dictionary<Type, IDictionary<FieldInfo, StatefulField>>();

        private readonly Control ctl;
        private readonly Control globalPlaceHolder;
        private readonly string stateKeyPrefix;
        private readonly StateBag states;
        protected IDictionary<FieldInfo, StatefulField> statefulfields;

        public StatefulFieldProcessor(Control c, Control globalPlaceHolder)
        {
            ctl = c;
            this.globalPlaceHolder = globalPlaceHolder;
            states = new StateBag();
            stateKeyPrefix = "NHibernate.Burrow.WebUtil.StatefulFieldsHolder" + "_" + ctl.UniqueID + "_";
            statefulfields = GetStatefulFields();
            if (statefulfields.Count > 0)
            {
                NameValueCollection form = c.Page.Request.Form;
                foreach (string key in form.AllKeys)
                {
                    if (key.Contains(stateKeyPrefix))
                    {
                        states.Add(key.Replace(stateKeyPrefix, ""), Deserialize(form[key]));
                    }
                }
            }
        }

        protected StateBag States
        {
            get { return states; }
        }

        public Control Control
        {
            get { return ctl; }
        }

        private StateBag GetViewState()
        {
            PropertyInfo pi =
                ctl.GetType().GetProperty("ViewState",
                                          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return (StateBag) pi.GetValue(ctl, new object[0]);
        }

        public void Process()
        {
            //if (Control is UserControl || Control is Page) // don't limit
            DoProcess();
            foreach (Control control in Control.Controls)
            {
                CreateSubProcessor(control, globalPlaceHolder).Process();
            }
        }

        protected abstract StatefulFieldProcessor CreateSubProcessor(Control c, Control globalPlaceHolder);

        protected abstract void DoProcess();

        /// <summary>
        /// Get the FieldInfo - Attribute pairs that have the customer attribute of type <typeparamref name="AT"/> 
        /// </summary>
        /// <typeparam name="AT"></typeparam>
        /// <returns></returns>
        protected IDictionary<FieldInfo, AT> GetFieldInfo<AT>() where AT : Attribute
        {
            IDictionary<FieldInfo, AT> retVal = new Dictionary<FieldInfo, AT>();
            foreach (FieldInfo fi in GetFields())
            {
                foreach (AT a in Attribute.GetCustomAttributes(fi, typeof (AT)))
                {
                    retVal.Add(fi, a);
                }
            }
            return retVal;
        }

        protected IDictionary<FieldInfo, StatefulField> GetStatefulFields()
        {
            IDictionary<FieldInfo, StatefulField> retVal;
            Type controlType = Control.GetType();
            if (!fieldInfoCache.TryGetValue(controlType, out retVal))
            {
                fieldInfoCache[controlType] = retVal = GetFieldInfo<StatefulField>();
            }
            return retVal;
        }

        private FieldInfo[] GetFields()
        {
            return Control.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        protected void SaveStates()
        {
            foreach (DictionaryEntry state in states)
            {
                HiddenField hf = new HiddenField();
                hf.ID = stateKeyPrefix + (string) state.Key;
                hf.Value = Serialize(state.Value != null ? ((StateItem) state.Value).Value : null);
                globalPlaceHolder.Controls.Add(hf);
            }
        }

        private object Deserialize(string value)
        {
            LosFormatter lf = new LosFormatter();
            return lf.Deserialize(value);
        }

        private string Serialize(object val)
        {
            LosFormatter lf = new LosFormatter();
            TextWriter tw = new StringWriter();
            lf.Serialize(tw, val);
            return tw.ToString();
        }
    }
}