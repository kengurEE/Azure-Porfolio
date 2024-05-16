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
        private CloudTable _table;
        public UserRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("users");
            _table.CreateIfNotExists();
        }
        public List<User> GetAll()
        {
            return _table.CreateQuery<User>().Where(x => x.PartitionKey == "User").ToList();
        }
        public void Add(User user)
        {
            TableOperation insertOperation = TableOperation.Insert(user);
            _table.Execute(insertOperation);
        }

        public User Get(string username)
        {
            return GetAll().Where(p => p.RowKey == username).FirstOrDefault();
        }

        public void Update(User user)
        {
            TableOperation updateOperation = TableOperation.Replace(user);
            _table.Execute(updateOperation);
        }
    }
}