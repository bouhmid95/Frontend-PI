using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class Commande
    {

        public int id { get; set; }
        [Display(Name = "Référence")]
        public String reference { get; set; }
        [Display(Name = "Date de la Commande")]
        public DateTime orderDate { get; set; }
     
        public String status { get; set; }
        [Display(Name = "Type de Paiement")]
        public String typePaiement { get; set; }
        public String adresse { get; set; }
        [Display(Name = "Code Postal")]
        public String codePostal { get; set; }
        public int idUser { get; set; }
        [ForeignKey("user_id")]
        public User user { get; set; }

        public Boolean isChecked {get; set;}

        public Commande() { }

        public Commande(string reference, DateTime commandeDate, string status, string typePaiement, string adresse, string codePostal,Boolean isChecked)
        {
            this.reference = reference;
            this.orderDate = commandeDate;
            this.status = status;
            this.typePaiement = typePaiement;
            this.adresse = adresse;
            this.codePostal = codePostal;
            this.isChecked = isChecked;
        }
    }
}