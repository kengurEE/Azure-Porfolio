using Common.Dtos;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoService
{
    public class CryptoServiceProvider
    {
        HashSet<string> currencies = new HashSet<string>()
        {
            "BTC", // Bitcoin
            "ETH", // Ethereum
            "BNB", // Binance Coin
            "USDT", // Tether
            "SOL", // Solana
            "ADA", // Cardano
            "XRP", // XRP
            "DOT", // Polkadot
            "DOGE", // Dogecoin
            "USDC" // USD Coin
        };

        public void ReadAndSaveCryptocurrencies()
        {
            return;
            CryptoData cryptoData = CryptoApiService.GetCryptoData();
            CryptocurrencyRepository cryptoRepository = new CryptocurrencyRepository();
            Dictionary<string, Cryptocurrency> cryptocurrencies = cryptoRepository.GetAll().ToDictionary(x => x.RowKey);
            foreach (var item in cryptoData.rates.Where(x => currencies.Contains(x.asset_id_quote)))
            {
                if (cryptocurrencies.TryGetValue(item.asset_id_quote, out var currency))
                {
                    currency.Value = 1 / item.rate;
                    cryptoRepository.Update(currency);
                }
                else
                {
                    currency = new Cryptocurrency(item.asset_id_quote) { Value = 1 / item.rate };
                    cryptoRepository.Add(currency);
                    cryptocurrencies[item.asset_id_quote] = currency;
                }
            }
        }
    }
}
