using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string IdentityType { get; set; }
        public bool IsTurkish { get; set; }
        public string IdentityNo { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string gender { get; set; }
        public DateTime birthdate { get; set; }

    }
}