using System.Security.Cryptography;
using System.Text;
namespace Trading_company.Misc
{
    /// <summary>
    /// Различные методы, обеспечивающие безопасность приложения
    /// </summary>
    public static class Security
    {
        /// <summary>
        /// Шифрование пароля (MD5)
        /// </summary>
        /// <param name="password">Пароль</param>
        public static string HashPassword(string password)
        {
            byte[] hash = MD5.HashData(Encoding.ASCII.GetBytes(password));

            StringBuilder hashPass = new();
            foreach (var item in hash)
            {
                hashPass.Append(item.ToString("X2"));
            }

            return hashPass.ToString();
        }
    }
}