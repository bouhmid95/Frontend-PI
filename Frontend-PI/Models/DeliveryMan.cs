using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Frontend_PI.Models
{
    public class DeliveryManStat
    {

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int total_orders { get; set; }
        public int orders_delivered { get; set; }

        public static explicit operator DeliveryManStat(Task<HttpResponseMessage> v)
        {
            throw new NotImplementedException();
        }
    }
}