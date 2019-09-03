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
            var tsservers = db.Tsservers.Where(id => (id.User.Id == current_user.Id)).ToList();

            ViewBag.current_user = current_user;

            return View(tsservers);
        }

        public IActionResult New()
        {
            var current_user = GetCurrentUser();

            ViewBag.current_user = current_user;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Tsserver tsserver)
        {
            tsserver.TimePayment = (DateTime.Now.AddMonths(tsserver.TimePayment.Day));
            tsserver.State = true;
            tsserver.MachineId = 0;
            tsserver.Port = (new Random(DateTime.Now.Millisecond)).Next(65565);
            tsserver.User = GetCurrentUser();

            db.Tsservers.Add(tsserver);
            db.SaveChanges();

            return RedirectToAction("Index", "Cabinet");
        }

        [HttpPost]
        public IActionResult free_dns(string dns)
        {
            if (db.Tsservers.Where(id => id.Dns == dns).Count() == 0)
                return Content("true");
            else
                return Content("false");
        }

        [Route("/Cabinet/Panel/{id:int}")]
        public IActionResult Panel(int id)
        {
            ViewBag.id = id;
            if (db.Tsservers.Find(id).User.Id != GetCurrentUser().Id)
            {
                return RedirectToAction("Index", "Cabinet");
            }
            return View();
        }



        private User GetCurrentUser()
        {
            return userManager.GetUserAsync(HttpContext.User).Result;
        }
    }
}
