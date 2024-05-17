using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioService
{
    public class UserRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _tableUsers;
        private CloudTable _tableAddresses;
        public UserRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _tableUsers = tableClient.GetTableReference("users");
            _tableUsers.CreateIfNotExists();
            _tableAddresses = tableClient.GetTableReference("users");
            _tableAddresses.CreateIfNotExists();
        }
        public List<User> GetAll()
        {
            var users = _tableUsers.CreateQuery<User>().Where(x => x.PartitionKey == "User").ToList();
            var addresses = _tableAddresses.CreateQuery<Address>().Where(x => x.PartitionKey == "Address").ToDictionary(x => x.RowKey);
            foreach (var item in users)
            {
                item.Address = addresses[item.AddressId];
            }
            return users;
        }
        public void Add(User user)
        {
            TableOperation insertOperation = TableOperation.Insert(user.Address);
            _tableAddresses.Execute(insertOperation);
            user.AddressId = user.Address.RowKey;
            insertOperation = TableOperation.Insert(user);
            _tableUsers.Execute(insertOperation);
        }

        public User Get(string email)
        {
            return GetAll().Where(p => p.Email == email).FirstOrDefault();
        }

        public void Update(User user)
        {
            TableOperation updateOperation = TableOperation.Replace(user);
            _tableUsers.Execute(updateOperation);
            updateOperation = TableOperation.Replace(user.Address);
            _tableUsers.Execute(updateOperation);
        }
    }
}