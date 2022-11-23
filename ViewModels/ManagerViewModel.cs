using Trading_company.Models;
namespace Trading_company.ViewModels
{
    /// <summary>
    /// Модельное представление нового менеджера и свободных руководителей
    /// </summary>
    public class ManagerViewModel
    {
        /// <summary>
        /// Регистрируемый менеджер
        /// </summary>
        public ManagerModel CurrentManager { get; set; } = new();

        /// <summary>
        /// Список доступных руководителей
        /// </summary>
        public List<ManagerModel> FreeLeaders { get; set; } = new();
    }
}