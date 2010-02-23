using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cfg;
using System.Text.RegularExpressions;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class EntityException
    {
        Regex entityMatch;
        Action<string,IMappingModel> bindAction;
        List<MemberExceptionBase> memberExceptions = new List<MemberExceptionBase>();
        List<AlterActionBase> alterActions = new List<AlterActionBase>();
        public EntityException(db2hbmconfEntity cfg)
        {
            entityMatch = new Regex(cfg.match, RegexOptions.Compiled);
            if (cfg.exclude == true)
                bindAction = (e, model) => model.RemoveEntity(e);
            else
                bindAction = (e, model) => ApplyExceptions(e, model);
            if (null != cfg.membername)
            {
                foreach (var m in cfg.membername)
                {
                    memberExceptions.Add(MemberExceptionBase.CreateMemberException(m));
                }
            }
            if (null != cfg.membertag)
            {
                foreach (var m in cfg.membertag)
                {
                    memberExceptions.Add(MemberExceptionBase.CreateTagException(m));
                }
            }
            if (null != cfg.alter)
            {
                foreach (var a in cfg.alter)
                {
                    alterActions.AddRange(AlterActionBase.Create(a));
                }
            }
            
            
        }
        public void Apply(@class entity,IMappingModel model)
        {
            if (entityMatch.IsProperMatch(entity.name))
            {
                bindAction(entity.name, model);
            }
        }
        private void ApplyExceptions(string entityName, IMappingModel model)
        {
            foreach (var me in memberExceptions)
            {
                foreach (var v in model.GetPropertyOfEntity(entityName))
                {
                    me.Apply(v,model,p=>model.RemoveProperty(entityName,(property)p));
                }
                foreach (var v in model.GetCollectionsOfEntity(entityName))
                {
                    me.Apply(v,model,c=>model.RemoveCollectionFromEntity(entityName,c));
                }
                foreach (var v in model.GetManyToOnesOfEntity(entityName))
                {
                    me.Apply(v,model,mto=>model.RemoveManyToOne(entityName,(manytoone)mto));
                }
            }
            foreach (var alter in alterActions)
            {
                alter.Alter(model.GetClassFromEntityName(entityName));
            }
        }
    }
}
