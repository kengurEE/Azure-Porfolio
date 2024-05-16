using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoService
{
    public class CryptoRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public CryptoRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("cryptocurrencies");
            _table.CreateIfNotExists();
        }
        public List<Cryptocurrency> GetAll()
        {
            return _table.CreateQuery<Cryptocurrency>().Where(x => x.PartitionKey == "cryptocurrency").ToList();
        }
        public void Add(Cryptocurrency user)
        {
            TableOperation insertOperation = TableOperation.Insert(user);
            _table.Execute(insertOperation);
        }

        public Cryptocurrency Get(string key)
        {
            return GetAll().Where(p => p.RowKey == key).FirstOrDefault();
        }

        public void Update(Cryptocurrency cryptocurrency)
        {
            TableOperation updateOperation = TableOperation.Replace(cryptocurrency);
            _table.Execute(updateOperation);
        }
    }
}
