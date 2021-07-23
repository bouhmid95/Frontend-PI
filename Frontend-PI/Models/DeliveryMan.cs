using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Frontend_PI.Models
{
    public class DeliveryManStat
    {

        public int id { get; set; }
        [Display(Name = "Nom")]
        public string first_name { get; set; }
        [Display(Name = "Prenom")]
    
        public string last_name { get; set; }
        [Display(Name = "Commandes Affectées")]
       
        public int total_orders { get; set; }
        [Display(Name = "Commandes Livrées")]
     
        public int orders_delivered { get; set; }
        [Display(Name = "Taux de Livraison")]
        public float taux_livraison { get; set; }
        [Display(Name = "% Affectation")]
        public float taux_affectation { get; set; }

        public static explicit operator DeliveryManStat(Task<HttpResponseMessage> v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator List<object>(DeliveryManStat v)
        {
            throw new NotImplementedException();
        }
    }
}