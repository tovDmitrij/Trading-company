using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Misc;
using Trading_company.Models;
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

            return View();
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

            var contractList = _db.contractswithoptionalinfo.FromSql($"select* from contractswithoptionalinfo where man_id == {currentManager.man_id}");
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

            string? error = "";
            if (!Validations.CheckContractValidation(contract, out error))
            {
                SetInfo(contract, error);
                return View();
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel currentManager = _db.managerswithoptionalinfo.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);
            contract.man_id = currentManager.man_id;

            if (_db.contragents.FirstOrDefault(contr => 
                    contr.name == contract.ContrFullName && contr.contr_id == contract.contr_id) is null)
            {
                SetInfo(contract, $"Контрагента {contract.ContrFullName}#{contract.contr_id} не существует!");
                return View();
            }
            contract.dayfrom = System.DateTime.Now;
            if ((contract.dayto - contract.dayfrom).Days < 7)
            {
                SetInfo(contract, "Контракт должен оформляться минимум на неделю!");
                return View();
            }
            if (_db.contractswithoptionalinfo.FirstOrDefault(contr =>
                    contr.contr_id == contract.contr_id && contr.man_id == contract.man_id && contr.dayfrom <= contract.dayfrom && contract.dayfrom <= contr.dayto) is not null)
            {
                SetInfo(contract, "С данным контрагентом уже есть действующий контракт!");
                return View();
            }

            try
            {
                _db.contractswithoptionalinfo.Add(contract);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = "";
                List<MessageModel> errorList = _db.messages.ToList();
                foreach (var item in errorList)
                {
                    message += item.ToString() + "\n";
                }
                //SetInfo(contract, $"{ex.InnerException}\n{message}");
                //return View();
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
            ViewData["Contract_ContrFullName"] = contract.ContrFullName;
            ViewData["Contract_ContrID"] = contract.contr_id;
            ViewData["Contract_Date"] = contract.dayto.ToString("yyyy-MM-dd");
        }

        #endregion



    }
}