using CompanyEmployees.Entities.Models;
using Microsoft.AspNetCore.Mvc;
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

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddingLowerEmployee(IEnumerable<string> LowwerEmployees)
        {
            foreach(string employee in LowwerEmployees)
            {
                var item = GetOneEmployeeByRecordNo(employee);
                
                if(item.UpperEmployee != null)
                {
                    throw new Exception($"This epmloyee already have a upper employee.({employee})");
                }

            }
        }

        public IEnumerable<string> LowwerEmployeeListCreator(string LowwerEmployees)
        {
            string pattern = "^[a-zA-Z0-9]*$";
            //split every employee
            var employeeList =  LowwerEmployees.Split('.');

            foreach (var employee in employeeList)
            {
                if(employee.Length!=11 && Regex.IsMatch(employee, pattern))
                {
                    throw new Exception("Some lowwer employees wrong.");
                }
                else if(GetOneEmployeeByRecordNo(employee)==null)
                {
                    throw new Exception($"There is no employee with this record no: {employee}" );
                }
            }
            return employeeList;
        }
    }
}
