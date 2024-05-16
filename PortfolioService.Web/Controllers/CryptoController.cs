using System.Web.Mvc;

namespace PortfolioService.Web.Controllers
{
    public class CryptoController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");
            return View();
        }
    }
}