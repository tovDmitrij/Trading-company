using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Misc;
using Trading_company.Models;
using Trading_company.Controllers;
using Trading_company.Areas.Manager.ViewModels;
namespace Trading_company.Areas.Manager.Controllers
{
    /// <summary>
    /// Работа с менеджером
    /// </summary>
    [Area("Manager")]
    [Controller]
    public sealed class ManagerController : TradingCompanyController
    {
        public ManagerController(DataContext dbContext) => _db = dbContext;



        #region Страницы

        /// <summary>
        /// Страница с регистрацией менеджера
        /// </summary>
        [Route("")]
        [Route("{controller}/{action}")]
        public IActionResult SignUp()
        {
            var freeLeaders = _db.managers_with_optional_info.FromSql(
                $"select * from managers_with_optional_info where man_id not in (select distinct parent_id from Managers where parent_id is not null group by parent_id having count(parent_id) > 3)").ToList();
            ManagerViewModel mvm = new()
            {
                Leaders = freeLeaders
            };

            return View(mvm);
        }

        /// <summary>
        /// Страница с авторизацией менеджера
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult SignIn() => View();

        /// <summary>
        /// Личный кабинет менеджера
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult PersonalArea()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("SignIn");
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            return View(manager);
        }

        #endregion



        #region Действия на страницах

        /// <summary>
        /// Регистрация менеджера
        /// </summary>
        /// <param name="manager">Информация о менеджере</param>
        [Route("{controller}/{action}")]
        [HttpPost]
        public IActionResult SignUp(ManagerModel manager)
        {
            if (manager.lead_id is null)
            {
                manager.comments = "Отсутствует руководитель";
            }
            manager.hire_day = DateTime.Now;

            try
            {
                _db.managers_with_optional_info.Add(manager);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

            HttpContext.Session.Set("manager", manager);
            return Redirect("~/Manager/PersonalArea");
        }

        /// <summary>
        /// Авторизация менеджера
        /// </summary>
        /// <param name="manager">Информация о менеджере</param>
        [Route("{controller}/{action}")]
        [HttpPost]
        public IActionResult SignIn(ManagerModel manager)
        {
            HttpContext.Session.Set("manager", manager);
            return Redirect("~/Manager/PersonalArea");
        }

        /// <summary>
        /// Выход из аккаунта менеджера
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult Exit()
        {
            HttpContext.Session.Clear();
            return Redirect("SignIn");
        }

        /// <summary>
        /// Удалить аккаунт менеджера
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult Delete()
        {
            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == managerInfo.email && man.password == managerInfo.password);

            try
            {
                _db.managers_with_optional_info.Remove(manager);
                _db.SaveChanges();
            }
            catch (Exception ex) { }

            HttpContext.Session.Clear();
            return Redirect("SignIn");
        }

        #endregion



    }
}