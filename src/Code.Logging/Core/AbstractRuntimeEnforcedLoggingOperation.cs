using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Threading;

namespace Code.Logging.Core
{
    public abstract class AbstractRuntimeEnforcedLoggingOperation : LogDecorator, ILoggingOperation
    {
        protected bool _disposed = false;
        protected bool _killOnMisuse = false;
        protected Stack<IDisposable> _ctx;
        protected int _controlledUUID = 0;

        public string Id { get; protected set; }

        protected string NextWrapId { get { return Interlocked.Increment(ref _controlledUUID).ToString(); } }
                
        public AbstractRuntimeEnforcedLoggingOperation(ILog logger)
            :base(logger)
        {
            Id = Guid.NewGuid().ToString();
            _ctx = new Stack<IDisposable>();
            _ctx.Push(LogicalThreadContext.Stacks["logCtx_UUID"].Push(Id));
        }

        ~AbstractRuntimeEnforcedLoggingOperation()
        {
            if (!_disposed)
            {
                if (_killOnMisuse)
                    throw new InvalidOperationException("Illegal use of ILoggingOperation. The object must be disposed in order to correctly take a sample log.");
                else
                    try
                    {
                        _logger.Fatal("Illegal use of ILoggingOperation. The object must be disposed in order to correctly take a sample log.");
                    }
                    catch (Exception e) { }

                while (_ctx.Count>0)
                {
                    _ctx.Pop().Dispose();
                }
            }
        }
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                while (_ctx.Count > 0)
                {
                    _ctx.Pop().Dispose();
                }

                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        public abstract IControlledLogTrace TraceStart([CallerMemberName] string memberName = "unknownMethod");
        
        protected abstract void LogWrapBegin(string message);
        protected abstract void LogWrapDone(string message);
        protected abstract void LogWrapFail(string message, Exception e);
        protected abstract void LogWrapAlways(string message);

        public ILoggingOperation Wrap(Action action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string,string> props = null)
        {
            Stack<IDisposable> disTmp = new Stack<IDisposable>();
            using (LogicalThreadContext.Stacks["wrapCtx_UUID"].Push(NextWrapId))
            using (LogicalThreadContext.Stacks["logCtx_memberName"].Push(memberName))
            {
                try
                {
                    if (props != null)
                        foreach (KeyValuePair<string,string> kv in props)
                            disTmp.Push(LogicalThreadContext.Stacks[kv.Key].Push(kv.Value));

                    LogWrapBegin("begin");
                    action?.Invoke();
                    LogWrapDone("done");
                }
                catch (Exception e)
                {
                    LogWrapFail("fail", e);
                    throw e;
                }
                finally
                {
                    LogWrapAlways("always");

                    while (disTmp.Count > 0)
                        disTmp.Pop().Dispose();
                }

                return this;
            }
        }
        public T Wrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null)
        {
            if (action == null)
                throw new ArgumentNullException("action must have a value.");
            Stack<IDisposable> disTmp = new Stack<IDisposable>();
            using (LogicalThreadContext.Stacks["wrapCtx_UUID"].Push(NextWrapId))
            using (LogicalThreadContext.Stacks["logCtx_memberName"].Push(memberName))
            {
                try
                {
                    if (props != null)
                        foreach (KeyValuePair<string, string> kv in props)
                            disTmp.Push(LogicalThreadContext.Stacks[kv.Key].Push(kv.Value));

                    LogWrapBegin("begin");
                    T tmp = action();
                    LogWrapDone("done");
                    return tmp;
                }
                catch (Exception e)
                {
                    LogWrapFail("fail", e);
                    throw e;
                }
                finally
                {
                    LogWrapAlways("always");

                    while (disTmp.Count > 0)
                        disTmp.Pop().Dispose();
                }
            }
        }
        public ILoggingOperation SinkWrap(Action action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null)
        {
            Stack<IDisposable> disTmp = new Stack<IDisposable>();
            using (LogicalThreadContext.Stacks["wrapCtx_UUID"].Push(NextWrapId))
            using (LogicalThreadContext.Stacks["logCtx_memberName"].Push(memberName))
            {
                try
                {
                    if (props != null)
                        foreach (KeyValuePair<string, string> kv in props)
                            disTmp.Push(LogicalThreadContext.Stacks[kv.Key].Push(kv.Value));

                    LogWrapBegin("begin");
                    action?.Invoke();
                    LogWrapDone("done");
                }
                catch (Exception e)
                {
                    LogWrapFail("fail", e);
                }
                finally
                {
                    LogWrapAlways("always");

                    while (disTmp.Count > 0)
                        disTmp.Pop().Dispose();
                }

                return this;
            }
        }
        public T SinkWrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod", Dictionary<string, string> props = null)
        {
            if (action == null)
                throw new ArgumentNullException("action must have a value.");
            Stack<IDisposable> disTmp = new Stack<IDisposable>();
            using (LogicalThreadContext.Stacks["wrapCtx_UUID"].Push(NextWrapId))
            using (LogicalThreadContext.Stacks["logCtx_memberName"].Push(memberName))
            {
                try
                {
                    if (props != null)
                        foreach (KeyValuePair<string, string> kv in props)
                            disTmp.Push(LogicalThreadContext.Stacks[kv.Key].Push(kv.Value));

                    LogWrapBegin("begin");
                    T tmp = action();
                    LogWrapDone("done");
                    return tmp;
                }
                catch (Exception e)
                {
                    LogWrapFail("fail", e);
                    return default(T);
                }
                finally
                {
                    LogWrapAlways("always");

                    while (disTmp.Count > 0)
                        disTmp.Pop().Dispose();
                }
            }
        }

        public ILoggingOperation AddProperty(string name, string value)
        {
            _ctx.Push(LogicalThreadContext.Stacks[name].Push(value));

            return this;
        }

        public ILoggingOperation AddProperty(string name, object value)
        {
            _ctx.Push(LogicalThreadContext.Stacks[name].Push(JsonConvert.SerializeObject(value)));

            return this;
        }
    }
}
