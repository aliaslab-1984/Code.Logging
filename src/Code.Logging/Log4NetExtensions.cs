using Code.Logging.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Logging
{
    public static class Log4NetExtensions
    {
        public static ILoggingOperation DebugOperation(this ILog ext)
        {
            return new DebugLoggingOperation(ext);
        }
        public static ILoggingOperation NormalOperation(this ILog ext)
        {
            return new NormalLoggingOperation(ext);
        }
        public static ILoggingOperation CriticalOperation(this ILog ext)
        {
            return new CriticalLoggingOperation(ext);
        }
    }
}
