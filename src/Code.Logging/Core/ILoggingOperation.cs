using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Code.Logging.Core
{
    public interface ILoggingOperation : IDisposable, ILog
    {
        string Id { get; }
        ILoggingOperation AddProperty(string name, string value);
        ILoggingOperation AddProperty(string name, object value);

        IControlledLogTrace TraceStart([CallerMemberName] string memberName = "unknownMethod");

        ILoggingOperation Wrap(Action action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null);
        T Wrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null);
        ILoggingOperation SinkWrap(Action action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null);
        T SinkWrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null);
    }
}
