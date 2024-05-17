using Common.Contracts;
using Common.Dtos;
using Common.Models;
using PortfolioService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthMonitoringService
{
    public class HealthServiceProvider : IHealthService
    {
        public HealthCheckReport GetHealthChecks()
        {
            HealthCheckRepository healthCheckRepository = new HealthCheckRepository();
            List<HealthCheck> healthChecks = healthCheckRepository.GetAll()
                                                    .Where(x => x.Timestamp > DateTime.Now.AddDays(-1))
                                                    .ToList();
            List<HealthCheckDto> healthCheckDtos = new List<HealthCheckDto>();
            foreach (var hc in healthChecks)
            {
                healthCheckDtos.Add(new HealthCheckDto
                {
                    Id = hc.RowKey,
                    IsHealth = hc.IsHealth,
                    ServiceName = hc.ServiceName,
                    Timestamp = hc.Timestamp.DateTime
                });
            }
            var notificationChecks = healthChecks.Where(x => x.ServiceName == "Notification").ToList();
            var portfolioChecks = healthChecks.Where(x => x.ServiceName == "Portfolio").ToList();
            return new HealthCheckReport
            {
                Notification = Math.Round((double)notificationChecks.Count(x => x.IsHealth) / notificationChecks.Count * 100, 2),
                Portfolio = Math.Round((double)portfolioChecks.Count(x => x.IsHealth) / portfolioChecks.Count * 100, 2),
                HealthChecks = healthCheckDtos.Take(50).ToList()
            };
        }
    }
}
