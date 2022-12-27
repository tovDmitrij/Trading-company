using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Trading_company.Areas.Price.Models
{
    /// <summary>
    /// Товар со списком цен на него
    /// </summary>
    [Keyless]
    public sealed class PriceModel
    {
        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required]
        public string product_name { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required]
        public int product_id { get; set; }

        /// <summary>
        /// Начало действия ценника
        /// </summary>
        [Required]
        public DateTime price_dayfrom { get; set; }

        /// <summary>
        /// Окончание действия ценника
        /// </summary>
        [Required]
        public DateTime price_dayto { get; set; }

        /// <summary>
        /// Значение ценника
        /// </summary>
        [Required]
        public double price_value { get; set; }

        public PriceModel(string product_name, int product_id, DateTime price_dayfrom, DateTime price_dayto, double price_value)
        {
            this.product_name = product_name;
            this.product_id = product_id;
            this.price_dayfrom = price_dayfrom;
            this.price_dayto = price_dayto;
            this.price_value = Math.Round(price_value, 2);
        }

        public PriceModel() { }
    }
}