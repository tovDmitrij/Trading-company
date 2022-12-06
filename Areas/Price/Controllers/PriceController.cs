using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Trading_company.Controllers;
using Trading_company.Models;
namespace Trading_company.Areas.Price.Controllers
{
    /// <summary>
    /// Взаимодействие с ценами на товары
    /// </summary>
    [Area("Price")]
    [Controller]
    public class PriceController : TradingCompanyController
    {
        public PriceController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Отобразить таблицу с ценами на товары
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult List()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var priceList = _db.prices_with_optional_info.FromSqlInterpolated($"select distinct product_name, product_id, now() price_dayfrom, now() price_dayto, 0 price_value from prices_with_optional_info order by product_name").ToList();

            return View(priceList);
        }

        #endregion



    }
}