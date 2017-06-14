using System;
using System.Configuration;
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {            
            string fileName = ConfigurationManager.AppSettings["FileName"];
            string arguments = ConfigurationManager.AppSettings["Arguments"];
            int intervalSeconds = 10;
            int.TryParse(ConfigurationManager.AppSettings["IntervalSeconds"], out intervalSeconds);
            bool waitForExit = true;
            bool.TryParse(ConfigurationManager.AppSettings["WaitForExit"], out waitForExit);
            bool useShellExecute = true;
            bool.TryParse(ConfigurationManager.AppSettings["UseShellExecute"], out useShellExecute);
            int waitTimeoutSeconds = 0;
            int waitTimeout = 0;
            var parseSuccess = int.TryParse(ConfigurationManager.AppSettings["WaitTimeoutSeconds"], out waitTimeoutSeconds);
            if (parseSuccess)
            {
                waitTimeout = waitTimeoutSeconds * 1000;
            }
            else
            {
                waitTimeout = Int32.MaxValue; // Infinity
            }

            foreach (var arg in args)
            {
                if (arg.ToLower().StartsWith("filename="))
                {
                    fileName = arg.Substring("filename=".Length);
                }
                else if (arg.ToLower().StartsWith("arguments="))
                {
                    arguments = arg.Substring("arguments=".Length);
                }
                else if (arg.ToLower().StartsWith("intervalseconds="))
                {
                    intervalSeconds = int.Parse(arg.Substring("intervalseconds=".Length));
                }
                else if (arg.ToLower().StartsWith("waitforexit="))
                {
                    waitForExit = bool.Parse(arg.Substring("waitforexit=".Length));
                }
                else if (arg.ToLower().StartsWith("useshellexecute="))
                {
                    useShellExecute = bool.Parse(arg.Substring("useshellexecute=".Length));
                }
                else if (arg.ToLower().StartsWith("waittimeoutseconds="))
                {
                    waitTimeout = int.Parse(arg.Substring("waittimeoutseconds=".Length));
                }
            }

            var worker = new Worker.Worker(fileName, arguments, intervalSeconds, waitForExit, waitTimeout, useShellExecute);
            worker.WorkerExecutedEvent += Worker_WorkerExecutedEvent;

            var message = "Running command '" + fileName + " " + arguments + "' at interval " + intervalSeconds + " " + (waitForExit ? "waiting for command to finish" : "");
            Console.WriteLine(message);
            Console.WriteLine("Press any key to exit");

            worker.StartWorker();
            Console.ReadKey();
            worker.StopWorker();
        }

        private static void Worker_WorkerExecutedEvent(object sender, Worker.WorkerEventArgs e)
        {
            Console.WriteLine(string.Format("HH:mm:ss", DateTime.Now) + " Executed " + e.Data);
        }
    }
}
