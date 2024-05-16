using Common;
using System;

namespace NotificationService
{
    internal class HealthMonitoring : IHealthMonitoring
    {
        public bool HealthCheck()
        {
            Console.WriteLine("Worker role checked in.");
            return true;
        }
    }
}
