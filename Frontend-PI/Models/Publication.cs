using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class Publication
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime publicationDate { get; set; }
        public int nbLike { get; set; }
        public int nbDisLike { get; set; }
        public bool validated { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public int idUser { get; set; }

    }
}