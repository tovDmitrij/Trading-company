using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Misc;
using Trading_company.Models;
using Trading_company.Controllers;
using Trading_company.Areas.Contract.ViewModels;
namespace Trading_company.Areas.Contract.Controllers
{
    /// <summary>
    /// Взаимодействие с контрактом
    /// </summary>
    [Area("Contract")]
    [Controller]
    [Route("Contract/{action}")]
    public sealed class ContractController : TradingCompanyController
    {
        public ContractController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Подписать новый контракт
        /// </summary>
        public IActionResult New()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel currentManager = _db.managers_with_optional_info
                .FirstOrDefault(x => x.email == managerInfo.email && x.password == Security.HashPassword(managerInfo.password));

            ContractViewModel cvm = new()
            {
                Contragents = _db.contragents
                    .FromSqlInterpolated($"select * from Contragents except select cg.* from Contracts cr left join Contragents cg on cr.Contr_ID = cg.Contr_ID where cr.man_id = {currentManager.man_id} and cr.dayto >= now()")
                    .OrderBy(x => x.contr_id)
                    .ToList()
            };

            ViewData["Contract_MinDate"] = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            return View(cvm);
        }

        /// <summary>
        /// Посмотреть список контрактов
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

            ContractViewModel cvm = new()
            {
                Contracts = _db.contracts_with_optional_info
                    .Where(x => x.man_id == manager.man_id)
                    .ToList()
            };

            return View(cvm);
        }

        #endregion



        #region Действия на страницах

        /// <summary>
        /// Подписать новый контракт
        /// </summary>
        /// <param name="contract">Информация о новом контракте</param>
        [HttpPost]
        public IActionResult New(ContractModel contract)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel currentManager = _db.managers_with_optional_info
                .FirstOrDefault(x => x.email == managerInfo.email && x.password == Security.HashPassword(managerInfo.password));

            contract.man_id = currentManager.man_id;
            contract.dayfrom = DateTime.Now;

            try
            {
                _db.contracts_with_optional_info.Add(contract);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

            return Redirect("~/Contract/List");
        }

        /// <summary>
        /// Обновить действующий контракт
        /// </summary>
        /// <param name="contract">Обновляемый контракт</param>
        [HttpPost]
        public IActionResult Update(ContractModel contract)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var currentContract = _db.contracts_with_optional_info
                .FirstOrDefault(x => x.id == contract.id);

            currentContract.comments = $"Менеджер сдвинул срок окончания контракта с даты {currentContract.dayto:dd-MM-yyyy} на дату {contract.dayto:dd-MM-yyyy}";

            currentContract.dayto = contract.dayto;

            _db.SaveChanges();

            return Redirect("~/Contract/List");
        }

        #endregion



    }
}