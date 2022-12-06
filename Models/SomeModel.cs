using Microsoft.EntityFrameworkCore;
namespace Trading_company.Models
{
    /// <summary>
    /// Модель для различных расчётов
    /// </summary>
    [Keyless]
    public class SomeModel
    {
        /// <summary>
        /// Некоторое значение
        /// </summary>
        public double value { get; set; }

        public SomeModel(double value) => this.value = value;

        public SomeModel() { }
    }
}