using System;
using System.Configuration.Install;
using System.Reflection;
using Common;

namespace Service
{
    public static class SelfInstaller
    {
        private static readonly string _exePath =
            Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });

                try
                {
                    var perfMon = new PerformanceMonitor("Process Starter Service");
                    perfMon.CreateCategory();
                }
                catch (Exception)
                {
                }
                
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                try
                {
                    var perfMon = new PerformanceMonitor("Process Starter Service");
                    perfMon.RemoveCategory();
                }
                catch (Exception)
                {
                }

                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
