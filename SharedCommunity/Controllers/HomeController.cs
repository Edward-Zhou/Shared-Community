using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedCommunity.Authorization;
using SharedCommunity.Models;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SharedCommunity.Helpers;

namespace SharedCommunity.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public HomeController(SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeCore(Groups: "Test",Roles:"Admin", Claims: "issuer")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        //[Authorize(Roles = "XX", Policy = "XX", Permission="Read")]
        //[CustomAuthorize(Groups = "Test")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            var user = User.Identity.Name;
            var info = _signInManager.Context.User;
            return View();
        }
        public IActionResult SendMail()
        {
            _emailSender.SmtpEmailAsync("Test","Hello World","v-tazho@hotmail.com");
            return Ok("Successfully");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
