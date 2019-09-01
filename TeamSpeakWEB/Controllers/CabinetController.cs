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
            var current_user = userManager.GetUserAsync(HttpContext.User).Result;
            var tsservers = db.Tsservers.Where(id => (id.user.Id == current_user.Id)).ToList();

            ViewBag.current_user = current_user;

            return View(tsservers);
        }

        public IActionResult New()
        {
            ViewBag.current_user = userManager.GetUserAsync(HttpContext.User).Result;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Tsserver tsserver)
        {

            return RedirectToAction("Index","Cabinet");
        }
    }
}
