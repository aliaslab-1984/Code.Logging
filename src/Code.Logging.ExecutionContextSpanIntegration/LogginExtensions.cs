using CodeProbe.ExecutionControl;
using Code.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Logging.ExecutionContextSpanIntegration
{
    public static class LogginExtensions
    {
        public static ILoggingOperation SpanContext(this ILoggingOperation ext)
        {
            if(ExecutionControlManager.Current!=null)
            {
                foreach (string key in ExecutionControlManager.Current.GetCtxKeys())
                {
                    ext.AddProperty($"remote-{key}", ExecutionControlManager.Current.GetCtxValue(key));
                }
            }

            return ext;
        }

        public static IControlledLogTrace SpanContext(this IControlledLogTrace ext)
        {
            if (ExecutionControlManager.Current != null)
            {
                foreach (string key in ExecutionControlManager.Current.GetCtxKeys())
                {
                    ext.AddProperty($"remote-{key}", ExecutionControlManager.Current.GetCtxValue(key));
                }
            }

            return ext;
        }
    }
}
