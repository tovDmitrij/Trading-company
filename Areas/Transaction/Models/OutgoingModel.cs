using System.ComponentModel.DataAnnotations;
namespace Trading_company.Areas.Transaction.Models
{
    /// <summary>
    /// Транзакция на продажу товара
    /// </summary>
    public sealed class OutgoingModel
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        [Key]
        public int transaction_id { get; set; }

        /// <summary>
        /// Идентификатор контракта
        /// </summary>
        [Required]
        public int contract_id { get; set; }

        /// <summary>
        /// Дата совершения транзакции
        /// </summary>
        [Required]
        public DateTime transaction_date { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required]
        public string prod_name { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required]
        public int prod_id { get; set; }

        /// <summary>
        /// Количество покупаемого товара
        /// </summary>
        [Required]
        public int prod_quantity { get; set; }

        /// <summary>
        /// Потрачено на транзакцию (без учёта налогов)
        /// </summary>
        public double transaction_earn { get; set; }

        /// <summary>
        /// Сколько менеджер заработал за совершённую транзакцию (без учёта налогов)
        /// </summary>
        public double manager_earn { get; set; }

        /// <summary>
        /// Потрачено на налоги
        /// </summary>
        public double cost { get; set; }

        public OutgoingModel(int transaction_id, int contract_id, DateTime transaction_date, string prod_name, int prod_id, int prod_quantity, double transaction_earn, double manager_earn, double cost)
        {
            this.transaction_id = transaction_id;
            this.contract_id = contract_id;
            this.transaction_date = transaction_date;
            this.prod_name = prod_name;
            this.prod_id = prod_id;
            this.prod_quantity = prod_quantity;
            this.transaction_earn = transaction_earn;
            this.manager_earn = manager_earn;
            this.cost = cost;
        }

        public OutgoingModel() { }
    }
}
