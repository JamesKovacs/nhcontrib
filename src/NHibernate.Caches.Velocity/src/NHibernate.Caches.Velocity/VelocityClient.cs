#region License

//Microsoft Public License (Ms-PL)
//
//This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
//
//1. Definitions
//
//The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
//
//A "contribution" is the original software, or any additions or changes to the software.
//
//A "contributor" is any person that distributes its contribution under this license.
//
//"Licensed patents" are a contributor's patent claims that read directly on its contribution.
//
//2. Grant of Rights
//
//(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
//
//(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
//
//3. Conditions and Limitations
//
//(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
//
//(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
//
//(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
//
//(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
//
//(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using log4net;

using System.Data.Caching;

using NHibernate.Cache;

namespace NHibernate.Caches.Velocity
{
    public class VelocityClient : ICache
    {
        #region Fields

        private const string CacheName = "nhibernate";
        private static readonly ILog _log;
        private string _region;
        private System.Data.Caching.Cache _cache;

        #endregion

        #region Constructor

        static VelocityClient()
		{
			_log = LogManager.GetLogger(typeof(VelocityClient));
		}

		public VelocityClient()
			: this("nhibernate", null)
		{
		}

        public VelocityClient(string regionName)
			: this(regionName, null)
		{
		}

        public VelocityClient(string regionName, IDictionary<string, string> properties)
		{
            _region = regionName.GetHashCode().ToString(); //because the region name length is limited
            System.Data.Caching.CacheFactory cacheCluster = new System.Data.Caching.CacheFactory();
            _cache = cacheCluster.GetCache(CacheName);
            try
            {
                _cache.CreateRegion(_region, true);
            }
            catch (System.Data.Caching.CacheException ce)
            {}
            

			if (properties != null)
			{
            }
        }

        #endregion

        #region ICache Members

        public object Get(object key)
        {
            if (key == null)
            {
                return null;
            }
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("fetching object {0} from the cache", key);
            }

            CacheItemVersion version = null;
            return _cache.Get(_region, key.ToString(), ref version);
        }

        public void Put(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "null key not allowed");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value", "null value not allowed");
            }

            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("setting value for item {0}", key);
            }

            _cache.Put(_region, key.ToString(), value, null, null);
        }

        public void Remove(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("removing item {0}", key);
            }

            if (Get(key.ToString()) != null)
                _cache.Remove(_region, key.ToString());
        }

        public void Clear()
        {
            _cache.ClearRegion(_region);
        }

        public void Destroy()
        {
            Clear();
        }

        public void Lock(object key)
        {
            LockHandle lockHandle = new LockHandle();
            if (Get(key.ToString()) != null)
            {
                try
                {
                    _cache.GetAndLock(_region, key.ToString(), TimeSpan.FromMilliseconds(Timeout), out lockHandle);
                }
                catch (System.Data.Caching.CacheException)
                { }
            }
        }

        public void Unlock(object key)
        {
            LockHandle lockHandle = new LockHandle();
            if (Get(key.ToString()) != null)
            {
                try
                {
                    _cache.Unlock(_region, key.ToString(), lockHandle);
                }
                catch (System.Data.Caching.CacheException)
                {}
            }
        }

        public long NextTimestamp()
        {
            return Timestamper.Next();
        }

        public int Timeout
        {
            get { return Timestamper.OneMs * 60000; } // 60 seconds
        }

        public string RegionName
        {
            get { return _region; }
        }

        #endregion
    }
}
