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
            if (CheckSignUpValidation(manager))
            {
                _db.managers.Add(manager);
                _db.SaveChanges();
            }
            else
            {
                ViewData["Man_fullname"] = manager.fullname;
                ViewData["Man_email"] = manager.email;
                ViewData["Man_password"] = manager.password;
                ViewData["Man_percent"] = manager.percent;
                ViewData["Man_leadid"] = manager.parent_id;
                return View();
            }

            var list = _db.messages.ToList();
            if (list.Count > 0)
            {
                ViewData["Error"] = list.ToString();
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
            if (!CheckSignInValidation(manager))
            {
                ViewData["Man_email"] = manager.email;
                ViewData["Man_password"] = manager.password;
                return View();
            }

            if (_db.managers.FirstOrDefault(man =>
                man.email == manager.email && man.password == manager.password) is null)
            {
                ViewData["Error"] = $"Менеджера с такой почтой и паролем не существует!";
                ViewData["Man_email"] = manager.email;
                ViewData["Man_password"] = manager.password;
                return View();
            }

            var list = _db.messages.ToList();
            if (list.Count > 0)
            {
                ViewData["Error"] = list.ToString();
                return View();
            }

            HttpContext.Session.Set("manager", manager);

            return Redirect("~/Manager/PersonalArea");
        }
        #endregion

        #region Прочее
        /// <summary>
        /// Проверка на валидность данных при регистрации
        /// </summary>
        /// <param name="manager">Данные о менеджере</param>
        [NonAction]
        private bool CheckSignUpValidation(ManagerModel manager)
        {
            #region ФИО
            if (manager.fullname is null)
            {
                ViewData["Error_fullname"] = "Пожалуйста, введите ФИО!";
                return false;
            }
            if (manager.fullname.Count(ch => ch == ' ') != 2)
            {
                ViewData["Error_fullname"] = "Между фамилией, именем и отчеством необходим один отступ в виде пробела!";
                return false;
            }
            #endregion

            #region Почта
            if (manager.email is null)
            {
                ViewData["Error_email"] = "Пожалуйста, введите почту!";
                return false;
            }
            if (manager.email.Count(ch => ch == '@') != 1)
            {
                ViewData["Error_email"] = "Отсутствует символ '@'!";
                return false;
            }
            if (manager.email.IndexOf('@') == manager.email.Length - 1)
            {
                ViewData["Error_email"] = "Отсутствует домен после символа '@'!";
                return false;
            }
            if (_db.managers.FirstOrDefault(man => man.email == manager.email) is not null)
            {
                ViewData["Error_email"] = "Почта уже занята!";
                return false;
            }
            #endregion

            #region Пароль
            if (manager.password is null)
            {
                ViewData["Error_password"] = "Пожалуйста, введите пароль!";
                return false;
            }
            if (manager.password.Length < 8)
            {
                ViewData["Error_password"] = "Длина пароля должна быть не менее 8 символов!";
                return false;
            }
            #endregion

            #region Процент с продаж
            if (manager.percent < 0.0)
            {
                ViewData["Error_percent"] = "Пожалуйста, задайте валидный процент с продаж!";
                return false;
            }
            if (manager.percent == 0.0)
            {
                ViewData["Error_percent"] = "Пожалуйста, задайте процент с продаж!";
                return false;
            }
            if (manager.percent > 50.0)
            {
                ViewData["Error_percent"] = "Процент с продаж должен быть меньше 51!";
                return false;
            }
            #endregion

            #region ID руководителя
            if (manager.parent_id is not null)
            {
                if (_db.managers.FirstOrDefault(leader => leader.man_id == manager.parent_id) is null)
                {
                    ViewData["Error_leadid"] = "Отсутствует руководитель с таким ID!";
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// Процерка на валидность данных при авторизации
        /// </summary>
        /// <param name="manager">Данные о менеджере</param>
        [NonAction]
        private bool CheckSignInValidation(ManagerModel manager)
        {
            #region Почта
            if (manager.email is null)
            {
                ViewData["Error_email"] = "Пожалуйста, введите почту!";
                return false;
            }
            if (manager.email.Count(ch => ch == '@') != 1)
            {
                ViewData["Error_email"] = "Отсутствует символ '@'!";
                return false;
            }
            if (manager.email.IndexOf('@') == manager.email.Length)
            {
                ViewData["Error_email"] = "Отсутствует домен после символа '@'!";
                return false;
            }
            #endregion

            #region Пароль
            if (manager.password is null)
            {
                ViewData["Error_password"] = "Пожалуйста, введите пароль!";
                return false;
            }
            if (manager.password.Length < 8)
            {
                ViewData["Error_password"] = "Длина пароля должна быть не менее 8 символов!";
                return false;
            }
            #endregion

            return true;
        }
        #endregion
    }
}