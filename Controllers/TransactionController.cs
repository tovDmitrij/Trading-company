using Microsoft.AspNetCore.Mvc;
namespace Trading_company.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Buy() => View();
        public IActionResult Sell() => View();
    }
}