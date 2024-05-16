using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Dtos
{
    public class CryptoData
    {
        public List<CryptoRate> Rates { get; set; }
    }
}