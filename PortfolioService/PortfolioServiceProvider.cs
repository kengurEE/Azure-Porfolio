using Common;
using Common.Dtos;
using Common.Helpers;
using Common.Models;
using CryptoService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PortfolioService
{
    public class PortfolioServiceProvider : IPortfolioService
    {
        TransactionRepository transactionRepository = new TransactionRepository();
        CryptocurrencyRepository cryptocurrencyRepository = new CryptocurrencyRepository();
        public CryptoPortfolioDto GetPortfolio(string user)
        {

            return PortfolioHelper.GetPortfolio(user);
        }

        public List<TransactionDto> GetTransactions(string user)
        {
            List<Transaction> transactions = transactionRepository.Get(user);
            List<TransactionDto> transactionDtos = new List<TransactionDto>();
            foreach (var transaction in transactions)
            {
                transactionDtos.Add(new TransactionDto
                {
                    Currency = transaction.Currency,
                    Price = transaction.Price,
                    Quantity = transaction.Quantity,
                    User = transaction.User,
                    Id = transaction.RowKey,
                    IsInvest = transaction.IsInvested,
                });
            }
            return transactionDtos;
        }
        public bool AddTransaction(TransactionDto transactionDto)
        {
            var portfolio = PortfolioHelper.GetPortfolio(transactionDto.User);

            var item = portfolio.Items.FirstOrDefault(x => x.Currency == transactionDto.Currency);
            if (!transactionDto.IsInvest)
            {
                if (item == null || item.Quantity < transactionDto.Quantity)
                    return false;
            }

            Transaction transaction = new Transaction()
            {
                Quantity = transactionDto.Quantity,
                Currency = transactionDto.Currency,
                IsInvested = transactionDto.IsInvest,
                Price = transactionDto.Price,
                User = transactionDto.User
            };
            transactionRepository.Add(transaction);

            return true;
        }
        public bool DeleteTransaction(string id)
        {
            return transactionRepository.Delete(id);
        }

        public UserDto Login(string email, string password)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.Get(email);
            if (user is null)
                return null;
            if (user.Password == password)
                return new UserDto
                {
                    City = user.City,
                    Street = user.Address.Street,
                    Number = user.Address.Number,
                    Country = user.Country,
                    Email = email,
                    FirstName = user.FirstName,
                    Image = user.ImageUrl,
                    LastName = user.LastName,
                    Phone = user.Phone
                };
            return null;
        }

        public bool Register(UserDto userDto)
        {
            User user = new User()
            {
                Address = new Address { Number = userDto.Number, Street = userDto.Street },
                City = userDto.City,
                Country = userDto.Country,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                ImageUrl = userDto.Image,
                LastName = userDto.LastName,
                Password = userDto.Password,
                Phone = userDto.Phone,

            };

            UserRepository userRepository = new UserRepository();
            var existingUser = userRepository.Get(user.Email);
            if (existingUser is null)
            {
                userRepository.Add(user);
                return true;
            }
            return false;

        }

        public bool Update(UserDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            var existingUser = userRepository.Get(userDto.Email);
            if (existingUser is null)
                return false;
            existingUser.Address.Number = userDto.Number;
            existingUser.Address.Street = userDto.Street;
            existingUser.City = userDto.City;
            existingUser.Country = userDto.Country;
            existingUser.Email = userDto.Email;
            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            if (!string.IsNullOrEmpty(userDto.Password))
                existingUser.Password = userDto.Password;
            if (!string.IsNullOrEmpty(userDto.Image))
                existingUser.ImageUrl = userDto.Image;
            existingUser.Phone = userDto.Phone;

            userRepository.Update(existingUser);
            return true;
        }

        public List<CryptocurrencyDto> GetCryptocurrencies()
        {
            CryptocurrencyRepository cryptocurrencyRepository = new CryptocurrencyRepository();
            var currencies = cryptocurrencyRepository.GetAll();
            List<CryptocurrencyDto> cryptocurrencyDtos = new List<CryptocurrencyDto>();
            foreach (var cryptocurrency in currencies)
            {
                cryptocurrencyDtos.Add(new CryptocurrencyDto { Code = cryptocurrency.RowKey, Value = cryptocurrency.Value });
            }
            return cryptocurrencyDtos;
        }

        public void AddAlarm(string email, double limit, string currency)
        {
            AlarmRepository cryptocurrencyRepository = new AlarmRepository();
            cryptocurrencyRepository.Add(new Alarm
            {
                Limit = limit,
                LastAccess = DateTime.MinValue,
                Currency = currency,
                Email = email,
                Active = true
            });
        }
    }
}
