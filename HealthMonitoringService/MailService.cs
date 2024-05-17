using Common.Contracts;
using System.Collections.Generic;

namespace HealthMonitoringService
{
    public class MailService : IMailService
    {
        MailRepository MailRepository = new MailRepository();
        public void Add(string address)
        {
            MailRepository.Add(new Mail { Address = address });
        }

        public void Delete(string address)
        {
            MailRepository.Delete(address);
        }

        public List<string> Get()
        {
            return MailRepository.Get();
        }
    }
}
