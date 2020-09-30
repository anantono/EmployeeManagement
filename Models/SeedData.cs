using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManagement.Data;
using System;
using System.Linq;

namespace EmployeeManagement.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EmployeeContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<EmployeeContext>>()))
            {
                // Look for any employees.
                if (context.Employee.Any())
                {
                    return;   // DB has been seeded
                }

                //adding the 4 first employees to Database
                context.Employee.AddRange(
                    new Employee
                    {
                        Name = "Nick",
                        Surname = "Smith",
                        Country = "USA",
                        City = "Madison",
                        Address = "42th Street, 5",
                        HiringDate = DateTime.Parse("2007-2-12"),
                        Department = "Engineering",
                        Skillset = "Programming, Communication, Leading",
                        JobTitle = "Principal Engineer",
                        MonthlySalary = 5000
                    },

                    new Employee
                    {
                        Name = "George",
                        Surname = "Cane",
                        Country = "USA",
                        City = "NY",
                        Address = "4th Street, 18",
                        HiringDate = DateTime.Parse("2010-5-23"),
                        Department = "Engineering",
                        Skillset = "Programming, Communication",
                        JobTitle = "Senior Engineer",
                        MonthlySalary = 3000
                    },

                    new Employee
                    {
                        Name = "Maria",
                        Surname = "Panagopoulou",
                        Country = "Greece",
                        City = "Athens",
                        Address = "Filolaou 59",
                        HiringDate = DateTime.Parse("2017-6-18"),
                        Department = "HR",
                        Skillset = "Interviewing, Recruiting",
                        JobTitle = "Mid Specialist",
                        MonthlySalary = 2000
                    },

                    new Employee
                    {
                        Name = "Kostas",
                        Surname = "Alexiou",
                        Country = "Greece",
                        City = "Athens",
                        Address = "Elvetias 78",
                        HiringDate = DateTime.Parse("2019-1-5"),
                        Department = "Product",
                        Skillset = "Design, Marketing",
                        JobTitle = "Product Owner",
                        MonthlySalary = 1500
                    }
                );
                context.SaveChanges();
            }
        }
    }
}