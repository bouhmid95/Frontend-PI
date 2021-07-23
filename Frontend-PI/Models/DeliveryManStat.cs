using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class DeliveryMan
    {

  
        public int id { get; set; }
        [Display(Name = "Nom")]
        [Required]
        public string firstName { get; set; }
        [Display(Name = "Prenom")]
        [Required]
        public string lastName { get; set; }
        [Display(Name = "Adresse")]
        [Required]
        public string adresse { get; set; }
        [Display(Name = "Code Postal")]
        [Required]
        public int codePostal { get; set; }
    
}
}