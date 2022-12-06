using Microsoft.AspNetCore.Mvc;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Почти подробная информация о проекте
    /// </summary>
    [Controller]
    public class AboutController : Controller
    {
        /// <summary>
        /// Отобразить инфомарцию о проекте.
        /// </summary>
        [Route("About")]
        public IActionResult About() => View();
    }
}