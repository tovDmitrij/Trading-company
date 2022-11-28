using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Misc;
using Trading_company.Models;
using Trading_company.ViewModels;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Оформление транзакции
    /// </summary>
    [Controller]
    public sealed class TransactionController : Controller
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

            ViewData["Min_Date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            IncomingViewModel ivm = new()
            {
                Contracts = _db.contracts_with_optional_info.FromSqlInterpolated($"select* from contracts_with_optional_info where man_id = {manager.man_id} and dayto >= {DateTime.Now}").ToList(),
                Products = _db.products_with_optional_info.FromSqlInterpolated($"select* from products_with_optional_info").ToList()
            };

            return View(ivm);
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

        /// <summary>
        /// Список транзакций
        /// </summary>
        public IActionResult List()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            TransactionViewModel tvm = new()
            {
                purchaseTransactions = _db.incoming_with_optional_info.FromSqlInterpolated($"select i.* from incoming_with_optional_info i left join Contracts c on i.contract_id = c.id where c.man_id = {manager.man_id}").ToList(),
                sellTransactions = _db.outgoing_with_optional_info.FromSqlInterpolated($"select o.* from outgoing_with_optional_info o left join Contracts c on o.contract_id = c.id where c.man_id = {manager.man_id}").ToList()
            };

            return View(tvm);
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

            if (_db.some_model.FromSqlInterpolated($"select value from Prices where prod_id = {transaction.prod_id} and dayfrom <= {transaction.transaction_date} and {transaction.transaction_date} <= dateto").ToList().FirstOrDefault() is null)
            {
                SetInfo(transaction, "Отсутствует действующий ценник на заданную дату");
                return Buy();
            }


            try
            {
                _db.incoming_with_optional_info.Add(transaction);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                /*
                 Здесь та же ошибка, что и при добавлении менеджера или контракта (см. ManagerController/SignUp)
                 */
            }

            return Redirect("~/Transaction/List");
        }

        /// <summary>
        /// Продажа товара
        /// </summary>
        /// <param name="transaction">Информация и транзакции</param>
        [HttpPost]
        public IActionResult Sell(OutgoingModel transaction)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            try
            {
                _db.outgoing_with_optional_info.Add(transaction);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                /*
                 Здесь та же ошибка, что и при добавлении менеджера или контракта (см. ManagerController/SignUp)
                 */
            }

            return Redirect("~/Transaction/List");

        }

        #endregion



        #region Прочее

        /// <summary>
        /// Отправить на форму введённую информацию о транзакции
        /// </summary>
        /// <param name="transaction">Менеджер</param>
        /// <param name="error">Ошибка, если она есть</param>
        [NonAction]
        private void SetInfo(IncomingModel transaction, string? error)
        {
            ViewData["Error"] = error;
            ViewData["Transaction_date"] = transaction.transaction_date.ToString("yyyy-MM-dd");
            ViewData["Transaction_quantity"] = transaction.prod_quantity;
            ViewData["Transaction_contrID"] = transaction.contract_id;
            ViewData["Transaction_productID"] = transaction.prod_id;
        }

        #endregion



    }
}