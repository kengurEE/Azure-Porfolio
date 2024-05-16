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
        public List<HealthCheckDto> GetHealthChecks()
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
                    Timestamp = hc.Timestamp
                });
            }
            return healthCheckDtos;
        }
    }
}
