using AutoMapper;
using CompanyEmployees.Dto;
using CompanyEmployees.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new Exception("You ."); ;
            }
            else
            {
                //alphanumeric sayı kontrolü için pattern
                string pattern = "^[a-zA-Z0-9]*$";
                
                //daha önce bu recordno ile bir çalışan kayıt edilmiş mi diye kontrol için alttaki işlem yapılıyor
                var item = await GetOneEmployeeByRecordNo(employee.RecordNo);


                if (Regex.IsMatch(employee.RecordNo, pattern) && item == null && employee.RecordNo.Length ==11)
                {
                    if (employee.LowerEmployee != null)
                    {
                        string lowEmp = employee.LowerEmployee;
                        var lowEmpList = await LowwerEmployeeListCreator(lowEmp);

                        //eklenen elemanın her bir alt çalışanına gidip her birinin üst çalışanı güncellenir
                        AddingLowerEmployee(lowEmpList,employee);
                    }
                    
                    //üst çalışana gidilip onun alt çalışanına eklenir yeni eklenen çalışan
                    if (employee.UpperEmployee != null)
                        await AddingUpperEmployee(employee.UpperEmployee, employee.RecordNo);
                    
                    //ekleme işlemini yapıyoruz
                    _context.Employees.Add(employee);
                }
                else
                {
                    throw new Exception("Input string should only contain alphanumeric characters.");
                }
            }
            
        }

        //tüm çalışanları çağırma
        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            return await _context.Employees.ToListAsync();
        }

        //recordno ya göre istenen elemanı çağırma
        public async Task<Employee> GetOneEmployeeByRecordNo(string recordNo)
        {
            var item = await _context.Employees.FirstOrDefaultAsync(x => x.RecordNo == recordNo);
            return item;
        }

        //update metodu
        public async Task UpdateEmployee(Employee employee)
        {
           _context.Employees.Update(employee);
            await Save();
        }
        
        //asenkron save metodu
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        
        //alt çalışan olarak eklenen kişilerin hepsinin üst çalışanlarının güncellenmesi
        public void AddingLowerEmployee(IEnumerable<string> LowerEmployees, Employee employee)
        {

            var empl = employee;
            foreach(string lowerEmployee in LowerEmployees)
            {
                var item =  GetOneEmployeeByRecordNo(lowerEmployee).Result;
                
                //kayıtlı üst çalışan kontrol
                if (item.UpperEmployee != null)
                {
                    throw new Exception($"This epmloyee already have a upper employee.({lowerEmployee})");
                }
                //üst çalışan olarak eklenmeye çalışan kişi kayıtlı alt elemanı mı kontrol
                else if (item.LowerEmployee != null)
                {
                    List<string> lowList = LowwerEmployeeListCreator(item.LowerEmployee).Result.ToList();
                    if (lowList.Contains(lowerEmployee))   
                        throw new Exception($"Your upper employee can not be your lower employee.({lowerEmployee})");
                }
                else
                {
                    //eklenen alt elemanın üst eleman özelliğinin güncellenmesi
                    item.UpperEmployee = empl.RecordNo;
                    _context.Employees.Update(item);
                }
            }
        }

        //string olarak gönderilen alt çalışanları IEnumerable<string> yapar aynı zamanda kotrolleri de gerçekleştirir
        public async Task<IEnumerable<string>> LowwerEmployeeListCreator(string LowerEmployees)
        {
            string pattern = "^[a-zA-Z0-9]*$";
            //split every employee
            if (LowerEmployees != null)
            {
                List<string> employeeList = ((string)LowerEmployees).Split('.').ToList();
                employeeList.RemoveAll(item => string.IsNullOrEmpty(item));

                foreach (var employee in employeeList)
                {
                    if (employee.Length != 11 && Regex.IsMatch(employee, pattern) && employee.Length != 0)
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
        public async Task CuttingRelationLowerEmployee(IEnumerable<string> LowerEmployees)
        {
            foreach(var employee in LowerEmployees)
            {
                var item = await GetOneEmployeeByRecordNo(employee);
                item.UpperEmployee = null;
                _context.Employees.Update(item);
            }
        }

        //üst olarak eklenen çalışanın, kime üst eklendiyse o çalışanı üstün alt eleman kısmına ekleme
        public async Task AddingUpperEmployee(string upperEmployee, string recordNumber)
        {
            var item = await GetOneEmployeeByRecordNo(upperEmployee);

            var lowerList = await LowwerEmployeeListCreator(item.LowerEmployee);
            if(lowerList != null)
            {
                List<string> lowerListForm = lowerList.ToList();  
                lowerListForm.Add(recordNumber);
                string lowersTurnedString = null;

                foreach(var lower in lowerListForm)
                {
                    lowersTurnedString = lowersTurnedString + lower + ".";
                }

                item.LowerEmployee = lowersTurnedString;
                _context.Employees.Update(item);
            }
            else
            {
                item.LowerEmployee = recordNumber;
                _context.Employees.Update(item);
            }
            

        }

        //bir çalışanın üst çalışanı silindiğinde, o üst çalışandan alt çalışanı çıkarma metodu
        public async Task CuttingRelationUpperEmployee(string upperEmployee, string recordNumber)
        {
            var item = await GetOneEmployeeByRecordNo(upperEmployee);

            var lowerList = await LowwerEmployeeListCreator(item.LowerEmployee);
            List<string> lowerListForm = lowerList.ToList();
            lowerListForm.Remove(recordNumber);
            string lowersTurnedString = null;

            foreach (var lower in lowerListForm)
            {
                lowersTurnedString = lowersTurnedString + lower + ".";
            }

            item.LowerEmployee = lowersTurnedString;
            _context.Employees.Update(item);
        }

        //update için ayarlamalar
        public async Task UpdateConfigration(IEnumerable<string> oldLower, IEnumerable<string> newLower,string oldUpper ,Employee employee)
        {
            var empl = employee;

            //üst elemanın değişikliğinin kontrolü
            if (empl.UpperEmployee != oldUpper) 
            {
                if(oldUpper != null)
                    await CuttingRelationUpperEmployee(oldUpper, empl.RecordNo);
                if(empl.UpperEmployee != null)
                    await AddingUpperEmployee(empl.UpperEmployee, empl.RecordNo);

            }
            //alt elemanlardaki farklılıkları bulur farklılığa göre işlemleri gerçekleştirir (giren ve çıkanları bulur)
            if (oldLower != null)
            {
                List<string> cikanlar = null;
                if (newLower != null)
                    cikanlar = oldLower.Except(newLower).ToList();
                else
                    cikanlar = oldLower.ToList();

                foreach (var cikan in cikanlar)
                {
                    await CuttingRelationLowerEmployee(cikanlar);
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
                    var item = await GetOneEmployeeByRecordNo(giren);

                    List<string> lowList = null;
                    if (item.LowerEmployee != null)
                    {
                        lowList = LowwerEmployeeListCreator(item.LowerEmployee).Result.ToList();

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
