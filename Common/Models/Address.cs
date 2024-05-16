using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Runtime.Serialization;

namespace Common.Models
{

    [DataContract]
    public class Address : TableEntity
    {
        public Address() { PartitionKey = "Address"; RowKey = Guid.NewGuid().ToString(); }
        public string Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
    }
}