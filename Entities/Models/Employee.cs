using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.Entities.Models
{
    public class Employee
    {
        [Key]
        [MaxLength(11, ErrorMessage = "Recorn no has to be 11 character.")]
        [MinLength(11, ErrorMessage = "Recorn no has to be 11 character.")]
        public string RecordNo { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string? UpperEmployee { get; set; }
        public string? LowerEmployee { get; set; }

    }
}
