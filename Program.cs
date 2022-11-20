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

            //���������� ����������� ����������� � ���������������
            builder.Services.AddControllersWithViews();

            //����������� ��
            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TradingCompanyDB")));

            //���������� �������������� (������� ����������� ������������) � ����������� (������� �����������, ����� �� ���� ����� ������� � ���������� �������) ����� ������
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new() { 
                    //����� �� �������������� �������� (?) ��� ��������� ������
                    ValidateIssuer = true,
                    //������, �������������� ��������
                    ValidIssuer = "MyAuthServer",
                    //����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    //������, �������������� ����������� ������
                    ValidAudience = "MyAuthClient",
                    //����� �� �������������� ����� ������������� (�� �����. 20 ���.)
                    ValidateLifetime = true,
                    //��������� ����� ������������
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretkey!123")),
                    //��������� ����� �������������
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

            #region �������� (������� �����-������ �����)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Manager}/{action=Index}");
            #endregion

            app.Run();
            #endregion
        }
    }
}