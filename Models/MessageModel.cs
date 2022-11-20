using System.ComponentModel.DataAnnotations;
namespace Trading_company.Models
{
    /// <summary>
    /// Сообщения, содержащие информацию об удачно или неудачно выполненных операциях
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        [Required]
        public string type { get; set; }

        /// <summary>
        /// Время появления сообщения
        /// </summary>
        [Required]
        public DateTime time { get; set; }

        /// <summary>
        /// Место в коде, где произошла ошибка
        /// </summary>
        [Required]
        public string causeoferror { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Required]
        public string description { get; set; }

        /// <param name="id">Идентификатор сообщения</param>
        /// <param name="type">Тип сообщения</param>
        /// <param name="time">Время появления сообщения</param>
        /// <param name="causeoferror">Место в коде, где произошла ошибка</param>
        /// <param name="description">Текст сообщения</param>
        public MessageModel(int id, string type, DateTime time, string causeoferror, string description)
        {
            this.id = id;
            this.type = type;
            this.time = time;
            this.causeoferror = causeoferror;
            this.description = description;
        }

        public MessageModel() { }
    }
}