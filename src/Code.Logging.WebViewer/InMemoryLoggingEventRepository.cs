using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Core;
using System.Runtime.Caching;

namespace Code.Logging.WebViewer
{
    public class InMemoryLoggingEventRepository : ILoggingEventRepository
    {
        protected MemoryCache _repo = new MemoryCache("loggingEvents");

        public void Clear(string logSourceId)
        {
            if (_repo.Get(logSourceId) != null)
                _repo.Remove(logSourceId);
        }

        public IEnumerable<object> Events(string logSourceId)
        {
            return (List<object>)_repo.Get(logSourceId);
        }

        public IEnumerable<object> Events(string logSourceId, DateTime? fromDate, DateTime? toDate)
        {
            return Events(logSourceId)?.Where(p => ((DateTime)((Newtonsoft.Json.Linq.JObject)p)["TimeStamp"]).ToUniversalTime() > (fromDate??DateTime.MinValue).ToUniversalTime() && ((DateTime)((Newtonsoft.Json.Linq.JObject)p)["TimeStamp"]).ToUniversalTime() < (toDate??DateTime.MaxValue).ToUniversalTime());
        }

        public void Put(string logSourceId, object value)
        {
            if (_repo.Get(logSourceId) == null)
                _repo.Add(new CacheItem(logSourceId, new List<object>()), new CacheItemPolicy() { SlidingExpiration=TimeSpan.FromHours(1) });
            ((List<object>)_repo.Get(logSourceId)).Add(value);
        }

        public IEnumerable<string> Sources()
        {
            return _repo.Select(p => p.Key);
        }
    }
}