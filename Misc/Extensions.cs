using System.Text.Json;
namespace Trading_company.Misc
{
    public static class Extensions
    {
        /// <summary>
        /// Метод, возвращающий значение куки из сессии по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Значение ключа</returns>
        public static T? Get<T>(this ISession session, string key)
        {
            string? value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        /// <summary>
        /// Метод, к-рый устанавливает значение куки в сессии по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }
    }
}