using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PortfolioService.Web.Controllers
{
    public class HomeController : Controller
    {
        IPortfolioService portfolioService = WcfClientHelper.Connect<IPortfolioService>("PortfolioService", "Portfolio");
        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");

            CryptoPortfolioDto portfolio = portfolioService.GetPortfolio((Session["user"] as UserDto).Email);
            ViewBag.Portfolio = portfolio;
            return View();
        }
        public ActionResult Transactions()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");

            List<TransactionDto> transactions = portfolioService.GetTransactions((Session["user"] as UserDto).Email);
            ViewBag.Transactions = transactions;

            return View();
        }
        public ActionResult AddTransaction(string message)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Message = message;
            ViewBag.Currencies = portfolioService.GetCryptocurrencies();

            return View();
        }
        public ActionResult DeleteTransaction(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Currencies = portfolioService.DeleteTransaction(id);
            return RedirectToAction("Transactions");

        }
        public ActionResult AddAlarm(double? limit, string currency)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");
            if (limit <= 0 || limit == null)
                return RedirectToAction("Index");

            portfolioService.AddAlarm((Session["user"] as UserDto).Email, limit.Value, currency);

            return RedirectToAction("Index");
        }
        public ActionResult AddTransactionSubmit(double? quantity, string currency, double? price, string isInvest, string useCurrentPrice)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index", "Login");
            if (currency == null || quantity < 0)
                return RedirectToAction("AddTransaction", new { message = "Nevalidna transakcija" });
            TransactionDto transactionDto = new TransactionDto
            {
                Currency = currency,
                IsInvest = isInvest == "on",
                Quantity = quantity.Value,
                User = (Session["user"] as UserDto).Email,

            };
            if (useCurrentPrice == "on")
            {
                var currencies = portfolioService.GetCryptocurrencies();
                transactionDto.Price = currencies.First(x => x.Code == transactionDto.Currency).Value;
            }
            else
            {
                if (!price.HasValue || price < 0)
                    return RedirectToAction("AddTransaction", new { message = "Nevalidna transakcija" });
                transactionDto.Price = price.Value;
            }

            if (portfolioService.AddTransaction(transactionDto))
                return RedirectToAction("Transactions");
            return RedirectToAction("AddTransaction", new { message = "Nevalidna transakcija" });
        }
    }
}