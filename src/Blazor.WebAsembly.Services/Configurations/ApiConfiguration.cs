using Blazor.WebAsembly.Services.AutoMapper;
using Blazor.WebAsembly.Services.Data;

namespace Blazor.WebAsembly.Services.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddCors(options =>
            {
                options.AddPolicy("All",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            return services;

        }
        public static WebApplication UseApiConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSwaggerConfiguration();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("All");

            app.UseIdentityConfiguration();
            app.MapControllers();
            return app;
        }
    }
}
