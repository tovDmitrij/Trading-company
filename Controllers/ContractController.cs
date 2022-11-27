using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            contract.dayfrom = DateTime.Now;

            try
            {
                _db.contracts_with_optional_info.Add(contract);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                /*
                Единственная ошибка, которая здесь возникает - column id is null.
                Проблема в том, что данный атрибут имеет PK и автоинкрементится в БД и, соответственно, не может быть nullable.
                Это EntityFramework выделывается. 
                 */
            }

            return Redirect("~/Contract/List");
        }

        /// <summary>
        /// Удалить действующий контракт
        /// </summary>
        /// <param name="id">Идентификатор контракта</param>
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var contract = _db.contracts_with_optional_info.FirstOrDefault(contr => contr.id == Convert.ToInt32(id));
            contract.dayto = DateTime.Now;
            contract.comments = "Завершён досрочно менеджером";
            _db.SaveChanges();

            return Redirect("~/Contract/List");
        }

        #endregion



    }
}