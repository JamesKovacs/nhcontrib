using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cfg;
using System.Text.RegularExpressions;
using System.Reflection;

namespace NHibernate.Tool.Db2hbm
{
    abstract class MemberExceptionBase
    {
        protected abstract bool IsMatch(object item);
        protected Regex nameRegex;
        bool exclude;
        List<AlterActionBase> alterActions = new List<AlterActionBase>();
        public MemberExceptionBase(string match,tagmodifier cfg)
        {
            nameRegex = new Regex(match, RegexOptions.Compiled);
            exclude = cfg.exclude;
            if (null != cfg.alter)
            {
                alterActions.AddRange(AlterActionBase.Create(cfg.alter));
            }
        }
        public static MemberExceptionBase CreateMemberException(tagmodifier m)
        {
            return new MemberException(m.match, m);
        }

        public static MemberExceptionBase CreateTagException(tagmodifier m)
        {
            return new TagException(m.match, m);
        }

        public void Apply(object member,IMappingModel model, Action<object> removeAction)
        {
            if (IsMatch(member))
            {
                if (exclude)
                {
                    removeAction(member);
                }
                else
                {
                    foreach (var action in alterActions)
                    {
                        action.Alter(member);
                    }
                }
            }
        }
    }
    class MemberException : MemberExceptionBase
    {
        public MemberException(string pattern,tagmodifier cfg)
            :base(pattern,cfg)
        {

        }
        protected override bool IsMatch(object item)
        {
            PropertyInfo pi = item.GetType().GetProperty("name");
            if (null != pi && pi.CanRead)
            {
                string targetName = pi.GetValue(item,null).ToString();
                return nameRegex.IsProperMatch(targetName);
            }
            else
            {
                return false;
            }
        }
    }
    class TagException : MemberExceptionBase
    {
        public TagException(string pattern, tagmodifier cfg)
            :base(pattern.Replace("-",""),cfg)
        {

        }
        protected override bool IsMatch(object item)
        {
            string targetName = item.GetType().Name;
            return nameRegex.IsProperMatch(targetName);
        }
    }
}
