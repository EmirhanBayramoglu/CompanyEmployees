using CompanyEmployees.Entities.Models;
using System.Text.RegularExpressions;

namespace CompanyEmployees.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            else
            {
                /*string pattern = "^[a-zA-Z0-9]*$";
                if(Regex.IsMatch(employee.RecordNo, pattern))
                {
                    //adding system
                }
                else
                {
                    throw new Exception("Input string should only contain alphanumeric characters.");
                }*/
            }
            
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            throw new NotImplementedException();
        }

        public Employee GetOneEmployeeByRecordNo(string RecordNo)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
