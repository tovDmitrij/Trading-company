using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Trading_company.Areas.Transaction.Models
{
    /// <summary>
    /// Информация о товаре
    /// </summary>
    [Keyless]
    public sealed class ProductModel
    {
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
        /// Наименование товара
        /// </summary>
        [Required]
        public string prod_name { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required]
        public int prod_id { get; set; }

        public ProductModel(string group_name, int group_id, string prod_name, int prod_id)
        {
            this.group_name = group_name;
            this.group_id = group_id;
            this.prod_name = prod_name;
            this.prod_id = prod_id;
        }

        public ProductModel() { }
    }
}