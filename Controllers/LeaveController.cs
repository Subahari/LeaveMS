using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public class LeaveController : Controller
{

    public IActionResult Applyleave()
    {
        string connectionString = "server=localhost;port=3306;database=leave;user=root;password=";
        var leaveTypes = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM leavetype";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leaveTypes.Add(reader["type"].ToString());
                    }
                }
            }
        }

        ViewBag.LeaveTypes = leaveTypes;

        return View();
    }

    private string connectionString = "server=localhost;port=3306;database=leave;user=root;password=";

    [HttpPost]
    public IActionResult Applyleave(ApplyLeaveModel applyleave)
    {
        // Create a MySqlConnection object with the provided connection string
        using (var connection = new MySqlConnection(connectionString))
        {
            // Open the database connection
            connection.Open();

            // Define the SQL query to insert the registration details into the Employees table
            var query = @"INSERT INTO applyleave (EmployeeId, LeaveId, purpose, startDate,enddate,days,leave_status,leave_approver_id,approve_or_reject_date) 
                          VALUES (@EmployeeId, @LeaveId, @Purpose, @StartDate, @EndDate, @Days, @LeaveStatus, @LeaveApproverId, @ApproveRejectDate)";
            var LeaveStatus = "Pending";
            TimeSpan duration = applyleave.EndDate - applyleave.StartDate;
            int days = duration.Days;
            

            // Create a MySqlCommand object with the SQL query and MySqlConnection
            using (var command = new MySqlCommand(query, connection))
            {
                // Set parameter values for the SQL query
                command.Parameters.AddWithValue("@EmployeeId", applyleave.EmployeeId);
                command.Parameters.AddWithValue("@LeaveId", applyleave.LeaveId);
                command.Parameters.AddWithValue("@Purpose", applyleave.Purpose);
                command.Parameters.AddWithValue("@StartDate", applyleave.StartDate);
                command.Parameters.AddWithValue("@EndDate", applyleave.EndDate);
                command.Parameters.AddWithValue("@Days", days);
                command.Parameters.AddWithValue("@LeaveStatus", LeaveStatus);
                command.Parameters.AddWithValue("@LeaveApproverId", applyleave.LeaveApproverId);
                //command.Parameters.AddWithValue("@ApplyDate", applyleave.ApplyDate);
                command.Parameters.AddWithValue("@ApproveRejectDate", applyleave.ApproveRejectDate);

                // Execute the SQL query to insert data into the database
                command.ExecuteNonQuery();
            }
        }

        // Redirect to a success page or return a success message
        return RedirectToAction("LeaveSuccess");
    }

}