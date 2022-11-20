using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Работа с менеджером
    /// </summary>
    public class ManagerController : Controller
    {
        /// <summary>
        /// БД "Торговое предприятие"
        /// </summary>
        private readonly DataContext _db;

        /// <param name="context">БД "Торговое предприятие"</param>
        public ManagerController(DataContext context) 
        { 
            _db = context;
        }

        /// <summary>
        /// Регистрация менеджера
        /// </summary>
        /// <param name="manager">Информация о менеджере</param>
        /// <returns>Редирект на профиль менеджера (ну почти)</returns>
        public IActionResult SignUp(ManagerModel manager)
        {
            try
            {
                _db.managers.Add(manager);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                //return Ok(ex.Message); 
                //_db.messages.Add(new(-1, "fail", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc), "Manager/SignUp", ex.Message));
                //_db.SaveChanges();
            }
            return Ok(_db.messages.ToList());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}