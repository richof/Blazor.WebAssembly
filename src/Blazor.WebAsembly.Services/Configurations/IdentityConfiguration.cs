using Blazor.WebAsembly.Services.IdentityData;
using Blazor.WebAssembly.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Blazor.WebAsembly.Services.Configurations
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
           
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var appSettingsSection = configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<TokenSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "JwtBearer"; //JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = "JwtBearer"; //JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("JwtBearer",bOptions =>
            {
                bOptions.RequireHttpsMetadata = true;
                bOptions.SaveToken = true;
                bOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer,
                    ValidateLifetime=true,
                    ClockSkew=TimeSpan.FromMinutes(5)
                };

            });
            
            return services;
        }

        public static WebApplication UseIdentityConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
