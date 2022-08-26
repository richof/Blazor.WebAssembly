using Blazor.WebAsembly.Services.Data;

namespace Blazor.WebAsembly.Services.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<ICompanyData, CompanyData>();
            return services;
        }   
    }
}
