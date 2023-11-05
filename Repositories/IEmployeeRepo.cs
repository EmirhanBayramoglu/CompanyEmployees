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
        public void UpdateConfigration(IEnumerable<string> oldLower, IEnumerable<string> newLower,string oldUpper ,Employee employee);
        IEnumerable<string> LowwerEmployeeListCreator(string LowerEmployees);
        public void Save();

    }
}
