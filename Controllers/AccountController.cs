using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LeaveMS.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
namespace LeaveMS.Controllers;

public class AccountController : Controller
{
    private readonly string connectionString = "server=localhost;port=3306;database=leave;user=root;password=";

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Check if username and password are correct
        bool isValidLogin = IsValidLogin(username, password);

        if (isValidLogin)
        {
            // Login successful
            return Json(new { success = true, message = "Login successful." });
            // return RedirectToAction("Dashboard");
            // return RedirectToAction("Index", "Home");
        }
        else
        {
            //     // Login failed
            //     return Json(new { success = false, message = "Invalid username or password." });
            // }
            ViewData["Error"] = "Invalid username or password";
            return View("Login");
        }
    }

    public IActionResult Dashboard()
    {
        return View("Dashboard");
    }

    private bool IsValidLogin(string username, string password)
    {
        // Implement your logic to validate username and password against database
        // You should query your database to check if the username and password match

        // For demonstration purposes, let's assume you have a Users table with columns Username and Password

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT COUNT(*) FROM employees WHERE EmployeeName = @Username AND Password = @Password";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", GetMD5Hash(password));
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

    private string GetMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
