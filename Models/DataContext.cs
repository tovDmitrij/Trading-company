using Microsoft.EntityFrameworkCore;
namespace Trading_company.Models
{
    /// <summary>
    /// Контекс базы данных
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Таблица сообщений о выполненных транзакциях
        /// </summary>
        public DbSet<MessageModel> messages { get; set; }

        /// <summary>
        /// Таблица менеджеров
        /// </summary>
        public DbSet<ManagerModel> managers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}