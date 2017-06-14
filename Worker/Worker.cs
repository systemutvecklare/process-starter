using log4net;
using System;
using System.Diagnostics;
using System.Timers;
using Common;

namespace Worker
{
    public class WorkerEventArgs : EventArgs
    {
        public string Data { get; private set; }
        public WorkerEventArgs(string data)
        {
            Data = data;
        }
    }

    public class Worker : IDisposable
    {        
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Timer timer;
        private string fileName;
        private string arguments;
        private int intervalSeconds;
        private bool waitForExit;
        private int waitTimeout;
        private bool useShellExecute;        

        public event EventHandler<WorkerEventArgs> WorkerExecutedEvent;

        PerformanceMonitor perfMon;

        public Worker(string fileName, string arguments, int intervalSeconds, bool waitForExit, int waitTimeout, bool useShellExecute)
        {
            perfMon = new PerformanceMonitor("Process Starter Service");            
            perfMon.CreateCategory();            

            this.fileName = fileName;
            this.arguments = arguments;
            this.intervalSeconds = intervalSeconds;
            this.waitForExit = waitForExit;
            this.useShellExecute = useShellExecute;
            this.waitTimeout = waitTimeout;

            timer = new Timer();
            timer.Interval = intervalSeconds * 1000;
            timer.Elapsed += DoWork;
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                Logging.DebugFormat(log, "Executing command '{0} {1}'", fileName, arguments);
                if (waitForExit)
                {
                    timer.Enabled = false;
                }

                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = useShellExecute;
                p.StartInfo.RedirectStandardOutput = !useShellExecute;
                p.StartInfo.FileName = fileName;
                p.StartInfo.Arguments = arguments;
                p.Start();
                perfMon.IncrementCounter(PerformanceMonitor.COUNTER_EXECUTIONS);
                if (waitForExit)
                {
                    Logging.DebugFormat(log, "Waiting for process to exit");
                    if (!p.WaitForExit(waitTimeout))
                    {
                        Logging.DebugFormat(log, "Process timed out after {0}ms (configuration value). Killing process.", waitTimeout);
                        p.Kill();
                        perfMon.IncrementCounter(PerformanceMonitor.COUNTER_EXECUTION_TIMEOUTS);
                    }
                }

                Logging.DebugFormat(log, "Done executing command");
                //WorkerExecutedEvent(this, new WorkerEventArgs(fileName + " " + arguments));
            }
            catch (Exception ex)
            {
                Logging.ErrorFormat(log, "Failed executing command. Error: {0}. Stacktrace: {1}", ex.Message, ex.StackTrace);
                perfMon.IncrementCounter(PerformanceMonitor.COUNTER_EXECUTION_FAILURES);
            }
            finally
            {
                if (!timer.Enabled)
                {
                    timer.Enabled = true;
                }
            }
            
        }

        public void StartWorker()
        {
            timer.Enabled = true;
            Logging.InfoFormat(log, "Worker started");
        }

        public void StopWorker()
        {
            timer.Enabled = false;
            Logging.InfoFormat(log, "Worker stopped");
        }

        public void Dispose()
        {
            if (WorkerExecutedEvent != null)
            {
                foreach (EventHandler<WorkerEventArgs> item in WorkerExecutedEvent.GetInvocationList())
                {
                    WorkerExecutedEvent -= item;
                }
            }
        }
    }
}
