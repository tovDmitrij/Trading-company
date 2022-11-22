using System.ComponentModel.DataAnnotations;
namespace Trading_company.Models
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
        [Required]
        public string address { get; set; }

        /// <summary>
        /// Телефон контрагента
        /// </summary>
        [Required]
        public string phone { get; set; }

        /// <summary>
        /// Доп. информация
        /// </summary>
        public string? comments { get; set; }


        /// <param name="contr_id">Идентификатор контрагента</param>
        /// <param name="name">ФИО контрагента</param>
        /// <param name="address">Адрес контрагента</param>
        /// <param name="phone">Телефон контрагента</param>
        /// <param name="comments">Доп. информация</param>
        public ContragentModel(int contr_id, string name, string address, string phone, string? comments)
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