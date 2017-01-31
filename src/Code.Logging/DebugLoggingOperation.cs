using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Code.Logging.Core;
using log4net;
using System.Threading;

namespace Code.Logging
{
    public class DebugLoggingOperation : AbstractRuntimeEnforcedLoggingOperation
    {
        #region nested types

        private class ControlledLogTrace : AbstractControlledLogTrace
        {
            public ControlledLogTrace(ILog logger, string opId, [CallerMemberName] string memberName = "unknownMethod") 
                : base(logger, opId, memberName)
            {}

            protected override void LogWrapAlways(string message)
            {
                Debug(message);
            }

            protected override void LogWrapBegin(string message)
            {
                Debug(message);
            }

            protected override void LogWrapDone(string message)
            {
                Debug(message);
            }

            protected override void LogWrapFail(string message, Exception e)
            {
                Debug(message, e);
            }
        }

        #endregion
        
        public DebugLoggingOperation(ILog logger) 
            : base(logger)
        {
        }
        
        public override IControlledLogTrace TraceStart([CallerMemberName] string memberName = "unknownMethod")
        {
            return new ControlledLogTrace(this, NextWrapId, memberName);
        }

        protected override void LogWrapAlways(string message)
        {
            Debug(message);
        }

        protected override void LogWrapBegin(string message)
        {
            Debug(message);
        }

        protected override void LogWrapDone(string message)
        {
            Debug(message);
        }

        protected override void LogWrapFail(string message, Exception e)
        {
            Debug(message, e);
        }
    }
}
