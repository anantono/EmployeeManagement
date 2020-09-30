using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }

        //Employee Context has now access to Skill Table and HistoryLog Table
        public DbSet<Skill> Skill { get; set; }

        public DbSet<HistoryLog> HistoryLog { get; set; }

    }
}