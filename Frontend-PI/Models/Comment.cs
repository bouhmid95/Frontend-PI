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
        public int idUser { get; set; }
    }


    /*  HttpClient httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri("http://localhost:8081");
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findUser/" + id).Result;
      if (responseMessage.IsSuccessStatusCode)
      {
          ViewBag.result = responseMessage.Content.ReadAsAsync<User>().Result;
      }
      return View();*/
}