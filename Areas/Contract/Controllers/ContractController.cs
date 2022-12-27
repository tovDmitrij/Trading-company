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
            ManagerModel currentManager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            ContractViewModel cvm = new()
            {
                Contragents = _db.contragents.FromSqlInterpolated(
                $"select * from Contragents except select cg.* from Contracts cr left join Contragents cg on cr.Contr_ID = cg.Contr_ID where cr.man_id = {currentManager.man_id} and cr.dayto >= now()").OrderBy(contragent => contragent.contr_id).ToList()
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
            ManagerModel manager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            var contractList = _db.contracts_with_optional_info.FromSql($"select* from contracts_with_optional_info where man_id = {manager.man_id}");

            ContractViewModel cvm = new()
            {
                Contracts = contractList.ToList()
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
            ManagerModel currentManager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

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

            var currentContract = _db.contracts_with_optional_info.FirstOrDefault(contr => contr.id == contract.id);
            currentContract.comments = $"Менеджер сдвинул срок окончания контракта с даты {currentContract.dayto.ToString("dd-MM-yyyy")} на дату {contract.dayto.ToString("dd-MM-yyyy")}";
            currentContract.dayto = contract.dayto;
            _db.SaveChanges();

            return Redirect("~/Contract/List");
        }

        #endregion



    }
}