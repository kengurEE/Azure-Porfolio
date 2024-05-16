using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Common.Models
{
    public class HealthCheck : TableEntity
    {
        public DateTime Timestamp { get; set; }
        public bool IsHealth { get; set; }
        public string ServiceName { get; set; }
        public HealthCheck()
        {
            PartitionKey = "healthcheck";
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }
    }
}
