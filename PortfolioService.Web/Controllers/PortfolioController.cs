using System.Web.Mvc;

namespace PortfolioService.Web.Controllers
{
    public class PortfolioController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
    }
}