using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Trading_company.Areas.Transaction.Models;
using Trading_company.Models;
namespace Trading_company.Hubs
{
    /// <summary>
    /// Хаб для обработки различных запросов в режиме реального времени
    /// </summary>
    public sealed class TradingCompanyHub : Hub
    {
        /// <summary>
        /// БД "Торговое предприятие
        /// </summary>
        private readonly DataContext _db;

        public TradingCompanyHub(DataContext dbContext) => _db = dbContext;



        #region Взаимодействие с аккаунтом менеджера

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
            else
            {
                Clients.Caller.SendAsync("Submit");
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
            else
            {
                Clients.Caller.SendAsync("Submit");
            }
        }

        #endregion



        #region Взаимодействие с транзакциями

        /// <summary>
        /// Генерация чека 
        /// </summary>
        /// <param name="prod_id">Идентификатор товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <param name="transaction_date">Дата продажи товара</param>
        public void GetCheck(string prod_id, string quantity, string transaction_date)
        {
            DateTime date = DateTime.Parse(transaction_date);

            var dbPrice = _db.some_model.FromSqlInterpolated($"select pc.value * (select value from cources where cur_idfrom = crc.cur_id and cur_idto = 1 and dayfrom < {DateTime.Now} and {DateTime.Now} <= dayto) cost from Products pd left join Prices pc on pd.prod_id = pc.prod_id left join Currencies crc on pc.cur_id = crc.cur_id left join Cources cr on crc.cur_id = cr.cur_idfrom where pd.prod_id = {Convert.ToInt32(prod_id)} and pc.dayfrom < {date} and  pc.dateto >= {date}").ToList().FirstOrDefault();

            if (dbPrice is null)
            {
                Clients.Caller.SendAsync("CheckError", "Отсутствует действующая цена товара на заданную дату");
                return;
            }

            //Цена товара
            double price = dbPrice.value;

            //Стоимость транзакции
            double cost = price * Convert.ToInt32(quantity);

            //Сколько заберут налоги
            double tax = _db.some_model.FromSqlInterpolated($"select value from Taxes where Tax_Id = 1").ToList()[0].value * cost;

            //Стоимость + налоги
            double totalCost = cost + tax;

            if (date > DateTime.Now)
            {
                string date_description = "*Учитывая сегодняшний курс валюты";
                Clients.Caller.SendAsync("GiveCheck", price.ToString("0.00"), cost.ToString("0.00"), tax.ToString("0.00"), totalCost.ToString("0.00"), date_description);
            }
            else
            {
                Clients.Caller.SendAsync("GiveCheck", price.ToString("0.00"), cost.ToString("0.00"), tax.ToString("0.00"), totalCost.ToString("0.00"));
            }
        }

        /// <summary>
        /// Подтверждение транзакции (покупка товара)
        /// </summary>
        /// <param name="prod_id">Идентификатор товара</param>
        /// <param name="transaction_date">Дата совершения транзакции</param>
        public void SubmitBuy(string prod_id, string transaction_date, string contract_id)
        {
            DateTime transactionDate = DateTime.Parse(transaction_date);
            int prodID = Convert.ToInt32(prod_id);
            int contractID = Convert.ToInt32(contract_id);

            if (_db.contracts_with_optional_info.FirstOrDefault(contract => contract.id == contractID && contract.dayto > transactionDate) is null)
            {
                Clients.Caller.SendAsync("Message", "На момент продажи контракт будет просрочен");
                return;
            }

            if (_db.some_model.FromSqlInterpolated($"select value from Prices where prod_id = {Convert.ToInt32(prod_id)} and dayfrom < {transactionDate} and {transactionDate} <= dateto").ToList().FirstOrDefault() is null)
            {
                Clients.Caller.SendAsync("Message", "Отсутствует действующий ценник на заданную дату");
            }
            else
            {
                Clients.Caller.SendAsync("Submit");
            }
        }

        /// <summary>
        /// Подтверждение транзакции (продажа товара)
        /// </summary>
        /// <param name="prod_id">Идентификатор товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <param name="transaction_date">Дата совершения транзакции</param>
        public void SubmitSell(string prod_id, string quantity, string transaction_date, string contract_id)
        {
            DateTime transactionDate = DateTime.Parse(transaction_date);
            int prodID = Convert.ToInt32(prod_id);
            int contractID = Convert.ToInt32(contract_id);
            int prodQuantity = Convert.ToInt32(quantity);

            if (_db.contracts_with_optional_info.FirstOrDefault(contract => contract.id == contractID && contract.dayto > transactionDate) is null)
            {
                Clients.Caller.SendAsync("Message", "На момент продажи контракт будет просрочен");
                return;
            }

            if (_db.some_model.FromSqlInterpolated($"select value from Prices where prod_id = {prodID} and dayfrom < {transactionDate} and {transactionDate} <= dateto").ToList().FirstOrDefault() is null)
            {
                Clients.Caller.SendAsync("Message", "Отсутствует действующий ценник на заданную дату");
            }
            else if (Convert.ToInt32(_db.some_model.FromSqlInterpolated($"select coalesce((select Sum(Incoming.quantify) from Incoming left join Products on Incoming.prod_id = Products.prod_id where Products.prod_id = {prodID} and Incoming.Inc_Date <= {transactionDate}),0) - coalesce((select Sum(Outgoing.quantify) from Outgoing left join Products on Outgoing.prod_id = Products.prod_id where Products.prod_id = {prodID} and Outgoing.Out_Date <= {transactionDate}),0)").ToList().First().value) - prodQuantity < 0)
            {
                Clients.Caller.SendAsync("Message", "На складе нет (не будет) товара в заданном количестве");
            }
            else
            {
                Clients.Caller.SendAsync("Submit");
            }
        }

        /// <summary>
        /// Отменить транзакцию продажи товара
        /// </summary>
        /// <param name="transaction_id">Идентификатор транзакции</param>
        public void DeleteSellTransaction(string transaction_id)
        {
            int transactionID = Convert.ToInt32(transaction_id);

            OutgoingModel outgoingModel = _db.outgoing_with_optional_info.First(transaction => transaction.transaction_id == transactionID);
            
            if (_db.some_model.FromSqlInterpolated($"select coalesce((select Sum(prod_quantity) from Incoming_With_Optional_Info where prod_id = {outgoingModel.prod_id}),0) - coalesce((select Sum(prod_quantity) from Outgoing_With_Optional_Info where transaction_id != {outgoingModel.transaction_id} and prod_id = {outgoingModel.prod_id}),0)").ToList().First().value < 0)
            {
                Clients.Caller.SendAsync("Message", "Отменить транзакцию невозможно, потому как есть запланированные в будущем контракты, зависящие от данного контракта");
            }
            else
            {
                Clients.Caller.SendAsync("SubmitDeleteSell", transaction_id);
            }
        }

        /// <summary>
        /// Отменить транзакцию покупки товара
        /// </summary>
        /// <param name="transaction_id">Идентификатор транзакции</param>
        public void DeleteBuyTransaction(string transaction_id)
        {
            int transactionID = Convert.ToInt32(transaction_id);

            IncomingModel incomingModel = _db.incoming_with_optional_info.First(transaction => transaction.transaction_id == transactionID);

            if (_db.some_model.FromSqlInterpolated($"select coalesce((select Sum(prod_quantity) from Incoming_With_Optional_Info where transaction_id != {incomingModel.transaction_id} and prod_id = {incomingModel.prod_id}),0) - coalesce((select Sum(prod_quantity) from Outgoing_With_Optional_Info where prod_id = {incomingModel.prod_id}),0)").ToList().First().value < 0)
            {
                Clients.Caller.SendAsync("Message", "Отменить транзакцию невозможно, потому как есть запланированные в будущем контракты, зависящие от данного контракта");
            }
            else
            {
                Clients.Caller.SendAsync("SubmitDeleteBuy", transaction_id);
            }
        }

        #endregion



        #region Взаимодействие с контрактами

        /// <summary>
        /// Проверка при подписании контракта, что на заданную дату у конкретного контрагента будет действующий банковский аккаунт
        /// </summary>
        /// <param name="contragent_id">Идентификатор контрагента</param>
        /// <param name="contract_dayto">Предполагаемая дата завершения контракта</param>
        public void CheckBankAccount(string contragent_id, string contract_dayto)
        {
            DateTime contractDayTo = DateTime.Parse(contract_dayto);
            int contragentID = Convert.ToInt32(contragent_id);

            var existedBankAccount = _db.accounts.FirstOrDefault(account => account.contr_id == contragentID && account.dayto > contractDayTo);

            if (existedBankAccount is null)
            {
                Clients.Caller.SendAsync("Message", "На заданную дату у контрагента не будет действующего банковского аккаунта");
            }
            else
            {
                Clients.Caller.SendAsync("Submit");
            }
        }

        /// <summary>
        /// Досрочное завершение контракта
        /// </summary>
        /// <param name="contract_id">Идентификатор контракта</param>
        /// <param name="contract_date">Желаемая дата</param>
        public void EndEarlyContract(string contract_id, string contract_date)
        {
            DateTime contractDate = DateTime.Parse(contract_date);
            int contractID = Convert.ToInt32(contract_id);
            int contragentID = _db.contracts_with_optional_info.FirstOrDefault(contract => contract.id == contractID).contr_id;

            if (
                _db.incoming_with_optional_info.FromSqlInterpolated($"select* from incoming_with_optional_info where contract_id = {contractID} and transaction_date > {contractDate}").ToList().Count() > 0
                || 
                _db.outgoing_with_optional_info.FromSqlInterpolated($"select* from outgoing_with_optional_info where contract_id = {contractID} and transaction_date > {contractDate}").ToList().Count() > 0)
            {
                Clients.Caller.SendAsync("Message", "Сдвинуть срок контракта на заданную дату невозможно: существуют транзакции, к-рые ещё не были совершены после заданной даты");
            }
            else if (_db.accounts.FirstOrDefault(account => account.contr_id == contragentID && account.dayto > contractDate) is null)
            {
                Clients.Caller.SendAsync("Message", "На заданную дату у контрагента не будет действующего банковского аккаунта");
            }
            else
            {
                Clients.Caller.SendAsync("Submit", contract_id, contract_date);
            }
        }

        #endregion



        #region Взаимодействие с курсами валют

        /// <summary>
        /// Получение значений курса валют за последние 14 дней
        /// </summary>
        /// <param name="courseID">Идентификатор корвертируемого курса</param>
        public void GetCourseValues(string courseID)
        {
            if (courseID is null || courseID == "")
            {
                Clients.Caller.SendAsync("Message", "Выберите конвертируемую валюту");
            }
            else
            {
                int course_id = Convert.ToInt32(courseID);

                var courseList = _db.course_with_optional_info.FromSqlInterpolated($"select * from course_with_optional_info where cur_idto = 1 and cur_idfrom = {course_id} and dayto >= {DateTime.Now.AddDays(-14)} order by dayto").ToList();

                Clients.Caller.SendAsync("Submit", courseList);
            }
        }

        #endregion



        #region Взаимодействие с ценниками товаров

        /// <summary>
        /// Получение ценников выбранного товара за последний месяц
        /// </summary>
        /// <param name="prodID">Идентификатор выбранного товара</param>
        public void GetPriceValues(string prodID)
        {
            if (prodID is null || prodID == "")
            {
                Clients.Caller.SendAsync("Message", "Выберите интересующий вас товар");
            }
            else
            {
                int product_id = Convert.ToInt32(prodID);

                var priceList = _db.prices_with_optional_info.FromSqlInterpolated($"select* from prices_with_optional_info where product_id = {product_id} and price_dayto >= {DateTime.Now.AddDays(-31)}").ToList();

                Clients.Caller.SendAsync("Submit", priceList);
            }
        }

        #endregion



    }
}