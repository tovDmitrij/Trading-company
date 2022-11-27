using Trading_company.Models;
namespace Trading_company.ViewModels
{
    /// <summary>
    /// Модельное представление оформления новой транзакции (продажа) и доступных товаров с контрактами для его подтверждения
    /// </summary>
    public sealed class OutgoingViewModel
    {
        /// <summary>
        /// Информация о новой транзакции
        /// </summary>
        public OutgoingModel Transaction { get; set; } = new();

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