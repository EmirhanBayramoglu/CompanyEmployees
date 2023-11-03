using AutoMapper;
using CompanyEmployees.Dto;
using CompanyEmployees.Entities.Models;
using CompanyEmployees.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _repository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult <IEnumerable<Employee>> GetAllEmployee()
        {
            var items  =  _repository.GetAllEmployee();

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(items));
        }

        [HttpGet("{RecordNo}")]
        public ActionResult <EmployeeDto> GetOneEmployee(string RecordNo)
        {
            var item = _repository.GetOneEmployeeByRecordNo(RecordNo);
            
            return Ok(_mapper.Map<EmployeeDto>(item));
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee(Employee employee)
        {
            _repository.AddEmployee(employee);
            _repository.Save();

            return Ok();
        }

        [HttpPut("RecordNo")]
        public ActionResult UpdateEmployee(string recordNo,EmployeeUpdateDto employeeUpdateDto)
        {
            var item = _repository.GetOneEmployeeByRecordNo(recordNo);

            _mapper.Map(employeeUpdateDto, item);

            _repository.UpdateEmployee(item);
            _repository.Save();

            return Ok();
        }



    }
}
