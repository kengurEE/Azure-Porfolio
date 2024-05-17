using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Models
{
    public class Cryptocurrency : TableEntity
    {
        public Cryptocurrency()
        {

        }
        public Cryptocurrency(string code) { PartitionKey = "cryptocurrency"; RowKey = code; }
        public double Value { get; set; }
    }
}
