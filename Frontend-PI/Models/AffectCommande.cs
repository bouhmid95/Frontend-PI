using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Frontend_PI.Models
{
    public class AffectCommande
    {
        public long idDeliveryMan { get; set; }
        public IEnumerable<SelectListItem> ocommandes { get; set; }

    }
}