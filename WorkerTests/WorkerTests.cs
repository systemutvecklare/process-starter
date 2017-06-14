using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Timers;
using System.Configuration;

namespace WorkerTests
{
    internal interface IWorkerExecutedSubscriber
    {
        void ExecutedCallback(object sender, EventArgs e);
    }
    [TestClass]
    public class WorkerTests
    {
        [TestMethod]
        public void WorkerCanStart()
        {
            var fileName = ConfigurationManager.AppSettings["FileName"];
            var arguments = ConfigurationManager.AppSettings["Arguments"];
            var intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
            var waitForExit = bool.Parse(ConfigurationManager.AppSettings["WaitForExit"]);
            int waitTimeoutSeconds = 0;
            int waitTimeout = 0;
            var parseSuccess = int.TryParse(ConfigurationManager.AppSettings["WaitTimeoutSeconds"], out waitTimeoutSeconds);
            if (parseSuccess)
            {
                waitTimeout = waitTimeoutSeconds * 1000;
            }

            var subscriber = Substitute.For<IWorkerExecutedSubscriber>();
            var worker = new Worker.Worker(fileName, arguments, intervalSeconds, waitForExit, waitTimeout, useShellExecute: true);
            worker.WorkerExecutedEvent += subscriber.ExecutedCallback;
            worker.StartWorker();
            subscriber.Received().ExecutedCallback(Arg.Any<object>(), Arg.Any<ElapsedEventArgs>());
        }
    }
}
