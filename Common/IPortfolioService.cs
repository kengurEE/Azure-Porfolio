using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IPortfolioService
    {
        [OperationContract]
        bool Register(UserDto user);
        [OperationContract]
        UserDto Get(string userId);
        [OperationContract]
        UserDto Login(string email, string password);
        [OperationContract]
        bool Update(UserDto user);
    }
}
