using EmployeeRegistration.Context;
using EmployeeRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeRegistration.Pages
{
    public class EmployeeModel : PageModel
    {
        private readonly EmployeeContext _context;

        public List<Employee> Employees { get; set; } = new List<Employee>();

        [BindProperty]
        public Employee NewEmployee { get; set; }
        public static IConfigurationRoot Configuration { get; set; }

        public EmployeeModel(EmployeeContext context)
        {
            _context = context;
        }

        public void OnGet(string searchTerm)
        {
            //Employees = _context.Employee.ToList();

            Employees = GetEmployees(); // Your method to fetch employees

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Employees = Employees.Where(e => e.LastName.Contains(searchTerm)
                                              || e.Phone.Contains(searchTerm))
                                     .ToList();
            }
        }

        public void Index(string searchTerm)
        {
            var employees = GetEmployees(); // Your method to fetch employees

            if (!string.IsNullOrEmpty(searchTerm))
            {
                employees = employees.Where(e => e.LastName.Contains(searchTerm)
                                              || e.Phone.Contains(searchTerm))
                                     .ToList();
            }
         
        }

        private List<Employee> GetEmployees()
        {
            return Employees = _context.Employee.FromSqlRaw("SELECT * FROM dbo.Employee order by HireDate").ToList();
        }

        public IActionResult OnPost()
        {
           var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            string connectionString = Configuration.GetConnectionString("MyContextConnection");

            using (SqlConnection openCon = new SqlConnection(connectionString))
            {
                string saveEmployee = "INSERT INTO [dbo].[Employee] ([LastName],[FirstName],[Phone],[Zip],[HireDate]) VALUES (@lastName,@firstName,@phone,@zip,@hireDate)";

                using (SqlCommand querySaveEmployee = new SqlCommand(saveEmployee))
                {
                    querySaveEmployee.Connection = openCon;
                    querySaveEmployee.Parameters.Add("@lastName", SqlDbType.VarChar, 30).Value = NewEmployee.LastName;
                    querySaveEmployee.Parameters.Add("@firstName", SqlDbType.VarChar, 30).Value = NewEmployee.FirstName;
                    querySaveEmployee.Parameters.Add("@phone", SqlDbType.VarChar, 30).Value = NewEmployee.Phone;
                    querySaveEmployee.Parameters.Add("@zip", SqlDbType.VarChar, 30).Value = NewEmployee.Zip;
                    querySaveEmployee.Parameters.Add("@hireDate", SqlDbType.VarChar, 30).Value = NewEmployee.HireDate;

                    openCon.Open();

                    querySaveEmployee.ExecuteNonQuery();
                }
            }
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
