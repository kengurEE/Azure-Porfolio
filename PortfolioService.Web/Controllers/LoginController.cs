using Common;
using Common.Helpers;
using Common.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PortfolioService.Web.Controllers
{
    public class LoginController : Controller
    {
        IPortfolioService portfolioService = WcfClientHelper.Connect<IPortfolioService>("PortfolioService", "Portfolio");
        public ActionResult Index(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        public ActionResult Registration(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Update(string errorMessage)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");
            ViewData["user"] = Session["user"];
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = portfolioService.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Index", new { errorMessage = "Pogrešni kredencijali" });
            }
            Session["user"] = user;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult RegistrationSubmit(UserDto userDto, HttpPostedFileBase file)
        {
            if (file != null)
            {
                BlobHelper blob = new BlobHelper();
                userDto.Image = blob.Upload(file);
            }
            portfolioService.Register(userDto);
            return RedirectToAction("Index");
        }
        public ActionResult UpdateSubmit(UserDto userDto, HttpPostedFileBase file)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");
            if (file != null)
            {
                BlobHelper blob = new BlobHelper();
                userDto.Image = blob.Upload(file);
            }
            portfolioService.Update(userDto);
            return RedirectToAction("Update");
        }
    }
}