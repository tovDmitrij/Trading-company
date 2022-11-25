using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

using Trading_company.Misc;
using Trading_company.Models;
using Trading_company.ViewModels;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Взаимодействие с контрактом
    /// </summary>
    [Controller]
    public sealed class ContractController : Controller
    {
        /// <summary>
        /// БД "Торговое предприятие"
        /// </summary>
        private readonly DataContext _db;

        /// <param name="context">БД "Торговое предприятие"</param>
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

            var freeContragents = _db.contragents.FromSqlInterpolated(
                $"select * from Contragents except select cg.* from Contracts cr left join Contragents cg on cr.Contr_ID = cg.Contr_ID where cr.man_id = {currentManager.man_id} and cr.dayto >= now();").ToList();
            ContractViewModel cvm = new();
            cvm.Contragents = freeContragents;
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
            ManagerModel currentManager = HttpContext.Session.Get<ManagerModel>("manager");

            var contractList = _db.contracts_with_optional_info.FromSql($"select* from contracts_with_optional_info where man_id == {currentManager.man_id}");
            return View(contractList);
        }
        
        #endregion



        #region Действия на страницах

        /// <summary>
        /// Подписать новый контракт
        /// </summary>
        /// <param name="contract">Котрнакт</param>
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
            contract.dayfrom = System.DateTime.Now;
            if(contract.contr_id == 0)
            {
                SetInfo(contract, "Выберите контрагента!");
                return New();
            }
            if ((contract.dayto - contract.dayfrom).Days < 7)
            {
                SetInfo(contract, "Контракт должен оформляться минимум на неделю!");
                return New();
            }

            try
            {
                _db.contracts_with_optional_info.Add(contract);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                //SetInfo(contract, $"{ex.InnerException}");
                //return New();
            }

            return Redirect("~/Manager/PersonalArea");
        }

        #endregion



        #region Прочее

        /// <summary>
        /// Отправить на форму введённую информацию о контракте
        /// </summary>
        /// <param name="contract">Контракт</param>
        /// <param name="error">Ошибка, если она есть</param>
        private void SetInfo(ContractModel contract, string? error)
        {
            ViewData["Error"] = error;
            ViewData["Contract_ContrID"] = contract.contr_id;
            ViewData["Contract_Date"] = contract.dayto.ToString("yyyy-MM-dd");
        }

        #endregion



    }
}