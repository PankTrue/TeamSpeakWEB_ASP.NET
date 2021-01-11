using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TeamSpeakWEB.Models
{
    public class Tsserver
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ip { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string Dns { get; set; }

        [Required]
        public int Slots { get; set; }

        [Required]
        public DateTime TimePayment { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int MachineId { get; set; }
    }
}
