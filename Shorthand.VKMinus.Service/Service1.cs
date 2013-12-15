using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Shorthand.VKMinus.Data;
using Shorthand.VKMinus.Parser;

namespace Shorthand.VKMinus.Service
{
    public partial class Service1 : ServiceBase
    {
        private readonly IScheduler _scheduler = null;
        public Service1()
        {
            InitializeComponent();

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            _scheduler = schedFact.GetScheduler();
            
            var parseJob = new JobDetailImpl("ParseJob", "ParserStuff", typeof(ParseJob));

            var parseTrigger = new CronTriggerImpl("ParseTime", "ParserStuff", "0 0/10 * * * ?") // Exactly every ten minutes
            {
                StartTimeUtc = DateTime.UtcNow
            };
            _scheduler.ScheduleJob(parseJob, parseTrigger);


            var summaryJob = new JobDetailImpl("SummaryJob", "SummaryStuff", typeof(SummaryJob));

            var summaryTrigger = new CronTriggerImpl("SummaryTime", "SummaryStuff", "0 0 4 1/1 * ? *") // At 04:00 AM every night
            {
                StartTimeUtc = DateTime.UtcNow
            };
            _scheduler.ScheduleJob(summaryJob, summaryTrigger);
        }

        protected override void OnStart(string[] args)
        {
            if (!_scheduler.IsStarted)
                _scheduler.Start();
        }

        protected override void OnStop()
        {
            if (_scheduler.IsStarted)
                _scheduler.Shutdown(true);
        }

        public class ParseJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                var data = VKParser.ParseStartpage();
                var repo = new Repository();
                repo.Save(data);
            }
        }

        public class SummaryJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
//                var repo = new Repository();
            }
        }
    }
}
