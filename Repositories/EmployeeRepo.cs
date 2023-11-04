using AutoMapper;
using CompanyEmployees.Dto;
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
                    if (employee.LowerEmployee != null)
                    {
                        string lowEmp = employee.LowerEmployee;
                        var lowEmpList = LowwerEmployeeListCreator(lowEmp);
                        AddingLowerEmployee(lowEmpList,employee);
                    }
                        
                    if (employee.UpperEmployee != null)
                        AddingUpperEmployee(employee.UpperEmployee, employee.RecordNo);

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

        
        public void AddingLowerEmployee(IEnumerable<string> LowerEmployees, Employee employee)
        {

            var empl = employee;
            foreach(string lowerEmployee in LowerEmployees)
            {
                var item = GetOneEmployeeByRecordNo(lowerEmployee);

                if (item.UpperEmployee != null)
                {
                    throw new Exception($"This epmloyee already have a upper employee.({lowerEmployee})");
                }
                else if (item.LowerEmployee != null)
                {
                    List<string> lowList = LowwerEmployeeListCreator(item.LowerEmployee).ToList();
                    if (lowList.Contains(lowerEmployee))   
                        throw new Exception($"Your upper employee can not be your lower employee.({lowerEmployee})");
                }
                else
                {
                    item.UpperEmployee = empl.RecordNo;
                    _context.Employees.Update(item);
                }
            }
        }

        public IEnumerable<string> LowwerEmployeeListCreator(string LowerEmployees)
        {
            string pattern = "^[a-zA-Z0-9]*$";
            //split every employee
            if (LowerEmployees != null)
            {
                var employeeList = ((string)LowerEmployees).Split('.');

                foreach (var employee in employeeList)
                {
                    if (employee.Length != 11 && Regex.IsMatch(employee, pattern))
                    {
                        throw new Exception("Some lowwer employees wrong.");
                    }
                    else if (GetOneEmployeeByRecordNo(employee) == null)
                    {
                        throw new Exception($"There is no employee with this record no: {employee}");
                    }
                }
                return employeeList;
            }
            else
                return null;

            
        }

        //bütün alt çalışanlar silinince, o alt çalışanların üstleri silinir
        public void CuttingRelationLowerEmployee(IEnumerable<string> LowerEmployees)
        {
            foreach(var employee in LowerEmployees)
            {
                var item = GetOneEmployeeByRecordNo(employee);
                item.UpperEmployee = null;
                _context.Employees.Update(item);
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
            _context.Employees.Update(item);
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
            _context.Employees.Update(item);
            Save();
        }

        public void UpdateConfigration(IEnumerable<string> oldLower, IEnumerable<string> newLower, Employee employee)
        {
            var empl = employee;

            if (oldLower != null)
            {
                List<string> cikanlar = null;
                if (newLower != null)
                    cikanlar = oldLower.Except(newLower).ToList();
                else
                    cikanlar = oldLower.ToList();

                foreach (var cikan in cikanlar)
                {
                    CuttingRelationLowerEmployee(cikanlar);
                }
            }

            if (newLower != null)
            {
                List<string> girenler = null;
                if (oldLower != null)
                    girenler = newLower.Except(oldLower).ToList();
                else
                    girenler = newLower.ToList();

                foreach (var giren in girenler)
                {
                    var item = GetOneEmployeeByRecordNo(giren);

                    List<string> lowList = null;
                    if (item.LowerEmployee != null)
                    {
                        LowwerEmployeeListCreator(item.LowerEmployee).ToList();

                        if (lowList.Contains(giren))
                        {
                            throw new Exception($"Your upper employee can not be your lower employee.({giren})");
                        }
                    }

                    if (item.UpperEmployee != null)
                    {
                        throw new Exception($"This epmloyee already have a upper employee.({giren})");
                    }  
                    else
                    {
                        item.UpperEmployee = empl.RecordNo;
                        _context.Employees.Update(item);
                    }
                }
            }          
        }
    }
}
