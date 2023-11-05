using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.Dto
{
    public class EmployeeDto
    {
        public string RecordNo { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string? UpperEmployee { get; set; }
        public string? LowerEmployee { get; set; }
    }
}
