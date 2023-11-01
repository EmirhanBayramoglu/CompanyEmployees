using CompanyEmployees.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Repositories
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> opt) : base(opt) 
        {

        }

        public DbSet<Employee> Employees { get; set; }

    }
}
