using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository.GenericRepository;
using Talabat.Service.AuthService;
using TalabatAPIs.Errors;
using TalabatAPIs.Helper;

namespace TalabatAPIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));

            /// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            ///builder.Services.AddOpenApi();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            }
            );
            return services;
        }

        public static IServiceCollection AddAuthService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:AuthKey"] ?? string.Empty)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero

                    };
                });
            return services;
        }
    }
}
