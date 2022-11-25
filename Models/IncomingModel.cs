using System.ComponentModel.DataAnnotations;
namespace Trading_company.Models
{
    /// <summary>
    /// Транзакция на покупку товара
    /// </summary>
    public class IncomingModel
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        [Key]
        public int inc_id { get; set; }

        /// <summary>
        /// Идентификатор контракта
        /// </summary>
        [Required]
        public int contract_id { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required]
        public int prod_id { get; set; }

        /// <summary>
        /// Количество покупаемого товара
        /// </summary>
        [Required]
        public int quantity { get; set; }

        /// <summary>
        /// Дата совершения транзакции
        /// </summary>
        [Required]
        public DateTime inc_date { get; set; }

        /// <summary>
        /// Потрачено на транзакцию
        /// </summary>
        public double paid { get; set; }

        /// <summary>
        /// Потрачено на налоги
        /// </summary>
        public double cost { get; set; }

        public IncomingModel(int inc_id, int contract_id, int prod_id, int quantity, DateTime inc_date, double paid, double cost)
        {
            this.inc_id = inc_id;
            this.contract_id = contract_id;
            this.prod_id = prod_id;
            this.quantity = quantity;
            this.inc_date = inc_date;
            this.paid = paid;
            this.cost = cost;
        }

        public IncomingModel() { }
    }
}