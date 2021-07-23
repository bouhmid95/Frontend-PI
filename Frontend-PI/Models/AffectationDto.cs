using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Frontend_PI.Models
{
    public class AffectationDto
    {

        public IEnumerable<Commande> orders { get; set; }
       
        [Display(Name="livreur")]
        [Required]
        public int idDeliveryMan { get; set; }

        public IEnumerable<SelectListItem> affected { get; set; }

        public static implicit operator AffectationDto(SelectListItem v)
        {
            throw new NotImplementedException();
        }
    }
}