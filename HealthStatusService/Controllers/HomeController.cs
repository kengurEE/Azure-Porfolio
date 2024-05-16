using Common.Contracts;
using Common.Helpers;
using System.Linq;
using System.Web.Mvc;

namespace HealthStatusService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IHealthService healthService = WcfClientHelper<IHealthService>.Connect(Service.HealthMonitoring);
            var healthChecks = healthService.GetHealthChecks();
            var notificationChecks = healthChecks.Where(x => x.ServiceName == "Notification").ToList();
            var portfolioChecks = healthChecks.Where(x => x.ServiceName == "Portfolio").ToList();

            ViewBag.Notification = notificationChecks.Count(x => x.IsHealth) / notificationChecks.Count * 100;
            ViewBag.Portfolio = portfolioChecks.Count(x => x.IsHealth) / portfolioChecks.Count * 100;
            return View();
        }

    }
}