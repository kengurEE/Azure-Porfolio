using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Common.Models
{
    public class Transaction : TableEntity
    {
        public Transaction() { PartitionKey = "Transaction"; RowKey = Guid.NewGuid().ToString(); }
        public string User { get; set; }
        public string Currency { get; set; }
        public decimal Quantity { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
    }
}
