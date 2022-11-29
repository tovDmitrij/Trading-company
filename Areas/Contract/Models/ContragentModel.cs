using System.ComponentModel.DataAnnotations;
namespace Trading_company.Areas.Contract.Models
{
    /// <summary>
    /// Контрагент
    /// </summary>
    public sealed class ContragentModel
    {
        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        [Key]
        public int contr_id { get; set; }

        /// <summary>
        /// ФИО контрагента
        /// </summary>
        [Required]
        public string name { get; set; }

        /// <summary>
        /// Адрес контрагента
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Телефон контрагента
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Доп. информация
        /// </summary>
        public string comments { get; set; }

        public ContragentModel(int contr_id, string name, string address, string phone, string comments)
        {
            this.contr_id = contr_id;
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.comments = comments;
        }

        public ContragentModel() { }
    }
}