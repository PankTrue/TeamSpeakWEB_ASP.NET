using Core.Flash;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Filters
{
    public class TsserverBelongsToCurrentUserFilter : Attribute, IActionFilter
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IFlasher flasher;


        public TsserverBelongsToCurrentUserFilter(ApplicationDbContext db, UserManager<User> userManager, IFlasher flasher)
        {
            this.db = db;
            this.userManager = userManager;
            this.flasher = flasher;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            object server_id;

            if (context.ActionArguments.ContainsKey("id"))
                server_id = context.ActionArguments["id"];
            else
                server_id = (context.ActionArguments.First().Value as Tsserver)?.Id;


            var tsserver = db.Tsservers.Find(server_id);
            var current_user = userManager.GetUserAsync(context.HttpContext.User).Result;

            if (tsserver == null || tsserver.User.Id != current_user.Id)
            {
                flasher.Flash(Types.Danger, "Этот сервер не существует или не пренадлежит вам",true);
                context.Result = new RedirectToActionResult("Index", "Cabinet", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){ }
    }
}
