﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Misc;
using Trading_company.Models;
using Trading_company.Controllers;
using Trading_company.Areas.Transaction.Models;
using Trading_company.Areas.Transaction.ViewModels;
namespace Trading_company.Areas.Transaction.Controllers
{
    /// <summary>
    /// Оформление транзакции
    /// </summary>
    [Area("Transaction")]
    [Controller]
    [Route("Transaction/{action}")]
    public sealed class TransactionController : TradingCompanyController
    {
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
            ManagerModel manager = _db.managers_with_optional_info
                .FirstOrDefault(x => x.email == managerInfo.email && x.password == Security.HashPassword(managerInfo.password));

            IncomingViewModel ivm = new()
            {
                Contracts = _db.contracts_with_optional_info
                    .Where(x => x.man_id == manager.man_id && x.dayto >= DateTime.Now)
                    .ToList(),
                Products = _db.products_with_optional_info
                    .Select(x => x)
                    .ToList()
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

            ViewData["Min_Date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers_with_optional_info
                .FirstOrDefault(x => x.email == managerInfo.email && x.password == Security.HashPassword(managerInfo.password));

            OutgoingViewModel ovm = new()
            {
                Contracts = _db.contracts_with_optional_info
                    .Where(x => x.man_id == manager.man_id && x.dayto >= DateTime.Now)
                    .ToList(),
                Products = _db.products_with_optional_info
                    .Select(x => x)
                    .ToList()
            };

            return View(ovm);
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
            ManagerModel manager = _db.managers_with_optional_info
                .FirstOrDefault(x => x.email == managerInfo.email && x.password == Security.HashPassword(managerInfo.password));

            TransactionViewModel tvm = new()
            {
                PurchaseTransactions = _db.incoming_with_optional_info
                    .FromSqlInterpolated($"select i.* from incoming_with_optional_info i left join Contracts c on i.contract_id = c.id where c.man_id = {manager.man_id}")
                    .OrderBy(transaction => transaction.transaction_date)
                    .ToList(),
                SellTransactions = _db.outgoing_with_optional_info
                    .FromSqlInterpolated($"select o.* from outgoing_with_optional_info o left join Contracts c on o.contract_id = c.id where c.man_id = {manager.man_id}")
                    .OrderBy(transaction => transaction.transaction_date)
                    .ToList()
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

            try
            {
                _db.incoming_with_optional_info.Add(transaction);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

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
            catch (Exception ex) { }

            return Redirect("~/Transaction/List");
        }

        /// <summary>
        /// Отменить транзакцию покупки товара
        /// </summary>
        /// <param name="transaction">Отменяемая транзакция</param>
        [HttpPost]
        public IActionResult DeleteBought(IncomingModel transaction)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var currentTransaction = _db.incoming_with_optional_info
                .FirstOrDefault(x => x.transaction_id == transaction.transaction_id);

            try
            {
                _db.incoming_with_optional_info.Remove(currentTransaction);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

            return Redirect("~/Transaction/List");
        }

        /// <summary>
        /// Отменить транзакцию продажи товара
        /// </summary>
        /// <param name="transaction">Отменяемая транзакция</param>
        [HttpPost]
        public IActionResult DeleteSold(OutgoingModel transaction)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var currentTransaction = _db.outgoing_with_optional_info
                .FirstOrDefault(x => x.transaction_id == transaction.transaction_id);

            try
            {
                _db.outgoing_with_optional_info.Remove(currentTransaction);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

            return Redirect("~/Transaction/List");
        }

        #endregion


        
    }
}