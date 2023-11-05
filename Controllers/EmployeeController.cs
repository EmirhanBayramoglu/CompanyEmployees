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
        public async Task<ActionResult> GetAllEmployee()
        {
            var items = await _repository.GetAllEmployee();

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(items));
        }

        [HttpGet("{RecordNo}")]
        public async Task<ActionResult<EmployeeDto>> GetOneEmployee(string RecordNo)
        {
            var item = await _repository.GetOneEmployeeByRecordNo(RecordNo);

            return Ok(_mapper.Map<EmployeeDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee([FromBody]Employee employee)
        {
            
            await _repository.AddEmployee(employee);
            await _repository.Save();

            return Ok();
        }

        [HttpPut("{RecordNo}")]
        public async Task<ActionResult> UpdateEmployee(string recordNo,EmployeeUpdateDto employeeUpdateDto)
        {
            var item = await _repository.GetOneEmployeeByRecordNo(recordNo);

            var oldList = await _repository.LowwerEmployeeListCreator(item.LowerEmployee);

            string oldUpper = item.UpperEmployee;

            _mapper.Map(employeeUpdateDto, item);

            var newList = await _repository.LowwerEmployeeListCreator(item.LowerEmployee);

            await _repository.UpdateConfigration(oldList, newList, oldUpper, item);

            await _repository.UpdateEmployee(item);

            return Ok();
        }



    }
}
