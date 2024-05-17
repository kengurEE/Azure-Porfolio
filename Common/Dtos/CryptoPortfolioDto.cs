using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Common.Models
{
    [DataContract]
    public class CryptoPortfolioDto
    {

        [DataMember]
        public string User { get; set; }
        [DataMember]
        public List<CryptoPortfolioItemDto> Items { get; set; } = new List<CryptoPortfolioItemDto>();
        public double Invested { get => Items.Sum(x => x.Invested); }
        public double TotalValue { get => Items.Sum(x => x.Value); }
        public double Profit { get => TotalValue - Invested; }
    }
}
