using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data
{
    public class HistoryLogContext : DbContext
    {
        public HistoryLogContext(DbContextOptions<HistoryLogContext> options)
           : base(options)
        {
        }

        public DbSet<HistoryLog> HistoryLog { get; set; }
    }
}
