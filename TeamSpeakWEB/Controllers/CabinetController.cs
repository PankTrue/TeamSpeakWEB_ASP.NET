using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Flash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Filters;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IFlasher flasher;

        public CabinetController(ApplicationDbContext db, UserManager<User> userManager, IFlasher flasher)
        {
            this.db = db;
            this.userManager = userManager;
            this.flasher = flasher;
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

        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Edit(int id)
        {
            var tsserver = db.Tsservers.Find(id);

            return View(tsserver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tsserver tsserver)
        {
            tsserver.TimePayment = (DateTime.Now.AddMonths(tsserver.TimePayment.Day));
            tsserver.State = true;
            tsserver.MachineId = 0;
            tsserver.Port = (new Random(DateTime.Now.Millisecond)).Next(65565);
            tsserver.User = GetCurrentUser();

            db.Tsservers.Add(tsserver);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                flasher.Flash("danger", "Не удалось создать сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            flasher.Flash("success", "Сервер успешно создан");

            return RedirectToAction("Index", "Cabinet");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Update(Tsserver tsserver)
        {
            var current_user = GetCurrentUser();
            var ts = db.Tsservers.Find(tsserver.Id);


            ts.Dns = tsserver.Dns;
            ts.Slots = tsserver.Slots;

            db.Tsservers.Update(ts);

            try{
                db.SaveChanges();
            }
            catch {
                flasher.Flash("danger", "Не удалось редактировать сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            flasher.Flash("success", "Сервер успешно редактирован");
            return RedirectToAction("Index","Cabinet");
        }

        [HttpDelete]
        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Destroy(int id)
        {
            db.Tsservers.Remove(new Tsserver { Id = id});

            try {
                db.SaveChanges();
            }
            catch {
                flasher.Flash("danger", "Не удалось удалить сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            flasher.Flash("success", "Сервер успешно удален");

            return RedirectToAction("Index", "Cabinet");
        }

        [HttpPost]
        public IActionResult free_dns(string dns)
        {
            if (!db.Tsservers.Where(id => id.Dns == dns).Any())
                return Content("true");
            else
                return Content("false");
        }

        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Panel(int id)
        {
            ViewBag.id = id;

            return View();
        }


        private User GetCurrentUser()
        {
            return userManager.GetUserAsync(HttpContext.User).Result;
        }
    }
}
