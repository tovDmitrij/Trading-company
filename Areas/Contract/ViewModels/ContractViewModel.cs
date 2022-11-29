using Trading_company.Areas.Contract.Models;
using Trading_company.Models;

namespace Trading_company.Areas.Contract.ViewModels
{
    /// <summary>
    /// Модельное представление нового контракта и доступных контрагентов
    /// </summary>
    public sealed class ContractViewModel
    {
        /// <summary>
        /// Оформляемый контракт
        /// </summary>
        public ContractModel Contract { get; set; } = new();

        /// <summary>
        /// Доступные для подписания контракта контрагенты
        /// </summary>
        public List<ContragentModel> Contragents { get; set; } = new();
    }
}