using System;
using Iesi.Collections;
using log4net;
using NHibernate.Burrow.AppBlock.SoftDelete;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Impl;
using NHibernate.Persister.Entity;

namespace NHibernate.Burrow.AppBlock.SoftDelete
{
    /// <summary>
    /// Allows for entities to be "SoftDeleted" by implementing ISoftDelete and registering this event listener. 
    /// </summary>
    /// <example>
    /// <code>
    /// Configuration cfg = new Configuration().Configure();
    /// cfg.EventListeners.DeleteEventListeners = new IDeleteEventListener[] { new SoftDeleteEventListener() };
    /// </code>
    /// </example>
    public class SoftDeleteEventListener : DefaultDeleteEventListener
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SoftDeleteEventListener));

        protected override void DeleteEntity(IEventSource session, object entity, EntityEntry entityEntry, bool isCascadeDeleteEnabled,
                                             IEntityPersister persister, ISet transientEntities)
        {
            if (!(entity is ISoftDelete))
            {
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
                return;
            }

            ISoftDelete softDeleteEntity = (ISoftDelete)entity;
            softDeleteEntity.Deleted = true;
            softDeleteEntity.DeleteDate = DateTime.Now;

            if (log.IsDebugEnabled)
            {
                log.Debug("temporal deleting " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory));
            }

            CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
            CascadeAfterDelete(session, persister, entity, transientEntities);
        }
    }
}
