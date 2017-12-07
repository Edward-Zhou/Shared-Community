using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedCommunity.Authorization;
using SharedCommunity.Models;

namespace SharedCommunity.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeCore("Role", "Admin,Viewer")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        //[Authorize(Roles = "XX", Policy = "XX", Permission="Read")]
        [CustomAuthorize(Groups = "Test")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            var user = User.Identity.Name;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
