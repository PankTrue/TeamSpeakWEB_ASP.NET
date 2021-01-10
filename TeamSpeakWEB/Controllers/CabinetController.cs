using Core.Flash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Filters;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IFlasher _flasher;

        public CabinetController(ApplicationDbContext db, UserManager<User> userManager, IFlasher flasher)
        {
            this._db = db;
            this._userManager = userManager;
            this._flasher = flasher;
        }

        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            var tsServer = _db.Tsservers.Where(id => (id.User.Id == currentUser.Id)).ToList();

            ViewBag.current_user = currentUser;

            return View(tsServer);
        }

        public IActionResult New()
        {
            var currentUser = GetCurrentUser();

            ViewBag.currentUser = currentUser;

            return View();
        }

        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Edit(int id)
        {
            var currentUser = GetCurrentUser();
            var tsServer = _db.Tsservers.Find(id);

            ViewBag.currentUser = currentUser;


            return View(tsServer);
        }

        [HttpPost]
        public IActionResult Create(Tsserver tsserver)
        {
            tsserver.TimePayment = (DateTime.Now.AddMonths(tsserver.TimePayment.Day));
            tsserver.State = true;
            tsserver.MachineId = 0;
            tsserver.Port = (new Random(DateTime.Now.Millisecond)).Next(65565);
            tsserver.User = GetCurrentUser();

            _db.Tsservers.Add(tsserver);

            try
            {
                _db.SaveChanges();
            }
            catch
            {
                _flasher.Flash("danger", "Не удалось создать сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            _flasher.Flash("success", "Сервер успешно создан");

            return RedirectToAction("Index", "Cabinet");
        }

        [HttpPost]
        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Update(Tsserver tsserver)
        {
            var ts = _db.Tsservers.Find(tsserver.Id);


            ts.Dns = tsserver.Dns;
            ts.Slots = tsserver.Slots;

            _db.Tsservers.Update(ts);

            try{
                _db.SaveChanges();
            }
            catch {
                _flasher.Flash("danger", "Не удалось редактировать сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            _flasher.Flash("success", "Сервер успешно редактирован");
            return RedirectToAction("Index","Cabinet");
        }

        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Destroy(int id)
        {

            _db.Tsservers.Remove(_db.Tsservers.Find(id));


            try {
                _db.SaveChanges();
            }
            catch {
                _flasher.Flash("danger", "Не удалось удалить сервер");
                return RedirectToAction("Index", "Cabinet");
            }

            _flasher.Flash("success", "Сервер успешно удален");

            return RedirectToAction("Index", "Cabinet");
        }

        public IActionResult free_dns(string dns)
        {
            if (!_db.Tsservers.Where(id => id.Dns == dns).Any())
                return Content("true");
            else
                return Content("false");
        }

        [ServiceFilter(typeof(TsserverBelongsToCurrentUserFilter))]
        public IActionResult Panel(int id)
        {
            var ts = _db.Tsservers.Find(id);

            return View(ts);
        }


        private User GetCurrentUser()
        {
            return _userManager.GetUserAsync(HttpContext.User).Result;
        }
    }
}
