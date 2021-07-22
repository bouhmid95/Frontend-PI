using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class CommandeDetails
    {
        public int id { get; set; }
        public int idProduct { get; set; }
        public int idOrder { get; set; }
        public int qte { get; set; }
    }
}