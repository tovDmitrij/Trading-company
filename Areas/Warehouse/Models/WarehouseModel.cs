using System.ComponentModel.DataAnnotations;
namespace Trading_company.Areas.Warehouse.Models
{
    /// <summary>
    /// Товар с информацией о его количестве на складе
    /// </summary>
    public sealed class WarehouseModel
    {
        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required]
        public string prod_name { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Key]
        public int prod_id { get; set; }

        /// <summary>
        /// Наименование группы товара
        /// </summary>
        [Required]
        public string group_name { get; set; }

        /// <summary>
        /// Идентификатор группы товара
        /// </summary>
        [Required]
        public int group_id { get; set; }

        /// <summary>
        /// Количество товара на складе
        /// </summary>
        [Required]
        public long prod_quantity { get; set; }

        public WarehouseModel(string prod_name, int prod_id, long prod_quantity)
        {
            this.prod_name = prod_name;
            this.prod_id = prod_id;
            this.prod_quantity = prod_quantity;
        }

        public WarehouseModel() { }
    }
}