using Microsoft.AspNetCore.Mvc;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Почти подробная информация о проекте (без осмотра кода проекта)
    /// </summary>
    [Controller]
    [Route("About")]
    public sealed class AboutController : Controller
    {
        /// <summary>
        /// Отобразить информацию о проекте.
        /// </summary>
        public IActionResult About() => View();
    }
}