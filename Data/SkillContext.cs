using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
    public class SkillContext : DbContext
    {
        public SkillContext(DbContextOptions<SkillContext> options)
            : base(options)
        {
        }

        public DbSet<Skill> Skill { get; set; }

        //Skill Context has now access to HistoryLog Table
        public DbSet<HistoryLog> HistoryLog { get; set; }
    }
}
