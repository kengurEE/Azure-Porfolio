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

        public ActionResult Update(string errorMessage)
        {
            ViewData["user"] = Session["user"];
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            IPortfolioService portfolioService = WcfClientHelper<IPortfolioService>.Connect(Service.Portfolio);
            var user = portfolioService.Login(username, password);
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

            byte[] imageBytes = new byte[file.ContentLength];
            using (BinaryReader theReader = new BinaryReader(file.InputStream))
            {
                imageBytes = theReader.ReadBytes(file.ContentLength);
            }
            BlobHelper blob = new BlobHelper();

            userDto.Image = blob.Upload(Convert.ToBase64String(imageBytes), "images", Guid.NewGuid().ToString());

            IPortfolioService portfolioService = WcfClientHelper<IPortfolioService>.Connect(Service.Portfolio);
            portfolioService.Register(userDto);
            return RedirectToAction("Index");
        }
        public ActionResult UpdateSubmit(UserDto userDto, HttpPostedFileBase file)
        {

            byte[] imageBytes = new byte[file.ContentLength];
            using (BinaryReader theReader = new BinaryReader(file.InputStream))
            {
                imageBytes = theReader.ReadBytes(file.ContentLength);
            }
            BlobHelper blob = new BlobHelper();

            userDto.Image = blob.Upload(Convert.ToBase64String(imageBytes), "images", Guid.NewGuid().ToString());
            IPortfolioService portfolioService = WcfClientHelper<IPortfolioService>.Connect(Service.Portfolio);
            portfolioService.Update(userDto);
            return RedirectToAction("Update");
        }
    }
}