using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSpeakWEB.Models
{
    public class User : IdentityUser
    {
        public decimal money { get; set; }
        public int referal { get;set; }
    }

}
