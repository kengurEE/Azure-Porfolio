using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Runtime.Serialization;

namespace Common.Models
{
    [DataContract]
    public class TransactionDto
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public double Quantity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public bool IsInvest { get; set; }
    }
}
