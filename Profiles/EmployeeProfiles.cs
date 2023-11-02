using AutoMapper;
using CompanyEmployees.Dto;
using CompanyEmployees.Entities.Models;

namespace CompanyEmployees.Profiles
{
    public class EmployeeProfiles : Profile
    {
        public EmployeeProfiles() 
        {
            CreateMap<Employee, EmployeeDto>();
        }

    }
}
