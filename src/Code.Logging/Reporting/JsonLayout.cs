using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using System.IO;
using Newtonsoft.Json;

namespace Code.Logging.Reporting
{
    public class JsonLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            //TODO: da decidere il tipo di oggetto
            writer.WriteLine(JsonConvert.SerializeObject(loggingEvent).Replace("\n","\\n").Replace("\r", "\\r").Replace("\t", "\\t"));
        }
    }
}
