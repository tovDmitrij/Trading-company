using System.ComponentModel.DataAnnotations;
namespace Trading_company.Areas.Transaction.Models
{
    /// <summary>
    /// Транзакция на покупку товара
    /// </summary>
    public sealed class IncomingModel
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
        /// Потрачено на транзакцию
        /// </summary>
        public double transaction_paid { get; set; }

        /// <summary>
        /// Потрачено на налоги
        /// </summary>
        public double cost { get; set; }

        public IncomingModel(int transaction_id, int contract_id, string prod_name, int prod_id, int prod_quantity, DateTime transaction_date, double transaction_paid, double cost)
        {
            this.transaction_id = transaction_id;
            this.contract_id = contract_id;
            this.prod_name = prod_name;
            this.prod_id = prod_id;
            this.prod_quantity = prod_quantity;
            this.transaction_date = transaction_date;
            this.transaction_paid = transaction_paid;
            this.cost = cost;
        }

        public IncomingModel() { }
    }
}