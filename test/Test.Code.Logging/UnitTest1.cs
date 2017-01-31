using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using Code.Logging;
using System.Runtime.Remoting.Messaging;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
using Code.Logging.Core;
using Code.Logging.Reporting;

namespace Test.Code.Logging
{
    //TODO: fare test veri
    [TestClass]
    public class UnitTest1
    {
        private const string LOG_PATTERN = "%d [%t] %logger %P %-5p %m%n";
        //[ClassInitialize]
        public static void Initialize(TestContext ctx)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            TraceAppender tracer = new TraceAppender();

            tracer.ImmediateFlush = true;

            //PatternLayout layout = new PatternLayout();
            //layout.ConversionPattern = LOG_PATTERN;
            //layout.ActivateOptions();

            JsonLayout layout = new JsonLayout();
            layout.ActivateOptions();

            tracer.Layout = layout;
            tracer.ActivateOptions();
            hierarchy.Root.AddAppender(tracer);

            HttpPostAppender appender = new HttpPostAppender() { BufferSize=20 };
            appender.Layout = layout;
            appender.PostUrl = "http://requestb.in/1euiz3m1";
            appender.LogSourceIdentity = "test";

            appender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);

            //FileAppender appender = new FileAppender();
            //appender.Layout = layout;
            //appender.File = @"..\..\utest.log";
            //appender.AppendToFile = true;

            //appender.ActivateOptions();
            //hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            CallContext.FreeNamedDataSlot("log4net.Util.LogicalThreadContextProperties");
        }

        [TestCategory("logging")]
        [TestMethod]
        public void TestMethod1()
        {
            using (ILoggingOperation op = LogManager.GetLogger("Test1").NormalOperation())
            {
                op.Wrap(() =>
                {
                    Assert.IsTrue(true);
                    op.InfoFormat("Prova di log format {0}",DateTime.Now);
                });
            }
            Assert.IsTrue(true);
        }
        [TestCategory("logging")]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMethod2()
        {
            using (ILoggingOperation op = LogManager.GetLogger("Test2").NormalOperation())
            {
                op.Wrap(() =>
                {
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                });
            }
            Assert.IsTrue(true);
        }
        [TestCategory("logging")]
        [TestMethod]
        public void TestMethod3()
        {
            using (ILoggingOperation op = LogManager.GetLogger("Test3").NormalOperation())
            {
                op.SinkWrap(() =>
                {
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                });
            }
            Assert.IsTrue(true);
        }
        [TestCategory("logging")]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMethod4()
        {
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
        }
    }
}
