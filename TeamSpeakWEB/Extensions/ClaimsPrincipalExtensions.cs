using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Claims;

namespace WebCore5Elastic.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            var res = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return res;
        }
    }

}