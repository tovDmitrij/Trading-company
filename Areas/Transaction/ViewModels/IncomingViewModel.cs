using Trading_company.Areas.Transaction.Models;
using Trading_company.Models;
namespace Trading_company.Areas.Transaction.ViewModels
{
    /// <summary>
    /// Модельное представление оформления новой транзакции (покупка) и доступных товаров с контрактами для его подтверждения
    /// </summary>
    public sealed class IncomingViewModel
    {
        /// <summary>
        /// Информация о новой транзакции
        /// </summary>
        public IncomingModel Transaction { get; set; } = new();

        /// <summary>
        /// Доступные контракты для покупки товара
        /// </summary>
        public List<ContractModel> Contracts { get; set; } = new();

        /// <summary>
        /// Доступные для покупки товары
        /// </summary>
        public List<ProductModel> Products { get; set; } = new();
    }
}