using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Models
{
    public class CryptoPortfolioItem : TableEntity
    {
        public CryptoPortfolioItem()
        {

        }
        public CryptoPortfolioItem(string portfolio, string cryptocurrency) { PartitionKey = "Cryptocurrency"; RowKey = portfolio + cryptocurrency; }
        public string Portfolio { get; set; }
        public string Cryptocurrency { get; set; }
        public decimal Quantity { get; set; }
    }
}
