using certificados.dal.DataAccess;
using certificados.models.Context;
using certificados.services.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using certificados.web.Infrastructure;

namespace certificados.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddServices();

            //builder.Services.AddScoped<LogService>();
            //builder.Services.AddScoped<UsuarioService>();
            //builder.Services.AddScoped<TpersonaDA>();
            //builder.Services.AddScoped<TusuarioDA>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Midelware para capturar las exepciones de todo TIPO y registrar en LA DB
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var logService = context.RequestServices.GetRequiredService<LogService>();
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                    if (exception != null)
                    {
                        await logService.RegistrarExcepcion(exception);
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Error = "Se produjo un error en el servidor. Por favor, contacte al soporte técnico."
                    });
                });
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}
