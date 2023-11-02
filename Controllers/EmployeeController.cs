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

        public EmployeeController(IEmployeeRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult <IEnumerable<Employee>> GetAllEmployee()
        {
            var items  =  _repository.GetAllEmployee();

            return Ok(items);
        }

        [HttpGet("{RecordNo}")]
        public ActionResult <Employee> GetOneEmployee(string RecordNo)
        {
            var item = _repository.GetOneEmployeeByRecordNo(RecordNo);
            
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee(Employee employee)
        {
            _repository.AddEmployee(employee);

            return Ok();
        }

    }
}
