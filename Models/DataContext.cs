using Microsoft.EntityFrameworkCore;
namespace Trading_company.Models
{
    /// <summary>
    /// Контекс базы данных
    /// </summary>
    public sealed class DataContext : DbContext
    {
        /// <summary>
        /// Таблица с сообщениями о выполненных транзакциях
        /// </summary>
        public DbSet<MessageModel> messages { get; set; }

        /// <summary>
        /// Таблица с контрагентами
        /// </summary>
        public DbSet<ContragentModel> contragents { get; set; }

        /// <summary>
        /// Таблица с менеджерами с дополнительной информацией
        /// </summary>
        public DbSet<ManagerModel> managerswithoptionalinfo { get; set; }

        /// <summary>
        /// Таблица с контрактами с дополнительной информацией
        /// </summary>
        public DbSet<ContractModel> contractswithoptionalinfo { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}