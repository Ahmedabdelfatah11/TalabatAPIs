
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.CartRepository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service.AuthService;
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
            webApplicationbuilder.Services.AddScoped<IConnectionMultiplexer, ConnectionMultiplexer>(ServiceProvider =>
            {
            var connection = webApplicationbuilder.Configuration.GetConnectionString("redisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });
            webApplicationbuilder.Services.AddScoped(typeof(ICartRepository),typeof(CartRepository));


            webApplicationbuilder.Services.AddDbContext<ApplicationIdentityDbContext> (options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationbuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            webApplicationbuilder.Services.AddAuthService(webApplicationbuilder.Configuration);
            #endregion
            var app = webApplicationbuilder.Build();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var _dbcontext = Services.GetRequiredService<StoreContext>();
            var _identityDbContext = Services.GetRequiredService<ApplicationIdentityDbContext>();

            var logger = Services.GetRequiredService<ILogger<Program>>();
            try
            {
                await StoredContextSeed.SeedAsync(_dbcontext);
                await _dbcontext.Database.MigrateAsync();
                await _identityDbContext.Database.MigrateAsync();

                var _userManager = Services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityDataSeeding.SeedUsersAsync(_userManager);

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
