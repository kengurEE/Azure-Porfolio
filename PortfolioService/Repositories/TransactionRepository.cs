using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PortfolioService
{
    public class TransactionRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public TransactionRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("transaction");
            _table.CreateIfNotExists();
        }
        public List<Transaction> Get(string user)
        {
            return _table.CreateQuery<Transaction>().Where(x => x.PartitionKey == "transaction" && x.User == user).ToList();
        }
        public void Add(Transaction transaction)
        {
            TableOperation insertOperation = TableOperation.Insert(transaction);
            _table.Execute(insertOperation);
        }

        public bool Delete(string id)
        {
            Transaction transaction = _table.CreateQuery<Transaction>().Where(x => x.RowKey == id).FirstOrDefault();
            if (transaction == null)
                return false;
            _table.Execute(TableOperation.Delete(transaction));
            return true;
        }
    }
}