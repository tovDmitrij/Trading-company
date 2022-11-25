using Microsoft.AspNetCore.Mvc;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Оформление транзакции
    /// </summary>
    [Controller]
    public class TransactionController : Controller
    {
        /// <summary>
        /// БД "Торговое предприятие"
        /// </summary>
        private readonly DataContext _db;

        /// <param name="context">БД "Торговое предприятие"</param>
        public TransactionController(DataContext context) => _db = context;



        #region Страницы
        /// <summary>
        /// Покупка товара
        /// </summary>
        public IActionResult Buy()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            return View();
        }

        /// <summary>
        /// Продажа товара
        /// </summary>
        public IActionResult Sell()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            return View();
        }
        #endregion



        #region Действия на страницах
        /// <summary>
        /// Покупка товара
        /// </summary>
        /// <param name="transaction">Информация и транзакции</param>
        [HttpPost]
        public IActionResult Buy(IncomingModel transaction)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }



            return Redirect("~/Manager/PersonalArea");
        }
        #endregion



        #region Прочее

        #endregion
    }
}