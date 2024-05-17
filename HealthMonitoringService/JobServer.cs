using Common;
using Common.Contracts;
using HealthMonitoringService;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace PortfolioService
{
    public class JobServer
    {
        private ServiceHost serviceHost1;
        private string endPointName1 = "HealthMonitor";
        private ServiceHost serviceHost2;
        private string endPointName2 = "Mail";
        public JobServer()
        {
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalRequest"];
            string endpoint = string.Format("net.tcp://{0}/{1}", inputEndPoint.IPEndpoint, endPointName1);
            serviceHost1 = new ServiceHost(typeof(HealthServiceProvider));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost1.AddServiceEndpoint(typeof(IHealthService), binding, endpoint);


            RoleInstanceEndpoint inputEndPoint2 = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Input"];
            string endpoint2 = string.Format("net.tcp://{0}/{1}", inputEndPoint2.IPEndpoint, endPointName2);
            serviceHost2 = new ServiceHost(typeof(MailService));
            serviceHost2.AddServiceEndpoint(typeof(IMailService), binding, endpoint2);
        }
        public void Open()
        {
            try
            {
                serviceHost1.Open();
                Trace.TraceInformation(string.Format("Host for {0} endpoint type opened successfully at {1}", endPointName1, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", endPointName1, e.Message);
            }

            try
            {
                serviceHost2.Open();
                Trace.TraceInformation(string.Format("Host for {0} endpoint type opened successfully at {1}", endPointName2, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", endPointName2, e.Message);
            }

        }
        public void Close()
        {
            try
            {
                serviceHost1.Close();
                Trace.TraceInformation(string.Format("Host for {0} endpoint type closed successfully at {1}", endPointName1, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Error message is: {1}. ", endPointName1, e.Message);
            }
            try
            {
                serviceHost2.Close();
                Trace.TraceInformation(string.Format("Host for {0} endpoint type closed successfully at {1}", endPointName2, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Error message is: {1}. ", endPointName2, e.Message);
            }
        }
    }
}
