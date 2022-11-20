using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Trading_company.Models;
namespace Trading_company
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Builder
            var builder = WebApplication.CreateBuilder(args);

            //Добавление функционала контроллёров с представлениями
            builder.Services.AddControllersWithViews();

            //Подключение БД
            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TradingCompanyDB")));

            //Добавление аутентификации (процесс определения пользователя) и авторизации (процесс определения, имеет ли юзер право доступа к некоторому ресурсу) через токены
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new() { 
                    //будет ли валидироваться издатель (?) при валидации токена
                    ValidateIssuer = true,
                    //строка, представляющая издателя
                    ValidIssuer = "MyAuthServer",
                    //будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    //строка, представляющая потребителя токена
                    ValidAudience = "MyAuthClient",
                    //будет ли валидироваться время существования (по умолч. 20 мин.)
                    ValidateLifetime = true,
                    //установка ключа безопасности
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretkey!123")),
                    //валидация ключа безопапсности
                    ValidateIssuerSigningKey = true
                };
            });
            #endregion

            #region App
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region Маршруты (настрою когда-нибудь потом)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Manager}/{action=Index}");
            #endregion

            app.Run();
            #endregion
        }
    }
}