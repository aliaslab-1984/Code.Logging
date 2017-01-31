# Code.Logging
Log4net extension library adding a lot of functionality for uniforming traces and enabling scope logging

## Description

The library extends log4net with _Decorators_ of the interface _ILog_, that add functionality beyond the base ones of tracing.

The basic idea is that every portion of _interesting code_ shound be delimited by:

- a beginning trace
- an ending trace (positive or negative)
- an closing trace (gloabl, independent from the outcome)

To perform theese logging operations is possibile to:

- wrap the code segment in an _Action_ and monitor the execution
- create a scope around the code segment and monitor the traversing

Both the alternatives have pros and cons and so the library allows them both.

Implementing the interface **ILoggingOperation**:

<pre>
    public interface ILoggingOperation : IDisposable, ILog
    {
        string Id { get; }
        ILoggingOperation AddProperty(string name, string value);
        ILoggingOperation AddProperty(string name, object value);

        IControlledLogTrace TraceStart([CallerMemberName] string memberName = "unknownMethod");

        ILoggingOperation Wrap(Action action, [CallerMemberName] string memberName = "unknownMethod");
        T Wrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod");
        ILoggingOperation SinkWrap(Action action, [CallerMemberName] string memberName = "unknownMethod");
        T SinkWrap<T>(Func<T> action, [CallerMemberName] string memberName = "unknownMethod");
    }
</pre>

is possible to build a _Decorator_ of ILog which allows a code segment wrapping.

Implementing the interface **IControlledLogTrace** (also used by ILoggingOperation):

<pre>
    public interface IControlledLogTrace : IDisposable, ILog
    {
        string Id { get; }

        IControlledLogTrace AddProperty(string name, string value);
        IControlledLogTrace AddProperty(string name, object value);
    }
</pre>

is possible to build a _Decorator_ of ILog which allows to create a monitored scope around a segment of code.

The library makes available two base abstract calsses for the implementation of the interfaces which allow to modify the behavior in the four case of tracing:

- **LogWrapBegin**: for the beginnig trace
- **LogWrapDone**: for the ending with success trace
- **LogWrapFail**: for the ending with failure trace
- **LogWrapAlways**: for the global closure trace

Three implementation exist:

- **CriticalLoggingOperation**: produce a trace of each type of _Info_ level and _Error_ for the failure one
- **NormalLoggingOperation**: produce a trace of each type of _Info_ level and _Warn_ for the failure one
- **DebugLoggingOperation**: produce a trace of each type of _Debug_ level

In addition to providing _wrapped_ and/or _scoped_ contexts, using that calsses implicitly create a collection of log4net property stacks (on LogicalThreadContext) which persists in every implicit trace (around the code segment) and explicit one, requested using the decorated ILog interface as usual. That allows to correlate every trace to the logical calling context and flux seamlessly.

Using the extensions provided by the package _Code.Logging.ExecutionContextSpanIntegration_ is possible to exploit the context spanning features of [CodeProbe.ExecutionControl](https://github.com/aliaslab-1984/CodeProbe) to propagate the logical flux correlation ids remotely.

## Examples

Log of opeartion through the wrapping of a code segment:
<pre>
            using (ILoggingOperation op = LogManager.GetLogger("Test1").NormalOperation())
            {
                op.Wrap(() =>
                {
                    Assert.IsTrue(true);
                    op.InfoFormat("Prova di log format {0}",DateTime.Now);
                });
            }
</pre>

Log of opeartion through the wrapping of a code segment with exception propagation:
<pre>
            using (ILoggingOperation op = LogManager.GetLogger("Test2").NormalOperation())
            {
                op.Wrap(() =>
                {
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                });
            }
</pre>

Log of opeartion through the wrapping of a code segment with exception sinking:
<pre>
            using (ILoggingOperation op = LogManager.GetLogger("Test3").NormalOperation())
            {
                op.SinkWrap(() =>
                {
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                });
            }
</pre>

Log of opeartion through the scoping of code segments with a final exception propagation:
<pre>
            using (ILoggingOperation op = LogManager.GetLogger("Test4").NormalOperation())
            {
                using (IControlledLogTrace tr = op.TraceStart())
                {
                    Assert.IsTrue(true);
                }
                using (IControlledLogTrace tr = op.TraceStart())
                {
                    Assert.IsTrue(true);
                }

                using (IControlledLogTrace tr = op.TraceStart())
                {
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                }
            }
</pre>

Propagation of the logging scope remotely:
<pre>
    using (ILoggingOperation op = LogManager.GetLogger("Test1").NormalOperation().SpanContext())
    {
        op.Wrap(() =>
        {
            Assert.IsTrue(true);
            op.InfoFormat("Prova di log format {0}",DateTime.Now);
        });

        using (IControlledLogTrace tr = op.TraceStart().SpanContext())
        {
            Assert.IsTrue(true);
        }
    }
</pre>
