using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Areas.Warehouse.Models;
using Trading_company.Controllers;
using Trading_company.Models;
namespace Trading_company.Areas.Warehouse.Controllers
{
    /// <summary>
    /// Взаимодействие со складом
    /// </summary>
    [Area("Warehouse")]
    public class WarehouseController : TradingCompanyController
    {
        public WarehouseController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Отобразить склад
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult Show()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            List<WarehouseModel> productList = _db.warehouse.FromSqlInterpolated($"select* from warehouse").ToList();
            return View(productList);
        }

        #endregion



    }
}