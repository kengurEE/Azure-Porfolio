using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService
{
    public class MailRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public MailRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("mail");
            _table.CreateIfNotExists();
        }
        public List<string> Get()
        {
            return _table.CreateQuery<Mail>().Where(x => x.PartitionKey == "mail").Select(x => x.Address).ToList();
        }
        public void Add(Mail mail)
        {
            TableOperation insertOperation = TableOperation.Insert(mail);
            _table.Execute(insertOperation);
        }

        public bool Delete(string address)
        {
            Mail transaction = _table.CreateQuery<Mail>().Where(x => x.Address == address).FirstOrDefault();
            if (transaction == null)
                return false;
            _table.Execute(TableOperation.Delete(transaction));
            return true;
        }
    }
}
