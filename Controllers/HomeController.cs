using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LeaveMS.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;


namespace LeaveMS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult Register()
    {
        return View();
    }

    private string GetMD5Hash(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object
        using (MD5 md5 = MD5.Create())
        {
            // Compute the hash value from the input string
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    private string connectionString = "server=localhost;port=3306;database=leave;user=root;password=";

    [HttpPost]
    public IActionResult Register(EmployeeModel employee)
    {
        // Check if NIC already exists in the database
        if (IsNICAlreadyExists(employee.NIC))
        {
            // Return error message indicating NIC is already in use
            return Json(new { success = false, message = "NIC is already used." });
        }

        string encryptedPassword = GetMD5Hash(employee.Password);

        // Create a MySqlConnection object with the provided connection string
        using (var connection = new MySqlConnection(connectionString))
        {
            // Open the database connection
            connection.Open();

            // Define the SQL query to insert the registration details into the Employees table
            var query = @"INSERT INTO Employees (EmployeeName, NIC, DOB, Address, Designation, Email, Password) 
                      VALUES (@EmployeeName, @NIC, @DOB, @Address, @Designation, @Email, @Password)";


            // Create a MySqlCommand object with the SQL query and MySqlConnection
            using (var command = new MySqlCommand(query, connection))
            {
                // Set parameter values for the SQL query
                command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                command.Parameters.AddWithValue("@NIC", employee.NIC);
                command.Parameters.AddWithValue("@DOB", employee.DOB);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@Designation", employee.Designation);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Password", encryptedPassword);

                // Execute the SQL query to insert data into the database
                command.ExecuteNonQuery();
            }
        }

        // Redirect to a success page or return a success message
        return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
        return View();
    }

    private bool IsNICAlreadyExists(string nic)
    {
        // Create a MySqlConnection object with the provided connection string
        using (var connection = new MySqlConnection(connectionString))
        {
            // Open the database connection
            connection.Open();

            // Define the SQL query to check if the NIC exists in the Employees table
            var query = "SELECT COUNT(*) FROM Employees WHERE NIC = @NIC";

            // Create a MySqlCommand object with the SQL query and MySqlConnection
            using (var command = new MySqlCommand(query, connection))
            {
                // Set parameter values for the SQL query
                command.Parameters.AddWithValue("@NIC", nic);

                // Execute the SQL query to count the number of records with the given NIC
                int count = Convert.ToInt32(command.ExecuteScalar());

                // Return true if count is greater than 0, indicating NIC exists; otherwise, return false
                return count > 0;
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
