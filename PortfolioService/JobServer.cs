using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace PortfolioService
{
    public class JobServer
    {
        private ServiceHost serviceHost1;
        private ServiceHost serviceHost2;
        private string endPointName1 = "Portfolio";
        private string endPointName2 = "health-monitoring";
        public JobServer()
        {
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalRequest"];
            string endpoint1 = string.Format("net.tcp://{0}/{1}", inputEndPoint.IPEndpoint, endPointName1);
            string endpoint2 = string.Format("net.tcp://{0}/{1}", inputEndPoint.IPEndpoint, endPointName2);
            serviceHost1 = new ServiceHost(typeof(PortfolioServiceProvider));
            serviceHost2 = new ServiceHost(typeof(HealthMonitoring));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost1.AddServiceEndpoint(typeof(IPortfolioService), binding, endpoint1);
            serviceHost2.AddServiceEndpoint(typeof(IHealthMonitoring), binding, endpoint2);
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
