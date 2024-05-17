using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IMailService
    {
        [OperationContract]
        void Add(string address);
        [OperationContract]
        void Delete(string address);
        [OperationContract]
        List<string> Get();
    }
}
