using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Logging.Core
{
    public interface IControlledLogTrace : IDisposable, ILog
    {
        string Id { get; }

        IControlledLogTrace AddProperty(string name, string value);
        IControlledLogTrace AddProperty(string name, object value);
    }
}
