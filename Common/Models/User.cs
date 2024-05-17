using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Runtime.Serialization;

namespace Common.Models
{
    [DataContract]
    public class User : TableEntity
    {
        public User() { PartitionKey = "User"; RowKey = Guid.NewGuid().ToString(); }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string AddressId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public double TotalInvestment { get; set; }
    }
}