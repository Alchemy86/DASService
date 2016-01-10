using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.ServiceProcess;
using System.Threading;
using Ninject;

namespace AuctionSniperWindowsService
{
    static class Program
    {
        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        [DebuggerNonUserCode]
        private static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                IKernel kernel = new StandardKernel(new Bindings());
#if DEBUG
                var s = new Service1(kernel);
                s.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new Service1(kernel) 
            };
                ServiceBase.Run(ServicesToRun);
#endif
            }
            catch (Exception e)
            {
                var message =
                e.Message + Environment.NewLine + e.Source + Environment.NewLine + e.StackTrace
                + Environment.NewLine + e.InnerException;
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var message =
                e.Exception.Message + Environment.NewLine + e.Exception.Source + Environment.NewLine + e.Exception.StackTrace
                + Environment.NewLine + e.Exception.InnerException;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            var message = ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace
                          + Environment.NewLine + ex.InnerException;
        }
    }
}
