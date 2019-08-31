using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;

        public HomeController(ApplicationDbContext db, UserManager<User> manager)
        {
            this.db = db;
            this.userManager = manager;
        }
        [Authorize]
        public IActionResult Index()
        {
                var ts = new Tsserver { dns = "biba", machine_id = 0, slots = 30,
                    state = true, time_payment = DateTime.Now,
                    user =  HttpContext.User.Identity.IsAuthenticated ? userManager.GetUserAsync(HttpContext.User).Result : db.Users.FirstOrDefault() };
            db.Tsservers.Add(ts);
            db.SaveChanges();

            var data = db.Tsservers.ToList().Take(1);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.User = HttpContext.User.Identity.Name;
            }
            else
            {
                ViewBag.User = "User is not authentification";
            }

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
