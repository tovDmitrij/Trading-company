using Microsoft.EntityFrameworkCore;
using Trading_company.Hubs;
using Trading_company.Models;
namespace Trading_company
{
    public class Program
    {
        public static void Main(string[] args)
        {



            #region Builder

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TradingCompanyDB")));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Manager.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            #endregion



            #region App

            var app = builder.Build();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.MapHub<TradingCompanyHub>("/hub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Manager}/{action=SignUp}");

            app.Run();

            #endregion



        }
    }
}