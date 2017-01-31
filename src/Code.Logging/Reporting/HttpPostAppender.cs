using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace Code.Logging.Reporting
{
    public class HttpPostAppender : BufferingAppenderSkeleton
    {
        public HttpPostAppender()
            :base()
        {
            FlushTimeout = TimeSpan.FromSeconds(10);
        }

        protected Timer _timedFlush;

        protected string _postUrl;
        public string PostUrl
        {
            get
            {
                UriBuilder bld = new UriBuilder(_postUrl);
                Dictionary<string, string> qry = bld.Query?.TrimStart('?').Split('&').Where(p => !string.IsNullOrEmpty(p)).Select(p =>
                {
                    string[] kv = p.Split('=');
                    return new KeyValuePair<string, string>(kv[0], kv.Length > 1 ? kv[1] : "");
                }).ToDictionary(p => p.Key, p => p.Value);

                if (!qry.ContainsKey("LogSourceIdentity"))
                    qry.Add("LogSourceIdentity", HttpUtility.UrlEncode(LogSourceIdentity));
                else
                    qry["LogSourceIdentity"] = LogSourceIdentity;
                bld.Query = string.Join("&", qry.Select(p => $"{p.Key}={p.Value}").ToArray());

                return bld.Uri.ToString();
            }
            set
            {
                _postUrl = value;
            }
        }
        public TimeSpan FlushTimeout { get; set; }
        public string LogSourceIdentity { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            base.Append(loggingEvent);
            if (_timedFlush != null)
                _timedFlush.Dispose();
            _timedFlush = new Timer(s => { Flush(); }, null, (int)FlushTimeout.TotalMilliseconds, Timeout.Infinite);
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            using (HttpClient client = new HttpClient())
            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (LoggingEvent item in events)
                {
                    Layout.Format(writer, item);
                    sb.Append(writer.ToString().Replace("\r","").Replace("\n",""));
                    sb.Append(",");
                    
                    writer.GetStringBuilder().Clear();
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");

                client.PostAsync(PostUrl, new StringContent(sb.ToString())).Wait();
            }
        }
    }
}
