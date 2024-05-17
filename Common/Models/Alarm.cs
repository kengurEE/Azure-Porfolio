using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Alarm : TableEntity
    {
        public Alarm() { PartitionKey = "alarm"; RowKey = Guid.NewGuid().ToString(); }
        public string Currency { get; set; }
        public string Email { get; set; }
        public double Limit { get; set; }
        public DateTime LastAccess { get; set; }
        public bool Active { get; set; }
    }
}
