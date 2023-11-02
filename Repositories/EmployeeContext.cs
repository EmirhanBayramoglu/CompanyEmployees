using CompanyEmployees.Entities.Models;
using CompanyEmployees.Repositories.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Repositories
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions opt) : 
            base(opt) 
        {

        }

        public DbSet<Employee> Employees { get; set; }

        

    }
}
