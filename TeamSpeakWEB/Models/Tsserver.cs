using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Data
{
    public class Tsserver
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int port { get; set; }

        [Required]
        public string dns { get; set; }

        [Required]
        public int slots { get; set; }

        [Required]
        public DateTime time_payment { get; set; }

        [Required]
        public bool state { get; set; }

        [Required]
        public User user { get; set; }

        [Required]
        public int machine_id { get; set; }


    }
}
