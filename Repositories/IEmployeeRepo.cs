using CompanyEmployees.Entities.Models;

namespace CompanyEmployees.Repositories
{
    public interface IEmployeeRepo
    {
        IEnumerable<Employee> GetAllEmployee();
        Employee GetOneEmployeeByRecordNo(string RecordNo);
        public void AddEmployee(Employee employee);
        public void UpdateEmployee(Employee employee);

    }
}
