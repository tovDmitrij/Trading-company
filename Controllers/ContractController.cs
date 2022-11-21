using Microsoft.AspNetCore.Mvc;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    /// <summary>
    /// Взаимодействие с контрактом
    /// </summary>
    [Controller]
    public sealed class ContractController : Controller
    {
        /// <summary>
        /// БД "Торговое предприятие"
        /// </summary>
        private readonly DataContext _db;

        /// <param name="context">БД "Торговое предприятие"</param>
        public ContractController(DataContext context) => _db = context;

        #region Страницы
        /// <summary>
        /// Подписать новый контракт
        /// </summary>
        public IActionResult New()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            return View();
        }

        /// <summary>
        /// Посмотреть список контрактов
        /// </summary>
        public IActionResult List()
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            List<ContractModel> contractList = new();
            return View(contractList);
        }
        #endregion

        #region Действия на страницах
        /// <summary>
        /// Подписать новый контракт
        /// </summary>
        /// <param name="contract">Котрнакт</param>
        [HttpPost]
        public IActionResult New(ContractModel contract)
        {
            if (!HttpContext.Session.Keys.Contains("manager"))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            return Redirect("");
        }
        #endregion
    }
}