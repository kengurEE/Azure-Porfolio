using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CryptoPortfolio : TableEntity
    {
        public CryptoPortfolio()
        {

        }
        public CryptoPortfolio(string User) { PartitionKey = "Cryptocurrency"; RowKey = User; }

        public string User { get; set; }
        public string TotalValue { get; set; }
        public List<CryptoPortfolioItem> Items { get; set; }
    }
}
