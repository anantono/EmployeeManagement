using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Text.Encodings.Web;

namespace EmployeeManagement.Controllers
{
    public class MainController : Controller
    {
        // 
        // GET: /MainController/

        //URL -> Main/...


        //Index method change to return View
        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /MainController/Welcome/ 

        //Welcome method change to request
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}