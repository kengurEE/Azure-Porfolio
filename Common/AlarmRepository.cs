using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class AlarmRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public AlarmRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("alarm");
            _table.CreateIfNotExists();
        }
        public Alarm Get(string id)
        {
            return _table.CreateQuery<Alarm>().Where(x => x.PartitionKey == "alarm" && x.RowKey == id).First();
        }
        public List<Alarm> Get20()
        {
            var alarms = _table.CreateQuery<Alarm>().Where(x => x.PartitionKey == "alarm").AsEnumerable().Where(x => x.Active).OrderBy(x => x.LastAccess).Take(20).ToList();

            /*  foreach (var alarm in alarms)
              {
                  alarm.LastAccess = DateTime.Now;
                  TableOperation updateOperation = TableOperation.Replace(alarm);
                  _table.Execute(updateOperation);
              }*/
            return alarms;
        }
        public void Add(Alarm alarm)
        {
            TableOperation insertOperation = TableOperation.Insert(alarm);
            _table.Execute(insertOperation);
        }


        public void Update(Alarm alarm)
        {
            TableOperation updateOperation = TableOperation.Replace(alarm);
            _table.Execute(updateOperation);
        }
    }
}
