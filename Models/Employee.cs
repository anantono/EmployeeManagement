using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [StringLength(30, MinimumLength = 2)]
        [Required]
        public string Surname { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string Country { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string City { get; set; }

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Hiring Date")]
        [DataType(DataType.Date)]
        public DateTime HiringDate { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string Department { get; set; }
        [Display(Name = "Skill Set")]


        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Skillset { get; set; }
        [Display(Name = "Job Tile")]

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string JobTitle { get; set; }
        [Display(Name = "Monthly Salary")]

        [Required]
        public int MonthlySalary { get; set; }

    }
}
