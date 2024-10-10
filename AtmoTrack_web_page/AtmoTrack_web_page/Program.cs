using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session; // Adicione esta linha
using Microsoft.Extensions.FileProviders;

namespace AtmoTrack_web_page
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Adiciona o servi�o de autentica��o por cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";  // Caminho para a p�gina de login
                    options.LogoutPath = "/Login/Logout"; // Caminho para logout
                    options.AccessDeniedPath = "/Home/Index"; // P�gina para acesso negado (opcional)
                });

            // Adiciona suporte a sess�es
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Dura��o da sess�o
                options.Cookie.HttpOnly = true; // O cookie da sess�o n�o pode ser acessado por scripts
                options.Cookie.IsEssential = true; // O cookie � essencial para o funcionamento da aplica��o
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            // Habilitar a autentica��o e autoriza��o
            app.UseAuthentication(); // Deve ser adicionado antes do Authorization
            app.UseAuthorization();

            // Habilitar sess�es
            app.UseSession(); // Adicione esta linha

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
