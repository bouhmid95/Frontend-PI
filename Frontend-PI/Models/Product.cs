using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class Product
    {
        public int id { get; set; }
        public String title { get; set; }
        public DateTime publicationDate { get; set; }
        public Category category { get; set; }
        public float price { get; set; }
        public String description { get; set; }
        public String image { get; set; }

    }
}