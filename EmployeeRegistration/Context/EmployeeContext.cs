using EmployeeRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistration.Context
{
    public class EmployeeContext: DbContext
    {
        public DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }
    }
}
