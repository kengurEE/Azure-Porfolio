using System.Net.Http.Headers;
using System.ServiceModel;
namespace Common.Helpers
{

    public static class WcfClientHelper<T>
    {
        public static T Connect(Service service)
        {
            var binding = new NetTcpBinding();
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, new EndpointAddress(ResolveAddress(service)));
            return factory.CreateChannel();
        }
        private static string ResolveAddress(Service service)
        {
            switch (service)
            {
                case Service.Portfolio:
                    if (typeof(T) is IPortfolioService)
                        return "net.tcp://127.255.0.5:10100/Portfolio";
                    if (typeof(T) is IHealthMonitoring)
                        return "net.tcp://127.255.0.5:10100/health-monitoring";
                    break;
                case Service.Notification:
                    if (typeof(T) is IHealthMonitoring)
                        return "net.tcp://127.255.0.5:10100/health-monitoring";
                    break;

            }
            throw new System.Exception("Service address does not exists");
        }

    }
}