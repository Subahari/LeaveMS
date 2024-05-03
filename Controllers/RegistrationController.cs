using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public class RegistrationController : Controller
{
    private string connectionString = "server=localhost;port=3306;database=leave;user=root;password=";

    [HttpPost]
    public IActionResult Register(EmployeeModel employee)
    {
        // Create a MySqlConnection object with the provided connection string
        using (var connection = new MySqlConnection(connectionString))
        {
            // Open the database connection
            connection.Open();

            // Define the SQL query to insert the registration details into the Employees table
            var query = @"INSERT INTO Employees (EmployeeName, NIC, DOB, Address, Designation, Email) 
                          VALUES (@EmployeeName, @NIC, @DOB, @Address, @Designation, @Email)";

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

                // Execute the SQL query to insert data into the database
                command.ExecuteNonQuery();
            }
        }

        // Redirect to a success page or return a success message
        return RedirectToAction("RegistrationSuccess");
    }
}
