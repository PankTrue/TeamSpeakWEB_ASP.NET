using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;

        public CabinetController(ApplicationDbContext db, UserManager<User> manager)
        {
            this.db = db;
            this.userManager = manager;
        }

        public IActionResult Index()
        {
            var current_user = GetCurrentUser();
            var tsservers = db.Tsservers.Where(id => (id.user.Id == current_user.Id)).ToList();

            ViewBag.current_user = current_user;

            return View(tsservers);
        }

        public IActionResult New()
        {
            ViewBag.current_user = GetCurrentUser();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Tsserver tsserver)
        {
            tsserver.time_payment = (DateTime.Now.AddMonths(tsserver.time_payment.Day));
            tsserver.state = true;
            tsserver.machine_id = 0;
            tsserver.port = (new Random(DateTime.Now.Millisecond)).Next(65565);
            tsserver.user = GetCurrentUser();

            db.Tsservers.Add(tsserver);
            db.SaveChanges();

            return RedirectToAction("Index","Cabinet");
        }

        [HttpPost]
        public IActionResult free_dns(string dns)
        {
            if (db.Tsservers.Where(id => id.dns == dns).Take(1).FirstOrDefault() == null)
                return Content("true");
            else
                return Content("false");
        }



        private User GetCurrentUser()
        {
            return userManager.GetUserAsync(HttpContext.User).Result;
        }
    }
}
