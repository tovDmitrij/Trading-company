using System.ComponentModel.DataAnnotations;
namespace Trading_company.Models
{
    /// <summary>
    /// Информация о менеджере
    /// </summary>
    public sealed class ManagerModel 
    {
        /// <summary>
        /// Идентификатор менеджера
        /// </summary>
        [Key]
        public int man_id { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        public string email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string password { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [Required]
        public string man_fullname { get; set; }

        /// <summary>
        /// Получаемый процент с продаж
        /// </summary>
        [Required]
        public double percent { get; set; }

        /// <summary>
        /// Дата найма
        /// </summary>
        [Required]
        public DateTime hire_day { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string? comments { get; set; }

        /// <summary>
        /// ФИО руководителя
        /// </summary>
        public string? lead_fullname { get; set; }

        /// <summary>
        /// Идентификатор руководителя
        /// </summary>
        public int? lead_id { get; set; }

        public ManagerModel(int man_id, string email, string password, string fullname, double percent, DateTime hire_day, string comments, string leadfullname, int parent_id)
        {
            this.man_id = man_id;
            this.email = email;
            this.password = password;
            this.man_fullname = fullname;
            this.percent = percent;
            this.hire_day = hire_day;
            this.comments = comments;
            this.lead_fullname = leadfullname;
            this.lead_id = parent_id;
        }

        public ManagerModel() { }
    }
}