using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Trading_company.Models;
namespace Trading_company.Hubs
{
    /// <summary>
    /// Хаб для обработки запросов в режиме реального времени
    /// </summary>
    public class TradingCompanyHub : Hub
    {
        /// <summary>
        /// БД "Торговое предприятие
        /// </summary>
        private readonly DataContext _db;

        /// <param name="dbContext">БД "Торговое предприятие</param>
        public TradingCompanyHub(DataContext dbContext) => _db = dbContext;

        /// <summary>
        /// Проверка перед регистрацией, что введённая почта ещё не занята
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        public void SignUpCheck(string email)
        {
            if (_db.managers_with_optional_info.FirstOrDefault(man => man.email == email) is not null)
            {
                Clients.Caller.SendAsync("Message", "Почта уже занята другим пользователем");
            }
        }

        /// <summary>
        /// Проверка перед авторизацией, что существует менеджер с таким набором логина и пароля
        /// </summary>
        /// <param name="email">Логин</param>
        /// <param name="password">Пароль</param>
        public void SignInCheck(string email, string password)
        {
            if (_db.managers_with_optional_info.FirstOrDefault(man => man.email == email && man.password == password) is null)
            {
                Clients.Caller.SendAsync("Message", "Менеджера с такой почтой и паролем не существует");
            }
        }

        /// <summary>
        /// Генерация чека 
        /// </summary>
        /// <param name="prod_id">Идентификатор товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <param name="transaction_date">Дата продажи товара</param>
        public void GetCheck(string prod_id, string quantity, string transaction_date)
        {
            //Стоимость транзакции
            double cost = 0.0;
            //Сколько заберут налоги
            double tax;
            //Стоимость + налоги
            double totalCost;

            DateTime date = DateTime.Parse(transaction_date);
            var transactionCost = _db.some_model.FromSqlInterpolated($"select {Convert.ToInt32(quantity)} * pc.value * (select value from cources where cur_idfrom = crc.cur_id and cur_idto = 1 and dayfrom <= {DateTime.Now} and {DateTime.Now} <= dayto) cost from Products pd left join Prices pc on pd.prod_id = pc.prod_id left join Currencies crc on pc.cur_id = crc.cur_id left join Cources cr on crc.cur_id = cr.cur_idfrom where pd.prod_id = {Convert.ToInt32(prod_id)} and pc.dayfrom <= {date} and  pc.dateto >= {date}").ToList().FirstOrDefault();

            if (transactionCost is null)
            {
                Clients.Caller.SendAsync("CheckError", "Отсутствует действующая цена товара на заданную дату");
                return;
            }

            cost = transactionCost.value;

            tax = _db.some_model.FromSqlInterpolated($"select value from Taxes where Tax_Id = 1").ToList()[0].value * cost;

            totalCost = cost + tax;

            if (date > DateTime.Now)
            {
                string date_description = "*Учитывая сегодняшний курс валюты";
                Clients.Caller.SendAsync("GiveCheck", cost.ToString("0.00"), tax.ToString("0.00"), totalCost.ToString("0.00"), date_description);
            }
            else
            {
                Clients.Caller.SendAsync("GiveCheck", cost.ToString("0.00"), tax.ToString("0.00"), totalCost.ToString("0.00"));
            }
        }

    }
}