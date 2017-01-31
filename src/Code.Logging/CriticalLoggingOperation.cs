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
    public class CriticalLoggingOperation : AbstractRuntimeEnforcedLoggingOperation
    {
        #region nested types

        private class ControlledLogTrace : AbstractControlledLogTrace
        {
            public ControlledLogTrace(ILog logger, string opId, [CallerMemberName] string memberName = "unknownMethod")
                : base(logger, opId, memberName)
            { }

            protected override void LogWrapAlways(string message)
            {
                Info(message);
            }

            protected override void LogWrapBegin(string message)
            {
                Info(message);
            }

            protected override void LogWrapDone(string message)
            {
                Info(message);
            }

            protected override void LogWrapFail(string message, Exception e)
            {
                Error(message, e);
            }
        }

        #endregion
        
        public CriticalLoggingOperation(ILog logger) 
            : base(logger)
        {
        }

        public override IControlledLogTrace TraceStart([CallerMemberName] string memberName = "unknownMethod")
        {
            return new ControlledLogTrace(this, NextWrapId, memberName);
        }

        protected override void LogWrapAlways(string message)
        {
            Info(message);
        }

        protected override void LogWrapBegin(string message)
        {
            Info(message);
        }

        protected override void LogWrapDone(string message)
        {
            Info(message);
        }

        protected override void LogWrapFail(string message, Exception e)
        {
            Error(message, e);
        }
    }
}
