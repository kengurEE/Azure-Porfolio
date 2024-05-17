using Common.Dtos;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IPortfolioService
    {
        [OperationContract]
        bool Register(UserDto user);
        [OperationContract]
        UserDto Login(string email, string password);
        [OperationContract]
        bool Update(UserDto user);
        [OperationContract]
        CryptoPortfolioDto GetPortfolio(string email);
        [OperationContract]
        List<TransactionDto> GetTransactions(string email);
        [OperationContract]
        List<CryptocurrencyDto> GetCryptocurrencies();
        [OperationContract]
        bool AddTransaction(TransactionDto transactionDto);
        [OperationContract]
        bool DeleteTransaction(string id);
        [OperationContract]
        void AddAlarm(string email, double limit, string currency);
    }
}
