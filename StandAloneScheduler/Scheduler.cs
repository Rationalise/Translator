using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.ServiceProcess;

namespace StandAloneScheduler 
{
    class Scheduler : ServiceBase
    {

        private static double intervalMilliseconds = 60 * 1000;
        System.Timers.Timer timer;

        public Scheduler()
        {
            intervalMilliseconds = 1 * 60 * 1000;
            timer = new System.Timers.Timer(intervalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
        }

        public void StartJob()
        {
            runJob();
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            runJob();
            timer.Start();
        }

        private void runJob()
        {
            Process[] pname = Process.GetProcessesByName("TranslatorWebInterface");
            if (pname.Length == 0)
                Process.Start(@"C:\Program Files (x86)\SDL\SDL Trados Studio\Studio4\TranslatorWebInterface.exe");

            Process.Start("taskkill", "/f /im winword.exe");
            Process.Start("taskkill", "/f /im excel.exe");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnStart(string[] args)
        {
            StartJob();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
