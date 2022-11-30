using Microsoft.EntityFrameworkCore;
using Trading_company.Areas.Contract.Models;
using Trading_company.Areas.Transaction.Models;
using Trading_company.Areas.Warehouse.Models;
namespace Trading_company.Models
{
    /// <summary>
    /// Взаимодействие с БД "Торговое предприятие"
    /// </summary>
    public sealed class DataContext : DbContext
    {
        /// <summary>
        /// Таблица с контрагентами
        /// </summary>
        public DbSet<ContragentModel> contragents { get; set; }

        /// <summary>
        /// Представление с менеджерами с дополнительной информацией
        /// </summary>
        public DbSet<ManagerModel> managers_with_optional_info { get; set; }

        /// <summary>
        /// Представление с контрактами с дополнительной информацией
        /// </summary>
        public DbSet<ContractModel> contracts_with_optional_info { get; set; }

        /// <summary>
        /// Представление с транзакциями на покупку товаров
        /// </summary>
        public DbSet<IncomingModel> incoming_with_optional_info { get; set; }

        /// <summary>
        /// Представление с транзакциями на продажу товаров
        /// </summary>
        public DbSet<OutgoingModel> outgoing_with_optional_info { get; set; }

        /// <summary>
        /// Представление с существующими товарами
        /// </summary>
        public DbSet<ProductModel> products_with_optional_info { get; set; }

        /// <summary>
        /// Представление склада
        /// </summary>
        public DbSet<WarehouseModel> warehouse { get; set; }

        /// <summary>
        /// Прочие вычисления
        /// </summary>
        public DbSet<SomeModel> some_model { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}