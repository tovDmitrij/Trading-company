using Trading_company.Areas.Transaction.Models;
namespace Trading_company.Areas.Transaction.ViewModels
{
    /// <summary>
    /// Модельное представление всех транзакций менеджера
    /// </summary>
    public sealed class TransactionViewModel
    {
        /// <summary>
        /// Текущая транзакция на покупку
        /// </summary>
        public IncomingModel PurchaseTransaction { get; set; }

        /// <summary>
        /// Текущая транзакция на продажу
        /// </summary>
        public OutgoingModel SellTransaction { get; set; }

        /// <summary>
        /// Список покупок менеджера
        /// </summary>
        public List<IncomingModel> PurchaseTransactions { get; set; } = new();

        /// <summary>
        /// Список продаж менеджера
        /// </summary>
        public List<OutgoingModel> SellTransactions { get; set; } = new();
    }
}