using Microsoft.AspNetCore.Mvc;
namespace Trading_company.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}