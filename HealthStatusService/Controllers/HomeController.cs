using Common.Contracts;
using Common.Dtos;
using Common.Helpers;
using System.Web.Mvc;

namespace HealthStatusService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IHealthService healthService = WcfClientHelper.Connect<IHealthService>("HealthMonitoringService", "HealthMonitor");
            HealthCheckReport report = healthService.GetHealthChecks();
            ViewBag.Portfolio = report.Portfolio;
            ViewBag.Notification = report.Notification;
            ViewBag.HealthChecks = report.HealthChecks;
            return View();
        }

    }
}