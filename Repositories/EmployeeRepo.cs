using CompanyEmployees.Entities.Models;
using System.Text.RegularExpressions;

namespace CompanyEmployees.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {

        private readonly EmployeeContext _context;

        public EmployeeRepo(EmployeeContext context)
        {
            _context = context;

        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            else
            {
                string pattern = "^[a-zA-Z0-9]*$";
                if(Regex.IsMatch(employee.RecordNo, pattern))
                {
                    _context.Employees.Add(employee);
                }
                else
                {
                    throw new Exception("Input string should only contain alphanumeric characters.");
                }
            }
            
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _context.Employees.ToList();
        }

        public Employee GetOneEmployeeByRecordNo(string recordNo)
        {
            return _context.Employees.FirstOrDefault(x => x.RecordNo == recordNo);
        }

        public void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
