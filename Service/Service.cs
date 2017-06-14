using log4net;
using System;
using System.Configuration;
using System.ServiceProcess;
using Common;

namespace Service
{
    public partial class Service : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Worker.Worker worker;
        public Service()
        {
            InitializeComponent();

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
            else
            {
                Logging.ErrorFormat(log, "Failed setting waitTimeout. Defaulting to infinity. Configuration value was {0}", ConfigurationManager.AppSettings["WaitTimeoutSeconds"]);
                waitTimeout = Int32.MaxValue; // Infinity
            }
            var useShellExecute = bool.Parse(ConfigurationManager.AppSettings["UseShellExecute"]);
            worker = new Worker.Worker(fileName, arguments, intervalSeconds, waitForExit, waitTimeout, useShellExecute);
            worker.WorkerExecutedEvent += WorkerExecuted;
        }

        private void WorkerExecuted(object sender, Worker.WorkerEventArgs e)
        {
            Logging.DebugFormat(log, "Worker has executed");
        }

        protected override void OnStart(string[] args)
        {
            Logging.InfoFormat(log, "Service starting");
            worker.StartWorker();
        }

        protected override void OnStop()
        {
            Logging.InfoFormat(log, "Service stopping");
            worker.StopWorker();
        }
    }
}
