using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trading_company.Controllers;
using Trading_company.Models;
namespace Trading_company.Areas.Course.Controllers
{
    /// <summary>
    /// Взаимодействие с курсами валют
    /// </summary>
    [Area("Course")]
    [Controller]
    public sealed class CourseController : TradingCompanyController
    {
        public CourseController(DataContext context) => _db = context;



        #region Страницы

        /// <summary>
        /// Отобразить таблицу с курсами валют
        /// </summary>
        [Route("{controller}/{action}")]
        public IActionResult Show()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return Redirect("~/Manager/SignIn");
            }

            var courseList = _db.course_with_optional_info.FromSqlInterpolated($"select distinct cur_namefrom, cur_idfrom, cur_nameto, cur_idto, now() dayfrom, now() dayto, 0 value from course_with_optional_info where cur_idto = 1 and cur_idfrom != 1 order by cur_idfrom").ToList();

            return View(courseList);
        }

        #endregion



    }
}