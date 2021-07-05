using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class Commande
    {

        public int id { get; set; }
        public String reference { get; set; }
        public DateTime orderDate { get; set; }
        public String status { get; set; }
        public String typePaiement { get; set; }
        public String adresse { get; set; }
        public String codePostal { get; set; }

        public Commande() { }

        public Commande(string reference, DateTime commandeDate, string status, string typePaiement, string adresse, string codePostal)
        {
            this.reference = reference;
            this.orderDate = commandeDate;
            this.status = status;
            this.typePaiement = typePaiement;
            this.adresse = adresse;
            this.codePostal = codePostal;
        }
    }
}