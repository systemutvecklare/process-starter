using System;
using System.Diagnostics;

namespace Common
{
    public class PerformanceMonitor
    {
        private bool active = true;
        private string category;
        private CounterCreationDataCollection counters = new CounterCreationDataCollection();

        public const string COUNTER_EXECUTIONS = "# executions";
        public const string COUNTER_EXECUTION_TIMEOUTS = "# timed out executions";
        public const string COUNTER_EXECUTION_FAILURES = "# failed executions";

        /// <summary> 
        /// Creates an instance of the class. 
        /// </summary> 
        /// <param name="categoryName">The name of the performance counter category.</param> 
        public PerformanceMonitor(string categoryName)
        {
            this.category = categoryName;
            AddCounter(COUNTER_EXECUTIONS, "Total number of times the process has been executed", PerformanceCounterType.NumberOfItems64);
            AddCounter(COUNTER_EXECUTION_TIMEOUTS, "Total number of times the process timed out when being executed", PerformanceCounterType.NumberOfItems64);
            AddCounter(COUNTER_EXECUTION_FAILURES, "Total number of times the process failed when being executed", PerformanceCounterType.NumberOfItems64);
        }

        /// <summary> 
        /// Creates the performance counters 
        /// </summary> 
        public void CreateCategory()
        {
            try
            {
                if (!PerformanceCounterCategory.Exists(this.category))
                {
                    PerformanceCounterCategory.Create(this.category, this.category, PerformanceCounterCategoryType.Unknown, this.counters);
                }
            }
            catch (Exception)
            {
                this.active = false;
            }
            
        }

        public void RemoveCategory()
        {
            if (PerformanceCounterCategory.Exists(this.category))
            {
                PerformanceCounterCategory.Delete(this.category);
            }
        }

        /// <summary> 
        /// Add a performance counter to the category of performance counters. 
        /// </summary> 
        public void AddCounter(string name, string helpText, PerformanceCounterType type)
        {
            CounterCreationData ccd = new CounterCreationData();
            ccd.CounterName = name;
            ccd.CounterHelp = helpText;
            ccd.CounterType = type;
            this.counters.Add(ccd);
        }

        public void IncrementCounter(string name)
        {
            if (!active)
            {
                return;
            }

            PerformanceCounter counter = new PerformanceCounter(categoryName: this.category, counterName: name, readOnly: false);            
            counter.Increment();
            counter.Close();
        }
    }
}
