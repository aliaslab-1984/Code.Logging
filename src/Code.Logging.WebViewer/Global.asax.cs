using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using log4net.Core;

namespace Code.Logging.WebViewer
{
    public class Global : HttpApplication
    {
        public static ILoggingEventRepository LoggingEventRepository { get; protected set; }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LoggingEventRepository = new InMemoryLoggingEventRepository();

#if DEBUG

            System.Threading.Timer t = new System.Threading.Timer(s=> {
                LoggingEventRepository.Put("DEBUG", new LoggingEvent(new LoggingEventData() { Level = log4net.Core.Level.Debug, LoggerName="Test", Message="Test", TimeStampUtc=DateTime.Now, LocationInfo=new LocationInfo(GetType()) }));
            },null,10000,10000);

#endif
        }
    }
}