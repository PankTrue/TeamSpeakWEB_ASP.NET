using Core.Flash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Data.ViewModels;
using TeamSpeakWEB.Filters;
using TeamSpeakWEB.Models;
using TeamSpeakWEB.Services;

namespace TeamSpeakWEB.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IFlasher _flasher;
        private readonly TeamSpeakQueryClient _teamspeakQueryClient;
        private readonly ILogger<CabinetController> _logger;
        private readonly IMapper _mapper;

        public CabinetController(ApplicationDbContext db, UserManager<User> userManager, IFlasher flasher,
                                TeamSpeakQueryClient teamspeakQueryClient, ILogger<CabinetController> logger,
                                IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _flasher = flasher;
            _teamspeakQueryClient = teamspeakQueryClient;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            var tsServer = _db.Tsservers.Where(id => (id.User.Id == currentUser.Id))
                                                            .ProjectTo<TsserverView>(_mapper.ConfigurationProvider).ToList();


            foreach (var ts in tsServer)
            {
                _teamspeakQueryClient.UseServer(ts.MachineId).Wait(1000);
                var s = _teamspeakQueryClient.WhoAmI().Result;
                ts.State = true;
                //TODO доделать
            }

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
            tsserver.MachineId = 0;
            tsserver.Ip = "127.0.0.1";
            tsserver.Port = (new Random(DateTime.Now.Millisecond)).Next(65565);
            tsserver.User = GetCurrentUser();

            _db.Tsservers.Add(tsserver);

            try
            {
                //var res = _teamspeakQueryClient.Client.Send($"servercreate virtualserver_name=TeamSpeak\\s]\\p[\\sServer " +
                //                                                                        $"virtualserver_port = {new Random().Next(2000, 65565)} " +
                //                                                                        $"virtualserver_maxclients = {tsserver.Slots}").Result;

                var res = _teamspeakQueryClient.Client.Send("servercreate virtualserver_name=TeamSpeak_Server virtualserver_port=2000 virtualserver_maxclients=32").Result;

                _db.SaveChanges();
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
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
