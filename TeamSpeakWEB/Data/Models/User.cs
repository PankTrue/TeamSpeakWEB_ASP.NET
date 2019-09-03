using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace TeamSpeakWEB.Models
{
    public class User : IdentityUser
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal money { get; set; }


        public virtual User referalFrom { get; set; }

        public virtual List<User> refUsers { get; set; }

        public virtual List<Tsserver> tsservers { get; set; }
    }

}
