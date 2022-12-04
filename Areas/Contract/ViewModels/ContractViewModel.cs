using Trading_company.Areas.Contract.Models;
using Trading_company.Models;
namespace Trading_company.Areas.Contract.ViewModels
{
    /// <summary>
    /// Модельное представление контракта, списка контрагентов и списка контрактов
    /// </summary>
    public sealed class ContractViewModel
    {
        /// <summary>
        /// Текущий контракт
        /// </summary>
        public ContractModel Contract { get; set; }

        /// <summary>
        /// Список контрактов менеджера
        /// </summary>
        public List<ContractModel> Contracts { get; set; } = new();

        /// <summary>
        /// Список контрагентов
        /// </summary>
        public List<ContragentModel> Contragents { get; set; } = new();
    }
}