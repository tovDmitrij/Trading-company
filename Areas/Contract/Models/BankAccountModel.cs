using System.ComponentModel.DataAnnotations;
namespace Trading_company.Areas.Contract.Models
{
    /// <summary>
    /// Банковский аккаунт контрагента
    /// </summary>
    public sealed class BankAccountModel
    {
        /// <summary>
        /// Идентификатор банковского аккаунта
        /// </summary>
        [Key]
        public int acc_id { get; set; }

        /// <summary>
        /// Идентификатор банка
        /// </summary>
        [Required]
        public int bank_id { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        [Required]
        public int contr_id { get; set; }

        /// <summary>
        /// Дата начала действия банковского аккаунта
        /// </summary>
        [Required]
        public DateTime dayfrom { get; set; }

        /// <summary>
        /// Дата окончания действия банковского аккаунта
        /// </summary>
        [Required]
        public DateTime dayto { get; set; }

        public BankAccountModel(int acc_id, int bank_id, int contr_id, DateTime dayfrom, DateTime dayto)
        {
            this.acc_id = acc_id;
            this.bank_id = bank_id;
            this.contr_id = contr_id;
            this.dayfrom = dayfrom;
            this.dayto = dayto;
        }

        public BankAccountModel() { }
    }
}
