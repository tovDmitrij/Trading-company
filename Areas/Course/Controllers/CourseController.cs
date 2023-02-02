using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Areas.Course.Models;
using Trading_company.Controllers;
using Trading_company.Models;
namespace Trading_company.Areas.Course.Controllers
{
    /// <summary>
    /// Взаимодействие с курсами валют
    /// </summary>
    [Area("Course")]
    [Route("Course/{action}")]
    [Controller]
    public sealed class CourseController : TradingCompanyController
    {
        public CourseController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Отобразить таблицу с курсами валют
        /// </summary>
        public IActionResult Show()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var courseList = _db.course_with_optional_info
                .Where(x => x.cur_idto == 1 && x.cur_idfrom != 1)
                .OrderBy(x => x.cur_idfrom)
                .ToList()
                .DistinctBy(x => x.cur_namefrom);

            return View(courseList);
        }

        #endregion



    }
}