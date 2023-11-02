using CompanyEmployees.Entities.Models;
using System.Collections.Generic;

namespace CompanyEmployees.Repositories
{
    public interface IEmployeeRepo
    {
        IEnumerable<Employee> GetAllEmployee();
        Employee GetOneEmployeeByRecordNo(string recordNo);
        public void AddEmployee(Employee employee);
        public void UpdateEmployee(Employee employee);

    }
}
