using CompanyEmployees.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

                if(Regex.IsMatch(employee.RecordNo, pattern) && GetOneEmployeeByRecordNo(employee.RecordNo) == null)
                {
                    if(employee.LowerEmployee != null)
                        AddingLowerEmployee(LowwerEmployeeListCreator(employee.LowerEmployee),employee.RecordNo);

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
            _context.Employees.Update(employee);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        //alt elemanları ekleme her record no arasında "." olacak tek string olarak depolanacak
        public void AddingLowerEmployee(IEnumerable<string> LowerEmployees,string recordNo)
        {
            foreach(string employee in LowerEmployees)
            {
                var item = GetOneEmployeeByRecordNo(employee);
                
                if(item.UpperEmployee != null)
                {
                    throw new Exception($"This epmloyee already have a upper employee.({employee})");
                }
                else
                {
                    item.UpperEmployee = recordNo;
                    UpdateEmployee(item);
                    Save();
                }
            }
        }

        public IEnumerable<string> LowwerEmployeeListCreator(string LowerEmployees)
        {
            string pattern = "^[a-zA-Z0-9]*$";
            //split every employee
            var employeeList =  LowerEmployees.Split('.');

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

        //bütün alt çalışanlar silinince, o alt çalışanların üstleri silinir
        public void CuttingRelationLowerEmployee(IEnumerable<string> LowerEmployees)
        {
            foreach(var employee in LowerEmployees)
            {
                var item = GetOneEmployeeByRecordNo(employee);
                item.UpperEmployee = null;
                Save();
            }
        }

        //üst olarak eklenen çalışanın, kime üst eklendiyse o çalışanı üstün alt eleman kısmına ekleme
        public void AddingUpperEmployee(string upperEmployee, string recordNumber)
        {
            var item = GetOneEmployeeByRecordNo(upperEmployee);

            var lowerList = LowwerEmployeeListCreator(item.LowerEmployee);
            List<string> lowerListForm = lowerList.ToList();
            lowerListForm.Add(recordNumber);
            string lowersTurnedString = null;

            foreach(var lower in lowerListForm)
            {
                lowersTurnedString = lowersTurnedString + item + ".";
            }

            item.UpperEmployee = lowersTurnedString;
            UpdateEmployee(item);
            Save();
        }

        //bir çalışanın üst çalışanı silindiğinde, o üst çalışandan alt çalışanı çıkarma metodu
        public void CuttingRelationUpperEmployee(string upperEmployee, string recordNumber)
        {
            var item = GetOneEmployeeByRecordNo(upperEmployee);

            var lowerList = LowwerEmployeeListCreator(item.LowerEmployee);
            List<string> lowerListForm = lowerList.ToList();
            lowerListForm.Remove(recordNumber);
            string lowersTurnedString = null;

            foreach (var lower in lowerListForm)
            {
                lowersTurnedString = lowersTurnedString + item + ".";
            }

            item.UpperEmployee = lowersTurnedString;
            UpdateEmployee(item);
            Save();
        }

        public void UpdateConfigration(IEnumerable<string> oldLower, IEnumerable<string> newLower, string recordNo)
        {
            List<string> cikanlar = oldLower.Except(newLower).ToList();
            List<string> girenler = newLower.Except(oldLower).ToList();

            foreach(var cikan in cikanlar)
            {
                CuttingRelationLowerEmployee(cikanlar);
            }

            foreach(var giren in girenler)
            {
                var item = GetOneEmployeeByRecordNo(giren);
                item.UpperEmployee = recordNo;
                Save();
            }


        }

    }
}
