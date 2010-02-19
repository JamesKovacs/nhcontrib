using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NHibernate.Tool.Db2hbm
{
    abstract class AlterActionBase
    {
        protected string Name { get; set; }
        public AlterActionBase(string name)
        {
            Name = name;
        }
        protected static AlterActionBase CreateSet(string name, string value)
        {
            return new SetAction(name,value);
        }
        protected static AlterActionBase CreateRemove(string name)
        {
            return new RemoveAction(name);
        }
        public abstract void Alter(object target);

        internal static AlterActionBase[] Create(cfg.alter a)
        {
            List<AlterActionBase> ret = new List<AlterActionBase>();
            if (a.remove != null)
            {
                foreach (var remove in a.remove)
                {
                    ret.Add(CreateRemove(remove.name));
                }
            }
            if (a.set != null)
            {
                foreach (var set in a.set)
                {
                    ret.Add(CreateSet(set.name,set.value));
                }
            }
            if (a.metaadd != null )
            {
                foreach (var ma in a.metaadd)
                { 
                    ret.Add(new MetaAddAction(ma.attribute,ma.Value,ma.inheritSpecified?ma.inherit:false));
                }
            }
            return ret.ToArray();
        }
    }

    class SetAction : AlterActionBase
    {
        protected string Value { get; set; }
        public SetAction(string name, string value)
            :base(name)
        {
            Value = value;
        }
        public override void Alter(object target)
        {
            PropertyInfo pi = target.GetType().GetProperty(Name);
            if (null != pi && pi.CanWrite)
            {
                PropertyInfo piSpec = target.GetType().GetProperty(Name + "Specified");
                if (null != piSpec)
                {
                    piSpec.SetValue(target, true, null);
                }
                if (!pi.PropertyType.IsEnum)
                {
                    pi.SetValue(target, Convert.ChangeType(Value, pi.PropertyType), null);
                }
                else
                {
                    object valEnum = Enum.Parse(pi.PropertyType, Value);
                    pi.SetValue(target, valEnum, null);
                }
            }
        }
    }
    class RemoveAction : AlterActionBase
    {
        public RemoveAction(string name)
            : base(name)
        {
        }
        public override void Alter(object target)
        {
            PropertyInfo pi = target.GetType().GetProperty(Name);
            if (null != pi && pi.CanWrite)
            {
                PropertyInfo piSpec = target.GetType().GetProperty(Name + "Specified");
                if (null != piSpec)
                {
                    piSpec.SetValue(target, false, null);
                }
                else
                {
                    pi.SetValue(target, null, null);
                }
            }
        }
    }
    class MetaAddAction : AlterActionBase
    {
        string text;
        bool inherit;
        public MetaAddAction(string name,string text,bool inherit)
            : base(name)
        {
            this.text = text;
            this.inherit = inherit;
        }
        public override void Alter(object target)
        {
            PropertyInfo pi = target.GetType().GetProperty("meta");
            if (null != pi)
            {
                if (pi.PropertyType == typeof(meta[]) && pi.CanRead && pi.CanWrite)
                {
                    List<meta> metas = new List<meta>();
                    var existing = pi.GetValue(target, null) as meta[];
                    if (null != existing)
                        metas.AddRange(existing);
                    metas.Add(new meta() { attribute=Name,inherit=inherit,Text=text.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries) });
                    pi.SetValue(target, metas.ToArray(), null);
                }
            }
        }
    }
}
