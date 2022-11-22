using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SignUp() => View();

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
            ManagerOptionalModel manager = _db.managerswithoptionalinfo.FirstOrDefault(man =>
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
            if (manager.parent_id is not null)
            {
                if (_db.managers.FirstOrDefault(leader => leader.man_id == manager.parent_id) is null)
                {
                    SetInfo(manager, $"Руководителя с ID {manager.parent_id} не существует!");
                    return View();
                }
                if (_db.managers.Count(leader => leader.parent_id == manager.parent_id) > 3)
                {
                    SetInfo(manager, $"Руководитель с ID {manager.parent_id} не может иметь больше трёх менеджеров. Пожалуйста, выберите другого или не выбирайте вовсе!");
                    return View();
                }
            }

            string? error = "";
            if (Validations.CheckSignUpValidation(manager, out error))
            {
                try
                {
                    _db.managers.Add(manager);
                    _db.SaveChanges();
                }
                catch(Exception ex)
                {
                    SetInfo(manager, ex.Message);
                    return View();
                }
            }
            else
            {
                SetInfo(manager, error);
                return View();
            }

            var list = _db.messages.ToList();
            if (list.Count > 0)
            {
                SetInfo(manager, list.ToString());
                return View();
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
            string? error = "";
            if (!Validations.CheckSignInValidation(manager, out error))
            {
                SetInfo(manager, error);
                return View();
            }

            if (_db.managers.FirstOrDefault(man =>
                man.email == manager.email && man.password == manager.password) is null)
            {
                SetInfo(manager, $"Менеджера с такой почтой и паролем не существует!");
                return View();
            }

            var list = _db.messages.ToList();
            if (list.Count > 0)
            {
                SetInfo(manager, list.ToString());
                return View();
            }

            HttpContext.Session.Set("manager", manager);

            return Redirect("~/Manager/PersonalArea");
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
            ViewData["Man_fullname"] = manager.fullname;
            ViewData["Man_email"] = manager.email;
            ViewData["Man_password"] = manager.password;
            ViewData["Man_percent"] = manager.percent;
            ViewData["Man_leadid"] = manager.parent_id;
        }
        #endregion
    }
}