using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Code.Logging.WebViewer.Controllers
{
    [RoutePrefix("api/LogAppender")]
    public class LogAppenderController : ApiController
    {
        // GET api/LogAppender/Events
        [Route("Events")]
        [HttpGet]
        public IEnumerable<object> Events([FromUri]string LogSourceIdentity, [FromUri]string fromDate = null, [FromUri]string toDate = null)
        {
            DateTime? _fromDate = fromDate!=null?JsonConvert.DeserializeObject<DateTime?>(fromDate):null;
            DateTime? _toDate = toDate!=null?JsonConvert.DeserializeObject<DateTime?>(toDate):null;

            return Global.LoggingEventRepository.Events(LogSourceIdentity, _fromDate, _toDate)??new LoggingEvent[0];
        }

        // GET api/LogAppender/Sources
        [Route("Sources")]
        [HttpGet]
        public IEnumerable<string> Sources()
        {
            return Global.LoggingEventRepository.Sources();
        }

        // POST api/LogAppender?LogSourceIdentity={LogSourceIdentity}
        [Route("Append")]
        [HttpPost]
        public void Append([FromUri]string LogSourceIdentity)
        {
            HttpContent requestContent = Request.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            object[] evts = JsonConvert.DeserializeObject<object[]>(jsonContent);
            if(evts!=null)
            {
                foreach (object evt in evts)
                {
                    Global.LoggingEventRepository.Put(LogSourceIdentity, evt);
                }
            }
        }

        // POST api/LogAppender/ClearSource?LogSourceIdentity={LogSourceIdentity}
        [Route("ClearSource")]
        [HttpPost]
        public void ClearSource([FromUri]string LogSourceIdentity)
        {
            Global.LoggingEventRepository.Clear(LogSourceIdentity);
        }
    }
}