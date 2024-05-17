using Common;
using Common.Helpers;
using Common.Models;
using CryptoService;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using PortfolioService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        JobServer JobServer = new JobServer();
        public override void Run()
        {
            Trace.TraceInformation("NotificationService is running");

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
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            JobServer.Open();
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("NotificationService has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("NotificationService is stopping");
            JobServer.Close();

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("NotificationService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                UpdateCryptocurrencyValues();
                CheckAlarms();
                await Task.Delay(10000);
            }
        }
        DateTime lastReq = DateTime.MinValue;
        private void UpdateCryptocurrencyValues()
        {
            if (DateTime.Now.AddHours(-1) > lastReq)
            {
                new CryptoServiceProvider().ReadAndSaveCryptocurrencies();
                lastReq = DateTime.Now;
            }
        }
        private void CheckAlarms()
        {
            AlarmRepository alarmRepository = new AlarmRepository();
            CryptocurrencyRepository cryptocurrencyRepository = new CryptocurrencyRepository();

            var cryptocurrencies = cryptocurrencyRepository.GetAll().ToDictionary(x => x.RowKey, x => x.Value);
            var alarms = alarmRepository.Get20();
            List<string> alarmIds = new List<string>();
            foreach (var alarm in alarms)
            {
                if (cryptocurrencies[alarm.Currency] > alarm.Limit)
                {
                    SendNotification(alarm);
                    alarm.Active = false;
                    alarmRepository.Update(alarm);
                    alarmIds.Add(alarm.RowKey);
                    Trace.TraceInformation($"{DateTime.Now}|{alarm.RowKey}|1");
                }
            }

            var queue = QueueHelper.GetQueueReference("alarm");
            queue.AddMessage(new CloudQueueMessage(string.Join("|", alarmIds)));
        }

        private void SendNotification(Alarm alarm)
        {
            MailHelper.SendMail("Alarm", $"Kriptovaluta {alarm.Currency} je dostigla limit od {alarm.Limit}USD", alarm.Email);
        }
    }
}
