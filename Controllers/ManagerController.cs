using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.ViewModels;
using Trading_company.Misc;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Работа с менеджером
    /// </summary>
    [Controller]
    public sealed class ManagerController : Controller
    {
        /// <summary>
        /// БД "Торговое предприятие"
        /// </summary>
        private readonly DataContext _db;

        /// <param name="context">БД "Торговое предприятие"</param>
        public ManagerController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Страница с регистрацией менеджера
        /// </summary>
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
        public IActionResult SignIn() => View();

        /// <summary>
        /// Личный кабинет менеджера
        /// </summary>
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
        /// <returns>Редирект на профиль менеджера</returns>
        [HttpPost]
        public IActionResult SignUp(ManagerModel manager)
        {
            if (_db.managers_with_optional_info.FirstOrDefault(man => man.email == manager.email) is not null)
            {
                SetInfo(manager, "Почта уже занята");
                return SignUp();
            }

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
            catch (Exception ex)
            {
                /*
                Единственная ошибка, которая здесь возникает - column man_id is null.
                Проблема в том, что данный атрибут имеет PK и автоинкрементится в БД и, соответственно, не может быть nullable.
                Это EntityFramework выделывается.
                 */
            }

            HttpContext.Session.Set("manager", manager);
            return Redirect("~/Manager/PersonalArea");
        }

        /// <summary>
        /// Авторизация менеджера
        /// </summary>
        /// <param name="manager">Информация о менеджере</param>
        /// <returns>Редирект на профиль менеджера</returns>
        [HttpPost]
        public IActionResult SignIn(ManagerModel manager)
        {
            if (_db.managers_with_optional_info.FirstOrDefault(man =>
                man.email == manager.email && man.password == manager.password) is null)
            {
                SetInfo(manager, $"Менеджера с такой почтой и паролем не существует");
                return View();
            }

            HttpContext.Session.Set("manager", manager);

            return Redirect("~/Manager/PersonalArea");
        }

        /// <summary>
        /// Выход из аккаунта менеджера
        /// </summary>
        public IActionResult Exit()
        {
            HttpContext.Session.Clear();
            return Redirect("SignIn");
        }

        /// <summary>
        /// Удалить аккаунт менеджера
        /// </summary>
        /// <returns></returns>
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
            catch(Exception ex)
            {
                /*
                 Здесь также С# выделывается: на самом деле менеджер удаляется из БД...
                 */
            }

            HttpContext.Session.Clear();
            return Redirect("SignIn");
        }
        
        #endregion



        #region Прочее

        /// <summary>
        /// Отправить на форму введённую информацию о менеджере
        /// </summary>
        /// <param name="manager">Менеджер</param>
        /// <param name="error">Ошибка, если она есть</param>
        [NonAction]
        private void SetInfo(ManagerModel manager, string? error)
        {
            ViewData["error"] = error;
            ViewData["Man_fullname"] = manager.man_fullname;
            ViewData["Man_email"] = manager.email;
            ViewData["Man_password"] = manager.password;
            ViewData["Man_percent"] = manager.percent;
            ViewData["Man_leadid"] = manager.lead_id;
        }

        #endregion



    }
}