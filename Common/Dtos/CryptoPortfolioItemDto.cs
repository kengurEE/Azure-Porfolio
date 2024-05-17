using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;

namespace Common.Models
{
    [DataContract]
    public class CryptoPortfolioItemDto
    {
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Quantity { get; set; }
        [DataMember]
        public double Invested { get; set; }
        public double Value { get => Price * Quantity; }
        public double Profit { get => Value - Invested; }
    }
}
