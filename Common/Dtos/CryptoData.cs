using System.Collections.Generic;

namespace Common.Dtos
{
    public class CryptoData
    {
        public List<CryptoRate> rates { get; set; } = new List<CryptoRate>();
    }
}