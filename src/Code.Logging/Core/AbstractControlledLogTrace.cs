using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Code.Logging.Core
{
    internal abstract class AbstractControlledLogTrace : LogDecorator, IControlledLogTrace
    {
        protected bool _disposed = false;
        protected bool _killOnMisuse = false;
        protected Stack<IDisposable> _ctx;

        public string Id { get; protected set; }

        public AbstractControlledLogTrace(ILog logger, string opId, [CallerMemberName] string memberName = "unknownMethod")
            :base(logger)
        {
            Id = opId;
            _ctx = new Stack<IDisposable>();
            _ctx.Push(LogicalThreadContext.Stacks["traceCtx_UUID"].Push(Id));
            _ctx.Push(LogicalThreadContext.Stacks["traceCtx_memberName"].Push(memberName));

            LogWrapBegin("begin");
        }
        ~AbstractControlledLogTrace()
        {
            if (!_disposed)
            {
                if (_killOnMisuse)
                    throw new InvalidOperationException("Illegal use of IControlledLogTrace. The object must be disposed in order to correctly take a sample log.");
                else
                    try
                    {
                        _logger.Fatal("Illegal use of IControlledLogTrace. The object must be disposed in order to correctly take a sample log.");
                    }
                    catch (Exception e) { }

                while (_ctx.Count > 0)
                {
                    _ctx.Pop().Dispose();
                }
            }
        }
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                if (Marshal.GetExceptionPointers() != IntPtr.Zero || Marshal.GetExceptionCode() != 0)
                    LogWrapFail("fail", null);// Marshal.GetExceptionForHR(Marshal.GetExceptionCode(), Marshal.GetExceptionPointers())); //non si può ottenere l'eccezione originale
                else
                    LogWrapDone("done");
                LogWrapAlways("always");

                while (_ctx.Count > 0)
                {
                    _ctx.Pop().Dispose();
                }

                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }
        protected abstract void LogWrapBegin(string message);
        protected abstract void LogWrapDone(string message);
        protected abstract void LogWrapFail(string message, Exception e);
        protected abstract void LogWrapAlways(string message);

        public IControlledLogTrace AddProperty(string name, string value)
        {
            _ctx.Push(LogicalThreadContext.Stacks[name].Push(value));

            return this;
        }
        public IControlledLogTrace AddProperty(string name, object value)
        {
            _ctx.Push(LogicalThreadContext.Stacks[name].Push(JsonConvert.SerializeObject(value)));

            return this;
        }
    }
}
