using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Common.Models
{
    public class Transaction : TableEntity
    {
        public Transaction() { PartitionKey = "transaction"; RowKey = Guid.NewGuid().ToString(); }
        public string User { get; set; }
        public string Currency { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public bool IsInvested { get; set; }
        public double Amount { get => Quantity * Price; }
    }
}
