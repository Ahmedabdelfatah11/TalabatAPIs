﻿using Microsoft.AspNetCore.Builder;

namespace TalabatAPIs.Extensions
{
    public static class SwaggerServicesExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
          services.AddEndpointsApiExplorer();
          services.AddSwaggerGen();
            return services;

        }
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app) 
            {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
