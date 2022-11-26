using System.ComponentModel.DataAnnotations;
namespace Trading_company.Models
{
    /// <summary>
    /// Контракт
    /// </summary>
    public class ContractModel
    {
        /// <summary>
        /// Идентификатор контракта
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// ФИО контрагента
        /// </summary>
        [Required]
        public string contr_fullname { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        [Required]
        public int contr_id { get; set; }

        /// <summary>
        /// ФИО менеджера
        /// </summary>
        [Required]
        public string man_fullname { get; set; }

        /// <summary>
        /// Идентификатор менеджера
        /// </summary>
        [Required]
        public int man_id { get; set; }

        /// <summary>
        /// Дата подписания контракта
        /// </summary>
        [Required]
        public DateTime dayfrom { get; set; }

        /// <summary>
        /// Дата окончания контракта
        /// </summary>
        [Required]
        public DateTime dayto { get; set; }

        /// <summary>
        /// Пояснение к контракту, если оно есть
        /// </summary>
        public string? comments { get; set; }

        public ContractModel(int id, string contr_fullname, int contr_id, string man_fullname, int man_id, DateTime dayfrom, DateTime dayto, string comments)
        {
            this.id = id;
            this.contr_fullname = contr_fullname;
            this.contr_id = contr_id;
            this.man_fullname = man_fullname;
            this.man_id = man_id;
            this.dayfrom = dayfrom;
            this.dayto = dayto;
            this.comments = comments;
        }

        public ContractModel() { }
    }
}