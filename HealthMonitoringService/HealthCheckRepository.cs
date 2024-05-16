using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioService
{
    public class HealthCheckRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public HealthCheckRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("healthcheck");
            _table.CreateIfNotExists();
        }
        public List<HealthCheck> GetAll()
        {
            return _table.CreateQuery<HealthCheck>().Where(x => x.PartitionKey == "healthcheck").ToList();
        }
        public void Add(HealthCheck healthCheck)
        {
            TableOperation insertOperation = TableOperation.Insert(healthCheck);
            _table.Execute(insertOperation);
        }

        public HealthCheck Get(string username)
        {
            return GetAll().Where(p => p.RowKey == username).FirstOrDefault();
        }

        public void Update(HealthCheck user)
        {
            TableOperation updateOperation = TableOperation.Replace(user);
            _table.Execute(updateOperation);
        }
    }
}