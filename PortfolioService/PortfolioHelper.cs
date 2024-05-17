using Common.Models;
using CryptoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioService
{
    public static class PortfolioHelper
    {
        public static CryptoPortfolioDto GetPortfolio(string user)
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            CryptocurrencyRepository cryptocurrencyRepository = new CryptocurrencyRepository();

            var transactions = transactionRepository.Get(user);
            var cryptocurrencies = cryptocurrencyRepository.GetAll().ToDictionary(x => x.RowKey, x => x.Value);
            List<CryptoPortfolioItemDto> portfolioItems = new List<CryptoPortfolioItemDto>();
            var transactionGroups = transactions.GroupBy(x => x.Currency);
            foreach (var transactionGroup in transactionGroups)
            {
                portfolioItems.Add(new CryptoPortfolioItemDto
                {
                    Currency = transactionGroup.Key,
                    Invested = transactionGroup.Sum(x => x.Amount * (x.IsInvested ? 1 : -1)),
                    Price = cryptocurrencies[transactionGroup.Key],
                    Quantity = transactionGroup.Sum(x => x.Quantity * (x.IsInvested ? 1 : -1)),
                });
            }
            return new CryptoPortfolioDto
            {
                Items = portfolioItems,
                User = user
            };
        }
    }
}
