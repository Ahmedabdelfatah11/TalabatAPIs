
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Repository.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

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
            webApplicationbuilder.Services.AddEndpointsApiExplorer();
            webApplicationbuilder.Services.AddSwaggerGen();
            webApplicationbuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationbuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            #endregion
            var app = webApplicationbuilder.Build();

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
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion



            app.Run();
        }
    }
}
