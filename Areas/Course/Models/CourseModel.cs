using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trading_company.Areas.Course.Models
{
    /// <summary>
    /// Курс валют
    /// </summary>
    [Keyless]
    public sealed class CourseModel
    {
        /// <summary>
        /// Наименование переводимого курса валюты
        /// </summary>
        [Required]
        public string cur_namefrom { get; set; }

        /// <summary>
        /// Идентификатор переводимого курса валюты
        /// </summary>
        [Required]
        public int cur_idfrom { get; set; }

        /// <summary>
        /// Наименование переведённого курса валюты
        /// </summary>
        [Required]
        public string cur_nameto { get; set; }

        /// <summary>
        /// Идентификатор переведённого курса валюты
        /// </summary>
        [Required]
        public int cur_idto { get; set; } = 0;

        /// <summary>
        /// Дата начала курса валюты
        /// </summary>
        [Required]
        public DateTime dayfrom { get; set; }

        /// <summary>
        /// Дата окончания курса валюты
        /// </summary>
        [Required]
        public DateTime dayto { get; set; }

        /// <summary>
        /// Значение курса валюты
        /// </summary>
        [Required]
        public double value { get; set; }

        public CourseModel(string cur_namefrom, int cur_idfrom, string cur_nameto, int cur_idto, DateTime dayfrom, DateTime dayto, double value)
        {
            this.cur_namefrom = cur_namefrom;
            this.cur_idfrom = cur_idfrom;
            this.cur_nameto = cur_nameto;
            this.cur_idto = cur_idto;
            this.dayfrom = dayfrom;
            this.dayto =dayto;
            this.value = value;
        }

        public CourseModel() { }
    }
}