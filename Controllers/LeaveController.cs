using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public class LeaveController : Controller
{

    public IActionResult Applyleave()
    {
        return View();
    }

}