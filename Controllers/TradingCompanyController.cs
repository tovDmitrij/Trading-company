using Microsoft.AspNetCore.Mvc;
using Trading_company.Models;
namespace Trading_company.Controllers
{
    public class TradingCompanyController : Controller
    {
        /// <summary>
        /// База данных "Торговое предприятие"
        /// </summary>
        protected DataContext _db;
    }
}