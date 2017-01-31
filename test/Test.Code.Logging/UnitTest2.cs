using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
using System.Runtime.Remoting.Messaging;

namespace Test.Code.Logging
{
    [TestClass]
    public class UnitTest2
    {
        private const string LOG_PATTERN = "%d [%t] %P %-5p %m%n";
        [ClassInitialize]
        public static void Initialize(TestContext ctx)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            TraceAppender tracer = new TraceAppender();
            PatternLayout patternLayout = new PatternLayout();

            tracer.ImmediateFlush = true;

            patternLayout.ConversionPattern = LOG_PATTERN;
            patternLayout.ActivateOptions();

            tracer.Layout = patternLayout;
            tracer.ActivateOptions();
            hierarchy.Root.AddAppender(tracer);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            CallContext.FreeNamedDataSlot("log4net.Util.LogicalThreadContextProperties");
        }

        [TestCategory("performance")]
        [TestMethod]
        public void TestMethod1()
        {
            ILog op = LogManager.GetLogger("Test1");
            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                op.Debug("");
                Assert.IsTrue(true);
                op.Debug("");
                op.Debug("");
            }
            Assert.IsTrue(true);
        }
        [TestCategory("performance")]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMethod2()
        {
            ILog op = LogManager.GetLogger("Test2");
            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                op.Debug("");
                Assert.IsTrue(true);
                op.Debug("");
                op.Debug("");
                throw new Exception("xy");
            }
        }
        [TestCategory("performance")]
        [TestMethod]
        public void TestMethod3()
        {
            ILog op = LogManager.GetLogger("Test3");
            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                try
                {
                    op.Debug("");
                    Assert.IsTrue(true);
                    throw new Exception("xy");
                }
                catch(Exception e)
                {
                    op.Debug("");
                    op.Debug("");
                }
            }
            Assert.IsTrue(true);
        }
        [TestCategory("performance")]
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMethod4()
        {
            ILog op = LogManager.GetLogger("Test4");
            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                op.Debug("");
                Assert.IsTrue(true);
                op.Debug("");
                op.Debug("");
            }
            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                op.Debug("");
                Assert.IsTrue(true);
                op.Debug("");
                op.Debug("");
            }

            using (LogicalThreadContext.Stacks["test"].Push("xy"))
            {
                op.Debug("");
                Assert.IsTrue(true);
                op.Debug("");
                op.Debug("");
                throw new Exception("xy");
            }
        }
    }
}
