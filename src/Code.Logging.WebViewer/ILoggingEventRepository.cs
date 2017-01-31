using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Logging.WebViewer
{
    public interface ILoggingEventRepository
    {
        void Put(string logSourceId, object value);

        void Clear(string logSourceId);

        IEnumerable<string> Sources();

        IEnumerable<object> Events(string logSourceId);
        IEnumerable<object> Events(string logSourceId, DateTime? fromDate, DateTime? toDate);
    }
}
