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
        public void ReadAndSaveCrypto()
        {
            CryptoData cryptoData = CryptoApiService.GetCryptoData();
            CryptoRepository cryptoRepository = new CryptoRepository();
            Dictionary<string, Cryptocurrency> cryptocurrencies = cryptoRepository.GetAll().ToDictionary(x => x.RowKey);
            foreach (var item in cryptoData.Rates)
            {
                if (cryptocurrencies.TryGetValue(item.asset_id_quote, out var currency))
                {
                    currency.Rate = 1 / item.Rate;
                    cryptoRepository.Update(currency);
                }
                else
                {
                    currency = new Cryptocurrency(item.asset_id_quote) { Rate = 1 / item.Rate };
                    cryptoRepository.Add(currency);
                    cryptocurrencies[item.asset_id_quote] = currency;
                }
                CheckAlarms();
            }
        }

        private void CheckAlarms()
        {
            throw new NotImplementedException();
        }
    }
}
