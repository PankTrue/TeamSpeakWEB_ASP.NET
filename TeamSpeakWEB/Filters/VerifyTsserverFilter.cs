using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Filters
{
    public class VerifyTsserverFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (Tsserver arg in context.ActionArguments.Values.Where(v => v is Tsserver))
            {
                //TODO
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
