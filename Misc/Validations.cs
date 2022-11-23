using Microsoft.AspNetCore.Mvc;
using Trading_company.Models;
namespace Trading_company.Misc
{
    /// <summary>
    /// Проверка валидности вводимых данных
    /// </summary>
    [NonController]
    public static class Validations
    {
        /// <summary>
        /// Проверка на валидность данных при регистрации
        /// </summary>
        /// <param name="manager">Данные о менеджере</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        public static bool CheckSignUpValidation(ManagerModel manager, out string? Error)
        {
            if (!IsFullNameValid(manager.fullname, out Error))
            {
                return false;
            }
            if (!IsEmailValid(manager.email, out Error))
            {
                return false;
            }
            if (!IsPasswordValid(manager.password, out Error))
            {
                return false;
            }
            if (!IsPercentValid(manager.percent, out Error))
            {
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Процерка на валидность данных при авторизации
        /// </summary>
        /// <param name="manager">Данные о менеджере</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        public static bool CheckSignInValidation(ManagerModel manager, out string? Error)
        {
            if (!IsEmailValid(manager.email, out Error))
            {
                return false;
            }
            if (!IsPasswordValid(manager.password, out Error))
            {
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Проверка на валидность данных при подписании контракта
        /// </summary>
        /// <param name="contract">Контракт</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        /// <returns></returns>
        public static bool CheckContractValidation(ContractModel contract, out string? Error)
        {
            if (!IsFullNameValid(contract.ContrFullName, out Error))
            {
                return false;
            }

            Error = null;
            return true;
        }



        /// <summary>
        /// Валидна ли введённая почта
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        private static bool IsEmailValid(string? email, out string? Error) 
        {
            if (email is null)
            {
                Error = "Пожалуйста, введите почту!";
                return false;
            }
            if (email.Count(ch => ch == '@') != 1)
            {
                Error = "В указанной почте отсутствует символ '@'!";
                return false;
            }
            if (email.IndexOf('@') == email.Length - 1)
            {
                Error = "В указанной почте отсутствует домен после символа '@'!";
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Валиден ли введённый пароль
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        private static bool IsPasswordValid(string? password, out string? Error)
        {
            if (password is null)
            {
                Error = "Пожалуйста, введите пароль!";
                return false;
            }
            if (password.Length < 8)
            {
                Error = "Длина пароля должна быть не менее 8 символов!";
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Валидна ли введённая ФИО
        /// </summary>
        /// <param name="fullName">ФИО</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        /// <returns></returns>
        private static bool IsFullNameValid(string? fullName, out string? Error)
        {
            if (fullName is null)
            {
                Error = "Пожалуйста, введите ФИО!";
                return false;
            }
            if (fullName.Count(ch => ch == ' ') != 2)
            {
                Error = "Между фамилией, именем и отчеством необходим один отступ в виде пробела!";
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Валиден ли введённый процент с продаж
        /// </summary>
        /// <param name="percent">Процент</param>
        /// <param name="Error">Ошибка, если она возникнет</param>
        /// <returns></returns>
        private static bool IsPercentValid(double percent, out string? Error)
        {
            if (percent < 0.0)
            {
                Error = "Пожалуйста, задайте валидный процент с продаж!";
                return false;
            }
            if (percent == 0.0)
            {
                Error = "Пожалуйста, задайте процент с продаж!";
                return false;
            }
            if (percent > 50.0)
            {
                Error = "Процент с продаж должен быть меньше 51!";
                return false;
            }

            Error = null;
            return true;
        }
    }
}