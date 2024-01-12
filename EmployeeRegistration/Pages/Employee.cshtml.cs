using EmployeeRegistration.Context;
using EmployeeRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace EmployeeRegistration.Pages
{
    public class EmployeeModel : PageModel
    {
        private readonly EmployeeContext _context;

        public List<Employee> Employees { get; set; } = new List<Employee>();

        [BindProperty]
        public Employee? NewEmployee { get; set; }

        public EmployeeModel(EmployeeContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            //Employees = _context.Employee.ToList();


            Employees = _context.Employee.FromSqlRaw("SELECT * FROM Employees").ToList();
            



        }

        public IActionResult OnPost()
        {
            _context.Employee.Add(NewEmployee);
            
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
