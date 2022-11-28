using Microsoft.EntityFrameworkCore;
namespace Trading_company.Models
{
    /// <summary>
    /// Модель для различных рассчётов
    /// </summary>
    [Keyless]
    public class SomeModel
    {
        public double summ { get; set; }

        public SomeModel(double summ) 
        { 
            this.summ = summ;
        }

        public SomeModel() { }
    }
}