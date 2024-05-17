using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace HealthMonitoringService
{
    public class Mail : TableEntity
    {
        public Mail() { PartitionKey = "mail"; RowKey = Guid.NewGuid().ToString(); }
        public string Address { get; set; }
    }
}
