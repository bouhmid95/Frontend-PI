using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Frontend_PI.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Frontend_PI.Controllers
{
    public class PublicationController : Controller


    {

        HttpClient httpClient;
        string baseAddress;
        public PublicationController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                                                 //var _AccessToken = Session[" AccessToken "];
                                                                                                                 // httpClient.DefaultRequestHeaders.Add(" Authorization ", String.Format(" Bearer {0} ", _AccessToken));
        }
        // GET: Publication
        public ActionResult Index()
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findValidatedPublications");

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<Publication>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }

        // GET: Publication/Details/5
        public ActionResult Details(int id)
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findPublication/" + id);

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<Publication>().Result;
                return View(ViewBag.result);
            }
            return View();


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

        // GET: Publication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publication/Create
        [HttpPost]
        public ActionResult Create(Publication publication)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<Publication>(baseAddress + "addPublication/",
                publication).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publication/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Publication/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publication/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Publication/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var APIResponse = httpClient.DeleteAsync(baseAddress + "deletePublication/" + id
                ).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
