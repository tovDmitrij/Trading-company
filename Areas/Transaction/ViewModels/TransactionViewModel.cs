using Trading_company.Areas.Transaction.Models;

namespace Trading_company.Areas.Transaction.ViewModels
{
    /// <summary>
    /// Модельное представление всех транзакций менеджера
    /// </summary>
    public sealed class TransactionViewModel
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