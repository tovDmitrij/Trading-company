using Trading_company.Models;
namespace Trading_company.ViewModels
{
    /// <summary>
    /// Модельное представление всех транзакций менеджера
    /// </summary>
    public class TransactionViewModel
    {
        /// <summary>
        /// Список покупок менеджера
        /// </summary>
        public List<IncomingModel> purchaseTransactions { get; set; } = new();

        /// <summary>
        /// Список продаж менеджера
        /// </summary>
        public List<OutgoingModel> sellTransactions { get; set; } = new();
    }
}