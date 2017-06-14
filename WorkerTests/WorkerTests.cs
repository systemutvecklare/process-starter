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
        public async void WorkerCanStart()
        {
            var fileName = ConfigurationManager.AppSettings["FileName"];
            var arguments = ConfigurationManager.AppSettings["Arguments"];
            var intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
            var waitForExit = bool.Parse(ConfigurationManager.AppSettings["WaitForExit"]);

            var subscriber = Substitute.For<IWorkerExecutedSubscriber>();
            var worker = new Worker.Worker(fileName, arguments, intervalSeconds, waitForExit);
            worker.WorkerExecutedEvent += subscriber.ExecutedCallback;
            worker.StartWorker();
            subscriber.Received().ExecutedCallback(Arg.Any<object>(), Arg.Any<ElapsedEventArgs>());
        }
    }
}
