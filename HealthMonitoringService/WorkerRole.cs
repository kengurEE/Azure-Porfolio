using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using Microsoft.WindowsAzure.ServiceRuntime;
using PortfolioService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitoringService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        JobServer _JobServer = new JobServer();
        public override void Run()
        {
            Trace.TraceInformation("HealthMonitoringService is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            _JobServer.Open();
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("HealthMonitoringService has been started");

            return result;
        }

        public override void OnStop()
        {
            _JobServer.Close();
            Trace.TraceInformation("HealthMonitoringService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("HealthMonitoringService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            Random random = new Random();
            HealthCheckRepository healthCheckRepository = new HealthCheckRepository();
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                bool healthPS;
                try
                {
                    var portfolioService = WcfClientHelper.Connect<IHealthMonitoring>("PortfolioService", "health-monitoring"); ;
                    healthPS = portfolioService.HealthCheck();
                }
                catch (Exception e)
                {
                    healthPS = false;

                    Trace.TraceInformation(e.Message);

                }
                healthCheckRepository.Add(new HealthCheck { Timestamp = DateTime.Now, IsHealth = healthPS, ServiceName = "Portfolio" });
                if (!healthPS)
                    SendMail("Portfolio");

                bool healthNS;
                try
                {
                    var notificationService = WcfClientHelper.Connect<IHealthMonitoring>("NotificationService", "health-monitoring");

                    healthNS = notificationService.HealthCheck();
                }
                catch (Exception e)
                {
                    healthNS = false;
                    Trace.TraceInformation(e.Message);
                }
                healthCheckRepository.Add(new HealthCheck { Timestamp = DateTime.Now, IsHealth = healthNS, ServiceName = "Notification" });
                if (!healthNS)
                    SendMail("Notification");

                await Task.Delay(random.Next(1000, 5000));
            }
        }
        MailRepository MailRepository = new MailRepository();
        private void SendMail(string serviceName)
        {
            foreach (string mail in MailRepository.Get())
            {
                MailHelper.SendMail("Problem sa servisom", $"Servis {serviceName} trenutno nije dostupan", mail);
            }
        }
    }
}
