using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend_PI.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime publicationDate { get; set; }
        public Publication publication { get; set; }
        public User user { get; set; }
    }
}