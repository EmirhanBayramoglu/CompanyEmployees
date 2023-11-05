using CompanyEmployees.Entities.Models;
using System.Collections.Generic;

namespace CompanyEmployees.Repositories
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<Employee>> GetAllEmployee();
        Task<Employee> GetOneEmployeeByRecordNo(string recordNo);
        public Task AddEmployee(Employee employee);
        public Task UpdateEmployee(Employee employee);
        public Task UpdateConfigration(IEnumerable<string> oldLower, IEnumerable<string> newLower,string oldUpper ,Employee employee);
        Task<IEnumerable<string>> LowwerEmployeeListCreator(string LowerEmployees);
        public Task Save();

    }
}
