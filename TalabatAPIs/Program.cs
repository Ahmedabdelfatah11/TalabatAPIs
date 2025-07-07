
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Repository.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using TalabatAPIs.Errors;
using TalabatAPIs.Extensions;
using TalabatAPIs.Helper;

namespace TalabatAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure
            webApplicationbuilder.Services.AddControllers();

            webApplicationbuilder.Services.AddSwaggerServices();
            webApplicationbuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationbuilder.Services.AddApplicationServices();
            #endregion
            var app = webApplicationbuilder.Build();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var _dbcontext = Services.GetRequiredService<StoreContext>();

            var logger = Services.GetRequiredService<ILogger<Program>>();
            try
            {
                await StoredContextSeed.SeedAsync(_dbcontext);
                await _dbcontext.Database.MigrateAsync();

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration");
            }

            // Configure the HTTP request pipeline.
            #region Configure Kestrel Middleares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion



            app.Run();
        }
    }
}
