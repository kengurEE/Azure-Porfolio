using Common;
using Common.Models;
using CryptoService;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        JobServer JobServer = new JobServer();
        public override void Run()
        {
            Trace.TraceInformation("PortfolioService is running");

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

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.
            JobServer.Open();
            bool result = base.OnStart();

            Trace.TraceInformation("PortfolioService has been started");

            return result;
        }

        public override void OnStop()
        {
            JobServer.Close();
            Trace.TraceInformation("PortfolioService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("PortfolioService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                CheckAlarmQueue();
                await Task.Delay(10000);
            }
        }

        private void CheckAlarmQueue()
        {


            var queue = QueueHelper.GetQueueReference("alarm");
            CloudQueueMessage message = queue.GetMessage();

            while (message != null)
            {
                if (!string.IsNullOrEmpty(message.AsString))
                {

                    string[] alarmIds = message.AsString.Split('|');
                    AlarmRepository alarmRepository = new AlarmRepository();
                    TransactionRepository transactionRepository = new TransactionRepository();
                    foreach (var alarmId in alarmIds)
                    {
                        Alarm alarm = alarmRepository.Get(alarmId);
                        var portfolio = PortfolioHelper.GetPortfolio(alarm.Email);
                        var portfolioItem = portfolio.Items.FirstOrDefault(x => x.Currency == alarm.Currency);
                        if (portfolioItem != null)
                        {

                            Transaction transaction = new Transaction
                            {
                                User = alarm.Email,
                                Currency = alarm.Currency,
                                Quantity = portfolioItem.Quantity,
                                Price = portfolioItem.Price,
                                IsInvested = false,
                            };
                            transactionRepository.Add(transaction);
                        }
                    }
                }
                queue.DeleteMessage(message);
                message = queue.GetMessage();
            }

        }
    }
}
