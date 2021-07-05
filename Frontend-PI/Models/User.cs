using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class User
    {


        public int id { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }


        public string confirmCode { get; set; }
        public bool confirmed { get; set; }
        public int wrongPassword { get; set; }
        public bool blocked { get; set; }
        public DateTime blockedDate { get; set; }
        public bool banned { get; set; }
        private string userRole { get; set; }

    }
}