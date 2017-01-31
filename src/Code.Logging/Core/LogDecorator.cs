using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace Code.Logging.Core
{
    public class LogDecorator : ILog
    {
        protected ILog _logger;

        public LogDecorator(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger must be not null.");
            _logger = logger;
        }

        #region ILog impl
        public virtual bool IsDebugEnabled
        {
            get
            {
                return _logger.IsDebugEnabled;
            }
        }

        public virtual bool IsInfoEnabled
        {
            get
            {
                return _logger.IsInfoEnabled;
            }
        }

        public virtual bool IsWarnEnabled
        {
            get
            {
                return _logger.IsWarnEnabled;
            }
        }

        public virtual bool IsErrorEnabled
        {
            get
            {
                return _logger.IsErrorEnabled;
            }
        }

        public virtual bool IsFatalEnabled
        {
            get
            {
                return _logger.IsFatalEnabled;
            }
        }

        public virtual ILogger Logger
        {
            get
            {
                return _logger.Logger;
            }
        }

        public virtual void Debug(object message)
        {
            _logger.Debug(message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
        }

        public virtual void DebugFormat(string format, object arg0)
        {
            _logger.DebugFormat(format, arg0);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1)
        {
            _logger.DebugFormat(format, arg0, arg1);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.DebugFormat(format, arg0, arg1, arg2);
        }

        public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.DebugFormat(provider, format, args);
        }

        public virtual void Info(object message)
        {
            _logger.Info(message);
        }

        public virtual void Info(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        public virtual void InfoFormat(string format, object arg0)
        {
            _logger.InfoFormat(format, arg0);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1)
        {
            _logger.InfoFormat(format, arg0, arg1);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.InfoFormat(format, arg0, arg1, arg2);
        }

        public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.InfoFormat(provider, format, args);
        }

        public virtual void Warn(object message)
        {
            _logger.Warn(message);
        }

        public virtual void Warn(object message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        public virtual void WarnFormat(string format, object arg0)
        {
            _logger.WarnFormat(format, arg0);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1)
        {
            _logger.WarnFormat(format, arg0, arg1);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.WarnFormat(format, arg0, arg1, arg2);
        }

        public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.WarnFormat(provider, format, args);
        }

        public virtual void Error(object message)
        {
            _logger.Error(message);
        }

        public virtual void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
        }

        public virtual void ErrorFormat(string format, object arg0)
        {
            _logger.ErrorFormat(format, arg0);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1)
        {
            _logger.ErrorFormat(format, arg0, arg1);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.ErrorFormat(provider, format, args);
        }

        public virtual void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            _logger.FatalFormat(format, args);
        }

        public virtual void FatalFormat(string format, object arg0)
        {
            _logger.FatalFormat(format, arg0);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1)
        {
            _logger.FatalFormat(format, arg0, arg1);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.FatalFormat(format, arg0, arg1, arg2);
        }

        public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.FatalFormat(provider, format, args);
        }
        #endregion
    }
}
