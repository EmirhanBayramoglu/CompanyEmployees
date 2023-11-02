using CompanyEmployees.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) => services.AddDbContext<EmployeeContext>(options =>
                                                options.UseSqlServer(configuration.GetConnectionString("EmployeeConnetion")));

        public static void ConfigureRepository(this IServiceCollection services) =>
            services.AddScoped<IEmployeeRepo, EmployeeRepo>();

    }
}
