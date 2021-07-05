using Frontend_PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Frontend_PI.Controllers
{
    public class UserController : Controller
    {

        // GET: User
        public ActionResult Index() 
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + " products ").Result;
            if (tokenResponse.IsSuccessStatusCode)
            {
                var products = tokenResponse.Content.ReadAsAsync<IEnumerable<User>>().Result;
                return View(products);
            }
            else
            {
                return View(new List<User>());
            }
        }
        HttpClient httpClient;
        string baseAddress;


        public UserController()
        {
            baseAddress = " https://8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(" application / json "));
            var _AccessToken = Session[" AccessToken "];
            httpClient.DefaultRequestHeaders.Add(" Authorization ", String.Format(" Bearer {0} ", _AccessToken));
        }




        [HttpPost]
        public ActionResult addUser(User user)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + " addUser /",
                user).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction(" Index ");
            }
            catch
            {
                return View();
            }
        }
    }
}