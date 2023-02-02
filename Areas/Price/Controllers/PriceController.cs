using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Areas.Price.Models;
using Trading_company.Controllers;
using Trading_company.Models;
namespace Trading_company.Areas.Price.Controllers
{
    /// <summary>
    /// Взаимодействие с ценами на товары
    /// </summary>
    [Area("Price")]
    [Controller]
    [Route("Price/{action}")]
    public sealed class PriceController : TradingCompanyController
    {
        public PriceController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Отобразить таблицу с ценами на товары
        /// </summary>
        public IActionResult List()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var priceList = _db.prices_with_optional_info
                .Select(x => x)
                .OrderBy(x => x.product_name)
                .ToList()
                .DistinctBy(x => x.product_name);

            return View(priceList);
        }

        #endregion



    }
}