
using Common.Dtos;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoService
{
    public class CryptoApiService
    {
        public static CryptoData GetCryptoData()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpClient = new HttpClient { BaseAddress = new Uri("https://rest.coinapi.io/v1/exchangerate/USD") };
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key", "4C6E5269-1F13-4159-A62C-9943370389EA");
            var response = httpClient.GetAsync("").Result;
            string str = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<CryptoData>(str);
        }
    }
}
