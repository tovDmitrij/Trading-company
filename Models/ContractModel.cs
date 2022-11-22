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
        public string ContrFullName { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        [Required]
        public int contr_id { get; set; }

        /// <summary>
        /// ФИО менеджера
        /// </summary>
        [Required]
        public string ManFullName { get; set; }

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

        /// <param name="id">Идентификатор контракта</param>
        /// <param name="ContrFullName">ФИО контрагента</param>
        /// <param name="contr_id">Идентификатор контрагента</param>
        /// <param name="ManFullName">ФИО менеджера</param>
        /// <param name="man_id">Идентификатор менеджера</param>
        /// <param name="dayfrom">Дата подписания контракта</param>
        /// <param name="dayto'">Дата окончания контракта</param>
        public ContractModel(int id, string ContrFullName, int contr_id, string ManFullName, int man_id, DateTime dayfrom, DateTime dayto)
        {
            this.id = id;
            this.ContrFullName = ContrFullName;
            this.contr_id = contr_id;
            this.ManFullName = ManFullName;
            this.man_id = man_id;
            this.dayfrom = dayfrom;
            this.dayto = dayto;
        }

        public ContractModel() { }
    }
}