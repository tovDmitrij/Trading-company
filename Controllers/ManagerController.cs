using Microsoft.AspNetCore.Mvc;
using Trading_company.Misc;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Работа с менеджером
    /// </summary>
    [Controller]
    public class ManagerController : Controller
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
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var managerInfo = HttpContext.Session.Get<ManagerModel>("manager");
            ManagerModel manager = _db.managers.FirstOrDefault(man => man.email == managerInfo.email && man.password == managerInfo.password);

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
            _db.managers.Add(manager);
            _db.SaveChanges();
            var list = _db.messages.ToList();
            if (list.Count > 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            HttpContext.Session.Set<ManagerModel>("manager", manager);

            return Redirect("PersonalArea");
        }

        /// <summary>
        /// Авторизация менеджера
        /// </summary>
        /// <param name="manager">Информация о менеджере</param>
        /// <returns>Редирект на профиль менеджера</returns>
        [HttpPost]
        public IActionResult SignIn(ManagerModel manager)
        {
            if (_db.managers.FirstOrDefault(man =>
                man.email == manager.email && man.password == manager.password) is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            HttpContext.Session.Set<ManagerModel>("manager", manager);

            return Redirect("PersonalArea");
        }
        #endregion
    }
}