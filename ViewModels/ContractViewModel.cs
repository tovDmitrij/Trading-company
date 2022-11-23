using Trading_company.Models;
namespace Trading_company.ViewModels
{
    /// <summary>
    /// Модельное представление нового контракта и доступных контрагентов
    /// </summary>
    public class ContractViewModel
    {
        /// <summary>
        /// Оформляемый контракт
        /// </summary>
        public ContractModel CurrentContract { get; set; } = new();

        /// <summary>
        /// Доступные для подписания контракта контрагенты
        /// </summary>
        public List<ContragentModel> FreeContragents { get; set; } = new();
    }
}