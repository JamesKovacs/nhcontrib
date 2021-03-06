﻿using System;

namespace NHibernate.Shards.Query
{
	public class SetCacheRegionEvent : IQueryEvent
    {
        private readonly string cacheRegion;

        public SetCacheRegionEvent(String cacheRegion)
        {
            this.cacheRegion = cacheRegion;
        }

        public void OnEvent(IQuery query)
        {
            query.SetCacheRegion(cacheRegion);
        }

    }
}
